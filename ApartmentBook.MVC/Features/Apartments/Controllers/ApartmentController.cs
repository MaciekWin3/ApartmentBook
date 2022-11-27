using ApartmentBook.MVC.Features.Apartments.DTOs;
using ApartmentBook.MVC.Features.Apartments.Models;
using ApartmentBook.MVC.Features.Apartments.Services;
using ApartmentBook.MVC.Features.Auth.Models;
using ApartmentBook.MVC.Features.Emails.Services;
using ApartmentBook.MVC.Features.Payments.Services;
using ApartmentBook.MVC.Features.Tenants.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace ApartmentBook.MVC.Features.Apartments.Controllers
{
    [Authorize]
    public class ApartmentsController : Controller
    {
        private readonly IApartmentService apartmentService;
        private readonly IPaymentService paymentService;
        private readonly ITenantService tenantService;
        private readonly IEmailService emailService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IMapper mapper;
        private readonly ILogger<ApartmentsController> logger;

        public ApartmentsController(IApartmentService apartmentService, IPaymentService paymentService, ITenantService tenantService,
            IEmailService emailService, UserManager<ApplicationUser> userManager, IMapper mapper, ILogger<ApartmentsController> logger)
        {
            this.apartmentService = apartmentService;
            this.paymentService = paymentService;
            this.tenantService = tenantService;
            this.emailService = emailService;
            this.userManager = userManager;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var user = await GetUser();
            var apartments = await apartmentService.GetUsersApartments(user.Id);
            return View(mapper.Map<List<ApartmentDTO>>(apartments));
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var apartment = await apartmentService.GetAsync(id);
            var payments = await paymentService.GetApartmentsPayments((Guid)id);
            ViewData["Payments"] = payments;
            if (apartment is null)
            {
                return NotFound();
            }

            return View(mapper.Map<ApartmentDTO>(apartment));
        }

        public async Task<IActionResult> Create()
        {
            var user = await GetUser();
            var tenants = await tenantService.GetUsersTenants(user.Id);
            ViewData["Tenants"] = tenants.Select(t => t.FirstName).ToList();
            return View();
        }

        // POST: Apartment/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ApartmentForCreateDTO apartamentForCreateDTO)
        {
            Apartment apartment = new();
            if (ModelState.IsValid)
            {
                apartment = mapper.Map<ApartmentForCreateDTO, Apartment>(apartamentForCreateDTO);
                apartment.Id = Guid.NewGuid();
                apartment.User = await GetUser();
                await apartmentService.CreateAsync(apartment);
                return RedirectToAction(nameof(Index));
            }
            return View(mapper.Map<Apartment>(apartment));
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var apartment = await apartmentService.GetAsync(id);
            if (apartment is null)
            {
                return NotFound();
            }
            return View(mapper.Map<ApartmentForUpdateDTO>(apartment));
        }

        // POST: Apartment/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ApartmentForUpdateDTO apartmentForUpdateDTO)
        {
            var apartmentExists = await apartmentService.GetAsync(id) is not null;
            Apartment apartment = new();
            if (ModelState.IsValid && apartmentExists)
            {
                try
                {
                    apartment = mapper.Map<Apartment>(apartmentForUpdateDTO);
                    await apartmentService.UpdateAsync(apartment);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (apartmentExists)
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
            return View(mapper.Map<ApartmentDTO>(apartment));
        }

        // GET: Apartment/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var apartment = await apartmentService.GetAsync(id);

            if (apartment is null)
            {
                return NotFound();
            }

            return View(apartment);
        }

        // POST: Apartment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var apartment = await apartmentService.GetAsync(id);
            if (apartment is not null)
            {
                await apartmentService.DeleteAsync(apartment.Id);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<bool> SendEmail(Guid id)
        {
            var apartment = await apartmentService.GetAsync(id);
            var to = apartment.Tenant.FirstOrDefault()?.Email;
            if (to is null)
            {
                logger.LogInformation("No tenant found for apartment! Email won't be send!");
                return false;
            }
            await emailService.SendEmailAsync(to, subject: "Apartment Book", message: "");

            return true;
        }

        [HttpGet]
        public async Task<JsonResult> GetChartData(Guid id)
        {
            Console.WriteLine(id);
            var data = await paymentService.GetChartData(DateTime.Now, id);
            List<object> iData = new();
            DataTable dt = new();
            dt.Columns.Add("Label", Type.GetType("System.String"));
            dt.Columns.Add("Value", Type.GetType("System.Int32"));
            foreach (var item in data)
            {
                DataRow dr = dt.NewRow();
                dr["Label"] = item.Key.ToString();
                dr["Value"] = item.Value;
                dt.Rows.Add(dr);
            }

            foreach (DataColumn dc in dt.Columns)
            {
                List<object> x = new();
                x = (from DataRow drr in dt.Rows select drr[dc.ColumnName]).ToList();
                iData.Add(x);
            }
            return Json(iData);
        }

        public IActionResult RedirectToCreatePayment(Guid id)
        {
            return RedirectToAction("Create", "Payments", new { apartmentId = id });
        }

        private async Task<ApplicationUser> GetUser()
        {
            return await userManager.GetUserAsync(HttpContext.User);
        }
    }
}