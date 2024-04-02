using StreetEye.Exceptions;
namespace StreetEye.Repository
{
    public sealed class SseEventRepository : ISseEventRepository
    {
        private readonly HttpClient _httpClient;

        public SseEventRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<System.IO.Stream> GetSseEventStreamAsync(string sseEndppointUrl, CancellationToken cancellationToken)
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
