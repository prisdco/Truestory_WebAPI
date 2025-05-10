using MediatR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Truestory.Domain.Models;
using Truestory.Infrastructure.Contracts;

namespace Truestory.Infrastructure.Factories
{
    public class ApplicationClientFactory : IApplicationClientFactory
    {
        private HttpClient Client { get; }


        public ApplicationClientFactory(HttpClient client, IConfiguration configuration)
        {
            client.BaseAddress = new Uri(configuration.GetValue<string>("IDENTITY_SERVER_BASE_ADDRESS"));
            Client = client;
        }

        public async Task<AddObjectResponse> AddObject(AddDevice request)
        {
            var requestContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var response = await Client.PostAsync("objects", requestContent);

            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Save object failed with status code {response.StatusCode}: {content}");
            }

            var createdObject = JsonConvert.DeserializeObject<AddObjectResponse>(content);

            return createdObject;
        }

        public async Task<AddObjectResponse> UpdateObjects(string Id, UpdateDevice data)
        {
            var requestContent = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var url = $"objects/{Id}";

            var response = await Client.PutAsync(url, requestContent);

            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Update failed with status code {response.StatusCode}: {content}");
            }
           
            var createdObject = JsonConvert.DeserializeObject<AddObjectResponse>(content);

            return createdObject;
        }

        public async Task<string> DeleteObjects(string Id)
        {
            var url = $"objects/{Id}";
            var response = await Client.DeleteAsync(url);

            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
                return content;

            return "Deleted successfully";
        }

        public async Task<IEnumerable<ListObjectResponse>> ListObjects()
        {
            using var response = await Client.GetAsync("objects", HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var createdObject = JsonConvert.DeserializeObject<List<ListObjectResponse>>(content);

            return createdObject;
        }

        public async Task<IEnumerable<ListObjectResponse>> ListObjectsByIds(string[] ids)
        {
            var query = string.Join("&", ids.Select(id => $"id={id}"));
            var url = $"objects?{query}";

            var response = await Client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var createdObject = JsonConvert.DeserializeObject<List<ListObjectResponse>>(content);

            return createdObject;
        }
    }
}
