using ContextInterfaceApplications.Web.Models;

namespace ContextInterfaceApplications.Web.Services;

public interface IStepComponentResolver
{
    Type? Resolve(string stepId, ProjectionTarget target);
}
