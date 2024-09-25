using EssentialCSharp.Web.Areas.Identity.Data;
using EssentialCSharp.Web.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EssentialCSharp.Web.Controllers;

public class BaseController(IReferralService referralService, UserManager<EssentialCSharpWebUser> userManager) : Controller
{
    public override ViewResult View()
    {
        string? id = userManager.GetUserId(User);
        if (id is not null)
        {
            ViewBag.ReferrerId = referralService.GetReferralIdAsync(id);
        }
        return base.View();
    }
}
