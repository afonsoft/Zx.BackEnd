using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using GeoJSON.Net.Geometry;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Zx.BackEnd.Data;
using Zx.BackEnd.Data.Context;
using Zx.BackEnd.Data.Entity;
using Zx.BackEnd.Data.Helper;


namespace Zx.BackEnd.Core
{
    public class PDVBusiness : IDisposable
    {
        private readonly PDVRepository repository;

        public PDVBusiness()
        {
            repository = new PDVRepository(new PontoDeVendaContext(EnumProvider.InMemory));
            //repository = new PDVRepository(new PontoDeVendaContext(EnumProvider.SQLServer, @"Server=(localdb)\\MSSQLLocalDB;Database=Master;Trusted_Connection=True;MultipleActiveResultSets=true"));
            //repository = new PDVRepository(new PontoDeVendaContext(EnumProvider.MySQL, @"Server=localhost;Database=Teste;Uid=root;Pwd=root;"));
        }

        public IEnumerable<Model.PontoDeVenda> Get()
        {
            return repository.Get().Select(x => x.ToModel());
        }

        public Model.PontoDeVenda GetById(int id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id));
            var item =  repository.GetById(id);

            return item?.ToModel();
        }

        public Model.PontoDeVenda Add(Model.PontoDeVenda pdv)
        {
            PontoDeVenda pdvDb = pdv.ToData();
            var cnpj = repository.GetByCNPJ(pdv.Document);
            if (cnpj == null)
            {
                repository.Add(pdvDb);
                return pdvDb.ToModel();
            }
            else
            {
                return null;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                repository.DeleteById(id);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public IEnumerable<Model.PontoDeVenda> Seach(double Latitude, double Longitude)
        {
            var address = VerifyAddress(Latitude, Longitude);
            return FindClosestPDV(address);
        }

        #region Metodos 
        public bool VerifyPDV(JObject obj, out Data.Entity.PontoDeVenda pdv)
        {
            var coverageArea = JsonConvert.DeserializeObject<MultiPolygon>(obj["coverageArea"]?.ToString());
            var address = JsonConvert.DeserializeObject<Point>(obj["address"]?.ToString());
            if (string.IsNullOrEmpty(obj["tradingName"].ToString()) ||
                string.IsNullOrEmpty(obj["ownerName"].ToString()) ||
                string.IsNullOrEmpty(obj["document"].ToString()) ||
                address == null || coverageArea == null)
            {
                pdv = null;
                return false;
            }
            else
            {
                pdv = new Data.Entity.PontoDeVenda()
                {
                    TradingName = obj["tradingName"].ToString(),
                    OwnerName = obj["ownerName"].ToString(),
                    Document = obj["document"].ToString(),
                    Address = new Data.Entity.Address(obj["address"].ToString()),
                    CoverageArea = new Data.Entity.CoverageArea(obj["coverageArea"].ToString()),
                };
            }
            return true;
        }

        public Address VerifyAddress(double Latitude, double Longitude)
        {
            Point point = new Point(new Position(Latitude, Longitude));
            return new Address(point);
        }

        public Address VerifyAddress(JObject obj)
        {
            if (obj["address"] == null) return null;
            var point = JsonConvert.DeserializeObject<Point>(obj["address"].ToString());
            if (point != null)
            {
                return new Address(obj["address"].ToString());
            }
            return null;
        }

        private IEnumerable<Model.PontoDeVenda> FindClosestPDV(Address address)
        {
            var todosPdvs = repository.Get().ToList();
            var lessDistant = double.MaxValue;
            PontoDeVenda nearestPdv = null;
            IList<Model.PontoDeVenda> nearestPdvAPI = new List<Model.PontoDeVenda>();
            foreach (var pdv in todosPdvs)
            {
                if (GeoJsonHelper.IsPointInMultiPolygon(address.ReturnLocation(), pdv.CoverageArea.ReturnCoverageAreaMultiPolygon()))
                {
                    double pdvDistance = pdv.ReturnDistance(address.Latitude, address.Longitude);
                    if (pdvDistance < lessDistant)
                    {
                        lessDistant = pdvDistance;
                        nearestPdv = pdv;
                    }
                }

                if (nearestPdv != null)
                {
                    var item = nearestPdv.ToModel();
                    item.Distance = lessDistant;
                    nearestPdvAPI.Add(item);
                    nearestPdv = null;
                }
            }
            
            return nearestPdvAPI.OrderBy(x=>x.Distance).Take(10);
        }
        #endregion

        public void Dispose()
        {
            repository.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
