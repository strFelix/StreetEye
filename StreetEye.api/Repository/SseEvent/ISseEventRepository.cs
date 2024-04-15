namespace StreetEye.Repository.SseEvent
{
    public interface ISseEventRepository
    {
        Task<Stream> GetSseEventStreamAsync(string sseEndpointUrl, CancellationToken cancellationToken);
    }
}
