using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;

namespace EasyIdentity;

public static class ResponseHelper
{
    public static string BuilderFormPostHtml(string url, IEnumerable<KeyValuePair<string, string?>> values)
    {
        var forms = values.Where(x => !string.IsNullOrWhiteSpace(x.Value)).Select(x => $"<input type='hide' name='{x.Key}' value='{HtmlEncoder.Default.Encode(x.Value!)}' />");

        url = HtmlEncoder.Default.Encode(url);

        return $"<html><head><meta http-equiv='X-UA-Compatible' content='IE=edge' /><base target='_self'/><title>Loading...</title></head><body><form method='post' action='{url}'>{forms}<noscript><button>Click to continue</button></noscript></form><script>window.addEventListener('load', function(){{document.forms[0].submit(); }});</script></body></html>";
    }
}
