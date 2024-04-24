using Demo.DAL.Models;
using Demo.PL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Demo.PL.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace Demo.PL.Controllers
{
    [Authorize]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public RoleController(RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
            _roleManager = roleManager;
            _mapper = mapper;
        }



        #region Index
        public async Task<IActionResult> Index(string SearchValue)
        {
            if (string.IsNullOrEmpty(SearchValue))
            {
               
                var Roles = await _roleManager.Roles.Select(R => new RoleViewModel()
                {
                  Id = R.Id,
                  RoleName = R.Name,
                }).ToListAsync();
                
                return View(Roles);
            }
            else
            {
                var Role = await _roleManager.FindByNameAsync(SearchValue);
                if (Role is not null)
                {
                    var MappedRole = new RoleViewModel
                    {
                        Id = Role.Id,
                        RoleName = Role.Name,
                    };
                    return View(new List<RoleViewModel> { MappedRole });
                }
                else
                {
                    return View(Enumerable.Empty<RoleViewModel>());
                }

            }
        }
        #endregion

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RoleViewModel RoleVm)
        {
            if (ModelState.IsValid) // Server Side Validation 
            {
               
                var MappedRole = _mapper.Map<RoleViewModel, IdentityRole>(RoleVm);
                await _roleManager.CreateAsync(MappedRole);
                return RedirectToAction(nameof(Index));
            }
            return View(RoleVm);
        }


        #region Details
        public async Task<IActionResult> Details(string Id, string ViewName = "Details")
        {
            var Role = await _roleManager.FindByIdAsync(Id);
            var MappedUser = _mapper.Map<IdentityRole, RoleViewModel>(Role);

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
        public async Task<IActionResult> Edit([FromRoute] string Id, RoleViewModel UpdatedRole)
        {
            if (Id != UpdatedRole.Id)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var Role = await _roleManager.FindByIdAsync(Id);
                    Role.Name = UpdatedRole.RoleName;
                    
                    //var MappedUser = _mapper.Map<UserViewModel, AppUser>(model);
                    await _roleManager.UpdateAsync(Role);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }

            }
            return View(UpdatedRole);
        }

        #endregion

        #region Delete
        public async Task<IActionResult> Delete(string Id)
        {
            return await Details(Id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] string Id, RoleViewModel DeletedRole)
        {
            if (Id != DeletedRole.Id)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var Role = await _roleManager.FindByIdAsync(Id);

                    //var MappedUser = _mapper.Map<UserViewModel, AppUser>(DeletedUser);
                    await _roleManager.DeleteAsync(Role);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }

            }
            return View(DeletedRole);
        }
        #endregion
    }
}
