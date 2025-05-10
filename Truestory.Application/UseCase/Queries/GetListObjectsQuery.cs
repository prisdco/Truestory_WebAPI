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
    public class GetListObjectsQuery : IRequest<Result<PaginatedResult<ListObjectResponse>>>, ICachableRequest
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string CacheKey => $"ListObjects_Page_{PageNumber}_Size_{PageSize}";
        public TimeSpan? AbsoluteExpiration => TimeSpan.FromMinutes(5);
        public class GetListObjectsQueryHandler : IRequestHandler<GetListObjectsQuery, Result<PaginatedResult<ListObjectResponse>>>
        {
            private readonly IApplicationClientFactory clientFactory;
            private readonly IHttpContextAccessor httpContextAccessor;
            //private readonly IMapper _mapper;
            public GetListObjectsQueryHandler(IApplicationClientFactory clientFactory)
            {
                this.clientFactory = clientFactory;
                //_mapper = mapper;
            }

            public async Task<Result<PaginatedResult<ListObjectResponse>>> Handle(GetListObjectsQuery request, CancellationToken cancellationToken)
            {
                var result = await clientFactory.ListObjects();

                var totalCount = result.Count();

                var pagedItems = result
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToList();

                var paginatedResult = new PaginatedResult<ListObjectResponse>(
                    pagedItems,
                    totalCount,
                    request.PageNumber,
                    request.PageSize
                );

                return ResultViewModel.Ok(paginatedResult);
            }
        }
    }

}
