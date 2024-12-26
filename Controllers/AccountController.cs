using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GESTIONCOMMANDES.Models.Entities;
using GESTIONCOMMANDES.data;
using Microsoft.AspNetCore.Identity;
using GESTIONCOMMANDES.enums;

namespace GESTIONCOMMANDES.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<User> SignInManager;
        private readonly UserManager<User> UserManager;

        private readonly AppDbContext _context;

        public AccountController(AppDbContext context, SignInManager<User> signInManager, UserManager<User> userManager)
        {
            this.SignInManager = signInManager;
            this.UserManager = userManager;
            _context = context;
        }


        // GET: User
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }
            }
            return View(model);
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Créer l'utilisateur
                User user = new User
                {
                    Nom = model.Nom,
                    Prenom = model.Prenom,
                    Email = model.Email,
                    UserName = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    Role = Role.CLIENT
                };

                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    Client client = new Client
                    {
                        Nom = model.Nom,
                        Prenom = model.Prenom,
                        Telephone = model.PhoneNumber,
                        Solde = 0,
                        Adresse = model.Adresse,
                        User = user
                    };

                    _context.Clients.Add(client);
                    await _context.SaveChangesAsync();

                    await SignInManager.SignInAsync(user, isPersistent: false);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View(model);
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await SignInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
