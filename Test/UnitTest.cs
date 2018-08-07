using System;
using TodoNotes.Controllers;
using Xunit;
using TodoNotes.Models;
using Microsoft.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using FluentAssertions;

namespace Test
{
    public class UnitTest
    {
        private NotesController _controller;
        public UnitTest()
        {
            var optionsBuilder = new DbContextOptionsBuilder<TodoNotesContext>();
            optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
            TodoNotesContext context = new TodoNotesContext(optionsBuilder.Options);
            _controller = new NotesController(context);
            PrepareInMemoryData(context);

        }
        public void PrepareInMemoryData(TodoNotesContext context)
        {
            var note = new Notes()
            {
                Title = "Note one",
                PlainText = "This is plaintext",
                PinStatus = true,
                checkList = new List<CheckList>()
                    {
                        new CheckList()
                        {
                            CheckListName="Note one CheckList",
                            CheckListStatus=true
                        }
                    },
                label = new List<Label>()
                    {
                        new Label()
                        {
                            LabelName ="note one label"
                        }
                    }
            };
            context.Notes.Add(note);
            context.SaveChanges();
        }

        [Fact]
        public async void TestGetTodoNotes()
        {

            var result = await _controller.GetTodoNotes();
            var OkObj = result.ToList();
            var actual = OkObj[0];
            Assert.Single(OkObj);
            actual.Title.Should().Be("Note one");
            actual.PlainText.Should().Be("This is plaintext");
            actual.PinStatus.Should().Be(true);
            actual.checkList[0].CheckListName.Should().Be("Note one CheckList");
            actual.checkList[0].CheckListStatus.Should().Be(true);
            actual.label[0].LabelName.Should().Be("note one label");
        }
        [Fact]
        public async void TestGetTodoNotesById()
        {
            //PrepareData(todocontext);
            var result = await _controller.GetTodoNotes();
            var data = result as List<Notes>;
           
           //var dataToTest = data[0] as Notes;
            var IdOfdataToTest = await _controller.GetTodoNotes(data[0].Id);
            var checkIdOddataTotest = IdOfdataToTest as OkObjectResult;
            var finalDataToBeComapred = checkIdOddataTotest.Value as Notes;
            finalDataToBeComapred.Title.Should().Be("Note one");
            
        }

        [Fact]
        public async void TestGetNotesBylabel()
        {

            var result = await _controller.GetTodoNotesLabel("note one label");
            var OkObj = result as OkObjectResult;
            var Notes = OkObj.Value as List<Notes>;
            Assert.Equal(OkObj.StatusCode, 200);
            var ToBeTested = Notes[0];
            ToBeTested.PlainText.Should().Be("This is plaintext");
            ToBeTested.PinStatus.Should().Be(true);
            ToBeTested.Title.Should().Be("Note one");

        }
        [Fact]
        public async void TestGetNotesByTitle()
        {
            var IdOfdataToTest = await _controller.GetTodoNotesByTitle("Note one");
           // var checkIdOddataTotest = IdOfdataToTest as OkObjectResult;
            var finalDataToBeComapred = IdOfdataToTest as List<Notes>;
            var finalSingleNote = finalDataToBeComapred[0];
            finalSingleNote.Title.Should().Be("Note one");


        }
        [Fact]
        public async void TestGetNotesByPinnedStatus()
        {
            var result = await _controller.GetTodoNotes(true);
            var finalDataToBeComapred = result as OkObjectResult;
            var finalSingleNote = finalDataToBeComapred.Value as List<Notes>;
            var data = finalSingleNote[0];
            Assert.NotNull(result);
            data.Title.Should().Be("Note one");

        }
        //[Fact]
        //public async void TestPut()
        //{

        //    var result = await _controller.GetTodoNotes();
        //    var data = result as List<Notes>;
        //    int save = data[0].Id;
        //    Notes note = new Notes
        //    {
        //        Id = save,
        //        Title = "testing Put",
        //        PlainText = "still testing",
        //        PinStatus = true,
        //    };
        //    var response = await _controller.PutTodoNotes(save, note);
        //    var resultAsOkObjectResult = response as CreatedAtActionResult;
        //    //var notes = resultAsOkObjectResult.Value as Notes;
        //    Assert.Equal(204, resultAsOkObjectResult.StatusCode);

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
            var result = await _controller.PostTodoNotes(note);
            var resultAsOkObjectResult = result as CreatedAtActionResult;
            var notes = resultAsOkObjectResult.Value as Notes;
            Assert.NotNull(notes);
            Assert.Equal(201, resultAsOkObjectResult.StatusCode);
        }
        [Fact]
        public async void TestDelete()
        {
            var result = await _controller.GetTodoNotes();
            var data = result as List<Notes>;
            int save = data[0].Id;
            var response = await _controller.DeleteTodoNotes(save);
            var resultAsOkObjectResult = response as OkObjectResult;
            var notes = resultAsOkObjectResult.Value as Notes;
            Assert.NotNull(notes);
            Assert.Equal(200, resultAsOkObjectResult.StatusCode);
        }

    }
}
