using MassTransit;
using Microsoft.AspNetCore.Mvc;
using SATS.Business.Consumers;

namespace SATS.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public MessagesController(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] StudentMessage message)
        {
            await _publishEndpoint.Publish(message);
            return Ok();
        }
    }
}
