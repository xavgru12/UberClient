// Decompiled with JetBrains decompiler
// Type: Mono.Xml.DTDAttListDeclaration
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System;
using System.Collections;

namespace Mono.Xml
{
  internal class DTDAttListDeclaration : DTDNode
  {
    private string name;
    private Hashtable attributeOrders = new Hashtable();
    private ArrayList attributes = new ArrayList();

    internal DTDAttListDeclaration(DTDObjectModel root) => this.SetRoot(root);

    public string Name
    {
      get => this.name;
      set => this.name = value;
    }

    public DTDAttributeDefinition this[int i] => this.Get(i);

    public DTDAttributeDefinition this[string name] => this.Get(name);

    public DTDAttributeDefinition Get(int i) => this.attributes[i] as DTDAttributeDefinition;

    public DTDAttributeDefinition Get(string name)
    {
      object attributeOrder = this.attributeOrders[(object) name];
      return attributeOrder != null ? this.attributes[(int) attributeOrder] as DTDAttributeDefinition : (DTDAttributeDefinition) null;
    }

    public IList Definitions => (IList) this.attributes;

    public void Add(DTDAttributeDefinition def)
    {
      if (this.attributeOrders[(object) def.Name] != null)
        throw new InvalidOperationException(string.Format("Attribute definition for {0} was already added at element {1}.", (object) def.Name, (object) this.Name));
      def.SetRoot(this.Root);
      this.attributeOrders.Add((object) def.Name, (object) this.attributes.Count);
      this.attributes.Add((object) def);
    }

    public int Count => this.attributeOrders.Count;
  }
}
