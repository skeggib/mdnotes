using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace MdNotes.Tests
{
    public class NoteDeleteTests
    {
        private readonly UserKey _user;

        public NoteDeleteTests()
        {
            var name = Utils.GetUniqueString();
            _user = new HttpClient().Post(
                $"{Utils.BaseUri}users",
                new JsonContent($"{{\"name\": \"{name}\"}}")).Content.DeserializeJson<UserKey>();
        }

        [Fact]
        public void CanDeleteNote()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, $"{Utils.BaseUri}notes");
            request.Content = new JsonContent("{ \"title\": \"Title\", \"content\": \"Content\" }");
            request.Headers.Add("Authorization", $"Bearer {_user.DefaultKey}");
            var responseCreate = new HttpClient().Send(request);
            var noteCreate = responseCreate.Content.DeserializeJson<Note>();

            var request2 = new HttpRequestMessage(HttpMethod.Delete, $"{Utils.BaseUri}notes/{noteCreate.Id}");
            request2.Headers.Add("Authorization", $"Bearer {_user.DefaultKey}");
            var response = new HttpClient().Send(request2);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public void DeletingNonExistingNoteGives404()
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, $"{Utils.BaseUri}users/9999");
            request.Headers.Add("Authorization", $"Bearer {_user.DefaultKey}");
            var response = new HttpClient().Send(request);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public void InvalidTokenGives401()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, $"{Utils.BaseUri}notes");
            request.Content = new JsonContent("{ \"title\": \"Title\", \"content\": \"Content\" }");
            request.Headers.Add("Authorization", $"Bearer {_user.DefaultKey}");
            var responseCreate = new HttpClient().Send(request);
            var noteCreate = responseCreate.Content.DeserializeJson<Note>();

            var request2 = new HttpRequestMessage(HttpMethod.Delete, $"{Utils.BaseUri}notes/{noteCreate.Id}");
            request2.Headers.Add("Authorization", $"Bearer 9999");
            var response = new HttpClient().Send(request2);
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public void MissingTokenGives401()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, $"{Utils.BaseUri}notes");
            request.Content = new JsonContent("{ \"title\": \"Title\", \"content\": \"Content\" }");
            request.Headers.Add("Authorization", $"Bearer {_user.DefaultKey}");
            var responseCreate = new HttpClient().Send(request);
            var noteCreate = responseCreate.Content.DeserializeJson<Note>();

            var request2 = new HttpRequestMessage(HttpMethod.Delete, $"{Utils.BaseUri}notes/{noteCreate.Id}");
            var response = new HttpClient().Send(request2);
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}
