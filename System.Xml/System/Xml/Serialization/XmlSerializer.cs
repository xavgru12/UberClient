// Decompiled with JetBrains decompiler
// Type: System.Xml.Serialization.XmlSerializer
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections;
using System.IO;
using System.Reflection;
using System.Security.Policy;
using System.Text;

namespace System.Xml.Serialization
{
  public class XmlSerializer
  {
    internal const string WsdlNamespace = "http://schemas.xmlsoap.org/wsdl/";
    internal const string EncodingNamespace = "http://schemas.xmlsoap.org/soap/encoding/";
    internal const string WsdlTypesNamespace = "http://microsoft.com/wsdl/types/";
    private static int generationThreshold;
    private static bool backgroundGeneration = true;
    private static bool deleteTempFiles = true;
    private static bool generatorFallback = true;
    private bool customSerializer;
    private XmlMapping typeMapping;
    private XmlSerializer.SerializerData serializerData;
    private static Hashtable serializerTypes = new Hashtable();
    private XmlAttributeEventHandler onUnknownAttribute;
    private XmlElementEventHandler onUnknownElement;
    private XmlNodeEventHandler onUnknownNode;
    private UnreferencedObjectEventHandler onUnreferencedObject;

    protected XmlSerializer() => this.customSerializer = true;

    public XmlSerializer(Type type)
      : this(type, (XmlAttributeOverrides) null, (Type[]) null, (XmlRootAttribute) null, (string) null)
    {
    }

    public XmlSerializer(XmlTypeMapping xmlTypeMapping) => this.typeMapping = (XmlMapping) xmlTypeMapping;

    internal XmlSerializer(XmlMapping mapping, XmlSerializer.SerializerData data)
    {
      this.typeMapping = mapping;
      this.serializerData = data;
    }

    public XmlSerializer(Type type, string defaultNamespace)
      : this(type, (XmlAttributeOverrides) null, (Type[]) null, (XmlRootAttribute) null, defaultNamespace)
    {
    }

    public XmlSerializer(Type type, Type[] extraTypes)
      : this(type, (XmlAttributeOverrides) null, extraTypes, (XmlRootAttribute) null, (string) null)
    {
    }

    public XmlSerializer(Type type, XmlAttributeOverrides overrides)
      : this(type, overrides, (Type[]) null, (XmlRootAttribute) null, (string) null)
    {
    }

    public XmlSerializer(Type type, XmlRootAttribute root)
      : this(type, (XmlAttributeOverrides) null, (Type[]) null, root, (string) null)
    {
    }

    public XmlSerializer(
      Type type,
      XmlAttributeOverrides overrides,
      Type[] extraTypes,
      XmlRootAttribute root,
      string defaultNamespace)
    {
      if (type == null)
        throw new ArgumentNullException(nameof (type));
      XmlReflectionImporter reflectionImporter = new XmlReflectionImporter(overrides, defaultNamespace);
      if (extraTypes != null)
      {
        foreach (Type extraType in extraTypes)
          reflectionImporter.IncludeType(extraType);
      }
      this.typeMapping = (XmlMapping) reflectionImporter.ImportTypeMapping(type, root, defaultNamespace);
    }

    [MonoTODO]
    public XmlSerializer(
      Type type,
      XmlAttributeOverrides overrides,
      Type[] extraTypes,
      XmlRootAttribute root,
      string defaultNamespace,
      string location,
      Evidence evidence)
    {
    }

    static XmlSerializer()
    {
      string str = (string) null;
      XmlSerializer.generationThreshold = -1;
      XmlSerializer.backgroundGeneration = false;
      XmlSerializer.deleteTempFiles = str == null || str == "no";
    }

    public event XmlAttributeEventHandler UnknownAttribute
    {
      add => this.onUnknownAttribute += value;
      remove => this.onUnknownAttribute -= value;
    }

