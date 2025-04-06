using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT.Model.Models
{
    public class Users
    {
        [Key]
        public int id { get; set; }

        [Required(ErrorMessage = "Role Name is required.")]
        public int roleId { get; set; }

        [Required(ErrorMessage = "First Name is required.")]
        public string? firstName { get; set; }

        [Required(ErrorMessage = "Last Name is required.")]
        public string? lastName { get; set; }

        [Required(ErrorMessage = "Email ID is required.")]
        public string? email { get; set; }
        public int? managerid { get; set; }
        public string? password { get; set; }
        public string? facebookurl { get; set; }
        public string? twitterurl { get; set; }
        public string? linkedinurl { get; set; }
        public string? profilepicture { get; set; }
        public int status { get; set; }
        public int? createdBy { get; set; }
        public DateTime? createdDate { get; set; }
        public int? updatedBy { get; set; }
        public DateTime? updatedDate { get; set; }
        public DateTime? lastlogin { get; set; }
        [NotMapped]
        public IFormFile? profilefile { get; set; }
        [NotMapped]
        public string? newpassword { get; set; }
        [NotMapped]
        public string? confirmpassword { get; set; }
    }
}
