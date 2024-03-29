﻿using ApartmentBook.MVC.Features.Apartments.Services;
using ApartmentBook.MVC.Features.Auth.Models;
using ApartmentBook.MVC.Features.Payments.DTOs;
using ApartmentBook.MVC.Features.Payments.Models;
using ApartmentBook.MVC.Features.Payments.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApartmentBook.MVC.Features.Payments.Controllers
{
    [Authorize]
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
            return View(mapper.Map<List<PaymentDTO>>(payments));
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

            return View(mapper.Map<PaymentDTO>(payment));
        }

        // GET: Payments/Create
        public async Task<IActionResult> Create(Guid apartmentId)
        {
            var user = await GetUser();
            var apartment = await apartmentService.GetAsync(apartmentId);
            if (apartment is null)
            {
                var apartments = await apartmentService.GetUsersApartments(user.Id);
                ViewData["Apartments"] = apartments.Select(a => a.Name).ToList();
                return View(new PaymentForCreateDTO { });
            }
            var paymentForCreateDTO = new PaymentForCreateDTO
            {
                ApartmentId = apartment?.Id.ToString() ?? string.Empty,
            };
            return View(paymentForCreateDTO);
        }

        // POST: Payments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PaymentForCreateDTO paymentForCreateDTO)
        {
            Payment payment = new();
            if (ModelState.IsValid)
            {
                payment = mapper.Map<PaymentForCreateDTO, Payment>(paymentForCreateDTO);
                payment.Id = Guid.NewGuid();
                payment.User = await GetUser();
                payment.Apartment = await apartmentService.GetAsync(Guid.Parse(paymentForCreateDTO.ApartmentId));
                await paymentService.CreateAsync(payment);
                return RedirectToAction("Details", "Apartments", new { id = paymentForCreateDTO.ApartmentId });
            }
            return View(mapper.Map<PaymentForCreateDTO>(payment));
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
            return RedirectToAction("Details", "Apartments", new { id = payment.Apartment.Id });
        }

        // POST: Payments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Type,Amount,AmountPaid")] PaymentForUpdateDTO paymentForUpdateDTO)
        {
            var paymentExists = await paymentService.GetAsync(id) is not null;
            Payment payment = new();

            if (ModelState.IsValid)
            {
                try
                {
                    payment = mapper.Map<Payment>(paymentForUpdateDTO);
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

        public async Task<IActionResult> PayPayment(Guid id)
        {
            var apartmentId = await paymentService.PayPaymentAndReturnApartmentId(id);
            return RedirectToAction("Details", "Apartments", new { id = apartmentId });
        }

        private async Task<ApplicationUser> GetUser()
        {
            return await userManager.GetUserAsync(HttpContext.User);
        }
    }
}