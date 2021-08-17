using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Models;
using Dapper;


namespace WebApp.Controllers
{
    public class EmployeeController : Controller
    {
        EmpRepository _db;
        public EmployeeController(EmpRepository db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            var emp = (from empModel in _db.GetAllEmployee()
                       where empModel.EmployeeId > 0
                       select new EmpModel
                       {
                           EmployeeId = empModel.EmployeeId,
                           Name = empModel.Name,
                           Gender = empModel.Gender,
                           Department = empModel.Department
                       }).ToList();
            return View(emp);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(EmpModel model)
        {
            try
            {
                ModelState.Remove("EmployeeId");
                if (ModelState.IsValid)
                {
                    _db.AddEmployee(model);
                    return RedirectToAction("Index");
                }
            }

            catch (Exception ex)
            {

            }
            return View(model);
        }

        public IActionResult Edit(int id)
        {
            EmpModel employee = _db.GetEmployeeData(id);
            return View("Create", employee);
        }

        [HttpPost]
        public IActionResult Edit(EmpModel employee)
        {
            try
            {
                _db.UpdateEmployee(employee);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {

            }
            return View("Create", employee);

        }

        public IActionResult Delete(int id)
        {

            EmpModel employee = _db.GetEmployeeData(id);
            if (employee != null)
            {
                _db.DeleteEmployee(id);
            }
            return RedirectToAction("Index");
        }
    }
}
