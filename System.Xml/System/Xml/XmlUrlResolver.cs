// Decompiled with JetBrains decompiler
// Type: System.Xml.XmlUrlResolver
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.IO;
using System.Net;

namespace System.Xml
{
  public class XmlUrlResolver : XmlResolver
  {
    private ICredentials credential;

    public override ICredentials Credentials
    {
      set => this.credential = value;
    }

    public override object GetEntity(Uri absoluteUri, string role, Type ofObjectToReturn)
    {
      if (ofObjectToReturn == null)
        ofObjectToReturn = typeof (Stream);
      if (ofObjectToReturn != typeof (Stream))
        throw new XmlException("This object type is not supported.");
      if (!absoluteUri.IsAbsoluteUri)
        throw new ArgumentException("uri must be absolute.", nameof (absoluteUri));
      if (absoluteUri.Scheme == "file")
      {
        if (absoluteUri.AbsolutePath == string.Empty)
          throw new ArgumentException("uri must be absolute.", nameof (absoluteUri));
        return (object) new FileStream(this.UnescapeRelativeUriBody(absoluteUri.LocalPath), FileMode.Open, FileAccess.Read, FileShare.Read);
      }
      WebRequest webRequest = WebRequest.Create(absoluteUri);
      if (this.credential != null)
        webRequest.Credentials = this.credential;
      return (object) webRequest.GetResponse().GetResponseStream();
    }

    public override Uri ResolveUri(Uri baseUri, string relativeUri) => base.ResolveUri(baseUri, relativeUri);

    private string UnescapeRelativeUriBody(string src) => src.Replace("%3C", "<").Replace("%3E", ">").Replace("%23", "#").Replace("%22", "\"").Replace("%20", " ").Replace("%25", "%");
  }
}
