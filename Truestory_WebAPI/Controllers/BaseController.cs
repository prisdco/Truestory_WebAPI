using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Truestory.Application.UseCase.Validations;
using Truestory.Domain.Models;

namespace Truestory_WebAPI.Controllers
{
    [ApiController]

    public class BaseController : ControllerBase
    {
        protected readonly IMediator Mediator;
        protected readonly ILogger<BaseController> Logger;
        protected readonly IConfiguration? Config;

        public BaseController(IMediator _Mediator, ILogger<BaseController> _Logger, IConfiguration _Config)
        {
            Mediator = _Mediator;
            Logger = _Logger;
            Config = _Config;
        }



    }
}
