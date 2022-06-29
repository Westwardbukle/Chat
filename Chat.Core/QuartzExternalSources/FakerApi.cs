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
    public class FakerApi :IUserApi
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMapper _mapper;
        private readonly string _fakerApi;

        public FakerApi(IMapper mapper, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _fakerApi = configuration.GetValue<string>("Faker");
            _mapper = mapper;
            _httpClientFactory = httpClientFactory;
        }
    
        public async Task<IEnumerable<UserModel>> SendRequest( )
        {
            var httpClient = _httpClientFactory.CreateClient();
            
            var response = await httpClient.GetAsync(_fakerApi);

            response.EnsureSuccessStatusCode();

            var users = await response.Content.ReadFromJsonAsync<FakerApiResponse>();
            
            var result = users.Data.Select(x => _mapper.Map<UserModel>(x));
            
            return result;
        }
    }
}