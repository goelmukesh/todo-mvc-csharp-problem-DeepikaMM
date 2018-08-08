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
        //private NotesController _controller;
        //public UnitTest()
        //{
        //    var optionsBuilder = new DbContextOptionsBuilder<TodoNotesContext>();
        //    optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
        //    TodoNotesContext context = new TodoNotesContext(optionsBuilder.Options);
        //    _controller = new NotesController(context);
        //    PrepareInMemoryData(context);

        //}
        public NotesController GetController()
        {
            var optionsBuilder = new DbContextOptionsBuilder<TodoNotesContext>();
            optionsBuilder.UseInMemoryDatabase<TodoNotesContext>(Guid.NewGuid().ToString());
            TodoNotesContext todoNotesContext = new TodoNotesContext(optionsBuilder.Options);
            PrepareInMemoryData(optionsBuilder.Options);
            return new NotesController(todoNotesContext);
        }
        public void PrepareInMemoryData(DbContextOptions<TodoNotesContext> options)
        {
            using (var todoNotesContext = new TodoNotesContext(options))
            {
                var notes = new List<Notes>
            {

                new Notes()
                {
                    Id=1,
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
            },
            new Notes()
                {
                Id=2,
                Title = "Note two",
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
                 }
            };

                todoNotesContext.Notes.AddRange(notes);
                todoNotesContext.SaveChanges();
            }
        }

        [Fact]
        public async void TestGetTodoNotes()
        {
            var _controller = GetController();
            var result = await _controller.GetTodoNotes();
            var OkObj = result.ToList();
            var actual = OkObj[0];
            //Assert.Single(OkObj);
            actual.Title.Should().Be("Note one");
            actual.PlainText.Should().Be("This is plaintext");
            actual.PinStatus.Should().Be(true);
            actual.checkList[0].CheckListName.Should().Be("Note one CheckList");
            actual.checkList[0].CheckListStatus.Should().Be(true);
            actual.label[0].LabelName.Should().Be("note one label");
        }


        [Fact]
        public async void TestGetNotesBylabel()
        {
            var _controller = GetController();
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
        public async void TestGetTodoNotesById()
        {
            //PrepareData(todocontext);
            var _controller = GetController();
            //var result = await _controller.GetTodoNotes();
            //var data = result as List<Notes>;

            //var dataToTest = data[0] as Notes;
            var IdOfdataToTest = await _controller.GetTodoNotes(1);
            var checkIdOddataTotest = IdOfdataToTest as OkObjectResult;
            var finalDataToBeComapred = checkIdOddataTotest.Value as Notes;
            finalDataToBeComapred.Title.Should().Be("Note one");

        }
        [Fact]
        public async void TestGetNotesByTitle()
        {
            var _controller = GetController();
            var IdOfdataToTest = await _controller.GetTodoNotesByTitle("Note one");
            // var checkIdOddataTotest = IdOfdataToTest as OkObjectResult;
            var finalDataToBeComapred = IdOfdataToTest as List<Notes>;
            var finalSingleNote = finalDataToBeComapred[0];
            finalSingleNote.Title.Should().Be("Note one");


        }
        [Fact]
        public async void TestGetNotesByPinnedStatus()
        {
            var _controller = GetController();
            var result = await _controller.GetTodoNotes(true);
            var finalDataToBeComapred = result as OkObjectResult;
            var finalSingleNote = finalDataToBeComapred.Value as List<Notes>;
            var data = finalSingleNote[0];
            Assert.NotNull(result);
            data.Title.Should().Be("Note one");

        }


        
        [Fact]
        public async void TestPut()
        {
            var _controller = GetController();
            //var r = await _controller.GetNotes();
            //var ListOfNotes = r as List<Notes>;
            //var ToBeChecked = ListOfNotes[0];
            //note.Id = ToBeChecked.Id;
            var NotePut = new Notes()
            {
                Id = 2,
                Title = "New Title"
            };
            //var json = JsonConvert.SerializeObject(NotePut);
            //var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
            var result = await _controller.PutTodoNotes(2, NotePut);
            var resultAsOkObjectResult = result as NoContentResult;
            //var notes = resultAsOkObjectResult.Value as Notes;
            Assert.Equal(resultAsOkObjectResult.StatusCode, 204);
        }
        [Fact]
        public async void TestPost()
        {
            Notes note = new Notes
            {
                Id = 3,
                Title = "Post",
                PlainText = "Testing Post",
                PinStatus = true,
            };
            var _controller = GetController();
            var result = await _controller.PostTodoNotes(note);
            var resultAsOkObjectResult = result as CreatedAtActionResult;
            var notes = resultAsOkObjectResult.Value as Notes;
            Assert.NotNull(notes);
            Assert.Equal(201, resultAsOkObjectResult.StatusCode);
        }


        [Fact]
        public async void TestDelete()
        {
            var _controller = GetController();
            //var result = await _controller.GetTodoNotes();
            //var data = result as List<Notes>;
            //int save = data[0].Id;
            var response = await _controller.DeleteTodoNotes(1);
            var resultAsOkObjectResult = response as OkObjectResult;
            //var notes = resultAsOkObjectResult.Value as Notes;
            //Assert.NotNull(notes);
            Assert.Equal(200, resultAsOkObjectResult.StatusCode);
        }

    }
}






























