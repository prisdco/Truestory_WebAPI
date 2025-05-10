using AutoMapper;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Truestory.Domain.Models;
using Truestory.Infrastructure.Contracts;
using Truestory.Infrastructure.Interface;
using Truestory.Infrastructure.Service;

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
            private readonly IMemoryCache _cache;
            private readonly IPolicyService _policyService;

            public UpdateObjectCommandHandler(ILogger<UpdateObjectCommandHandler> logger, IMemoryCache cache, 
                IApplicationClientFactory clientFactory, IPolicyService policyService)
            {
                _logger = logger;
                this.clientFactory = clientFactory;
                this._cache = cache;
                _policyService = policyService;
            }

            public async Task<ResultViewModel> Handle(UpdateObjectCommand request, CancellationToken cancellationToken)
            {
                _logger.LogInformation("Start Updating Object.");

                try
                {                     
                    var _Response = await _policyService.ExecuteWithRetryAsync(() => clientFactory.UpdateObjects(request.Id, request.data));
                    if (_Response.name != string.Empty && _Response.name is not null)
                    {
                        _cache.Remove("ListObjects");
                        _cache.Remove($"ListObjectsByIds:{request.Id}");
                        return ResultViewModel.Ok(_Response);
                    }
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
