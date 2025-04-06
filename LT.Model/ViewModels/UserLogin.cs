using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT.Model.ViewModels
{
    public class UserLogin
    {
        public string emailid { get; set; }

        [DataType(DataType.Password)]
        public string password { get; set; }

        [Display(Name = "Remember Me")]
        public bool rememberme { get; set; }
    }
}
