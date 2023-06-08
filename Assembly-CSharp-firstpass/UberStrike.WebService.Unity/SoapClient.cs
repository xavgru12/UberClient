using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using UnityEngine;

namespace UberStrike.WebService.Unity
{
	internal static class SoapClient
	{
		private static int _requestId;

		private static void LogRequest(int id, float time, int sizeBytes, string interfaceName, string serviceName, string methodName)
		{
			if (Configuration.RequestLogger != null)
			{
				string text = ((float)sizeBytes / 1000f).ToString();
				Configuration.RequestLogger($"[REQ] ID:{id} Time:{time:N2} Size:{text:N2}Kb Service:{serviceName} Interface:{interfaceName} Method:{methodName}");
			}
		}

		private static void LogResponse(int id, float time, string message, float duration, int sizeBytes)
		{
			if (Configuration.RequestLogger != null)
			{
				string text = ((float)sizeBytes / 1000f).ToString();
				Configuration.RequestLogger($"[RSP] ID:{id} Time:{time:N2} Size:{text:N2}Kb Duration:{duration:N2}s Status:{message}");
			}
		}

		public static IEnumerator MakeRequest(string interfaceName, string serviceName, string methodName, byte[] data, Action<byte[]> requestCallback, Action<Exception> exceptionHandler)
		{
			int requestId = _requestId++;
			string s = "<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\"><s:Body><" + methodName + " xmlns=\"http://tempuri.org/\"><data>" + Convert.ToBase64String(data) + "</data></" + methodName + "></s:Body></s:Envelope>";
			byte[] byteArray = Encoding.UTF8.GetBytes(s);
			Dictionary<string, string> headers = new Dictionary<string, string>
			{
				{
					"SOAPAction",
					"\"http://tempuri.org/" + interfaceName + "/" + methodName + "\""
				},
				{
					"Content-type",
					"text/xml; charset=utf-8"
				}
			};
			XmlDocument doc = new XmlDocument();
			float startTime = Time.realtimeSinceStartup;
			LogRequest(requestId, startTime, data.Length, interfaceName, serviceName, methodName);
			yield return new WaitForEndOfFrame();
			if (WebServiceStatistics.IsEnabled)
			{
				WebServiceStatistics.RecordWebServiceBegin(methodName, byteArray.Length);
			}
			byte[] returnData = null;
			using (WWW request = new WWW(Configuration.WebserviceBaseUrl + serviceName, byteArray, headers))
			{
				yield return request;
				if (WebServiceStatistics.IsEnabled)
				{
					WebServiceStatistics.RecordWebServiceEnd(methodName, request.bytes.Length, request.isDone && string.IsNullOrEmpty(request.error));
				}
				try
				{
					if (Configuration.SimulateWebservicesFail)
					{
						throw new Exception("Simulated Webservice fail when calling " + interfaceName + "/" + methodName);
					}
					if (!request.isDone || !string.IsNullOrEmpty(request.error))
					{
						LogResponse(requestId, Time.realtimeSinceStartup, request.error, Time.time - startTime, 0);
						throw new Exception(request.error + "\nWWW Url: " + Configuration.WebserviceBaseUrl + "\nService: " + serviceName + "\nMethod: " + methodName);
					}
					if (!string.IsNullOrEmpty(request.text))
					{
						try
						{
							doc.LoadXml(request.text);
							XmlNodeList elementsByTagName = doc.GetElementsByTagName(methodName + "Result");
							if (elementsByTagName.Count <= 0)
							{
								LogResponse(requestId, Time.realtimeSinceStartup, request.text, Time.time - startTime, 0);
								throw new Exception("WWW Request to " + Configuration.WebserviceBaseUrl + serviceName + " failed with content" + request.text);
							}
							returnData = Convert.FromBase64String(elementsByTagName[0].InnerXml);
							if (returnData.Length == 0)
							{
								LogResponse(requestId, Time.realtimeSinceStartup, request.text, Time.time - startTime, 0);
								throw new Exception("WWW Request to " + Configuration.WebserviceBaseUrl + serviceName + " failed with content" + request.text);
							}
							LogResponse(requestId, Time.realtimeSinceStartup, "OK", Time.realtimeSinceStartup - startTime, request.bytes.Length);
						}
						catch
						{
							LogResponse(requestId, Time.time, request.text, Time.realtimeSinceStartup - startTime, 0);
							throw new Exception("Error reading XML return for method call " + interfaceName + "/" + methodName + ":" + request.text);
						}
					}
					requestCallback?.Invoke(returnData);
				}
				catch (Exception ex)
				{
					if (exceptionHandler != null)
					{
						exceptionHandler(ex);
					}
					else
					{
						Debug.LogError("SoapClient Unhandled Exception: " + ex.Message + "\n" + ex.StackTrace);
					}
				}
			}
		}
	}
}
