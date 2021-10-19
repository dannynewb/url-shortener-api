using Microsoft.AspNetCore.Mvc;
using Moq;
using UrlShortener.Api;
using UrlShortener.Api.Controllers;
using UrlShortener.Api.Dtos;
using UrlShortener.Domain;
using Xunit;

namespace UrlShortener.Tests.ApiTests
{
	public class DecodeControllerTests
	{
		private readonly Mock<IUrlShortenerService> mockUrlShortenerService = new Mock<IUrlShortenerService>();

		[Fact]
		public void GivenDecodingAUrl_WhenTheRequestDoesNotContainAUrl_ThenBadRequestIsReturned()
		{
			var decodeController = new DecodeController(this.mockUrlShortenerService.Object);

			var result = decodeController.Post(new ShortenedUrlDto());

			Assert.IsType<BadRequestResult>(result as BadRequestResult);
		}

		[Fact]
		public void GivenDecodingAUrl_WhenOriginalIsNotFound_ThenNotFoundResultIsReturned()
		{
			var decodeController = new DecodeController(this.mockUrlShortenerService.Object);

			var result = decodeController.Post(new ShortenedUrlDto
			{
				ShortenedUrl = "https://localhost.com/TestId"
			});

			Assert.IsType<NotFoundResult>(result as NotFoundResult);
		}

		[Fact]
		public void GivenDecodingAUrl_ThenOriginalUrlIsObtainedFromTheUrlShortenerService()
		{
			var expectedShortenedUrl = "https://localhost.com/TestId";

			var decodeController = new DecodeController(this.mockUrlShortenerService.Object);

			decodeController.Post(new ShortenedUrlDto
			{
				ShortenedUrl = expectedShortenedUrl
			});

			this.mockUrlShortenerService.Verify(m => m.GetOriginalUrl(expectedShortenedUrl), Times.Once);
		}

		[Fact]
		public void GivenDecodingAUrl_ThenOkObjectResultIsReturned()
		{
			var shortenedUrl = "https://localhost.com/TestId";
			var expectedOriginalUrl = "https://bbc.co.uk";

			this.mockUrlShortenerService.Setup(m => m.GetOriginalUrl(shortenedUrl))
				.Returns(expectedOriginalUrl);

			var decodeController = new DecodeController(this.mockUrlShortenerService.Object);

			var result = decodeController.Post(new ShortenedUrlDto
			{
				ShortenedUrl = shortenedUrl
			});

			Assert.IsType<OkObjectResult>(result as OkObjectResult);
		}

		[Fact]
		public void GivenDecodingAUrl_ThenExpectedResponseIsReturned()
		{
			var shortenedUrl = "https://localhost.com/TestId";
			var expectedOriginalUrl = "https://bbc.co.uk";

			this.mockUrlShortenerService.Setup(m => m.GetOriginalUrl(shortenedUrl))
				.Returns(expectedOriginalUrl);

			var decodeController = new DecodeController(this.mockUrlShortenerService.Object);

			var result = decodeController.Post(new ShortenedUrlDto
			{
				ShortenedUrl = shortenedUrl
			}) as OkObjectResult;

			Assert.Equal(expectedOriginalUrl, (result.Value as DecodeResponse).OriginalUrl);
		}
	}
}
