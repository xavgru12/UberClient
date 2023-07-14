// Decompiled with JetBrains decompiler
// Type: System.Xml.Serialization.XmlSerializationReader
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections;
using System.Globalization;
using System.Reflection;

namespace System.Xml.Serialization
{
  [MonoTODO]
  public abstract class XmlSerializationReader : XmlSerializationGeneratedCode
  {
    private XmlDocument document;
    private XmlReader reader;
    private ArrayList fixups;
    private Hashtable collFixups;
    private ArrayList collItemFixups;
    private Hashtable typesCallbacks;
    private ArrayList noIDTargets;
    private Hashtable targets;
    private Hashtable delayedListFixups;
    private XmlSerializer eventSource;
    private int delayedFixupId;
    private Hashtable referencedObjects;
    private int readCount;
    private int whileIterationCount;
    private string w3SchemaNS;
    private string w3InstanceNS;
    private string w3InstanceNS2000;
    private string w3InstanceNS1999;
    private string soapNS;
    private string wsdlNS;
    private string nullX;
    private string nil;
    private string typeX;
    private string arrayType;
    private XmlQualifiedName arrayQName;

    internal void Initialize(XmlReader reader, XmlSerializer eventSource)
    {
      this.w3SchemaNS = reader.NameTable.Add("http://www.w3.org/2001/XMLSchema");
      this.w3InstanceNS = reader.NameTable.Add("http://www.w3.org/2001/XMLSchema-instance");
      this.w3InstanceNS2000 = reader.NameTable.Add("http://www.w3.org/2000/10/XMLSchema-instance");
      this.w3InstanceNS1999 = reader.NameTable.Add("http://www.w3.org/1999/XMLSchema-instance");
      this.soapNS = reader.NameTable.Add("http://schemas.xmlsoap.org/soap/encoding/");
      this.wsdlNS = reader.NameTable.Add("http://schemas.xmlsoap.org/wsdl/");
      this.nullX = reader.NameTable.Add("null");
      this.nil = reader.NameTable.Add("nil");
      this.typeX = reader.NameTable.Add("type");
      this.arrayType = reader.NameTable.Add("arrayType");
      this.reader = reader;
      this.eventSource = eventSource;
      this.arrayQName = new XmlQualifiedName("Array", this.soapNS);
      this.InitIDs();
    }

    private ArrayList EnsureArrayList(ArrayList list)
    {
      if (list == null)
        list = new ArrayList();
      return list;
    }

    private Hashtable EnsureHashtable(Hashtable hash)
    {
      if (hash == null)
        hash = new Hashtable();
      return hash;
    }

    protected XmlDocument Document
    {
      get
      {
        if (this.document == null)
          this.document = new XmlDocument(this.reader.NameTable);
        return this.document;
      }
    }

    protected XmlReader Reader => this.reader;

    [MonoTODO]
    protected bool IsReturnValue
    {
      get => throw new NotImplementedException();
      set => throw new NotImplementedException();
    }

    protected int ReaderCount => this.readCount;

    protected void AddFixup(XmlSerializationReader.CollectionFixup fixup)
    {
      this.collFixups = this.EnsureHashtable(this.collFixups);
      this.collFixups[fixup.Id] = (object) fixup;
      if (this.delayedListFixups == null || !this.delayedListFixups.ContainsKey(fixup.Id))
        return;
      fixup.CollectionItems = this.delayedListFixups[fixup.Id];
      this.delayedListFixups.Remove(fixup.Id);
    }

    protected void AddFixup(XmlSerializationReader.Fixup fixup)
    {
      this.fixups = this.EnsureArrayList(this.fixups);
      this.fixups.Add((object) fixup);
    }

    private void AddFixup(XmlSerializationReader.CollectionItemFixup fixup)
    {
      this.collItemFixups = this.EnsureArrayList(this.collItemFixups);
      this.collItemFixups.Add((object) fixup);
    }

