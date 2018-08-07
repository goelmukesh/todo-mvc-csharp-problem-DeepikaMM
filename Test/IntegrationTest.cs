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
using Newtonsoft.Json.Linq;

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
        public async void TestAll()
        {
            Notes note = new Notes
            {
                Id = 1,
                Title = "GOT",
                PlainText = "Testing GOT",
                PinStatus = true,
               
            };
            var data = JsonConvert.SerializeObject(note);
           
            var stringContent = new StringContent(data, UnicodeEncoding.UTF8, "application/json");
            var Response = await _client.PostAsync("/api/Notes", stringContent);
            Response.EnsureSuccessStatusCode();
            var TestGetById = await _client.GetAsync("/api/Notes/1");
            var content = await TestGetById.Content.ReadAsStringAsync();
            Console.WriteLine(content);
            var ActualDataToTestGet = JObject.Parse(content);
            //var ActualDataToTestGet = ActualData[0];
            Assert.Equal(ActualDataToTestGet["id"].ToString(), "1");
            Assert.Equal(ActualDataToTestGet["title"], "GOT");
            Notes notes = new Notes
            {
                Id = 1,
                Title = "Young Sheldon",
                PlainText = "Testing Young Sheldon",
                PinStatus = false,

            };
            var dataPut = JsonConvert.SerializeObject(notes);
            var stringContentPut = new StringContent(dataPut, UnicodeEncoding.UTF8, "application/json");
            var TestPut = await _client.PutAsync("/api/Notes/1", stringContentPut);
            Assert.Equal(TestPut.StatusCode.ToString(), "NoContent");
            var TestGetByIdAfterPut = await _client.GetAsync("/api/Notes/1");
            var contentAfterPut = await TestGetByIdAfterPut.Content.ReadAsStringAsync();
            Console.WriteLine(contentAfterPut);
            //var ActualDataToTestGetAfterPut = JObject.Parse(contentAfterPut);
            //var ActualDataToTestGet = ActualData[0];
            //Assert.Equal(ActualDataToTestGetAfterPut["Id"].ToString(), "1");
            //Assert.Equal(ActualDataToTestGetAfterPut["Title"], "Young Sheldon");
            var TestDelete = await _client.DeleteAsync("/api/Notes/1");
            TestDelete.EnsureSuccessStatusCode();
        
        //var result = await _client.GetAsync("/api/Notes", data);
    }


    }
}
