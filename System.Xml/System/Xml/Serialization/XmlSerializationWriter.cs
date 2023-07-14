// Decompiled with JetBrains decompiler
// Type: System.Xml.Serialization.XmlSerializationWriter
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Runtime.Serialization;

namespace System.Xml.Serialization
{
  public abstract class XmlSerializationWriter : XmlSerializationGeneratedCode
  {
    private const string xmlNamespace = "http://www.w3.org/2000/xmlns/";
    private const string unexpectedTypeError = "The type {0} was not expected. Use the XmlInclude or SoapInclude attribute to specify types that are not known statically.";
    private ObjectIDGenerator idGenerator;
    private int qnameCount;
    private bool topLevelElement;
    private ArrayList namespaces;
    private XmlWriter writer;
    private Queue referencedElements;
    private Hashtable callbacks;
    private Hashtable serializedObjects;

    protected XmlSerializationWriter()
    {
      this.qnameCount = 0;
      this.serializedObjects = new Hashtable();
    }

    internal void Initialize(XmlWriter writer, XmlSerializerNamespaces nss)
    {
      this.writer = writer;
      if (nss == null)
        return;
      this.namespaces = new ArrayList();
      foreach (XmlQualifiedName xmlQualifiedName in nss.ToArray())
      {
        if (xmlQualifiedName.Name != string.Empty && xmlQualifiedName.Namespace != string.Empty)
          this.namespaces.Add((object) xmlQualifiedName);
      }
    }

    protected ArrayList Namespaces
    {
      get => this.namespaces;
      set => this.namespaces = value;
    }

    protected XmlWriter Writer
    {
      get => this.writer;
      set => this.writer = value;
    }

    protected void AddWriteCallback(
      Type type,
      string typeName,
      string typeNs,
      XmlSerializationWriteCallback callback)
    {
      XmlSerializationWriter.WriteCallbackInfo writeCallbackInfo = new XmlSerializationWriter.WriteCallbackInfo();
      writeCallbackInfo.Type = type;
      writeCallbackInfo.TypeName = typeName;
      writeCallbackInfo.TypeNs = typeNs;
      writeCallbackInfo.Callback = callback;
      if (this.callbacks == null)
        this.callbacks = new Hashtable();
      this.callbacks.Add((object) type, (object) writeCallbackInfo);
    }

    protected Exception CreateChoiceIdentifierValueException(
      string value,
      string identifier,
      string name,
      string ns)
    {
      return (Exception) new InvalidOperationException(string.Format("Value '{0}' of the choice identifier '{1}' does not match element '{2}' from namespace '{3}'.", (object) value, (object) identifier, (object) name, (object) ns));
    }

    protected Exception CreateInvalidChoiceIdentifierValueException(string type, string identifier) => (Exception) new InvalidOperationException(string.Format("Invalid or missing choice identifier '{0}' of type '{1}'.", (object) identifier, (object) type));

    protected Exception CreateMismatchChoiceException(
      string value,
      string elementName,
      string enumValue)
    {
      return (Exception) new InvalidOperationException(string.Format("Value of {0} mismatches the type of {1}, you need to set it to {2}.", (object) elementName, (object) value, (object) enumValue));
    }

    protected Exception CreateUnknownAnyElementException(string name, string ns) => (Exception) new InvalidOperationException(string.Format("The XML element named '{0}' from namespace '{1}' was not expected. The XML element name and namespace must match those provided via XmlAnyElementAttribute(s).", (object) name, (object) ns));

    protected Exception CreateUnknownTypeException(object o) => this.CreateUnknownTypeException(o.GetType());

    protected Exception CreateUnknownTypeException(Type type) => (Exception) new InvalidOperationException(string.Format("The type {0} may not be used in this context.", (object) type));

    protected static byte[] FromByteArrayBase64(byte[] value) => value;

    protected static string FromByteArrayHex(byte[] value) => XmlCustomFormatter.FromByteArrayHex(value);

    protected static string FromChar(char value) => XmlCustomFormatter.FromChar(value);

    protected static string FromDate(DateTime value) => XmlCustomFormatter.FromDate(value);

    protected static string FromDateTime(DateTime value) => XmlCustomFormatter.FromDateTime(value);

    protected static string FromEnum(long value, string[] values, long[] ids) => XmlCustomFormatter.FromEnum(value, values, ids);

    protected static string FromTime(DateTime value) => XmlCustomFormatter.FromTime(value);

