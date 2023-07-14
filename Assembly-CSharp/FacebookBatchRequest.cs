// Decompiled with JetBrains decompiler
// Type: FacebookBatchRequest
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using System.Text;

public class FacebookBatchRequest
{
  public Dictionary<string, string> _parameters = new Dictionary<string, string>();
  private Dictionary<string, object> _requestDict = new Dictionary<string, object>();

  public FacebookBatchRequest(string relativeUrl, string method)
  {
    this._requestDict[nameof (method)] = (object) method.ToUpper();
    this._requestDict["relative_url"] = (object) relativeUrl;
  }

  public void addParameter(string key, string value) => this._parameters[key] = value;

  public Dictionary<string, object> requestDictionary()
  {
    if (this._parameters.Count > 0)
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (KeyValuePair<string, string> parameter in this._parameters)
        stringBuilder.AppendFormat("{0}={1}&", (object) parameter.Key, (object) parameter.Value);
      stringBuilder.Remove(stringBuilder.Length - 1, 1);
      this._requestDict["body"] = (object) stringBuilder.ToString();
    }
    return this._requestDict;
  }
}
