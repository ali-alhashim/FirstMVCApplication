using System;
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

namespace FirstMVCApplication.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmployeeController(ApplicationDbContext context)
        {
            _context = context;
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
          
                employee.UserName = employee.Email;

                byte[] salt = RandomNumberGenerator.GetBytes(128 / 8);
                Convert.ToBase64String(salt);
                string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: HttpContext.Request.Form["Password"],
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

                employee.EmailConfirmed = true;
                employee.NormalizedUserName = employee.UserName.ToLower();
                employee.NormalizedEmail = employee.Email.ToLower();

                employee.PasswordHash = hashedPassword;

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
        public async Task<IActionResult> Edit(int? id)
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
        public async Task<IActionResult> Edit(int id, [Bind("EmployeeId,Name,Email,Gender,Salary")] Employee employee)
        {
            if (id.ToString() != employee.Id)
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
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Employee == null)
            {
                return NotFound();
            }

            var employee = await _context.Employee
                .FirstOrDefaultAsync(m => m.Id == id.ToString());
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employee/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
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
        public IActionResult Login(string Email, string Password)
        {
            return View();
        }



    }
}
