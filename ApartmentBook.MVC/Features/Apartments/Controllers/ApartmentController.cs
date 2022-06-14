using ApartmentBook.MVC.Features.Apartments.DTOs;
using ApartmentBook.MVC.Features.Apartments.Models;
using ApartmentBook.MVC.Features.Apartments.Services;
using ApartmentBook.MVC.Features.Auth.Models;
using ApartmentBook.MVC.Features.Payments.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Data;

namespace ApartmentBook.MVC.Features.Apartments.Controllers
{
    [Authorize]
    public class ApartmentsController : Controller
    {
        private readonly IApartmentService apartmentService;
        private readonly IPaymentService paymentService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IMapper mapper;
        private readonly IDistributedCache cache;

        public ApartmentsController(IApartmentService apartmentService, IPaymentService paymentService,
            UserManager<ApplicationUser> userManager, IMapper mapper, IDistributedCache cache)
        {
            this.apartmentService = apartmentService;
            this.paymentService = paymentService;
            this.userManager = userManager;
            this.mapper = mapper;
            this.cache = cache;
        }

        public async Task<IActionResult> Index()
        {
            var user = await GetUser();

            List<Apartment> apartments = new();

            var cachedApartments = cache.GetString("apartmentList");
            if (!string.IsNullOrEmpty(cachedApartments))
            {
                apartments = JsonConvert.DeserializeObject<List<Apartment>>(cachedApartments);
            }
            else
            {
                apartments = await apartmentService.GetUsersApartments(user.Id);
                DistributedCacheEntryOptions options = new();
                options.SetAbsoluteExpiration(new TimeSpan(0, 0, 30));
                cache.SetString("apartmentList", JsonConvert.SerializeObject(apartments, Formatting.Indented,
                    new JsonSerializerSettings
                    {
                        PreserveReferencesHandling = PreserveReferencesHandling.Objects
                    }), options);
            }

            return View(apartments);
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
            ViewData["ChartData"] = await paymentService.GetChartData(DateTime.Now, apartment.Id);
            if (apartment is null)
            {
                return NotFound();
            }

            return View(apartment);
        }

        public IActionResult Create()
        {
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
                await cache.RemoveAsync("apartmentList");
                apartment = mapper.Map<ApartmentForCreateDTO, Apartment>(apartamentForCreateDTO);
                apartment.Id = Guid.NewGuid();
                apartment.User = await GetUser();
                await apartmentService.CreateAsync(apartment);
                return RedirectToAction(nameof(Index));
            }
            return View(apartment);
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
            return View(apartment);
        }

        // POST: Apartment/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Apartment apartment)
        {
            if (id != apartment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await cache.RemoveAsync("apartmentList");
                    await apartmentService.UpdateAsync(apartment);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (await apartmentService.GetAsync(id) is null)
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
            return View(apartment);
        }

        // GET: Apartment/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var apartment = await apartmentService.GetAsync(id);

            if (apartment == null)
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

            //Looping and extracting each DataColumn to List<Object>
            foreach (DataColumn dc in dt.Columns)
            {
                List<object> x = new List<object>();
                x = (from DataRow drr in dt.Rows select drr[dc.ColumnName]).ToList();
                iData.Add(x);
            }
            //Source data returned as JSON
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