using Expense.Helpers;
using Expense.Models;
using Expense.Models.ViewModel;
using Expense.Repository.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Expense.Controllers
{
    public class UserController : Controller
    {
        private readonly IPhotoService _photoservice;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IUserRepo _userRepo;   
        public UserController(IPhotoService photo,IHttpContextAccessor ht,UserManager<AppUser> manager,SignInManager<AppUser> sign, IUserRepo repo) {
        
        _contextAccessor = ht;
            _photoservice = photo;
            _userManager = manager;
            _signInManager = sign;
            _userRepo = repo;   
        } 


        public async Task<IActionResult> Profile()
        {
            var currentuserid = _contextAccessor.HttpContext.User.GetUserId();
            var currentuser = await _userRepo.Get(currentuserid);
            return View(currentuser);
        }
        public IActionResult Login() {
            var login = new LoginViewModel();
            return View(login); 
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var checkmail = await _userManager.FindByEmailAsync(model.Email);
            if (checkmail != null)
            {
                var checkpassword = await _userManager.CheckPasswordAsync(checkmail, model.Password);

                if (checkpassword)
                {
                    var response = await _signInManager.PasswordSignInAsync(checkmail, model.Password, false, false);
                    if (response.Succeeded)
                        return RedirectToAction("Index", "Home");
                }
                TempData["Error"] = "Incorrect Details";

                return View(model);
            }
            TempData["Error"] = "User Doesnot Exist";
            return View(model);
        }
        
        public async Task<IActionResult> Logout()
        {
           await _signInManager.SignOutAsync();
           
            return RedirectToAction("Index","DashBoard");   
        }
        
        public IActionResult Register() {
            var register = new RegisterViewModel();
            return View(register);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }
            var checkusermail = await _userManager.FindByEmailAsync(model.Email);
            if(checkusermail!=null)
            {
                TempData["Error"] = "User already Registered";
                return View(model);
            }
            var user = new AppUser
            {
                Email=model.Email,
                UserName=model.Name,
            };
            var response = await _userManager.CreateAsync(user,model.Password);
            return RedirectToAction("Login", "User");
        }


        public async Task<IActionResult> Edit()
        {
            var currentUserId = _contextAccessor.HttpContext.User.GetUserId();
            var currentuser = await _userRepo.Get(currentUserId);
            var register = new EditViewModel();
            register.Name=currentuser.UserName; 
            register.Street=currentuser.Street;
            register.City=currentuser.City;
            register.Country=currentuser.Country;
            register.Number=currentuser.Number;
            register.ProfileImg=currentuser.ProfileImg;
            return View(register);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var currentUserId = _contextAccessor.HttpContext.User.GetUserId();
            var currentuser = await _userRepo.Get(currentUserId);
            currentuser.UserName = model.Name;
            currentuser.Street=model.Street;
            currentuser.City=model.City;
            currentuser.Country=model.Country;
            currentuser.PhoneNumber = model.Number;

            if(model.Image!=null)
            {    
                if(model.ProfileImg!=null)
                 await _photoservice.DeletePhotoAsync(model.ProfileImg);
                var insert = await _photoservice.AddPhotoAsync(model.Image);
                var inserturl = insert.Url.ToString();
                currentuser.ProfileImg=inserturl;
            }
             _userRepo.Update(currentuser);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult PasswordChange()
        {
            var register = new ChangeViewModel();

            return View(register);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PasswordChange(ChangeViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var currentuserid = _contextAccessor.HttpContext.User.GetUserId();
            var currentuser = await _userRepo.Get(currentuserid);

            var oldpasswordchecking = await _userManager.CheckPasswordAsync(currentuser,model.CurrentPassword);
            if (oldpasswordchecking) {

                var removepassword= await _userManager.RemovePasswordAsync(currentuser);
                if(removepassword.Succeeded)
                {
                    var addingpassword = await _userManager.AddPasswordAsync(currentuser,model.NewPassword);

                    if(addingpassword.Succeeded)
                    {
                        return RedirectToAction("Index","Home");
                    }
                    TempData["Error"] = "Cant Change Password";
                    await _userManager.AddPasswordAsync(currentuser,model.CurrentPassword);
                    return View(model);
                }
                await _userManager.AddPasswordAsync(currentuser, model.CurrentPassword);
                TempData["Error"] = "Cant Change Password";
                return View(model);
            }
            TempData["Error"] = "Incorrec old password";
            return View(model);
        }

        public IActionResult ForgotPassword()
        {
            var register = new ForgotViewModel();
            return View(register);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotViewModel model)
        {
            if(!ModelState.IsValid)
                return View(model);
            var checkmail = await _userManager.FindByEmailAsync(model.Email);
            if(checkmail!=null)
            {
                var deletepassdword = await _userManager.RemovePasswordAsync(checkmail);
                if(deletepassdword.Succeeded)
                {
                    var addnewpassword = await _userManager.AddPasswordAsync(checkmail,model.NewPassword);
                    if(addnewpassword.Succeeded)
                    {
                        return RedirectToAction("Index","Home");
                    }
                    TempData["Error"] = "can't create new Password";
                    return View(model);
                }
                TempData["Error"] = "can't create new Password ";
                return View(model);
            }
            TempData["Error"] = "Email Doesn't Exists";
            return View(model);
        }

        public IActionResult All()
        {
            return View();
        }
    }
}
