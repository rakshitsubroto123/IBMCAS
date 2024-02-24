using IBMCAS.Models;
using System;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Linq;

public class EmailValidationAttribute : ValidationAttribute
{
    IBMCASDBEntities2 _db = new IBMCASDBEntities2();

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value != null)
        {
           
            
            UserCred uc = _db.UserCreds.Where(u => u.UserName == value.ToString()).FirstOrDefault();
            if (uc == null)
            {
                PatientRegistrationQueue pr = _db.PatientRegistrationQueues.Where(u => u.PatientEmail == value.ToString()).FirstOrDefault();
                if (pr == null)
                {
                    return ValidationResult.Success;
                }
                return new ValidationResult("Email Already in use");
            }
            return new ValidationResult("Email Already in use");
        }

        // Validation succeeded
        return new ValidationResult("Email Cannot be Null");
    }
}