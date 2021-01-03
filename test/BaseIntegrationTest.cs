using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using FluentAssertions;
using KiancaAPI.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Xunit;
using Microsoft.Extensions.DependencyInjection;

namespace KiancaAPI.Test 
{ 
    public class BaseIntegrationTest: IClassFixture<WebApplicationFactory<Startup>> 
    {
        private readonly HttpClient _client;
     
        public BaseIntegrationTest(WebApplicationFactory<Startup> factory)
        {
            var projectDir = Directory.GetCurrentDirectory();
            var configPath = Path.Combine(projectDir, "appsettings.Development.json");
     
            var web = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureAppConfiguration((context,conf) =>
                {
                    conf.AddJsonFile(configPath);
                });
                
                builder.ConfigureTestServices(services =>
                {
                    services.PostConfigure<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme, options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters()
                        {
                            IssuerSigningKey = FakeJwtManager.SecurityKey,
                            ValidIssuer = FakeJwtManager.Issuer,
                            ValidAudience = FakeJwtManager.Audience,
                        };
                    });
                });
            });
            
            _client = web.CreateClient();
        }
        
        [Theory]
        [InlineData("GET")]
        public async Task Get_Should_Retrieve_All_Person(string method)
        {
            // Arrange
            var request = new HttpRequestMessage(new HttpMethod(method), "/api/Person");
            var accessToken = FakeJwtManager.GenerateJwtToken();
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            
            // ACT
            var response = await _client.SendAsync(request);
            
            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            using (Stream stream = await response.Content.ReadAsStreamAsync())
            using (StreamReader streamReader = new StreamReader(stream))
            {    
                string responseContent = streamReader.ReadToEnd();

                var lista = responseContent.ToObjects<Person>();
                lista.Should().HaveCount(100);
            }
        }
        
        [Fact]
        public async Task Get_Should_Delete_All_Person()
        {
            // Arrange
            var request = new HttpRequestMessage(new HttpMethod("GET"), "/api/Person");
            var accessToken = FakeJwtManager.GenerateJwtToken();
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            
            // ACT
            var response = await _client.SendAsync(request);
            
            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            List<Person> lista;
            using (Stream stream = await response.Content.ReadAsStreamAsync())
            using (StreamReader streamReader = new StreamReader(stream))
            {    
                string responseContent = streamReader.ReadToEnd();

                lista = responseContent.ToObjects<Person>();
            }

            int n = 0;
            foreach (var person in lista)
            {
                n += 1;
                var requestDel = new HttpRequestMessage(new HttpMethod("DELETE"), $"/api/Person/{person.Id}");
                requestDel.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            
                // ACT
                var responseDel = await _client.SendAsync(requestDel);
                responseDel.EnsureSuccessStatusCode();
                Assert.Equal(HttpStatusCode.NoContent, responseDel.StatusCode);
            
                var request2 = new HttpRequestMessage(new HttpMethod("GET"), "/api/Person");
                request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                response = await _client.SendAsync(request2);
            
                // Assert
                response.EnsureSuccessStatusCode();
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                using (Stream stream = await response.Content.ReadAsStreamAsync())
                using (StreamReader streamReader = new StreamReader(stream))
                {    
                    string responseContent = streamReader.ReadToEnd();

                    var result = responseContent.ToObjects<Person>();
                    result.Should().HaveCount(lista.Count - n);
                }
            }
        }
    }
}
