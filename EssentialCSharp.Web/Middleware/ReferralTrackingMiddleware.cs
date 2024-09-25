using System.Security.Claims;
using EssentialCSharp.Web.Areas.Identity.Data;
using EssentialCSharp.Web.Services;
using Microsoft.AspNetCore.Identity;

namespace EssentialCSharp.Web.Middleware;

public class ReferralMiddleware
{
    private readonly RequestDelegate _Next;

    public ReferralMiddleware(RequestDelegate next)
    {
        _Next = next;
    }

    public async Task InvokeAsync(HttpContext context, IReferralService referralService, UserManager<EssentialCSharpWebUser> userManager)
    {
        // Retrieve current referral Id for processing
        string referralId = context.Request.Query["rid"].ToString();


        if (context.User is { } claimsUser && claimsUser.Identity is not null && claimsUser.Identity.IsAuthenticated)
        {
            await TrackReferralAsync(referralService, referralId, claimsUser);

            // Add the referralId to the request context if it exists on a user
            EssentialCSharpWebUser? user = await userManager.GetUserAsync(claimsUser);
            if (user is not null)
            {
                context.Items["rid"] = await referralService.GetReferralIdAsync(user.Id);
            }
        }
        else
        {
            await TrackReferralAsync(referralService, referralId, null);
        }

        await _Next(context);

        static async Task TrackReferralAsync(IReferralService referralService, string? referralId, ClaimsPrincipal? claimsUser)
        {
            if (!string.IsNullOrWhiteSpace(referralId))
            {
                _ = await referralService.TrackReferralAsync(referralId, claimsUser);
            }
        }
    }
}
