using ApartmentBook.MVC.Features.Auth.Models;
using ApartmentBook.MVC.Features.Tenants.DTOs;
using ApartmentBook.MVC.Features.Tenants.Models;
using ApartmentBook.MVC.Features.Tenants.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApartmentBook.MVC.Features.Tenants.Controllers
{
    [Authorize]
    public class TenantsController : Controller
    {
        private readonly ITenantService tenantService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IMapper mapper;

        public TenantsController(ITenantService tenantService,
            UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            this.tenantService = tenantService;
            this.userManager = userManager;
            this.mapper = mapper;
        }

        // GET: Tenants
        public async Task<IActionResult> Index()
        {
            var user = await GetUser();
            var tenants = await tenantService.GetUsersTenants(user.Id);
            return View(mapper.Map<List<TenantDTO>>(tenants));
        }

        // GET: Tenants/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id is null)
            {
                return NotFound();
            }
            var tenant = await tenantService.GetAsync(id);
            if (tenant is null)
            {
                return NotFound();
            }

            return View(mapper.Map<TenantDTO>(tenant));
        }

        // GET: Tenants/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tenants/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName,Surname,PhoneNumber,Email,RoomatesAmount,Notes")] TenantForCreateDTO tenantDTO)
        {
            Tenant tenant = new();
            if (ModelState.IsValid)
            {
                tenant = mapper.Map<Tenant>(tenantDTO);
                tenant.Id = Guid.NewGuid();
                tenant.User = await GetUser();
                //tenant.Apartment =
                await tenantService.CreateAsync(tenant);
                return RedirectToAction(nameof(Index));
            }
            return View(tenant);
        }

        // GET: Tenants/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var tenant = await tenantService.GetAsync(id);
            if (tenant is null)
            {
                return NotFound();
            }
            return View(tenant);
        }

        // POST: Tenants/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("FirstName,Surname,PhoneNumber,Email,RoomatesAmount,Notes")] TenantForUpdateDTO tenantForUpdateDTO)
        {
            var tenantExists = await tenantService.GetAsync(id) is not null;
            Tenant tenant = new();
            if (ModelState.IsValid && tenantExists)
            {
                try
                {
                    tenant = mapper.Map<Tenant>(tenantForUpdateDTO);
                    await tenantService.UpdateAsync(tenant);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (tenantExists is not true)
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
            return View(mapper.Map<TenantDTO>(tenant));
        }

        // GET: Tenants/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id is null)
            {
                return NotFound();
            }
            var tenant = await tenantService.GetAsync(id);
            if (tenant is null)
            {
                return NotFound();
            }
            // Map to DTO?
            return View(tenant);
        }

        // POST: Tenants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var tenant = await tenantService.GetAsync(id);
            if (tenant is not null)
            {
                await tenantService.DeleteAsync(tenant.Id);
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task<ApplicationUser> GetUser()
        {
            return await userManager.GetUserAsync(HttpContext.User);
        }
    }
}