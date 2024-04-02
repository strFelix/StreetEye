namespace StreetEye.Exceptions
{
    public sealed class SseRequestException(HttpResponseMessage response) : Exception($"\"Failed to connect to SSE endpoint. Status code: {response.StatusCode}\"")
    {
    }
}
