using CKOBank.Services;
using Domain.Models.DTOs;
using Domain.Models.Entities;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    public class TransactionProcessingServiceTests
    {
        private TransactionProcessingService _transactionProcessingService;

        public TransactionProcessingServiceTests()
        {
            _transactionProcessingService = new TransactionProcessingService();
        }

        [Fact]
        public async Task ProcessTransaction_ShouldReturnSuccess_WhenCalledWithAnyPayment()
        {
            var result = await _transactionProcessingService.ProcessTransaction(new PaymentDTO());
            result.IsSuccess.Should().BeTrue();
        }
    }

}
