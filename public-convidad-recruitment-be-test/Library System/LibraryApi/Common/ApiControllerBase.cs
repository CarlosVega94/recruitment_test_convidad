using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryDatabase.Common
{
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Produces("application/json")]
    [ApiController]
    [Authorize]
    public abstract class ApiControllerBase : ControllerBase
    {
        private ISender _mediator;
        private IValidateParams _validateParams;

        protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();

        protected IValidateParams ValidateParams =>
            _validateParams ??= HttpContext.RequestServices.GetRequiredService<IValidateParams>();
    }
}
