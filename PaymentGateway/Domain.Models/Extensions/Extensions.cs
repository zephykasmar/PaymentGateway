using Domain.Models.DTOs;
using Domain.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Extensions
{
    public static class Extensions
    {
        public static Payment ToPayment(this PaymentDTO dto)
        {
            return new Payment
            {
                TransactionId = dto.TransactionId,
                MerchantId = dto.MerchantId,
                CardholdersName = dto.CardholdersName,
                PAN = Utils.PANUtils.MaskPan(dto.PAN),
                Expiry = dto.Expiry,
                Amount = dto.Amount,
                CurrencyCode = dto.CurrencyCode,
            };
        }

        public static PaymentDTO ToPaymentDTO(this Payment d)
        {
            return new PaymentDTO
            {
                TransactionId = d.TransactionId,
                MerchantId = d.MerchantId,
                CardholdersName = d.CardholdersName,
                PAN = Utils.PANUtils.MaskPan(d.PAN),
                Expiry = d.Expiry,
                Amount = d.Amount,
                CurrencyCode = d.CurrencyCode,
            };
        }
    }
}
