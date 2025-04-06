using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT.Model.Models
{
    public class Menu
    {
        [Key]
        public int id { get; set; }
        public string? name { get; set; }
        public string? icon { get; set; }
        public int? parentid { get; set; }
        public string? controller { get; set; }
        public string? action { get; set; }
        public int? status { get; set; }
        public int? internalstatus { get; set; }
        public int? order { get; set; }
        public int? createdby { get; set; }
        public DateTime? createddate { get; set; }
        public int? updatedby { get; set; }
        public DateTime? updateddate { get; set; }
    }
}
