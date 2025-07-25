namespace SK.Rag.Application.Services.Interfaces;

public interface IChatService
{
    public Task<string> ChatAsync(string prompt);
}
