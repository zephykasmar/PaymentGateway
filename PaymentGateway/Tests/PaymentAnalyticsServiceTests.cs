using Analytics.Services;
using Data.Repositories.Interfaces;
using Domain.Models;
using Domain.Models.DTOs;
using Domain.Models.Requests;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    public class PaymentsAnalyticsServiceTests
    {
        private Mock<IPaymentService> _paymentServiceMock;
        private PaymentsAnalyticsService _paymentsAnalyticsService;

        public PaymentsAnalyticsServiceTests()
        {
            _paymentServiceMock = new Mock<IPaymentService>();
            _paymentsAnalyticsService = new PaymentsAnalyticsService(_paymentServiceMock.Object);
        }

        [Fact]
        public async Task TryGetPayment_ShouldReturnExpectedPayment_WhenCalledWithValidId()
        {
            var expectedPayment = new PaymentDTO();
            _paymentServiceMock.Setup(s => s.TryGetPaymentAsync(It.IsAny<string>()))
                .ReturnsAsync(ServiceResult<PaymentDTO>.Success(expectedPayment));

            var result = await _paymentsAnalyticsService.TryGetPayment("valid-id");

            result.Result.Should().Be(expectedPayment);
        }

        [Fact]
        public async Task TryGetPayment_ShouldFail_WhenCalledWithInvalidId()
        {
            var expectedPayment = new PaymentDTO();
            _paymentServiceMock.Setup(s => s.TryGetPaymentAsync(It.IsAny<string>()))
                .ReturnsAsync(ServiceResult<PaymentDTO>.Fail(new ServiceError(ErrorTypes.PaymentNotFound)));

            var result = await _paymentsAnalyticsService.TryGetPayment("valid-id");

            result.IsSuccess.Should().BeFalse();
            result.Error.ErrorCode.Should().Be(404);
        }
    }

}
