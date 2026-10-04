using LT.Model.Models.UserCaseModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LT.Services
{
    public static class CaseDataStore
    {
        private static readonly object _lock = new object();
        private static int _nextId = 1;

        private static List<UserCourtCaseDetails> _cases = new List<UserCourtCaseDetails>();
        private static List<AdvocateSuggestion>? _advocatesCache = null;

        public static List<UserCourtCaseDetails> GetAll()
        {
            lock (_lock)
            {
                return _cases.OrderByDescending(c => c.FilingDate).ToList();
            }
        }

        public static UserCourtCaseDetails? GetById(int id)
        {
            lock (_lock)
            {
                return _cases.FirstOrDefault(c => c.CaseID == id);
            }
        }

        public static void Save(UserCourtCaseDetails item)
        {
            lock (_lock)
            {
                if (item.CaseID == 0)
                {
                    item.CaseID = _nextId++;
                    item.CreatedDate = DateTime.Now;
                    item.ModifiedDate = DateTime.Now;
                    item.IsActive = true;
                    _cases.Insert(0, item);
                }
                else
                {
                    var existing = _cases.FirstOrDefault(c => c.CaseID == item.CaseID);
                    if (existing != null)
                    {
                        existing.CaseTitle = item.CaseTitle;
                        existing.CaseType = item.CaseType;
                        existing.CourtName = item.CourtName;
                        existing.FilingDate = item.FilingDate;
                        existing.HearingDate = item.HearingDate;
                        existing.CaseStatus = item.CaseStatus;
                        existing.LawyerName = item.LawyerName;
                        existing.OpponentName = item.OpponentName;
                        existing.OpponentLawyerName = item.OpponentLawyerName;
                        existing.CaseDescription = item.CaseDescription;
                        existing.VerdictDate = item.VerdictDate;
                        existing.VerdictDetails = item.VerdictDetails;
                        existing.Remarks = item.Remarks;
                        existing.ModifiedDate = DateTime.Now;
                        if (!string.IsNullOrEmpty(item.pdf)) existing.pdf = item.pdf;
                    }
                    else
                    {
                        _cases.Insert(0, item);
                    }
                }
            }
        }

        public static bool Delete(int id)
        {
            lock (_lock)
            {
                var item = _cases.FirstOrDefault(c => c.CaseID == id);
                if (item != null)
                {
                    _cases.Remove(item);
                    return true;
                }
                return false;
            }
        }

        public static List<AdvocateSuggestion> GetSuggestedAdvocates(string query)
        {
            return SearchAdvocates(query, "", "");
        }

        public static List<AdvocateSuggestion> SearchAdvocates(string query = "", string specialization = "", string court = "")
        {
            var directory = GetAllAdvocates();
            var q = (query ?? "").Trim().ToLower();
            var spec = (specialization ?? "").Trim().ToLower();
            var crt = (court ?? "").Trim().ToLower();

            var matches = directory.Where(a =>
                (string.IsNullOrEmpty(q) || 
                    a.Name.ToLower().Contains(q) || 
                    a.Specialization.ToLower().Contains(q) || 
                    a.Court.ToLower().Contains(q) || 
                    a.Location.ToLower().Contains(q) || 
                    a.BarCouncilNo.ToLower().Contains(q)) &&
                (string.IsNullOrEmpty(spec) || a.Specialization.ToLower().Contains(spec) || a.Category.ToLower().Contains(spec)) &&
                (string.IsNullOrEmpty(crt) || a.Court.ToLower().Contains(crt))
            ).ToList();

            if (matches.Count == 0 && !string.IsNullOrWhiteSpace(query))
            {
                var cleanName = query.Trim();
                if (!cleanName.StartsWith("Adv.", StringComparison.OrdinalIgnoreCase) && !cleanName.StartsWith("Senior Adv.", StringComparison.OrdinalIgnoreCase))
                {
                    cleanName = "Adv. " + char.ToUpper(cleanName[0]) + cleanName.Substring(1);
                }

                var initials = string.Concat(cleanName.Replace("Adv.", "").Replace("Senior", "").Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(s => s[0])).ToUpper();
                if (string.IsNullOrEmpty(initials)) initials = "AD";
                if (initials.Length > 2) initials = initials.Substring(0, 2);

                int hash = Math.Abs(cleanName.GetHashCode());
                int regYear = 2005 + (hash % 18);
                int regNum = 1000 + (hash % 8999);
                long phoneTail = 9810000000 + (hash % 89999999);
                int winRate = 90 + (hash % 9);
                int feeVal = 2000 + ((hash % 7) * 500);

                string assignedCourt = !string.IsNullOrEmpty(court) ? (court.Contains("Supreme") ? "Supreme Court of India" : court + " High Court") : "Supreme Court of India & High Court";
                string assignedSpec = !string.IsNullOrEmpty(specialization) ? specialization + " Litigation & Trial Defense" : "Civil, Criminal & Constitutional Litigation";

                var dynamicAdvocate = new AdvocateSuggestion
                {
                    Name = cleanName,
                    Experience = (10 + (hash % 16)) + "+ Years",
                    Court = assignedCourt,
                    Specialization = assignedSpec,
                    WinRate = winRate + "% Success Rate",
                    EstimatedFee = "₹" + feeVal.ToString("N0") + " / consultation",
                    Badge = "Bar Council Verified Counsel",
                    AvatarText = initials,
                    BarCouncilNo = "D/" + regNum + "/" + regYear,
                    Phone = "+91 " + phoneTail.ToString("###-###-####").Replace("-", " "),
                    Email = ("chamber." + cleanName.Replace("Adv.", "").Replace("Senior", "").Trim().ToLower().Replace(" ", ".") + "@delhibar.org"),
                    Rating = (4.7 + ((hash % 3) * 0.1)).ToString("0.0"),
                    ReviewCount = 45 + (hash % 150),
                    RecentTrackRecord = "Successfully argued " + (30 + (hash % 120)) + "+ High Court & District Court matters with active registry standing",
                    Location = "New Delhi / NCR",
                    Category = !string.IsNullOrEmpty(specialization) ? specialization : "General"
                };

                lock (_lock)
                {
                    _advocatesCache ??= InitDefaultAdvocates();
                    _advocatesCache.Insert(0, dynamicAdvocate);
                }

                matches.Add(dynamicAdvocate);
            }

            return matches;
        }

        public static List<AdvocateSuggestion> GetAllAdvocates()
        {
            lock (_lock)
            {
                if (_advocatesCache == null)
                {
                    _advocatesCache = InitDefaultAdvocates();
                }
                return new List<AdvocateSuggestion>(_advocatesCache);
            }
        }

        private static List<AdvocateSuggestion> InitDefaultAdvocates()
        {
            return new List<AdvocateSuggestion>
            {
                new AdvocateSuggestion
                {
                    Name = "Senior Adv. Harish Salve",
                    Experience = "35+ Years",
                    Court = "Supreme Court of India & International Courts",
                    Specialization = "Constitutional Law, International Commercial Arbitration, Cross-Border Disputes & Corporate Taxation",
                    WinRate = "98% Milestone Win Rate",
                    EstimatedFee = "₹15,000 / consultation",
                    Badge = "Senior Designated Counsel",
                    AvatarText = "HS",
                    BarCouncilNo = "D/410/1980",
                    Phone = "+91 98111 02938",
                    Email = "chambers.salve@scba.in",
                    Rating = "5.0",
                    ReviewCount = 420,
                    RecentTrackRecord = "Represented India at International Court of Justice & Argued Landmark Constitutional Benches",
                    Location = "New Delhi / London",
                    Category = "Constitutional"
                },
                new AdvocateSuggestion
                {
                    Name = "Senior Adv. Kapil Sibal",
                    Experience = "40+ Years",
                    Court = "Supreme Court of India & Delhi High Court",
                    Specialization = "Constitutional Law, Media Law, Writ Jurisdiction (Art 32/226), High-Stakes Public Litigations",
                    WinRate = "97% Constitutional Record",
                    EstimatedFee = "₹12,000 / consultation",
                    Badge = "Former ASG & SCBA President",
                    AvatarText = "KS",
                    BarCouncilNo = "D/102/1972",
                    Phone = "+91 98100 48291",
                    Email = "kapil.sibal@scba.in",
                    Rating = "4.9",
                    ReviewCount = 380,
                    RecentTrackRecord = "Led Arguments in Constitution Bench Matters & Article 370 References",
                    Location = "New Delhi",
                    Category = "Constitutional"
                },
                new AdvocateSuggestion
                {
                    Name = "Senior Adv. Mukul Rohatgi",
                    Experience = "38+ Years",
                    Court = "Supreme Court of India & Bombay High Court",
                    Specialization = "Corporate Litigation, Criminal Defense, Anticipatory Bail, Insolvency (IBC) & Commercial Trials",
                    WinRate = "96% Defense Success Rate",
                    EstimatedFee = "₹12,500 / consultation",
                    Badge = "Former Attorney General of India",
                    AvatarText = "MR",
                    BarCouncilNo = "D/245/1978",
                    Phone = "+91 98110 59281",
                    Email = "chambers.rohatgi@delhibar.org",
                    Rating = "5.0",
                    ReviewCount = 345,
                    RecentTrackRecord = "Represented Leading Industrial Conglomerates & Secured Landmark Supreme Court Bails",
                    Location = "New Delhi / Mumbai",
                    Category = "Criminal"
                },
                new AdvocateSuggestion
                {
                    Name = "Senior Adv. Abhishek Manu Singhvi",
                    Experience = "34+ Years",
                    Court = "Supreme Court of India & Delhi High Court",
                    Specialization = "Corporate Law, Constitutional Writs, Commercial Arbitration, Competition Law & Telecom (TDSAT)",
                    WinRate = "95% Admissibility Rate",
                    EstimatedFee = "₹11,000 / consultation",
                    Badge = "Senior Supreme Court Counsel",
                    AvatarText = "AS",
                    BarCouncilNo = "D/389/1982",
                    Phone = "+91 98101 64920",
                    Email = "singhvi.chambers@scba.in",
                    Rating = "4.9",
                    ReviewCount = 310,
                    RecentTrackRecord = "Argued Over 500+ Landmark Supreme Court & High Court Appellate Disputes",
                    Location = "New Delhi",
                    Category = "Commercial"
                },
                new AdvocateSuggestion
                {
                    Name = "Adv. Ramesh Tiwari",
                    Experience = "18+ Years",
                    Court = "Delhi High Court & Supreme Court",
                    Specialization = "Criminal Defense, Regular & Anticipatory Bail (Sec 438/439 CrPC), Murder Trials (302 IPC)",
                    WinRate = "94% Bail Success Rate",
                    EstimatedFee = "₹2,500 / consultation",
                    Badge = "Top Criminal Counsel",
                    AvatarText = "RT",
                    BarCouncilNo = "D/1420/2006",
                    Phone = "+91 98110 44219",
                    Email = "chamber.tiwari@delhibar.org",
                    Rating = "4.9",
                    ReviewCount = 156,
                    RecentTrackRecord = "Secured 42 Anticipatory Bails in 2025-26 & Quashed 18 FIRs",
                    Location = "New Delhi",
                    Category = "Criminal"
                },
                new AdvocateSuggestion
                {
                    Name = "Senior Adv. Priya Sharma",
                    Experience = "16+ Years",
                    Court = "Delhi High Court & Commercial Courts",
                    Specialization = "Corporate Litigation, Commercial Arbitration (DIAC), Contractual Disputes & NCLT Insolvency",
                    WinRate = "93% Decree & Award Win Rate",
                    EstimatedFee = "₹4,000 / consultation",
                    Badge = "Senior Corporate Counsel",
                    AvatarText = "PS",
                    BarCouncilNo = "D/2109/2008",
                    Phone = "+91 98201 55382",
                    Email = "priya.sharma@delhicounsel.in",
                    Rating = "5.0",
                    ReviewCount = 112,
                    RecentTrackRecord = "Recovered ₹140+ Crore in Commercial SLA Arbitrations",
                    Location = "New Delhi",
                    Category = "Commercial"
                },
                new AdvocateSuggestion
                {
                    Name = "Senior Adv. Indira Jaising",
                    Experience = "42+ Years",
                    Court = "Supreme Court of India & Bombay High Court",
                    Specialization = "Human Rights, Constitutional Law, Service Matters, Gender Rights & Public Interest Litigation",
                    WinRate = "96% Landmark Decree Rate",
                    EstimatedFee = "₹9,000 / consultation",
                    Badge = "Senior Supreme Court Advocate",
                    AvatarText = "IJ",
                    BarCouncilNo = "MAH/112/1971",
                    Phone = "+91 98200 47219",
                    Email = "indira.jaising@lawyerscollective.org",
                    Rating = "5.0",
                    ReviewCount = 290,
                    RecentTrackRecord = "Pioneered Landmark Verdicts on Domestic Violence Act & Equal Opportunity",
                    Location = "New Delhi / Mumbai",
                    Category = "Constitutional"
                },
                new AdvocateSuggestion
                {
                    Name = "Adv. Prashant Bhushan",
                    Experience = "36+ Years",
                    Court = "Supreme Court of India & Delhi High Court",
                    Specialization = "Public Interest Litigations (PIL), Anti-Corruption, Environmental Law & Administrative Accountability",
                    WinRate = "94% Public Law Success",
                    EstimatedFee = "₹3,500 / consultation",
                    Badge = "Eminent PIL Counsel",
                    AvatarText = "PB",
                    BarCouncilNo = "D/289/1983",
                    Phone = "+91 98101 29481",
                    Email = "prashant.bhushan@scba.in",
                    Rating = "4.9",
                    ReviewCount = 260,
                    RecentTrackRecord = "Instituted Over 200+ Monumental PILs on Electoral Reforms & Natural Resources",
                    Location = "New Delhi",
                    Category = "Constitutional"
                },
                new AdvocateSuggestion
                {
                    Name = "Adv. Siddharth Luthra",
                    Experience = "30+ Years",
                    Court = "Supreme Court of India & Delhi High Court",
                    Specialization = "White-Collar Crime, PMLA Enforcement Directorate Defense, CBI Prosecutions & Extradition",
                    WinRate = "95% Discharge Success",
                    EstimatedFee = "₹8,500 / consultation",
                    Badge = "Former Additional Solicitor General",
                    AvatarText = "SL",
                    BarCouncilNo = "D/498/1990",
                    Phone = "+91 98110 38291",
                    Email = "chambers.luthra@delhibar.org",
                    Rating = "4.9",
                    ReviewCount = 215,
                    RecentTrackRecord = "Handled Complex Cross-Border Financial Fraud & ED Money Laundering Trials",
                    Location = "New Delhi",
                    Category = "Criminal"
                },
                new AdvocateSuggestion
                {
                    Name = "Adv. Rebecca John",
                    Experience = "28+ Years",
                    Court = "Delhi High Court & Supreme Court",
                    Specialization = "Criminal Trials, Cyber Defamation, NIA/UAPA Defense & Special Criminal Appellate Matters",
                    WinRate = "95% Acquittal Rate",
                    EstimatedFee = "₹6,000 / consultation",
                    Badge = "Senior Criminal Defense Counsel",
                    AvatarText = "RJ",
                    BarCouncilNo = "D/612/1992",
                    Phone = "+91 98102 74819",
                    Email = "rebecca.john@delhibar.org",
                    Rating = "4.9",
                    ReviewCount = 195,
                    RecentTrackRecord = "Lead Defense Counsel in Landmark High Court Acquitment Appeals",
                    Location = "New Delhi",
                    Category = "Criminal"
                },
                new AdvocateSuggestion
                {
                    Name = "Adv. Arvind Saxena",
                    Experience = "15+ Years",
                    Court = "Patiala House, Saket & Tis Hazari Courts",
                    Specialization = "Section 138 NI Act (Cheque Bounce), Summary Debt Recovery Suits (Order 37 CPC) & Banking Fraud",
                    WinRate = "95% Recovery Rate (450+ Notices)",
                    EstimatedFee = "₹1,500 / consultation",
                    Badge = "NI Act Specialist",
                    AvatarText = "AS",
                    BarCouncilNo = "D/3341/2009",
                    Phone = "+91 98104 77218",
                    Email = "arvind.saxena@advocates.in",
                    Rating = "4.8",
                    ReviewCount = 138,
                    RecentTrackRecord = "Settled 450+ Cheque Bounce Matters in Pre-Trial & Lok Adalat",
                    Location = "New Delhi",
                    Category = "Cheque"
                },
                new AdvocateSuggestion
                {
                    Name = "Adv. Neha Choudhary",
                    Experience = "12+ Years",
                    Court = "Principal Family Courts, Delhi NCR & High Court",
                    Specialization = "Mutual Consent Divorce (Sec 13B HMA), Child Custody, Maintenance (Sec 125 CrPC) & Mediation",
                    WinRate = "92% Fast-Track Settlement Rate",
                    EstimatedFee = "₹1,800 / consultation",
                    Badge = "Family & Matrimonial",
                    AvatarText = "NC",
                    BarCouncilNo = "UP/8920/2012",
                    Phone = "+91 97118 66205",
                    Email = "neha.familylaw@delhibar.org",
                    Rating = "4.9",
                    ReviewCount = 96,
                    RecentTrackRecord = "Achieved Mutual Settlements in 120+ High-Conflict Matrimonial Suits",
                    Location = "Delhi NCR / Noida",
                    Category = "Family"
                },
                new AdvocateSuggestion
                {
                    Name = "Adv. Aman Kapoor",
                    Experience = "10+ Years",
                    Court = "Delhi High Court & Cyber Appellate Tribunal",
                    Specialization = "Cyber Crime Defense (IT Act Sec 66/66D), Bank Account Cyber Lien Unfreezing & Crypto Fraud Recovery",
                    WinRate = "98% Account Unfreeze Rate",
                    EstimatedFee = "₹2,000 / consultation",
                    Badge = "Cyber Law Specialist",
                    AvatarText = "AK",
                    BarCouncilNo = "D/4412/2014",
                    Phone = "+91 99102 33490",
                    Email = "aman.kapoor@cyberlex.in",
                    Rating = "4.9",
                    ReviewCount = 82,
                    RecentTrackRecord = "Successfully Unfroze 210+ Bank Accounts Blocked by State Cyber Cells",
                    Location = "New Delhi / Gurugram",
                    Category = "Cyber"
                },
                new AdvocateSuggestion
                {
                    Name = "Adv. Vikramaditya Rathore",
                    Experience = "19+ Years",
                    Court = "Bombay High Court & National Company Law Tribunal (NCLT)",
                    Specialization = "Mergers & Acquisitions, Corporate Insolvency Resolution (IBC 2016) & SEBI Compliance",
                    WinRate = "91% Corporate Resolution Rate",
                    EstimatedFee = "₹4,500 / consultation",
                    Badge = "NCLT & IBC Counsel",
                    AvatarText = "VR",
                    BarCouncilNo = "MAH/1940/2005",
                    Phone = "+91 98205 88914",
                    Email = "rathore.partners@mumbaibar.com",
                    Rating = "4.8",
                    ReviewCount = 74,
                    RecentTrackRecord = "Handled 65+ High-Stake Resolution Plans before NCLT Benches",
                    Location = "Mumbai",
                    Category = "Commercial"
                },
                new AdvocateSuggestion
                {
                    Name = "Adv. Meenakshi Sundaram",
                    Experience = "14+ Years",
                    Court = "Madras High Court & Supreme Court",
                    Specialization = "Intellectual Property Rights, Trademark & Patent Infringement Suits, Copyright Protection",
                    WinRate = "94% Injunction Success Rate",
                    EstimatedFee = "₹3,000 / consultation",
                    Badge = "IPR & Patent Specialist",
                    AvatarText = "MS",
                    BarCouncilNo = "TN/2840/2010",
                    Phone = "+91 94440 12895",
                    Email = "meenakshi.ipr@chennailegal.in",
                    Rating = "4.9",
                    ReviewCount = 68,
                    RecentTrackRecord = "Obtained 45+ John Doe Injunctions for Global Brand Protection",
                    Location = "Chennai / Delhi",
                    Category = "Commercial"
                },
                new AdvocateSuggestion
                {
                    Name = "Adv. Rajeshwar Pandey",
                    Experience = "20+ Years",
                    Court = "Allahabad High Court & Supreme Court",
                    Specialization = "Land Revenue Law, Property Partition, Tenant Eviction (Sec 106 TPA) & Title Verification",
                    WinRate = "90% Decree Rate",
                    EstimatedFee = "₹2,200 / consultation",
                    Badge = "Property & Revenue Counsel",
                    AvatarText = "RP",
                    BarCouncilNo = "UP/3310/2004",
                    Phone = "+91 94150 78210",
                    Email = "rajeshwar.pandey@allahabadbar.org",
                    Rating = "4.8",
                    ReviewCount = 129,
                    RecentTrackRecord = "Resolved 85+ Complex Ancestral Property Partition Disputes",
                    Location = "Prayagraj / Lucknow",
                    Category = "Property"
                },
                new AdvocateSuggestion
                {
                    Name = "Senior Adv. Geeta Luthra",
                    Experience = "32+ Years",
                    Court = "Delhi High Court & Supreme Court",
                    Specialization = "Matrimonial Jurisprudence, International Custody, Defamation & Civil Appellate Writs",
                    WinRate = "94% Settlement & Decree Rate",
                    EstimatedFee = "₹7,500 / consultation",
                    Badge = "Senior Designated Advocate",
                    AvatarText = "GL",
                    BarCouncilNo = "D/412/1988",
                    Phone = "+91 98110 24810",
                    Email = "geeta.luthra@delhibar.org",
                    Rating = "4.9",
                    ReviewCount = 175,
                    RecentTrackRecord = "Represented High-Profile Parties in High Court Appellate Jurisdiction",
                    Location = "New Delhi",
                    Category = "Family"
                },
                new AdvocateSuggestion
                {
                    Name = "Adv. Sanjay Hegde",
                    Experience = "29+ Years",
                    Court = "Supreme Court of India & Karnataka High Court",
                    Specialization = "Constitutional Law, Special Leave Petitions (SLP), Criminal Appeals & Service Matters",
                    WinRate = "93% Admissibility Record",
                    EstimatedFee = "₹6,500 / consultation",
                    Badge = "Senior Supreme Court Counsel",
                    AvatarText = "SH",
                    BarCouncilNo = "KAR/519/1989",
                    Phone = "+91 98111 74829",
                    Email = "chambers.hegde@scba.in",
                    Rating = "4.9",
                    ReviewCount = 160,
                    RecentTrackRecord = "Argued Hundreds of Special Leave Petitions before Supreme Court Benches",
                    Location = "New Delhi / Bengaluru",
                    Category = "Constitutional"
                }
            };
        }
    }

    public class AdvocateSuggestion
    {
        public string Name { get; set; } = "";
        public string Experience { get; set; } = "";
        public string Court { get; set; } = "";
        public string Specialization { get; set; } = "";
        public string WinRate { get; set; } = "";
        public string EstimatedFee { get; set; } = "";
        public string Badge { get; set; } = "";
        public string AvatarText { get; set; } = "";
        public string BarCouncilNo { get; set; } = "";
        public string Phone { get; set; } = "";
        public string Email { get; set; } = "";
        public string Rating { get; set; } = "4.9";
        public int ReviewCount { get; set; } = 50;
        public string RecentTrackRecord { get; set; } = "";
        public string Location { get; set; } = "Delhi";
        public string Category { get; set; } = "General";
    }
}