    protected void AddReadCallback(
      string name,
      string ns,
      Type type,
      XmlSerializationReadCallback read)
    {
      XmlSerializationReader.WriteCallbackInfo writeCallbackInfo = new XmlSerializationReader.WriteCallbackInfo();
      writeCallbackInfo.Type = type;
      writeCallbackInfo.TypeName = name;
      writeCallbackInfo.TypeNs = ns;
      writeCallbackInfo.Callback = read;
      this.typesCallbacks = this.EnsureHashtable(this.typesCallbacks);
      this.typesCallbacks.Add((object) new XmlQualifiedName(name, ns), (object) writeCallbackInfo);
    }

    protected void AddTarget(string id, object o)
    {
      if (id != null)
      {
        this.targets = this.EnsureHashtable(this.targets);
        if (this.targets[(object) id] != null)
          return;
        this.targets.Add((object) id, o);
      }
      else
      {
        if (o != null)
          return;
        this.noIDTargets = this.EnsureArrayList(this.noIDTargets);
        this.noIDTargets.Add(o);
      }
    }

    private string CurrentTag()
    {
      XmlNodeType nodeType = this.reader.NodeType;
      switch (nodeType)
      {
        case XmlNodeType.Element:
          return string.Format("<{0} xmlns='{1}'>", (object) this.reader.LocalName, (object) this.reader.NamespaceURI);
        case XmlNodeType.Attribute:
          return this.reader.Value;
        case XmlNodeType.Text:
          return "CDATA";
        case XmlNodeType.Entity:
          return "<?";
        case XmlNodeType.ProcessingInstruction:
          return "<--";
        default:
          return nodeType == XmlNodeType.EndElement ? ">" : "(unknown)";
      }
    }

    protected Exception CreateCtorHasSecurityException(string typeName) => (Exception) new InvalidOperationException(string.Format("The type '{0}' cannot be serialized because its parameterless constructor is decorated with declarative security permission attributes. Consider using imperative asserts or demands in the constructor.", (object) typeName));

    protected Exception CreateInaccessibleConstructorException(string typeName) => (Exception) new InvalidOperationException(string.Format("{0} cannot be serialized because it does not have a default public constructor.", (object) typeName));

    protected Exception CreateAbstractTypeException(string name, string ns) => (Exception) new InvalidOperationException("The specified type is abstrace: name='" + name + "' namespace='" + ns + "', at " + this.CurrentTag());

    protected Exception CreateInvalidCastException(Type type, object value) => (Exception) new InvalidCastException(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "Cannot assign object of type {0} to an object of type {1}.", (object) value.GetType(), (object) type));

    protected Exception CreateReadOnlyCollectionException(string name) => (Exception) new InvalidOperationException(string.Format("Could not serialize {0}. Default constructors are required for collections and enumerators.", (object) name));

    protected Exception CreateUnknownConstantException(string value, Type enumType) => (Exception) new InvalidOperationException(string.Format("'{0}' is not a valid value for {1}.", (object) value, (object) enumType));

    protected Exception CreateUnknownNodeException() => (Exception) new InvalidOperationException(this.CurrentTag() + " was not expected");

    protected Exception CreateUnknownTypeException(XmlQualifiedName type) => (Exception) new InvalidOperationException("The specified type was not recognized: name='" + type.Name + "' namespace='" + type.Namespace + "', at " + this.CurrentTag());

    protected void CheckReaderCount(ref int whileIterations, ref int readerCount)
    {
      whileIterations = this.whileIterationCount;
      readerCount = this.readCount;
    }

    protected Array EnsureArrayIndex(Array a, int index, Type elementType)
    {
      if (a != null && index < a.Length)
        return a;
      int length = a != null ? a.Length * 2 : 32;
      Array instance = Array.CreateInstance(elementType, length);
      if (a != null)
        Array.Copy(a, instance, index);
      return instance;
    }

    [MonoTODO]
    protected void FixupArrayRefs(object fixup) => throw new NotImplementedException();

    [MonoTODO]
    protected int GetArrayLength(string name, string ns) => throw new NotImplementedException();

