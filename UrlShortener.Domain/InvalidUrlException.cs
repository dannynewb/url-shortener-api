using System;

namespace UrlShortener.Api
{
	public class InvalidUrlException : Exception
	{
		public InvalidUrlException(string invalidUrl) : base($"Invalid url: {invalidUrl}") { }
	}
}
