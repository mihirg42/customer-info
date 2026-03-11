using customer_info.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Customers_Unit_temp
{
    [Key]
    public int UnitID { get; set; }

    public string? UnitNo { get; set; }

    public int? Area { get; set; }

    public int? UnitCost { get; set; }

    public int? PersonID { get; set; }

    [ForeignKey("PersonID")]
    public Customers_temp? Customer { get; set; }
}
