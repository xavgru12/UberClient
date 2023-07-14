// Decompiled with JetBrains decompiler
// Type: System.Xml.XmlNodeChangedEventArgs
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

namespace System.Xml
{
  public class XmlNodeChangedEventArgs : EventArgs
  {
    private XmlNode _oldParent;
    private XmlNode _newParent;
    private XmlNodeChangedAction _action;
    private XmlNode _node;
    private string _oldValue;
    private string _newValue;

    public XmlNodeChangedEventArgs(
      XmlNode node,
      XmlNode oldParent,
      XmlNode newParent,
      string oldValue,
      string newValue,
      XmlNodeChangedAction action)
    {
      this._node = node;
      this._oldParent = oldParent;
      this._newParent = newParent;
      this._oldValue = oldValue;
      this._newValue = newValue;
      this._action = action;
    }

    public XmlNodeChangedAction Action => this._action;

    public XmlNode Node => this._node;

    public XmlNode OldParent => this._oldParent;

    public XmlNode NewParent => this._newParent;

    public string OldValue => this._oldValue != null ? this._oldValue : this._node.Value;

    public string NewValue => this._newValue != null ? this._newValue : this._node.Value;
  }
}
