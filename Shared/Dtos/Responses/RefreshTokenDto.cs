namespace E_Commerce_Shop_Api.Dtos.Responses
{
    public class RefreshTokenDto
    {
        public string RefreshToken { get; set; } = string.Empty;
        public string AccessToken { get; set; } = string.Empty;
        public DateTime RefreshTokenExipirityDate { get; set; }
    }
}
