using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace customer_info.Models
{
    [Table("Customers_temp")]
    public class Customers_temp
    {
        [Key]
        public int PersonID { get; set; }

        public DateTime? DateCol { get; set; }

        public string? NameCol { get; set; }

        public string? AddressCol { get; set; }

        public string? Mobile { get; set; }

        public string? Email { get; set; }

        public string? Gender { get; set; }

        public string? Occupation { get; set; }

        public int? Cost { get; set; }

        public string? ImagePath { get; set; }
    }
}
