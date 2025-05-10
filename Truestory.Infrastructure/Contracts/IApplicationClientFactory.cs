using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Truestory.Domain.Models;

namespace Truestory.Infrastructure.Contracts
{
    public interface IApplicationClientFactory
    {

        [Post("/objects")]
        Task<AddObjectResponse> AddObject(AddDevice request);

        [Put("/objects")]
        Task<AddObjectResponse> UpdateObjects(string Id, UpdateDevice data);

        [Delete("/objects")]
        Task<string> DeleteObjects(string Id);

        [Get("/objects")]
        Task<IEnumerable<ListObjectResponse>> ListObjects();

        [Get("/objects")]
        Task<IEnumerable<ListObjectResponse>> ListObjectsByIds(string[] Id);

    }
}
