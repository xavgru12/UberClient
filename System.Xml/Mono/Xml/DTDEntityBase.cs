// Decompiled with JetBrains decompiler
// Type: Mono.Xml.DTDEntityBase
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System;
using System.IO;
using System.Xml;

namespace Mono.Xml
{
  internal class DTDEntityBase : DTDNode
  {
    private string name;
    private string publicId;
    private string systemId;
    private string literalValue;
    private string replacementText;
    private string uriString;
    private Uri absUri;
    private bool isInvalid;
    private bool loadFailed;
    private XmlResolver resolver;

    protected DTDEntityBase(DTDObjectModel root) => this.SetRoot(root);

    internal bool IsInvalid
    {
      get => this.isInvalid;
      set => this.isInvalid = value;
    }

    public bool LoadFailed
    {
      get => this.loadFailed;
      set => this.loadFailed = value;
    }

    public string Name
    {
      get => this.name;
      set => this.name = value;
    }

    public string PublicId
    {
      get => this.publicId;
      set => this.publicId = value;
    }

    public string SystemId
    {
      get => this.systemId;
      set => this.systemId = value;
    }

    public string LiteralEntityValue
    {
      get => this.literalValue;
      set => this.literalValue = value;
    }

    public string ReplacementText
    {
      get => this.replacementText;
      set => this.replacementText = value;
    }

    public XmlResolver XmlResolver
    {
      set => this.resolver = value;
    }

    public string ActualUri
    {
      get
      {
        if (this.uriString == null)
        {
          if (this.resolver == null || this.SystemId == null || this.SystemId.Length == 0)
          {
            this.uriString = this.BaseURI;
          }
          else
          {
            Uri baseUri = (Uri) null;
            try
            {
              if (this.BaseURI != null)
              {
                if (this.BaseURI.Length > 0)
                  baseUri = new Uri(this.BaseURI);
              }
            }
            catch (UriFormatException ex)
            {
            }
            this.absUri = this.resolver.ResolveUri(baseUri, this.SystemId);
            this.uriString = !(this.absUri != (Uri) null) ? string.Empty : this.absUri.ToString();
          }
        }
        return this.uriString;
      }
    }

    public void Resolve()
    {
      if (this.ActualUri == string.Empty)
      {
        this.LoadFailed = true;
        this.LiteralEntityValue = string.Empty;
      }
      else
      {
        if (this.Root.ExternalResources.ContainsKey((object) this.ActualUri))
          this.LiteralEntityValue = (string) this.Root.ExternalResources[(object) this.ActualUri];
        Stream input = (Stream) null;
        try
        {
          input = this.resolver.GetEntity(this.absUri, (string) null, typeof (Stream)) as Stream;
          this.LiteralEntityValue = new Mono.Xml2.XmlTextReader(this.ActualUri, input, this.Root.NameTable).GetRemainder().ReadToEnd();
          this.Root.ExternalResources.Add((object) this.ActualUri, (object) this.LiteralEntityValue);
          if (this.Root.ExternalResources.Count > 256)
            throw new InvalidOperationException("The total amount of external entities exceeded the allowed number.");
        }
        catch (Exception ex)
        {
          this.LiteralEntityValue = string.Empty;
          this.LoadFailed = true;
        }
        finally
        {
          input?.Close();
        }
      }
    }
  }
}
