using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NutraBioticsBackend.Models
{
    public class ChashHeaderView
    {

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

        public bool pFacturaConSaldo { get; set; }

        public bool pFiltroCliente { get; set; }

        public Decimal CashAmt { get; set; }


        public List<CashHeader> CashHeaders { get; set; }

    }
}