using Microsoft.AspNetCore.Mvc;
using System;
using UrlShortener.Domain;

namespace UrlShortener.Api.Controllers
{
	public class FollowController : ControllerBase
	{
		private readonly IUrlShortenerService urlShortenerService;

		public FollowController(IUrlShortenerService urlShortenerService)
		{
			this.urlShortenerService = urlShortenerService ?? throw new ArgumentNullException(nameof(urlShortenerService));
		}

		public IActionResult Follow()
		{
			var url = this.urlShortenerService.GetOriginalUrl(
				$"{this.HttpContext.Request.Scheme}://{this.HttpContext.Request.Host}/{this.HttpContext.Request.Path}");

			if (url == null)
			{
				return new BadRequestResult();
			}

			return new RedirectResult(url);
		}
	}
}
