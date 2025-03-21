namespace BaseAppSettings.Middlewares
{
    public class OutgoingRequestLoggingHandler : DelegatingHandler
    {
        private readonly LoggingDbContext _dbContext;
        public OutgoingRequestLoggingHandler(LoggingDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Log request parameters here
            //Console.WriteLine("Request URI: " + request.RequestUri);
            //Console.WriteLine("Request Method: " + request.Method);
            //Console.WriteLine("Request Headers:");
            //foreach (var header in request.Headers)
            //{
            //    Console.WriteLine($"{header.Key}: {string.Join(",", header.Value)}");
            //}
            //if (request.Content != null)
            //{
            //    Console.WriteLine("Request Content:");
            //    Console.WriteLine(await request.Content.ReadAsStringAsync());
            //}
            string reqBody = await FormatRequestBody(request);
            // Call the inner handler
            var response = await base.SendAsync(request, cancellationToken);

            string resBody = "";
            string responseBody = await response.Content.ReadAsStringAsync();
            // Store the response content in a memory stream
            byte[] contentBytes = System.Text.Encoding.UTF8.GetBytes(responseBody);
            using (MemoryStream memoryStream = new MemoryStream(contentBytes))
            {
                // Reset the position of the memory stream to the beginning
                memoryStream.Position = 0;

                // Read from the memory stream
                using (StreamReader reader = new StreamReader(memoryStream))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        // Log or process each line of the response content
                        resBody += line;
                    }
                }
            }


            // Log response if needed
            // Note: Logging the response might not be appropriate in all cases,
            // especially when dealing with large responses or sensitive data.

            await _dbContext.LogApiCall("Logistics",
                request.RequestUri.ToString(),
                reqBody,
                FormatRequestHeaders(request),
                resBody,
                FormatResponseHeaders(response),
                (int)response.StatusCode,
                request.Method.ToString()
            );

            await ResetHTTPResponseMessagePosition(response);

            return response;
        }
        private static string FormatRequestHeaders(HttpRequestMessage request)
        {
            return request.Headers.ToString();
            //StringBuilder sb = new StringBuilder();
            //foreach (var header in request.Headers)
            //{
            //    sb.AppendFormat("{0}:{1}\r\n", header.Key, header.Value);
            //}
            //return sb.ToString();
        }
        private static string FormatResponseHeaders(HttpResponseMessage? response)
        {
            if (response == null) { return string.Empty; }
            return response.Headers.ToString();
            //StringBuilder sb = new StringBuilder();
            //foreach (var header in response.Headers)
            //{
            //    sb.AppendFormat("{0}:{1}\r\n", header.Key, header.Value);
            //}
            //return sb.ToString();
        }
        private async Task<string> FormatRequestBody(HttpRequestMessage request)
        {
            if (request.Content == null) { return string.Empty; }
            return await request.Content.ReadAsStringAsync();
        }
        private async Task<string> FormatResponseContent(HttpResponseMessage? response)
        {
            if (response == null || true) { return string.Empty; }
            return await response.Content.ReadAsStringAsync();
        }
        private async Task ResetHTTPResponseMessagePosition(HttpResponseMessage? response)
        {
            // Reset the position of the response content stream to the beginning
            if (response.Content != null)
            {
                // Read the content as a stream
                Stream contentStream = await response.Content.ReadAsStreamAsync();

                // Reset the stream position to the beginning
                if (contentStream.CanSeek)
                {
                    contentStream.Seek(0, SeekOrigin.Begin);
                }
            }
            //// Reset the position of the response content stream to the beginning
            //if (response.Content != null && response.Content is StreamContent streamContent)
            //{
            //    //streamContent.Headers.AllowReadStreamBuffering = false; // Ensure that content buffering is disabled
            //    streamContent.Headers.ContentLength = null; // Reset content length to allow seeking
            //    streamContent.Headers.ContentRange = null; // Reset content range
            //    streamContent.Headers.ContentDisposition = null; // Reset content disposition
            //    streamContent.Headers.ContentType = null; // Reset content type
            //    streamContent.Headers.ContentEncoding.Clear(); // Clear any content encoding
            //    streamContent.Headers.ContentMD5 = null; // Reset content MD5

            //    response.Content.Headers.ContentLength = null; // Reset response content length
            //    response.Content.Headers.ContentRange = null; // Reset response content range

            //    streamContent.Stream.Position = 0; // Reset the stream position to the beginning
            //}

        }
    }

}
