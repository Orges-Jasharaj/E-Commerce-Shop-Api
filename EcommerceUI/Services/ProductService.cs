using E_Commerce_Shop_Api.Dtos.Responses;
using System.Net.Http.Json;

namespace EcommerceUI.Services
{
    public class ProductService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ProductService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ResponseDto<List<ProductDto>>> GetAllProductsAsync()
        {
            try
            {
                var client = _httpClientFactory.CreateClient("ProductAPI");
                var response = await client.GetAsync("get-all-products");
                response.EnsureSuccessStatusCode();
                var products = await response.Content.ReadFromJsonAsync<ResponseDto<List<ProductDto>>>();
                return products!;
            }
            catch
            {
                throw;
            }
        }
    }
}
