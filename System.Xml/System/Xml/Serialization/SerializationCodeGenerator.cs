// Decompiled with JetBrains decompiler
// Type: System.Xml.Serialization.SerializationCodeGenerator
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml.Schema;

namespace System.Xml.Serialization
{
  internal class SerializationCodeGenerator
  {
    private XmlMapping _typeMap;
    private SerializationFormat _format;
    private TextWriter _writer;
    private int _tempVarId;
    private int _indent;
    private Hashtable _uniqueNames = new Hashtable();
    private int _methodId;
    private SerializerInfo _config;
    private ArrayList _mapsToGenerate = new ArrayList();
    private ArrayList _fixupCallbacks;
    private ArrayList _referencedTypes = new ArrayList();
    private GenerationResult[] _results;
    private GenerationResult _result;
    private XmlMapping[] _xmlMaps;
    private CodeIdentifiers classNames = new CodeIdentifiers();
    private ArrayList _listsToFill = new ArrayList();
    private Hashtable _hookVariables;
    private Stack _hookContexts;
    private Stack _hookOpenHooks;

    public SerializationCodeGenerator(XmlMapping[] xmlMaps)
      : this(xmlMaps, (SerializerInfo) null)
    {
    }

    public SerializationCodeGenerator(XmlMapping[] xmlMaps, SerializerInfo config)
    {
      this._xmlMaps = xmlMaps;
      this._config = config;
    }

    public SerializationCodeGenerator(XmlMapping xmlMap, SerializerInfo config)
    {
      this._xmlMaps = new XmlMapping[1]{ xmlMap };
      this._config = config;
    }

    public static void Generate(string configFileName, string outputPath)
    {
      SerializationCodeGeneratorConfiguration generatorConfiguration = (SerializationCodeGeneratorConfiguration) null;
      StreamReader streamReader = new StreamReader(configFileName);
      try
      {
        generatorConfiguration = (SerializationCodeGeneratorConfiguration) new XmlSerializer(new XmlReflectionImporter()
        {
          AllowPrivateTypes = true
        }.ImportTypeMapping(typeof (SerializationCodeGeneratorConfiguration))).Deserialize((TextReader) streamReader);
      }
      finally
      {
        streamReader.Close();
      }
      if (outputPath == null)
        outputPath = string.Empty;
      CodeIdentifiers codeIdentifiers = new CodeIdentifiers();
      if (generatorConfiguration.Serializers == null)
        return;
      foreach (SerializerInfo serializer in generatorConfiguration.Serializers)
      {
        Type type;
        if (serializer.Assembly != null)
        {
          Assembly assembly;
          try
          {
            assembly = Assembly.Load(serializer.Assembly);
          }
          catch
          {
            assembly = Assembly.LoadFrom(serializer.Assembly);
          }
          type = assembly.GetType(serializer.ClassName, true);
        }
        else
          type = Type.GetType(serializer.ClassName);
        if (type == null)
          throw new InvalidOperationException("Type " + serializer.ClassName + " not found");
        string path2 = serializer.OutFileName;
        if (path2 == null || path2.Length == 0)
        {
          int num = serializer.ClassName.LastIndexOf(".");
          string identifier = num == -1 ? serializer.ClassName : serializer.ClassName.Substring(num + 1);
          path2 = codeIdentifiers.AddUnique(identifier, (object) type) + "Serializer.cs";
        }
        StreamWriter writer = new StreamWriter(Path.Combine(outputPath, path2));
        try
        {
          new SerializationCodeGenerator(serializer.SerializationFormat != SerializationFormat.Literal ? (XmlMapping) new SoapReflectionImporter().ImportTypeMapping(type) : (XmlMapping) new XmlReflectionImporter().ImportTypeMapping(type), serializer).GenerateSerializers((TextWriter) writer);
        }
        finally
        {
          writer.Close();
        }
      }
    }

    public void GenerateSerializers(TextWriter writer)
    {
      this._writer = writer;
      this._results = new GenerationResult[this._xmlMaps.Length];
      this.WriteLine("// It is automatically generated");
      this.WriteLine("using System;");
      this.WriteLine("using System.Xml;");
      this.WriteLine("using System.Xml.Schema;");
      this.WriteLine("using System.Xml.Serialization;");
      this.WriteLine("using System.Text;");
      this.WriteLine("using System.Collections;");
      this.WriteLine("using System.Globalization;");
      if (this._config != null && this._config.NamespaceImports != null && this._config.NamespaceImports.Length > 0)
      {
        foreach (string namespaceImport in this._config.NamespaceImports)
          this.WriteLine("using " + namespaceImport + ";");
      }
      this.WriteLine(string.Empty);
      string s1 = (string) null;
      string s2 = (string) null;
      string s3 = (string) null;
      string s4 = (string) null;
      string str1 = (string) null;
      if (this._config != null)
      {
        s1 = this._config.ReaderClassName;
        s2 = this._config.WriterClassName;
        s3 = this._config.BaseSerializerClassName;
        s4 = this._config.ImplementationClassName;
        str1 = this._config.Namespace;
      }
      if (s1 == null || s1.Length == 0)
        s1 = "GeneratedReader";
      if (s2 == null || s2.Length == 0)
        s2 = "GeneratedWriter";
      if (s3 == null || s3.Length == 0)
        s3 = "BaseXmlSerializer";
      if (s4 == null || s4.Length == 0)
        s4 = "XmlSerializerContract";
      string uniqueClassName1 = this.GetUniqueClassName(s1);
      string uniqueClassName2 = this.GetUniqueClassName(s2);
      string uniqueClassName3 = this.GetUniqueClassName(s3);
      string uniqueClassName4 = this.GetUniqueClassName(s4);
      Hashtable hashtable1 = new Hashtable();
      Hashtable hashtable2 = new Hashtable();
      for (int index = 0; index < this._xmlMaps.Length; ++index)
      {
        this._typeMap = this._xmlMaps[index];
        if (this._typeMap != null)
        {
          this._result = hashtable2[(object) this._typeMap] as GenerationResult;
          if (this._result != null)
          {
            this._results[index] = this._result;
          }
          else
          {
            this._result = new GenerationResult();
            this._results[index] = this._result;
            hashtable2[(object) this._typeMap] = (object) this._result;
            string str2 = !(this._typeMap is XmlTypeMapping) ? this._typeMap.ElementName : CodeIdentifier.MakeValid(((XmlTypeMapping) this._typeMap).TypeData.CSharpName);
            this._result.ReaderClassName = uniqueClassName1;
            this._result.WriterClassName = uniqueClassName2;
            this._result.BaseSerializerClassName = uniqueClassName3;
            this._result.ImplementationClassName = uniqueClassName4;
            this._result.Namespace = str1 == null || str1.Length == 0 ? "Mono.GeneratedSerializers." + (object) this._typeMap.Format : str1;
            this._result.WriteMethodName = this.GetUniqueName("rwo", (object) this._typeMap, "WriteRoot_" + str2);
            this._result.ReadMethodName = this.GetUniqueName("rro", (object) this._typeMap, "ReadRoot_" + str2);
            this._result.Mapping = this._typeMap;
            ArrayList arrayList = (ArrayList) hashtable1[(object) this._result.Namespace];
            if (arrayList == null)
            {
              arrayList = new ArrayList();
              hashtable1[(object) this._result.Namespace] = (object) arrayList;
            }
            arrayList.Add((object) this._result);
          }
        }
      }
      foreach (DictionaryEntry dictionaryEntry in hashtable1)
      {
        ArrayList arrayList = (ArrayList) dictionaryEntry.Value;
        this.WriteLine("namespace " + dictionaryEntry.Key);
        this.WriteLineInd("{");
        if (this._config == null || !this._config.NoReader)
          this.GenerateReader(uniqueClassName1, arrayList);
        this.WriteLine(string.Empty);
        if (this._config == null || !this._config.NoWriter)
          this.GenerateWriter(uniqueClassName2, arrayList);
        this.WriteLine(string.Empty);
        this.GenerateContract(arrayList);
        this.WriteLineUni("}");
        this.WriteLine(string.Empty);
      }
    }

    public GenerationResult[] GenerationResults => this._results;

    public ArrayList ReferencedTypes => this._referencedTypes;

    private void UpdateGeneratedTypes(ArrayList list)
    {
      for (int index = 0; index < list.Count; ++index)
      {
        if (list[index] is XmlTypeMapping xmlTypeMapping && !this._referencedTypes.Contains((object) xmlTypeMapping.TypeData.Type))
          this._referencedTypes.Add((object) xmlTypeMapping.TypeData.Type);
      }
    }

    private static string ToCSharpFullName(Type type) => TypeData.ToCSharpName(type, true);

    public void GenerateContract(ArrayList generatedMaps)
    {
      if (generatedMaps.Count == 0)
        return;
      GenerationResult generatedMap1 = (GenerationResult) generatedMaps[0];
      string serializerClassName = generatedMap1.BaseSerializerClassName;
      string str = this._config == null || !this._config.GenerateAsInternal ? "public" : "internal";
      this.WriteLine(string.Empty);
      this.WriteLine(str + " class " + serializerClassName + " : System.Xml.Serialization.XmlSerializer");
      this.WriteLineInd("{");
      this.WriteLineInd("protected override System.Xml.Serialization.XmlSerializationReader CreateReader () {");
      this.WriteLine("return new " + generatedMap1.ReaderClassName + " ();");
      this.WriteLineUni("}");
      this.WriteLine(string.Empty);
      this.WriteLineInd("protected override System.Xml.Serialization.XmlSerializationWriter CreateWriter () {");
      this.WriteLine("return new " + generatedMap1.WriterClassName + " ();");
      this.WriteLineUni("}");
      this.WriteLine(string.Empty);
      this.WriteLineInd("public override bool CanDeserialize (System.Xml.XmlReader xmlReader) {");
      this.WriteLine("return true;");
      this.WriteLineUni("}");
      this.WriteLineUni("}");
      this.WriteLine(string.Empty);
      foreach (GenerationResult generatedMap2 in generatedMaps)
      {
        generatedMap2.SerializerClassName = this.GetUniqueClassName(generatedMap2.Mapping.ElementName + "Serializer");
        this.WriteLine(str + " sealed class " + generatedMap2.SerializerClassName + " : " + serializerClassName);
        this.WriteLineInd("{");
        this.WriteLineInd("protected override void Serialize (object obj, System.Xml.Serialization.XmlSerializationWriter writer) {");
        this.WriteLine("((" + generatedMap2.WriterClassName + ")writer)." + generatedMap2.WriteMethodName + "(obj);");
        this.WriteLineUni("}");
        this.WriteLine(string.Empty);
        this.WriteLineInd("protected override object Deserialize (System.Xml.Serialization.XmlSerializationReader reader) {");
        this.WriteLine("return ((" + generatedMap2.ReaderClassName + ")reader)." + generatedMap2.ReadMethodName + "();");
        this.WriteLineUni("}");
        this.WriteLineUni("}");
        this.WriteLine(string.Empty);
      }
      this.WriteLine("#if !TARGET_JVM");
      this.WriteLine(str + " class " + generatedMap1.ImplementationClassName + " : System.Xml.Serialization.XmlSerializerImplementation");
      this.WriteLineInd("{");
      this.WriteLine("System.Collections.Hashtable readMethods = null;");
      this.WriteLine("System.Collections.Hashtable writeMethods = null;");
      this.WriteLine("System.Collections.Hashtable typedSerializers = null;");
      this.WriteLine(string.Empty);
      this.WriteLineInd("public override System.Xml.Serialization.XmlSerializationReader Reader {");
      this.WriteLineInd("get {");
      this.WriteLine("return new " + generatedMap1.ReaderClassName + "();");
      this.WriteLineUni("}");
      this.WriteLineUni("}");
      this.WriteLine(string.Empty);
      this.WriteLineInd("public override System.Xml.Serialization.XmlSerializationWriter Writer {");
      this.WriteLineInd("get {");
      this.WriteLine("return new " + generatedMap1.WriterClassName + "();");
      this.WriteLineUni("}");
      this.WriteLineUni("}");
      this.WriteLine(string.Empty);
      this.WriteLineInd("public override System.Collections.Hashtable ReadMethods {");
      this.WriteLineInd("get {");
      this.WriteLineInd("lock (this) {");
      this.WriteLineInd("if (readMethods == null) {");
      this.WriteLine("readMethods = new System.Collections.Hashtable ();");
      foreach (GenerationResult generatedMap3 in generatedMaps)
        this.WriteLine("readMethods.Add (@\"" + generatedMap3.Mapping.GetKey() + "\", @\"" + generatedMap3.ReadMethodName + "\");");
      this.WriteLineUni("}");
      this.WriteLine("return readMethods;");
      this.WriteLineUni("}");
      this.WriteLineUni("}");
      this.WriteLineUni("}");
      this.WriteLine(string.Empty);
      this.WriteLineInd("public override System.Collections.Hashtable WriteMethods {");
      this.WriteLineInd("get {");
      this.WriteLineInd("lock (this) {");
      this.WriteLineInd("if (writeMethods == null) {");
      this.WriteLine("writeMethods = new System.Collections.Hashtable ();");
      foreach (GenerationResult generatedMap4 in generatedMaps)
        this.WriteLine("writeMethods.Add (@\"" + generatedMap4.Mapping.GetKey() + "\", @\"" + generatedMap4.WriteMethodName + "\");");
      this.WriteLineUni("}");
      this.WriteLine("return writeMethods;");
      this.WriteLineUni("}");
      this.WriteLineUni("}");
      this.WriteLineUni("}");
      this.WriteLine(string.Empty);
      this.WriteLineInd("public override System.Collections.Hashtable TypedSerializers {");
      this.WriteLineInd("get {");
      this.WriteLineInd("lock (this) {");
      this.WriteLineInd("if (typedSerializers == null) {");
      this.WriteLine("typedSerializers = new System.Collections.Hashtable ();");
      foreach (GenerationResult generatedMap5 in generatedMaps)
        this.WriteLine("typedSerializers.Add (@\"" + generatedMap5.Mapping.GetKey() + "\", new " + generatedMap5.SerializerClassName + "());");
      this.WriteLineUni("}");
      this.WriteLine("return typedSerializers;");
      this.WriteLineUni("}");
      this.WriteLineUni("}");
      this.WriteLineUni("}");
      this.WriteLine(string.Empty);
      this.WriteLine("public override XmlSerializer GetSerializer (Type type)");
      this.WriteLineInd("{");
      this.WriteLine("switch (type.FullName) {");
      foreach (GenerationResult generatedMap6 in generatedMaps)
      {
        if (generatedMap6.Mapping is XmlTypeMapping)
        {
          this.WriteLineInd("case \"" + ((XmlTypeMapping) generatedMap6.Mapping).TypeData.CSharpFullName + "\":");
          this.WriteLine("return (XmlSerializer) TypedSerializers [\"" + generatedMap6.Mapping.GetKey() + "\"];");
          this.WriteLineUni(string.Empty);
        }
      }
      this.WriteLine("}");
      this.WriteLine("return base.GetSerializer (type);");
      this.WriteLineUni("}");
      this.WriteLine(string.Empty);
      this.WriteLineInd("public override bool CanSerialize (System.Type type) {");
      foreach (GenerationResult generatedMap7 in generatedMaps)
      {
        if (generatedMap7.Mapping is XmlTypeMapping)
          this.WriteLine("if (type == typeof(" + (generatedMap7.Mapping as XmlTypeMapping).TypeData.CSharpFullName + ")) return true;");
      }
      this.WriteLine("return false;");
      this.WriteLineUni("}");
      this.WriteLineUni("}");
      this.WriteLine(string.Empty);
      this.WriteLine("#endif");
    }

