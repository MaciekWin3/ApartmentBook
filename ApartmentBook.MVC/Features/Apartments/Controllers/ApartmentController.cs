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
using Newtonsoft.Json;

namespace ApartmentBook.MVC.Features.Apartments.Controllers
{
    [Authorize]
    public class ApartmentsController : Controller
    {
        private readonly IApartmentService apartmentService;
        private readonly IPaymentService paymentService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IMapper mapper;

        public ApartmentsController(IApartmentService apartmentService, IPaymentService paymentService,
            UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            this.apartmentService = apartmentService;
            this.paymentService = paymentService;
            this.userManager = userManager;
            this.mapper = mapper;
        }

        // GET: Apartment
        public async Task<IActionResult> Index()
        {
            var user = await GetUser();
            var apartments = await apartmentService.GetUsersApartments(user.Id);
            return View(apartments);
        }

        // GET: Apartment/Details/5
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

            return View(apartment);
        }

        // GET: Apartment/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Apartment/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Street,Building,Flat,Rent")] ApartmentForCreateDTO apartamentForCreateDTO)
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
            return View(apartment);
        }

        // GET: Apartment/Edit/5
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
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,Street,Building,Flat,Rent")] Apartment apartment)
        {
            if (id != apartment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
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