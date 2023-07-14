// Decompiled with JetBrains decompiler
// Type: System.Xml.XmlImplementation
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections.Generic;
using System.Globalization;

namespace System.Xml
{
  public class XmlImplementation
  {
    internal XmlNameTable InternalNameTable;

    public XmlImplementation()
      : this((XmlNameTable) new NameTable())
    {
    }

    public XmlImplementation(XmlNameTable nameTable) => this.InternalNameTable = nameTable;

    public virtual XmlDocument CreateDocument() => new XmlDocument(this);

    public bool HasFeature(string strFeature, string strVersion)
    {
      if (string.Compare(strFeature, "xml", true, CultureInfo.InvariantCulture) == 0)
      {
        string key = strVersion;
        if (key != null)
        {
          // ISSUE: reference to a compiler-generated field
          if (XmlImplementation.\u003C\u003Ef__switch\u0024map4C == null)
          {
            // ISSUE: reference to a compiler-generated field
            XmlImplementation.\u003C\u003Ef__switch\u0024map4C = new Dictionary<string, int>(2)
            {
              {
                "1.0",
                0
              },
              {
                "2.0",
                0
              }
            };
          }
          int num;
          // ISSUE: reference to a compiler-generated field
          if (!XmlImplementation.\u003C\u003Ef__switch\u0024map4C.TryGetValue(key, out num) || num != 0)
            goto label_6;
        }
        return true;
      }
label_6:
      return false;
    }
  }
}
