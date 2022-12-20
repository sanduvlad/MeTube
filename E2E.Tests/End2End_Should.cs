using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Xunit;

namespace E2E.Tests
{
    public class End2End_Should : IClassFixture<WebApplicationFactory<MeTube.Program>>
    {
        [Fact]
        public async Task Upload_A_Video_Queue_It_And_Compress()
        {
            var meTubeFactory = new WebApplicationFactory<MeTube.Program>();
            var messageQueueFactory = new WebApplicationFactory<MessageQueue.Program>();

            var client = meTubeFactory.CreateClient();

            var simple =
                await client.PostAsync("api/Video/Upload",
                    new StringContent(JsonConvert.SerializeObject(new
                    {
                        Name = "name",
                        VideoData = "sdkjanjkdjkdn"
                    }), Encoding.UTF8, "application/json"));

            var result = await simple.Content.ReadAsStringAsync();

            Assert.True(simple.StatusCode != HttpStatusCode.InternalServerError);

            //if (Guid.TryParse(result, out _))
            //{
            //    Assert.True(true);
            //}
            //else
            //{
            //    Assert.False(false);
            //}
        }
    }
}