    public event XmlElementEventHandler UnknownElement
    {
      add => this.onUnknownElement += value;
      remove => this.onUnknownElement -= value;
    }

    public event XmlNodeEventHandler UnknownNode
    {
      add => this.onUnknownNode += value;
      remove => this.onUnknownNode -= value;
    }

    public event UnreferencedObjectEventHandler UnreferencedObject
    {
      add => this.onUnreferencedObject += value;
      remove => this.onUnreferencedObject -= value;
    }

    internal XmlMapping Mapping => this.typeMapping;

    internal virtual void OnUnknownAttribute(XmlAttributeEventArgs e)
    {
      if (this.onUnknownAttribute == null)
        return;
      this.onUnknownAttribute((object) this, e);
    }

    internal virtual void OnUnknownElement(XmlElementEventArgs e)
    {
      if (this.onUnknownElement == null)
        return;
      this.onUnknownElement((object) this, e);
    }

    internal virtual void OnUnknownNode(XmlNodeEventArgs e)
    {
      if (this.onUnknownNode == null)
        return;
      this.onUnknownNode((object) this, e);
    }

    internal virtual void OnUnreferencedObject(UnreferencedObjectEventArgs e)
    {
      if (this.onUnreferencedObject == null)
        return;
      this.onUnreferencedObject((object) this, e);
    }

    public virtual bool CanDeserialize(XmlReader xmlReader)
    {
      int content = (int) xmlReader.MoveToContent();
      return this.typeMapping is XmlMembersMapping || this.typeMapping.ElementName == xmlReader.LocalName;
    }

    protected virtual XmlSerializationReader CreateReader() => throw new NotImplementedException();

    protected virtual XmlSerializationWriter CreateWriter() => throw new NotImplementedException();

    public object Deserialize(Stream stream) => this.Deserialize((XmlReader) new XmlTextReader(stream)
    {
      Normalization = true,
      WhitespaceHandling = WhitespaceHandling.Significant
    });

    public object Deserialize(TextReader textReader) => this.Deserialize((XmlReader) new XmlTextReader(textReader)
    {
      Normalization = true,
      WhitespaceHandling = WhitespaceHandling.Significant
    });

    public object Deserialize(XmlReader xmlReader)
    {
      XmlSerializationReader reader = !this.customSerializer ? this.CreateReader(this.typeMapping) : this.CreateReader();
      reader.Initialize(xmlReader, this);
      return this.Deserialize(reader);
    }

    protected virtual object Deserialize(XmlSerializationReader reader)
    {
      if (this.customSerializer)
        throw new NotImplementedException();
      try
      {
        return reader is XmlSerializationReaderInterpreter ? ((XmlSerializationReaderInterpreter) reader).ReadRoot() : this.serializerData.ReaderMethod.Invoke((object) reader, (object[]) null);
      }
      catch (Exception ex)
      {
        switch (ex)
        {
          case InvalidOperationException _:
          case InvalidCastException _:
            throw new InvalidOperationException("There is an error in XML document.", ex);
          default:
            throw;
        }
      }
    }

    public static XmlSerializer[] FromMappings(XmlMapping[] mappings)
    {
      XmlSerializer[] xmlSerializerArray = new XmlSerializer[mappings.Length];
      XmlSerializer.SerializerData[] serializerDataArray = new XmlSerializer.SerializerData[mappings.Length];
      XmlSerializer.GenerationBatch generationBatch = new XmlSerializer.GenerationBatch();
      generationBatch.Maps = mappings;
      generationBatch.Datas = serializerDataArray;
      for (int index = 0; index < mappings.Length; ++index)
      {
        if (mappings[index] != null)
        {
          XmlSerializer.SerializerData data = new XmlSerializer.SerializerData();
          data.Batch = generationBatch;
          xmlSerializerArray[index] = new XmlSerializer(mappings[index], data);
          serializerDataArray[index] = data;
        }
      }
      return xmlSerializerArray;
    }