    protected bool GetNullAttr() => (this.reader.GetAttribute(this.nullX, this.w3InstanceNS) ?? this.reader.GetAttribute(this.nil, this.w3InstanceNS) ?? this.reader.GetAttribute(this.nullX, this.w3InstanceNS2000) ?? this.reader.GetAttribute(this.nullX, this.w3InstanceNS1999)) != null;

    protected object GetTarget(string id)
    {
      if (this.targets == null)
        return (object) null;
      object target = this.targets[(object) id];
      if (target != null)
      {
        if (this.referencedObjects == null)
          this.referencedObjects = new Hashtable();
        this.referencedObjects[target] = target;
      }
      return target;
    }

    private bool TargetReady(string id) => this.targets != null && this.targets.ContainsKey((object) id);

    protected XmlQualifiedName GetXsiType()
    {
      string attribute = this.Reader.GetAttribute(this.typeX, "http://www.w3.org/2001/XMLSchema-instance");
      if (attribute == string.Empty || attribute == null)
      {
        attribute = this.Reader.GetAttribute(this.typeX, this.w3InstanceNS1999);
        if (attribute == string.Empty || attribute == null)
        {
          attribute = this.Reader.GetAttribute(this.typeX, this.w3InstanceNS2000);
          if (attribute == string.Empty || attribute == null)
            return (XmlQualifiedName) null;
        }
      }
      int length = attribute.IndexOf(":");
      if (length == -1)
        return new XmlQualifiedName(attribute, this.Reader.NamespaceURI);
      string prefix = attribute.Substring(0, length);
      return new XmlQualifiedName(attribute.Substring(length + 1), this.Reader.LookupNamespace(prefix));
    }

    protected abstract void InitCallbacks();

    protected abstract void InitIDs();

    protected bool IsXmlnsAttribute(string name)
    {
      int length = name.Length;
      if (length < 5)
        return false;
      return length == 5 ? name == "xmlns" : name.StartsWith("xmlns:");
    }

    protected void ParseWsdlArrayType(XmlAttribute attr)
    {
      if (!(attr.NamespaceURI == this.wsdlNS) || !(attr.LocalName == this.arrayType))
        return;
      string ns = string.Empty;
      string type;
      string dimensions;
      TypeTranslator.ParseArrayType(attr.Value, out type, out ns, out dimensions);
      if (ns != string.Empty)
        ns = this.Reader.LookupNamespace(ns) + ":";
      attr.Value = ns + type + dimensions;
    }

    protected XmlQualifiedName ReadElementQualifiedName()
    {
      ++this.readCount;
      if (this.reader.IsEmptyElement)
      {
        this.reader.Skip();
        return this.ToXmlQualifiedName(string.Empty);
      }
      this.reader.ReadStartElement();
      XmlQualifiedName xmlQualifiedName = this.ToXmlQualifiedName(this.reader.ReadString());
      this.reader.ReadEndElement();
      return xmlQualifiedName;
    }

    protected void ReadEndElement()
    {
      ++this.readCount;
      while (this.reader.NodeType == XmlNodeType.Whitespace)
        this.reader.Skip();
      if (this.reader.NodeType != XmlNodeType.None)
        this.reader.ReadEndElement();
      else
        this.reader.Skip();
    }

    protected bool ReadNull()
    {
      if (!this.GetNullAttr())
        return false;
      ++this.readCount;
      if (this.reader.IsEmptyElement)
      {
        this.reader.Skip();
        return true;
      }
      this.reader.ReadStartElement();
      while (this.reader.NodeType != XmlNodeType.EndElement)
        this.UnknownNode((object) null);
      this.ReadEndElement();
      return true;
    }

    protected XmlQualifiedName ReadNullableQualifiedName() => this.ReadNull() ? (XmlQualifiedName) null : this.ReadElementQualifiedName();

    protected string ReadNullableString()
    {
      if (this.ReadNull())
        return (string) null;
      ++this.readCount;
      return this.reader.ReadElementString();
    }

