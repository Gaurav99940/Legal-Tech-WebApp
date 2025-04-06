using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT.Model.ViewModels
{
    public class UserSignUp
    {
        [Required(ErrorMessage = "First Name is required.")]
        public string? firstName { get; set; }

        [Required(ErrorMessage = "Last Name is required.")]
        public string? lastName { get; set; }

        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Email ID is required.")]
        public string? email { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Email ID is required.")]
        public string? password { get; set; }
    }
}
