using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using AutoMapper;
using Chat.Core.ExternalSources.Abstract;
using Chat.Core.ExternalSources.Dto;
using Chat.Database.Model;
using Microsoft.Extensions.Configuration;

namespace Chat.Core.QuartzExternalSources
{
    public class FakeApi : IUserApi
    {
        private readonly IMapper _mapper;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _fakeApi;

        public FakeApi(IMapper mapper, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _fakeApi = configuration.GetValue<string>("Fake");
            _mapper = mapper;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IEnumerable<UserModel>> SendRequest()
        {
            var httpClient = _httpClientFactory.CreateClient();

            var response = await httpClient.GetAsync("http://fakeapi.jsonparseronline.com/users");

            response.EnsureSuccessStatusCode();

            var users = await response.Content.ReadFromJsonAsync<List<UserFakeApi>>();

            var result = users.Select(x => _mapper.Map<UserModel>(x));

            return result;
        }
    }
}