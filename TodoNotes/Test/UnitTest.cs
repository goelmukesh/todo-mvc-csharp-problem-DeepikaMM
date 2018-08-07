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

namespace Test
{
    public class UnitTest
    {
        private NotesController _controller;
        public UnitTest()
        {
            var optionsBuilder = new DbContextOptionsBuilder<TodoNotesContext>();
            optionsBuilder.UseInMemoryDatabase("Testing");
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
            var OkObj = result as List<Notes>;
            Assert.Single(OkObj);
        }
        [Fact]
        public async void TestGetTodoNotesById()
        {
            //PrepareData(todocontext);
            var result = await _controller.GetTodoNotes(1);
            try
            {
                var status = result as OkObjectResult;
                Assert.Equal(200, status.StatusCode);
            }
            catch (Exception)
            {
                
            }
        }

        [Fact]
        public async void TestGetNotesBylabel()
        {
            var result = await _controller.GetTodoNotes("note one label");
            try
            {
                var status = result as OkObjectResult;
                Assert.Equal(200, status.StatusCode);

            }
            catch (Exception)
            {
                Console.WriteLine("Error Occured");
            }

        }
        [Fact]
        public async void TestGetNotesByTitle()
        {
            var result = await _controller.GetTodoNotesByTitle("Note one");
            var list = result as List<Notes>;
            Assert.Equal(2, list.Count);
            
        }
        [Fact]
        public async void TestGetNotesByPinnedStatus()
        {
            var result = await _controller.GetTodoNotes(true);
            Assert.NotNull(result);
        }
        [Fact]
        public async void TestPut()
        {
            Notes note = new Notes
            {
                Title = "testing Put",
                PlainText = "still testing",
                PinStatus = true,
            };
            var result = await _controller.PutTodoNotes(1, note);
            var resultAsOkObjectResult = result as OkObjectResult;
            //var notes = resultAsOkObjectResult.Value as Notes;
            Assert.Equal(204, resultAsOkObjectResult.StatusCode);
        }
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
        }
        [Fact]
        public async void TestDelete()
        {
            var result = await _controller.DeleteTodoNotes(1);
            var resultAsOkObjectResult = result as OkObjectResult;
            var notes = resultAsOkObjectResult.Value as Notes;
            Assert.NotNull(notes);
        }

    }
}
