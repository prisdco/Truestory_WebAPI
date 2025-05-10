using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Truestory.Domain.Models;
using Truestory.Infrastructure.Contracts;
using Truestory.Infrastructure.Interface;

namespace Truestory.Application.UseCase.Queries
{
    public class GetListObjectsQuery : IRequest<Result<IEnumerable<ListObjectResponse>>>, ICachableRequest
    {
        public string CacheKey => "ListObjects";
        public TimeSpan? AbsoluteExpiration => TimeSpan.FromMinutes(5);
        public class GetListObjectsQueryHandler : IRequestHandler<GetListObjectsQuery, Result<IEnumerable<ListObjectResponse>>>
        {
            private readonly IApplicationClientFactory clientFactory;
            private readonly IHttpContextAccessor httpContextAccessor;
            //private readonly IMapper _mapper;
            public GetListObjectsQueryHandler(IApplicationClientFactory clientFactory)
            {
                this.clientFactory = clientFactory;
                //_mapper = mapper;
            }

            public async Task<Result<IEnumerable<ListObjectResponse>>> Handle(GetListObjectsQuery request, CancellationToken cancellationToken)
            {
                var result = await clientFactory.ListObjects();
                            
                return ResultViewModel.Ok(result);
            }
        }
    }

}
