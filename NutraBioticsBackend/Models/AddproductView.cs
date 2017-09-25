﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NutraBioticsBackend.Models
{
    public class AddproductView
    {
        [Display(Name = "Lista de precios")]
        public int PriceListId { get; set; }

        [Display(Name = "Producto")]
        public int PartId { get; set; }

        [Display(Name = "Cantidad")]
        public decimal OrderQty { get; set; }

        [Display(Name = "U. Bonificadas")]
        public decimal Reference { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        [Display(Name = "Precio Unitario")]
        [DataType(DataType.Currency)]
        public decimal UnitPrice { get; set; }
    }
}