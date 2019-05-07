using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GeoJSON.Net;
using GeoJSON.Net.Geometry;
using Newtonsoft.Json;

namespace Zx.BackEnd.Data.Entity
{
    public class CoverageArea
    {
        private MultiPolygon multiPolygon;

        [Required]
        [Column("CoverageAreaType")]
        public GeoJSONObjectType Type { get; set; }
        [Required]
        [StringLength(8000)]
        [Column("CoverageArea")]
        public string Coordinates { get; set; }

        public CoverageArea()
        {
        }

        public CoverageArea(string area)
        {
            Coordinates = area;
            Type = GeoJSON.Net.GeoJSONObjectType.MultiPolygon;
            multiPolygon = JsonConvert.DeserializeObject<MultiPolygon>(this.Coordinates);
        }
        public CoverageArea(MultiPolygon area)
        {
            multiPolygon = area;
            Type = GeoJSON.Net.GeoJSONObjectType.MultiPolygon;
            this.Coordinates = JsonConvert.SerializeObject(multiPolygon);
        }

        public MultiPolygon ReturnCoverageAreaMultiPolygon()
        {
            multiPolygon = JsonConvert.DeserializeObject<MultiPolygon>(this.Coordinates);
            return multiPolygon;
        }
    }
}
