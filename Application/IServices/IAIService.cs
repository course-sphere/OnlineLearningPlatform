namespace Application.IServices
{
    public interface IAIService
    {
        Task<string> GetAIResponseAsync(string prompt);
    }
}
