//using System.Threading;
//using System.Threading.Tasks;
//using EasyIdentity.Models;
//using Microsoft.AspNetCore.Http;
//using Microsoft.Extensions.DependencyInjection;

//namespace EasyIdentity.Endpoints;

//public abstract class EndpointResult : IEndpointResult
//{
//    public IRequestDataCollection RequestData { get; } = null!;

//    public abstract Task ExecuteAsync(HttpContext context, CancellationToken cancellationToken = default);

//    //public virtual async Task ExecuteAsync(HttpContext context, CancellationToken cancellationToken = default)
//    //{
//    //    var executor = context.RequestServices.GetRequiredService<IEndpointResultExecutor<EndpointResult>>();
//    //    await executor.ExecuteAsync(context, this, cancellationToken);
//    //}
//}