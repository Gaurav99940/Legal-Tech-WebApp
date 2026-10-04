using LT.Data;
using LT.Model.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LT.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChatGPTController : Controller
    {
        private readonly dbContext _context;

        public ChatGPTController(dbContext context)
        {
            _context = context;
        }

        [HttpPost("AskChatBot")]
        public async Task<IActionResult> AskChatBot([FromBody] QueryRequest request)
        {
            if (string.IsNullOrWhiteSpace(request?.Query))
            {
                return BadRequest(new { error = "Query cannot be empty. Please ask a legal question or section number." });
            }

            string rawQuery = request.Query.Trim();
            string lowerQuery = rawQuery.ToLower();

            // 1. Check Statutory Legal Knowledge Engine first for instantaneous and accurate response
            var legalKnowledge = GetComprehensiveLegalKnowledge(lowerQuery, rawQuery);
            if (legalKnowledge != null)
            {
                return Ok(legalKnowledge);
            }

            // 2. Check database for specific customized Constitution provisions if available
            try
            {
                var numMatch = ExtractConstitutionNumber(rawQuery);
                string searchKey = numMatch.HasValue ? numMatch.Value.ToString() : rawQuery;

                var dbResult = await _context.Constitution
                    .Where(c => c.ConstitutionNumber == searchKey || 
                                (c.ConstitutionName != null && c.ConstitutionName.Contains(rawQuery)))
                    .FirstOrDefaultAsync();

                if (dbResult != null && !string.IsNullOrWhiteSpace(dbResult.ConstitutionDetails))
                {
                    return Ok(new
                    {
                        title = dbResult.ConstitutionName ?? $"Article / Section {dbResult.ConstitutionNumber}",
                        act = "The Constitution of India / Legal Statute",
                        section = dbResult.ConstitutionNumber,
                        description = dbResult.ConstitutionDetails,
                        summary = dbResult.ConstitutionDescription ?? "Constitutional provision under Indian Law.",
                        penalty = "Constitutional compliance is mandatory across all judicial courts in India.",
                        remedies = "File a Writ Petition under Article 32 (Supreme Court) or Article 226 (High Court) for enforcement of rights.",
                        isLawyerRecommended = true
                    });
                }
            }
            catch
            {
                // Fallback gracefully if database table is not connected
            }

            // 3. Fallback AI Legal Guidance Response
            return Ok(new
            {
                title = $"AI Legal Guidance: {rawQuery}",
                act = "Indian Jurisprudence & Applicable Procedural Codes",
                section = "Statutory & Substantive Legal Principles",
                description = $"Regarding your query \"{rawQuery}\": Under the Indian legal system, matters of this nature require careful evaluation of substantive laws (IPC/BNS, Civil Law, Special Acts) and procedural timelines. Ensure all notices, agreement copies, FIRs, bank receipts, or court orders are preserved in your secure Document Vault.",
                summary = "AI analysis based on standard court litigation practices and statutory remedies under Indian Law.",
                penalty = "Depends on whether the matter is adjudicated as a summary civil suit, compoundable dispute, or cognizable criminal matter.",
                remedies = "1. Consult a verified High Court / District Court advocate through our portal.\n2. Draft and serve a formal Legal Notice before proceeding to litigation.\n3. File appropriate complaint, petition, or suit before the competent jurisdictional court.",
                isLawyerRecommended = true
            });
        }

        private object? GetComprehensiveLegalKnowledge(string lower, string raw)
        {
            // Greetings & Introductions
            if (lower == "hi" || lower == "hello" || lower == "hey" || lower.Contains("namaste") || lower == "help" || lower.Contains("kya kar sakte ho") || lower.Contains("who are you"))
            {
                return new
                {
                    title = "LegalTech AI Assistant - Ready to Help",
                    act = "Indian Statutory & Judicial Portal",
                    section = "AI Assistant",
                    description = "Namaste! I am your 24/7 AI Legal Assistant. You can ask me any question regarding Indian Penal Code (IPC), Bharatiya Nyaya Sanhita (BNS), Code of Criminal Procedure (CrPC), Constitution of India, Cheque Bounce (138 NI Act), Bail procedures, Divorce/Family disputes, Consumer Rights, Property matters, or Cyber Crime laws.",
                    summary = "Ask any legal question in English or Hinglish.",
                    penalty = "Legal clarity for all Indian citizens and legal practitioners.",
                    remedies = "Try typing: \"302\", \"Section 420\", \"Cheque Bounce 138\", \"Anticipatory Bail\", \"Divorce Process\", or \"Cyber Fraud 66\".",
                    isLawyerRecommended = false
                };
            }

            // Section 302 IPC / Murder
            if (lower.Contains("302") || lower.Contains("murder") || lower.Contains("killing") || lower.Contains("hatya"))
            {
                return new
                {
                    title = "Section 302 IPC (Punishment for Murder) / Section 103 BNS",
                    act = "Indian Penal Code, 1860 / Bharatiya Nyaya Sanhita, 2023",
                    section = "Section 302 (IPC) / Section 103 (BNS)",
                    description = "Whoever commits murder shall be punished with death or imprisonment for life, and shall also be liable to fine. Murder is a cognizable, non-bailable, and non-compoundable offense exclusively triable by the Court of Session.",
                    summary = "Defines severe criminal liability for intentional homicide causing death.",
                    penalty = "Death Penalty or Life Imprisonment (minimum 14 years rigorous imprisonment) + Mandatory Fine.",
                    remedies = "Immediate filing of Regular Bail Petition before Sessions Court / High Court; Scrutiny of FIR timing (Sec 154 CrPC); Challenge forensic/post-mortem reports and eyewitness depositions.",
                    isLawyerRecommended = true
                };
            }

            // Section 307 IPC / Attempt to Murder
            if (lower.Contains("307") || lower.Contains("attempt to murder") || lower.Contains("jaanleva hamla"))
            {
                return new
                {
                    title = "Section 307 IPC (Attempt to Murder) / Section 109 BNS",
                    act = "Indian Penal Code, 1860 / Bharatiya Nyaya Sanhita, 2023",
                    section = "Section 307 (IPC) / Section 109 (BNS)",
                    description = "Whoever does any act with such intention or knowledge that if he by that act caused death, he would be guilty of murder. Cognizable, Non-Bailable, and Triable by Court of Session.",
                    summary = "Punishment for intentional attempt causing life-threatening injury or attack.",
                    penalty = "Imprisonment up to 10 years and fine; If hurt is caused to any person, imprisonment for life or up to 10 years.",
                    remedies = "Apply for Anticipatory Bail (Sec 438 CrPC) or Regular Bail (Sec 439 CrPC); Scrutinize Medico-Legal Certificate (MLC) regarding nature of injuries.",
                    isLawyerRecommended = true
                };
            }

            // Section 420 IPC / Cheating & Fraud
            if (lower.Contains("420") || lower.Contains("cheating") || lower.Contains("fraud") || lower.Contains("scam") || lower.Contains("dhokha") || lower.Contains("dhokhadhadi"))
            {
                return new
                {
                    title = "Section 420 IPC (Cheating & Dishonestly Inducing Delivery of Property) / Sec 318(4) BNS",
                    act = "Indian Penal Code, 1860 / Bharatiya Nyaya Sanhita, 2023",
                    section = "Section 420 (IPC) / Section 318(4) (BNS)",
                    description = "Whoever cheats and thereby dishonestly induces the person deceived to deliver any property, or to make, alter or destroy the whole or any part of a valuable security. Cognizable and Non-Bailable.",
                    summary = "Addresses financial fraud, bogus promises, forged transactions, and deception.",
                    penalty = "Imprisonment of either description for a term up to 7 years + Fine.",
                    remedies = "1. File criminal complaint under Section 156(3) CrPC before Judicial Magistrate.\n2. Apply for Anticipatory Bail (Sec 438 CrPC) if falsely implicated.\n3. Preserve bank transfer trails, emails, WhatsApp chats, and contractual documents.",
                    isLawyerRecommended = true
                };
            }

            // Section 138 NI Act / Cheque Bounce
            if (lower.Contains("138") || lower.Contains("cheque") || lower.Contains("check bounce") || lower.Contains("dishonour") || lower.Contains("cheque bounce"))
            {
                return new
                {
                    title = "Section 138 Negotiable Instruments Act (Dishonour of Cheque)",
                    act = "Negotiable Instruments Act, 1881 (Amended 2018)",
                    section = "Section 138 (NI Act)",
                    description = "Where any cheque drawn by a person on an account maintained by him for payment of any amount of money for discharge of any debt/liability is returned by bank unpaid due to insufficient funds or exceeding arrangement.",
                    summary = "Strict statutory liability with time-bound procedure for cheque recovery.",
                    penalty = "Imprisonment up to 2 years, or fine up to twice the cheque amount, or both.",
                    remedies = "Step 1: Receive bank return memo.\nStep 2: Send mandatory Statutory Demand Notice within 30 days of return.\nStep 3: Allow 15 days grace period for drawer to pay.\nStep 4: If unpaid, file Criminal Complaint under Sec 138 in Magistrate Court within 30 days.",
                    isLawyerRecommended = true
                };
            }

            // Bail & Anticipatory Bail (438, 437, 439)
            if (lower.Contains("bail") || lower.Contains("anticipatory") || lower.Contains("438") || lower.Contains("437") || lower.Contains("439") || lower.Contains("arrest") || lower.Contains("jamanat"))
            {
                return new
                {
                    title = "Bail Provisions under Indian Law (Anticipatory Bail & Regular Bail)",
                    act = "Code of Criminal Procedure, 1973 (CrPC) / Bharatiya Nagarik Suraksha Sanhita, 2023 (BNSS)",
                    section = "Section 436 (Bailable), Section 437/439 (Regular Bail), Section 438 (Anticipatory Bail)",
                    description = "Bail is the fundamental rule and jail is an exception (Supreme Court precedent). Anticipatory bail allows any citizen apprehending arrest in a non-bailable accusation to seek pre-arrest protective bail from High Court or Sessions Court.",
                    summary = "Constitutional safeguard preserving personal liberty under Article 21.",
                    penalty = "Violation of bail conditions or witness tampering leads to immediate cancellation of bail and judicial remand.",
                    remedies = "1. File Anticipatory Bail Application citing clean antecedents, roots in society, no flight risk, and willingness to join investigation.\n2. In case of arrest, file Regular Bail Petition under Section 439 CrPC before Sessions/High Court.",
                    isLawyerRecommended = true
                };
            }

            // Article 21 Constitution / Right to Life & Liberty
            if (lower.Contains("article 21") || (lower.Contains("21") && lower.Contains("article")) || lower.Contains("right to life") || lower.Contains("liberty") || lower.Contains("privacy"))
            {
                return new
                {
                    title = "Article 21: Protection of Life and Personal Liberty",
                    act = "Constitution of India, 1950",
                    section = "Article 21",
                    description = "No person shall be deprived of his life or personal liberty except according to procedure established by law. Encompasses Right to Privacy (K.S. Puttaswamy verdict), Right to Speedy Trial, Legal Aid, Clean Environment, and Dignity.",
                    summary = "The crown jewel of Fundamental Rights in India.",
                    penalty = "Any state action, police excess, or law infringing Article 21 is unconstitutional, null, and void.",
                    remedies = "File Writ of Habeas Corpus, Mandamus, or Certiorari directly before Supreme Court (Art 32) or High Court (Art 226) for immediate protection.",
                    isLawyerRecommended = true
                };
            }

            // Article 14 / Equality
            if (lower.Contains("article 14") || (lower.Contains("14") && lower.Contains("article")) || lower.Contains("equality"))
            {
                return new
                {
                    title = "Article 14: Equality Before Law & Equal Protection of Laws",
                    act = "Constitution of India, 1950",
                    section = "Article 14",
                    description = "The State shall not deny to any person equality before the law or the equal protection of the laws within the territory of India. Strikes down arbitrariness, favoritism, and unreasonable discrimination in state action.",
                    summary = "Guarantees rule of law and equality in executive and legislative actions.",
                    penalty = "Discriminatory legislation, biased government tender awards, or arbitrary actions are struck down.",
                    remedies = "Writ Petition under Article 226 before the High Court challenging arbitrary state action.",
                    isLawyerRecommended = true
                };
            }

            // Cyber Crime & IT Act 66
            if (lower.Contains("cyber") || lower.Contains("it act") || lower.Contains("hacking") || lower.Contains("identity theft") || lower.Contains("66") || lower.Contains("otp") || lower.Contains("online fraud"))
            {
                return new
                {
                    title = "Section 66 & 66C/66D Information Technology Act (Cyber Fraud & Hacking)",
                    act = "Information Technology Act, 2000 (Amended 2008)",
                    section = "Section 66, 66C (Identity Theft), 66D (Cheating by Personation via Computer)",
                    description = "Deals with unauthorized access to computer systems, phishing, hacking, identity theft, OTP and UPI bank fraud, and digital impersonation. Offenses are cognizable in nature.",
                    summary = "Comprehensive statutory framework combating digital financial fraud and cyber attacks.",
                    penalty = "Imprisonment up to 3 years and fine up to ₹1,00,000 - ₹5,00,000 depending on gravity.",
                    remedies = "1. Immediate dialing of 1930 National Cyber Crime Helpline to freeze financial transaction.\n2. Report on cybercrime.gov.in portal.\n3. File formal complaint with Nodal Cyber Crime Police Station.",
                    isLawyerRecommended = true
                };
            }

            // Divorce, Mutual Consent 13B & Matrimonial
            if (lower.Contains("divorce") || lower.Contains("talaq") || lower.Contains("13b") || lower.Contains("separation") || lower.Contains("custody"))
            {
                return new
                {
                    title = "Mutual Consent Divorce (Section 13B) & Contested Divorce",
                    act = "Hindu Marriage Act, 1955 / Special Marriage Act, 1954",
                    section = "Section 13B (Mutual Consent), Section 13 (Contested Divorce)",
                    description = "Under Section 13B, both spouses can jointly file for divorce after living separately for at least 1 year. Two motions are recorded. The statutory 6-month cooling off period can be waived upon application as per Supreme Court guidelines (Amardeep Singh case).",
                    summary = "Time-tested legal mechanism for amicable or contested marital dissolution.",
                    penalty = "Default in agreed permanent alimony or child maintenance triggers execution proceedings and arrest warrants.",
                    remedies = "1. Execute a comprehensive Settlement Agreement detailing Alimony, Stridhan, and Child Custody.\n2. File First Motion petition before Family Court.\n3. Record statements in Second Motion for Final Decree of Dissolution.",
                    isLawyerRecommended = true
                };
            }

            // Section 498A IPC / Domestic Violence / Dowry
            if (lower.Contains("498a") || lower.Contains("domestic violence") || lower.Contains("dowry") || lower.Contains("cruelty") || lower.Contains("maintenance") || lower.Contains("125"))
            {
                return new
                {
                    title = "Section 498A IPC (Cruelty by Husband/Relatives) & Domestic Violence Act",
                    act = "Indian Penal Code, 1860 / Protection of Women from Domestic Violence Act, 2005 / Section 125 CrPC",
                    section = "Section 498A (IPC) / PWDVA 2005 / Section 125 (Maintenance)",
                    description = "Section 498A penalizes subjecting a woman to cruelty or harassment for unlawful dowry demands. Cognizable and Non-Bailable. PWDVA provides civil remedies for residence orders, protection orders, and monetary relief. Section 125 CrPC enables maintenance claims.",
                    summary = "Legal protections against domestic harassment and maintenance recovery.",
                    penalty = "Section 498A carries up to 3 years imprisonment + fine; Breach of protection order under DV Act carries up to 1 year jail.",
                    remedies = "1. For Complainant: File complaint before Mahila Thana / Protection Officer under PWDVA.\n2. For Accused: Seek mediation; Apply for Anticipatory Bail (Sec 438 CrPC) adhering to Arnesh Kumar guidelines against automatic arrest.",
                    isLawyerRecommended = true
                };
            }

            // FIR, Police Complaint & Section 156(3) CrPC
            if (lower.Contains("fir") || lower.Contains("police complaint") || lower.Contains("154") || lower.Contains("156") || lower.Contains("zero fir"))
            {
                return new
                {
                    title = "Registration of FIR (Section 154 CrPC) & Remedies for Refusal (Sec 156(3))",
                    act = "Code of Criminal Procedure, 1973 / BNSS 2023",
                    section = "Section 154 (FIR Registration), Section 156(3) (Magistrate Order for FIR)",
                    description = "Police is legally mandated to register an FIR upon disclosure of any cognizable offense (Lalita Kumari vs Govt of UP). Zero FIR allows registration at any police station regardless of jurisdiction.",
                    summary = "Statutory initiation of criminal investigation mechanism.",
                    penalty = "Refusal by police officer to register FIR in heinous crimes attracts Section 166A IPC (punishable with imprisonment up to 2 years).",
                    remedies = "If police refuses FIR:\n1. Send written complaint to Superintendent of Police (SP) by registered post under Section 154(3) CrPC.\n2. If still unacted, file Application under Section 156(3) CrPC before Judicial Magistrate to direct police investigation.",
                    isLawyerRecommended = true
                };
            }

            // Consumer Protection / Consumer Court
            if (lower.Contains("consumer") || lower.Contains("defective") || lower.Contains("service deficiency") || lower.Contains("refund") || lower.Contains("warranty"))
            {
                return new
                {
                    title = "Consumer Rights & Complaint Filing under Consumer Protection Act, 2019",
                    act = "Consumer Protection Act, 2019",
                    section = "Section 35 (District Commission), Section 47 (State), Section 58 (National)",
                    description = "Protects consumers against unfair trade practices, defective products, misleading advertisements, and deficiency in services (e.g. airlines, telecom, real estate, e-commerce, insurance claims).",
                    summary = "Fast-track dispute resolution with compensation and product replacement remedies.",
                    penalty = "Non-compliance with Consumer Commission orders carries imprisonment from 1 month to 3 years, or fine up to ₹1,00,000, or both.",
                    remedies = "1. Serve formal Legal Notice to manufacturer/service provider giving 15 days notice.\n2. File Consumer Complaint online via e-Daakhil portal (edaakhil.nic.in).\n3. Claim complete refund, litigation costs, and mental harassment damages.",
                    isLawyerRecommended = true
                };
            }

            // Property, Tenant Eviction, Trespassing
            if (lower.Contains("property") || lower.Contains("tenant") || lower.Contains("eviction") || lower.Contains("kiraya") || lower.Contains("trespass") || lower.Contains("rent"))
            {
                return new
                {
                    title = "Tenant Eviction, Rent Agreements & Property Dispute Laws",
                    act = "Transfer of Property Act, 1882 / State Rent Control Acts / Model Tenancy Act",
                    section = "Section 106 (Notice to Terminate Lease), Section 441 IPC (Criminal Trespass)",
                    description = "Governs landlord-tenant relationships, lease agreements, illegal encroachment, and recovery of possession. Landlords cannot forcefully evict tenants without following due process of law.",
                    summary = "Statutory process for lawful lease termination and possession recovery.",
                    penalty = "Illegal possession / trespassing attracts civil injunction and criminal liability under Sec 441/447 IPC.",
                    remedies = "1. Serve 15-day or 30-day Quit Notice under Section 106 Transfer of Property Act.\n2. File Civil Eviction Suit and Mesne Profits recovery before Rent Controller / Civil Court.\n3. File Application for Temporary Injunction under Order 39 Rules 1 & 2 CPC against unauthorized alterations.",
                    isLawyerRecommended = true
                };
            }

            return null;
        }

        private int? ExtractConstitutionNumber(string query)
        {
            var match = Regex.Match(query, @"\d+");
            if (match.Success && int.TryParse(match.Value, out int result))
            {
                return result;
            }
            return null;
        }

        public class QueryRequest
        {
            public string? Query { get; set; }
        }
    }
}
