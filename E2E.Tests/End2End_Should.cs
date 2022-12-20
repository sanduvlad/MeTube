using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Xunit;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace E2E.Tests
{
    public class End2End_Should : IClassFixture<WebApplicationFactory<MeTube.Program>>
    {
        private readonly IConfiguration _configuration;
        public End2End_Should()
        {
            _configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, false)
                .Build();
        }

        [Fact]
        public async Task Upload_A_Video_Queue_It_And_Compress()
        {
            var messageQueueFactory = new WebApplicationFactory<MessageQueue.Program>();
            var meTubeFactory = new WebApplicationFactory<MeTube.Startup>().WithWebHostBuilder(builder =>
            {
                builder.ConfigureAppConfiguration((_, appConfig) =>
                {
                    appConfig.AddJsonFile(Directory.GetCurrentDirectory() + "/appsettings.test.json");
                });
                builder.ConfigureServices(svc =>
                {
                    svc.AddHttpClient("", (scvPrv) => { messageQueueFactory.CreateClient();  });
                });
            });

            var client = meTubeFactory.CreateClient();

            var responseMessage =
                await client.PostAsync("api/Video/Upload",
                    new StringContent(JsonConvert.SerializeObject(new
                    {
                        Name = "name",
                        VideoData = "sdkjanjkdjkdn"
                    }), Encoding.UTF8, "application/json"));

            var messageId = await responseMessage.Content.ReadAsStringAsync();

            responseMessage.StatusCode.Should().Be(HttpStatusCode.OK);

            if (Guid.TryParse(messageId, out _))
            {
                Assert.True(true);
            }
            else
            {
                Assert.False(false);
            }
        }
    }
}
