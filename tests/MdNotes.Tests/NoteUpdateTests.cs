using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using Xunit;

namespace MdNotes.Tests
{
    public class NoteUpdateTests
    {
        private readonly UserKey _userKey;
        private Note _note;

        public NoteUpdateTests()
        {
            var name = Utils.GetUniqueString();
            _userKey = new HttpClient().Post(
                $"{Utils.BaseUri}users",
                new JsonContent($"{{\"name\": \"{name}\"}}")).Content.DeserializeJson<UserKey>();

            var request = new HttpRequestMessage(HttpMethod.Post, $"{Utils.BaseUri}notes");
            request.Headers.Add("Authorization", $"Bearer {_userKey.DefaultKey}");
            request.Content = new JsonContent("{\"title\":\"Title\",\"content\":\"Content\"}");

            _note = new HttpClient().Send(request).Content.DeserializeJson<Note>();
        }

        [Fact]
        public void CanUpdateNote()
        {
            var requestUpdate = new HttpRequestMessage(HttpMethod.Put, $"{Utils.BaseUri}notes/{_note.Id}");
            requestUpdate.Headers.Add("Authorization", $"Bearer {_userKey.DefaultKey}");
            requestUpdate.Content = new JsonContent("{\"title\":\"Title_test\",\"content\":\"Content_test\"}");

            var responseUpdate = new HttpClient().Send(requestUpdate);
            var noteUpdate = responseUpdate.Content.DeserializeJson<Note>();

            var requestGet = new HttpRequestMessage(HttpMethod.Get, $"{Utils.BaseUri}notes/{_note.Id}");
            requestGet.Headers.Add("Authorization", $"Bearer {_userKey.DefaultKey}");

            var responseGet = new HttpClient().Send(requestGet);
            var noteGet = responseGet.Content.DeserializeJson<Note>();

            Assert.Equal(HttpStatusCode.OK, responseUpdate.StatusCode);
            Assert.Equal(noteUpdate.Title, noteGet.Title);
            Assert.Equal(noteUpdate.Content, noteGet.Content);
        }

        [Fact]
        public void UpdatingNotExistingNoteGives404()
        {
            var requestGet = new HttpRequestMessage(HttpMethod.Get, $"{Utils.BaseUri}notes/9999");
            requestGet.Headers.Add("Authorization", $"Bearer {_userKey.DefaultKey}");

            var responseGet = new HttpClient().Send(requestGet);
            Assert.Equal(HttpStatusCode.NotFound, responseGet.StatusCode);
        }

        [Fact]
        public void UpdatingNoteWhithoutRightKey401()
        {
            UserKey _userKeyOther = new HttpClient().Post(
                $"{Utils.BaseUri}users",
                new JsonContent("{\"name\": \"other\"}")).Content.DeserializeJson<UserKey>();

            var request = new HttpRequestMessage(HttpMethod.Put, $"{Utils.BaseUri}notes/{_note.Id}");
            request.Headers.Add("Authorization", $"Bearer {_userKeyOther.DefaultKey}");
            request.Content = new JsonContent("{\"title\":\"Title_test_Other\",\"content\":\"Content_test_Other\"}");

            var putResponse = new HttpClient().Send(request);
            Assert.Equal(HttpStatusCode.Unauthorized, putResponse.StatusCode);
        }

        [Fact]
        public void FalseTokenUpdateNote()
        {
            var requestUpdate = new HttpRequestMessage(HttpMethod.Put, $"{Utils.BaseUri}notes/{_note.Id}");
            requestUpdate.Content = new JsonContent("{\"title\":\"Title_test\",\"content\":\"Content_test\"}");

            var responseUpdate = new HttpClient().Send(requestUpdate);

            Assert.Equal(HttpStatusCode.Unauthorized, responseUpdate.StatusCode);
        }
        [Fact]
        public void InvalidTokenUpdateNote()
        {
            var requestUpdate = new HttpRequestMessage(HttpMethod.Put, $"{Utils.BaseUri}notes/{_note.Id}");
            requestUpdate.Content = new JsonContent("{\"title\":\"Title_test\",\"content\":\"Content_test\"}");
            requestUpdate.Headers.Add("Authorization", "Bearer 99999999");
            var responseUpdate = new HttpClient().Send(requestUpdate);

            Assert.Equal(HttpStatusCode.Unauthorized, responseUpdate.StatusCode);
        }
    }
}
