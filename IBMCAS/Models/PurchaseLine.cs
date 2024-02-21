using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IBMCAS.Models
{
    public class pBody
    {
        public string DrugName { get; set; }
        public int Qty { get; set; }
        public string ShortNote { get; set; }
    }
    public class PurchaseLine
    {
        public int SupplierId { get; set; }
        public string Note { get; set; }
        public List<pBody> pbdies { get; set; }
    }
}