using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IBMCAS.Models
{
    public class PrescriptionViewModel
    {
        public string DrugName { get; set; }
        public string DrugDescription { get; set; }
    }

    public class AdvicePrescriptionViewModel
    {
        public int ScheduleID { get; set; }
        public string AdviceText { get; set; }

        public List<PrescriptionViewModel> Prescriptions { get; set; }
    }
   
}