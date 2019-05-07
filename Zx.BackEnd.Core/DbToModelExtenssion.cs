using System;
using System.Collections.Generic;
using System.Text;
using GeoJSON.Net.Geometry;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Zx.BackEnd.Core
{
    public static class DbToModelExtenssion
    {
        public static Model.PontoDeVenda ToModel(this Data.Entity.PontoDeVenda pdv)
        {
            return new Model.PontoDeVenda()
            {
                TradingName = pdv.TradingName,
                Document = pdv.Document,
                Id = pdv.Id,
                OwnerName = pdv.OwnerName,
                Address = JsonConvert.DeserializeObject<Point>(pdv.Address.Coordinates),
                CoverageArea = JsonConvert.DeserializeObject<MultiPolygon>(pdv.CoverageArea.Coordinates)
            };
        }

        public static Data.Entity.PontoDeVenda ToData(this Model.PontoDeVenda pdv)
        {
            return new Data.Entity.PontoDeVenda()
            {
                TradingName = pdv.TradingName,
                Document = pdv.Document,
                Id = pdv.Id,
                OwnerName = pdv.OwnerName,
                Address = new Data.Entity.Address(pdv.Address),
                CoverageArea = new Data.Entity.CoverageArea(pdv.CoverageArea)
            };
        }
    }
}
