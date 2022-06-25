using ApartmentBook.MVC.Features.Apartments.DTOs;
using ApartmentBook.MVC.Features.Apartments.Models;
using ApartmentBook.MVC.Features.Apartments.Services;
using ApartmentBook.MVC.Features.Auth.Models;
using ApartmentBook.MVC.Features.Emails;
using ApartmentBook.MVC.Features.Payments.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Data;

// Fix bug - Controller name
namespace ApartmentBook.MVC.Features.Apartments.Controllers
{
    [Authorize]
    public class ApartmentsController : Controller
    {
        private readonly IApartmentService apartmentService;
        private readonly IPaymentService paymentService;
        private readonly IEmailService emailService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IMapper mapper;

        public ApartmentsController(IApartmentService apartmentService, IPaymentService paymentService,
            IEmailService emailService, UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            this.apartmentService = apartmentService;
            this.paymentService = paymentService;
            this.emailService = emailService;
            this.userManager = userManager;
            this.mapper = mapper;
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

        public IActionResult Create()
        {
            ViewData["Tenants"] = new List<string>() { "Maciek", "Filip", "Karol" };
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
            return View(mapper.Map<ApartmentDTO>(apartment));
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
            var to = apartment.Tenant.FirstOrDefault().Email;
            //await emailService.SendEmailAsync(to);

            return true;
        }

        [HttpGet]
        public async Task<JsonResult> NewChart(Guid id)
        {
            Console.WriteLine(id);
            var data = await paymentService.GetChartData(DateTime.Now, id);
            List<object> iData = new List<object>();
            DataTable dt = new DataTable();
            dt.Columns.Add("Label", System.Type.GetType("System.String"));
            dt.Columns.Add("Value", System.Type.GetType("System.Int32"));
            foreach (var item in data)
            {
                DataRow dr = dt.NewRow();
                dr["Label"] = item.Key.ToString();
                dr["Value"] = item.Value;
                dt.Rows.Add(dr);
            }

            foreach (DataColumn dc in dt.Columns)
            {
                List<object> x = new List<object>();
                x = (from DataRow drr in dt.Rows select drr[dc.ColumnName]).ToList();
                iData.Add(x);
            }
            return Json(iData);
        }

        public async Task<IActionResult> RedirectToCreatePayment(Guid id)
        {
            TempData["Apartament"] = JsonConvert.SerializeObject(await apartmentService.GetAsync(id));
            TempData.Keep("Apartament");
            return RedirectToAction("Create", "Payments");
        }

        private async Task<ApplicationUser> GetUser()
        {
            return await userManager.GetUserAsync(HttpContext.User);
        }
    }
}