    protected static string FromXmlName(string name) => XmlCustomFormatter.FromXmlName(name);

    protected static string FromXmlNCName(string ncName) => XmlCustomFormatter.FromXmlNCName(ncName);

    protected static string FromXmlNmToken(string nmToken) => XmlCustomFormatter.FromXmlNmToken(nmToken);

    protected static string FromXmlNmTokens(string nmTokens) => XmlCustomFormatter.FromXmlNmTokens(nmTokens);

    protected string FromXmlQualifiedName(XmlQualifiedName xmlQualifiedName) => xmlQualifiedName == (XmlQualifiedName) null || xmlQualifiedName == XmlQualifiedName.Empty ? (string) null : this.GetQualifiedName(xmlQualifiedName.Name, xmlQualifiedName.Namespace);

    private string GetId(object o, bool addToReferencesList)
    {
      if (this.idGenerator == null)
        this.idGenerator = new ObjectIDGenerator();
      return string.Format((IFormatProvider) CultureInfo.InvariantCulture, "id{0}", (object) this.idGenerator.GetId(o, out bool _));
    }

    private bool AlreadyQueued(object ob)
    {
      if (this.idGenerator == null)
        return false;
      bool firstTime;
      this.idGenerator.HasId(ob, out firstTime);
      return !firstTime;
    }

    private string GetNamespacePrefix(string ns)
    {
      string localName = this.Writer.LookupPrefix(ns);
      if (localName == null)
      {
        localName = string.Format((IFormatProvider) CultureInfo.InvariantCulture, "q{0}", (object) ++this.qnameCount);
        this.WriteAttribute("xmlns", localName, (string) null, ns);
      }
      return localName;
    }

    private string GetQualifiedName(string name, string ns)
    {
      if (ns == string.Empty)
        return name;
      string namespacePrefix = this.GetNamespacePrefix(ns);
      return namespacePrefix == string.Empty ? name : string.Format("{0}:{1}", (object) namespacePrefix, (object) name);
    }

    protected abstract void InitCallbacks();

    protected void TopLevelElement() => this.topLevelElement = true;

    protected void WriteAttribute(string localName, byte[] value) => this.WriteAttribute(localName, string.Empty, value);

    protected void WriteAttribute(string localName, string value) => this.WriteAttribute(string.Empty, localName, string.Empty, value);

    protected void WriteAttribute(string localName, string ns, byte[] value)
    {
      if (value == null)
        return;
      this.Writer.WriteStartAttribute(localName, ns);
      this.WriteValue(value);
      this.Writer.WriteEndAttribute();
    }

    protected void WriteAttribute(string localName, string ns, string value) => this.WriteAttribute((string) null, localName, ns, value);

    protected void WriteAttribute(string prefix, string localName, string ns, string value)
    {
      if (value == null)
        return;
      this.Writer.WriteAttributeString(prefix, localName, ns, value);
    }

    private void WriteXmlNode(XmlNode node)
    {
      if (node is XmlDocument)
        node = (XmlNode) ((XmlDocument) node).DocumentElement;
      node.WriteTo(this.Writer);
    }

    protected void WriteElementEncoded(
      XmlNode node,
      string name,
      string ns,
      bool isNullable,
      bool any)
    {
      if (name != string.Empty)
      {
        if (node == null)
        {
          if (!isNullable)
            return;
          this.WriteNullTagEncoded(name, ns);
        }
        else
        {
          this.Writer.WriteStartElement(name, ns);
          this.WriteXmlNode(node);
          this.Writer.WriteEndElement();
        }
      }
      else
        this.WriteXmlNode(node);
    }

    protected void WriteElementLiteral(
      XmlNode node,
      string name,
      string ns,
      bool isNullable,
      bool any)
    {
      if (name != string.Empty)
      {
        if (node == null)
        {
          if (!isNullable)
            return;
          this.WriteNullTagLiteral(name, ns);
        }
        else
        {
          this.Writer.WriteStartElement(name, ns);
          this.WriteXmlNode(node);
          this.Writer.WriteEndElement();
        }
      }
      else
        this.WriteXmlNode(node);
    }

    protected void WriteElementQualifiedName(string localName, XmlQualifiedName value) => this.WriteElementQualifiedName(localName, string.Empty, value, (XmlQualifiedName) null);

