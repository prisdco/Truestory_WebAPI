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

namespace Truestory.Application.UseCase.Commands
{
    public class DeleteObjectCommand : IRequest<ResultViewModel>
    {
        private string Id { get; set; }
        public DeleteObjectCommand (string Id) => this.Id = Id;
        public class DeleteObjectCommandHandler : IRequestHandler<DeleteObjectCommand, ResultViewModel>
        {
            private readonly ILogger<DeleteObjectCommandHandler> _logger;
            private readonly IApplicationClientFactory clientFactory;
            private readonly IMemoryCache _cache;
            private readonly IPolicyService _policyService;
            public DeleteObjectCommandHandler(ILogger<DeleteObjectCommandHandler> logger,
                IApplicationClientFactory clientFactory, IMemoryCache cache, IPolicyService policyService)
            {
                _logger = logger;
                this.clientFactory = clientFactory;
                this._cache = cache;
                _policyService = policyService;
            }

            public async Task<ResultViewModel> Handle(DeleteObjectCommand request, CancellationToken cancellationToken)
            {
                _logger.LogInformation("Start Deleting Object.");

                try
                {
                    var _Response = await _policyService.ExecuteWithRetryAsync(() => clientFactory.DeleteObjects(request.Id));

                    if (_Response != string.Empty)
                    {
                        _cache.Remove("ListObjects");
                        _cache.Remove($"ListObjectsByIds:{request.Id}");
                        return ResultViewModel.Ok(_Response);
                    }
                    else
                        return ResultViewModel.Fail("Object Data Failed To Delete.");
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
