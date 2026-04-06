using Microsoft.AspNetCore.Components;

namespace ContextInterfaceApplications.Web.Services;

public interface IActionComponentResolver
{
    Type Resolve(string sourceComponent);
}
