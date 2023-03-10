using MessageQueue.Models;
using MessageQueue.Storage;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MessageQueue.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IQueueManager _queueManager;
        private readonly ILogger<MessageController> _logger;

        public MessageController(IQueueManager queueManager, ILogger<MessageController> logger)
        {
            _queueManager = queueManager;
            _logger = logger;
        }

        [HttpPost("Publish")]
        public async Task<IActionResult> AddMessage([FromBody] Message message)
        {
            try
            {
                var messageId = _queueManager.AddMessage(message);
                var compressServiceUrl =  "https://localhost:44305/";

                var client = new HttpClient();
                client.DefaultRequestHeaders.Add("compressReady", messageId.ToString());
                client.GetAsync(compressServiceUrl);


                return Ok(new { messageId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return BadRequest($"Error while adding message {message.Name}");
            }
        }

        [HttpPost("PutBack")]
        public async Task<IActionResult> ReplaceMessage([FromBody] Message message)
        {
            try
            {
                _queueManager.ReplaceMessage(message);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return BadRequest($"Error while replacing message {message.Name}");
            }
        }

        [HttpGet("ByName")]
        public IActionResult RetrieveMessageByName([FromQuery] string videoName)
        {
            Message result = null;
            try
            {
                result = _queueManager.GetMessage(videoName.Replace("\"", ""));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return BadRequest($"Error retrieving message with id {videoName}");
            }

            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return NotFound($"Video with id {videoName} was not found");
            }
        }

        [HttpGet]
        public IActionResult RetrieveMessageById([FromQuery] Guid messageId)
        {
            Message result = null;
            try
            {
                result = _queueManager.GetMessage(messageId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return BadRequest($"Error retrieving message with id {messageId}");
            }

            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return NotFound($"Video with id {messageId} was not found");
            }
        }
    }
}
