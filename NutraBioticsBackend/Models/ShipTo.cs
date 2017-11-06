namespace NutraBioticsBackend.Models
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class ShipTo
    {
        [Key]
        [Display(Name = "Paciente")]
        public int ShipToId { get; set; }

        [Display(Name = "Cliente POS")]
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string ShipToNum { get; set; }  //Epicor

        [Editable(false)]
        public int CustNum { get; set; } //epicor

        [Display(Name = "Compañia")]
        public string Company { get; set; }

        [Display(Name = "Paciente")]
        public string ShipToName { get; set; }

        [Display(Name = "Territorio")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string TerritoryEpicorId { get; set; }

        [Display(Name = "Pais")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public int CountryId { get; set; }

        [Display(Name = "Pais")]
        public string Country { get; set; }

        [Display(Name = "Departamento")]
        public string State { get; set; }

        [Display(Name = "Ciudad")]
        public string City { get; set; }

        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} carácteres")]
        [Display(Name = "Direccion")]
        public string Address { get; set; }

        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} carácteres")]
        [Display(Name = "Indicaciones")]
        public string Address2 { get; set; }

        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} carácteres")]
        [Display(Name = "Barrio")]
        public string Address3 { get; set; }

        [MaxLength(20, ErrorMessage = "El campo {0} debe tener máximo {1} carácteres")]
        [Display(Name = "Telefono")]
        public string PhoneNum { get; set; }

        public string Email { get; set; }

        public int VendorId { get; set; }


        public string RowMod { get; set; }


        public bool SincronizadoEpicor { get; set; }

        [JsonIgnore]
        public virtual ICollection<Contact> Contacts { get; set; }

        [JsonIgnore]
        public virtual Customer Customer { get; set; }

        [JsonIgnore]
        public virtual ICollection<OrderHeader> OrderHeaders { get; set; }
    }
}