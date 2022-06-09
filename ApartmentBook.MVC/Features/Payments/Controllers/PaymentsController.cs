using ApartmentBook.MVC.Features.Apartments.Models;
using ApartmentBook.MVC.Features.Apartments.Services;
using ApartmentBook.MVC.Features.Auth.Models;
using ApartmentBook.MVC.Features.Payments.Models;
using ApartmentBook.MVC.Features.Payments.Services;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ApartmentBook.MVC.Features.Payments.Controllers
{
    public class PaymentsController : Controller
    {
        private readonly IPaymentService paymentService;
        private readonly IApartmentService apartmentService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IMapper mapper;

        public PaymentsController(IPaymentService paymentService, IApartmentService apartmentService,
            UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            this.paymentService = paymentService;
            this.apartmentService = apartmentService;
            this.userManager = userManager;
            this.mapper = mapper;
        }

        // GET: Payments
        public async Task<IActionResult> Index()
        {
            var user = await GetUser();
            var payments = await paymentService.GetUsersPayments(user.Id);
            return View(payments);
        }

        // GET: Payments/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var payment = await paymentService.GetAsync(id);
            if (payment is null)
            {
                return NotFound();
            }

            return View(payment);
        }

        // GET: Payments/Create
        public async Task<IActionResult> Create()
        {
            var apartment = JsonConvert.DeserializeObject<Apartment>(TempData["Apartament"].ToString());
            TempData.Keep("Apartament");
            // Mapping obiektu do dto i w view przesłanie go :)
            ViewData["Apartment"] = apartment;
            return View();
        }

        // POST: Payments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Type,Amount,AmountPaid")] Payment payment)
        {
            if (ModelState.IsValid)
            {
                payment.Id = Guid.NewGuid();
                // Checking for apartment
                payment.User = await GetUser();
                await paymentService.CreateAsync(payment);
                return RedirectToAction(nameof(Index));
            }
            return View(payment);
        }

        // GET: Payments/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var payment = await paymentService.GetAsync(id);
            if (payment is null)
            {
                return NotFound();
            }
            return View(payment);
        }

        // POST: Payments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Type,Amount,AmountPaid")] Payment payment)
        {
            if (id != payment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await paymentService.UpdateAsync(payment);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (await paymentService.GetAsync(id) is null)
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
            return View(payment);
        }

        // GET: Payments/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var payment = await paymentService.GetAsync(id);

            if (payment == null)
            {
                return NotFound();
            }

            return View(payment);
        }

        // POST: Payments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var payment = await paymentService.GetAsync(id);
            if (payment is not null)
            {
                await paymentService.DeleteAsync(payment.Id);
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task<ApplicationUser> GetUser()
        {
            return await userManager.GetUserAsync(HttpContext.User);
        }
    }
}