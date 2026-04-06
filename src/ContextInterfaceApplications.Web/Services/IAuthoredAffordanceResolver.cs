using ContextInterfaceApplications.Web.Models;

namespace ContextInterfaceApplications.Web.Services;

public interface IAuthoredAffordanceResolver
{
    IReadOnlyList<AuthoredToolContract> GetTools(string stepId);
    IReadOnlyList<AuthoredActionContract> GetActions(string stepId);
}
