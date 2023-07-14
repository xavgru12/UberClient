// Decompiled with JetBrains decompiler
// Type: Mono.Xml.DTDParameterEntityDeclarationCollection
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections;

namespace Mono.Xml
{
  internal class DTDParameterEntityDeclarationCollection
  {
    private Hashtable peDecls = new Hashtable();
    private DTDObjectModel root;

    public DTDParameterEntityDeclarationCollection(DTDObjectModel root) => this.root = root;

    public DTDParameterEntityDeclaration this[string name] => this.peDecls[(object) name] as DTDParameterEntityDeclaration;

    public void Add(string name, DTDParameterEntityDeclaration decl)
    {
      if (this.peDecls[(object) name] != null)
        return;
      decl.SetRoot(this.root);
      this.peDecls.Add((object) name, (object) decl);
    }

    public ICollection Keys => this.peDecls.Keys;

    public ICollection Values => this.peDecls.Values;
  }
}