    public static XmlSerializer[] FromTypes(Type[] mappings)
    {
      XmlSerializer[] xmlSerializerArray = new XmlSerializer[mappings.Length];
      for (int index = 0; index < mappings.Length; ++index)
        xmlSerializerArray[index] = new XmlSerializer(mappings[index]);
      return xmlSerializerArray;
    }

    protected virtual void Serialize(object o, XmlSerializationWriter writer)
    {
      if (this.customSerializer)
        throw new NotImplementedException();
      if (writer is XmlSerializationWriterInterpreter)
        ((XmlSerializationWriterInterpreter) writer).WriteRoot(o);
      else
        this.serializerData.WriterMethod.Invoke((object) writer, new object[1]
        {
          o
        });
    }

    public void Serialize(Stream stream, object o) => this.Serialize((XmlWriter) new XmlTextWriter(stream, Encoding.Default)
    {
      Formatting = Formatting.Indented
    }, o, (XmlSerializerNamespaces) null);

    public void Serialize(TextWriter textWriter, object o) => this.Serialize((XmlWriter) new XmlTextWriter(textWriter)
    {
      Formatting = Formatting.Indented
    }, o, (XmlSerializerNamespaces) null);

    public void Serialize(XmlWriter xmlWriter, object o) => this.Serialize(xmlWriter, o, (XmlSerializerNamespaces) null);

    public void Serialize(Stream stream, object o, XmlSerializerNamespaces namespaces) => this.Serialize((XmlWriter) new XmlTextWriter(stream, Encoding.Default)
    {
      Formatting = Formatting.Indented
    }, o, namespaces);

    public void Serialize(TextWriter textWriter, object o, XmlSerializerNamespaces namespaces)
    {
      XmlTextWriter writer = new XmlTextWriter(textWriter);
      writer.Formatting = Formatting.Indented;
      this.Serialize((XmlWriter) writer, o, namespaces);
      writer.Flush();
    }

    public void Serialize(XmlWriter writer, object o, XmlSerializerNamespaces namespaces)
    {
      try
      {
        XmlSerializationWriter writer1 = !this.customSerializer ? this.CreateWriter(this.typeMapping) : this.CreateWriter();
        if (namespaces == null || namespaces.Count == 0)
        {
          namespaces = new XmlSerializerNamespaces();
          namespaces.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");
          namespaces.Add("xsd", "http://www.w3.org/2001/XMLSchema");
        }
        writer1.Initialize(writer, namespaces);
        this.Serialize(o, writer1);
        writer.Flush();
      }
      catch (Exception ex)
      {
        Exception innerException = ex;
        if (innerException is TargetInvocationException)
          innerException = innerException.InnerException;
        if (innerException is InvalidOperationException || innerException is InvalidCastException)
          throw new InvalidOperationException("There was an error generating the XML document.", innerException);
        throw;
      }
    }

    [MonoTODO]
    public object Deserialize(
      XmlReader xmlReader,
      string encodingStyle,
      XmlDeserializationEvents events)
    {
      throw new NotImplementedException();
    }

    [MonoTODO]
    public object Deserialize(XmlReader xmlReader, string encodingStyle) => throw new NotImplementedException();

    [MonoTODO]
    public object Deserialize(XmlReader xmlReader, XmlDeserializationEvents events) => throw new NotImplementedException();

    [MonoTODO]
    public static XmlSerializer[] FromMappings(XmlMapping[] mappings, Evidence evidence) => throw new NotImplementedException();

    [MonoTODO]
    public static XmlSerializer[] FromMappings(XmlMapping[] mappings, Type type) => throw new NotImplementedException();

    public static string GetXmlSerializerAssemblyName(Type type) => type.Assembly.GetName().Name + ".XmlSerializers";

    public static string GetXmlSerializerAssemblyName(Type type, string defaultNamespace) => XmlSerializer.GetXmlSerializerAssemblyName(type) + "." + (object) defaultNamespace.GetHashCode();

