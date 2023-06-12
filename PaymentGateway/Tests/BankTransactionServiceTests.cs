using Domain.Models.DTOs;
using Domain.Models;
using Domain.Models.Requests;
using FluentAssertions;
using Fulfilment.Services;
using Moq;
using System.Net.Http.Json;
using Moq.Protected;
using System.Net;
using Moq.Contrib.HttpClient;

namespace Tests
{
    public class BankTransactionServiceTests
    {
        private IBankTransactionService _bankTransactionService;
        private Mock<HttpMessageHandler> _handler;

        public BankTransactionServiceTests()
        {
            _handler = new Mock<HttpMessageHandler>();
            var client = _handler.CreateClient();
            client.BaseAddress = new Uri("http://localhost");
            _bankTransactionService = new BankTransactionService(client);
        }

        [Fact]
        public async Task ProcessBankTransaction_ShouldReturnSuccess_WhenCalledWithAnyPayment()
        {
            _handler.SetupAnyRequest()
                .ReturnsResponse(HttpStatusCode.OK);

            var result = await _bankTransactionService.Process(new AcquiringBankPaymentRequest());
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task ProcessBankTransaction_ShouldFail_WhenCalledWithAnyPayment()
        {
            _handler.SetupAnyRequest()
                .ReturnsResponse(HttpStatusCode.OK);

            var result = await _bankTransactionService.Process(new AcquiringBankPaymentRequest());
            result.IsSuccess.Should().BeTrue();
        }
    }

}