    public void GenerateWriter(string writerClassName, ArrayList maps)
    {
      this._mapsToGenerate = new ArrayList();
      this.InitHooks();
      if (this._config == null || !this._config.GenerateAsInternal)
        this.WriteLine("public class " + writerClassName + " : XmlSerializationWriter");
      else
        this.WriteLine("internal class " + writerClassName + " : XmlSerializationWriter");
      this.WriteLineInd("{");
      this.WriteLine("const string xmlNamespace = \"http://www.w3.org/2000/xmlns/\";");
      this.WriteLine("static readonly System.Reflection.MethodInfo toBinHexStringMethod = typeof (XmlConvert).GetMethod (\"ToBinHexString\", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic, null, new Type [] {typeof (byte [])}, null);");
      this.WriteLine("static string ToBinHexString (byte [] input)");
      this.WriteLineInd("{");
      this.WriteLine("return input == null ? null : (string) toBinHexStringMethod.Invoke (null, new object [] {input});");
      this.WriteLineUni("}");
      for (int index = 0; index < maps.Count; ++index)
      {
        GenerationResult map = (GenerationResult) maps[index];
        this._typeMap = map.Mapping;
        this._format = this._typeMap.Format;
        this._result = map;
        this.GenerateWriteRoot();
      }
      for (int index = 0; index < this._mapsToGenerate.Count; ++index)
      {
        XmlTypeMapping xmlTypeMapping = (XmlTypeMapping) this._mapsToGenerate[index];
        this.GenerateWriteObject(xmlTypeMapping);
        if (xmlTypeMapping.TypeData.SchemaType == SchemaTypes.Enum)
          this.GenerateGetXmlEnumValue(xmlTypeMapping);
      }
      this.GenerateWriteInitCallbacks();
      this.UpdateGeneratedTypes(this._mapsToGenerate);
      this.WriteLineUni("}");
    }

    private void GenerateWriteRoot()
    {
      this.WriteLine("public void " + this._result.WriteMethodName + " (object o)");
      this.WriteLineInd("{");
      this.WriteLine("WriteStartDocument ();");
      if (this._typeMap is XmlTypeMapping)
      {
        this.WriteLine(this.GetRootTypeName() + " ob = (" + this.GetRootTypeName() + ") o;");
        XmlTypeMapping typeMap = (XmlTypeMapping) this._typeMap;
        if (typeMap.TypeData.SchemaType == SchemaTypes.Class || typeMap.TypeData.SchemaType == SchemaTypes.Array)
          this.WriteLine("TopLevelElement ();");
        if (this._format == SerializationFormat.Literal)
        {
          this.WriteLine(this.GetWriteObjectName(typeMap) + " (ob, " + this.GetLiteral((object) typeMap.ElementName) + ", " + this.GetLiteral((object) typeMap.Namespace) + ", true, false, true);");
        }
        else
        {
          this.RegisterReferencingMap(typeMap);
          this.WriteLine("WritePotentiallyReferencingElement (" + this.GetLiteral((object) typeMap.ElementName) + ", " + this.GetLiteral((object) typeMap.Namespace) + ", ob, " + this.GetTypeOf(typeMap.TypeData) + ", true, false);");
        }
      }
      else
      {
        if (!(this._typeMap is XmlMembersMapping))
          throw new InvalidOperationException("Unknown type map");
        this.WriteLine("object[] pars = (object[]) o;");
        this.GenerateWriteMessage((XmlMembersMapping) this._typeMap);
      }
      if (this._format == SerializationFormat.Encoded)
        this.WriteLine("WriteReferencedElements ();");
      this.WriteLineUni("}");
      this.WriteLine(string.Empty);
    }

    private void GenerateWriteMessage(XmlMembersMapping membersMap)
    {
      if (membersMap.HasWrapperElement)
      {
        this.WriteLine("TopLevelElement ();");
        this.WriteLine("WriteStartElement (" + this.GetLiteral((object) membersMap.ElementName) + ", " + this.GetLiteral((object) membersMap.Namespace) + ", (" + this.GetLiteral((object) (this._format == SerializationFormat.Encoded)) + "));");
      }
      this.GenerateWriteObjectElement((XmlMapping) membersMap, "pars", true);
      if (!membersMap.HasWrapperElement)
        return;
      this.WriteLine("WriteEndElement();");
    }

    private void GenerateGetXmlEnumValue(XmlTypeMapping map)
    {
      EnumMap objectMap = (EnumMap) map.ObjectMap;
      string str1 = (string) null;
      string str2 = (string) null;
      if (objectMap.IsFlags)
      {
        str1 = this.GetUniqueName("gxe", (object) map, "_xmlNames" + map.XmlType);
        this.Write("static readonly string[] " + str1 + " = { ");
        for (int index = 0; index < objectMap.XmlNames.Length; ++index)
        {
          if (index > 0)
            this._writer.Write(',');
          this._writer.Write('"');
          this._writer.Write(objectMap.XmlNames[index]);
          this._writer.Write('"');
        }
        this._writer.WriteLine(" };");
        str2 = this.GetUniqueName("gve", (object) map, "_values" + map.XmlType);
        this.Write("static readonly long[] " + str2 + " = { ");
        for (int index = 0; index < objectMap.Values.Length; ++index)
        {
          if (index > 0)
            this._writer.Write(',');
          this._writer.Write(objectMap.Values[index].ToString((IFormatProvider) CultureInfo.InvariantCulture));
          this._writer.Write("L");
        }
        this._writer.WriteLine(" };");
        this.WriteLine(string.Empty);
      }
      this.WriteLine("string " + this.GetGetEnumValueName(map) + " (" + map.TypeData.CSharpFullName + " val)");
      this.WriteLineInd("{");
      this.WriteLineInd("switch (val) {");
      for (int index = 0; index < objectMap.EnumNames.Length; ++index)
        this.WriteLine("case " + map.TypeData.CSharpFullName + ".@" + objectMap.EnumNames[index] + ": return " + this.GetLiteral((object) objectMap.XmlNames[index]) + ";");
      if (objectMap.IsFlags)
      {
        this.WriteLineInd("default:");
        this.WriteLine("if (val.ToString () == \"0\") return string.Empty;");
        this.Write("return FromEnum ((long) val, " + str1 + ", " + str2);
        this._writer.Write(", typeof (");
        this._writer.Write(map.TypeData.CSharpFullName);
        this._writer.Write(").FullName");
        this._writer.Write(')');
        this.WriteUni(";");
      }
      else
        this.WriteLine("default: throw CreateInvalidEnumValueException ((long) val, typeof (" + map.TypeData.CSharpFullName + ").FullName);");
      this.WriteLineUni("}");
      this.WriteLineUni("}");
      this.WriteLine(string.Empty);
    }

    private void GenerateWriteObject(XmlTypeMapping typeMap)
    {
      this.WriteLine("void " + this.GetWriteObjectName(typeMap) + " (" + typeMap.TypeData.CSharpFullName + " ob, string element, string namesp, bool isNullable, bool needType, bool writeWrappingElem)");
      this.WriteLineInd("{");
      this.PushHookContext();
      this.SetHookVar("$TYPE", typeMap.TypeData.CSharpName);
      this.SetHookVar("$FULLTYPE", typeMap.TypeData.CSharpFullName);
      this.SetHookVar("$OBJECT", "ob");
      this.SetHookVar("$NULLABLE", "isNullable");
      if (this.GenerateWriteHook(HookType.type, typeMap.TypeData.Type))
      {
        this.WriteLineUni("}");
        this.WriteLine(string.Empty);
        this.PopHookContext();
      }
      else
      {
        if (!typeMap.TypeData.IsValueType)
        {
          this.WriteLine("if (((object)ob) == null)");
          this.WriteLineInd("{");
          this.WriteLineInd("if (isNullable)");
          if (this._format == SerializationFormat.Literal)
            this.WriteLine("WriteNullTagLiteral(element, namesp);");
          else
            this.WriteLine("WriteNullTagEncoded (element, namesp);");
          this.WriteLineUni("return;");
          this.WriteLineUni("}");
          this.WriteLine(string.Empty);
        }
        if (typeMap.TypeData.SchemaType == SchemaTypes.XmlNode)
        {
          if (this._format == SerializationFormat.Literal)
            this.WriteLine("WriteElementLiteral (ob, \"\", \"\", true, false);");
          else
            this.WriteLine("WriteElementEncoded (ob, \"\", \"\", true, false);");
          this.GenerateEndHook();
          this.WriteLineUni("}");
          this.WriteLine(string.Empty);
          this.PopHookContext();
        }
        else if (typeMap.TypeData.SchemaType == SchemaTypes.XmlSerializable)
        {
          this.WriteLine("WriteSerializable (ob, element, namesp, isNullable);");
          this.GenerateEndHook();
          this.WriteLineUni("}");
          this.WriteLine(string.Empty);
          this.PopHookContext();
        }
        else
        {
          ArrayList derivedTypes = typeMap.DerivedTypes;
          this.WriteLine("System.Type type = ob.GetType ();");
          this.WriteLine("if (type == typeof(" + typeMap.TypeData.CSharpFullName + "))");
          this.WriteLine("{ }");
          for (int index = 0; index < derivedTypes.Count; ++index)
          {
            XmlTypeMapping typeMap1 = (XmlTypeMapping) derivedTypes[index];
            this.WriteLineInd("else if (type == typeof(" + typeMap1.TypeData.CSharpFullName + ")) { ");
            this.WriteLine(this.GetWriteObjectName(typeMap1) + "((" + typeMap1.TypeData.CSharpFullName + ")ob, element, namesp, isNullable, true, writeWrappingElem);");
            this.WriteLine("return;");
            this.WriteLineUni("}");
          }
          if (typeMap.TypeData.Type == typeof (object))
          {
            this.WriteLineInd("else {");
            this.WriteLineInd("if (ob.GetType().IsArray && typeof(XmlNode).IsAssignableFrom(ob.GetType().GetElementType())) {");
            this.WriteLine("Writer.WriteStartElement (" + this.GetLiteral((object) typeMap.ElementName) + ", " + this.GetLiteral((object) typeMap.Namespace) + ");");
            this.WriteLineInd("foreach (XmlNode node in (System.Collections.IEnumerable) ob)");
            this.WriteLineUni("node.WriteTo (Writer);");
            this.WriteLineUni("Writer.WriteEndElement ();");
            this.WriteLine("}");
            this.WriteLineInd("else");
            this.WriteLineUni("WriteTypedPrimitive (element, namesp, ob, true);");
            this.WriteLine("return;");
            this.WriteLineUni("}");
          }
          else
          {
            this.WriteLineInd("else {");
            this.WriteLine("throw CreateUnknownTypeException (ob);");
            this.WriteLineUni("}");
            this.WriteLine(string.Empty);
            this.WriteLineInd("if (writeWrappingElem) {");
            if (this._format == SerializationFormat.Encoded)
              this.WriteLine("needType = true;");
            this.WriteLine("WriteStartElement (element, namesp, ob);");
            this.WriteLineUni("}");
            this.WriteLine(string.Empty);
            this.WriteLine("if (needType) WriteXsiType(" + this.GetLiteral((object) typeMap.XmlType) + ", " + this.GetLiteral((object) typeMap.XmlTypeNamespace) + ");");
            this.WriteLine(string.Empty);
            switch (typeMap.TypeData.SchemaType)
            {
              case SchemaTypes.Primitive:
                this.GenerateWritePrimitiveElement(typeMap, "ob");
                break;
              case SchemaTypes.Enum:
                this.GenerateWriteEnumElement(typeMap, "ob");
                break;
              case SchemaTypes.Array:
                this.GenerateWriteListElement(typeMap, "ob");
                break;
              case SchemaTypes.Class:
                this.GenerateWriteObjectElement((XmlMapping) typeMap, "ob", false);
                break;
            }
            this.WriteLine("if (writeWrappingElem) WriteEndElement (ob);");
          }
          this.GenerateEndHook();
          this.WriteLineUni("}");
          this.WriteLine(string.Empty);
          this.PopHookContext();
        }
      }
    }

