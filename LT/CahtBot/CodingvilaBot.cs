using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;

namespace LT.CahtBot
{
    public class CodingvilaBot : IBot
    {
        public async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default)
        {
            if(turnContext.Activity.Type == ActivityTypes.Message)
            {
                string userInput = turnContext.Activity.Text;
                string response = ProcessInput(userInput);
                await turnContext.SendActivityAsync(MessageFactory.Text(response), cancellationToken);
            }
        }

        private string ProcessInput(string input)
        {
            if (input.Contains("Hello"))
            {
                return "Hello! Welcome to the Legal Tech Solution, How can I assist you today?";
            }
            else if (input.Contains("help"))
            {
                return "Here are some things you can ask me...";
            }
            else
            {
                return "I'm not sure how to help with that, but I'm learning more every day!";
            }
        }
    }
}
