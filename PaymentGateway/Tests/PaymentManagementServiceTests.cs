using Data.Repositories.Interfaces;
using Domain.Models;
using Fulfilment.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models.Requests;
using FluentAssertions;
using Domain.Models.DTOs;
using Domain.Models.Entities;

namespace Tests
{
    public class PaymentManagementServiceTests
    {
        private Mock<IPaymentService> _paymentServiceMock;
        private Mock<IMerchantManagementService> _merchantsServiceMock;
        private Mock<IBankTransactionService> _bankServiceMock;

        private PaymentManagementService _paymentManagementService;

        public PaymentManagementServiceTests()
        {
            _paymentServiceMock = new Mock<IPaymentService>();
            _merchantsServiceMock = new Mock<IMerchantManagementService>();
            _bankServiceMock = new Mock<IBankTransactionService>();

            _paymentManagementService = new PaymentManagementService(
                _paymentServiceMock.Object,
                _merchantsServiceMock.Object,
                _bankServiceMock.Object);
        }

        [Fact]
        public async Task ProcessPaymentAsync_ShouldReturnSuccessfulPayment_WhenAllDependenciesSucceed()
        {
            var expectedPayment = new PaymentDTO { PAN = "8929ac9c-a8a6-4810-a470-db7372109d81" }; 
            _merchantsServiceMock.Setup(s => s.ValidateMerchant(It.IsAny<string>()))
                .ReturnsAsync(ServiceResult<bool>.Success(true));
            _bankServiceMock.Setup(s => s.Process(It.IsAny<AcquiringBankPaymentRequest>()))
                .ReturnsAsync(ServiceResult<bool>.Success(true));
            _paymentServiceMock.Setup(s => s.TryInsertPaymentAsync(It.IsAny<Payment>()))
                .ReturnsAsync(ServiceResult<PaymentDTO>.Success(expectedPayment));

            var result = await _paymentManagementService.ProcessPaymentAsync(expectedPayment);

            result.IsSuccess.Should().BeTrue();
            result.Result.Should().Be(expectedPayment);
        }

        [Fact]
        public async Task ProcessPaymentAsync_ShouldFail_WhenMerchantDoesNotExist()
        {
            var expectedPayment = new PaymentDTO { PAN = "8929ac9c-a8a6-4810-a470-db7372109d81" }; 
            _merchantsServiceMock.Setup(s => s.ValidateMerchant(It.IsAny<string>()))
                .ReturnsAsync(ServiceResult<bool>.Fail(new ServiceError(ErrorTypes.MerchantNotFound)));

            var result = await _paymentManagementService.ProcessPaymentAsync(expectedPayment);

            result.IsSuccess.Should().BeFalse();
            result.Error.ErrorCode.Should().Be(404);
        }

        [Fact]
        public async Task ProcessPaymentAsync_ShouldFail_WhenBankServiceCallFails()
        {
            var expectedPayment = new PaymentDTO { PAN = "8929ac9c-a8a6-4810-a470-db7372109d81" };
            _merchantsServiceMock.Setup(s => s.ValidateMerchant(It.IsAny<string>()))
                .ReturnsAsync(ServiceResult<bool>.Success(true));
            _bankServiceMock.Setup(s => s.Process(It.IsAny<AcquiringBankPaymentRequest>()))
                .ReturnsAsync(ServiceResult<bool>.Fail(new ServiceError(ErrorTypes.InternalServerError)));

            var result = await _paymentManagementService.ProcessPaymentAsync(expectedPayment);

            result.IsSuccess.Should().BeFalse();
            result.Error.ErrorCode.Should().Be(500);
        }

        [Fact]
        public async Task ProcessPaymentAsync_ShouldFail_WhenPaymentInsertFails()
        {
            var expectedPayment = new PaymentDTO { PAN = "8929ac9c-a8a6-4810-a470-db7372109d81" };
            _merchantsServiceMock.Setup(s => s.ValidateMerchant(It.IsAny<string>()))
                .ReturnsAsync(ServiceResult<bool>.Success(true));
            _bankServiceMock.Setup(s => s.Process(It.IsAny<AcquiringBankPaymentRequest>()))
                .ReturnsAsync(ServiceResult<bool>.Success(true));
            _paymentServiceMock.Setup(s => s.TryInsertPaymentAsync(It.IsAny<Payment>()))
                .ReturnsAsync(ServiceResult<PaymentDTO>.Fail(new ServiceError(ErrorTypes.InternalServerError)));

            var result = await _paymentManagementService.ProcessPaymentAsync(expectedPayment);

            result.IsSuccess.Should().BeFalse();
            result.Error.ErrorCode.Should().Be(500);
        }
    }
}
