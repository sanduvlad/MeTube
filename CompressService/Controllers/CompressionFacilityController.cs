using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CompressService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompressionFacilityController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> CompressVideo([FromQuery] Guid videoToCompressId)
        {
            var messageQueueUrl = "https://localhost:44322/api/message";

            var client = new HttpClient();

            var response = await (await client.GetAsync($"{messageQueueUrl}?messageId={videoToCompressId}")).Content.ReadAsStringAsync();

            var videoToCompress = JsonConvert.DeserializeObject(response).ToString();

            await Task.Delay(10000);

            var compressedVideo = videoToCompress.ToLower();

            var content = new StringContent(compressedVideo, Encoding.UTF8, "application/json");
            var responseMessage = await client.PostAsync(messageQueueUrl + "/PutBack", content); // should not be awaited

            



            return Ok();
        }
    }
}
