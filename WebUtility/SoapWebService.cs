using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web;
using System.Xml.Linq;
using System.Xml.Serialization;

public class SOAPWebService
{
	public string Url { get; set; }
	public string MethodName { get; set; }
	public Dictionary<string, string> Params = new Dictionary<string, string>();
	public XDocument ResultXML;
	public string ResultString;

	public SOAPWebService()
	{

	}

	public SOAPWebService(string url, string methodName)
	{
		Url = url;
		MethodName = methodName;
	}

	/// <summary>
	/// Invokes service
	/// </summary>
	public void Invoke()
	{
		Invoke(true);
	}

	/// <summary>
	/// Invokes service
	/// </summary>
	/// <param name="encode">Added parameters will encode? (default: true)</param>
	public void Invoke(bool encode)
	{
		string soapStr =
			@"<?xml version=""1.0"" encoding=""utf-8""?>
            <soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" 
               xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" 
               xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
              <soap:Body>
                <{0} xmlns=""http://tempuri.org/"">
                  {1}
                </{0}>
              </soap:Body>
            </soap:Envelope>";

		HttpWebRequest req = (HttpWebRequest)WebRequest.Create(Url);
		req.Headers.Add("SOAPAction", "\"http://tempuri.org/IWmsService/" + MethodName + "\"");
		req.ContentType = "text/xml;charset=\"utf-8\"";
		req.Accept = "application/json";
		req.Method = "POST";

		using (Stream stm = req.GetRequestStream())
		{
			string postValues = "";
			foreach (var param in Params)
			{
				if (param.Value.StartsWith('<'))
					postValues += string.Format("<{0}>{1}</{0}>", param.Key, param.Value);
				else if (encode)
					postValues += string.Format("<{0}>{1}</{0}>", HttpUtility.UrlEncode(param.Key), HttpUtility.UrlEncode(param.Value));
				else
					postValues += string.Format("<{0}>{1}</{0}>", param.Key, param.Value);
			}

			soapStr = string.Format(soapStr, MethodName, postValues);
			using (StreamWriter stmw = new StreamWriter(stm))
			{
				stmw.Write(soapStr);
			}
		}

		using (StreamReader responseReader = new StreamReader(req.GetResponse().GetResponseStream()))
		{
			string result = responseReader.ReadToEnd();
			ResultXML = XDocument.Parse(result);
			ResultString = result;
		}
	}

	public void AddParameter(string key, object value)
	{
		if (IsSimpleType(value.GetType()))
			Params[key] = value.ToString();
		else if (value.GetType().GetMethod("AddToWebService") != null)
		{
			MethodInfo mi = value.GetType().GetMethod("AddToWebService");
			mi.Invoke(value, new object[] { this });
		}
		else
			Params[key] = GetXML(value);
	}

	public void AddListParameter<T>(string key,List<T> ts)
	{
		var template = $"<{typeof(T).Name.ToLower()} xmlns=\"http://schemas.microsoft.com/2003/10/Serialization/Arrays\">{{0}}</{typeof(T).Name.ToLower()}>";
		var body = "";
		foreach (var item in ts)
		{
			var res = item.ToString();
			if (typeof(T) == typeof(Guid))
				res = res.Replace("{", "").Replace("}", "");
			body += string.Format(template, Convert.ChangeType(res,typeof(string))) + "\r\n";
		}
		Params[key] = $"{body}";
	}

	private string GetXML(object value)
	{
		var xs = new XmlSerializer(value.GetType());
		TextWriter txtWriter = new StringWriter();
		xs.Serialize(txtWriter, value);
		return txtWriter.ToString();
	}

	public T GetObject<T>(string path = "") where T : class, new()
	{
		T res = new T();
		var typeOfT = typeof(T);
		bool isGeneric = typeOfT.IsGenericType;
		var IListRef = typeof(List<>);
		XDocument document = XDocument.Load(new StringReader(ResultString));
		path = $"Body{(path.StartsWith("/") ? "" : "/")}{path}";
		var pathParts = path.Split('/');
		var elements = document.Root.Elements();

		foreach (var part in pathParts)
		{
			elements = elements.Where(m => m.Name.LocalName.Equals(part, StringComparison.OrdinalIgnoreCase)).Elements();
		}

		if (isGeneric)
		{
			typeOfT = typeOfT.GenericTypeArguments[0];
			Type[] IListParam = { typeOfT };
			object Result = Activator.CreateInstance(IListRef.MakeGenericType(IListParam));
			foreach (var e in elements)
			{
				Result.GetType().GetMethod("Add").Invoke(Result, new[] { Get(e, typeOfT) });
			}
			return (T)Result;
		}
		else
		{
			return (T)Get(elements, typeOfT);
		}
		return (T)Get(elements.FirstOrDefault(), typeOfT);
	}

	private object Get(IEnumerable<XElement> e,Type T)
	{
		var props = T.GetProperties();
		object res = Activator.CreateInstance(T);
		foreach (var prop in props)
		{
			var propValue = e.FirstOrDefault(m => m.Name.LocalName == prop.Name).Value;
			if (prop.PropertyType == typeof(Guid))
				prop.SetValue(res, new Guid(propValue));
			else
				prop.SetValue(res, Convert.ChangeType(propValue, prop.PropertyType));
		}
		return res;
	}

	private object Get(XElement e, Type T)
	{
		var props = T.GetProperties();
		object res = Activator.CreateInstance(T);
		foreach (var prop in props)
		{
			var propValue = e.Elements().FirstOrDefault(m => m.Name.LocalName == prop.Name).Value;
			if (prop.PropertyType == typeof(Guid))
				prop.SetValue(res, new Guid(propValue));
			else
				prop.SetValue(res, Convert.ChangeType(propValue, prop.PropertyType));
		}
		return res;
	}

	public bool IsSimpleType(Type type)
	{
		return
			type.IsPrimitive ||
			new Type[] {
			typeof(Enum),
			typeof(string),
			typeof(decimal),
			typeof(DateTime),
			typeof(DateTimeOffset),
			typeof(TimeSpan),
			typeof(Guid)
			}.Contains(type) ||
			Convert.GetTypeCode(type) != TypeCode.Object ||
			(type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>) && IsSimpleType(type.GetGenericArguments()[0]))
			;
	}
}
