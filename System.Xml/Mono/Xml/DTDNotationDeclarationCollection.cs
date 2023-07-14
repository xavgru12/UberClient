// Decompiled with JetBrains decompiler
// Type: Mono.Xml.DTDNotationDeclarationCollection
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System;

namespace Mono.Xml
{
  internal class DTDNotationDeclarationCollection : DTDCollectionBase
  {
    public DTDNotationDeclarationCollection(DTDObjectModel root)
      : base(root)
    {
    }

    public DTDNotationDeclaration this[string name] => this.BaseGet(name) as DTDNotationDeclaration;

    public void Add(string name, DTDNotationDeclaration decl)
    {
      if (this.Contains(name))
        throw new InvalidOperationException(string.Format("Notation declaration for {0} was already added.", (object) name));
      decl.SetRoot(this.Root);
      this.BaseAdd(name, (DTDNode) decl);
    }
  }
}