    protected bool ReadReference(out string fixupReference)
    {
      string attribute = this.reader.GetAttribute("href");
      if (attribute == null)
      {
        fixupReference = (string) null;
        return false;
      }
      fixupReference = attribute[0] == '#' ? attribute.Substring(1) : throw new InvalidOperationException("href not found: " + attribute);
      ++this.readCount;
      if (!this.reader.IsEmptyElement)
      {
        this.reader.ReadStartElement();
        this.ReadEndElement();
      }
      else
        this.reader.Skip();
      return true;
    }

    protected object ReadReferencedElement() => this.ReadReferencedElement(this.Reader.LocalName, this.Reader.NamespaceURI);

    private XmlSerializationReader.WriteCallbackInfo GetCallbackInfo(XmlQualifiedName qname)
    {
      if (this.typesCallbacks == null)
      {
        this.typesCallbacks = new Hashtable();
        this.InitCallbacks();
      }
      return (XmlSerializationReader.WriteCallbackInfo) this.typesCallbacks[(object) qname];
    }

    protected object ReadReferencedElement(string name, string ns)
    {
      XmlQualifiedName qname = this.GetXsiType();
      if (qname == (XmlQualifiedName) null)
        qname = new XmlQualifiedName(name, ns);
      string attribute1 = this.Reader.GetAttribute("id");
      string attribute2 = this.Reader.GetAttribute(this.arrayType, this.soapNS);
      object resultList;
      if (qname == this.arrayQName || attribute2 != null && attribute2.Length > 0)
      {
        XmlSerializationReader.CollectionFixup collFixup = this.collFixups == null ? (XmlSerializationReader.CollectionFixup) null : (XmlSerializationReader.CollectionFixup) this.collFixups[(object) attribute1];
        if (this.ReadList(out resultList))
        {
          if (collFixup != null)
          {
            collFixup.Callback(collFixup.Collection, resultList);
            this.collFixups.Remove((object) attribute1);
            resultList = collFixup.Collection;
          }
        }
        else if (collFixup != null)
        {
          collFixup.CollectionItems = (object) (object[]) resultList;
          resultList = collFixup.Collection;
        }
      }
      else
      {
        XmlSerializationReader.WriteCallbackInfo callbackInfo = this.GetCallbackInfo(qname);
        resultList = callbackInfo != null ? callbackInfo.Callback() : this.ReadTypedPrimitive(qname, attribute1 != null);
      }
      this.AddTarget(attribute1, resultList);
      return resultList;
    }

    private bool ReadList(out object resultList)
    {
      XmlQualifiedName xmlQualifiedName = this.ToXmlQualifiedName(this.Reader.GetAttribute(this.arrayType, this.soapNS) ?? this.Reader.GetAttribute(this.arrayType, this.wsdlNS));
      int num1 = xmlQualifiedName.Name.LastIndexOf('[');
      string str1 = xmlQualifiedName.Name.Substring(num1);
      string name = xmlQualifiedName.Name.Substring(0, num1);
      int length = int.Parse(str1.Substring(1, str1.Length - 2), (IFormatProvider) CultureInfo.InvariantCulture);
      int num2 = name.IndexOf('[');
      if (num2 == -1)
        num2 = name.Length;
      string str2 = name.Substring(0, num2);
      string typeName;
      if (xmlQualifiedName.Namespace == this.w3SchemaNS)
      {
        typeName = TypeTranslator.GetPrimitiveTypeData(str2).Type.FullName + name.Substring(num2);
      }
      else
      {
        XmlSerializationReader.WriteCallbackInfo callbackInfo = this.GetCallbackInfo(new XmlQualifiedName(str2, xmlQualifiedName.Namespace));
        typeName = callbackInfo.Type.FullName + name.Substring(num2) + ", " + callbackInfo.Type.Assembly.FullName;
      }
      Array instance = Array.CreateInstance(Type.GetType(typeName), length);
      bool flag = true;
      if (this.Reader.IsEmptyElement)
      {
        ++this.readCount;
        this.Reader.Skip();
      }
      else
      {
        this.Reader.ReadStartElement();
        for (int index = 0; index < length; ++index)
        {
          ++this.whileIterationCount;
          ++this.readCount;
          int content = (int) this.Reader.MoveToContent();
          string fixupReference;
          object obj = this.ReadReferencingElement(name, xmlQualifiedName.Namespace, out fixupReference);
          if (fixupReference == null)
          {
            instance.SetValue(obj, index);
          }
          else
          {
            this.AddFixup(new XmlSerializationReader.CollectionItemFixup(instance, index, fixupReference));
            flag = false;
          }
        }
        this.whileIterationCount = 0;
        this.Reader.ReadEndElement();
      }
      resultList = (object) instance;
      return flag;
    }

