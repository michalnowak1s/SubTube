using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using Microsoft.AspNetCore.Mvc;

namespace SubTube.Controllers
{
    public class YoutubeController : Controller
    {
        public ActionResult Index(string channelName)
        {
            // Replace "YOUR_API_KEY" with your actual API key obtained from the Google Cloud Console
            string apiKey = "AIzaSyBhKfhfuAjKG54sPcRMErqy0JdQvkaaSms";

            // Create a YouTubeService instance with the API key
            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = apiKey
            });

            // Define the request to search for the channel based on the provided name
            var channelSearchRequest = youtubeService.Search.List("snippet");
            channelSearchRequest.Q = channelName;
            channelSearchRequest.MaxResults = 1; // Limit the search results to 1

            // Execute the channel search request
            var channelSearchResponse = channelSearchRequest.Execute();

            if (channelSearchResponse.Items.Count > 0)
            {
                // Get the channel ID from the search response
                var channelId = channelSearchResponse.Items[0].Snippet.ChannelId;

                // Define the request to retrieve the most recent videos from the found channel
                var videoRequest = youtubeService.Search.List("snippet");
                videoRequest.ChannelId = channelId;
                videoRequest.MaxResults = 10; // Maximum number of videos to retrieve
                videoRequest.Order = SearchResource.ListRequest.OrderEnum.Date; // Sort by date

                // Execute the video request
                var videoSearchResponse = videoRequest.Execute();

                // Pass the search results to the view
                return View(videoSearchResponse.Items);
            }
            else
            {
                // Handle case when channel not found
                return Content("Channel not found");
            }
        }

    }
}
