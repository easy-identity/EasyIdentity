using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace EasyIdentity.Extensions;

public static class HttpResponseExtensions
{
    public static void SetNoCache(this HttpResponse response)
    {
        if (!response.Headers.ContainsKey("Cache-Control"))
        {
            response.Headers.Append("Cache-Control", "no-store, no-cache, max-age=0");
        }
        else
        {
            response.Headers["Cache-Control"] = "no-store, no-cache, max-age=0";
        }

        if (!response.Headers.ContainsKey("Pragma"))
        {
            response.Headers.Append("Pragma", "no-cache");
        }
    }

    public static async Task WriteHtmlAsync(this HttpResponse response, string html)
    {
        response.ContentType = "text/html; charset=UTF-8";
        await response.WriteAsync(html, Encoding.UTF8);
        await response.Body.FlushAsync();
    }
}
