// Decompiled with JetBrains decompiler
// Type: Mono.Xml.Schema.XsdParticleStateManager
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;

namespace Mono.Xml.Schema
{
  internal class XsdParticleStateManager
  {
    private Hashtable table;
    private XmlSchemaContentProcessing processContents;
    public XmlSchemaElement CurrentElement;
    public Stack ContextStack = new Stack();
    public XsdValidationContext Context = new XsdValidationContext();

    public XsdParticleStateManager()
    {
      this.table = new Hashtable();
      this.processContents = XmlSchemaContentProcessing.Strict;
    }

    public XmlSchemaContentProcessing ProcessContents => this.processContents;

    public void PushContext() => this.ContextStack.Push(this.Context.Clone());

    public void PopContext() => this.Context = (XsdValidationContext) this.ContextStack.Pop();

    internal void SetProcessContents(XmlSchemaContentProcessing value) => this.processContents = value;

    public XsdValidationState Get(XmlSchemaParticle xsobj)
    {
      if (!(this.table[(object) xsobj] is XsdValidationState xsdValidationState))
        xsdValidationState = this.Create((XmlSchemaObject) xsobj);
      return xsdValidationState;
    }

    public XsdValidationState Create(XmlSchemaObject xsobj)
    {
      string name = xsobj.GetType().Name;
      if (name != null)
      {
        // ISSUE: reference to a compiler-generated field
        if (XsdParticleStateManager.\u003C\u003Ef__switch\u0024map2 == null)
        {
          // ISSUE: reference to a compiler-generated field
          XsdParticleStateManager.\u003C\u003Ef__switch\u0024map2 = new Dictionary<string, int>(6)
          {
            {
              "XmlSchemaElement",
              0
            },
            {
              "XmlSchemaSequence",
              1
            },
            {
              "XmlSchemaChoice",
              2
            },
            {
              "XmlSchemaAll",
              3
            },
            {
              "XmlSchemaAny",
              4
            },
            {
              "EmptyParticle",
              5
            }
          };
        }
        int num;
        // ISSUE: reference to a compiler-generated field
        if (XsdParticleStateManager.\u003C\u003Ef__switch\u0024map2.TryGetValue(name, out num))
        {
          switch (num)
          {
            case 0:
              return (XsdValidationState) this.AddElement((XmlSchemaElement) xsobj);
            case 1:
              return (XsdValidationState) this.AddSequence((XmlSchemaSequence) xsobj);
            case 2:
              return (XsdValidationState) this.AddChoice((XmlSchemaChoice) xsobj);
            case 3:
              return (XsdValidationState) this.AddAll((XmlSchemaAll) xsobj);
            case 4:
              return (XsdValidationState) this.AddAny((XmlSchemaAny) xsobj);
            case 5:
              return (XsdValidationState) this.AddEmpty();
          }
        }
      }
      throw new InvalidOperationException("Should not occur.");
    }

    internal XsdValidationState MakeSequence(XsdValidationState head, XsdValidationState rest) => head is XsdEmptyValidationState ? rest : (XsdValidationState) new XsdAppendedValidationState(this, head, rest);

    private XsdElementValidationState AddElement(XmlSchemaElement element) => new XsdElementValidationState(element, this);

    private XsdSequenceValidationState AddSequence(XmlSchemaSequence sequence) => new XsdSequenceValidationState(sequence, this);

    private XsdChoiceValidationState AddChoice(XmlSchemaChoice choice) => new XsdChoiceValidationState(choice, this);

    private XsdAllValidationState AddAll(XmlSchemaAll all) => new XsdAllValidationState(all, this);

    private XsdAnyValidationState AddAny(XmlSchemaAny any) => new XsdAnyValidationState(any, this);

    private XsdEmptyValidationState AddEmpty() => new XsdEmptyValidationState(this);
  }
}
