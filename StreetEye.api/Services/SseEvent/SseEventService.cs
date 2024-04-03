using StreetEye.Repository.SseEvent;

namespace StreetEye.Services.SseEvent
{
    public sealed class SseEventService : ISseEventService
    {
        private readonly ISseEventRepository _sseEventRepository;

        public SseEventService(ISseEventRepository sseEventRepository)
        {
            _sseEventRepository = sseEventRepository;
        }

        public async Task<List<string>> ConsumeSseEventStreamAsync(string sseEndpointUrl, CancellationToken cancellationToken)
        {
            using Stream stream = await _sseEventRepository.GetSseEventStreamAsync(sseEndpointUrl, cancellationToken);

            using StreamReader reader = new System.IO.StreamReader(stream);
            List<string> events = new List<string>();

            while (!cancellationToken.IsCancellationRequested && !reader.EndOfStream)
            {
                string line = await reader.ReadLineAsync();

                if (!string.IsNullOrEmpty(line) && !line.StartsWith(":"))
                {
                    // Process the SSE event
                    events.Add(line);
                }
            }
            return events;
        }
    }
}
