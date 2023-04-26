using AddressBookEL.IdentityModels;
using AddressBookPL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AddressBookPL.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Register()
        {

            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            try
            {
                if (!ModelState.IsValid) 
                {
                    return View(model); 
                }
                //aynı username den varsa hata versin
                var sameUser = _userManager.FindByNameAsync(model.Username).Result; //async bir metodun sonuna .Result yazarsak metod senkron çalışır.
                if(sameUser != null)
                {
                    ModelState.AddModelError("", "Bu kullanıcı ismi sistemde mevcuttur! Farklı kullanıcı adı deneyiniz!");
                }
                //aynı email'den varsa hata versin
                sameUser = _userManager.FindByEmailAsync(model.Email).Result; //async bir metodun sonuna .Result yazarsak metod senkron çalışır.
                if (sameUser != null)
                {
                    ModelState.AddModelError("", "Bu kullanıcı ismi sistemde mevcuttur! Farklı kullanıcı adı deneyiniz!");
                }
                //artık sisteme kayıt olabilir
                AppUser user = new AppUser()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,                    
                    PhoneNumber = model.Phone,
                    Email = model.Email,
                    UserName = model.Username,
                    CreatedDate = DateTime.Now,
                    EmailConfirmed = true, // doğruluk yapmış kabul ettik
                    IsPassive = false                   
                };
                if (model.BirthDate != null) 
                    user.Birthdate = model.BirthDate;
                //user kaydedilsin
                var result = _userManager.CreateAsync(user,model.Password).Result;
                if (result.Succeeded)
                {
                    //Kullanıcı oluşursa ona rol ataması yapılacak !!

                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    ModelState.AddModelError("", "Ekleme başarısız!");
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                    return View(model);
                }                

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Beklenmedik hata oluştu" +ex.Message);
                return View(model);
            }

        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
    }
}
