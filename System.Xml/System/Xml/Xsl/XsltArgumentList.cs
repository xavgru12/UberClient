// Decompiled with JetBrains decompiler
// Type: System.Xml.Xsl.XsltArgumentList
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections;
using System.Xml.XPath;

namespace System.Xml.Xsl
{
  public class XsltArgumentList
  {
    internal Hashtable extensionObjects;
    internal Hashtable parameters;

    public XsltArgumentList()
    {
      this.extensionObjects = new Hashtable();
      this.parameters = new Hashtable();
    }

    public event XsltMessageEncounteredEventHandler XsltMessageEncountered;

    public void AddExtensionObject(string namespaceUri, object extension)
    {
      switch (namespaceUri)
      {
        case null:
          throw new ArgumentException("The namespaceUri is a null reference.");
        case "http://www.w3.org/1999/XSL/Transform":
          throw new ArgumentException("The namespaceUri is http://www.w3.org/1999/XSL/Transform.");
        default:
          if (this.extensionObjects.Contains((object) namespaceUri))
            throw new ArgumentException("The namespaceUri already has an extension object associated with it.");
          this.extensionObjects[(object) namespaceUri] = extension;
          break;
      }
    }

    public void AddParam(string name, string namespaceUri, object parameter)
    {
      switch (namespaceUri)
      {
        case null:
          throw new ArgumentException("The namespaceUri is a null reference.");
        case "http://www.w3.org/1999/XSL/Transform":
          throw new ArgumentException("The namespaceUri is http://www.w3.org/1999/XSL/Transform.");
        default:
          XmlQualifiedName key = name != null ? new XmlQualifiedName(name, namespaceUri) : throw new ArgumentException("The parameter name is a null reference.");
          if (this.parameters.Contains((object) key))
            throw new ArgumentException("The namespaceUri already has a parameter associated with it.");
          parameter = this.ValidateParam(parameter);
          this.parameters[(object) key] = parameter;
          break;
      }
    }

    public void Clear()
    {
      this.extensionObjects.Clear();
      this.parameters.Clear();
    }

    public object GetExtensionObject(string namespaceUri) => this.extensionObjects[(object) namespaceUri];

    public object GetParam(string name, string namespaceUri)
    {
      if (name == null)
        throw new ArgumentException("The parameter name is a null reference.");
      return this.parameters[(object) new XmlQualifiedName(name, namespaceUri)];
    }

    public object RemoveExtensionObject(string namespaceUri)
    {
      object extensionObject = this.GetExtensionObject(namespaceUri);
      this.extensionObjects.Remove((object) namespaceUri);
      return extensionObject;
    }

    public object RemoveParam(string name, string namespaceUri)
    {
      XmlQualifiedName key = new XmlQualifiedName(name, namespaceUri);
      object obj = this.GetParam(name, namespaceUri);
      this.parameters.Remove((object) key);
      return obj;
    }

    private object ValidateParam(object parameter)
    {
      switch (parameter)
      {
        case string _:
          return parameter;
        case bool _:
          return parameter;
        case double _:
          return parameter;
        case XPathNavigator _:
          return parameter;
        case XPathNodeIterator _:
          return parameter;
        case short num1:
          return (object) (double) num1;
        case ushort num2:
          return (object) (double) num2;
        case int num3:
          return (object) (double) num3;
        case long num4:
          return (object) (double) num4;
        case ulong num5:
          return (object) (double) num5;
        case float num6:
          return (object) (double) num6;
        case Decimal num7:
          return (object) (double) num7;
        default:
          return (object) parameter.ToString();
      }
    }
  }
}
