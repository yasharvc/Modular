using System;
using System.Collections.Generic;
using Contracts;
using CoreCommons;
using CoreCommons.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;

namespace RequestHandler
{
	public class RequestHandler
	{
		public RequestInformation RequestInformation { get; } = new RequestInformation();
		public ContentType ContentType { get; private set; } = ContentType.None;
		public CoreCommons.Enums.HttpMethod Method { get; private set; } = CoreCommons.Enums.HttpMethod.GET;
		HttpContext Context { get; set; }
		public string BodyString { get; private set; } = "";
		public List<RequestParameter> RequestParameters { get; set; } = new List<RequestParameter>();
		public RequestHandler(HttpContext context)
		{
			Context = context;
		}

		public void ParseRequestData()
		{
			GetContentType();
			GetHttpMethod();
			GetRequestInformation();
			GetRequestParameters();
		}

		private void GetRequestParameters()
		{
			GetBodyString();
			if (ContentType == ContentType.Multipart_Form_Data)
			{
				foreach (var form in Context.Request.Form)
					RequestParameters.Add(new RequestParameter { Name = form.Key, Value = form.Value[0] });
				foreach (var file in Context.Request.Form.Files)
					RequestParameters.Add(new RequestParameter { Name = file.Name, File = file });
			}
			else
			{
				RequestParameters.AddRange(new RequestBodyParser().Process(BodyString));
			}
		}

		private void GetHttpMethod()
		{
			switch(Context.Request.Method.ToLower())
			{
				case "post":
					Method = CoreCommons.Enums.HttpMethod.POST;
					break;
				case "get":
					Method = CoreCommons.Enums.HttpMethod.GET;
					break;
				case "put":
					Method = CoreCommons.Enums.HttpMethod.PUT;
					break;
				case "delete":
					Method = CoreCommons.Enums.HttpMethod.DELETE;
					break;
				case "patch":
					Method = CoreCommons.Enums.HttpMethod.PATCH;
					break;
				case "head":
					Method = CoreCommons.Enums.HttpMethod.HEAD;
					break;
				case "options":
					Method = CoreCommons.Enums.HttpMethod.OPTIONS;
					break;
				case "trace":
					Method = CoreCommons.Enums.HttpMethod.TRACE;
					break;
				case "connect":
					Method = CoreCommons.Enums.HttpMethod.CONNECT;
					break;
				default:
					Method = CoreCommons.Enums.HttpMethod.GET;
					break;
			}
		}

		private void GetContentType()
		{
			if (Context.Request.ContentType == null)
				ContentType = ContentType.None;
			else if (Context.Request.ContentType.Contains("multipart/form-data", StringComparison.OrdinalIgnoreCase))
				ContentType = ContentType.Multipart_Form_Data;
			else if (Context.Request.ContentType.Contains("application/x-www-form-urlencoded;", StringComparison.OrdinalIgnoreCase))
				ContentType = ContentType.Url_Encoded_Form;
			else if (Context.Request.ContentType.Contains("text/plain", StringComparison.OrdinalIgnoreCase))
				ContentType = ContentType.Text_Plain;
			else
				ContentType = ContentType.None;
		}

		private void GetRequestInformation()
		{
			if (Context.Request.Headers is HeaderDictionary requestHeaders)
				GetHeaderDictionaryHeaders(requestHeaders);
			else
				GetHttpRequestHeaders();
		}

		private void GetBodyString()
		{
			BodyString = Context.Request.BodyToString();
		}

		private void GetHeaderDictionaryHeaders(HeaderDictionary requestHeaders)
		{
			RequestInformation.HeaderHost = requestHeaders.ContainsKey("HeaderHost") && requestHeaders["HeaderHost"].Count > 0 ? requestHeaders["HeaderHost"][0] : "";
			RequestInformation.HeaderReferer = requestHeaders.ContainsKey("HeaderReferer") && requestHeaders["HeaderReferer"].Count > 0 ? requestHeaders["HeaderReferer"][0] : "";
			var index = RequestInformation.HeaderReferer.IndexOf(RequestInformation.HeaderHost);
			RequestInformation.UrlRequestPart = index >= 0 ? RequestInformation.HeaderReferer.Substring(index + RequestInformation.HeaderHost.Length) : RequestInformation.HeaderReferer;
		}

		private void GetHttpRequestHeaders()
		{
			var requestHeaders = (HttpRequestHeaders)Context.Request.Headers;
			RequestInformation.HeaderHost = requestHeaders.HeaderHost;
			RequestInformation.HeaderReferer = requestHeaders.HeaderReferer;
			var index = RequestInformation.HeaderReferer.IndexOf(RequestInformation.HeaderHost);
			RequestInformation.UrlRequestPart = index >= 0 ? RequestInformation.HeaderReferer.Substring(index + RequestInformation.HeaderHost.Length) : RequestInformation.HeaderReferer;
		}

		public bool IsRequestInformationEmpty() => RequestInformation.HeaderHost.Length == 0 && RequestInformation.HeaderReferer.Length == 0 && RequestInformation.UrlRequestPart.Length == 0;
	}
}
