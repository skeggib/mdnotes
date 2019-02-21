using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace MdNotes.Tests
{
    public static class HttpClientExtension
    {
        public static HttpResponseMessage Get(this HttpClient client, string uri)
        {
            var task = client.GetAsync(uri);
            task.Wait();
            return task.Result;
        }

        public static HttpResponseMessage Post(this HttpClient client, string uri, HttpContent content)
        {
            var task = client.PostAsync(uri, content);
            task.Wait();
            return task.Result;
        }

        public static HttpResponseMessage Put(this HttpClient client, string uri, HttpContent content)
        {
            var task = client.PutAsync(uri, content);
            task.Wait();
            return task.Result;
        }

        public static HttpResponseMessage Delete(this HttpClient client, string uri)
        {
            var task = client.DeleteAsync(uri);
            task.Wait();
            return task.Result;
        }
    }
}
