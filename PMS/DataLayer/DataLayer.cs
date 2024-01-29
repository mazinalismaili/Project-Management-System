using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PMS.Data;
using PMS.Models;
using SQLitePCL;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMS.DataLayer
{
    public class DataLayer
    {
        private readonly UserManager<Employee> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public DataLayer(UserManager<Employee> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public async Task<List<IdentityRole>> GetRolesAsync()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return roles;
        }
        public async Task<List<Employee>> GetUsersAsync()
        {

            var users = await _userManager.Users.ToListAsync();
            return users;
        }
        public async Task<Employee?> GetUserAsync(string Id)
        {
            var user = await _userManager.FindByIdAsync(Id);
            if (user == null)
            {
                return null;
            }
            return user;
        }
        public async Task<IdentityRole?> GetRoleAsync(string Id)
        {
            var role = await _roleManager.FindByIdAsync(Id);
            if (role == null) return null;
            return role;
        }
        /*public List<string UserId,string RoleId> GetUsersRoles()
        {

        }*/
        public async Task<bool> DeleteRole(string name) {
            // check if the role exist or return false
            if (!await _roleManager.RoleExistsAsync(name)){ 
                return false;
            }
            var role = await _roleManager.FindByNameAsync(name);
            IdentityResult result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded) {
                return true;
            }
            return false;
        }
        public async Task<bool> EditRole(IdentityRole role ,string name)
        {
            // if role does not exist: return false -> can't edit non-existed thing!

            IdentityResult result = await _roleManager.SetRoleNameAsync(role,name);
            if (result.Succeeded)
            {
                //await dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public async Task<bool> CreateNewRole(string name)
        {
            if(await _roleManager.RoleExistsAsync(name)) {
                Console.WriteLine("Role {0} already exist.", name);
                return true;
            }

            IdentityResult result = await _roleManager.CreateAsync(new IdentityRole { Name = name });
            if (result.Succeeded) { 
                return true;
            }
            return false;
        }
        public async Task<List<Status>> GetStatusesAsync()
        {
            var statuses = await _context.Status.ToListAsync();

            return statuses;

        }
        public async Task<bool> CreateStatus(string status)
        {
            Status Status = new Status()
            {
                Id = Guid.NewGuid().ToString(),
                Name = status,
            };
            _context.Add(Status);
            await _context.SaveChangesAsync();
            
            return true;
        }
        public async Task<bool> DeleteStatus(string id)
        {
            if (await _context.Status.AnyAsync(s => s.Id == id))
            {
                var status = await _context.Status.FindAsync(id);
                _context.Remove(status);
                await _context.SaveChangesAsync();

                return true;

            }
            return false;
        }
        internal async Task<bool> EditStatus(string id, string name)
        {
            if (!await _context.Status.AnyAsync(s => s.Name == name))
            {
                var status = await _context.Status.FindAsync(id);
                if (status != null)
                {
                    status.Name = name;
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            return false;
        }
        public async Task<List<Priority>?> GetPrioritiesAsync()
        {
            var priorities = await _context.Priority.ToListAsync();
            if (priorities.Any())
            {
                return priorities;
            }
            return null;
        }
        public async Task<Priority?> GetPriorityAsync(string id)
        {
            if (!id.IsNullOrEmpty())
            {
                var priority = await _context.Priority.FindAsync(id);
                return priority;
            }
            return null;
        }
        public async Task<bool> CreatePriority(string priority)
        {
            if (!priority.IsNullOrEmpty())
            {
                Priority Priority = new Priority()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = priority
                };
                _context.Add(Priority);
                await _context.SaveChangesAsync();

                return true;
            }
            return false;
        }
        public async Task<bool> DeletePriority(string id)
        {
            if (!id.IsNullOrEmpty() && await _context.Priority.AnyAsync(i => i.Id == id))
            {
                var priority = await _context.Priority.FindAsync(id);
                _context.Remove(priority);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public async Task<bool> EditPriority(string id, string name)
        {
            if (!name.IsNullOrEmpty() && !await _context.Status.AnyAsync(s => s.Name == name))
            {
                var status = await _context.Status.FindAsync(id);
                if (status != null)
                {
                    status.Name = name;
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            return false;
        }

    }
}
