using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT.Model.Models
{
    public class Constitution
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Constitution number is required")]
        public string? ConstitutionNumber { get; set; }
        [Required(ErrorMessage = "Constitution Name is required")]
        public string? ConstitutionName { get; set; }
        [Required(ErrorMessage = "Constitution Description is required")]
        public string? ConstitutionDescription { get; set; }
        [Required(ErrorMessage = "Constitution Description is required")]
        public string? ConstitutionDetails { get; set; }
    }
}
