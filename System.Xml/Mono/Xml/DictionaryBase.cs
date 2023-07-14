// Decompiled with JetBrains decompiler
// Type: Mono.Xml.DictionaryBase
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections.Generic;

namespace Mono.Xml
{
  internal class DictionaryBase : List<KeyValuePair<string, DTDNode>>
  {
    public IEnumerable<DTDNode> Values
    {
      get
      {
        DictionaryBase.\u003C\u003Ec__Iterator3 values = new DictionaryBase.\u003C\u003Ec__Iterator3()
        {
          \u003C\u003Ef__this = this
        };
        values.\u0024PC = -2;
        return (IEnumerable<DTDNode>) values;
      }
    }
  }
}
