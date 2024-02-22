using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IBMCAS.Models
{
    public class PatientDashBoardModel
    {
        public List<AppointmentRequest> AppointmentRequests { get; set; }

        public List<Appointment> Appointments { get; set; }

    }
}