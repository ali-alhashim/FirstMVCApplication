﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FirstMVCApplication.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using FirstMVCApplication.ViewModels;

using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;

namespace FirstMVCApplication.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<Employee> _signInManager;
        public EmployeeController(ApplicationDbContext context, SignInManager<Employee> signInManager)
        {
            _context = context;
            _signInManager = signInManager;
        }

        // GET: Employee
        public async Task<IActionResult> Index()
        {
              return _context.Employee != null ? 
                          View(await _context.Employee.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Employee'  is null.");
        }

        // GET: Employee/Details/5
        public async Task<IActionResult> Details(string? id)
        {
            if (id == null || _context.Employee == null)
            {
                return NotFound();
            }

            var employee = await _context.Employee
                .FirstOrDefaultAsync(m => m.Id == id.ToString());

            var Addresses =  _context.Address.Where(m => m.EmployeeId == id).ToList();

            if (employee == null)
            {
                return NotFound();
            }
            ViewModelEmployeeAddress viewModelEmployeeAddress = new ViewModelEmployeeAddress()
            {
                Employee = employee,
                Address  = Addresses
            };
            return View(viewModelEmployeeAddress);
        }




        // GET: Employee/Create
        public IActionResult Create()
        {
            return View();
        }




        // POST: Employee/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Email,Gender,Salary")] Employee employee)
        {
                 var hasher = new PasswordHasher<Employee>();
                 employee.UserName = employee.Email;

                //byte[] salt = RandomNumberGenerator.GetBytes(128 / 8);
                //Convert.ToBase64String(salt);
                //string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                //password: HttpContext.Request.Form["Password"],
                //salt: salt,
                //prf: KeyDerivationPrf.HMACSHA256,
                //iterationCount: 100000,
                //numBytesRequested: 256 / 8));

                employee.EmailConfirmed      = true;
                employee.NormalizedUserName = employee.UserName.ToUpper();
                employee.NormalizedEmail    = employee.Email.ToUpper();

                employee.PasswordHash       = hasher.HashPassword(null, HttpContext.Request.Form["Password"]);

                try
                {
                    _context.Add(employee);
                    await _context.SaveChangesAsync();
                }
                catch(Exception ex) {
                    Console.WriteLine("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX");
                    Console.WriteLine(ex.ToString()); 
                    
                }
              

                // add many address to the Employee

                int TotalAddress = Convert.ToInt32(HttpContext.Request.Form["TotalAddress"]);

                int i = 1;
                while(i <=  TotalAddress)
                {
                    Address address = new Address()
                    {
                         Country = HttpContext.Request.Form["Country_"+i.ToString()],
                         City    = HttpContext.Request.Form["City_" + i.ToString()],
                         Employee = employee,
                         State   = HttpContext.Request.Form["State_" + i.ToString()],
                         Pin     = HttpContext.Request.Form["Pin_" + i.ToString()]
                    };

                    _context.Add(address);
                    await _context.SaveChangesAsync();

                    i++;
                }

                return RedirectToAction(nameof(Index));
            
           
            return View(employee);
        }





        // GET: Employee/Edit/5
        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null || _context.Employee == null)
            {
                return NotFound();
            }

            var employee = await _context.Employee.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        // POST: Employee/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("EmployeeId,Name,Email,Gender,Salary")] Employee employee)
        {
            if (id != employee.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (employee.Id == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        // GET: Employee/Delete/5
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null || _context.Employee == null)
            {
                return NotFound();
            }

            var employee = await _context.Employee
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employee/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Employee == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Employee'  is null.");
            }
            var employee = await _context.Employee.FindAsync(id);
            if (employee != null)
            {
                _context.Employee.Remove(employee);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id)
        {
          return (_context.Employee?.Any(e => e.Id == id.ToString())).GetValueOrDefault();
        }



        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(string Email, string Password, bool RememberMe)
        {
            
           
                var result = await _signInManager.PasswordSignInAsync(Email, Password, RememberMe, lockoutOnFailure: true);
           
            if (result.Succeeded)
            {
                Console.WriteLine($"Welcome {Email}");
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewData["LoginResult"] = "Invalid login attempt.";
                return View();
            }

           
        }



    }
}
