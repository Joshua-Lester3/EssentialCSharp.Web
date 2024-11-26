using System.Globalization;

namespace EssentialCSharp.Web.Services;

public class SiteMappingService : ISiteMappingService
{
    public IList<SiteMapping> SiteMappings { get; }

    public SiteMappingService(IWebHostEnvironment webHostEnvironment)
    {
        string path = Path.Combine(webHostEnvironment.ContentRootPath, "Chapters", "sitemap.json");
        List<SiteMapping>? siteMappings = System.Text.Json.JsonSerializer.Deserialize<List<SiteMapping>>(File.OpenRead(path)) ?? throw new InvalidOperationException("No table of contents found");
        SiteMappings = siteMappings;
    }

    public string GetPercentComplete(string currentPageKey)
    {
        int currentMappingCount = 1;
        int overallMappingCount = 1;
        bool currentPageFound = false;
        IEnumerable<IGrouping<int, SiteMapping>> chapterGroupings = SiteMappings.GroupBy(x => x.ChapterNumber).OrderBy(g => g.Key);
        foreach (IGrouping<int, SiteMapping> chapterGrouping in chapterGroupings)
        {
            IEnumerable<IGrouping<int, SiteMapping>> pageGroupings = chapterGrouping.GroupBy(x => x.PageNumber).OrderBy(g => g.Key);
            foreach (IGrouping<int, SiteMapping> pageGrouping in pageGroupings)
            {
                foreach (SiteMapping siteMapping in pageGrouping)
                {
                    if (siteMapping.Key == currentPageKey)
                    {
                        currentPageFound = true;
                    }
                    if (!currentPageFound)
                    {
                        currentMappingCount++;
                    }
                    overallMappingCount++;
                }
            }
        }
        double result = (double)currentMappingCount / overallMappingCount * 100;
        return string.Format(CultureInfo.InvariantCulture, "{0:0.00}", result);
    }
}
