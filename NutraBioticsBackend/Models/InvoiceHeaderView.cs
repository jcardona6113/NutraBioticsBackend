using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NutraBioticsBackend.Models
{
    public class InvoiceHeaderView
    {

            [Key]
            public int InvoiceHeaderId { get; set; }


        public int CustNum { get; set; }


        public decimal InvoiceBal { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime? InvoiceDate { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime? ClosedDate { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime? DueDate { get; set; }


        [DataType(DataType.DateTime)]
        public DateTime? pFechaInicial { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? pFechaFinal { get; set; }


        [DataType(DataType.DateTime)]
        public DateTime? pCalendarFInicial { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? pCalendarFFinal { get; set; }

        public bool pFiltroFechas { get; set; }

        public bool pFiltroPeriodo { get; set; }

        public bool pFacturaConSaldo { get; set ; }

        public bool pFiltroCliente { get; set; }


        public List<InvoiceHeader> InvoiceHeaders { get; set; }


        [Display(Name = "Total")]
        [Editable(false)]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal TotalLine
        {
            get
            {
                if (InvoiceHeaders == null)
                {
                    return 0;
                }

                return InvoiceHeaders.Sum(od => od.InvoiceAmt);
            }

        }

    }
}



