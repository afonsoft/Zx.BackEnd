using System.Collections.Generic;
using GeoJSON.Net.Geometry;
using Newtonsoft.Json.Linq;

namespace Zx.BackEnd.Model
{
    public class PontoDeVendas
    {
       public IList<PontoDeVenda> pdvs { get; set; }
    }
    public class PontoDeVenda
    {
        public int Id { get; set; }
        public string TradingName { get; set; }
        public string OwnerName { get; set; }
        public string Document { get; set; }
        public Point Address { get; set; }
        public MultiPolygon CoverageArea { get; set; }
        public double Distance { get; set; }
    }
}
