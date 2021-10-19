using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using UrlShortener.Api;
using UrlShortener.Api.Controllers;
using UrlShortener.Api.Dtos;
using UrlShortener.Domain;
using Xunit;

namespace UrlShortener.Tests.ApiTests
{
	public class EncodeControllerTests
	{
		private readonly Mock<IUrlShortenerService> mockUrlShortenerService = new Mock<IUrlShortenerService>();

		[Fact]
		public void GivenEncodingAUrl_WhenTheRequestDoesNotContainAUrl_ThenBadRequestIsReturned()
		{
			var encodeController = new EncodeController(this.mockUrlShortenerService.Object);

			var result = encodeController.Post(new UrlDto());

			Assert.IsType<BadRequestResult>(result as BadRequestResult);
		}

		[Fact]
		public void GivenEncodingAUrl_ThenTheUrlIsShortenedViaTheUrlShortenerService()
		{
			var expectedUrl = "https://bbc.co.uk";
			var expectedScheme = "https";
			var expectedHost = "localhost";

			var mockHttpContext = new Mock<HttpContext>();
			var mockHttpRequest = new Mock<HttpRequest>();
			mockHttpContext.Setup(m => m.Request).Returns(mockHttpRequest.Object);

			mockHttpRequest.Setup(m => m.Scheme).Returns(expectedScheme);
			mockHttpRequest.Setup(m => m.Host).Returns(new HostString(expectedHost));

			var encodeController = new EncodeController(this.mockUrlShortenerService.Object)
			{
				ControllerContext = new ControllerContext
				{
					HttpContext = mockHttpContext.Object
				}
			};

			encodeController.Post(new UrlDto
			{
				Url = expectedUrl
			});

			this.mockUrlShortenerService.Verify(m => m.ShortenUrl(expectedUrl, expectedScheme, expectedHost), Times.Once);
		}

		[Fact]
		public void GivenEncodingAUrl_ThenOkObjectResultIsReturned()
		{
			var mockHttpContext = new Mock<HttpContext>();
			var mockHttpRequest = new Mock<HttpRequest>();
			mockHttpContext.Setup(m => m.Request).Returns(mockHttpRequest.Object);

			mockHttpRequest.Setup(m => m.Scheme).Returns("https");
			mockHttpRequest.Setup(m => m.Host).Returns(new HostString("localhost"));

			var encodeController = new EncodeController(this.mockUrlShortenerService.Object)
			{
				ControllerContext = new ControllerContext
				{
					HttpContext = mockHttpContext.Object
				}
			};

			var result = encodeController.Post(new UrlDto
			{
				Url = "https://bbc.co.uk"
			});

			Assert.IsType<OkObjectResult>(result as OkObjectResult);
		}

		[Fact]
		public void GivenEncodingAUrl_ThenExpectedResponseIsReturned()
		{
			var originalUrl = "https://bbc.co.uk";
			var scheme = "https";
			var host = "localhost";

			var expectedShortenedUrl = "https://localhost/testId";

			this.mockUrlShortenerService.Setup(m => m.ShortenUrl(originalUrl, scheme, host))
				.Returns(expectedShortenedUrl);

			var mockHttpContext = new Mock<HttpContext>();
			var mockHttpRequest = new Mock<HttpRequest>();
			mockHttpContext.Setup(m => m.Request).Returns(mockHttpRequest.Object);

			mockHttpRequest.Setup(m => m.Scheme).Returns(scheme);
			mockHttpRequest.Setup(m => m.Host).Returns(new HostString(host));

			var encodeController = new EncodeController(this.mockUrlShortenerService.Object)
			{
				ControllerContext = new ControllerContext
				{
					HttpContext = mockHttpContext.Object
				}
			};

			var result = encodeController.Post(new UrlDto
			{
				Url = originalUrl
			}) as OkObjectResult;

			Assert.Equal(expectedShortenedUrl, (result.Value as EncodeResponse).ShortenedUrl);
		}

		[Fact]
		public void GivenEncodingAUrl_WhenEncodeThrowsAnException_ThenBadRequestResultIsReturned()
		{
			this.mockUrlShortenerService.Setup(m => m.ShortenUrl(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
				.Throws(new InvalidUrlException(""));

			var mockHttpContext = new Mock<HttpContext>();
			var mockHttpRequest = new Mock<HttpRequest>();
			mockHttpContext.Setup(m => m.Request).Returns(mockHttpRequest.Object);

			mockHttpRequest.Setup(m => m.Scheme).Returns("https");
			mockHttpRequest.Setup(m => m.Host).Returns(new HostString("localhost"));

			var encodeController = new EncodeController(this.mockUrlShortenerService.Object)
			{
				ControllerContext = new ControllerContext
				{
					HttpContext = mockHttpContext.Object
				}
			};

			var result = encodeController.Post(new UrlDto
			{
				Url = "https://bbc.co.uk"
			});

			Assert.IsType<BadRequestResult>(result as BadRequestResult);
		}
	}
}
