using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT.Model.Models
{
    public class Roles
    {
        [Key]
        public int id { get; set; }

        [Required(ErrorMessage = "Role Name is required.")]
        public string? name { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        public int status { get; set; }
        public int? createdby { get; set; }
        public int? newstatus { get; set; }
        public int? modifystatus { get; set; }
        public int? deletestatus { get; set; }
        public int? impersonatestatus { get; set; }
        public int? viewstatus { get; set; }
        public DateTime? createddate { get; set; }
        public int? updatedby { get; set; }
        public DateTime? updateddate { get; set; }
    }
}
