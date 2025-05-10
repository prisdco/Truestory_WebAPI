using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Truestory.Domain.Models;
using Truestory.Infrastructure.Contracts;
using Truestory.Infrastructure.Interface;

namespace Truestory.Application.UseCase.Queries
{
    public class GetListObjectsByIdsQuery : IRequest<Result<IEnumerable<ListObjectResponse>>>, ICachableRequest
    {
        private string[] Id { get; set; }
             
        public GetListObjectsByIdsQuery(string[] id) => Id = id;
        public string CacheKey => $"ListObjectsByIds:{string.Join(",", Id.OrderBy(x => x))}";

        public TimeSpan? AbsoluteExpiration => TimeSpan.FromMinutes(5);
        public class GetListObjectsByIdsQueryHandler : IRequestHandler<GetListObjectsByIdsQuery, Result<IEnumerable<ListObjectResponse>>>
        {
            private readonly IApplicationClientFactory clientFactory;
            private readonly IHttpContextAccessor httpContextAccessor;
            //private readonly IMapper _mapper;
            public GetListObjectsByIdsQueryHandler(IApplicationClientFactory clientFactory)
            {
                this.clientFactory = clientFactory;
            }

            public async Task<Result<IEnumerable<ListObjectResponse>>> Handle(GetListObjectsByIdsQuery request, CancellationToken cancellationToken)
            {
                var result = await clientFactory.ListObjectsByIds(request.Id);


                return ResultViewModel.Ok(result);
            }
        }
    }
}
