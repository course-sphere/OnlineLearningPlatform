namespace Application.IServices
{
    public interface IOllamaService
    {
        Task<string> GetAIResponseAsync(string prompt);
    }
}
