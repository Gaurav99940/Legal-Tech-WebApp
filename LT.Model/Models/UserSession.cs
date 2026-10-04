using System.ComponentModel.DataAnnotations;

namespace LT.Model.Models
{
    public class UserSession
    {
        [Key]
        public int id { get; set; }
        public string? userid { get; set; }
        public string? sessionid { get; set; }
    }
}
