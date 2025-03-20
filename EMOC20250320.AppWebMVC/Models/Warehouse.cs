using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EMOC20250320.AppWebMVC.Models;

public partial class Warehouse
{
    public int WarehouseId { get; set; }

    [Display(Name = "Bodega")]
    [Required(ErrorMessage = "La bodega es obligatorio")]
    public string WarehouseName { get; set; } = null!;

    [Display(Name = "Notas")]
    [Required(ErrorMessage = "La nota es obligatoria")]
    public string? Notes { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
