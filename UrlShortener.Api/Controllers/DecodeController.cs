using Microsoft.AspNetCore.Mvc;
using System;
using UrlShortener.Api.Dtos;
using UrlShortener.Domain;

namespace UrlShortener.Api.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class DecodeController : ControllerBase
	{
		private readonly IUrlShortenerService urlShortenerService;

		public DecodeController(IUrlShortenerService urlShortenerService)
		{
			this.urlShortenerService = urlShortenerService ?? throw new ArgumentNullException(nameof(urlShortenerService));
		}

		[HttpPost]
		public IActionResult Post(ShortenedUrlDto shortenedUrl)
		{
			if (string.IsNullOrEmpty(shortenedUrl.ShortenedUrl))
			{
				return new BadRequestResult();
			}

			var url = this.urlShortenerService.GetOriginalUrl(shortenedUrl.ShortenedUrl);
			if (string.IsNullOrEmpty(url))
			{
				return new NotFoundResult();
			}

			return new OkObjectResult(new DecodeResponse(url));
		}
	}
}
