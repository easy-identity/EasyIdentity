using System.Threading.Tasks;
using EasyIdentity.Endpoints;

namespace EasyIdentity.Services;

public interface IEndpointResultResponseProvider
{
    Task ExecuteAsync(IEndpointResult endpointResult);
}