    protected void ReadReferencedElements()
    {
      int content1 = (int) this.reader.MoveToContent();
      for (XmlNodeType nodeType = this.reader.NodeType; nodeType != XmlNodeType.EndElement && nodeType != XmlNodeType.None; nodeType = this.reader.NodeType)
      {
        ++this.whileIterationCount;
        ++this.readCount;
        this.ReadReferencedElement();
        int content2 = (int) this.reader.MoveToContent();
      }
      this.whileIterationCount = 0;
      if (this.delayedListFixups != null)
      {
        foreach (DictionaryEntry delayedListFixup in this.delayedListFixups)
          this.AddTarget((string) delayedListFixup.Key, delayedListFixup.Value);
      }
      if (this.collItemFixups != null)
      {
        foreach (XmlSerializationReader.CollectionItemFixup collItemFixup in this.collItemFixups)
          collItemFixup.Collection.SetValue(this.GetTarget(collItemFixup.Id), collItemFixup.Index);
      }
      if (this.collFixups != null)
      {
        foreach (XmlSerializationReader.CollectionFixup collectionFixup in (IEnumerable) this.collFixups.Values)
          collectionFixup.Callback(collectionFixup.Collection, collectionFixup.CollectionItems);
      }
      if (this.fixups != null)
      {
        foreach (XmlSerializationReader.Fixup fixup in this.fixups)
          fixup.Callback((object) fixup);
      }
      if (this.targets == null)
        return;
      foreach (DictionaryEntry target in this.targets)
      {
        if (target.Value != null && (this.referencedObjects == null || !this.referencedObjects.Contains(target.Value)))
          this.UnreferencedObject((string) target.Key, target.Value);
      }
    }

    protected object ReadReferencingElement(out string fixupReference) => this.ReadReferencingElement(this.Reader.LocalName, this.Reader.NamespaceURI, false, out fixupReference);

    protected object ReadReferencingElement(string name, string ns, out string fixupReference) => this.ReadReferencingElement(name, ns, false, out fixupReference);

    protected object ReadReferencingElement(
      string name,
      string ns,
      bool elementCanBeType,
      out string fixupReference)
    {
      if (this.ReadNull())
      {
        fixupReference = (string) null;
        return (object) null;
      }
      string id = this.Reader.GetAttribute("href");
      if (id == string.Empty || id == null)
      {
        fixupReference = (string) null;
        XmlQualifiedName qname = this.GetXsiType();
        if (qname == (XmlQualifiedName) null)
          qname = new XmlQualifiedName(name, ns);
        string attribute = this.Reader.GetAttribute(this.arrayType, this.soapNS);
        if (qname == this.arrayQName || attribute != null)
        {
          this.delayedListFixups = this.EnsureHashtable(this.delayedListFixups);
          fixupReference = "__<" + (object) this.delayedFixupId++ + ">";
          object resultList;
          this.ReadList(out resultList);
          this.delayedListFixups[(object) fixupReference] = resultList;
          return (object) null;
        }
        XmlSerializationReader.WriteCallbackInfo callbackInfo = this.GetCallbackInfo(qname);
        return callbackInfo == null ? this.ReadTypedPrimitive(qname, true) : callbackInfo.Callback();
      }
      if (id.StartsWith("#"))
        id = id.Substring(1);
      ++this.readCount;
      this.Reader.Skip();
      if (this.TargetReady(id))
      {
        fixupReference = (string) null;
        return this.GetTarget(id);
      }
      fixupReference = id;
      return (object) null;
    }

