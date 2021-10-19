using System;
using System.Linq;
using UrlShortener.Api;

namespace UrlShortener.Domain
{
	public class UrlShortenerService : IUrlShortenerService
	{
		private readonly IUrlRepository urlRepository;

		public UrlShortenerService(IUrlRepository urlRepository)
		{
			this.urlRepository = urlRepository ?? throw new ArgumentNullException(nameof(urlRepository));
		}

		public string GetOriginalUrl(string shortenedUrl)
		{
			var processedUrl = this.urlRepository.ReadByShortenedUrl(shortenedUrl);
			if (processedUrl != null)
			{
				return processedUrl.OriginalUrl;
			}

			return null;
		}

		public string ShortenUrl(string urlToShorten, string shortenedUrlScheme, string shortenedUrlHost)
		{
			if (!Uri.IsWellFormedUriString(urlToShorten, UriKind.Absolute))
			{
				throw new InvalidUrlException(urlToShorten);
			}

			var existingEncodedUrl = this.urlRepository.ReadByOriginalUrl(urlToShorten);
			if (existingEncodedUrl != null)
			{
				return $"{shortenedUrlScheme}://{shortenedUrlHost}/{existingEncodedUrl.Id}";
			}

			var id = this.CreateRandomString();

			this.urlRepository.Create(new ProcessedUrl
			{
				Id = id,
				OriginalUrl = urlToShorten
			});

			return $"{shortenedUrlScheme}://{shortenedUrlHost}/{id}";
		}

		private string CreateRandomString()
		{
			var random = new Random();
			var characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz123456789";
			var length = 6;

			var randomString = "";

			for (int i = 0; i < length; i++)
			{
				int randomChar = random.Next(61);
				randomString += characters.ElementAt(randomChar);
			}

			return randomString;
		}
	}
}
