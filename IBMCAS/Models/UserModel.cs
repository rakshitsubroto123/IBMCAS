using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IBMCAS.Models
{
    public class UserModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string UserPassword { get; set; }
    }

    public class CurrentUserModel
    {
        public int UserID { get; set; }
        public string UserName { get; set; }

        public string Role { get; set; }
        public int? ReferenceToId { get; set; }
    }


}