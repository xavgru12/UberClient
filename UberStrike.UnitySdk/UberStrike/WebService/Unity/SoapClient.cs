
using System;
using System.Collections;
using System.Text;
using System.Xml;
using UnityEngine;

namespace UberStrike.WebService.Unity
{
  internal static class SoapClient
  {
    private static int _requestId = 0;

    private static void LogRequest(
      int id,
      float time,
      int sizeBytes,
      string interfaceName,
      string serviceName,
      string methodName)
    {
      if (Configuration.RequestLogger == null)
        return;
      string str = ((float) sizeBytes / 1000f).ToString();
      Configuration.RequestLogger(string.Format("[REQ] ID:{0} Time:{1:N2} Size:{2:N2}Kb Service:{3} Interface:{4} Method:{5}", (object) id, (object) time, (object) str, (object) serviceName, (object) interfaceName, (object) methodName));
    }

    private static void LogResponse(
      int id,
      float time,
      string message,
      float duration,
      int sizeBytes)
    {
      if (Configuration.RequestLogger == null)
        return;
      string str = ((float) sizeBytes / 1000f).ToString();
      Configuration.RequestLogger(string.Format("[RSP] ID:{0} Time:{1:N2} Size:{2:N2}Kb Duration:{3:N2}s Status:{4}", (object) id, (object) time, (object) str, (object) duration, (object) message));
    }

    public static IEnumerator MakeRequest(
      string interfaceName,
      string serviceName,
      string methodName,
      byte[] data,
      Action<byte[]> requestCallback,
      Action<Exception> exceptionHandler)
    {
      int requestId = SoapClient._requestId++;
      string postData = "<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\"><s:Body><" + methodName + " xmlns=\"http://tempuri.org/\"><data>" + Convert.ToBase64String(data) + "</data></" + methodName + "></s:Body></s:Envelope>";
      byte[] byteArray = Encoding.UTF8.GetBytes(postData);
      Hashtable headers = new Hashtable();
      headers.Add((object) "SOAPAction", (object) ("\"http://tempuri.org/" + interfaceName + "/" + methodName + "\""));
      headers.Add((object) "Content-type", (object) "text/xml; charset=utf-8");
      XmlDocument doc = new XmlDocument();
      float startTime = Time.realtimeSinceStartup;
      SoapClient.LogRequest(requestId, startTime, data.Length, interfaceName, serviceName, methodName);
      yield return (object) new WaitForEndOfFrame();
      if (WebServiceStatistics.IsEnabled)
        WebServiceStatistics.RecordWebServiceBegin(methodName, byteArray.Length);
      using (WWW request = new WWW(Configuration.WebserviceBaseUrl + serviceName, byteArray, headers))
      {
        yield return (object) request;
        if (WebServiceStatistics.IsEnabled)
          WebServiceStatistics.RecordWebServiceEnd(methodName, request.bytes.Length, request.isDone && string.IsNullOrEmpty(request.error));
        try
        {
          if (Configuration.SimulateWebservicesFail)
            throw new Exception("Simulated Webservice fail when calling " + interfaceName + "/" + methodName);
          byte[] numArray = (byte[]) null;
          if (request.isDone && string.IsNullOrEmpty(request.error))
          {
            if (!string.IsNullOrEmpty(request.text))
            {
              try
              {
                doc.LoadXml(request.text);
                XmlNodeList elementsByTagName = doc.GetElementsByTagName(methodName + "Result");
                if (elementsByTagName.Count > 0)
                {
                  numArray = Convert.FromBase64String(elementsByTagName[0].InnerXml);
                  SoapClient.LogResponse(requestId, Time.realtimeSinceStartup, "OK", Time.realtimeSinceStartup - startTime, request.bytes.Length);
                }
                else
                {
                  SoapClient.LogResponse(requestId, Time.realtimeSinceStartup, request.text, Time.time - startTime, 0);
                  throw new Exception("WWW Request to " + Configuration.WebserviceBaseUrl + serviceName + " failed with content" + request.text);
                }
              }
              catch
              {
                SoapClient.LogResponse(requestId, Time.time, request.text, Time.realtimeSinceStartup - startTime, 0);
                throw new Exception("Error reading XML return for method call " + interfaceName + "/" + methodName + ":" + request.text);
              }
            }
            if (requestCallback != null)
              requestCallback(numArray);
          }
          else
          {
            SoapClient.LogResponse(requestId, Time.realtimeSinceStartup, request.error, Time.time - startTime, 0);
            throw new Exception("WWW Url: " + Configuration.WebserviceBaseUrl + "\nService: " + serviceName + "\nMethod: " + methodName + "\nMessage: " + request.error);
          }
        }
        catch (Exception ex)
        {
          if (exceptionHandler != null)
            exceptionHandler(ex);
          else
            throw;
        }
      }
    }
  }
}