    private void GenerateWriteObjectElement(XmlMapping xmlMap, string ob, bool isValueList)
    {
      Type type1 = !(xmlMap is XmlTypeMapping xmlTypeMapping) ? typeof (object[]) : xmlTypeMapping.TypeData.Type;
      ClassMap objectMap = (ClassMap) xmlMap.ObjectMap;
      if (!this.GenerateWriteHook(HookType.attributes, type1))
      {
        if (objectMap.NamespaceDeclarations != null)
        {
          this.WriteLine("WriteNamespaceDeclarations ((XmlSerializerNamespaces) " + ob + ".@" + objectMap.NamespaceDeclarations.Name + ");");
          this.WriteLine(string.Empty);
        }
        XmlTypeMapMember anyAttributeMember = (XmlTypeMapMember) objectMap.DefaultAnyAttributeMember;
        if (anyAttributeMember != null && !this.GenerateWriteMemberHook(type1, anyAttributeMember))
        {
          string hasValueCondition = this.GenerateMemberHasValueCondition(anyAttributeMember, ob, isValueList);
          if (hasValueCondition != null)
            this.WriteLineInd("if (" + hasValueCondition + ") {");
          string obTempVar1 = this.GetObTempVar();
          this.WriteLine("ICollection " + obTempVar1 + " = " + this.GenerateGetMemberValue(anyAttributeMember, ob, isValueList) + ";");
          this.WriteLineInd("if (" + obTempVar1 + " != null) {");
          string obTempVar2 = this.GetObTempVar();
          this.WriteLineInd("foreach (XmlAttribute " + obTempVar2 + " in " + obTempVar1 + ")");
          this.WriteLineInd("if (" + obTempVar2 + ".NamespaceURI != xmlNamespace)");
          this.WriteLine("WriteXmlAttribute (" + obTempVar2 + ", " + ob + ");");
          this.Unindent();
          this.Unindent();
          this.WriteLineUni("}");
          if (hasValueCondition != null)
            this.WriteLineUni("}");
          this.WriteLine(string.Empty);
          this.GenerateEndHook();
        }
        ICollection attributeMembers = objectMap.AttributeMembers;
        if (attributeMembers != null)
        {
          foreach (XmlTypeMapMemberAttribute member in (IEnumerable) attributeMembers)
          {
            if (!this.GenerateWriteMemberHook(type1, (XmlTypeMapMember) member))
            {
              string getMemberValue = this.GenerateGetMemberValue((XmlTypeMapMember) member, ob, isValueList);
              string hasValueCondition = this.GenerateMemberHasValueCondition((XmlTypeMapMember) member, ob, isValueList);
              if (hasValueCondition != null)
                this.WriteLineInd("if (" + hasValueCondition + ") {");
              string getStringValue = this.GenerateGetStringValue(member.MappedType, member.TypeData, getMemberValue, false);
              this.WriteLine("WriteAttribute (" + this.GetLiteral((object) member.AttributeName) + ", " + this.GetLiteral((object) member.Namespace) + ", " + getStringValue + ");");
              if (hasValueCondition != null)
                this.WriteLineUni("}");
              this.GenerateEndHook();
            }
          }
          this.WriteLine(string.Empty);
        }
        this.GenerateEndHook();
      }
      if (this.GenerateWriteHook(HookType.elements, type1))
        return;
      ICollection elementMembers = objectMap.ElementMembers;
      if (elementMembers != null)
      {
        foreach (XmlTypeMapMemberElement member in (IEnumerable) elementMembers)
        {
          if (!this.GenerateWriteMemberHook(type1, (XmlTypeMapMember) member))
          {
            string hasValueCondition = this.GenerateMemberHasValueCondition((XmlTypeMapMember) member, ob, isValueList);
            if (hasValueCondition != null)
              this.WriteLineInd("if (" + hasValueCondition + ") {");
            string getMemberValue = this.GenerateGetMemberValue((XmlTypeMapMember) member, ob, isValueList);
            Type type2 = member.GetType();
            if (type2 == typeof (XmlTypeMapMemberList))
              this.GenerateWriteMemberElement((XmlTypeMapElementInfo) member.ElementInfo[0], getMemberValue);
            else if (type2 == typeof (XmlTypeMapMemberFlatList))
            {
              this.WriteLineInd("if (" + getMemberValue + " != null) {");
              this.GenerateWriteListContent(ob, member.TypeData, ((XmlTypeMapMemberFlatList) member).ListMap, getMemberValue, false);
              this.WriteLineUni("}");
            }
            else if (type2 == typeof (XmlTypeMapMemberAnyElement))
            {
              this.WriteLineInd("if (" + getMemberValue + " != null) {");
              this.GenerateWriteAnyElementContent((XmlTypeMapMemberAnyElement) member, getMemberValue);
              this.WriteLineUni("}");
            }
            else if (type2 == typeof (XmlTypeMapMemberAnyElement))
            {
              this.WriteLineInd("if (" + getMemberValue + " != null) {");
              this.GenerateWriteAnyElementContent((XmlTypeMapMemberAnyElement) member, getMemberValue);
              this.WriteLineUni("}");
            }
            else if (type2 != typeof (XmlTypeMapMemberAnyAttribute))
            {
              if (type2 != typeof (XmlTypeMapMemberElement))
                throw new InvalidOperationException("Unknown member type");
              if (member.ElementInfo.Count == 1)
                this.GenerateWriteMemberElement((XmlTypeMapElementInfo) member.ElementInfo[0], getMemberValue);
              else if (member.ChoiceMember != null)
              {
                string str = ob + ".@" + member.ChoiceMember;
                foreach (XmlTypeMapElementInfo elem in (ArrayList) member.ElementInfo)
                {
                  this.WriteLineInd("if (" + str + " == " + this.GetLiteral(elem.ChoiceValue) + ") {");
                  this.GenerateWriteMemberElement(elem, this.GetCast(elem.TypeData, member.TypeData, getMemberValue));
                  this.WriteLineUni("}");
                }
              }
              else
              {
                bool flag = true;
                foreach (XmlTypeMapElementInfo elem in (ArrayList) member.ElementInfo)
                {
                  this.WriteLineInd((!flag ? "else " : string.Empty) + "if (" + getMemberValue + " is " + elem.TypeData.CSharpFullName + ") {");
                  this.GenerateWriteMemberElement(elem, this.GetCast(elem.TypeData, member.TypeData, getMemberValue));
                  this.WriteLineUni("}");
                  flag = false;
                }
              }
            }
            if (hasValueCondition != null)
              this.WriteLineUni("}");
            this.GenerateEndHook();
          }
        }
      }
      this.GenerateEndHook();
    }

    private void GenerateWriteMemberElement(XmlTypeMapElementInfo elem, string memberValue)
    {
      switch (elem.TypeData.SchemaType)
      {
        case SchemaTypes.Primitive:
        case SchemaTypes.Enum:
          if (this._format == SerializationFormat.Literal)
          {
            this.GenerateWritePrimitiveValueLiteral(memberValue, elem.ElementName, elem.Namespace, elem.MappedType, elem.TypeData, elem.WrappedElement, elem.IsNullable);
            break;
          }
          this.GenerateWritePrimitiveValueEncoded(memberValue, elem.ElementName, elem.Namespace, new XmlQualifiedName(elem.TypeData.XmlType, elem.DataTypeNamespace), elem.MappedType, elem.TypeData, elem.WrappedElement, elem.IsNullable);
          break;
        case SchemaTypes.Array:
          this.WriteLineInd("if (" + memberValue + " != null) {");
          if (elem.MappedType.MultiReferenceType)
          {
            this.WriteMetCall("WriteReferencingElement", this.GetLiteral((object) elem.ElementName), this.GetLiteral((object) elem.Namespace), memberValue, this.GetLiteral((object) elem.IsNullable));
            this.RegisterReferencingMap(elem.MappedType);
          }
          else
          {
            this.WriteMetCall("WriteStartElement", this.GetLiteral((object) elem.ElementName), this.GetLiteral((object) elem.Namespace), memberValue);
            this.GenerateWriteListContent((string) null, elem.TypeData, (ListMap) elem.MappedType.ObjectMap, memberValue, false);
            this.WriteMetCall("WriteEndElement", memberValue);
          }
          this.WriteLineUni("}");
          if (!elem.IsNullable)
            break;
          this.WriteLineInd("else");
          if (this._format == SerializationFormat.Literal)
            this.WriteMetCall("WriteNullTagLiteral", this.GetLiteral((object) elem.ElementName), this.GetLiteral((object) elem.Namespace));
          else
            this.WriteMetCall("WriteNullTagEncoded", this.GetLiteral((object) elem.ElementName), this.GetLiteral((object) elem.Namespace));
          this.Unindent();
          break;
        case SchemaTypes.Class:
          if (elem.MappedType.MultiReferenceType)
          {
            this.RegisterReferencingMap(elem.MappedType);
            if (elem.MappedType.TypeData.Type == typeof (object))
            {
              this.WriteMetCall("WritePotentiallyReferencingElement", this.GetLiteral((object) elem.ElementName), this.GetLiteral((object) elem.Namespace), memberValue, "null", "false", this.GetLiteral((object) elem.IsNullable));
              break;
            }
            this.WriteMetCall("WriteReferencingElement", this.GetLiteral((object) elem.ElementName), this.GetLiteral((object) elem.Namespace), memberValue, this.GetLiteral((object) elem.IsNullable));
            break;
          }
          this.WriteMetCall(this.GetWriteObjectName(elem.MappedType), memberValue, this.GetLiteral((object) elem.ElementName), this.GetLiteral((object) elem.Namespace), this.GetLiteral((object) elem.IsNullable), "false", "true");
          break;
        case SchemaTypes.XmlSerializable:
          this.WriteMetCall("WriteSerializable", "(" + SerializationCodeGenerator.ToCSharpFullName(elem.MappedType.TypeData.Type) + ") " + memberValue, this.GetLiteral((object) elem.ElementName), this.GetLiteral((object) elem.Namespace), this.GetLiteral((object) elem.IsNullable));
          break;
        case SchemaTypes.XmlNode:
          string ob = !elem.WrappedElement ? string.Empty : elem.ElementName;
          if (this._format == SerializationFormat.Literal)
          {
            this.WriteMetCall("WriteElementLiteral", memberValue, this.GetLiteral((object) ob), this.GetLiteral((object) elem.Namespace), this.GetLiteral((object) elem.IsNullable), "false");
            break;
          }
          this.WriteMetCall("WriteElementEncoded", memberValue, this.GetLiteral((object) ob), this.GetLiteral((object) elem.Namespace), this.GetLiteral((object) elem.IsNullable), "false");
          break;
        default:
          throw new NotSupportedException("Invalid value type");
      }
    }

    private void GenerateWriteListElement(XmlTypeMapping typeMap, string ob)
    {
      if (this._format == SerializationFormat.Encoded)
      {
        string getListCount = this.GenerateGetListCount(typeMap.TypeData, ob);
        string localName;
        string ns;
        this.GenerateGetArrayType((ListMap) typeMap.ObjectMap, getListCount, out localName, out ns);
        string str;
        if (ns != string.Empty)
          str = "FromXmlQualifiedName (new XmlQualifiedName(" + localName + "," + ns + "))";
        else
          str = this.GetLiteral((object) localName);
        this.WriteMetCall("WriteAttribute", this.GetLiteral((object) "arrayType"), this.GetLiteral((object) "http://schemas.xmlsoap.org/soap/encoding/"), str);
      }
      this.GenerateWriteListContent((string) null, typeMap.TypeData, (ListMap) typeMap.ObjectMap, ob, false);
    }

    private void GenerateWriteAnyElementContent(
      XmlTypeMapMemberAnyElement member,
      string memberValue)
    {
      bool flag = member.TypeData.Type == typeof (XmlElement);
      string str1;
      if (flag)
      {
        str1 = memberValue;
      }
      else
      {
        str1 = this.GetObTempVar();
        this.WriteLineInd("foreach (XmlNode " + str1 + " in " + memberValue + ") {");
      }
      string obTempVar = this.GetObTempVar();
      this.WriteLine("XmlNode " + obTempVar + " = " + str1 + ";");
      this.WriteLine("if (" + obTempVar + " is XmlElement) {");
      if (!member.IsDefaultAny)
      {
        for (int index = 0; index < member.ElementInfo.Count; ++index)
        {
          XmlTypeMapElementInfo typeMapElementInfo = (XmlTypeMapElementInfo) member.ElementInfo[index];
          string str2 = "(" + obTempVar + ".LocalName == " + this.GetLiteral((object) typeMapElementInfo.ElementName) + " && " + obTempVar + ".NamespaceURI == " + this.GetLiteral((object) typeMapElementInfo.Namespace) + ")";
          if (index == member.ElementInfo.Count - 1)
            str2 += ") {";
          if (index == 0)
            this.WriteLineInd("if (" + str2);
          else
            this.WriteLine("|| " + str2);
        }
      }
      this.WriteLine("}");
      this.WriteLine("else " + obTempVar + ".WriteTo (Writer);");
      if (this._format == SerializationFormat.Literal)
        this.WriteLine("WriteElementLiteral (" + obTempVar + ", \"\", \"\", false, true);");
      else
        this.WriteLine("WriteElementEncoded (" + obTempVar + ", \"\", \"\", false, true);");
      if (!member.IsDefaultAny)
      {
        this.WriteLineUni("}");
        this.WriteLineInd("else");
        this.WriteLine("throw CreateUnknownAnyElementException (" + obTempVar + ".Name, " + obTempVar + ".NamespaceURI);");
        this.Unindent();
      }
      if (flag)
        return;
      this.WriteLineUni("}");
    }

    private void GenerateWritePrimitiveElement(XmlTypeMapping typeMap, string ob) => this.WriteLine("Writer.WriteString (" + this.GenerateGetStringValue(typeMap, typeMap.TypeData, ob, false) + ");");

    private void GenerateWriteEnumElement(XmlTypeMapping typeMap, string ob) => this.WriteLine("Writer.WriteString (" + this.GenerateGetEnumXmlValue(typeMap, ob) + ");");

    private string GenerateGetStringValue(
      XmlTypeMapping typeMap,
      TypeData type,
      string value,
      bool isNullable)
    {
      if (type.SchemaType == SchemaTypes.Array)
      {
        string strTempVar = this.GetStrTempVar();
        this.WriteLine("string " + strTempVar + " = null;");
        this.WriteLineInd("if (" + value + " != null) {");
        string writeListContent = this.GenerateWriteListContent((string) null, typeMap.TypeData, (ListMap) typeMap.ObjectMap, value, true);
        this.WriteLine(strTempVar + " = " + writeListContent + ".ToString ().Trim ();");
        this.WriteLineUni("}");
        return strTempVar;
      }
      if (type.SchemaType == SchemaTypes.Enum)
      {
        if (!isNullable)
          return this.GenerateGetEnumXmlValue(typeMap, value);
        return "(" + value + ").HasValue ? " + this.GenerateGetEnumXmlValue(typeMap, "(" + value + ").Value") + " : null";
      }
      if (type.Type == typeof (XmlQualifiedName))
        return "FromXmlQualifiedName (" + value + ")";
      return value == null ? (string) null : XmlCustomFormatter.GenerateToXmlString(type, value);
    }

    private string GenerateGetEnumXmlValue(XmlTypeMapping typeMap, string ob) => this.GetGetEnumValueName(typeMap) + " (" + ob + ")";

    private string GenerateGetListCount(TypeData listType, string ob) => listType.Type.IsArray ? "ob.Length" : "ob.Count";

    private void GenerateGetArrayType(
      ListMap map,
      string itemCount,
      out string localName,
      out string ns)
    {
      string str = !(itemCount != string.Empty) ? "[]" : string.Empty;
      XmlTypeMapElementInfo typeMapElementInfo = (XmlTypeMapElementInfo) map.ItemInfo[0];
      if (typeMapElementInfo.TypeData.SchemaType == SchemaTypes.Array)
      {
        string localName1;
        this.GenerateGetArrayType((ListMap) typeMapElementInfo.MappedType.ObjectMap, string.Empty, out localName1, out ns);
        localName = localName1 + str;
      }
      else if (typeMapElementInfo.MappedType != null)
      {
        localName = typeMapElementInfo.MappedType.XmlType + str;
        ns = typeMapElementInfo.MappedType.Namespace;
      }
      else
      {
        localName = typeMapElementInfo.TypeData.XmlType + str;
        ns = typeMapElementInfo.DataTypeNamespace;
      }
      if (!(itemCount != string.Empty))
        return;
      localName = "\"" + localName + "[\" + " + itemCount + " + \"]\"";
      ns = this.GetLiteral((object) ns);
    }

    private string GenerateWriteListContent(
      string container,
      TypeData listType,
      ListMap map,
      string ob,
      bool writeToString)
    {
      string targetString = (string) null;
      if (writeToString)
      {
        targetString = this.GetStrTempVar();
        this.WriteLine("System.Text.StringBuilder " + targetString + " = new System.Text.StringBuilder();");
      }
      if (listType.Type.IsArray)
      {
        string numTempVar = this.GetNumTempVar();
        this.WriteLineInd("for (int " + numTempVar + " = 0; " + numTempVar + " < " + ob + ".Length; " + numTempVar + "++) {");
        this.GenerateListLoop(container, map, ob + "[" + numTempVar + "]", numTempVar, listType.ListItemTypeData, targetString);
        this.WriteLineUni("}");
      }
      else if (typeof (ICollection).IsAssignableFrom(listType.Type))
      {
        string numTempVar = this.GetNumTempVar();
        this.WriteLineInd("for (int " + numTempVar + " = 0; " + numTempVar + " < " + ob + ".Count; " + numTempVar + "++) {");
        this.GenerateListLoop(container, map, ob + "[" + numTempVar + "]", numTempVar, listType.ListItemTypeData, targetString);
        this.WriteLineUni("}");
      }
      else
      {
        if (!typeof (IEnumerable).IsAssignableFrom(listType.Type))
          throw new Exception("Unsupported collection type");
        string obTempVar = this.GetObTempVar();
        this.WriteLineInd("foreach (" + listType.ListItemTypeData.CSharpFullName + " " + obTempVar + " in " + ob + ") {");
        this.GenerateListLoop(container, map, obTempVar, (string) null, listType.ListItemTypeData, targetString);
        this.WriteLineUni("}");
      }
      return targetString;
    }

