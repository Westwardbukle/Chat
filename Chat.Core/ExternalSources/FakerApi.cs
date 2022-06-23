using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using AutoMapper;
using Chat.Common.Dto.User;
using Chat.Core.ExternalSources.Abstract;
using Chat.Core.ExternalSources.Dto;
using Chat.Database.Model;

namespace Chat.Core.ExternalSources
{
    public class FakerApi : IUserApi
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMapper _mapper;

        public FakerApi(IMapper mapper, IHttpClientFactory httpClientFactory)
        {
            _mapper = mapper;
            _httpClientFactory = httpClientFactory;
        }
    
        public async Task<List<UserModel>> SendRequest( )
        {
            var httpClient = _httpClientFactory.CreateClient();
            
            var response = await httpClient.GetAsync("https://fakerapi.it/api/v1/users?_quantity=100");

            response.EnsureSuccessStatusCode();

            var users = await response.Content.ReadFromJsonAsync<FakerApiResponse>();
            
            var result = users.Data.Select(x => _mapper.Map<UserModel>(x)).ToList();
            
            return result;
        }
    }
}