    protected IXmlSerializable ReadSerializable(IXmlSerializable serializable)
    {
      if (this.ReadNull())
        return (IXmlSerializable) null;
      int depth = this.reader.Depth;
      ++this.readCount;
      serializable.ReadXml(this.reader);
      int content = (int) this.Reader.MoveToContent();
      while (this.reader.Depth > depth)
        this.reader.Skip();
      if (this.reader.Depth == depth && this.reader.NodeType == XmlNodeType.EndElement)
        this.reader.ReadEndElement();
      return serializable;
    }

    protected string ReadString(string value)
    {
      ++this.readCount;
      return value == null || value == string.Empty ? this.reader.ReadString() : value + this.reader.ReadString();
    }

    protected object ReadTypedPrimitive(XmlQualifiedName qname) => this.ReadTypedPrimitive(qname, false);

    private object ReadTypedPrimitive(XmlQualifiedName qname, bool reportUnknown)
    {
      if (qname == (XmlQualifiedName) null)
        qname = this.GetXsiType();
      TypeData primitiveTypeData = TypeTranslator.FindPrimitiveTypeData(qname.Name);
      if (primitiveTypeData == null || primitiveTypeData.SchemaType != SchemaTypes.Primitive)
      {
        ++this.readCount;
        XmlNode node = this.Document.ReadNode(this.reader);
        if (reportUnknown)
          this.OnUnknownNode(node, (object) null, (string) null);
        if (node.ChildNodes.Count == 0 && node.Attributes.Count == 0)
          return new object();
        if (!(node is XmlElement xmlElement))
          return (object) new XmlNode[1]{ node };
        XmlNode[] xmlNodeArray = new XmlNode[xmlElement.Attributes.Count + xmlElement.ChildNodes.Count];
        int num = 0;
        foreach (XmlNode attribute in (XmlNamedNodeMap) xmlElement.Attributes)
          xmlNodeArray[num++] = attribute;
        foreach (XmlNode childNode in xmlElement.ChildNodes)
          xmlNodeArray[num++] = childNode;
        return (object) xmlNodeArray;
      }
      if (primitiveTypeData.Type == typeof (XmlQualifiedName))
        return (object) this.ReadNullableQualifiedName();
      ++this.readCount;
      return XmlCustomFormatter.FromXmlString(primitiveTypeData, this.Reader.ReadElementString());
    }

    protected XmlNode ReadXmlNode(bool wrapped)
    {
      ++this.readCount;
      XmlNode xmlNode = this.Document.ReadNode(this.reader);
      return wrapped ? xmlNode.FirstChild : xmlNode;
    }

    protected XmlDocument ReadXmlDocument(bool wrapped)
    {
      ++this.readCount;
      if (wrapped)
        this.reader.ReadStartElement();
      int content = (int) this.reader.MoveToContent();
      XmlDocument xmlDocument = new XmlDocument();
      XmlNode newChild = xmlDocument.ReadNode(this.reader);
      xmlDocument.AppendChild(newChild);
      if (wrapped)
        this.reader.ReadEndElement();
      return xmlDocument;
    }

    protected void Referenced(object o)
    {
      if (o == null)
        return;
      if (this.referencedObjects == null)
        this.referencedObjects = new Hashtable();
      this.referencedObjects[o] = o;
    }

    protected Array ShrinkArray(Array a, int length, Type elementType, bool isNullable)
    {
      if (length == 0 && isNullable)
        return (Array) null;
      if (a == null)
        return Array.CreateInstance(elementType, length);
      if (a.Length == length)
        return a;
      Array instance = Array.CreateInstance(elementType, length);
      Array.Copy(a, instance, length);
      return instance;
    }

    protected byte[] ToByteArrayBase64(bool isNull)
    {
      ++this.readCount;
      if (!isNull)
        return XmlSerializationReader.ToByteArrayBase64(this.Reader.ReadString());
      this.Reader.ReadString();
      return (byte[]) null;
    }

