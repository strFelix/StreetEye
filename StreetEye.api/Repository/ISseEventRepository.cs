namespace StreetEye.Repository
{
    public interface ISseEventRepository
    {
        Task<System.IO.Stream> GetSseEventStreamAsync(string sseEndpointUrl, CancellationToken cancellationToken);
    }
}
