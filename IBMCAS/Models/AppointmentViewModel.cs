using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IBMCAS.Models
{
    public class AppointmentViewModel
    {
        public Appointment appointment;
        public Advice advice;
        public List<Prescription> prescriptions;
    }
}