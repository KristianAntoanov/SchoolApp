using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using static SchoolApp.Common.ApplicationConstants;

namespace SchoolApp.Web.Areas.Admin.Controllers;

[Area(AdminRole)]
[Authorize(Roles = AdminRole)]
public class AdminBaseController : Controller
{
    
}