    protected void WriteElementQualifiedName(string localName, string ns, XmlQualifiedName value) => this.WriteElementQualifiedName(localName, ns, value, (XmlQualifiedName) null);

    protected void WriteElementQualifiedName(
      string localName,
      XmlQualifiedName value,
      XmlQualifiedName xsiType)
    {
      this.WriteElementQualifiedName(localName, string.Empty, value, xsiType);
    }

    protected void WriteElementQualifiedName(
      string localName,
      string ns,
      XmlQualifiedName value,
      XmlQualifiedName xsiType)
    {
      localName = XmlCustomFormatter.FromXmlNCName(localName);
      this.WriteStartElement(localName, ns);
      if (xsiType != (XmlQualifiedName) null)
        this.WriteXsiType(xsiType.Name, xsiType.Namespace);
      this.Writer.WriteString(this.FromXmlQualifiedName(value));
      this.WriteEndElement();
    }

    protected void WriteElementString(string localName, string value) => this.WriteElementString(localName, string.Empty, value, (XmlQualifiedName) null);

    protected void WriteElementString(string localName, string ns, string value) => this.WriteElementString(localName, ns, value, (XmlQualifiedName) null);

    protected void WriteElementString(string localName, string value, XmlQualifiedName xsiType) => this.WriteElementString(localName, string.Empty, value, xsiType);

    protected void WriteElementString(
      string localName,
      string ns,
      string value,
      XmlQualifiedName xsiType)
    {
      if (value == null)
        return;
      if (xsiType != (XmlQualifiedName) null)
      {
        localName = XmlCustomFormatter.FromXmlNCName(localName);
        this.WriteStartElement(localName, ns);
        this.WriteXsiType(xsiType.Name, xsiType.Namespace);
        this.Writer.WriteString(value);
        this.WriteEndElement();
      }
      else
        this.Writer.WriteElementString(localName, ns, value);
    }

    protected void WriteElementStringRaw(string localName, byte[] value) => this.WriteElementStringRaw(localName, string.Empty, value, (XmlQualifiedName) null);

    protected void WriteElementStringRaw(string localName, string value) => this.WriteElementStringRaw(localName, string.Empty, value, (XmlQualifiedName) null);

    protected void WriteElementStringRaw(string localName, byte[] value, XmlQualifiedName xsiType) => this.WriteElementStringRaw(localName, string.Empty, value, xsiType);

    protected void WriteElementStringRaw(string localName, string ns, byte[] value) => this.WriteElementStringRaw(localName, ns, value, (XmlQualifiedName) null);

    protected void WriteElementStringRaw(string localName, string ns, string value) => this.WriteElementStringRaw(localName, ns, value, (XmlQualifiedName) null);

    protected void WriteElementStringRaw(string localName, string value, XmlQualifiedName xsiType) => this.WriteElementStringRaw(localName, string.Empty, value, (XmlQualifiedName) null);

    protected void WriteElementStringRaw(
      string localName,
      string ns,
      byte[] value,
      XmlQualifiedName xsiType)
    {
      if (value == null)
        return;
      this.WriteStartElement(localName, ns);
      if (xsiType != (XmlQualifiedName) null)
        this.WriteXsiType(xsiType.Name, xsiType.Namespace);
      if (value.Length > 0)
        this.Writer.WriteBase64(value, 0, value.Length);
      this.WriteEndElement();
    }

    protected void WriteElementStringRaw(
      string localName,
      string ns,
      string value,
      XmlQualifiedName xsiType)
    {
      localName = XmlCustomFormatter.FromXmlNCName(localName);
      this.WriteStartElement(localName, ns);
      if (xsiType != (XmlQualifiedName) null)
        this.WriteXsiType(xsiType.Name, xsiType.Namespace);
      this.Writer.WriteRaw(value);
      this.WriteEndElement();
    }

    protected void WriteEmptyTag(string name) => this.WriteEmptyTag(name, string.Empty);

    protected void WriteEmptyTag(string name, string ns)
    {
      name = XmlCustomFormatter.FromXmlName(name);
      this.WriteStartElement(name, ns);
      this.WriteEndElement();
    }

    protected void WriteEndElement() => this.WriteEndElement((object) null);

    protected void WriteEndElement(object o)
    {
      if (o != null)
        this.serializedObjects.Remove(o);
      this.Writer.WriteEndElement();
    }

    protected void WriteId(object o) => this.WriteAttribute("id", this.GetId(o, true));

