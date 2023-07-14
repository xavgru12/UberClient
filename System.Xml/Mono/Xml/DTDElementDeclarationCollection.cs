﻿// Decompiled with JetBrains decompiler
// Type: Mono.Xml.DTDElementDeclarationCollection
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System;
using System.Xml;

namespace Mono.Xml
{
  internal class DTDElementDeclarationCollection : DTDCollectionBase
  {
    public DTDElementDeclarationCollection(DTDObjectModel root)
      : base(root)
    {
    }

    public DTDElementDeclaration this[string name] => this.Get(name);

    public DTDElementDeclaration Get(string name) => this.BaseGet(name) as DTDElementDeclaration;

    public void Add(string name, DTDElementDeclaration decl)
    {
      if (this.Contains(name))
      {
        this.Root.AddError(new XmlException(string.Format("Element declaration for {0} was already added.", (object) name), (Exception) null));
      }
      else
      {
        decl.SetRoot(this.Root);
        this.BaseAdd(name, (DTDNode) decl);
      }
    }
  }
}
