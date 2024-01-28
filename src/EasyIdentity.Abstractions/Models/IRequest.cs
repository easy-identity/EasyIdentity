namespace EasyIdentity.Models;

public interface IRequest
{
    string Url { get; set; }
    string Method { get; }
    IRequestCollection Data { get; }
    RequestAuthorization Authorization { get; }
}