    protected void WriteNamespaceDeclarations(XmlSerializerNamespaces ns)
    {
      if (ns == null)
        return;
      foreach (XmlQualifiedName xmlQualifiedName in (IEnumerable) ns.Namespaces.Values)
      {
        if (xmlQualifiedName.Namespace != string.Empty && this.Writer.LookupPrefix(xmlQualifiedName.Namespace) != xmlQualifiedName.Name)
          this.WriteAttribute("xmlns", xmlQualifiedName.Name, "http://www.w3.org/2000/xmlns/", xmlQualifiedName.Namespace);
      }
    }

    protected void WriteNullableQualifiedNameEncoded(
      string name,
      string ns,
      XmlQualifiedName value,
      XmlQualifiedName xsiType)
    {
      if (value != (XmlQualifiedName) null)
        this.WriteElementQualifiedName(name, ns, value, xsiType);
      else
        this.WriteNullTagEncoded(name, ns);
    }

    protected void WriteNullableQualifiedNameLiteral(
      string name,
      string ns,
      XmlQualifiedName value)
    {
      if (value != (XmlQualifiedName) null)
        this.WriteElementQualifiedName(name, ns, value);
      else
        this.WriteNullTagLiteral(name, ns);
    }

    protected void WriteNullableStringEncoded(
      string name,
      string ns,
      string value,
      XmlQualifiedName xsiType)
    {
      if (value != null)
        this.WriteElementString(name, ns, value, xsiType);
      else
        this.WriteNullTagEncoded(name, ns);
    }

    protected void WriteNullableStringEncodedRaw(
      string name,
      string ns,
      byte[] value,
      XmlQualifiedName xsiType)
    {
      if (value == null)
        this.WriteNullTagEncoded(name, ns);
      else
        this.WriteElementStringRaw(name, ns, value, xsiType);
    }

    protected void WriteNullableStringEncodedRaw(
      string name,
      string ns,
      string value,
      XmlQualifiedName xsiType)
    {
      if (value == null)
        this.WriteNullTagEncoded(name, ns);
      else
        this.WriteElementStringRaw(name, ns, value, xsiType);
    }

    protected void WriteNullableStringLiteral(string name, string ns, string value)
    {
      if (value != null)
        this.WriteElementString(name, ns, value, (XmlQualifiedName) null);
      else
        this.WriteNullTagLiteral(name, ns);
    }

    protected void WriteNullableStringLiteralRaw(string name, string ns, byte[] value)
    {
      if (value == null)
        this.WriteNullTagLiteral(name, ns);
      else
        this.WriteElementStringRaw(name, ns, value);
    }

    protected void WriteNullableStringLiteralRaw(string name, string ns, string value)
    {
      if (value == null)
        this.WriteNullTagLiteral(name, ns);
      else
        this.WriteElementStringRaw(name, ns, value);
    }

    protected void WriteNullTagEncoded(string name) => this.WriteNullTagEncoded(name, string.Empty);

    protected void WriteNullTagEncoded(string name, string ns)
    {
      this.Writer.WriteStartElement(name, ns);
      this.Writer.WriteAttributeString("nil", "http://www.w3.org/2001/XMLSchema-instance", "true");
      this.Writer.WriteEndElement();
    }

    protected void WriteNullTagLiteral(string name) => this.WriteNullTagLiteral(name, string.Empty);

    protected void WriteNullTagLiteral(string name, string ns)
    {
      this.WriteStartElement(name, ns);
      this.Writer.WriteAttributeString("nil", "http://www.w3.org/2001/XMLSchema-instance", "true");
      this.WriteEndElement();
    }

    protected void WritePotentiallyReferencingElement(string n, string ns, object o) => this.WritePotentiallyReferencingElement(n, ns, o, (Type) null, false, false);

    protected void WritePotentiallyReferencingElement(
      string n,
      string ns,
      object o,
      Type ambientType)
    {
      this.WritePotentiallyReferencingElement(n, ns, o, ambientType, false, false);
    }

    protected void WritePotentiallyReferencingElement(
      string n,
      string ns,
      object o,
      Type ambientType,
      bool suppressReference)
    {
      this.WritePotentiallyReferencingElement(n, ns, o, ambientType, suppressReference, false);
    }