    [MonoTODO]
    public void Serialize(
      XmlWriter xmlWriter,
      object o,
      XmlSerializerNamespaces namespaces,
      string encodingStyle)
    {
      throw new NotImplementedException();
    }

    [MonoNotSupported("")]
    public void Serialize(
      XmlWriter xmlWriter,
      object o,
      XmlSerializerNamespaces namespaces,
      string encodingStyle,
      string id)
    {
      throw new NotImplementedException();
    }

    private XmlSerializationWriter CreateWriter(XmlMapping typeMapping)
    {
      XmlSerializationWriter writer;
      lock (this)
      {
        if (this.serializerData != null)
        {
          lock (this.serializerData)
            writer = this.serializerData.CreateWriter();
          if (writer != null)
            return writer;
        }
      }
      if (!typeMapping.Source.CanBeGenerated || XmlSerializer.generationThreshold == -1)
        return (XmlSerializationWriter) new XmlSerializationWriterInterpreter(typeMapping);
      this.CheckGeneratedTypes(typeMapping);
      lock (this)
      {
        lock (this.serializerData)
          writer = this.serializerData.CreateWriter();
        if (writer != null)
          return writer;
        if (!XmlSerializer.generatorFallback)
          throw new InvalidOperationException("Error while generating serializer");
      }
      return (XmlSerializationWriter) new XmlSerializationWriterInterpreter(typeMapping);
    }

    private XmlSerializationReader CreateReader(XmlMapping typeMapping)
    {
      XmlSerializationReader reader;
      lock (this)
      {
        if (this.serializerData != null)
        {
          lock (this.serializerData)
            reader = this.serializerData.CreateReader();
          if (reader != null)
            return reader;
        }
      }
      if (!typeMapping.Source.CanBeGenerated || XmlSerializer.generationThreshold == -1)
        return (XmlSerializationReader) new XmlSerializationReaderInterpreter(typeMapping);
      this.CheckGeneratedTypes(typeMapping);
      lock (this)
      {
        lock (this.serializerData)
          reader = this.serializerData.CreateReader();
        if (reader != null)
          return reader;
        if (!XmlSerializer.generatorFallback)
          throw new InvalidOperationException("Error while generating serializer");
      }
      return (XmlSerializationReader) new XmlSerializationReaderInterpreter(typeMapping);
    }

    private void CheckGeneratedTypes(XmlMapping typeMapping) => throw new NotImplementedException();

    private void GenerateSerializersAsync(XmlSerializer.GenerationBatch batch) => throw new NotImplementedException();

    private void RunSerializerGeneration(object obj) => throw new NotImplementedException();

    private XmlSerializer.GenerationBatch LoadFromSatelliteAssembly(
      XmlSerializer.GenerationBatch batch)
    {
      return batch;
    }

    internal class SerializerData
    {
      public int UsageCount;
      public Type ReaderType;
      public MethodInfo ReaderMethod;
      public Type WriterType;
      public MethodInfo WriterMethod;
      public XmlSerializer.GenerationBatch Batch;
      public XmlSerializerImplementation Implementation;

      public XmlSerializationReader CreateReader()
      {
        if (this.ReaderType != null)
          return (XmlSerializationReader) Activator.CreateInstance(this.ReaderType);
        return this.Implementation != null ? this.Implementation.Reader : (XmlSerializationReader) null;
      }

      public XmlSerializationWriter CreateWriter()
      {
        if (this.WriterType != null)
          return (XmlSerializationWriter) Activator.CreateInstance(this.WriterType);
        return this.Implementation != null ? this.Implementation.Writer : (XmlSerializationWriter) null;
      }
    }

    internal class GenerationBatch
    {
      public bool Done;
      public XmlMapping[] Maps;
      public XmlSerializer.SerializerData[] Datas;
    }
  }
}
