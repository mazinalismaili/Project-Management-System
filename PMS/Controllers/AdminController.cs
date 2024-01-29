using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PMS.DataLayer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PMS.Models;
using Microsoft.AspNetCore.Authorization;
using SQLitePCL;
using PMS.Data;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Mvc.Rendering;
using PMS.Migrations;


namespace PMS.Controllers
{
    /*[Authorize(Roles = "Administrator")]*/

    public class AdminController : Controller
    {
        private readonly DataLayer.DataLayer _dataLayer;
        private readonly ILogger<AdminController> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<Employee> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IUserStore<Employee> _userStore;
        //private readonly IUserEmailStore<Employee> _emailStore;
        private readonly SignInManager<Employee> _signInManager;




        public AdminController(UserManager<Employee> userManager,DataLayer.DataLayer dataLayer, ApplicationDbContext context, RoleManager<IdentityRole> roleManager ,ILogger<AdminController> logger, IUserStore<Employee> userStore, SignInManager<Employee> signInManager)
        {
            _dataLayer = dataLayer;
            _roleManager = roleManager;
            _userManager = userManager;
            _logger = logger;
            _context = context;
            _userStore = userStore;
            _signInManager = signInManager;
        }

        // GET: AdminController
        public async Task<ActionResult> Index()
        {
            // Main Admin Page

            var manager = _userManager.Users.Where(i => i.isManager == true);
            
            var users = await _dataLayer.GetUsersAsync(); // get all users
            var roles = await _dataLayer.GetRolesAsync(); // get all roles
            var statuses = await _dataLayer.GetStatusesAsync();
            var priorities = await _dataLayer.GetPrioritiesAsync();

            /*var userRole = await _dataLayer.GetUsersRoles();*/
            List<UserRolesModel> userRoles = new List<UserRolesModel>();
            foreach(var user in users)
            {
                var _userRoles = await _userManager.GetRolesAsync(user);
                userRoles.Add(new UserRolesModel { Employee =  user , Roles = _userRoles});
            }

            AdminHomeModel model = new AdminHomeModel();

            var userModel = users.Select(user => new UserModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
            }).ToList();

            var roleModel = roles.Select(role => new Role
            {
                Id= role.Id,
                Name = role.Name,
            }).ToList();

            var statusModel = statuses.Select(status => new Status
            {
                Id = status.Id,
                Name = status.Name,
            }).ToList();

            var priorityModel = priorities?.Select(priority => new Priority
            {
                Id = priority.Id,
                Name = priority.Name,
            }).ToList();


            model.Users = userModel;
            model.Roles = roleModel;
            model.Statuses = statusModel;
            model.Priorities = priorityModel;
            model.UserRoles = userRoles;


            ViewData["Project"] = _context.Project.Count();
            ViewData["Task"] = _context.Task.Count();
            // this is comment to test github


            return View(model);
        }

        // GET: AdminController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AdminController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string name)//, IFormCollection collection)
        {
                if (await _dataLayer.CreateNewRole(name))
                {
                    Console.WriteLine("New Role Added Sucsussfly.");
                    return RedirectToAction("Index");
                }
            return View();
        }

        // GET: AdminController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AdminController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string Id,string name)
        {
            IdentityRole role = await _roleManager.FindByIdAsync(Id);
            role.Name = name;

            await _roleManager.UpdateAsync(role);
           // if (await _dataLayer.EditRole(role, name)) {
           //     Console.WriteLine("Changed!"); 
           // }

            return RedirectToAction("Index");
        }

