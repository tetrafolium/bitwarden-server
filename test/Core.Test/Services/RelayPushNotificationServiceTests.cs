using System;
using Bit.Core.Repositories;
using Bit.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Bit.Core.Test.Services
{
public class RelayPushNotificationServiceTests
{
    private readonly RelayPushNotificationService _sut;

    private readonly IDeviceRepository _deviceRepository;
    private readonly GlobalSettings _globalSettings;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<RelayPushNotificationService> _logger;

    public RelayPushNotificationServiceTests()
    {
        _deviceRepository = Substitute.For<IDeviceRepository>();
        _globalSettings = new GlobalSettings();
        _httpContextAccessor = Substitute.For<IHttpContextAccessor>();
        _logger = Substitute.For<ILogger<RelayPushNotificationService>>();

        _sut = new RelayPushNotificationService(
            _deviceRepository,
            _globalSettings,
            _httpContextAccessor,
            _logger
        );
    }

    // Remove this test when we add actual tests. It only proves that
    // we've properly constructed the system under test.
    [Fact(Skip = "Needs additional work")]
    public void ServiceExists()
    {
        Assert.NotNull(_sut);
    }
}
}
