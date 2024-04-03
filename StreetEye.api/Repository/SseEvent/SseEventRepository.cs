using StreetEye.Exceptions;
namespace StreetEye.Repository.SseEvent
{
    public sealed class SseEventRepository : ISseEventRepository
    {
        private readonly HttpClient _httpClient;

        public SseEventRepository(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<Stream> GetSseEventStreamAsync(string sseEndppointUrl, CancellationToken cancellationToken)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(sseEndppointUrl, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                throw new SseRequestException(response);
            }
            return await response.Content.ReadAsStreamAsync();
        }
    }
}
