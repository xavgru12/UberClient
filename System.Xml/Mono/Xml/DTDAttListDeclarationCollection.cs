// Decompiled with JetBrains decompiler
// Type: Mono.Xml.DTDAttListDeclarationCollection
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections;

namespace Mono.Xml
{
  internal class DTDAttListDeclarationCollection : DTDCollectionBase
  {
    public DTDAttListDeclarationCollection(DTDObjectModel root)
      : base(root)
    {
    }

    public DTDAttListDeclaration this[string name] => this.BaseGet(name) as DTDAttListDeclaration;

    public void Add(string name, DTDAttListDeclaration decl)
    {
      DTDAttListDeclaration attListDeclaration = this[name];
      if (attListDeclaration != null)
      {
        foreach (DTDAttributeDefinition definition in (IEnumerable) decl.Definitions)
        {
          if (decl.Get(definition.Name) == null)
            attListDeclaration.Add(definition);
        }
      }
      else
      {
        decl.SetRoot(this.Root);
        this.BaseAdd(name, (DTDNode) decl);
      }
    }
  }
}
