// Decompiled with JetBrains decompiler
// Type: Mono.Xml.DTDElementAutomata
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

namespace Mono.Xml
{
  internal class DTDElementAutomata : DTDAutomata
  {
    private string name;

    public DTDElementAutomata(DTDObjectModel root, string name)
      : base(root)
    {
      this.name = name;
    }

    public string Name => this.name;

    public override DTDAutomata TryStartElement(string name) => name == this.Name ? (DTDAutomata) this.Root.Empty : (DTDAutomata) this.Root.Invalid;
  }
}
