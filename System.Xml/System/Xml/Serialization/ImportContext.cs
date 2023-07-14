// Decompiled with JetBrains decompiler
// Type: System.Xml.Serialization.ImportContext
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections;
using System.Collections.Specialized;

namespace System.Xml.Serialization
{
  public class ImportContext
  {
    private bool _shareTypes;
    private CodeIdentifiers _typeIdentifiers;
    private StringCollection _warnings = new StringCollection();
    internal Hashtable MappedTypes;
    internal Hashtable DataMappedTypes;
    internal Hashtable SharedAnonymousTypes;

    public ImportContext(CodeIdentifiers identifiers, bool shareTypes)
    {
      this._typeIdentifiers = identifiers;
      this._shareTypes = shareTypes;
      if (!shareTypes)
        return;
      this.MappedTypes = new Hashtable();
      this.DataMappedTypes = new Hashtable();
      this.SharedAnonymousTypes = new Hashtable();
    }

    public bool ShareTypes => this._shareTypes;

    public CodeIdentifiers TypeIdentifiers => this._typeIdentifiers;

    public StringCollection Warnings => this._warnings;
  }
}
