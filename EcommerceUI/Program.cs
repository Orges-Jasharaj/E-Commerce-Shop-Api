using Blazored.LocalStorage;
using EcommerceUI.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace EcommerceUI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {

            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddScoped<AuthTokenHandler>();
            builder.Services.AddScoped<CustomAuthStateProvider>();
            builder.Services.AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<CustomAuthStateProvider>());
            builder.Services.AddScoped<AuthServices>();
            builder.Services.AddAuthorizationCore();


            builder.Services.AddHttpClient("AuthAPI", client =>
            {
                client.BaseAddress = new Uri("https://localhost:7192/api/Auth/"); // Replace with your API base address
            }).AddHttpMessageHandler<AuthTokenHandler>();

            builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("AuthAPI"));


            await builder.Build().RunAsync();
        }
    }
}
