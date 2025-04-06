using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT.Model.Models
{
    public class RoleMenuMapping
    {
        [Key]
        public int id { get; set; }
        public int? roleid { get; set; }
        public int? menuid { get; set; }
        public int? status { get; set; }
        public int? createdby { get; set; }
        public int? updatedby { get; set; }
        public DateTime? createddate { get; set; }
        public DateTime? updateddate { get; set; }
    }
}
