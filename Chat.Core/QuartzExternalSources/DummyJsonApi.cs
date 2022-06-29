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
    public class DummyJsonApi : IUserApi
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMapper _mapper;
        private readonly string _dummyApi;

        public DummyJsonApi(IMapper mapper, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _dummyApi = configuration.GetValue<string>("Dummy");
            _mapper = mapper;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IEnumerable<UserModel>> SendRequest()
        {
            var httpClient = _httpClientFactory.CreateClient();

            var response = await httpClient.GetAsync(_dummyApi);

            response.EnsureSuccessStatusCode();

            var users = await response.Content.ReadFromJsonAsync<Users>();

            var result = users.users.Select(x => _mapper.Map<UserModel>(x));

            return result;
        }
    }
}