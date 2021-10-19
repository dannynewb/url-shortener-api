namespace UrlShortener.Api
{
	public class DecodeResponse
	{
		public DecodeResponse(string url)
		{
			this.OriginalUrl = url;
		}

		public string OriginalUrl { get; set; }
	}
}
