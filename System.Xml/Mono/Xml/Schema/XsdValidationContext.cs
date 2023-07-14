// Decompiled with JetBrains decompiler
// Type: Mono.Xml.Schema.XsdValidationContext
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections;
using System.Xml.Schema;

namespace Mono.Xml.Schema
{
  internal class XsdValidationContext
  {
    private object xsi_type;
    internal XsdValidationState State;
    private Stack element_stack = new Stack();

    public object XsiType
    {
      get => this.xsi_type;
      set => this.xsi_type = value;
    }

    public XmlSchemaElement Element => this.element_stack.Count > 0 ? this.element_stack.Peek() as XmlSchemaElement : (XmlSchemaElement) null;

    public void PushCurrentElement(XmlSchemaElement element) => this.element_stack.Push((object) element);

    public void PopCurrentElement() => this.element_stack.Pop();

    public object ActualType
    {
      get
      {
        if (this.element_stack.Count == 0)
          return (object) null;
        if (this.XsiType != null)
          return this.XsiType;
        return this.Element != null ? this.Element.ElementType : (object) null;
      }
    }

    public XmlSchemaType ActualSchemaType
    {
      get
      {
        object actualType = this.ActualType;
        if (actualType == null)
          return (XmlSchemaType) null;
        if (!(actualType is XmlSchemaType actualSchemaType))
          actualSchemaType = (XmlSchemaType) XmlSchemaType.GetBuiltInSimpleType(((XmlSchemaDatatype) actualType).TypeCode);
        return actualSchemaType;
      }
    }

    public bool IsInvalid => this.State == XsdValidationState.Invalid;

    public object Clone() => this.MemberwiseClone();

    public void EvaluateStartElement(string localName, string ns) => this.State = this.State.EvaluateStartElement(localName, ns);

    public bool EvaluateEndElement() => this.State.EvaluateEndElement();
  }
}
