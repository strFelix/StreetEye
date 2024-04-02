namespace StreetEye.Services.SseEvent
{
    public interface ISseEventService
    {
        Task<List<string>> ConsumeSseEventStreamAsync(string sseEndpointUrl, CancellationToken cancellationToken);
    }
}
