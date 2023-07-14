// Decompiled with JetBrains decompiler
// Type: Mono.Xml.DTDEntityDeclaration
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections;
using System.Xml;

namespace Mono.Xml
{
  internal class DTDEntityDeclaration : DTDEntityBase
  {
    private string entityValue;
    private string notationName;
    private ArrayList ReferencingEntities = new ArrayList();
    private bool scanned;
    private bool recursed;
    private bool hasExternalReference;

    internal DTDEntityDeclaration(DTDObjectModel root)
      : base(root)
    {
    }

    public string NotationName
    {
      get => this.notationName;
      set => this.notationName = value;
    }

    public bool HasExternalReference
    {
      get
      {
        if (!this.scanned)
          this.ScanEntityValue(new ArrayList());
        return this.hasExternalReference;
      }
    }

    public string EntityValue
    {
      get
      {
        if (this.IsInvalid || this.PublicId == null && this.SystemId == null && this.LiteralEntityValue == null)
          return string.Empty;
        if (this.entityValue == null)
        {
          if (this.NotationName != null)
            this.entityValue = string.Empty;
          else if (this.SystemId == null || this.SystemId == string.Empty)
          {
            this.entityValue = this.ReplacementText;
            if (this.entityValue == null)
              this.entityValue = string.Empty;
          }
          else
            this.entityValue = this.ReplacementText;
          this.ScanEntityValue(new ArrayList());
        }
        return this.entityValue;
      }
    }

    public void ScanEntityValue(ArrayList refs)
    {
      string str1 = this.EntityValue;
      if (this.SystemId != null)
        this.hasExternalReference = true;
      this.recursed = !this.recursed ? true : throw this.NotWFError("Entity recursion was found.");
      if (this.scanned)
      {
        foreach (string str2 in refs)
        {
          if (this.ReferencingEntities.Contains((object) str2))
            throw this.NotWFError(string.Format("Nested entity was found between {0} and {1}", (object) str2, (object) this.Name));
        }
        this.recursed = false;
      }
      else
      {
        int length = str1.Length;
        int startIndex = 0;
        for (int index = 0; index < length; ++index)
        {
          switch (str1[index])
          {
            case '&':
              startIndex = index + 1;
              break;
            case ';':
              if (startIndex != 0)
              {
                string name = str1.Substring(startIndex, index - startIndex);
                if (name.Length == 0)
                  throw this.NotWFError("Entity reference name is missing.");
                if (name[0] != '#' && XmlChar.GetPredefinedEntity(name) < 0)
                {
                  this.ReferencingEntities.Add((object) name);
                  DTDEntityDeclaration entityDecl = this.Root.EntityDecls[name];
                  if (entityDecl != null)
                  {
                    if (entityDecl.SystemId != null)
                      this.hasExternalReference = true;
                    refs.Add((object) this.Name);
                    entityDecl.ScanEntityValue(refs);
                    foreach (string referencingEntity in entityDecl.ReferencingEntities)
                      this.ReferencingEntities.Add((object) referencingEntity);
                    refs.Remove((object) this.Name);
                    str1 = str1.Remove(startIndex - 1, name.Length + 2).Insert(startIndex - 1, entityDecl.EntityValue);
                    index -= name.Length + 1;
                    length = str1.Length;
                  }
                  startIndex = 0;
                  break;
                }
                break;
              }
              break;
          }
        }
        if (startIndex != 0)
          this.Root.AddError(new XmlException((IXmlLineInfo) this, this.BaseURI, "Invalid reference character '&' is specified."));
        this.scanned = true;
        this.recursed = false;
      }
    }
  }
}
