using UrlShortener.Domain;
using Xunit;

namespace UrlShortener.Tests.DomainTests
{
	public class UrlRepositoryTests
	{
		[Fact]
		public void GivenReadingByShortenedUrl_ThenExpectedProcessedUrlIsReturned()
		{
			var expectedProcessedUrl = new ProcessedUrl
			{
				Id = "54321",
				OriginalUrl = "https://expectedUrl.co.uk"
			};

			var urlRepository = new UrlRepository();

			urlRepository.Create(new ProcessedUrl { Id = "12345", OriginalUrl = "https://bbc.co.uk" });
			urlRepository.Create(expectedProcessedUrl);

			var processedUrlToTest = urlRepository.ReadByShortenedUrl(expectedProcessedUrl.Id);

			Assert.Equal(expectedProcessedUrl, processedUrlToTest);
		}

		[Fact]
		public void GivenReadingByShortenedUrl_WhenProcessedUrlIsNotFound_ThenNullIsReturned()
		{
			var urlRepository = new UrlRepository();

			Assert.Null(urlRepository.ReadByShortenedUrl("This doesnt exist"));
		}

		[Fact]
		public void GivenReadingByOriginalUrl_henExpectedProcessedUrlIsReturned()
		{
			var expectedProcessedUrl = new ProcessedUrl
			{
				Id = "54321",
				OriginalUrl = "https://expectedUrl.co.uk"
			};

			var urlRepository = new UrlRepository();

			urlRepository.Create(new ProcessedUrl { Id = "12345", OriginalUrl = "https://bbc.co.uk" });
			urlRepository.Create(expectedProcessedUrl);

			var processedUrlToTest = urlRepository.ReadByOriginalUrl(expectedProcessedUrl.OriginalUrl);

			Assert.Equal(expectedProcessedUrl, processedUrlToTest);
		}

		[Fact]
		public void GivenReadingByOriginalUrl_WhenProcessedUrlIsNotFound_ThenNullIsReturned()
		{
			var urlRepository = new UrlRepository();

			Assert.Null(urlRepository.ReadByOriginalUrl("This doesnt exist"));
		}
	}
}