    private void GenerateListLoop(
      string container,
      ListMap map,
      string item,
      string index,
      TypeData itemTypeData,
      string targetString)
    {
      bool flag = map.ItemInfo.Count > 1;
      if (map.ChoiceMember != null && container != null && index != null)
      {
        this.WriteLineInd("if ((" + container + ".@" + map.ChoiceMember + " == null) || (" + index + " >= " + container + ".@" + map.ChoiceMember + ".Length))");
        this.WriteLine("throw CreateInvalidChoiceIdentifierValueException (" + container + ".GetType().ToString(), \"" + map.ChoiceMember + "\");");
        this.Unindent();
      }
      if (flag)
        this.WriteLine("if (((object)" + item + ") == null) { }");
      foreach (XmlTypeMapElementInfo elem in (ArrayList) map.ItemInfo)
      {
        if (map.ChoiceMember != null && flag)
          this.WriteLineInd("else if (" + container + ".@" + map.ChoiceMember + "[" + index + "] == " + this.GetLiteral(elem.ChoiceValue) + ") {");
        else if (flag)
          this.WriteLineInd("else if (" + item + ".GetType() == typeof(" + elem.TypeData.CSharpFullName + ")) {");
        if (targetString == null)
        {
          this.GenerateWriteMemberElement(elem, this.GetCast(elem.TypeData, itemTypeData, item));
        }
        else
        {
          string getStringValue = this.GenerateGetStringValue(elem.MappedType, elem.TypeData, this.GetCast(elem.TypeData, itemTypeData, item), false);
          this.WriteLine(targetString + ".Append (" + getStringValue + ").Append (\" \");");
        }
        if (flag)
          this.WriteLineUni("}");
      }
      if (!flag)
        return;
      this.WriteLine("else throw CreateUnknownTypeException (" + item + ");");
    }

    private void GenerateWritePrimitiveValueLiteral(
      string memberValue,
      string name,
      string ns,
      XmlTypeMapping mappedType,
      TypeData typeData,
      bool wrapped,
      bool isNullable)
    {
      if (!wrapped)
        this.WriteMetCall("WriteValue", this.GenerateGetStringValue(mappedType, typeData, memberValue, false));
      else if (isNullable)
      {
        if (typeData.Type == typeof (XmlQualifiedName))
        {
          this.WriteMetCall("WriteNullableQualifiedNameLiteral", this.GetLiteral((object) name), this.GetLiteral((object) ns), memberValue);
        }
        else
        {
          string getStringValue = this.GenerateGetStringValue(mappedType, typeData, memberValue, true);
          this.WriteMetCall("WriteNullableStringLiteral", this.GetLiteral((object) name), this.GetLiteral((object) ns), getStringValue);
        }
      }
      else if (typeData.Type == typeof (XmlQualifiedName))
      {
        this.WriteMetCall("WriteElementQualifiedName", this.GetLiteral((object) name), this.GetLiteral((object) ns), memberValue);
      }
      else
      {
        string getStringValue = this.GenerateGetStringValue(mappedType, typeData, memberValue, false);
        this.WriteMetCall("WriteElementString", this.GetLiteral((object) name), this.GetLiteral((object) ns), getStringValue);
      }
    }

    private void GenerateWritePrimitiveValueEncoded(
      string memberValue,
      string name,
      string ns,
      XmlQualifiedName xsiType,
      XmlTypeMapping mappedType,
      TypeData typeData,
      bool wrapped,
      bool isNullable)
    {
      if (!wrapped)
        this.WriteMetCall("WriteValue", this.GenerateGetStringValue(mappedType, typeData, memberValue, false));
      else if (isNullable)
      {
        if (typeData.Type == typeof (XmlQualifiedName))
        {
          this.WriteMetCall("WriteNullableQualifiedNameEncoded", this.GetLiteral((object) name), this.GetLiteral((object) ns), memberValue, this.GetLiteral((object) xsiType));
        }
        else
        {
          string getStringValue = this.GenerateGetStringValue(mappedType, typeData, memberValue, true);
          this.WriteMetCall("WriteNullableStringEncoded", this.GetLiteral((object) name), this.GetLiteral((object) ns), getStringValue, this.GetLiteral((object) xsiType));
        }
      }
      else if (typeData.Type == typeof (XmlQualifiedName))
      {
        this.WriteMetCall("WriteElementQualifiedName", this.GetLiteral((object) name), this.GetLiteral((object) ns), memberValue, this.GetLiteral((object) xsiType));
      }
      else
      {
        string getStringValue = this.GenerateGetStringValue(mappedType, typeData, memberValue, false);
        this.WriteMetCall("WriteElementString", this.GetLiteral((object) name), this.GetLiteral((object) ns), getStringValue, this.GetLiteral((object) xsiType));
      }
    }

    private string GenerateGetMemberValue(XmlTypeMapMember member, string ob, bool isValueList)
    {
      if (!isValueList)
        return ob + ".@" + member.Name;
      return this.GetCast(member.TypeData, TypeTranslator.GetTypeData(typeof (object)), ob + "[" + (object) member.GlobalIndex + "]");
    }

    private string GenerateMemberHasValueCondition(
      XmlTypeMapMember member,
      string ob,
      bool isValueList)
    {
      if (isValueList)
        return ob + ".Length > " + (object) member.GlobalIndex;
      if (member.DefaultValue != DBNull.Value)
      {
        string str = ob + ".@" + member.Name;
        if (member.DefaultValue == null)
          return str + " != null";
        return member.TypeData.SchemaType == SchemaTypes.Enum ? str + " != " + this.GetCast(member.TypeData, this.GetLiteral(member.DefaultValue)) : str + " != " + this.GetLiteral(member.DefaultValue);
      }
      return member.IsOptionalValueType ? ob + ".@" + member.Name + "Specified" : (string) null;
    }

    private void GenerateWriteInitCallbacks()
    {
      this.WriteLine("protected override void InitCallbacks ()");
      this.WriteLineInd("{");
      if (this._format == SerializationFormat.Encoded)
      {
        foreach (XmlMapping xmlMapping in this._mapsToGenerate)
        {
          if (xmlMapping is XmlTypeMapping typeMap)
            this.WriteMetCall("AddWriteCallback", this.GetTypeOf(typeMap.TypeData), this.GetLiteral((object) typeMap.XmlType), this.GetLiteral((object) typeMap.Namespace), "new XmlSerializationWriteCallback (" + this.GetWriteObjectCallbackName(typeMap) + ")");
        }
      }
      this.WriteLineUni("}");
      this.WriteLine(string.Empty);
      if (this._format != SerializationFormat.Encoded)
        return;
      foreach (XmlTypeMapping map in this._mapsToGenerate)
      {
        if (map != null)
        {
          if (map.TypeData.SchemaType == SchemaTypes.Enum)
            this.WriteWriteEnumCallback(map);
          else
            this.WriteWriteObjectCallback(map);
        }
      }
    }

    private void WriteWriteEnumCallback(XmlTypeMapping map)
    {
      this.WriteLine("void " + this.GetWriteObjectCallbackName(map) + " (object ob)");
      this.WriteLineInd("{");
      this.WriteMetCall(this.GetWriteObjectName(map), this.GetCast(map.TypeData, "ob"), this.GetLiteral((object) map.ElementName), this.GetLiteral((object) map.Namespace), "false", "true", "false");
      this.WriteLineUni("}");
      this.WriteLine(string.Empty);
    }

    private void WriteWriteObjectCallback(XmlTypeMapping map)
    {
      this.WriteLine("void " + this.GetWriteObjectCallbackName(map) + " (object ob)");
      this.WriteLineInd("{");
      this.WriteMetCall(this.GetWriteObjectName(map), this.GetCast(map.TypeData, "ob"), this.GetLiteral((object) map.ElementName), this.GetLiteral((object) map.Namespace), "false", "false", "false");
      this.WriteLineUni("}");
      this.WriteLine(string.Empty);
    }

    public void GenerateReader(string readerClassName, ArrayList maps)
    {
      if (this._config == null || !this._config.GenerateAsInternal)
        this.WriteLine("public class " + readerClassName + " : XmlSerializationReader");
      else
        this.WriteLine("internal class " + readerClassName + " : XmlSerializationReader");
      this.WriteLineInd("{");
      this.WriteLine("static readonly System.Reflection.MethodInfo fromBinHexStringMethod = typeof (XmlConvert).GetMethod (\"FromBinHexString\", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic, null, new Type [] {typeof (string)}, null);");
      this.WriteLine("static byte [] FromBinHexString (string input)");
      this.WriteLineInd("{");
      this.WriteLine("return input == null ? null : (byte []) fromBinHexStringMethod.Invoke (null, new object [] {input});");
      this.WriteLineUni("}");
      this._mapsToGenerate = new ArrayList();
      this._fixupCallbacks = new ArrayList();
      this.InitHooks();
      for (int index = 0; index < maps.Count; ++index)
      {
        GenerationResult map = (GenerationResult) maps[index];
        this._typeMap = map.Mapping;
        this._format = this._typeMap.Format;
        this._result = map;
        this.GenerateReadRoot();
      }
      for (int index = 0; index < this._mapsToGenerate.Count; ++index)
      {
        if (this._mapsToGenerate[index] is XmlTypeMapping typeMap)
        {
          this.GenerateReadObject(typeMap);
          if (typeMap.TypeData.SchemaType == SchemaTypes.Enum)
            this.GenerateGetEnumValueMethod(typeMap);
        }
      }
      this.GenerateReadInitCallbacks();
      if (this._format == SerializationFormat.Encoded)
      {
        this.GenerateFixupCallbacks();
        this.GenerateFillerCallbacks();
      }
      this.WriteLineUni("}");
      this.UpdateGeneratedTypes(this._mapsToGenerate);
    }

    private void GenerateReadRoot()
    {
      this.WriteLine("public object " + this._result.ReadMethodName + " ()");
      this.WriteLineInd("{");
      this.WriteLine("Reader.MoveToContent();");
      if (this._typeMap is XmlTypeMapping)
      {
        XmlTypeMapping typeMap = (XmlTypeMapping) this._typeMap;
        if (this._format == SerializationFormat.Literal)
        {
          if (typeMap.TypeData.SchemaType == SchemaTypes.XmlNode)
          {
            if (typeMap.TypeData.Type == typeof (XmlDocument))
              this.WriteLine("return ReadXmlDocument (false);");
            else
              this.WriteLine("return ReadXmlNode (false);");
          }
          else
          {
            this.WriteLineInd("if (Reader.LocalName != " + this.GetLiteral((object) typeMap.ElementName) + " || Reader.NamespaceURI != " + this.GetLiteral((object) typeMap.Namespace) + ")");
            this.WriteLine("throw CreateUnknownNodeException();");
            this.Unindent();
            this.WriteLine("return " + this.GetReadObjectCall(typeMap, this.GetLiteral((object) typeMap.IsNullable), "true") + ";");
          }
        }
        else
        {
          this.WriteLine("object ob = null;");
          this.WriteLine("Reader.MoveToContent();");
          this.WriteLine("if (Reader.NodeType == System.Xml.XmlNodeType.Element) ");
          this.WriteLineInd("{");
          this.WriteLineInd("if (Reader.LocalName == " + this.GetLiteral((object) typeMap.ElementName) + " && Reader.NamespaceURI == " + this.GetLiteral((object) typeMap.Namespace) + ")");
          this.WriteLine("ob = ReadReferencedElement();");
          this.Unindent();
          this.WriteLineInd("else ");
          this.WriteLine("throw CreateUnknownNodeException();");
          this.Unindent();
          this.WriteLineUni("}");
          this.WriteLineInd("else ");
          this.WriteLine("UnknownNode(null);");
          this.Unindent();
          this.WriteLine(string.Empty);
          this.WriteLine("ReadReferencedElements();");
          this.WriteLine("return ob;");
          this.RegisterReferencingMap(typeMap);
        }
      }
      else
        this.WriteLine("return " + this.GenerateReadMessage((XmlMembersMapping) this._typeMap) + ";");
      this.WriteLineUni("}");
      this.WriteLine(string.Empty);
    }

    private string GenerateReadMessage(XmlMembersMapping typeMap)
    {
      this.WriteLine("object[] parameters = new object[" + (object) typeMap.Count + "];");
      this.WriteLine(string.Empty);
      if (typeMap.HasWrapperElement)
      {
        if (this._format == SerializationFormat.Encoded)
        {
          this.WriteLine("while (Reader.NodeType == System.Xml.XmlNodeType.Element)");
          this.WriteLineInd("{");
          this.WriteLine("string root = Reader.GetAttribute (\"root\", " + this.GetLiteral((object) "http://schemas.xmlsoap.org/soap/encoding/") + ");");
          this.WriteLine("if (root == null || System.Xml.XmlConvert.ToBoolean(root)) break;");
          this.WriteLine("ReadReferencedElement ();");
          this.WriteLine("Reader.MoveToContent ();");
          this.WriteLineUni("}");
          this.WriteLine(string.Empty);
          this.WriteLine("if (Reader.NodeType != System.Xml.XmlNodeType.EndElement)");
          this.WriteLineInd("{");
          this.WriteLineInd("if (Reader.IsEmptyElement) {");
          this.WriteLine("Reader.Skip();");
          this.WriteLine("Reader.MoveToContent();");
          this.WriteLineUni("}");
          this.WriteLineInd("else {");
          this.WriteLine("Reader.ReadStartElement();");
          this.GenerateReadMembers((XmlMapping) typeMap, (ClassMap) typeMap.ObjectMap, "parameters", true, false);
          this.WriteLine("ReadEndElement();");
          this.WriteLineUni("}");
          this.WriteLine(string.Empty);
          this.WriteLine("Reader.MoveToContent();");
          this.WriteLineUni("}");
        }
        else
        {
          ArrayList allMembers = ((ClassMap) typeMap.ObjectMap).AllMembers;
          for (int index = 0; index < allMembers.Count; ++index)
          {
            XmlTypeMapMember member = (XmlTypeMapMember) allMembers[index];
            if (!member.IsReturnValue && member.TypeData.IsValueType)
              this.GenerateSetMemberValueFromAttr(member, "parameters", string.Format("({0}) Activator.CreateInstance(typeof({0}), true)", (object) member.TypeData.FullTypeName), true);
          }
          this.WriteLine("while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.ReadState == ReadState.Interactive)");
          this.WriteLineInd("{");
          this.WriteLine("if (Reader.IsStartElement(" + this.GetLiteral((object) typeMap.ElementName) + ", " + this.GetLiteral((object) typeMap.Namespace) + "))");
          this.WriteLineInd("{");
          bool first = false;
          this.GenerateReadAttributeMembers((XmlMapping) typeMap, (ClassMap) typeMap.ObjectMap, "parameters", true, ref first);
          this.WriteLine("if (Reader.IsEmptyElement)");
          this.WriteLineInd("{");
          this.WriteLine("Reader.Skip(); Reader.MoveToContent(); continue;");
          this.WriteLineUni("}");
          this.WriteLine("Reader.ReadStartElement();");
          this.GenerateReadMembers((XmlMapping) typeMap, (ClassMap) typeMap.ObjectMap, "parameters", true, false);
          this.WriteLine("ReadEndElement();");
          this.WriteLine("break;");
          this.WriteLineUni("}");
          this.WriteLineInd("else ");
          this.WriteLine("UnknownNode(null);");
          this.Unindent();
          this.WriteLine(string.Empty);
          this.WriteLine("Reader.MoveToContent();");
          this.WriteLineUni("}");
        }
      }
      else
        this.GenerateReadMembers((XmlMapping) typeMap, (ClassMap) typeMap.ObjectMap, "parameters", true, this._format == SerializationFormat.Encoded);
      if (this._format == SerializationFormat.Encoded)
        this.WriteLine("ReadReferencedElements();");
      return "parameters";
    }

