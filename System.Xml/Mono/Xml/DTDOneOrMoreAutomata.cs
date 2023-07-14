// Decompiled with JetBrains decompiler
// Type: Mono.Xml.DTDOneOrMoreAutomata
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

namespace Mono.Xml
{
  internal class DTDOneOrMoreAutomata : DTDAutomata
  {
    private DTDAutomata children;

    public DTDOneOrMoreAutomata(DTDObjectModel root, DTDAutomata children)
      : base(root)
    {
      this.children = children;
    }

    public DTDAutomata Children => this.children;

    public override DTDAutomata TryStartElement(string name)
    {
      DTDAutomata dtdAutomata = this.children.TryStartElement(name);
      return dtdAutomata != this.Root.Invalid ? dtdAutomata.MakeSequence(this.Root.Empty.MakeChoice((DTDAutomata) this)) : (DTDAutomata) this.Root.Invalid;
    }

    public override DTDAutomata TryEndElement() => this.Emptiable ? this.children.TryEndElement() : (DTDAutomata) this.Root.Invalid;
  }
}
