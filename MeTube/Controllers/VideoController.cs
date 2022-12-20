using MeTube.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace MeTube.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideoController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public VideoController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        [HttpPost("Upload")]
        public async Task<IActionResult> UploadVideo([FromBody] VideoUploadRequest videoUploadRequest)
        {
            var _client = _httpClientFactory.CreateClient();
            var url = _configuration.get
                "https://localhost:44322/api/message/publish";
            var messageData = new 
            {
                Name = videoUploadRequest.Name,
                Data = videoUploadRequest.VideoData
            };
            var content = new StringContent(JsonConvert.SerializeObject(messageData), Encoding.UTF8, "application/json");
            var responseMessage = await _client.PostAsync(url, content); // should not be awaited

            var videoId = JsonConvert.DeserializeObject<VideoUploadResponse>(await responseMessage.Content.ReadAsStringAsync());
            return Ok(videoId);
        }

        [HttpGet]
        public IActionResult WatchVideo([FromQuery] Guid videoId)
        {

            return Ok(videoId);
        }
    }
}
