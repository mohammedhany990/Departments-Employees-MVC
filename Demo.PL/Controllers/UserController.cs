using AutoMapper;
using Demo.DAL.Models;
using Demo.PL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IMapper _mapper;
        public UserController(UserManager<AppUser> userManager,
                              SignInManager<AppUser> signInManager,
                              IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
        }

        #region Index
        public async Task<IActionResult> Index(string SearchValue)
        {
            if (string.IsNullOrEmpty(SearchValue))
            {
                var Users = _userManager.Users;
                var mapped = _mapper.Map<IEnumerable<AppUser>, IEnumerable< UserViewModel>>(Users);
                
                // auto mapping
                /*
                var Users = await _userManager.Users.Select(U => new UserViewModel()
                {
                    Id = U.Id,
                    FName = U.FName,
                    LName = U.LName,
                    Email = U.Email,
                    PhoneNumber = U.PhoneNumber,
                    UserRoles = _userManager.GetRolesAsync(U).Result,
                }).ToListAsync();
                */
                return View(mapped);
            }
            else
            {
                var User = await _userManager.FindByEmailAsync(SearchValue);
                if (User is not null)
                {
                    var MappedUser = new UserViewModel
                    {
                        Id = User.Id,
                        FName = User.FName,
                        LName = User.LName,
                        Email = User.Email,
                        PhoneNumber = User.PhoneNumber,
                        UserRoles = _userManager.GetRolesAsync(User).Result,
                    };
                    return View(new List<UserViewModel> { MappedUser });
                }
                else
                {
                    return View(Enumerable.Empty<UserViewModel>());
                }

            }
        } 
        #endregion

        #region Details
        public async Task<IActionResult> Details(string Id, string ViewName = "Details")
        {
            var User = await _userManager.FindByIdAsync(Id);
            var MappedUser = _mapper.Map<AppUser, UserViewModel>(User);

            return View(ViewName, MappedUser);
        } 
        #endregion

        #region Edit
        public async Task<IActionResult> Edit(string Id)
        {
            return await Details(Id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] string Id, UserViewModel UpdatedUser)
        {
            if (Id != UpdatedUser.Id)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var User = await _userManager.FindByIdAsync(Id);
                    User.FName = UpdatedUser.FName;
                    User.LName = UpdatedUser.LName;
                    User.PhoneNumber = UpdatedUser.PhoneNumber;
                    //var MappedUser = _mapper.Map<UserViewModel, AppUser>(model);
                    await _userManager.UpdateAsync(User);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }

            }
            return View(UpdatedUser);
        }

        #endregion

        #region Delete
        public async Task<IActionResult> Delete(string Id)
        {
            return await Details(Id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] string Id, UserViewModel DeletedUser)
        {
            if (Id != DeletedUser.Id)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var User = await _userManager.FindByIdAsync(Id);

                    //var MappedUser = _mapper.Map<UserViewModel, AppUser>(DeletedUser);
                    await _userManager.DeleteAsync(User);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }

            }
            return View(DeletedUser);
        } 
        #endregion
    }
}