    private void GenerateReadObject(XmlTypeMapping typeMap)
    {
      string isNullable;
      if (this._format == SerializationFormat.Literal)
      {
        this.WriteLine("public " + typeMap.TypeData.CSharpFullName + " " + this.GetReadObjectName(typeMap) + " (bool isNullable, bool checkType)");
        isNullable = "isNullable";
      }
      else
      {
        this.WriteLine("public object " + this.GetReadObjectName(typeMap) + " ()");
        isNullable = "true";
      }
      this.WriteLineInd("{");
      this.PushHookContext();
      this.SetHookVar("$TYPE", typeMap.TypeData.CSharpName);
      this.SetHookVar("$FULLTYPE", typeMap.TypeData.CSharpFullName);
      this.SetHookVar("$NULLABLE", "isNullable");
      switch (typeMap.TypeData.SchemaType)
      {
        case SchemaTypes.Primitive:
          this.GenerateReadPrimitiveElement(typeMap, isNullable);
          break;
        case SchemaTypes.Enum:
          this.GenerateReadEnumElement(typeMap, isNullable);
          break;
        case SchemaTypes.Array:
          string readListElement = this.GenerateReadListElement(typeMap, (string) null, isNullable, true);
          if (readListElement != null)
          {
            this.WriteLine("return " + readListElement + ";");
            break;
          }
          break;
        case SchemaTypes.Class:
          this.GenerateReadClassInstance(typeMap, isNullable, "checkType");
          break;
        case SchemaTypes.XmlSerializable:
          this.GenerateReadXmlSerializableElement(typeMap, isNullable);
          break;
        case SchemaTypes.XmlNode:
          this.GenerateReadXmlNodeElement(typeMap, isNullable);
          break;
        default:
          throw new Exception("Unsupported map type");
      }
      this.WriteLineUni("}");
      this.WriteLine(string.Empty);
      this.PopHookContext();
    }

    private void GenerateReadClassInstance(
      XmlTypeMapping typeMap,
      string isNullable,
      string checkType)
    {
      this.SetHookVar("$OBJECT", "ob");
      if (!typeMap.TypeData.IsValueType)
      {
        this.WriteLine(typeMap.TypeData.CSharpFullName + " ob = null;");
        if (this.GenerateReadHook(HookType.type, typeMap.TypeData.Type))
        {
          this.WriteLine("return ob;");
          return;
        }
        if (this._format == SerializationFormat.Literal)
        {
          this.WriteLine("if (" + isNullable + " && ReadNull()) return null;");
          this.WriteLine(string.Empty);
          this.WriteLine("if (checkType) ");
          this.WriteLineInd("{");
        }
        else
        {
          this.WriteLine("if (ReadNull()) return null;");
          this.WriteLine(string.Empty);
        }
      }
      else
      {
        this.WriteLine(typeMap.TypeData.CSharpFullName + string.Format(" ob = ({0}) Activator.CreateInstance(typeof({0}), true);", (object) typeMap.TypeData.CSharpFullName));
        if (this.GenerateReadHook(HookType.type, typeMap.TypeData.Type))
        {
          this.WriteLine("return ob;");
          return;
        }
      }
      this.WriteLine("System.Xml.XmlQualifiedName t = GetXsiType();");
      this.WriteLine("if (t == null)");
      if (typeMap.TypeData.Type != typeof (object))
        this.WriteLine("{ }");
      else
        this.WriteLine("\treturn " + this.GetCast(typeMap.TypeData, "ReadTypedPrimitive (new System.Xml.XmlQualifiedName(\"anyType\", System.Xml.Schema.XmlSchema.Namespace))") + ";");
      foreach (XmlTypeMapping derivedType in typeMap.DerivedTypes)
      {
        this.WriteLineInd("else if (t.Name == " + this.GetLiteral((object) derivedType.XmlType) + " && t.Namespace == " + this.GetLiteral((object) derivedType.XmlTypeNamespace) + ")");
        this.WriteLine("return " + this.GetReadObjectCall(derivedType, isNullable, checkType) + ";");
        this.Unindent();
      }
      this.WriteLine("else if (t.Name != " + this.GetLiteral((object) typeMap.XmlType) + " || t.Namespace != " + this.GetLiteral((object) typeMap.XmlTypeNamespace) + ")");
      if (typeMap.TypeData.Type == typeof (object))
        this.WriteLine("\treturn " + this.GetCast(typeMap.TypeData, "ReadTypedPrimitive (t)") + ";");
      else
        this.WriteLine("\tthrow CreateUnknownTypeException(t);");
      if (!typeMap.TypeData.IsValueType)
      {
        if (this._format == SerializationFormat.Literal)
          this.WriteLineUni("}");
        if (typeMap.TypeData.Type.IsAbstract)
        {
          this.GenerateEndHook();
          this.WriteLine("return ob;");
          return;
        }
        this.WriteLine(string.Empty);
        this.WriteLine(string.Format("ob = ({0}) Activator.CreateInstance(typeof({0}), true);", (object) typeMap.TypeData.CSharpFullName));
      }
      this.WriteLine(string.Empty);
      this.WriteLine("Reader.MoveToElement();");
      this.WriteLine(string.Empty);
      this.GenerateReadMembers((XmlMapping) typeMap, (ClassMap) typeMap.ObjectMap, "ob", false, false);
      this.WriteLine(string.Empty);
      this.GenerateEndHook();
      this.WriteLine("return ob;");
    }

