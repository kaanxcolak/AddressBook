using AddressBookBL.EmailSenderBusiness;
using AddressBookBL.InterfacesOfManagers;
using AddressBookDL.InterfacesOfRepo;
using AddressBookEL.IdentityModels;
using AddressBookPL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AddressBookPL.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICityManager _cityManager;
        private readonly IDistrictManager _districtManager;
        private readonly INeighbourhoodManager _neighbourhoodManager;
        private readonly IUserAddressManager _userAddressManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IEmailSender _emailManager;

        public IActionResult Index()
        {
            return View();
        }

    }
}