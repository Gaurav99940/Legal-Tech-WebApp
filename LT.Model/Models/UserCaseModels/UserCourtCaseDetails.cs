using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LT.Model.Models.UserCaseModels
{
    public class UserCourtCaseDetails
    {
        [Key]
        public int CaseID { get; set; }

        public int? UserID { get; set; }

        public string? CaseTitle { get; set; }

        public string? CaseType { get; set; }

        public string? CourtName { get; set; }

        public DateTime FilingDate { get; set; } = DateTime.Now;

        public DateTime? HearingDate { get; set; }

        public string? CaseStatus { get; set; }

        public string? LawyerName { get; set; }

        public string? OpponentName { get; set; }

        public string? OpponentLawyerName { get; set; }

        public string? CaseDescription { get; set; }

        public DateTime? VerdictDate { get; set; }

        public string? VerdictDetails { get; set; }

        public DateTime? CreatedDate { get; set; } = DateTime.Now;

        public DateTime? ModifiedDate { get; set; } = DateTime.Now;

        public bool? IsActive { get; set; } = true;

        public string? pdf { get; set; }

        public string? Remarks { get; set; }

        [NotMapped]
        public IFormFile? pdfFile { get; set; }
    }
}