    private void GenerateReadMembers(
      XmlMapping xmlMap,
      ClassMap map,
      string ob,
      bool isValueList,
      bool readByOrder)
    {
      Type type1 = !(xmlMap is XmlTypeMapping xmlTypeMapping) ? typeof (object[]) : xmlTypeMapping.TypeData.Type;
      bool first = false;
      this.GenerateReadAttributeMembers(xmlMap, map, ob, isValueList, ref first);
      if (!isValueList)
      {
        this.WriteLine("Reader.MoveToElement();");
        this.WriteLineInd("if (Reader.IsEmptyElement) {");
        this.WriteLine("Reader.Skip ();");
        this.GenerateSetListMembersDefaults(xmlTypeMapping, map, ob, isValueList);
        this.WriteLine("return " + ob + ";");
        this.WriteLineUni("}");
        this.WriteLine(string.Empty);
        this.WriteLine("Reader.ReadStartElement();");
      }
      this.WriteLine("Reader.MoveToContent();");
      this.WriteLine(string.Empty);
      if (!this.GenerateReadHook(HookType.elements, type1))
      {
        string[] strArray1 = (string[]) null;
        if (map.ElementMembers != null && !readByOrder)
        {
          string str = string.Empty;
          strArray1 = new string[map.ElementMembers.Count];
          int index = 0;
          foreach (XmlTypeMapMember elementMember in (IEnumerable) map.ElementMembers)
          {
            if (!(elementMember is XmlTypeMapMemberElement) || !((XmlTypeMapMemberElement) elementMember).IsXmlTextCollector)
            {
              strArray1[index] = this.GetBoolTempVar();
              if (str.Length > 0)
                str += ", ";
              str = str + strArray1[index] + "=false";
            }
            ++index;
          }
          if (str.Length > 0)
            this.WriteLine("bool " + str + ";");
          this.WriteLine(string.Empty);
        }
        string[] strArray2 = (string[]) null;
        string[] strArray3 = (string[]) null;
        string[] strArray4 = (string[]) null;
        if (map.FlatLists != null)
        {
          strArray2 = new string[map.FlatLists.Count];
          strArray3 = new string[map.FlatLists.Count];
          string str = "int ";
          for (int index = 0; index < map.FlatLists.Count; ++index)
          {
            XmlTypeMapMemberElement flatList = (XmlTypeMapMemberElement) map.FlatLists[index];
            strArray2[index] = this.GetNumTempVar();
            if (index > 0)
              str += ", ";
            str = str + strArray2[index] + "=0";
            if (!this.MemberHasReadReplaceHook(type1, (XmlTypeMapMember) flatList))
            {
              strArray3[index] = this.GetObTempVar();
              this.WriteLine(flatList.TypeData.CSharpFullName + " " + strArray3[index] + ";");
              if (this.IsReadOnly(xmlTypeMapping, (XmlTypeMapMember) flatList, flatList.TypeData, isValueList))
              {
                string getMemberValue = this.GenerateGetMemberValue((XmlTypeMapMember) flatList, ob, isValueList);
                this.WriteLine(strArray3[index] + " = " + getMemberValue + ";");
              }
              else if (flatList.TypeData.Type.IsArray)
              {
                string initializeList = this.GenerateInitializeList(flatList.TypeData);
                this.WriteLine(strArray3[index] + " = " + initializeList + ";");
              }
              else
              {
                this.WriteLine(strArray3[index] + " = " + this.GenerateGetMemberValue((XmlTypeMapMember) flatList, ob, isValueList) + ";");
                this.WriteLineInd("if (((object)" + strArray3[index] + ") == null) {");
                this.WriteLine(strArray3[index] + " = " + this.GenerateInitializeList(flatList.TypeData) + ";");
                this.GenerateSetMemberValue((XmlTypeMapMember) flatList, ob, strArray3[index], isValueList);
                this.WriteLineUni("}");
              }
            }
            if (flatList.ChoiceMember != null)
            {
              if (strArray4 == null)
                strArray4 = new string[map.FlatLists.Count];
              strArray4[index] = this.GetObTempVar();
              string initializeList = this.GenerateInitializeList(flatList.ChoiceTypeData);
              this.WriteLine(flatList.ChoiceTypeData.CSharpFullName + " " + strArray4[index] + " = " + initializeList + ";");
            }
          }
          this.WriteLine(str + ";");
          this.WriteLine(string.Empty);
        }
        if (this._format == SerializationFormat.Encoded && map.ElementMembers != null)
        {
          this._fixupCallbacks.Add((object) xmlMap);
          this.WriteLine("Fixup fixup = new Fixup(" + ob + ", new XmlSerializationFixupCallback(" + this.GetFixupCallbackName(xmlMap) + "), " + (object) map.ElementMembers.Count + ");");
          this.WriteLine("AddFixup (fixup);");
          this.WriteLine(string.Empty);
        }
        ArrayList arrayList = (ArrayList) null;
        int count;
        if (readByOrder)
        {
          count = map.ElementMembers == null ? 0 : map.ElementMembers.Count;
        }
        else
        {
          arrayList = new ArrayList();
          arrayList.AddRange(map.AllElementInfos);
          count = arrayList.Count;
          this.WriteLine("while (Reader.NodeType != System.Xml.XmlNodeType.EndElement) ");
          this.WriteLineInd("{");
          this.WriteLine("if (Reader.NodeType == System.Xml.XmlNodeType.Element) ");
          this.WriteLineInd("{");
        }
        first = true;
        for (int index = 0; index < count; ++index)
        {
          XmlTypeMapElementInfo elem = !readByOrder ? (XmlTypeMapElementInfo) arrayList[index] : map.GetElement(index);
          if (!readByOrder)
          {
            if (!elem.IsTextElement && !elem.IsUnnamedAnyElement)
            {
              string str1 = (!first ? "else " : string.Empty) + "if (";
              if (!elem.Member.IsReturnValue || this._format != SerializationFormat.Encoded)
              {
                string str2 = str1 + "Reader.LocalName == " + this.GetLiteral((object) elem.ElementName);
                if (!map.IgnoreMemberNamespace)
                  str2 = str2 + " && Reader.NamespaceURI == " + this.GetLiteral((object) elem.Namespace);
                str1 = str2 + " && ";
              }
              this.WriteLineInd(str1 + "!" + strArray1[elem.Member.Index] + ") {");
            }
            else
              continue;
          }
          if (elem.Member.GetType() == typeof (XmlTypeMapMemberList))
          {
            if (this._format == SerializationFormat.Encoded && elem.MultiReferenceType)
            {
              string obTempVar = this.GetObTempVar();
              this.WriteLine("object " + obTempVar + " = ReadReferencingElement (out fixup.Ids[" + (object) elem.Member.Index + "]);");
              this.RegisterReferencingMap(elem.MappedType);
              this.WriteLineInd("if (fixup.Ids[" + (object) elem.Member.Index + "] == null) {");
              if (this.IsReadOnly(xmlTypeMapping, elem.Member, elem.TypeData, isValueList))
                this.WriteLine("throw CreateReadOnlyCollectionException (" + this.GetLiteral((object) elem.TypeData.CSharpFullName) + ");");
              else
                this.GenerateSetMemberValue(elem.Member, ob, this.GetCast(elem.Member.TypeData, obTempVar), isValueList);
              this.WriteLineUni("}");
              if (!elem.MappedType.TypeData.Type.IsArray)
              {
                this.WriteLineInd("else {");
                if (this.IsReadOnly(xmlTypeMapping, elem.Member, elem.TypeData, isValueList))
                {
                  this.WriteLine(obTempVar + " = " + this.GenerateGetMemberValue(elem.Member, ob, isValueList) + ";");
                }
                else
                {
                  this.WriteLine(obTempVar + " = " + this.GenerateCreateList(elem.MappedType.TypeData.Type) + ";");
                  this.GenerateSetMemberValue(elem.Member, ob, this.GetCast(elem.Member.TypeData, obTempVar), isValueList);
                }
                this.WriteLine("AddFixup (new CollectionFixup (" + obTempVar + ", new XmlSerializationCollectionFixupCallback (" + this.GetFillListName(elem.Member.TypeData) + "), fixup.Ids[" + (object) elem.Member.Index + "]));");
                this.WriteLine("fixup.Ids[" + (object) elem.Member.Index + "] = null;");
                this.WriteLineUni("}");
              }
            }
            else if (!this.GenerateReadMemberHook(type1, elem.Member))
            {
              if (this.IsReadOnly(xmlTypeMapping, elem.Member, elem.TypeData, isValueList))
                this.GenerateReadListElement(elem.MappedType, this.GenerateGetMemberValue(elem.Member, ob, isValueList), this.GetLiteral((object) elem.IsNullable), false);
              else if (elem.MappedType.TypeData.Type.IsArray)
              {
                if (elem.IsNullable)
                {
                  this.GenerateSetMemberValue(elem.Member, ob, this.GenerateReadListElement(elem.MappedType, (string) null, this.GetLiteral((object) elem.IsNullable), true), isValueList);
                }
                else
                {
                  string obTempVar = this.GetObTempVar();
                  this.WriteLine(elem.MappedType.TypeData.CSharpFullName + " " + obTempVar + " = " + this.GenerateReadListElement(elem.MappedType, (string) null, this.GetLiteral((object) elem.IsNullable), true) + ";");
                  this.WriteLineInd("if (((object)" + obTempVar + ") != null) {");
                  this.GenerateSetMemberValue(elem.Member, ob, obTempVar, isValueList);
                  this.WriteLineUni("}");
                }
              }
              else
              {
                string obTempVar = this.GetObTempVar();
                this.WriteLine(elem.MappedType.TypeData.CSharpFullName + " " + obTempVar + " = " + this.GenerateGetMemberValue(elem.Member, ob, isValueList) + ";");
                this.WriteLineInd("if (((object)" + obTempVar + ") == null) {");
                this.WriteLine(obTempVar + " = " + this.GenerateCreateList(elem.MappedType.TypeData.Type) + ";");
                this.GenerateSetMemberValue(elem.Member, ob, obTempVar, isValueList);
                this.WriteLineUni("}");
                this.GenerateReadListElement(elem.MappedType, obTempVar, this.GetLiteral((object) elem.IsNullable), true);
              }
              this.GenerateEndHook();
            }
            if (!readByOrder)
              this.WriteLine(strArray1[elem.Member.Index] + " = true;");
          }
          else if (elem.Member.GetType() == typeof (XmlTypeMapMemberFlatList))
          {
            XmlTypeMapMemberFlatList member = (XmlTypeMapMemberFlatList) elem.Member;
            if (!this.GenerateReadArrayMemberHook(type1, elem.Member, strArray2[member.FlatArrayIndex]))
            {
              this.GenerateAddListValue(member.TypeData, strArray3[member.FlatArrayIndex], strArray2[member.FlatArrayIndex], this.GenerateReadObjectElement(elem), !this.IsReadOnly(xmlTypeMapping, elem.Member, elem.TypeData, isValueList));
              if (member.ChoiceMember != null)
                this.GenerateAddListValue(member.ChoiceTypeData, strArray4[member.FlatArrayIndex], strArray2[member.FlatArrayIndex], this.GetLiteral(elem.ChoiceValue), true);
              this.GenerateEndHook();
            }
            this.WriteLine(strArray2[member.FlatArrayIndex] + "++;");
          }
          else if (elem.Member.GetType() == typeof (XmlTypeMapMemberAnyElement))
          {
            XmlTypeMapMemberAnyElement member = (XmlTypeMapMemberAnyElement) elem.Member;
            if (member.TypeData.IsListType)
            {
              if (!this.GenerateReadArrayMemberHook(type1, elem.Member, strArray2[member.FlatArrayIndex]))
              {
                this.GenerateAddListValue(member.TypeData, strArray3[member.FlatArrayIndex], strArray2[member.FlatArrayIndex], this.GetReadXmlNode(member.TypeData.ListItemTypeData, false), true);
                this.GenerateEndHook();
              }
              this.WriteLine(strArray2[member.FlatArrayIndex] + "++;");
            }
            else if (!this.GenerateReadMemberHook(type1, elem.Member))
            {
              this.GenerateSetMemberValue((XmlTypeMapMember) member, ob, this.GetReadXmlNode(member.TypeData, false), isValueList);
              this.GenerateEndHook();
            }
          }
          else
          {
            if (elem.Member.GetType() != typeof (XmlTypeMapMemberElement))
              throw new InvalidOperationException("Unknown member type");
            if (!readByOrder)
              this.WriteLine(strArray1[elem.Member.Index] + " = true;");
            if (this._format == SerializationFormat.Encoded)
            {
              string obTempVar = this.GetObTempVar();
              this.RegisterReferencingMap(elem.MappedType);
              if (elem.Member.TypeData.SchemaType != SchemaTypes.Primitive)
                this.WriteLine("object " + obTempVar + " = ReadReferencingElement (out fixup.Ids[" + (object) elem.Member.Index + "]);");
              else
                this.WriteLine("object " + obTempVar + " = ReadReferencingElement (" + this.GetLiteral((object) elem.Member.TypeData.XmlType) + ", " + this.GetLiteral((object) "http://www.w3.org/2001/XMLSchema") + ", out fixup.Ids[" + (object) elem.Member.Index + "]);");
              if (elem.MultiReferenceType)
                this.WriteLineInd("if (fixup.Ids[" + (object) elem.Member.Index + "] == null) {");
              else
                this.WriteLineInd("if (" + obTempVar + " != null) {");
              this.GenerateSetMemberValue(elem.Member, ob, this.GetCast(elem.Member.TypeData, obTempVar), isValueList);
              this.WriteLineUni("}");
            }
            else if (!this.GenerateReadMemberHook(type1, elem.Member))
            {
              if (elem.ChoiceValue != null)
              {
                XmlTypeMapMemberElement member = (XmlTypeMapMemberElement) elem.Member;
                this.WriteLine(ob + ".@" + member.ChoiceMember + " = " + this.GetLiteral(elem.ChoiceValue) + ";");
              }
              this.GenerateSetMemberValue(elem.Member, ob, this.GenerateReadObjectElement(elem), isValueList);
              this.GenerateEndHook();
            }
          }
          if (!readByOrder)
            this.WriteLineUni("}");
          else
            this.WriteLine("Reader.MoveToContent();");
          first = false;
        }
        if (!readByOrder)
        {
          if (!first)
            this.WriteLineInd("else {");
          if (map.DefaultAnyElementMember != null)
          {
            XmlTypeMapMemberAnyElement anyElementMember = map.DefaultAnyElementMember;
            if (anyElementMember.TypeData.IsListType)
            {
              if (!this.GenerateReadArrayMemberHook(type1, (XmlTypeMapMember) anyElementMember, strArray2[anyElementMember.FlatArrayIndex]))
              {
                this.GenerateAddListValue(anyElementMember.TypeData, strArray3[anyElementMember.FlatArrayIndex], strArray2[anyElementMember.FlatArrayIndex], this.GetReadXmlNode(anyElementMember.TypeData.ListItemTypeData, false), true);
                this.GenerateEndHook();
              }
              this.WriteLine(strArray2[anyElementMember.FlatArrayIndex] + "++;");
            }
            else if (!this.GenerateReadMemberHook(type1, (XmlTypeMapMember) anyElementMember))
            {
              this.GenerateSetMemberValue((XmlTypeMapMember) anyElementMember, ob, this.GetReadXmlNode(anyElementMember.TypeData, false), isValueList);
              this.GenerateEndHook();
            }
          }
          else if (!this.GenerateReadHook(HookType.unknownElement, type1))
          {
            this.WriteLine("UnknownNode (" + ob + ");");
            this.GenerateEndHook();
          }
          if (!first)
            this.WriteLineUni("}");
          this.WriteLineUni("}");
          if (map.XmlTextCollector != null)
          {
            this.WriteLine("else if (Reader.NodeType == System.Xml.XmlNodeType.Text || Reader.NodeType == System.Xml.XmlNodeType.CDATA)");
            this.WriteLineInd("{");
            if (map.XmlTextCollector is XmlTypeMapMemberExpandable)
            {
              XmlTypeMapMemberExpandable xmlTextCollector = (XmlTypeMapMemberExpandable) map.XmlTextCollector;
              TypeData type2 = xmlTextCollector is XmlTypeMapMemberFlatList mapMemberFlatList ? mapMemberFlatList.ListMap.FindTextElement().TypeData : xmlTextCollector.TypeData.ListItemTypeData;
              if (!this.GenerateReadArrayMemberHook(type1, map.XmlTextCollector, strArray2[xmlTextCollector.FlatArrayIndex]))
              {
                string str = type2.Type != typeof (string) ? this.GetReadXmlNode(type2, false) : "Reader.ReadString()";
                this.GenerateAddListValue(xmlTextCollector.TypeData, strArray3[xmlTextCollector.FlatArrayIndex], strArray2[xmlTextCollector.FlatArrayIndex], str, true);
                this.GenerateEndHook();
              }
              this.WriteLine(strArray2[xmlTextCollector.FlatArrayIndex] + "++;");
            }
            else if (!this.GenerateReadMemberHook(type1, map.XmlTextCollector))
            {
              XmlTypeMapMemberElement xmlTextCollector = (XmlTypeMapMemberElement) map.XmlTextCollector;
              XmlTypeMapElementInfo typeMapElementInfo = (XmlTypeMapElementInfo) xmlTextCollector.ElementInfo[0];
              if (typeMapElementInfo.TypeData.Type == typeof (string))
              {
                this.GenerateSetMemberValue((XmlTypeMapMember) xmlTextCollector, ob, "ReadString (" + this.GenerateGetMemberValue((XmlTypeMapMember) xmlTextCollector, ob, isValueList) + ")", isValueList);
              }
              else
              {
                this.WriteLineInd("{");
                string strTempVar = this.GetStrTempVar();
                this.WriteLine("string " + strTempVar + " = Reader.ReadString();");
                this.GenerateSetMemberValue((XmlTypeMapMember) xmlTextCollector, ob, this.GenerateGetValueFromXmlString(strTempVar, typeMapElementInfo.TypeData, typeMapElementInfo.MappedType, typeMapElementInfo.IsNullable), isValueList);
                this.WriteLineUni("}");
              }
              this.GenerateEndHook();
            }
            this.WriteLineUni("}");
          }
          this.WriteLine("else");
          this.WriteLine("\tUnknownNode(" + ob + ");");
          this.WriteLine(string.Empty);
          this.WriteLine("Reader.MoveToContent();");
          this.WriteLineUni("}");
        }
        else
          this.WriteLine("Reader.MoveToContent();");
        if (strArray3 != null)
        {
          this.WriteLine(string.Empty);
          foreach (XmlTypeMapMemberExpandable flatList in map.FlatLists)
          {
            if (!this.MemberHasReadReplaceHook(type1, (XmlTypeMapMember) flatList))
            {
              string str = strArray3[flatList.FlatArrayIndex];
              if (flatList.TypeData.Type.IsArray)
                this.WriteLine(str + " = (" + flatList.TypeData.CSharpFullName + ") ShrinkArray (" + str + ", " + strArray2[flatList.FlatArrayIndex] + ", " + this.GetTypeOf(flatList.TypeData.Type.GetElementType()) + ", true);");
              if (!this.IsReadOnly(xmlTypeMapping, (XmlTypeMapMember) flatList, flatList.TypeData, isValueList) && flatList.TypeData.Type.IsArray)
                this.GenerateSetMemberValue((XmlTypeMapMember) flatList, ob, str, isValueList);
            }
          }
        }
        if (strArray4 != null)
        {
          this.WriteLine(string.Empty);
          foreach (XmlTypeMapMemberExpandable flatList in map.FlatLists)
          {
            if (!this.MemberHasReadReplaceHook(type1, (XmlTypeMapMember) flatList) && flatList.ChoiceMember != null)
            {
              string str = strArray4[flatList.FlatArrayIndex];
              this.WriteLine(str + " = (" + flatList.ChoiceTypeData.CSharpFullName + ") ShrinkArray (" + str + ", " + strArray2[flatList.FlatArrayIndex] + ", " + this.GetTypeOf(flatList.ChoiceTypeData.Type.GetElementType()) + ", true);");
              this.WriteLine(ob + ".@" + flatList.ChoiceMember + " = " + str + ";");
            }
          }
        }
        this.GenerateSetListMembersDefaults(xmlTypeMapping, map, ob, isValueList);
        this.GenerateEndHook();
      }
      if (isValueList)
        return;
      this.WriteLine(string.Empty);
      this.WriteLine("ReadEndElement();");
    }

    private void GenerateReadAttributeMembers(
      XmlMapping xmlMap,
      ClassMap map,
      string ob,
      bool isValueList,
      ref bool first)
    {
      Type type = !(xmlMap is XmlTypeMapping xmlTypeMapping) ? typeof (object[]) : xmlTypeMapping.TypeData.Type;
      if (this.GenerateReadHook(HookType.attributes, type))
        return;
      XmlTypeMapMember anyAttributeMember = (XmlTypeMapMember) map.DefaultAnyAttributeMember;
      if (anyAttributeMember != null)
      {
        this.WriteLine("int anyAttributeIndex = 0;");
        this.WriteLine(anyAttributeMember.TypeData.CSharpFullName + " anyAttributeArray = null;");
      }
      this.WriteLine("while (Reader.MoveToNextAttribute())");
      this.WriteLineInd("{");
      first = true;
      if (map.AttributeMembers != null)
      {
        foreach (XmlTypeMapMemberAttribute attributeMember in (IEnumerable) map.AttributeMembers)
        {
          this.WriteLineInd((!first ? "else " : string.Empty) + "if (Reader.LocalName == " + this.GetLiteral((object) attributeMember.AttributeName) + " && Reader.NamespaceURI == " + this.GetLiteral((object) attributeMember.Namespace) + ") {");
          if (!this.GenerateReadMemberHook(type, (XmlTypeMapMember) attributeMember))
          {
            this.GenerateSetMemberValue((XmlTypeMapMember) attributeMember, ob, this.GenerateGetValueFromXmlString("Reader.Value", attributeMember.TypeData, attributeMember.MappedType, false), isValueList);
            this.GenerateEndHook();
          }
          this.WriteLineUni("}");
          first = false;
        }
      }
      this.WriteLineInd((!first ? "else " : string.Empty) + "if (IsXmlnsAttribute (Reader.Name)) {");
      if (map.NamespaceDeclarations != null && !this.GenerateReadMemberHook(type, (XmlTypeMapMember) map.NamespaceDeclarations))
      {
        string str = ob + ".@" + map.NamespaceDeclarations.Name;
        this.WriteLine("if (" + str + " == null) " + str + " = new XmlSerializerNamespaces ();");
        this.WriteLineInd("if (Reader.Prefix == \"xmlns\")");
        this.WriteLine(str + ".Add (Reader.LocalName, Reader.Value);");
        this.Unindent();
        this.WriteLineInd("else");
        this.WriteLine(str + ".Add (\"\", Reader.Value);");
        this.Unindent();
        this.GenerateEndHook();
      }
      this.WriteLineUni("}");
      this.WriteLineInd("else {");
      if (anyAttributeMember != null)
      {
        if (!this.GenerateReadArrayMemberHook(type, anyAttributeMember, "anyAttributeIndex"))
        {
          this.WriteLine("System.Xml.XmlAttribute attr = (System.Xml.XmlAttribute) Document.ReadNode(Reader);");
          if (typeof (XmlSchemaAnnotated).IsAssignableFrom(type))
            this.WriteLine("ParseWsdlArrayType (attr);");
          this.GenerateAddListValue(anyAttributeMember.TypeData, "anyAttributeArray", "anyAttributeIndex", this.GetCast(anyAttributeMember.TypeData.ListItemTypeData, "attr"), true);
          this.GenerateEndHook();
        }
        this.WriteLine("anyAttributeIndex++;");
      }
      else if (!this.GenerateReadHook(HookType.unknownAttribute, type))
      {
        this.WriteLine("UnknownNode (" + ob + ");");
        this.GenerateEndHook();
      }
      this.WriteLineUni("}");
      this.WriteLineUni("}");
      if (anyAttributeMember != null && !this.MemberHasReadReplaceHook(type, anyAttributeMember))
      {
        this.WriteLine(string.Empty);
        this.WriteLine("anyAttributeArray = (" + anyAttributeMember.TypeData.CSharpFullName + ") ShrinkArray (anyAttributeArray, anyAttributeIndex, " + this.GetTypeOf(anyAttributeMember.TypeData.Type.GetElementType()) + ", true);");
        this.GenerateSetMemberValue(anyAttributeMember, ob, "anyAttributeArray", isValueList);
      }
      this.WriteLine(string.Empty);
      this.WriteLine("Reader.MoveToElement ();");
      this.GenerateEndHook();
    }

