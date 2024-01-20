using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCBooksWebApi.Data;
using MVCBooksWebApi.Models;
using MVCBooksWebApi.Models.Domain;
using System;
using System.Threading.Tasks;

namespace MVCBooksWebApi.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly MvcBooksDbContext _mvcBooksDbContext;

        public UsersController(MvcBooksDbContext mvcBooksDbContext)
        {
            _mvcBooksDbContext = mvcBooksDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = await _mvcBooksDbContext.Users.ToListAsync();
            return View(users);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AddUserViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                // Check if a user with the same username already exists
                var existingUser = await _mvcBooksDbContext.Users.FirstOrDefaultAsync(u => u.Username == userViewModel.Username);

                if (existingUser != null)
                {
                    // Display an error message
                    TempData["ErrorMessage"] = "Użytkownik o takim loginie już istnieje";
                    return View(userViewModel);
                }

                var newUser = new User
                {
                    Username = userViewModel.Username,
                    Password = userViewModel.Password,
                    Role = userViewModel.Role
                };

                await _mvcBooksDbContext.Users.AddAsync(newUser);
                await _mvcBooksDbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(userViewModel);
        }


        [HttpGet]
        public async Task<IActionResult> View(int id)
        {
            var user = await _mvcBooksDbContext.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (user != null)
            {
                var updateUserViewModel = new UpdateUserViewModel
                {
                    Id = user.Id,
                    Username = user.Username,
                    Password = user.Password,
                    Role = user.Role
                };

                return View("View", updateUserViewModel);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> View(UpdateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _mvcBooksDbContext.Users.FindAsync(model.Id);

                if (user != null)
                {
                    user.Username = model.Username;
                    user.Password = model.Password;
                    user.Role = model.Role;

                    await _mvcBooksDbContext.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(UpdateUserViewModel model)
        {
            var user = await _mvcBooksDbContext.Users.FindAsync(model.Id);

            if (user != null)
            {
                _mvcBooksDbContext.Users.Remove(user);
                await _mvcBooksDbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }
    }
}
