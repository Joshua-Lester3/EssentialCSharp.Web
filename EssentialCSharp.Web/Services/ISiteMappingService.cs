﻿using EssentialCSharp.Web.Models;

namespace EssentialCSharp.Web.Services;

public interface ISiteMappingService
{
    IList<SiteMapping> SiteMappings { get; }
}
