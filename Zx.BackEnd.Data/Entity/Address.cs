using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GeoJSON.Net;
using GeoJSON.Net.Geometry;
using Newtonsoft.Json;

namespace Zx.BackEnd.Data.Entity
{
    public class Address
    {
        private Point point;

        public double Latitude => point.Coordinates.Latitude;

        public double Longitude => point.Coordinates.Longitude;

        [Required]
        [StringLength(8000)]
        [Column("AddressCoordinates")]
        public string Coordinates { get; set; }

        [Required]
        [Column("AddressType")]
        public GeoJSONObjectType Type { get; set; }

        public Address()
        {
        }

        public Address(string coordinates)
        {
            this.point = JsonConvert.DeserializeObject<Point>(coordinates);
            this.Coordinates = coordinates;
            this.Type = this.point.Type;
        }

        public Address(Point point)
        {
            this.point = point; 
            this.Coordinates = JsonConvert.SerializeObject(point);
            this.Type = this.point.Type;
        }

        public Position ReturnLocation()
        {
            if (point == null && !string.IsNullOrEmpty(Coordinates))
                point = JsonConvert.DeserializeObject<Point>(Coordinates);

            if (point != null)
                return new Position((float) point.Coordinates.Latitude, (float) point.Coordinates.Longitude);
            return null;
        }
    }
}
