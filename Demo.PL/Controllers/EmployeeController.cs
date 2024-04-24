using Demo.BLL.Interfaces;
using Demo.DAL.Contexts;
using Demo.DAL.Models;
using Demo.PL.MappingProfiles;
using Demo.PL.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using AutoMapper;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Demo.PL.Helpers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Demo.PL.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EmployeeController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        #region Index
        public async Task<IActionResult> Index(string SearchValue)
        {
            IEnumerable<Employee> employees;
            if (string.IsNullOrEmpty(SearchValue))
            {
                employees = await _unitOfWork.EmployeeRepository.GetAll();
            }
            else
            {
                employees = _unitOfWork.EmployeeRepository.SearchEmployeesByName(SearchValue);
            }
            var MappedEmployee = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(employees);
            return View(MappedEmployee);
        }
        #endregion

        #region Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Departments = await _unitOfWork.DepartmentRepository.GetAll();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(EmployeeViewModel employeeViewModel)
        {
            if (ModelState.IsValid) // Server Side Validation 
            {
                employeeViewModel.ImageName = DocumentSettings.UploadFile(employeeViewModel.Image, "Images");
                var MappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(employeeViewModel);
                await _unitOfWork.EmployeeRepository.Add(MappedEmployee);
                await _unitOfWork.Complete();
                return RedirectToAction(nameof(Index));
            }
            return View(employeeViewModel);
        }
        #endregion

        #region Details
        public async Task<IActionResult> Details(int? id, string ViewName = "Details")
        {

            if (id is null)
            {
                return BadRequest();
            }
            var employee = await _unitOfWork.EmployeeRepository.GetId(id.Value);
            if (employee == null)
            {
                return NotFound();
            }

            var MappedEmployee = _mapper.Map<Employee, EmployeeViewModel>(employee);

            ViewBag.departmentName = _unitOfWork.DepartmentRepository
                                                .GetId(MappedEmployee.DepartmentId.Value)
                                                .Result
                                                .Name;

            return View(ViewName, MappedEmployee);
        }
        #endregion

        #region Edit
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.Departments = await _unitOfWork.DepartmentRepository.GetAll();
            return await Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, EmployeeViewModel employeeViewModel)
        {
            if (id != employeeViewModel.Id)
            {
                return BadRequest();
            }
            try
            {
                var MappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(employeeViewModel);

                _unitOfWork.EmployeeRepository.Update(MappedEmployee);
                await _unitOfWork.Complete();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return View(employeeViewModel);
        }

        #endregion

        #region Delete
        public async Task<IActionResult> Delete(int? id)
        {
            return await Details(id, "Delete");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] int id, EmployeeViewModel employeeViewModel)
        {
            if (id != employeeViewModel.Id)
            {
                return BadRequest();
            }
            try
            {
                var MappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(employeeViewModel);

                _unitOfWork.EmployeeRepository.Delete(MappedEmployee);
                int Result = await _unitOfWork.Complete();

                if (Result > 0)
                {
                    DocumentSettings.DeleteFile(employeeViewModel.ImageName, "Images");
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return View(employeeViewModel);
        } 
        #endregion
    }
}
