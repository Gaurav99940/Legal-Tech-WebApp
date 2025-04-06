using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT.Model.Models.UserCaseModels
{
    public class UserCourtCaseDetails
    {
        [Key]
        public int CaseID { get; set; } // Primary Key

        [Required]
        public int UserID { get; set; } // Foreign Key linking to the Users table

        [Required]
        [MaxLength(255)]
        public string CaseTitle { get; set; } // Short description of the case

        [Required]
        [MaxLength(100)]
        public string CaseType { get; set; } // e.g., Civil, Criminal, etc.

        [Required]
        [MaxLength(255)]
        public string CourtName { get; set; } // Court handling the case

        [Required]
        public DateTime FilingDate { get; set; } // Date the case was filed

        public DateTime? HearingDate { get; set; } // Next hearing date (nullable)

        [Required]
        [MaxLength(50)]
        public string CaseStatus { get; set; } // Status of the case (Pending, Closed, etc.)

        [MaxLength(255)]
        public string LawyerName { get; set; } // User's lawyer name

        [MaxLength(255)]
        public string OpponentName { get; set; } // Name of the opposing party

        [MaxLength(255)]
        public string OpponentLawyerName { get; set; } // Opponent's lawyer name

        public string CaseDescription { get; set; } // Detailed description of the case

        public DateTime? VerdictDate { get; set; } // Date of final verdict (nullable)

        public string VerdictDetails { get; set; } // Details of the verdict (nullable)

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.Now; // Auto-set creation date

        [Required]
        public DateTime ModifiedDate { get; set; } = DateTime.Now; // Auto-set modification date

        [Required]
        public bool IsActive { get; set; } = true; // Flag to mark the case as active/inactive

        public string? pdf { get; set; }

        public string Remarks { get; set; } // Additional remarks or notes

        [NotMapped]
        public IFormFile? pdfFile { get; set; }

    }
}
