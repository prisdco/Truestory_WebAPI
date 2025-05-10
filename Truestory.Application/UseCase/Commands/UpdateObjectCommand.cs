using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Truestory.Domain.Models;
using Truestory.Infrastructure.Contracts;

namespace Truestory.Application.UseCase.Commands
{

    public class UpdateObjectCommand : IRequest<ResultViewModel>
    {
        private string Id { get; set; }
        private UpdateDevice data { get; set; }
        public UpdateObjectCommand(string Id, UpdateDevice data)
        {
            this.Id = Id;
            this.data = data;
        }

        public class UpdateObjectCommandHandler : IRequestHandler<UpdateObjectCommand, ResultViewModel>
        {
            private readonly ILogger<UpdateObjectCommandHandler> _logger;
            private readonly IApplicationClientFactory clientFactory;
            private readonly IMapper mapper;

            public UpdateObjectCommandHandler(ILogger<UpdateObjectCommandHandler> logger, IMapper mapper, IApplicationClientFactory clientFactory)
            {
                _logger = logger;
                this.clientFactory = clientFactory;
                this.mapper = mapper;
            }

            public async Task<ResultViewModel> Handle(UpdateObjectCommand request, CancellationToken cancellationToken)
            {
                _logger.LogInformation("Start Updating Object.");

                try
                {   
                    var _Response = await clientFactory.UpdateObjects(request.Id, request.data);
                    if (_Response.name != string.Empty && _Response.name is not null)
                        return ResultViewModel.Ok(_Response);
                    else
                        return ResultViewModel.Fail("Object Data Failed To Update.");
                }
                catch (Exception ex)
                {
                    _logger.LogInformation(ex.Message);
                    return ResultViewModel.Fail(ex.Message);
                }
            }
        }
    }
}
