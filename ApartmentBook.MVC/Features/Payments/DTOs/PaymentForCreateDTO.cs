﻿using ApartmentBook.MVC.Features.Payments.Models;
using System.ComponentModel.DataAnnotations;

namespace ApartmentBook.MVC.Features.Payments.DTOs
{
    public record PaymentForCreateDTO
    {
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }

        [DataType(DataType.Currency)]
        public decimal AmountPaid { get; set; } = 0;

        public PaymentType Type { get; set; }
        public Month PaymentMonth { get; set; }
        public int PaymentYear { get; set; }
        public Guid UserId { get; set; }

        // Apartment Data | was Guid
        public string ApartmentId { get; set; }

        public string ApartmentName { get; set; }
    }
}