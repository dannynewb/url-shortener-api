using Moq;
using System;
using UrlShortener.Api;
using UrlShortener.Domain;
using Xunit;

namespace UrlShortener.Tests.DomainTests
{
	public class UrlShortenerTests
	{
		private readonly Mock<IUrlRepository> mockUrlRepository = new Mock<IUrlRepository>();

		[Fact]
		public void GivenAUrlIsBeingShortened_ThenAValidUrlIsReturned()
		{
			var urlShortenerService = new UrlShortenerService(this.mockUrlRepository.Object);

			var shortenedUrl = urlShortenerService.ShortenUrl("https://www.bbc.co.uk", "http", "localhost.com");

			Assert.True(Uri.IsWellFormedUriString(shortenedUrl, UriKind.RelativeOrAbsolute));
		}

		[Fact]
		public void GivenAUrlIsBeingShortened_WhenOriginalUrlIsNotValid_ThenExpectedExceptionIsThrown()
		{
			var urlShortenerService = new UrlShortenerService(this.mockUrlRepository.Object);

			Assert.Throws<InvalidUrlException>(() => urlShortenerService.ShortenUrl("Absolutely not a valid URL", "http", "localhost.com"));
		}

		[Fact]
		public void GivenAUrlIsBeingShortened_ThenExpectedLengthShortenedUrlIsReturned()
		{
			var urlShortenerService = new UrlShortenerService(this.mockUrlRepository.Object);

			var shortenedUrl = new Uri(urlShortenerService.ShortenUrl("https://www.bbc.co.uk", "http", "localhost.com"));

			Assert.Equal(6, shortenedUrl.LocalPath.Replace("/", "").Length);
		}


		[Fact]
		public void GivenAUrlIsBeingShortened_ThenTheOriginalUrlAndShortenedUrlIsStoredInDataPersistence()
		{
			var expectedUrl = "https://www.bbc.co.uk";

			var urlShortenerService = new UrlShortenerService(this.mockUrlRepository.Object);

			urlShortenerService.ShortenUrl(expectedUrl, "http", "localhost.com");

			this.mockUrlRepository.Verify(m => m.Create(
				It.Is<ProcessedUrl>(processedUrl =>
					processedUrl.OriginalUrl == expectedUrl)));
		}

		[Fact]
		public void GivenAUrlIsBeingShortened_WhenTheUrlHasAlreadyBeenShortened_ThenTheShortenedUrlIsReadFromDataPersistence()
		{
			var url = "https://bbc.co.uk";
			var expectedShortenedUrl = "http://localhost.com/TestId";

			this.mockUrlRepository.Setup(m => m.ReadByOriginalUrl(url))
				.Returns(new ProcessedUrl
				{
					OriginalUrl = url,
					Id = "TestId"
				});

			var urlShortenerService = new UrlShortenerService(this.mockUrlRepository.Object);

			var shortenedUrl = urlShortenerService.ShortenUrl(url, "http", "localhost.com");

			Assert.Equal(expectedShortenedUrl, shortenedUrl);
		}

		[Fact]
		public void GivenAUrlIsBeingShortened_WhenTheUrlHasAlreadyBeenShortened_ThenANewShortenedUrlIsNotStoredInDataPersistence()
		{
			var url = "https://bbc.co.uk";

			this.mockUrlRepository.Setup(m => m.ReadByOriginalUrl(url))
				.Returns(new ProcessedUrl
				{
					OriginalUrl = url,
					Id = "TestId"
				});

			var urlShortenerService = new UrlShortenerService(this.mockUrlRepository.Object);

			urlShortenerService.ShortenUrl(url, "http", "localhost.com");

			this.mockUrlRepository.Verify(m => m.Create(It.IsAny<ProcessedUrl>()), Times.Never);
		}

		[Fact]
		public void GivenGettingOriginalUrl_WhenOriginalUrlIsNotFound_ThenNullIsReturned()
		{
			var urlShortenerService = new UrlShortenerService(this.mockUrlRepository.Object);

			Assert.Null(urlShortenerService.GetOriginalUrl("This doesn't exist"));
		}

		[Fact]
		public void GivenGettingOriginalUrl_ThenExpectedUrlIsReturnedFromDataPersistence()
		{
			var expectedOriginalUrl = "https://bbc.co.uk";
			var shortenedUrl = "http://localhost.com/testId";

			this.mockUrlRepository.Setup(m => m.ReadByShortenedUrl(shortenedUrl))
				.Returns(new ProcessedUrl
				{
					OriginalUrl = expectedOriginalUrl
				});

			var urlShortenerService = new UrlShortenerService(this.mockUrlRepository.Object);

			var originalUrlToTest = urlShortenerService.GetOriginalUrl(shortenedUrl);

			Assert.Equal(expectedOriginalUrl, originalUrlToTest);
		}
	}
}
