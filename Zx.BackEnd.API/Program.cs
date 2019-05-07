using System;
using System.IO;
using System.Reflection;
using GeoJSON.Net.Geometry;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Zx.BackEnd.Core;
using Zx.BackEnd.Model;


namespace Zx.BackEnd.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();


            try
            {

                using (PDVBusiness bo = new PDVBusiness())
                {
                    var rawObj = JObject.Parse(File.ReadAllText(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "pdvs.json")));
                    var pdvs = (JArray) rawObj["pdvs"];
                    foreach (var obj in pdvs)
                    {
                        var pdv = new PontoDeVenda()
                        {
                            Id = 0,
                            Document = obj["document"].ToString(),
                            OwnerName = obj["ownerName"].ToString(),
                            TradingName = obj["tradingName"].ToString(),
                            Address = JsonConvert.DeserializeObject<Point>(obj["address"].ToString()),
                            CoverageArea =
                                JsonConvert.DeserializeObject<MultiPolygon>(obj["coverageArea"].ToString())
                        };

                        bo.Add(pdv);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }


            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