    protected void WritePotentiallyReferencingElement(
      string n,
      string ns,
      object o,
      Type ambientType,
      bool suppressReference,
      bool isNullable)
    {
      if (o == null)
      {
        if (!isNullable)
          return;
        this.WriteNullTagEncoded(n, ns);
      }
      else
      {
        this.WriteStartElement(n, ns, true);
        this.CheckReferenceQueue();
        if (this.callbacks != null && this.callbacks.ContainsKey((object) o.GetType()))
        {
          XmlSerializationWriter.WriteCallbackInfo callback = (XmlSerializationWriter.WriteCallbackInfo) this.callbacks[(object) o.GetType()];
          if (o.GetType().IsEnum)
            callback.Callback(o);
          else if (suppressReference)
          {
            this.Writer.WriteAttributeString("id", this.GetId(o, false));
            if (ambientType != o.GetType())
              this.WriteXsiType(callback.TypeName, callback.TypeNs);
            callback.Callback(o);
          }
          else
          {
            if (!this.AlreadyQueued(o))
              this.referencedElements.Enqueue(o);
            this.Writer.WriteAttributeString("href", "#" + this.GetId(o, true));
          }
        }
        else
        {
          TypeData typeData = TypeTranslator.GetTypeData(o.GetType());
          if (typeData.SchemaType == SchemaTypes.Primitive)
          {
            this.WriteXsiType(typeData.XmlType, "http://www.w3.org/2001/XMLSchema");
            this.Writer.WriteString(XmlCustomFormatter.ToXmlString(typeData, o));
          }
          else
          {
            if (!this.IsPrimitiveArray(typeData))
              throw new InvalidOperationException("Invalid type: " + o.GetType().FullName);
            if (!this.AlreadyQueued(o))
              this.referencedElements.Enqueue(o);
            this.Writer.WriteAttributeString("href", "#" + this.GetId(o, true));
          }
        }
        this.WriteEndElement();
      }
    }

    protected void WriteReferencedElements()
    {
      if (this.referencedElements == null || this.callbacks == null)
        return;
      while (this.referencedElements.Count > 0)
      {
        object o = this.referencedElements.Dequeue();
        TypeData typeData = TypeTranslator.GetTypeData(o.GetType());
        XmlSerializationWriter.WriteCallbackInfo callback = (XmlSerializationWriter.WriteCallbackInfo) this.callbacks[(object) o.GetType()];
        if (callback != null)
        {
          this.WriteStartElement(callback.TypeName, callback.TypeNs, true);
          this.Writer.WriteAttributeString("id", this.GetId(o, false));
          if (typeData.SchemaType != SchemaTypes.Array)
            this.WriteXsiType(callback.TypeName, callback.TypeNs);
          callback.Callback(o);
          this.WriteEndElement();
        }
        else if (this.IsPrimitiveArray(typeData))
          this.WriteArray(o, typeData);
      }
    }

    private bool IsPrimitiveArray(TypeData td)
    {
      if (td.SchemaType != SchemaTypes.Array)
        return false;
      return td.ListItemTypeData.SchemaType == SchemaTypes.Primitive || td.ListItemType == typeof (object) || this.IsPrimitiveArray(td.ListItemTypeData);
    }

    private void WriteArray(object o, TypeData td)
    {
      TypeData typeData = td;
      int num = -1;
      string xmlType;
      do
      {
        typeData = typeData.ListItemTypeData;
        xmlType = typeData.XmlType;
        ++num;
      }
      while (typeData.SchemaType == SchemaTypes.Array);
      while (num-- > 0)
        xmlType += "[]";
      this.WriteStartElement("Array", "http://schemas.xmlsoap.org/soap/encoding/", true);
      this.Writer.WriteAttributeString("id", this.GetId(o, false));
      if (td.SchemaType == SchemaTypes.Array)
      {
        Array array = (Array) o;
        int length = array.Length;
        this.Writer.WriteAttributeString("arrayType", "http://schemas.xmlsoap.org/soap/encoding/", this.GetQualifiedName(xmlType, "http://www.w3.org/2001/XMLSchema") + "[" + length.ToString() + "]");
        for (int index = 0; index < length; ++index)
          this.WritePotentiallyReferencingElement("Item", string.Empty, array.GetValue(index), td.ListItemType, false, true);
      }
      this.WriteEndElement();
    }

    protected void WriteReferencingElement(string n, string ns, object o) => this.WriteReferencingElement(n, ns, o, false);

