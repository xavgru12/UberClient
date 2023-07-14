// Decompiled with JetBrains decompiler
// Type: System.Xml.Serialization.XmlElementEventArgs
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

namespace System.Xml.Serialization
{
  public class XmlElementEventArgs : EventArgs
  {
    private XmlElement attr;
    private int lineNumber;
    private int linePosition;
    private object obj;
    private string expectedElements;

    internal XmlElementEventArgs(XmlElement attr, int lineNum, int linePos, object source)
    {
      this.attr = attr;
      this.lineNumber = lineNum;
      this.linePosition = linePos;
      this.obj = source;
    }

    public XmlElement Element => this.attr;

    public int LineNumber => this.lineNumber;

    public int LinePosition => this.linePosition;

    public object ObjectBeingDeserialized => this.obj;

    public string ExpectedElements
    {
      get => this.expectedElements;
      internal set => this.expectedElements = value;
    }
  }
}
