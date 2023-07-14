// Decompiled with JetBrains decompiler
// Type: Mono.Xml.DTDSequenceAutomata
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

namespace Mono.Xml
{
  internal class DTDSequenceAutomata : DTDAutomata
  {
    private DTDAutomata left;
    private DTDAutomata right;
    private bool hasComputedEmptiable;
    private bool cachedEmptiable;

    public DTDSequenceAutomata(DTDObjectModel root, DTDAutomata left, DTDAutomata right)
      : base(root)
    {
      this.left = left;
      this.right = right;
    }

    public DTDAutomata Left => this.left;

    public DTDAutomata Right => this.right;

    public override DTDAutomata TryStartElement(string name)
    {
      DTDAutomata dtdAutomata1 = this.left.TryStartElement(name);
      DTDAutomata dtdAutomata2 = this.right.TryStartElement(name);
      if (dtdAutomata1 == this.Root.Invalid)
        return this.left.Emptiable ? dtdAutomata2 : dtdAutomata1;
      DTDAutomata other = dtdAutomata1.MakeSequence(this.right);
      return this.left.Emptiable ? dtdAutomata2.MakeChoice(other) : other;
    }

    public override DTDAutomata TryEndElement() => this.left.Emptiable ? this.right : (DTDAutomata) this.Root.Invalid;

    public override bool Emptiable
    {
      get
      {
        if (!this.hasComputedEmptiable)
        {
          this.cachedEmptiable = this.left.Emptiable && this.right.Emptiable;
          this.hasComputedEmptiable = true;
        }
        return this.cachedEmptiable;
      }
    }
  }
}