    protected void WriteReferencingElement(string n, string ns, object o, bool isNullable)
    {
      if (o == null)
      {
        if (!isNullable)
          return;
        this.WriteNullTagEncoded(n, ns);
      }
      else
      {
        this.CheckReferenceQueue();
        if (!this.AlreadyQueued(o))
          this.referencedElements.Enqueue(o);
        this.Writer.WriteStartElement(n, ns);
        this.Writer.WriteAttributeString("href", "#" + this.GetId(o, true));
        this.Writer.WriteEndElement();
      }
    }

    private void CheckReferenceQueue()
    {
      if (this.referencedElements != null)
        return;
      this.referencedElements = new Queue();
      this.InitCallbacks();
    }

    [MonoTODO]
    protected void WriteRpcResult(string name, string ns) => throw new NotImplementedException();

    protected void WriteSerializable(
      IXmlSerializable serializable,
      string name,
      string ns,
      bool isNullable)
    {
      this.WriteSerializable(serializable, name, ns, isNullable, true);
    }

    protected void WriteSerializable(
      IXmlSerializable serializable,
      string name,
      string ns,
      bool isNullable,
      bool wrapped)
    {
      if (serializable == null)
      {
        if (!isNullable || !wrapped)
          return;
        this.WriteNullTagLiteral(name, ns);
      }
      else
      {
        if (wrapped)
          this.Writer.WriteStartElement(name, ns);
        serializable.WriteXml(this.Writer);
        if (!wrapped)
          return;
        this.Writer.WriteEndElement();
      }
    }

    protected void WriteStartDocument()
    {
      if (this.Writer.WriteState != WriteState.Start)
        return;
      this.Writer.WriteStartDocument();
    }

    protected void WriteStartElement(string name) => this.WriteStartElement(name, string.Empty, (object) null, false);

    protected void WriteStartElement(string name, string ns) => this.WriteStartElement(name, ns, (object) null, false);

    protected void WriteStartElement(string name, string ns, bool writePrefixed) => this.WriteStartElement(name, ns, (object) null, writePrefixed);

    protected void WriteStartElement(string name, string ns, object o) => this.WriteStartElement(name, ns, o, false);

    protected void WriteStartElement(string name, string ns, object o, bool writePrefixed) => this.WriteStartElement(name, ns, o, writePrefixed, (ICollection) this.namespaces);

    protected void WriteStartElement(
      string name,
      string ns,
      object o,
      bool writePrefixed,
      XmlSerializerNamespaces xmlns)
    {
      if (xmlns == null)
        throw new ArgumentNullException(nameof (xmlns));
      this.WriteStartElement(name, ns, o, writePrefixed, (ICollection) xmlns.ToArray());
    }

    private void WriteStartElement(
      string name,
      string ns,
      object o,
      bool writePrefixed,
      ICollection namespaces)
    {
      if (o != null)
        this.serializedObjects[o] = !this.serializedObjects.Contains(o) ? o : throw new InvalidOperationException("A circular reference was detected while serializing an object of type " + o.GetType().Name);
      string prefix = (string) null;
      if (this.topLevelElement && ns != null && ns.Length != 0)
      {
        foreach (XmlQualifiedName xmlQualifiedName in (IEnumerable) namespaces)
        {
          if (xmlQualifiedName.Namespace == ns)
          {
            prefix = xmlQualifiedName.Name;
            writePrefixed = true;
            break;
          }
        }
      }
      if (writePrefixed && ns != string.Empty)
      {
        name = XmlCustomFormatter.FromXmlName(name);
        if (prefix == null)
          prefix = this.Writer.LookupPrefix(ns);
        if (prefix == null || prefix.Length == 0)
          prefix = "q" + (object) ++this.qnameCount;
        this.Writer.WriteStartElement(prefix, name, ns);
      }
      else
        this.Writer.WriteStartElement(name, ns);
      if (!this.topLevelElement)
        return;
      if (namespaces != null)
      {
        foreach (XmlQualifiedName xmlQualifiedName in (IEnumerable) namespaces)
        {
          string str = this.Writer.LookupPrefix(xmlQualifiedName.Namespace);
          if (str == null || str.Length == 0)
            this.WriteAttribute("xmlns", xmlQualifiedName.Name, "http://www.w3.org/2000/xmlns/", xmlQualifiedName.Namespace);
        }
      }
      this.topLevelElement = false;
    }

