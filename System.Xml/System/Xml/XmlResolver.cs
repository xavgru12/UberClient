// Decompiled with JetBrains decompiler
// Type: System.Xml.XmlResolver
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.IO;
using System.Net;

namespace System.Xml
{
  public abstract class XmlResolver
  {
    public abstract ICredentials Credentials { set; }

    public abstract object GetEntity(Uri absoluteUri, string role, Type type);

    public virtual Uri ResolveUri(Uri baseUri, string relativeUri)
    {
      if (baseUri == (Uri) null)
      {
        if (relativeUri == null)
          throw new ArgumentNullException("Either baseUri or relativeUri are required.");
        return relativeUri.StartsWith("http:") || relativeUri.StartsWith("https:") || relativeUri.StartsWith("ftp:") || relativeUri.StartsWith("file:") ? new Uri(relativeUri) : new Uri(Path.GetFullPath(relativeUri));
      }
      return relativeUri == null ? baseUri : new Uri(baseUri, this.EscapeRelativeUriBody(relativeUri));
    }

    private string EscapeRelativeUriBody(string src) => src.Replace("<", "%3C").Replace(">", "%3E").Replace("#", "%23").Replace("%", "%25").Replace("\"", "%22");
  }
}
