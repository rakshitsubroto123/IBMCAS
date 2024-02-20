using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace IBMCAS.Controllers
{
    public class FutureDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true; // Let the required attribute handle null values
            }

            DateTime date = (DateTime)value;
            return date.Date > DateTime.Now.Date;
        }
    }
}