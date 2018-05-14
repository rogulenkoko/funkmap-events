using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Funkmap.Events.Web;
using Funkmap.Events.Web.Controllers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Funkmap.Events.Tests
{
    public class BandmapAuthorizationTest
    {
        private readonly HttpClient _client;

        public BandmapAuthorizationTest()
        {
            var server = new TestServer(new WebHostBuilder()
                .UseStartup<Startup>());
            _client = server.CreateClient();
        }


        [Fact]
        public void AuthTest()
        {
            var response = _client.PostAsync("/api/event", null).GetAwaiter().GetResult();
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);


            //var bandmapApiUrl = "http://bandmap-api.azurewebsites.net/api";
            var bandmapApiUrl = "http://localhost/api";
            var bandmapLogin = "demo";
            var bandmapPassword = "demo";

            String token;

            using (var httpClient = new HttpClient())
            {
                var request = new HttpRequestMessage()
                {
                    Content = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>()
                    {
                        new KeyValuePair<string, string>("username", bandmapLogin),
                        new KeyValuePair<string, string>("password", bandmapPassword),
                        new KeyValuePair<string, string>("grant_type", "password"),
                        new KeyValuePair<string, string>("client_id", "funkmap"),
                        new KeyValuePair<string, string>("client_secret", "funkmap"),
                    }),
                    Method = HttpMethod.Post,
                    RequestUri = new Uri($"{bandmapApiUrl}/token")
                };

                HttpResponseMessage loginResponse = httpClient.SendAsync(request).GetAwaiter().GetResult();
                Assert.Equal(HttpStatusCode.OK, loginResponse.StatusCode);

                var contentJson = loginResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                token = JObject.Parse(contentJson).SelectToken("access_token").ToString();
                Assert.NotEmpty(token);
            }

            var requestMessage = new HttpRequestMessage
            {
                RequestUri = new Uri("/api/event", UriKind.Relative),
                Headers = { {"Authorization", $"Bearer {token}" } }, 
                Method = HttpMethod.Post
            };
            
            response = _client.SendAsync(requestMessage).GetAwaiter().GetResult();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
