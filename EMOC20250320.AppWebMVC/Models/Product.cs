using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EMOC20250320.AppWebMVC.Models;

public partial class Product
{
    public int ProductId { get; set; }

    [Required(ErrorMessage = "El nombre del producto es obligatorio")]
    [Display(Name = "Nombre del Producto")]
    public string ProductName { get; set; } = null!;

    [Required(ErrorMessage = "La descripcion es obligatoria")]
    [Display(Name = "Descripción")]
    public string? Description { get; set; }

    [Range(typeof(decimal), "0.001", "99999999.99", ErrorMessage = "El precio debe estar entre 0.01 y 99999999.99")]
    [Display(Name = "Precio")]
    public decimal Price { get; set; }

    [Display(Name = "Bodega")]
    [Required(ErrorMessage = "La bodega es obligatoria")]
    public int? WarehouseId { get; set; }

    [Display(Name = "Marca")]
    [Required(ErrorMessage = "La marca es obligatoria")]
    public int? BrandId { get; set; }

    public virtual Brand? Brand { get; set; }

    public virtual Warehouse? Warehouse { get; set; }
}
