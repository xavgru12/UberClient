// Decompiled with JetBrains decompiler
// Type: System.Xml.Serialization.XmlTypeMapMemberElement
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections;

namespace System.Xml.Serialization
{
  internal class XmlTypeMapMemberElement : XmlTypeMapMember
  {
    private XmlTypeMapElementInfoList _elementInfo;
    private string _choiceMember;
    private bool _isTextCollector;
    private TypeData _choiceTypeData;

    public XmlTypeMapElementInfoList ElementInfo
    {
      get
      {
        if (this._elementInfo == null)
          this._elementInfo = new XmlTypeMapElementInfoList();
        return this._elementInfo;
      }
      set => this._elementInfo = value;
    }

    public string ChoiceMember
    {
      get => this._choiceMember;
      set => this._choiceMember = value;
    }

    public TypeData ChoiceTypeData
    {
      get => this._choiceTypeData;
      set => this._choiceTypeData = value;
    }

    public XmlTypeMapElementInfo FindElement(object ob, object memberValue)
    {
      if (this._elementInfo.Count == 1)
        return (XmlTypeMapElementInfo) this._elementInfo[0];
      if (this._choiceMember != null)
      {
        object obj = XmlTypeMapMember.GetValue(ob, this._choiceMember);
        foreach (XmlTypeMapElementInfo element in (ArrayList) this._elementInfo)
        {
          if (element.ChoiceValue != null && element.ChoiceValue.Equals(obj))
            return element;
        }
      }
      else
      {
        if (memberValue == null)
          return (XmlTypeMapElementInfo) this._elementInfo[0];
        foreach (XmlTypeMapElementInfo element in (ArrayList) this._elementInfo)
        {
          if (element.TypeData.Type.IsInstanceOfType(memberValue))
            return element;
        }
      }
      return (XmlTypeMapElementInfo) null;
    }

    public void SetChoice(object ob, object choice) => XmlTypeMapMember.SetValue(ob, this._choiceMember, choice);

    public bool IsXmlTextCollector
    {
      get => this._isTextCollector;
      set => this._isTextCollector = value;
    }

    public override bool RequiresNullable
    {
      get
      {
        foreach (XmlTypeMapElementInfo typeMapElementInfo in (ArrayList) this.ElementInfo)
        {
          if (typeMapElementInfo.IsNullable)
            return true;
        }
        return false;
      }
    }
  }
}
