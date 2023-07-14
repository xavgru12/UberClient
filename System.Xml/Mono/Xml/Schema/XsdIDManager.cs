// Decompiled with JetBrains decompiler
// Type: Mono.Xml.Schema.XsdIDManager
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections;
using System.Xml;
using System.Xml.Schema;

namespace Mono.Xml.Schema
{
  internal class XsdIDManager
  {
    private Hashtable idList = new Hashtable();
    private ArrayList missingIDReferences;
    private string thisElementId;

    private ArrayList MissingIDReferences
    {
      get
      {
        if (this.missingIDReferences == null)
          this.missingIDReferences = new ArrayList();
        return this.missingIDReferences;
      }
    }

    public void OnStartElement() => this.thisElementId = (string) null;

    public string AssessEachAttributeIdentityConstraint(
      XmlSchemaDatatype dt,
      object parsedValue,
      string elementName)
    {
      string key1 = parsedValue as string;
      switch (dt.TokenizedType)
      {
        case XmlTokenizedType.ID:
          if (this.thisElementId != null)
            return "ID type attribute was already assigned in the containing element.";
          this.thisElementId = key1;
          if (this.idList.ContainsKey((object) key1))
            return "Duplicate ID value was found.";
          this.idList.Add((object) key1, (object) elementName);
          if (this.MissingIDReferences.Contains((object) key1))
          {
            this.MissingIDReferences.Remove((object) key1);
            break;
          }
          break;
        case XmlTokenizedType.IDREF:
          if (!this.idList.Contains((object) key1))
          {
            this.MissingIDReferences.Add((object) key1);
            break;
          }
          break;
        case XmlTokenizedType.IDREFS:
          foreach (string key2 in (string[]) parsedValue)
          {
            if (!this.idList.Contains((object) key2))
              this.MissingIDReferences.Add((object) key2);
          }
          break;
      }
      return (string) null;
    }

    public bool HasMissingIDReferences() => this.missingIDReferences != null && this.missingIDReferences.Count > 0;

    public string GetMissingIDString() => string.Join(" ", this.MissingIDReferences.ToArray(typeof (string)) as string[]);
  }
}
