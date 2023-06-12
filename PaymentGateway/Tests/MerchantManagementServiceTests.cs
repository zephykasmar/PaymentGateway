using Data.Repositories.Interfaces;
using Domain.Models.Entities;
using Domain.Models;
using Fulfilment.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;

namespace Tests
{
    public class MerchantManagementServiceTests
    {
        private Mock<ICacheService> _cacheMock;
        private Mock<IMerchantService> _merchantServiceMock;

        private MerchantManagementService _merchantManagementService;
        private Dictionary<string, string> _cacheMock2 = new Dictionary<string, string>();

        public MerchantManagementServiceTests()
        {
            _cacheMock = new Mock<ICacheService>();
            _merchantServiceMock = new Mock<IMerchantService>();

            _merchantManagementService = new MerchantManagementService(_cacheMock.Object, _merchantServiceMock.Object);
        }

        [Fact]
        public async Task ValidateMerchant_ShouldReturnSuccess_WhenMerchantIsInCache()
        {
            _cacheMock.Setup(s => s.TryGet(It.IsAny<string>()))
                .ReturnsAsync(ServiceResult<string>.Success("merchant-in-cache"));
            var result = await _merchantManagementService.ValidateMerchant("merchant-id");
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task ValidateMerchant_ShouldReturnSuccess_WhenMerchantIsNotInCacheButExistsInDb()
        {
            _cacheMock.Setup(s => s.TryGet(It.IsAny<string>()))
                .ReturnsAsync(ServiceResult<string>.Fail(new ServiceError(ErrorTypes.MerchantNotFound)));
            _merchantServiceMock.Setup(s => s.TryGetMerchantAsync(It.IsAny<string>()))
                .ReturnsAsync(ServiceResult<Merchant>.Success(new Merchant()));
            _cacheMock.Setup(s => s.TrySet(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(ServiceResult<bool>.Success(true));
            var result = await _merchantManagementService.ValidateMerchant("merchant-id");
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task ValidateMerchant_ShouldReturnFail_WhenMerchantDoesNotExist()
        {
            _cacheMock.Setup(s => s.TryGet(It.IsAny<string>()))
                .ReturnsAsync(ServiceResult<string>.Fail(new ServiceError(ErrorTypes.MerchantNotFound)));
            _merchantServiceMock.Setup(s => s.TryGetMerchantAsync(It.IsAny<string>()))
                .ReturnsAsync(ServiceResult<Merchant>.Fail(new ServiceError(ErrorTypes.MerchantNotFound)));
            var result = await _merchantManagementService.ValidateMerchant("merchant-id");
            result.IsSuccess.Should().BeFalse();
            result.Error.ErrorCode.Should().Be(404);
        }
    }

}
