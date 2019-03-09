using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace DynamicTranslator.Tests
{
    public class TestMessageHandler : HttpMessageHandler
    {
        public Func<HttpRequestMessage, Task<HttpResponseMessage>> Sender { private get; set; }

        public List<HttpRequestMessage> SentRequestMessages { get; } = new List<HttpRequestMessage>();

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            SentRequestMessages.Add(request);

            if (Sender == null)
            {
                return new HttpResponseMessage(HttpStatusCode.NotImplemented);
            }

            return await Sender?.Invoke(request);
        }
    }
}