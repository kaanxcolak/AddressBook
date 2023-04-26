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
                    // kullanıcıya customer rolünü atayalım
                    var roleResult = _userManager.AddToRoleAsync(user, "Customer").Result;
                    if (roleResult.Succeeded)
                    {
                        TempData["RegisterSuccessMsg"] = "Kayıt Başarılı!";
                    }
                    else
                    {
                        TempData["RegisterWarningMsg"] = "Kullanıcı oluştu! Ancak rolü atanamadı! Sistem yöneticisine ulaşarak rol ataması yapılmalıdır!";

                    }

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

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                
                var user = _userManager.FindByNameAsync(model.UsernameOrEmail).Result;
                if (user == null)
                {
                    user = _userManager.FindByEmailAsync(model.UsernameOrEmail).Result;
                }
                if (user == null)
                {
                    ModelState.AddModelError("", "Kullanıcı Adı/Email ya da şifreniz hatalıdır!");
                    
                    return View(model);
                }
                //user'ı buldu
                var logininresult = _signInManager.PasswordSignInAsync(user, model.Password, true).Result;

                //sen hatalı mısın?
                //Eğer başarısız  giriş yaparsa başarısız olduğunun sayısını kaydediyoruz!
                user.AccessFailedCount++;
                //eğer başarısızlık 2 olduysa kilitlenecek
                if (user.AccessFailedCount == 2)
                {
                    user.LockoutEnd = DateTime.Now.AddMinutes(2);
                    ModelState.AddModelError("", $"Hatalı giriş sayısı 2 olmuştur! xxx kadar sonra tekrar deneyiniz!");
                }
                var updateResult = _userManager.UpdateAsync(user).Result;
                if (user.AccessFailedCount == 2)
                {
                    var d = user.LockoutEnd;
                    return View(model);
                    
                }
                return View(model);
                
            }

            catch (Exception ex)
            {
                ModelState.AddModelError("", "Beklenmedik bir hata oluştu!");   //"","" deki ilk çift tırnak herhangi bir yerde genel gelen mesaj için. Gerekirse key veririz oraya
                return View(model);
            }
        }
    }
}