    protected static byte[] ToByteArrayBase64(string value) => Convert.FromBase64String(value);

    protected byte[] ToByteArrayHex(bool isNull)
    {
      ++this.readCount;
      if (!isNull)
        return XmlSerializationReader.ToByteArrayHex(this.Reader.ReadString());
      this.Reader.ReadString();
      return (byte[]) null;
    }

    protected static byte[] ToByteArrayHex(string value) => XmlConvert.FromBinHexString(value);

    protected static char ToChar(string value) => XmlCustomFormatter.ToChar(value);

    protected static DateTime ToDate(string value) => XmlCustomFormatter.ToDate(value);

    protected static DateTime ToDateTime(string value) => XmlCustomFormatter.ToDateTime(value);

    protected static long ToEnum(string value, Hashtable h, string typeName) => XmlCustomFormatter.ToEnum(value, h, typeName, true);

    protected static DateTime ToTime(string value) => XmlCustomFormatter.ToTime(value);

    protected static string ToXmlName(string value) => XmlCustomFormatter.ToXmlName(value);

    protected static string ToXmlNCName(string value) => XmlCustomFormatter.ToXmlNCName(value);

    protected static string ToXmlNmToken(string value) => XmlCustomFormatter.ToXmlNmToken(value);

    protected static string ToXmlNmTokens(string value) => XmlCustomFormatter.ToXmlNmTokens(value);

    protected XmlQualifiedName ToXmlQualifiedName(string value)
    {
      int length = value.LastIndexOf(':');
      string name1 = XmlConvert.DecodeName(value);
      string name2;
      string ns;
      if (length < 0)
      {
        name2 = this.reader.NameTable.Add(name1);
        ns = this.reader.LookupNamespace(string.Empty);
      }
      else
      {
        string prefix = value.Substring(0, length);
        ns = this.reader.LookupNamespace(prefix);
        if (ns == null)
          throw new InvalidOperationException("namespace " + prefix + " not defined");
        name2 = this.reader.NameTable.Add(value.Substring(length + 1));
      }
      return new XmlQualifiedName(name2, ns);
    }

    protected void UnknownAttribute(object o, XmlAttribute attr) => this.UnknownAttribute(o, attr, (string) null);

    protected void UnknownAttribute(object o, XmlAttribute attr, string qnames)
    {
      int lineNum;
      int linePos;
      if (this.Reader is XmlTextReader)
      {
        lineNum = ((XmlTextReader) this.Reader).LineNumber;
        linePos = ((XmlTextReader) this.Reader).LinePosition;
      }
      else
      {
        lineNum = 0;
        linePos = 0;
      }
      XmlAttributeEventArgs e = new XmlAttributeEventArgs(attr, lineNum, linePos, o);
      e.ExpectedAttributes = qnames;
      if (this.eventSource == null)
        return;
      this.eventSource.OnUnknownAttribute(e);
    }

    protected void UnknownElement(object o, XmlElement elem) => this.UnknownElement(o, elem, (string) null);

    protected void UnknownElement(object o, XmlElement elem, string qnames)
    {
      int lineNum;
      int linePos;
      if (this.Reader is XmlTextReader)
      {
        lineNum = ((XmlTextReader) this.Reader).LineNumber;
        linePos = ((XmlTextReader) this.Reader).LinePosition;
      }
      else
      {
        lineNum = 0;
        linePos = 0;
      }
      XmlElementEventArgs e = new XmlElementEventArgs(elem, lineNum, linePos, o);
      e.ExpectedElements = qnames;
      if (this.eventSource == null)
        return;
      this.eventSource.OnUnknownElement(e);
    }

    protected void UnknownNode(object o) => this.UnknownNode(o, (string) null);

    protected void UnknownNode(object o, string qnames) => this.OnUnknownNode(this.ReadXmlNode(false), o, qnames);

