using LT.Data;
using LT.Model.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
                return BadRequest("Query cannot be empty.");
            }

            // Extract the constitution number from the query
            var constitutionNumber = ExtractConstitutionNumber(request.Query);
            if (!constitutionNumber.HasValue)
            {
                return BadRequest("Invalid query. Please provide a valid constitution number.");
            }

            // Fetch description from the database using the extracted constitution number
            var description = await _context.Constitution
                .Where(c => c.ConstitutionNumber == request.Query)
                .Select(c => c.ConstitutionDetails)
                .FirstOrDefaultAsync();

            if (string.IsNullOrWhiteSpace(description))
            {
                return Ok(new { description = "I could not find the details for the given Constitution Number." });
            }

            return Ok(new { description });
        }



        private int? ExtractConstitutionNumber(string query)
        {
            // Extract the first number found in the query (assuming it represents the constitution number)
            var match = Regex.Match(query, @"\d+");
            if (match.Success && int.TryParse(match.Value, out int result))
            {
                return result;
            }
            return null;
        }

        public class QueryRequest
        {
            public string Query { get; set; }
        }
    }
}
