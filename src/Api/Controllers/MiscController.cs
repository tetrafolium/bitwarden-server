﻿using System;
using Microsoft.AspNetCore.Mvc;
using Bit.Core.Models.Api;
using System.Threading.Tasks;
using Bit.Core.Utilities;
using Microsoft.AspNetCore.Authorization;
using Bit.Core;
using Stripe;

namespace Bit.Api.Controllers
{
public class MiscController : Controller
{
private readonly BitPayClient _bitPayClient;
private readonly GlobalSettings _globalSettings;

public MiscController(
	BitPayClient bitPayClient,
	GlobalSettings globalSettings)
{
	_bitPayClient = bitPayClient;
	_globalSettings = globalSettings;
}

[HttpGet("~/alive")]
[HttpGet("~/now")]
public DateTime Get()
{
	return DateTime.UtcNow;
}

[HttpGet("~/version")]
public VersionResponseModel Version()
{
	return new VersionResponseModel();
}

[HttpGet("~/ip")]
public JsonResult Ip()
{
	return new JsonResult(new
			{
				Ip = HttpContext.Connection?.RemoteIpAddress?.ToString(),
				Headers = HttpContext.Request?.Headers,
			});
}

[Authorize("Application")]
[HttpPost("~/bitpay-invoice")]
[SelfHosted(NotSelfHostedOnly = true)]
public async Task<string> PostBitPayInvoice([FromBody] BitPayInvoiceRequestModel model)
{
	var invoice = await _bitPayClient.CreateInvoiceAsync(model.ToBitpayInvoice(_globalSettings));
	return invoice.Url;
}

[Authorize("Application")]
[HttpPost("~/setup-payment")]
[SelfHosted(NotSelfHostedOnly = true)]
public async Task<string> PostSetupPayment()
{
	var options = new SetupIntentCreateOptions
	{
		Usage = "off_session"
	};
	var service = new SetupIntentService();
	var setupIntent = await service.CreateAsync(options);
	return setupIntent.ClientSecret;
}
}
}
