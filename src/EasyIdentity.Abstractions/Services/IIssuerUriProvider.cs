using System;
using System.Threading.Tasks;

namespace EasyIdentity.Services;

public interface IIssuerUriProvider
{
    Task<Uri> GetAsync();
}