        // GET: AdminController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AdminController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string name, IFormCollection collection)
        {
            try
            {
                if (await _dataLayer.DeleteRole(name))
                {
                    Console.WriteLine("Deleted Sucssefuly!");

                    return RedirectToAction(nameof(Index));
                }
                return View()
;            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateStatus(IFormCollection collection)
        {
            string statusName = collection["Name"].ToString();
            if (await _context.Status.AnyAsync(s => s.Name == statusName))
            {
                return RedirectToAction("Index");
            }
            
            if (await _dataLayer.CreateStatus(statusName)) {
               
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditStatus(string id, string name)
        {
           if(await _dataLayer.EditStatus(id,name))
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteStatus(string id)
        {
            if(await _dataLayer.DeleteStatus(id))
            {
                return RedirectToAction("Index");
            }
            
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePriority(IFormCollection collection) {

            if(ModelState.IsValid && !collection["name"].IsNullOrEmpty()) {
                string name = collection["name"].ToString();
                if (await _dataLayer.CreatePriority(name))
                {
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPriority(IFormCollection collection)
        {
            if (ModelState.IsValid)
            {
                string id = collection["id"].ToString();
                string name = collection["name"].ToString();

                if (await _dataLayer.EditPriority(id, name)) { 
                return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePriority(IFormCollection collection) { 
            if (ModelState.IsValid) {
                string id = collection["id"].ToString();
                if (await _dataLayer.DeletePriority(id))
                {
                    return RedirectToAction("Index");
                }
            }
        return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteRoleFromUser(string id,string name)
        {
            Employee employee = await _userManager.FindByIdAsync(id);
            if (ModelState.IsValid && employee != null)
            {
                IdentityResult result = await _userManager.RemoveFromRoleAsync(employee,name);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> addRoleToUser(string userid, string rolename)
        {
            Employee employee = await _userManager.FindByIdAsync(userid);
            if (ModelState.IsValid && employee !=null)
            {
                IdentityResult result = await _userManager.AddToRoleAsync(employee,rolename);
            }

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            return View(user);
        }

        public async Task<ActionResult> EditProfile(string Id)
        {
            var user = await _userManager.FindByIdAsync(Id);
            ViewData["ManagerId"] = new SelectList(_context.Set<Employee>().Where(i => i.isManager == true), "Id", "UserName");

            return View(user);
        }
        /*
                [HttpPost]
                [ValidateAntiForgeryToken]
                public async Task<IActionResult> EditProfile([Bind("Id,UserName,Email,FirstName,LastName,ManagerId")] Employee employee)
                {
                    ModelState.Remove("Tasks");
                    ModelState.Remove("Comments");
                    ModelState.Remove("Projects");
                    *//*            ModelState.Remove("");
                                ModelState.Remove("");*//*


                    if (ModelState.IsValid)
                    {
                        Employee emp = await _userManager.FindByIdAsync(employee.Id);
                        if (emp != null)
                        {
                            *//*await _userStore.SetUserNameAsync(emp, employee.UserName, CancellationToken.None);
                            await _emailStore.SetEmailAsync(emp, employee.Email, CancellationToken.None);*//*
                            await _userManager.SetUserNameAsync(emp, employee.UserName);
                            await _userManager.SetEmailAsync(emp, employee.Email);
                            //await _userManager.ChangeEmailAsync(emp, employee.Email, CancellationToken.None);
                            *//*emp.UserName = employee.UserName;
                            emp.Email = employee.Email;*//*
                            emp.FirstName = employee.FirstName;
                            emp.LastName = employee.LastName;
                            emp.ManagerId = employee.ManagerId;
                        }
                        await _userManager.UpdateAsync(emp);
                       //IdentityResult result = await _userManager.UpdateAsync(employee);
                        *//*await _context.SaveChangesAsync();*//*
                    }
                    return RedirectToAction("Profile");
                }*/

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile([Bind("Id,UserName,Email,FirstName,LastName,ManagerId")] Employee employee)
        {
            ModelState.Remove("Tasks");
            ModelState.Remove("Comments");
            ModelState.Remove("Projects");

            if (ModelState.IsValid)
            {
                Employee emp = await _userManager.FindByIdAsync(employee.Id);

                if (emp != null)
                {
                    var oldUserName = emp.UserName;

                    emp.UserName = employee.UserName;
                    emp.Email = employee.Email;
                    emp.FirstName = employee.FirstName;
                    emp.LastName = employee.LastName;
                    emp.ManagerId = employee.ManagerId;

                    // Update user properties
                    var result = await _userManager.UpdateAsync(emp);

                    if (result.Succeeded)
                    {
                        // If UserName has changed, update the authentication cookie
                        if (!string.Equals(oldUserName, employee.UserName, StringComparison.OrdinalIgnoreCase))
                        {
                            await _signInManager.RefreshSignInAsync(emp);
                            await _signInManager.SignOutAsync();
                            await _signInManager.SignInAsync(emp, isPersistent: false);
                        }

                        return RedirectToAction("Profile");
                    }
                    else
                    {
                        // Handle errors
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
            }

            // If ModelState is not valid or if there are errors, return to the view
            return View("Profile", employee);
        }

    }
}
