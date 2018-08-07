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
using TodoNotes.Models;

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
        [Fact]
        public async void TestGetTodoNotes()
        {
            var result = await _client.GetAsync("/api/Notes");

            var resultBody = await result.Content.ReadAsStringAsync();
            result.EnsureSuccessStatusCode();
        }
        //[Fact]
        //public async void TestGetNotesById()
        //{
        //    var result = await _client.GetAsync("/api/Notes/1");
        //    result.EnsureSuccessStatusCode();

        //}
        [Fact]
        public async void TestGetNotesByTitle()
        {
            var result = await _client.GetAsync("/api/Notes/title/BBT");
            result.EnsureSuccessStatusCode();
        }
        [Fact]
        public async void TestGetNotesByLabel()
        {

            var result = await _client.GetAsync("/api/Notes?Label=friend suggestion");
            result.EnsureSuccessStatusCode();
        }
        [Fact]
        public async void TestGetNotesByPinnedStatus()
        {

            var result = await _client.GetAsync("/api/Notes/pin/true");
            result.EnsureSuccessStatusCode();
        }
        //[Fact]
        //public async void TestPut()
        //{
        //    Notes note = new Notes
        //    {
        //        Id = save,
        //        Title = "testing Put",
        //        PlainText = "still testing",
        //        PinStatus = true,
        //    };
        //    var result = await _client.GetAsync("/api/Notes/5", note);
        //    result.EnsureSuccessStatusCode();
        //}
        [Fact]
        public async void TestPost()
        {
            Notes note = new Notes
            {
                Title = "Post",
                PlainText = "Testing Post",
                PinStatus = true,
            };
            var data = JsonConvert.SerializeObject(note);
           
            var stringContent = new StringContent(data, UnicodeEncoding.UTF8, "application/json");
            var Response = await _client.PostAsync("/api/Notes", stringContent);
            Response.EnsureSuccessStatusCode();
            //var result = await _client.GetAsync("/api/Notes", data);
        }


    }
}