    private void GenerateSetListMembersDefaults(
      XmlTypeMapping typeMap,
      ClassMap map,
      string ob,
      bool isValueList)
    {
      if (map.ListMembers == null)
        return;
      ArrayList listMembers = map.ListMembers;
      for (int index = 0; index < listMembers.Count; ++index)
      {
        XmlTypeMapMember member = (XmlTypeMapMember) listMembers[index];
        if (!this.IsReadOnly(typeMap, member, member.TypeData, isValueList))
        {
          this.WriteLineInd("if (" + this.GenerateGetMemberValue(member, ob, isValueList) + " == null) {");
          this.GenerateSetMemberValue(member, ob, this.GenerateInitializeList(member.TypeData), isValueList);
          this.WriteLineUni("}");
        }
      }
    }

    private bool IsReadOnly(
      XmlTypeMapping map,
      XmlTypeMapMember member,
      TypeData memType,
      bool isValueList)
    {
      return !isValueList && member.IsReadOnly(map.TypeData.Type) || !memType.HasPublicConstructor;
    }

    private void GenerateSetMemberValue(
      XmlTypeMapMember member,
      string ob,
      string value,
      bool isValueList)
    {
      if (isValueList)
      {
        this.WriteLine(ob + "[" + (object) member.GlobalIndex + "] = " + value + ";");
      }
      else
      {
        this.WriteLine(ob + ".@" + member.Name + " = " + value + ";");
        if (!member.IsOptionalValueType)
          return;
        this.WriteLine(ob + "." + member.Name + "Specified = true;");
      }
    }

    private void GenerateSetMemberValueFromAttr(
      XmlTypeMapMember member,
      string ob,
      string value,
      bool isValueList)
    {
      if (member.TypeData.Type.IsEnum)
        value = this.GetCast(member.TypeData.Type, value);
      this.GenerateSetMemberValue(member, ob, value, isValueList);
    }

    private string GenerateReadObjectElement(XmlTypeMapElementInfo elem)
    {
      switch (elem.TypeData.SchemaType)
      {
        case SchemaTypes.Primitive:
        case SchemaTypes.Enum:
          return this.GenerateReadPrimitiveValue(elem);
        case SchemaTypes.Array:
          return this.GenerateReadListElement(elem.MappedType, (string) null, this.GetLiteral((object) elem.IsNullable), true);
        case SchemaTypes.Class:
          return this.GetReadObjectCall(elem.MappedType, this.GetLiteral((object) elem.IsNullable), "true");
        case SchemaTypes.XmlSerializable:
          return this.GetCast(elem.TypeData, string.Format("({0}) ReadSerializable (({0}) Activator.CreateInstance(typeof({0}), true))", (object) elem.TypeData.CSharpFullName));
        case SchemaTypes.XmlNode:
          return this.GetReadXmlNode(elem.TypeData, true);
        default:
          throw new NotSupportedException("Invalid value type");
      }
    }

    private string GenerateReadPrimitiveValue(XmlTypeMapElementInfo elem)
    {
      if (elem.TypeData.Type == typeof (XmlQualifiedName))
        return elem.IsNullable ? "ReadNullableQualifiedName ()" : "ReadElementQualifiedName ()";
      if (elem.IsNullable)
      {
        string strTempVar = this.GetStrTempVar();
        this.WriteLine("string " + strTempVar + " = ReadNullableString ();");
        return this.GenerateGetValueFromXmlString(strTempVar, elem.TypeData, elem.MappedType, true);
      }
      string strTempVar1 = this.GetStrTempVar();
      this.WriteLine("string " + strTempVar1 + " = Reader.ReadElementString ();");
      return this.GenerateGetValueFromXmlString(strTempVar1, elem.TypeData, elem.MappedType, false);
    }

    private string GenerateGetValueFromXmlString(
      string value,
      TypeData typeData,
      XmlTypeMapping typeMap,
      bool isNullable)
    {
      if (typeData.SchemaType == SchemaTypes.Array)
        return this.GenerateReadListString(typeMap, value);
      if (typeData.SchemaType == SchemaTypes.Enum)
        return this.GenerateGetEnumValue(typeMap, value, isNullable);
      return typeData.Type == typeof (XmlQualifiedName) ? "ToXmlQualifiedName (" + value + ")" : XmlCustomFormatter.GenerateFromXmlString(typeData, value);
    }

    private string GenerateReadListElement(
      XmlTypeMapping typeMap,
      string list,
      string isNullable,
      bool canCreateInstance)
    {
      Type type = typeMap.TypeData.Type;
      ListMap objectMap = (ListMap) typeMap.ObjectMap;
      bool flag1 = typeMap.TypeData.Type.IsArray;
      if (canCreateInstance && typeMap.TypeData.HasPublicConstructor)
      {
        if (list == null)
        {
          list = this.GetObTempVar();
          this.WriteLine(typeMap.TypeData.CSharpFullName + " " + list + " = null;");
          if (flag1)
            this.WriteLineInd("if (!ReadNull()) {");
          this.WriteLine(list + " = " + this.GenerateCreateList(type) + ";");
        }
        else if (flag1)
          this.WriteLineInd("if (!ReadNull()) {");
      }
      else if (list != null)
      {
        this.WriteLineInd("if (((object)" + list + ") == null)");
        this.WriteLine("throw CreateReadOnlyCollectionException (" + this.GetLiteral((object) typeMap.TypeData.CSharpFullName) + ");");
        this.Unindent();
        flag1 = false;
      }
      else
      {
        this.WriteLine("throw CreateReadOnlyCollectionException (" + this.GetLiteral((object) typeMap.TypeData.CSharpFullName) + ");");
        return list;
      }
      this.WriteLineInd("if (Reader.IsEmptyElement) {");
      this.WriteLine("Reader.Skip();");
      if (type.IsArray)
        this.WriteLine(list + " = (" + typeMap.TypeData.CSharpFullName + ") ShrinkArray (" + list + ", 0, " + this.GetTypeOf(type.GetElementType()) + ", false);");
      this.Unindent();
      this.WriteLineInd("} else {");
      string numTempVar = this.GetNumTempVar();
      this.WriteLine("int " + numTempVar + " = 0;");
      this.WriteLine("Reader.ReadStartElement();");
      this.WriteLine("Reader.MoveToContent();");
      this.WriteLine(string.Empty);
      this.WriteLine("while (Reader.NodeType != System.Xml.XmlNodeType.EndElement) ");
      this.WriteLineInd("{");
      this.WriteLine("if (Reader.NodeType == System.Xml.XmlNodeType.Element) ");
      this.WriteLineInd("{");
      bool flag2 = true;
      foreach (XmlTypeMapElementInfo elem in (ArrayList) objectMap.ItemInfo)
      {
        this.WriteLineInd((!flag2 ? "else " : string.Empty) + "if (Reader.LocalName == " + this.GetLiteral((object) elem.ElementName) + " && Reader.NamespaceURI == " + this.GetLiteral((object) elem.Namespace) + ") {");
        this.GenerateAddListValue(typeMap.TypeData, list, numTempVar, this.GenerateReadObjectElement(elem), false);
        this.WriteLine(numTempVar + "++;");
        this.WriteLineUni("}");
        flag2 = false;
      }
      if (!flag2)
        this.WriteLine("else UnknownNode (null);");
      else
        this.WriteLine("UnknownNode (null);");
      this.WriteLineUni("}");
      this.WriteLine("else UnknownNode (null);");
      this.WriteLine(string.Empty);
      this.WriteLine("Reader.MoveToContent();");
      this.WriteLineUni("}");
      this.WriteLine("ReadEndElement();");
      if (type.IsArray)
        this.WriteLine(list + " = (" + typeMap.TypeData.CSharpFullName + ") ShrinkArray (" + list + ", " + numTempVar + ", " + this.GetTypeOf(type.GetElementType()) + ", false);");
      this.WriteLineUni("}");
      if (flag1)
        this.WriteLineUni("}");
      return list;
    }

    private string GenerateReadListString(XmlTypeMapping typeMap, string values)
    {
      Type type = typeMap.TypeData.Type;
      ListMap objectMap = (ListMap) typeMap.ObjectMap;
      string csharpFullName = SerializationCodeGenerator.ToCSharpFullName(type.GetElementType());
      string obTempVar1 = this.GetObTempVar();
      this.WriteLine(csharpFullName + "[] " + obTempVar1 + ";");
      string strTempVar = this.GetStrTempVar();
      this.WriteLine("string " + strTempVar + " = " + values + ".Trim();");
      this.WriteLineInd("if (" + strTempVar + " != string.Empty) {");
      string obTempVar2 = this.GetObTempVar();
      this.WriteLine("string[] " + obTempVar2 + " = " + strTempVar + ".Split (' ');");
      this.WriteLine(obTempVar1 + " = new " + this.GetArrayDeclaration(type, obTempVar2 + ".Length") + ";");
      XmlTypeMapElementInfo typeMapElementInfo = (XmlTypeMapElementInfo) objectMap.ItemInfo[0];
      string numTempVar = this.GetNumTempVar();
      this.WriteLineInd("for (int " + numTempVar + " = 0; " + numTempVar + " < " + obTempVar2 + ".Length; " + numTempVar + "++)");
      this.WriteLine(obTempVar1 + "[" + numTempVar + "] = " + this.GenerateGetValueFromXmlString(obTempVar2 + "[" + numTempVar + "]", typeMapElementInfo.TypeData, typeMapElementInfo.MappedType, typeMapElementInfo.IsNullable) + ";");
      this.Unindent();
      this.WriteLineUni("}");
      this.WriteLine("else");
      this.WriteLine("\t" + obTempVar1 + " = new " + this.GetArrayDeclaration(type, "0") + ";");
      return obTempVar1;
    }

    private string GetArrayDeclaration(Type type, string length)
    {
      Type elementType = type.GetElementType();
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append('[').Append(length).Append(']');
      for (; elementType.IsArray; elementType = elementType.GetElementType())
        stringBuilder.Append("[]");
      stringBuilder.Insert(0, SerializationCodeGenerator.ToCSharpFullName(elementType));
      return stringBuilder.ToString();
    }

    private void GenerateAddListValue(
      TypeData listType,
      string list,
      string index,
      string value,
      bool canCreateInstance)
    {
      Type type = listType.Type;
      if (type.IsArray)
      {
        this.WriteLine(list + " = (" + SerializationCodeGenerator.ToCSharpFullName(type) + ") EnsureArrayIndex (" + list + ", " + index + ", " + this.GetTypeOf(type.GetElementType()) + ");");
        this.WriteLine(list + "[" + index + "] = " + value + ";");
      }
      else
      {
        this.WriteLine("if (((object)" + list + ") == null)");
        if (canCreateInstance)
          this.WriteLine("\t" + list + string.Format(" = ({0}) Activator.CreateInstance(typeof({0}), true);", (object) listType.CSharpFullName));
        else
          this.WriteLine("\tthrow CreateReadOnlyCollectionException (" + this.GetLiteral((object) listType.CSharpFullName) + ");");
        this.WriteLine(list + ".Add (" + value + ");");
      }
    }

    private string GenerateCreateList(Type listType)
    {
      if (!listType.IsArray)
        return "new " + SerializationCodeGenerator.ToCSharpFullName(listType) + "()";
      return "(" + SerializationCodeGenerator.ToCSharpFullName(listType) + ") EnsureArrayIndex (null, 0, " + this.GetTypeOf(listType.GetElementType()) + ")";
    }

    private string GenerateInitializeList(TypeData listType) => listType.Type.IsArray ? "null" : "new " + listType.CSharpFullName + "()";

    private void GenerateFillerCallbacks()
    {
      foreach (TypeData td in this._listsToFill)
      {
        this.WriteLine("void " + this.GetFillListName(td) + " (object list, object source)");
        this.WriteLineInd("{");
        this.WriteLine("if (((object)list) == null) throw CreateReadOnlyCollectionException (" + this.GetLiteral((object) td.CSharpFullName) + ");");
        this.WriteLine(string.Empty);
        this.WriteLine(td.CSharpFullName + " dest = (" + td.CSharpFullName + ") list;");
        this.WriteLine("foreach (object ob in (IEnumerable)source)");
        this.WriteLine("\tdest.Add (" + this.GetCast(td.ListItemTypeData, "ob") + ");");
        this.WriteLineUni("}");
        this.WriteLine(string.Empty);
      }
    }

    private void GenerateReadXmlNodeElement(XmlTypeMapping typeMap, string isNullable) => this.WriteLine("return " + this.GetReadXmlNode(typeMap.TypeData, false) + ";");

    private void GenerateReadPrimitiveElement(XmlTypeMapping typeMap, string isNullable)
    {
      this.WriteLine("XmlQualifiedName t = GetXsiType();");
      this.WriteLine("if (t == null) t = new XmlQualifiedName (" + this.GetLiteral((object) typeMap.XmlType) + ", " + this.GetLiteral((object) typeMap.Namespace) + ");");
      this.WriteLine("return " + this.GetCast(typeMap.TypeData, "ReadTypedPrimitive (t)") + ";");
    }

    private void GenerateReadEnumElement(XmlTypeMapping typeMap, string isNullable)
    {
      this.WriteLine("Reader.ReadStartElement ();");
      this.WriteLine(typeMap.TypeData.CSharpFullName + " res = " + this.GenerateGetEnumValue(typeMap, "Reader.ReadString()", false) + ";");
      this.WriteLineInd("if (Reader.NodeType != XmlNodeType.None)");
      this.WriteLineUni("Reader.ReadEndElement ();");
      this.WriteLine("return res;");
    }

    private string GenerateGetEnumValue(XmlTypeMapping typeMap, string val, bool isNullable)
    {
      if (!isNullable)
        return this.GetGetEnumValueName(typeMap) + " (" + val + ")";
      return "(" + val + ") != null ? " + this.GetGetEnumValueName(typeMap) + " (" + val + ") : (" + typeMap.TypeData.CSharpFullName + "?) null";
    }

    private void GenerateGetEnumValueMethod(XmlTypeMapping typeMap)
    {
      string str1 = this.GetGetEnumValueName(typeMap);
      if (((EnumMap) typeMap.ObjectMap).IsFlags)
      {
        string str2 = str1 + "_Switch";
        this.WriteLine(typeMap.TypeData.CSharpFullName + " " + str1 + " (string xmlName)");
        this.WriteLineInd("{");
        this.WriteLine("xmlName = xmlName.Trim();");
        this.WriteLine("if (xmlName.Length == 0) return (" + typeMap.TypeData.CSharpFullName + ")0;");
        this.WriteLine(typeMap.TypeData.CSharpFullName + " sb = (" + typeMap.TypeData.CSharpFullName + ")0;");
        this.WriteLine("string[] enumNames = xmlName.Split (null);");
        this.WriteLine("foreach (string name in enumNames)");
        this.WriteLineInd("{");
        this.WriteLine("if (name == string.Empty) continue;");
        this.WriteLine("sb |= " + str2 + " (name); ");
        this.WriteLineUni("}");
        this.WriteLine("return sb;");
        this.WriteLineUni("}");
        this.WriteLine(string.Empty);
        str1 = str2;
      }
      this.WriteLine(typeMap.TypeData.CSharpFullName + " " + str1 + " (string xmlName)");
      this.WriteLineInd("{");
      this.GenerateGetSingleEnumValue(typeMap, "xmlName");
      this.WriteLineUni("}");
      this.WriteLine(string.Empty);
    }

    private void GenerateGetSingleEnumValue(XmlTypeMapping typeMap, string val)
    {
      EnumMap objectMap = (EnumMap) typeMap.ObjectMap;
      this.WriteLine("switch (" + val + ")");
      this.WriteLineInd("{");
      foreach (EnumMap.EnumMapMember member in objectMap.Members)
        this.WriteLine("case " + this.GetLiteral((object) member.XmlName) + ": return " + typeMap.TypeData.CSharpFullName + ".@" + member.EnumName + ";");
      this.WriteLineInd("default:");
      this.WriteLine("throw CreateUnknownConstantException (" + val + ", typeof(" + typeMap.TypeData.CSharpFullName + "));");
      this.Unindent();
      this.WriteLineUni("}");
    }

