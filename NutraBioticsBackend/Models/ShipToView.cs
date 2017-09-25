using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NutraBioticsBackend.Models
{
    public class ShipToView
    {
        [Key]
        public int ShipToId { get; set; }

        [Display(Name = "Cliente POS")]
        public int CustomerId { get; set; }

        public string RowMod { get; set; }


        [Display(Name = "Cedula Paciente")]
        public string ShipToNum { get; set; }  //Epicor

        public int CustNum { get; set; } //Epicor

        [Display(Name = "Compañia")]
        public string Company { get; set; }

        [Display(Name = "Paciente")]
        public string ShipToName { get; set; }

        [Display(Name = "Territorio")]
        public string TerritoryEpicorId { get; set; }

        [Display(Name = "Pais")]
        public int CountryId { get; set; }

        [Display(Name = "Pais")]
        public string Country { get; set; }

        [Display(Name = "Departamento")]
        public string State { get; set; }

        [Display(Name = "Ciudad")]
        public string City { get; set; }

        [Display(Name = "Direccion")]
        public string Address { get; set; }

        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} carácteres")]
        [Display(Name = "Indicaciones")]
        public string Address2 { get; set; }

        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} carácteres")]
        [Display(Name = "Barrio")]
        public string Address3 { get; set; }

        [Display(Name = "Telefono")]
        public string PhoneNum { get; set; }

        public string Email { get; set; }

        public int VendorId { get; set; }

        public bool SincronizadoEpicor { get; set; }

        public List<Contact> Contacts { get; set; }

    }
}
