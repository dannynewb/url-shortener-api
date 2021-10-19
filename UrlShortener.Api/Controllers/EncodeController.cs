using Microsoft.AspNetCore.Mvc;
using System;
using UrlShortener.Api.Dtos;
using UrlShortener.Domain;

namespace UrlShortener.Api.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class EncodeController : ControllerBase
	{
		private readonly IUrlShortenerService urlShortenerService;

		public EncodeController(IUrlShortenerService urlShortenerService)
		{
			this.urlShortenerService = urlShortenerService ?? throw new ArgumentNullException(nameof(urlShortenerService));
		}

		[HttpPost]
		public IActionResult Post(UrlDto url)
		{
			if (string.IsNullOrEmpty(url.Url))
			{
				return new BadRequestResult();
			}

			try
			{
				var shortenedUrl = this.urlShortenerService.ShortenUrl(url.Url, this.HttpContext.Request.Scheme, this.HttpContext.Request.Host.ToUriComponent());

				return new OkObjectResult(new EncodeResponse(shortenedUrl));
			}
			catch (Exception)
			{
				return new BadRequestResult();
			}
		}
	}
}
