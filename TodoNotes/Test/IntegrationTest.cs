using System;
using Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text;
using TodoNotes;

namespace Test
{
    public class IntegrationTest
    {
        private HttpClient _client;
        public IntegrationTest()
        {
            var host = new TestServer(new WebHostBuilder()
                .UseEnvironment("Testing")
                .UseStartup<Startup>());
            _client = host.CreateClient();
        }
        //[Fact]
        //public async void TestGetTodoNotes()
        //{
        //    var result = await _client.GetAsync("/api/Notes");
        //    var resultBody = await result.Content.ReadAsStringAsync();
        //    Console.WriteLine("heello wprl",resultBody.Length);
        //    Assert.Equal(2, resultBody.Length);
        //}
        //[Fact]
        //public async void TestGetNotesById()
        //{
        //    var result = await _client.GetAsync("/api/Notes/20");
        //    Assert.Equal("NotFound",result.StatusCode.ToString());
        //}

    }
}