    private void OnUnknownNode(XmlNode node, object o, string qnames)
    {
      int linenumber;
      int lineposition;
      if (this.Reader is XmlTextReader)
      {
        linenumber = ((XmlTextReader) this.Reader).LineNumber;
        lineposition = ((XmlTextReader) this.Reader).LinePosition;
      }
      else
      {
        linenumber = 0;
        lineposition = 0;
      }
      if (node is XmlAttribute)
        this.UnknownAttribute(o, (XmlAttribute) node, qnames);
      else if (node is XmlElement)
      {
        this.UnknownElement(o, (XmlElement) node, qnames);
      }
      else
      {
        if (this.eventSource != null)
          this.eventSource.OnUnknownNode(new XmlNodeEventArgs(linenumber, lineposition, node.LocalName, node.Name, node.NamespaceURI, node.NodeType, o, node.Value));
        if (this.Reader.ReadState == ReadState.EndOfFile)
          throw new InvalidOperationException("End of document found");
      }
    }

    protected void UnreferencedObject(string id, object o)
    {
      if (this.eventSource == null)
        return;
      this.eventSource.OnUnreferencedObject(new UnreferencedObjectEventArgs(o, id));
    }

    [MonoTODO]
    protected bool DecodeName
    {
      get => throw new NotImplementedException();
      set => throw new NotImplementedException();
    }

    [MonoTODO]
    protected string CollapseWhitespace(string value) => throw new NotImplementedException();

    [MonoTODO]
    protected Exception CreateBadDerivationException(
      string xsdDerived,
      string nsDerived,
      string xsdBase,
      string nsBase,
      string clrDerived,
      string clrBase)
    {
      throw new NotImplementedException();
    }

    [MonoTODO]
    protected Exception CreateInvalidCastException(Type type, object value, string id) => throw new NotImplementedException();

    [MonoTODO]
    protected Exception CreateMissingIXmlSerializableType(string name, string ns, string clrType) => throw new NotImplementedException();

    [MonoTODO]
    protected string ReadString(string value, bool trim) => throw new NotImplementedException();

    [MonoTODO]
    protected object ReadTypedNull(XmlQualifiedName type) => throw new NotImplementedException();

    [MonoTODO]
    protected static Assembly ResolveDynamicAssembly(string assemblyFullName) => throw new NotImplementedException();

    private class WriteCallbackInfo
    {
      public Type Type;
      public string TypeName;
      public string TypeNs;
      public XmlSerializationReadCallback Callback;
    }

    protected class CollectionFixup
    {
      private XmlSerializationCollectionFixupCallback callback;
      private object collection;
      private object collectionItems;
      private string id;

      public CollectionFixup(
        object collection,
        XmlSerializationCollectionFixupCallback callback,
        string id)
      {
        this.callback = callback;
        this.collection = collection;
        this.id = id;
      }

      public XmlSerializationCollectionFixupCallback Callback => this.callback;

      public object Collection => this.collection;

      public object Id => (object) this.id;

      internal object CollectionItems
      {
        get => this.collectionItems;
        set => this.collectionItems = value;
      }
    }

    protected class Fixup
    {
      private object source;
      private string[] ids;
      private XmlSerializationFixupCallback callback;

      public Fixup(object o, XmlSerializationFixupCallback callback, int count)
      {
        this.source = o;
        this.callback = callback;
        this.ids = new string[count];
      }

      public Fixup(object o, XmlSerializationFixupCallback callback, string[] ids)
      {
        this.source = o;
        this.ids = ids;
        this.callback = callback;
      }

      public XmlSerializationFixupCallback Callback => this.callback;

      public string[] Ids => this.ids;

      public object Source
      {
        get => this.source;
        set => this.source = value;
      }
    }

    protected class CollectionItemFixup
    {
      private Array list;
      private int index;
      private string id;

      public CollectionItemFixup(Array list, int index, string id)
      {
        this.list = list;
        this.index = index;
        this.id = id;
      }

      public Array Collection => this.list;

      public int Index => this.index;

      public string Id => this.id;
    }
  }
}
