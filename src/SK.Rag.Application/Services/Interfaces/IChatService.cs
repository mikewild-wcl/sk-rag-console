namespace SK.Rag.Application.Services.Interfaces;

public interface IChatService
{
    public Task<string> Chat(string prompt, CancellationToken cancellationToken = default);

    IAsyncEnumerable<string> GetResponseAsync(string userMessage, CancellationToken cancellationToken = default);
}