    private void GenerateReadXmlSerializableElement(XmlTypeMapping typeMap, string isNullable)
    {
      this.WriteLine("Reader.MoveToContent ();");
      this.WriteLine("if (Reader.NodeType == XmlNodeType.Element)");
      this.WriteLineInd("{");
      this.WriteLine("if (Reader.LocalName == " + this.GetLiteral((object) typeMap.ElementName) + " && Reader.NamespaceURI == " + this.GetLiteral((object) typeMap.Namespace) + ")");
      this.WriteLine(string.Format("\treturn ({0}) ReadSerializable (({0}) Activator.CreateInstance(typeof({0}), true));", (object) typeMap.TypeData.CSharpFullName));
      this.WriteLine("else");
      this.WriteLine("\tthrow CreateUnknownNodeException ();");
      this.WriteLineUni("}");
      this.WriteLine("else UnknownNode (null);");
      this.WriteLine(string.Empty);
      this.WriteLine("return null;");
    }

    private void GenerateReadInitCallbacks()
    {
      this.WriteLine("protected override void InitCallbacks ()");
      this.WriteLineInd("{");
      if (this._format == SerializationFormat.Encoded)
      {
        foreach (XmlMapping xmlMapping in this._mapsToGenerate)
        {
          if (xmlMapping is XmlTypeMapping typeMap && (typeMap.TypeData.SchemaType == SchemaTypes.Class || typeMap.TypeData.SchemaType == SchemaTypes.Enum))
            this.WriteMetCall("AddReadCallback", this.GetLiteral((object) typeMap.XmlType), this.GetLiteral((object) typeMap.Namespace), this.GetTypeOf(typeMap.TypeData.Type), "new XmlSerializationReadCallback (" + this.GetReadObjectName(typeMap) + ")");
        }
      }
      this.WriteLineUni("}");
      this.WriteLine(string.Empty);
      this.WriteLine("protected override void InitIDs ()");
      this.WriteLine("{");
      this.WriteLine("}");
      this.WriteLine(string.Empty);
    }

    private void GenerateFixupCallbacks()
    {
      foreach (XmlMapping fixupCallback in this._fixupCallbacks)
      {
        bool isValueList = fixupCallback is XmlMembersMapping;
        string str = isValueList ? "object[]" : ((XmlTypeMapping) fixupCallback).TypeData.CSharpFullName;
        this.WriteLine("void " + this.GetFixupCallbackName(fixupCallback) + " (object obfixup)");
        this.WriteLineInd("{");
        this.WriteLine("Fixup fixup = (Fixup)obfixup;");
        this.WriteLine(str + " source = (" + str + ") fixup.Source;");
        this.WriteLine("string[] ids = fixup.Ids;");
        this.WriteLine(string.Empty);
        ICollection elementMembers = ((ClassMap) fixupCallback.ObjectMap).ElementMembers;
        if (elementMembers != null)
        {
          foreach (XmlTypeMapMember member in (IEnumerable) elementMembers)
          {
            this.WriteLineInd("if (ids[" + (object) member.Index + "] != null)");
            string val = "GetTarget(ids[" + (object) member.Index + "])";
            if (!isValueList)
              val = this.GetCast(member.TypeData, val);
            this.GenerateSetMemberValue(member, "source", val, isValueList);
            this.Unindent();
          }
        }
        this.WriteLineUni("}");
        this.WriteLine(string.Empty);
      }
    }

    private string GetReadXmlNode(TypeData type, bool wrapped) => type.Type == typeof (XmlDocument) ? this.GetCast(type, TypeTranslator.GetTypeData(typeof (XmlDocument)), "ReadXmlDocument (" + this.GetLiteral((object) wrapped) + ")") : this.GetCast(type, TypeTranslator.GetTypeData(typeof (XmlNode)), "ReadXmlNode (" + this.GetLiteral((object) wrapped) + ")");

    private void InitHooks()
    {
      this._hookContexts = new Stack();
      this._hookOpenHooks = new Stack();
      this._hookVariables = new Hashtable();
    }

    private void PushHookContext()
    {
      this._hookContexts.Push((object) this._hookVariables);
      this._hookVariables = (Hashtable) this._hookVariables.Clone();
    }

    private void PopHookContext() => this._hookVariables = (Hashtable) this._hookContexts.Pop();

    private void SetHookVar(string var, string value) => this._hookVariables[(object) var] = (object) value;

    private bool GenerateReadHook(HookType hookType, Type type) => this.GenerateHook(hookType, XmlMappingAccess.Read, type, (string) null);

    private bool GenerateWriteHook(HookType hookType, Type type) => this.GenerateHook(hookType, XmlMappingAccess.Write, type, (string) null);

    private bool GenerateWriteMemberHook(Type type, XmlTypeMapMember member)
    {
      this.SetHookVar("$MEMBER", member.Name);
      return this.GenerateHook(HookType.member, XmlMappingAccess.Write, type, member.Name);
    }

    private bool GenerateReadMemberHook(Type type, XmlTypeMapMember member)
    {
      this.SetHookVar("$MEMBER", member.Name);
      return this.GenerateHook(HookType.member, XmlMappingAccess.Read, type, member.Name);
    }

    private bool GenerateReadArrayMemberHook(Type type, XmlTypeMapMember member, string index)
    {
      this.SetHookVar("$INDEX", index);
      return this.GenerateReadMemberHook(type, member);
    }

    private bool MemberHasReadReplaceHook(Type type, XmlTypeMapMember member) => this._config != null && this._config.GetHooks(HookType.member, XmlMappingAccess.Read, HookAction.Replace, type, member.Name).Count > 0;

    private bool GenerateHook(HookType hookType, XmlMappingAccess dir, Type type, string member)
    {
      this.GenerateHooks(hookType, dir, type, (string) null, HookAction.InsertBefore);
      if (this.GenerateHooks(hookType, dir, type, (string) null, HookAction.Replace))
      {
        this.GenerateHooks(hookType, dir, type, (string) null, HookAction.InsertAfter);
        return true;
      }
      this._hookOpenHooks.Push((object) new SerializationCodeGenerator.HookInfo()
      {
        HookType = hookType,
        Type = type,
        Member = member,
        Direction = dir
      });
      return false;
    }

    private void GenerateEndHook()
    {
      SerializationCodeGenerator.HookInfo hookInfo = (SerializationCodeGenerator.HookInfo) this._hookOpenHooks.Pop();
      this.GenerateHooks(hookInfo.HookType, hookInfo.Direction, hookInfo.Type, hookInfo.Member, HookAction.InsertAfter);
    }

    private bool GenerateHooks(
      HookType hookType,
      XmlMappingAccess dir,
      Type type,
      string member,
      HookAction action)
    {
      if (this._config == null)
        return false;
      ArrayList hooks = this._config.GetHooks(hookType, dir, action, type, (string) null);
      if (hooks.Count == 0)
        return false;
      foreach (Hook hook in hooks)
      {
        string code = hook.GetCode(action);
        foreach (DictionaryEntry hookVariable in this._hookVariables)
          code = code.Replace((string) hookVariable.Key, (string) hookVariable.Value);
        this.WriteMultilineCode(code);
      }
      return true;
    }

    private string GetRootTypeName() => this._typeMap is XmlTypeMapping ? ((XmlTypeMapping) this._typeMap).TypeData.CSharpFullName : "object[]";

    private string GetNumTempVar() => "n" + (object) this._tempVarId++;

    private string GetObTempVar() => "o" + (object) this._tempVarId++;

    private string GetStrTempVar() => "s" + (object) this._tempVarId++;

    private string GetBoolTempVar() => "b" + (object) this._tempVarId++;

    private string GetUniqueName(string uniqueGroup, object ob, string name)
    {
      name = name.Replace("[]", "_array");
      Hashtable hashtable = (Hashtable) this._uniqueNames[(object) uniqueGroup];
      if (hashtable == null)
      {
        hashtable = new Hashtable();
        this._uniqueNames[(object) uniqueGroup] = (object) hashtable;
      }
      string uniqueName = (string) hashtable[ob];
      if (uniqueName != null)
        return uniqueName;
      foreach (string str in (IEnumerable) hashtable.Values)
      {
        if (str == name)
          return this.GetUniqueName(uniqueGroup, ob, name + (object) this._methodId++);
      }
      hashtable[ob] = (object) name;
      return name;
    }

    private void RegisterReferencingMap(XmlTypeMapping typeMap)
    {
      if (typeMap == null || this._mapsToGenerate.Contains((object) typeMap))
        return;
      this._mapsToGenerate.Add((object) typeMap);
    }

    private string GetWriteObjectName(XmlTypeMapping typeMap)
    {
      if (!this._mapsToGenerate.Contains((object) typeMap))
        this._mapsToGenerate.Add((object) typeMap);
      return this.GetUniqueName("rw", (object) typeMap, "WriteObject_" + typeMap.XmlType);
    }

    private string GetReadObjectName(XmlTypeMapping typeMap)
    {
      if (!this._mapsToGenerate.Contains((object) typeMap))
        this._mapsToGenerate.Add((object) typeMap);
      return this.GetUniqueName("rr", (object) typeMap, "ReadObject_" + typeMap.XmlType);
    }

    private string GetGetEnumValueName(XmlTypeMapping typeMap)
    {
      if (!this._mapsToGenerate.Contains((object) typeMap))
        this._mapsToGenerate.Add((object) typeMap);
      return this.GetUniqueName("ge", (object) typeMap, "GetEnumValue_" + typeMap.XmlType);
    }

    private string GetWriteObjectCallbackName(XmlTypeMapping typeMap)
    {
      if (!this._mapsToGenerate.Contains((object) typeMap))
        this._mapsToGenerate.Add((object) typeMap);
      return this.GetUniqueName("wc", (object) typeMap, "WriteCallback_" + typeMap.XmlType);
    }

    private string GetFixupCallbackName(XmlMapping typeMap)
    {
      if (!this._mapsToGenerate.Contains((object) typeMap))
        this._mapsToGenerate.Add((object) typeMap);
      return typeMap is XmlTypeMapping ? this.GetUniqueName("fc", (object) typeMap, "FixupCallback_" + ((XmlTypeMapping) typeMap).XmlType) : this.GetUniqueName("fc", (object) typeMap, "FixupCallback__Message");
    }

    private string GetUniqueClassName(string s) => this.classNames.AddUnique(s, (object) null);

    private string GetReadObjectCall(XmlTypeMapping typeMap, string isNullable, string checkType)
    {
      if (this._format != SerializationFormat.Literal)
        return this.GetCast(typeMap.TypeData, this.GetReadObjectName(typeMap) + " ()");
      return this.GetReadObjectName(typeMap) + " (" + isNullable + ", " + checkType + ")";
    }

    private string GetFillListName(TypeData td)
    {
      if (!this._listsToFill.Contains((object) td))
        this._listsToFill.Add((object) td);
      return this.GetUniqueName("fl", (object) td, "Fill_" + CodeIdentifier.MakeValid(td.CSharpName));
    }

    private string GetCast(TypeData td, TypeData tdval, string val) => td.CSharpFullName == tdval.CSharpFullName ? val : this.GetCast(td, val);

    private string GetCast(TypeData td, string val) => "((" + td.CSharpFullName + ") " + val + ")";

    private string GetCast(Type td, string val) => "((" + SerializationCodeGenerator.ToCSharpFullName(td) + ") " + val + ")";

    private string GetTypeOf(TypeData td) => "typeof(" + td.CSharpFullName + ")";

    private string GetTypeOf(Type td) => "typeof(" + SerializationCodeGenerator.ToCSharpFullName(td) + ")";

    private string GetLiteral(object ob)
    {
      switch (ob)
      {
        case null:
          return "null";
        case string _:
          return "\"" + ob.ToString().Replace("\"", "\"\"") + "\"";
        case DateTime dateTime:
          return "new DateTime (" + (object) dateTime.Ticks + ")";
        case DateTimeOffset dateTimeOffset:
          return "new DateTimeOffset (" + (object) dateTimeOffset.Ticks + ")";
        case TimeSpan timeSpan:
          return "new TimeSpan (" + (object) timeSpan.Ticks + ")";
        case bool flag:
          return flag ? "true" : "false";
        default:
          if ((object) (ob as XmlQualifiedName) != null)
          {
            XmlQualifiedName xmlQualifiedName = (XmlQualifiedName) ob;
            return "new XmlQualifiedName (" + this.GetLiteral((object) xmlQualifiedName.Name) + "," + this.GetLiteral((object) xmlQualifiedName.Namespace) + ")";
          }
          switch (ob)
          {
            case Enum _:
              string csharpFullName = SerializationCodeGenerator.ToCSharpFullName(ob.GetType());
              StringBuilder stringBuilder = new StringBuilder();
              string str1 = Enum.Format(ob.GetType(), ob, "g");
              char[] chArray = new char[1]{ ',' };
              foreach (string str2 in str1.Split(chArray))
              {
                string str3 = str2.Trim();
                if (str3.Length != 0)
                {
                  if (stringBuilder.Length > 0)
                    stringBuilder.Append(" | ");
                  stringBuilder.Append(csharpFullName);
                  stringBuilder.Append('.');
                  stringBuilder.Append(str3);
                }
              }
              return stringBuilder.ToString();
            case IFormattable _:
              return ((IFormattable) ob).ToString((string) null, (IFormatProvider) CultureInfo.InvariantCulture);
            default:
              return ob.ToString();
          }
      }
    }

    private void WriteLineInd(string code)
    {
      this.WriteLine(code);
      ++this._indent;
    }

    private void WriteLineUni(string code)
    {
      if (this._indent > 0)
        --this._indent;
      this.WriteLine(code);
    }

    private void Write(string code)
    {
      if (code.Length > 0)
        this._writer.Write(new string('\t', this._indent));
      this._writer.Write(code);
    }

    private void WriteUni(string code)
    {
      if (this._indent > 0)
        --this._indent;
      this._writer.Write(code);
      this._writer.WriteLine(string.Empty);
    }

    private void WriteLine(string code)
    {
      if (code.Length > 0)
        this._writer.Write(new string('\t', this._indent));
      this._writer.WriteLine(code);
    }

    private void WriteMultilineCode(string code)
    {
      string str = new string('\t', this._indent);
      code = code.Replace("\r", string.Empty);
      code = code.Replace("\t", string.Empty);
      while (code.StartsWith("\n"))
        code = code.Substring(1);
      while (code.EndsWith("\n"))
        code = code.Substring(0, code.Length - 1);
      code = code.Replace("\n", "\n" + str);
      this.WriteLine(code);
    }

    private string Params(params string[] pars)
    {
      string empty = string.Empty;
      foreach (string par in pars)
      {
        if (empty != string.Empty)
          empty += ", ";
        empty += par;
      }
      return empty;
    }

    private void WriteMetCall(string method, params string[] pars) => this.WriteLine(method + " (" + this.Params(pars) + ");");

    private void Unindent() => --this._indent;

    private class HookInfo
    {
      public HookType HookType;
      public Type Type;
      public string Member;
      public XmlMappingAccess Direction;
    }
  }
}
