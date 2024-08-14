using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SATS.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SatsBaseController : ControllerBase
    {

        protected readonly ISender _mediator;

        protected SatsBaseController(ISender mediator)
        {
            _mediator = mediator;
        }
    }
}
