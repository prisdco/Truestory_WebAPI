using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Truestory.Domain.Models;
using Truestory.Infrastructure.Contracts;

namespace Truestory.Application.UseCase.Commands
{

    public class CreateObjectCommand : IRequest<ResultViewModel>
    {
        private string name { get; set; }
        private AddDeviceProperties data { get; set; }

        public CreateObjectCommand (string name, AddDeviceProperties data)
        {
            this.name = name;
            this.data = data;
        }

        public class CreateObjectHandler : IRequestHandler<CreateObjectCommand, ResultViewModel>
        {
            private readonly ILogger<CreateObjectHandler> _logger;
            private readonly IApplicationClientFactory clientFactory;
            private readonly IMapper mapper;

            public CreateObjectHandler(ILogger<CreateObjectHandler> logger, IMapper mapper, IApplicationClientFactory clientFactory)
            {
                _logger = logger;
                this.clientFactory = clientFactory;
                this.mapper = mapper;   
            }

            public async Task<ResultViewModel> Handle(CreateObjectCommand request, CancellationToken cancellationToken)
            {
                _logger.LogInformation("Start Saving Object.");

                try
                {   // create program
                    var requestObject = new AddDevice { name = request.name, data = request.data };
                    var _Response = await clientFactory.AddObject(requestObject);
                    if (_Response.name != string.Empty)
                        return ResultViewModel.Ok(_Response);
                    else
                        return ResultViewModel.Fail("Object Data Failed To Submit.");
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
