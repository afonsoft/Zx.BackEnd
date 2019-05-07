using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Zx.BackEnd.Data.Entity;

namespace Zx.BackEnd.Data.Entity
{
    public class PontoDeVenda
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string TradingName { get; set; }
        [Required]
        [StringLength(100)]
        public string OwnerName { get; set; }
        [Required]
        [StringLength(20)]
        public string Document { get; set; }
        public Address Address { get; set; }
        public CoverageArea CoverageArea { get; set; }
        
        public double ReturnDistance(double latitude, double longitude)
        {
            return Helper.GeoJsonHelper.GetDistance(this.Address.ReturnLocation().Latitude, this.Address.ReturnLocation().Longitude, latitude, longitude);
        }
    }
}
