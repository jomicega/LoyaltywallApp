using Loyaltywall.Prism.Responses;
using System.Threading.Tasks;

namespace Loyaltywall.Prism.Services
{
    public interface IApiService
    {
        Task<Response> GetPonitsAsync<T>(string urlBase, string servicePrefix, string controller, T model);

        Task<Response> GetPonitsexpiredAsync(string urlBase, string servicePrefix, string controller);

        Task<Response> GetListAllPointsAsync(string urlBase, string servicePrefix, string controller);

        Task<Response> GetListAccumulatedPonitsAsync<T>(string urlBase, string servicePrefix, string controller, T model);

        Task<Response> GetListconsumedPonitsAsync<T>(string urlBase, string servicePrefix, string controller, T model);
    }
}
