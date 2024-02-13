using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IBMCAS.Models
{
    public enum GenderEnum
    {
        Male,
        Female
    }
    public partial class PatientRegistrationForm
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EnumDataType(typeof(GenderEnum))]
        public GenderEnum Gender { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        [MaxLength(12)]
        [MinLength(12)]
        public string AadhaarNumber { get; set; }

        public string MedicalHistory { get; set; }
    }
}