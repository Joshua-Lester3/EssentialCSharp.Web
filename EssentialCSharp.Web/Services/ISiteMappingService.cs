namespace EssentialCSharp.Web.Services;

public interface ISiteMappingService
{
    IList<SiteMapping> SiteMappings { get; }
    string GetPercentComplete(string currentPageKey);
}
