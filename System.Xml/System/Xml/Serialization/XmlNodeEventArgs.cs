// Decompiled with JetBrains decompiler
// Type: System.Xml.Serialization.XmlNodeEventArgs
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

namespace System.Xml.Serialization
{
  public class XmlNodeEventArgs : EventArgs
  {
    private int linenumber;
    private int lineposition;
    private string localname;
    private string name;
    private string nsuri;
    private XmlNodeType nodetype;
    private object source;
    private string text;

    internal XmlNodeEventArgs(
      int linenumber,
      int lineposition,
      string localname,
      string name,
      string nsuri,
      XmlNodeType nodetype,
      object source,
      string text)
    {
      this.linenumber = linenumber;
      this.lineposition = lineposition;
      this.localname = localname;
      this.name = name;
      this.nsuri = nsuri;
      this.nodetype = nodetype;
      this.source = source;
      this.text = text;
    }

    public int LineNumber => this.linenumber;

    public int LinePosition => this.lineposition;

    public string LocalName => this.localname;

    public string Name => this.name;

    public string NamespaceURI => this.nsuri;

    public XmlNodeType NodeType => this.nodetype;

    public object ObjectBeingDeserialized => this.source;

    public string Text => this.text;
  }
}
