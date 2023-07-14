// Decompiled with JetBrains decompiler
// Type: Mono.Xml.DTDAutomataFactory
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections;

namespace Mono.Xml
{
  internal class DTDAutomataFactory
  {
    private DTDObjectModel root;
    private Hashtable choiceTable = new Hashtable();
    private Hashtable sequenceTable = new Hashtable();

    public DTDAutomataFactory(DTDObjectModel root) => this.root = root;

    public DTDChoiceAutomata Choice(DTDAutomata left, DTDAutomata right)
    {
      if (!(this.choiceTable[(object) left] is Hashtable hashtable))
      {
        hashtable = new Hashtable();
        this.choiceTable[(object) left] = (object) hashtable;
      }
      if (!(hashtable[(object) right] is DTDChoiceAutomata dtdChoiceAutomata))
      {
        dtdChoiceAutomata = new DTDChoiceAutomata(this.root, left, right);
        hashtable[(object) right] = (object) dtdChoiceAutomata;
      }
      return dtdChoiceAutomata;
    }

    public DTDSequenceAutomata Sequence(DTDAutomata left, DTDAutomata right)
    {
      if (!(this.sequenceTable[(object) left] is Hashtable hashtable))
      {
        hashtable = new Hashtable();
        this.sequenceTable[(object) left] = (object) hashtable;
      }
      if (!(hashtable[(object) right] is DTDSequenceAutomata sequenceAutomata))
      {
        sequenceAutomata = new DTDSequenceAutomata(this.root, left, right);
        hashtable[(object) right] = (object) sequenceAutomata;
      }
      return sequenceAutomata;
    }
  }
}
