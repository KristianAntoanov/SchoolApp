using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SchoolApp.Web.Controllers;

[Authorize(Roles = "Admin,Parent,Teacher")]
public class BaseController : Controller
{
	
}