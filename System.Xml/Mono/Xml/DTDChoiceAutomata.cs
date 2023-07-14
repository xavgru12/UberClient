// Decompiled with JetBrains decompiler
// Type: Mono.Xml.DTDChoiceAutomata
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

namespace Mono.Xml
{
  internal class DTDChoiceAutomata : DTDAutomata
  {
    private DTDAutomata left;
    private DTDAutomata right;
    private bool hasComputedEmptiable;
    private bool cachedEmptiable;

    public DTDChoiceAutomata(DTDObjectModel root, DTDAutomata left, DTDAutomata right)
      : base(root)
    {
      this.left = left;
      this.right = right;
    }

    public DTDAutomata Left => this.left;

    public DTDAutomata Right => this.right;

    public override DTDAutomata TryStartElement(string name) => this.left.TryStartElement(name).MakeChoice(this.right.TryStartElement(name));

    public override DTDAutomata TryEndElement() => this.left.TryEndElement().MakeChoice(this.right.TryEndElement());

    public override bool Emptiable
    {
      get
      {
        if (!this.hasComputedEmptiable)
        {
          this.cachedEmptiable = this.left.Emptiable || this.right.Emptiable;
          this.hasComputedEmptiable = true;
        }
        return this.cachedEmptiable;
      }
    }
  }
}
