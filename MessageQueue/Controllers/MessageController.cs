using MessageQueue.Models;
using MessageQueue.Storage;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessageQueue.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IQueueManager _queueManager;
        private ILogger<MessageController> _logger;

        public MessageController(IQueueManager queueManager, ILogger<MessageController> logger)
        {
            _queueManager = queueManager;
            _logger = logger;
        }

        [HttpPost("Publish")]
        public IActionResult AddMessage([FromBody] Message message)
        {
            try
            {
                var messageId = _queueManager.AddMessage(message);

                return Ok(new { messageId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return BadRequest($"Error while adding message {message.Name}");
            }
        }

        [HttpGet]
        public IActionResult RetrieveMessage([FromQuery] Guid messageId)
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
