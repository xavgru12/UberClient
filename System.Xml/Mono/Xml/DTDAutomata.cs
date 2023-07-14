// Decompiled with JetBrains decompiler
// Type: Mono.Xml.DTDAutomata
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

namespace Mono.Xml
{
  internal abstract class DTDAutomata
  {
    private DTDObjectModel root;

    public DTDAutomata(DTDObjectModel root) => this.root = root;

    public DTDObjectModel Root => this.root;

    public DTDAutomata MakeChoice(DTDAutomata other)
    {
      if (this == this.Root.Invalid)
        return other;
      if (other == this.Root.Invalid || this == this.Root.Empty && other == this.Root.Empty || this == this.Root.Any && other == this.Root.Any)
        return this;
      return other == this.Root.Empty ? (DTDAutomata) this.Root.Factory.Choice(other, this) : (DTDAutomata) this.Root.Factory.Choice(this, other);
    }

    public DTDAutomata MakeSequence(DTDAutomata other)
    {
      if (this == this.Root.Invalid || other == this.Root.Invalid)
        return (DTDAutomata) this.Root.Invalid;
      if (this == this.Root.Empty)
        return other;
      return other == this.Root.Empty ? this : (DTDAutomata) this.Root.Factory.Sequence(this, other);
    }

    public abstract DTDAutomata TryStartElement(string name);

    public virtual DTDAutomata TryEndElement() => (DTDAutomata) this.Root.Invalid;

    public virtual bool Emptiable => false;
  }
}
