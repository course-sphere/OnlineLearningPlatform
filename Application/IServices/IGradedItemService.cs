using Domain.Requests.GradedItem;
using Domain.Responses;

namespace Application.IServices
{
    public interface IGradedItemService
    {
        Task<ApiResponse> CreateNewGradedItemAsync(CreateGradedItemRequest request);
    }
}
