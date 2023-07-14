// Decompiled with JetBrains decompiler
// Type: System.Xml.Serialization.SerializerInfo
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections;
using System.ComponentModel;

namespace System.Xml.Serialization
{
  [XmlType("serializer")]
  internal class SerializerInfo
  {
    [XmlAttribute("class")]
    public string ClassName;
    [XmlAttribute("assembly")]
    public string Assembly;
    [XmlElement("reader")]
    public string ReaderClassName;
    [XmlElement("writer")]
    public string WriterClassName;
    [XmlElement("baseSerializer")]
    public string BaseSerializerClassName;
    [XmlElement("implementation")]
    public string ImplementationClassName;
    [XmlElement("noreader")]
    public bool NoReader;
    [XmlElement("nowriter")]
    public bool NoWriter;
    [XmlElement("generateAsInternal")]
    public bool GenerateAsInternal;
    [XmlElement("namespace")]
    public string Namespace;
    [XmlArray("namespaceImports")]
    [XmlArrayItem("namespaceImport")]
    public string[] NamespaceImports;
    [DefaultValue(SerializationFormat.Literal)]
    public SerializationFormat SerializationFormat = SerializationFormat.Literal;
    [XmlElement("outFileName")]
    public string OutFileName;
    [XmlArray("readerHooks")]
    public Hook[] ReaderHooks;
    [XmlArray("writerHooks")]
    public Hook[] WriterHooks;

    public ArrayList GetHooks(
      HookType hookType,
      XmlMappingAccess dir,
      HookAction action,
      Type type,
      string member)
    {
      if ((dir & XmlMappingAccess.Read) != XmlMappingAccess.None)
        return this.FindHook(this.ReaderHooks, hookType, action, type, member);
      if ((dir & XmlMappingAccess.Write) != XmlMappingAccess.None)
        return this.FindHook(this.WriterHooks, hookType, action, type, member);
      throw new Exception("INTERNAL ERROR");
    }

    private ArrayList FindHook(
      Hook[] hooks,
      HookType hookType,
      HookAction action,
      Type type,
      string member)
    {
      ArrayList hook1 = new ArrayList();
      if (hooks == null)
        return hook1;
      foreach (Hook hook2 in hooks)
      {
        if ((action != HookAction.InsertBefore || hook2.InsertBefore != null && !(hook2.InsertBefore == string.Empty)) && (action != HookAction.InsertAfter || hook2.InsertAfter != null && !(hook2.InsertAfter == string.Empty)) && (action != HookAction.Replace || hook2.Replace != null && !(hook2.Replace == string.Empty)) && hook2.HookType == hookType)
        {
          if (hook2.Select != null)
          {
            if ((hook2.Select.TypeName == null || !(hook2.Select.TypeName != string.Empty) || !(hook2.Select.TypeName != type.FullName)) && (hook2.Select.TypeMember == null || !(hook2.Select.TypeMember != string.Empty) || !(hook2.Select.TypeMember != member)))
            {
              if (hook2.Select.TypeAttributes != null && hook2.Select.TypeAttributes.Length > 0)
              {
                object[] customAttributes = type.GetCustomAttributes(true);
                bool flag = false;
                foreach (object obj in customAttributes)
                {
                  if (Array.IndexOf<string>(hook2.Select.TypeAttributes, obj.GetType().FullName) != -1)
                  {
                    flag = true;
                    break;
                  }
                }
                if (!flag)
                  continue;
              }
            }
            else
              continue;
          }
          hook1.Add((object) hook2);
        }
      }
      return hook1;
    }
  }
}
