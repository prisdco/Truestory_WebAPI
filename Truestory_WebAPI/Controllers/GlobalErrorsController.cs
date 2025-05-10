using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Truestory.Application.Extensions;
using Truestory.Infrastructure.Exceptions;

namespace Truestory_WebAPI.Controllers
{
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]

    public class GlobalErrorsController : ControllerBase
    {
        IConfiguration configuration;
        ILogger<GlobalErrorsController> _logger;

        public GlobalErrorsController(ILogger<GlobalErrorsController> logger, IConfiguration configuration)
        {
            this._logger = logger;
            this.configuration = configuration;

        }

        [Route("/error-development")]
        public IActionResult HandleError() => Problem();

        [Route("/error")]
        public IActionResult HandleErrorDevelopment([FromServices] IHostEnvironment hostEnvironment)
        {
            // if (!hostEnvironment.IsDevelopment()) return NotFound();

            var contextException = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var exceptionName = contextException.Error.GetType().Name;

            var responseStatusCode = exceptionName switch
            {
                "NullReferenceException" => HttpStatusCode.BadRequest,
                "UnauthorizedAccessException" => HttpStatusCode.Forbidden,
                "FormatException" => HttpStatusCode.BadRequest,
                "InternalServerError" => HttpStatusCode.InternalServerError,
                _ => HttpStatusCode.ServiceUnavailable,
            };

            _logger.RecursivelyLogWrtException(contextException.Error, configuration);
            _logger.LogError("++++++++++++++++++++++++++++++++++++++++++++++++++++++");



            if (exceptionName != nameof(HttpResponseException))
            {
                return Problem(detail: contextException.Error.Message, statusCode: (int)responseStatusCode);
            }
            else
            {
                var e = (HttpResponseException)contextException.Error;
                return Problem(detail: e.httpResponseMessage.ReasonPhrase, statusCode: (int)e.httpResponseMessage.StatusCode);
            }



        }
    }
}
