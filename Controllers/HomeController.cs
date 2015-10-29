using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using sentimentR_frontend.Models;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace sentimentR_frontend.Controllers
{
    public class HomeController : Controller
    {
        public async Task<ActionResult> Index(string Title = "#welc2017", bool ShowScore = true, bool ShowDots = true, int RefreshFrequency = 10, string AzureEventHubName = "welcome2017output-eh")
        {
            string[] bgColors =
            {
                "rgba(255, 0, 0, 0.5)",
                "rgba(230, 25, 22, 0.5)",
                "rgba(180, 50, 25, 0.5)",
                "rgba(160, 80, 25, 0.5)",
                "rgba(140, 110, 25, 0.5)",
                "rgba(110, 140, 25, 0.5)",
                "rgba(80, 160, 25, 0.5)",
                "rgba(50, 180, 25, 0.5)",
                "rgba(25, 230, 25, 0.5)",
                "rgba(0, 255, 0, 0.5)"
            };

            string[] boxColors =
            {
                "red",
                "red",
                "red",
                "orange",
                "orange",
                "orange",
                "orange",
                "green",
                "green",
                "green"
            };
            
            int score = new Random().Next(25, 30); //generate a random default score between 25 and 30. This way we know that if it stays at that level, something has gone wrong but the demo will still spear to work.

            //get the score from the API
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://reasonsapi.azurewebsites.net");
                var response = await client.GetAsync(client.BaseAddress + "/api/sentimentdata/" + AzureEventHubName);

                if (response.IsSuccessStatusCode)
                {
                    //only update the score if we have a success code, otherwise keep the default score of 25-30.
                    var responseString = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<SentimentData>(responseString);
                    score = (result == null) ?
                        score :
                        result.AverageSentiment;
                }

            }

            //analyse score and setup orb colours based on it
            int scoreDiv10 = score / 10;

            //set anything above 9 to 9 so we do not get outside the bounds of the colour arrays
            if (scoreDiv10 > 9) scoreDiv10 = 9;

            //create view model
            var viewmodel = new homeViewModels()
            {
                Background = bgColors[scoreDiv10],
                Box = boxColors[scoreDiv10],
                Score = score,
                Title = Title,
                ShowScore = ShowScore,
                RefreshFrequency = RefreshFrequency,
                ShowDots = ShowDots
            };

            return View(viewmodel);
        }

    }
}