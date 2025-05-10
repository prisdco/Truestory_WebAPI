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
    public class DeleteObjectCommand : IRequest<ResultViewModel>
    {
        private string Id { get; set; }
        public DeleteObjectCommand (string Id) => this.Id = Id;
        public class DeleteObjectCommandHandler : IRequestHandler<DeleteObjectCommand, ResultViewModel>
        {
            private readonly ILogger<DeleteObjectCommandHandler> _logger;
            private readonly IApplicationClientFactory clientFactory;
            private readonly IMapper mapper;

            public DeleteObjectCommandHandler(ILogger<DeleteObjectCommandHandler> logger, IMapper mapper, IApplicationClientFactory clientFactory)
            {
                _logger = logger;
                this.clientFactory = clientFactory;
                this.mapper = mapper;
            }

            public async Task<ResultViewModel> Handle(DeleteObjectCommand request, CancellationToken cancellationToken)
            {
                _logger.LogInformation("Start Deleting Object.");

                try
                {
                    var _Response = await clientFactory.DeleteObjects(request.Id);
                    if (_Response != string.Empty)
                        return ResultViewModel.Ok(_Response);
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
