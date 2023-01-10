using Microsoft.AspNetCore.Mvc.Testing;
using NuGet.Common;
using NuGet.ContentModel;
using SimpleExpenseTracker.Shared.DTO;
using SimpleExpenseTracker.Shared.DTO.UserDTO;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json;

namespace SimpleExpenseTracker.Server.Test
{
    [TestClass]
    public class AuthControllerTest
    {
        private HttpClient _httpClient;
        private UserRegistrationDTO _user;

        public AuthControllerTest()
        {
            var webAppFactory = new WebApplicationFactory<Program>();
            _httpClient = webAppFactory.CreateDefaultClient();
            _user = new UserRegistrationDTO()
            {
                Name = "Test User",
                Email = Guid.NewGuid().ToString().Replace("-", "") + "@test.com",
                Password = "SuperStrongPassword",
                ConfirmPassword = "SuperStrongPassword"
            };
        }

        [TestMethod]
        public void Can_register_new_user_and_login()
        {
            // Register user
            var response = _httpClient.PostAsJsonAsync("/api/Auth/register", _user).Result;
            if (!response.IsSuccessStatusCode)
                Assert.Fail("Failed to register new user");

            // Login
            response = _httpClient.PostAsJsonAsync("/api/Auth/login", new UserLoginDTO
            {
                Email = _user.Email,
                Password = _user.Password,
            }).Result;

            string result = response.Content.ReadAsStringAsync().Result;
            var claims = ParseClaimsFromJwt(result);

            string role = claims?.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value;

            Assert.AreEqual("User", role);
        }       


        #region Helpers
        private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
            return keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()));
        }
        private static byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }
        #endregion
    }
}