    protected void WriteTypedPrimitive(string name, string ns, object o, bool xsiType)
    {
      TypeData typeData = TypeTranslator.GetTypeData(o.GetType());
      if (typeData.SchemaType != SchemaTypes.Primitive)
        throw new InvalidOperationException(string.Format("The type of the argument object '{0}' is not primitive.", (object) typeData.FullTypeName));
      if (name == null)
      {
        ns = !typeData.IsXsdType ? "http://microsoft.com/wsdl/types/" : "http://www.w3.org/2001/XMLSchema";
        name = typeData.XmlType;
      }
      else
        name = XmlCustomFormatter.FromXmlName(name);
      this.Writer.WriteStartElement(name, ns);
      string str = (object) (o as XmlQualifiedName) == null ? XmlCustomFormatter.ToXmlString(typeData, o) : this.FromXmlQualifiedName((XmlQualifiedName) o);
      if (xsiType)
      {
        if (typeData.SchemaType != SchemaTypes.Primitive)
          throw new InvalidOperationException(string.Format("The type {0} was not expected. Use the XmlInclude or SoapInclude attribute to specify types that are not known statically.", (object) o.GetType().FullName));
        this.WriteXsiType(typeData.XmlType, !typeData.IsXsdType ? "http://microsoft.com/wsdl/types/" : "http://www.w3.org/2001/XMLSchema");
      }
      this.WriteValue(str);
      this.Writer.WriteEndElement();
    }

    protected void WriteValue(byte[] value) => this.Writer.WriteBase64(value, 0, value.Length);

    protected void WriteValue(string value)
    {
      if (value == null)
        return;
      this.Writer.WriteString(value);
    }

    protected void WriteXmlAttribute(XmlNode node) => this.WriteXmlAttribute(node, (object) null);

    protected void WriteXmlAttribute(XmlNode node, object container)
    {
      if (!(node is XmlAttribute xmlAttribute))
        throw new InvalidOperationException("The node must be either type XmlAttribute or a derived type.");
      if (xmlAttribute.NamespaceURI == "http://schemas.xmlsoap.org/wsdl/" && xmlAttribute.LocalName == "arrayType")
      {
        string type;
        string ns;
        string dimensions;
        TypeTranslator.ParseArrayType(xmlAttribute.Value, out type, out ns, out dimensions);
        string qualifiedName = this.GetQualifiedName(type + dimensions, ns);
        this.WriteAttribute(xmlAttribute.Prefix, xmlAttribute.LocalName, xmlAttribute.NamespaceURI, qualifiedName);
      }
      else
        this.WriteAttribute(xmlAttribute.Prefix, xmlAttribute.LocalName, xmlAttribute.NamespaceURI, xmlAttribute.Value);
    }

    protected void WriteXsiType(string name, string ns)
    {
      if (ns != null && ns != string.Empty)
        this.WriteAttribute("type", "http://www.w3.org/2001/XMLSchema-instance", this.GetQualifiedName(name, ns));
      else
        this.WriteAttribute("type", "http://www.w3.org/2001/XMLSchema-instance", name);
    }

    protected Exception CreateInvalidAnyTypeException(object o) => o == null ? (Exception) new InvalidOperationException("null is invalid as anyType in XmlSerializer") : this.CreateInvalidAnyTypeException(o.GetType());

    protected Exception CreateInvalidAnyTypeException(Type t) => (Exception) new InvalidOperationException(string.Format("An object of type '{0}' is invalid as anyType in XmlSerializer", (object) t));

    protected Exception CreateInvalidEnumValueException(object value, string typeName) => (Exception) new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, "'{0}' is not a valid value for {1}.", value, (object) typeName));

    protected static string FromEnum(long value, string[] values, long[] ids, string typeName) => XmlCustomFormatter.FromEnum(value, values, ids, typeName);

    [MonoTODO]
    protected string FromXmlQualifiedName(XmlQualifiedName xmlQualifiedName, bool ignoreEmpty) => throw new NotImplementedException();

    [MonoTODO]
    protected static Assembly ResolveDynamicAssembly(string assemblyFullName) => throw new NotImplementedException();

    [MonoTODO]
    protected bool EscapeName
    {
      get => throw new NotImplementedException();
      set => throw new NotImplementedException();
    }

    private class WriteCallbackInfo
    {
      public Type Type;
      public string TypeName;
      public string TypeNs;
      public XmlSerializationWriteCallback Callback;
    }
  }
}
