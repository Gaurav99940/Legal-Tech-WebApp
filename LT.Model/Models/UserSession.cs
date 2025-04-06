using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT.Model.Models
{
    public class UserSession
    {
        [Key]
        public int id { get; set; }
        public string userid { get; set; }
        public string sessionid { get; set; }
    }
}
