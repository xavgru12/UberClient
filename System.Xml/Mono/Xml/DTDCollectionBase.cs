// Decompiled with JetBrains decompiler
// Type: Mono.Xml.DTDCollectionBase
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections.Generic;

namespace Mono.Xml
{
  internal class DTDCollectionBase : DictionaryBase
  {
    private DTDObjectModel root;

    protected DTDCollectionBase(DTDObjectModel root) => this.root = root;

    protected DTDObjectModel Root => this.root;

    public DictionaryBase InnerHashtable => (DictionaryBase) this;

    protected void BaseAdd(string name, DTDNode value) => this.Add(new KeyValuePair<string, DTDNode>(name, value));

    public bool Contains(string key)
    {
      foreach (KeyValuePair<string, DTDNode> keyValuePair in (List<KeyValuePair<string, DTDNode>>) this)
      {
        if (keyValuePair.Key == key)
          return true;
      }
      return false;
    }

    protected object BaseGet(string name)
    {
      foreach (KeyValuePair<string, DTDNode> keyValuePair in (List<KeyValuePair<string, DTDNode>>) this)
      {
        if (keyValuePair.Key == name)
          return (object) keyValuePair.Value;
      }
      return (object) null;
    }
  }
}
