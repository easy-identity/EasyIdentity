//using System;
//using System.Reflection;
//using System.Threading;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Http;
//using Microsoft.Extensions.DependencyInjection;

//namespace EasyIdentity.Endpoints;

//public class DefaultEndpointResultExecutor : IEndpointResultExecutor
//{
//    public virtual async Task ExecuteAsync(HttpContext context, IEndpointResult result, CancellationToken cancellationToken = default)
//    {
//        Console.WriteLine(result.GetType());

//        var executorType = typeof(IEndpointResultExecutor<>).MakeGenericType(result.GetType());

//        // var executor =  Activator.CreateInstance(executorType);

//        var tmp = context.RequestServices.GetService (executorType);

//        // Console.WriteLine(executor.GetType());
//        Console.WriteLine(executorType.GetType());

//        Console.WriteLine(tmp.GetType());
//        var iii = tmp.GetType().GetInterfaces();

//        Console.WriteLine(typeof(IEndpointResultExecutor<IEndpointResult>).IsAssignableFrom(tmp.GetType()));

//        //if (!(context.RequestServices.GetService(executorType) is IEndpointResultExecutor<IEndpointResult> executor))
//        //{
//        //    throw new System.Exception($"Result type '{result.GetType()}' executor not found.");
//        //}

//        //await executor.ExecuteAsync(context, result, cancellationToken);

//        throw new NotImplementedException();
//    }
//}
