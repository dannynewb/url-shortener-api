using System;
using System.Collections.Generic;
using System.Linq;

namespace UrlShortener.Domain
{
	public class UrlRepository : IUrlRepository
	{
		private readonly List<ProcessedUrl> processedUrls = new List<ProcessedUrl>();

		public void Create(ProcessedUrl processedUrl)
		{
			this.processedUrls.Add(processedUrl);
		}

		public ProcessedUrl ReadByOriginalUrl(string originalUrl)
		{
			return this.processedUrls.FirstOrDefault(u => u.OriginalUrl == originalUrl);
		}

		public ProcessedUrl ReadByShortenedUrl(string shortenedUrl)
		{
			return this.processedUrls.FirstOrDefault(u => shortenedUrl.Contains(u.Id));
		}
	}
}
