using AutoMapper;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Truestory.Domain.Models;
using Truestory.Infrastructure.Contracts;
using Truestory.Infrastructure.Interface;
using Truestory.Infrastructure.Service;

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
            private readonly IMemoryCache _cache;
            private readonly IPolicyService _policyService;

            public CreateObjectHandler(ILogger<CreateObjectHandler> logger, IMemoryCache cache, 
                IApplicationClientFactory clientFactory, IPolicyService policyService)
            {
                _logger = logger;
                this.clientFactory = clientFactory;
                this._cache = cache;
                _policyService = policyService;
            }

            public async Task<ResultViewModel> Handle(CreateObjectCommand request, CancellationToken cancellationToken)
            {
                _logger.LogInformation("Creating object with name: {Name}", JsonConvert.SerializeObject(request));

              
                try
                {   // create program
                    var requestObject = new AddDevice { name = request.name, data = request.data };

                    var _Response = await _policyService.ExecuteWithRetryAsync(() => clientFactory.AddObject(requestObject));
                   
                    if (_Response.name != string.Empty)
                    {
                        _cache.Remove("ListObjects");
                        return ResultViewModel.Ok(_Response);
                    }
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
