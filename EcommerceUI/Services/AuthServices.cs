using E_Commerce_Shop_Api.Dtos.Requests;
using E_Commerce_Shop_Api.Dtos.Responses;
using System.Net.Http.Json;

namespace EcommerceUI.Services
{
    public class AuthServices
    {

        private readonly IHttpClientFactory _httpClientFactory;


        public AuthServices(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ResponseDto<LoginResponseDto>> LoginAsync(LoginDto loginDto)
        {

            try
            {
                var client = _httpClientFactory.CreateClient("AuthAPI");
                var response = await client.PostAsJsonAsync("Login", loginDto);
                response.EnsureSuccessStatusCode();

                var loginResponse = await response.Content.ReadFromJsonAsync<ResponseDto<LoginResponseDto>>();

                return loginResponse;
            }
            catch(Exception ex)
            {
                throw;
            }

        }


    }
}
