using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using Xunit;

namespace MdNotes.Tests
{
    public class NoteGetTests
    {
        private readonly User _user;

        // public NoteGetTests()
        // {
        //     var name = Utils.GetUniqueString();
        //     _user = new HttpClient().Post(
        //         $"{Utils.BaseUri}users",
        //         new JsonContent($"{{\"name\": \"{name}\"}}")).Content.DeserializeJson<User>();
        // }

        // [Fact]
        // public void CanGetNote()
        // {
        //     var responseCreate = new HttpClient().Post(
        //         $"{Utils.BaseUri}notes?user_id={_user.Id}",
        //         new JsonContent("{\"title\":\"Title\",\"content\":\"Content\"}"));
        //     var noteCreate = responseCreate.Content.DeserializeJson<Note>();

        //     var response = new HttpClient().Get($"{Utils.BaseUri}notes/{noteCreate.Id}");
        //     var noteGet = response.Content.DeserializeJson<Note>();

        //     Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        //     Assert.Equal(noteCreate.Title, noteGet.Title);
        //     Assert.Equal(noteCreate.Content, noteGet.Content);
        // }

        // [Fact]
        // public void GettingNotExistingNoteGives404()
        // {
        //     var response = new HttpClient().Get($"{Utils.BaseUri}notes/9999");
        //     Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        // }
    }
}
