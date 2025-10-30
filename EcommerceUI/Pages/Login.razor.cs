using Blazored.LocalStorage;
using E_Commerce_Shop_Api.Dtos.Requests;
using EcommerceUI.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.ComponentModel.DataAnnotations;

namespace EcommerceUI.Pages
{
    public partial class Login
    {
        private string? errorMessage;

        [SupplyParameterFromForm]
        private InputModel Input { get; set; } = new();

        [Inject]
        public AuthServices AuthService { get; set; } = default!;
        [Inject]
        protected IJSRuntime JSRuntime { get; set; } = default!;
        [Inject]
        private NavigationManager NavigationManager { get; set; } = default!;
        [Inject]
        private ILocalStorageService LocalStorage { get; set; } = default!;
        [Inject]
        private CustomAuthStateProvider CustomAuthStateProvider { get; set; } = default!;

        public async Task LoginUser()
        {

            var data = new LoginDto
            {
                Email = Input.Email,
                Password = Input.Password
            };

            var result = await AuthService.LoginAsync(data);
            if (result.Success)
            {
                await LocalStorage.SetItemAsync("authToken", result.Data.AccessToken);
                await LocalStorage.SetItemAsync("refreshToken", result.Data.RefreshToken);

                CustomAuthStateProvider.NotifyUserAuthentication(result.Data.AccessToken);

                NavigationManager.NavigateTo("/", true);
            }
            else
            {
                
            }
        }


        private sealed class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; } = "";

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; } = "";
        }
    }
}
