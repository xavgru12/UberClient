// Decompiled with JetBrains decompiler
// Type: UberStrike.WebService.Unity.SoapClient
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

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
      byte[] byteArray = Encoding.UTF8.GetBytes("<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\"><s:Body><" + methodName + " xmlns=\"http://tempuri.org/\"><data>" + Convert.ToBase64String(data) + "</data></" + methodName + "></s:Body></s:Envelope>");
      Dictionary<string, string> headers = new Dictionary<string, string>()
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
      SoapClient.LogRequest(requestId, startTime, data.Length, interfaceName, serviceName, methodName);
      yield return (object) new WaitForEndOfFrame();
      if (WebServiceStatistics.IsEnabled)
        WebServiceStatistics.RecordWebServiceBegin(methodName, byteArray.Length);
      byte[] returnData = (byte[]) null;
      using (WWW request = new WWW(Configuration.WebserviceBaseUrl + serviceName, byteArray, headers))
      {
        yield return (object) request;
        if (WebServiceStatistics.IsEnabled)
          WebServiceStatistics.RecordWebServiceEnd(methodName, request.bytes.Length, request.isDone && string.IsNullOrEmpty(request.error));
        try
        {
          if (Configuration.SimulateWebservicesFail)
            throw new Exception("Simulated Webservice fail when calling " + interfaceName + "/" + methodName);
          if (!request.isDone || !string.IsNullOrEmpty(request.error))
          {
            SoapClient.LogResponse(requestId, Time.realtimeSinceStartup, request.error, Time.time - startTime, 0);
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
                SoapClient.LogResponse(requestId, Time.realtimeSinceStartup, request.text, Time.time - startTime, 0);
                throw new Exception("WWW Request to " + Configuration.WebserviceBaseUrl + serviceName + " failed with content" + request.text);
              }
              returnData = Convert.FromBase64String(elementsByTagName[0].InnerXml);
              if (returnData.Length == 0)
              {
                SoapClient.LogResponse(requestId, Time.realtimeSinceStartup, request.text, Time.time - startTime, 0);
                throw new Exception("WWW Request to " + Configuration.WebserviceBaseUrl + serviceName + " failed with content" + request.text);
              }
              SoapClient.LogResponse(requestId, Time.realtimeSinceStartup, "OK", Time.realtimeSinceStartup - startTime, request.bytes.Length);
            }
            catch
            {
              SoapClient.LogResponse(requestId, Time.time, request.text, Time.realtimeSinceStartup - startTime, 0);
              throw new Exception("Error reading XML return for method call " + interfaceName + "/" + methodName + ":" + request.text);
            }
          }
          Action<byte[]> action = requestCallback;
          if (action != null)
            action(returnData);
        }
        catch (Exception ex)
        {
          if (exceptionHandler != null)
            exceptionHandler(ex);
          else
            Debug.LogError((object) ("SoapClient Unhandled Exception: " + ex.Message + "\n" + ex.StackTrace));
        }
      }
    }
  }
}
