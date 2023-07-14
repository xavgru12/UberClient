// Decompiled with JetBrains decompiler
// Type: Mono.Xml.DTDValidatingReader
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Schema;

namespace Mono.Xml
{
  internal class DTDValidatingReader : 
    XmlReader,
    IHasXmlParserContext,
    IHasXmlSchemaInfo,
    IXmlLineInfo,
    IXmlNamespaceResolver
  {
    private EntityResolvingXmlReader reader;
    private System.Xml.XmlTextReader sourceTextReader;
    private XmlValidatingReader validatingReader;
    private DTDObjectModel dtd;
    private XmlResolver resolver;
    private string currentElement;
    private DTDValidatingReader.AttributeSlot[] attributes;
    private int attributeCount;
    private int currentAttribute = -1;
    private bool consumedAttribute;
    private Stack elementStack;
    private Stack automataStack;
    private bool popScope;
    private bool isStandalone;
    private DTDAutomata currentAutomata;
    private DTDAutomata previousAutomata;
    private ArrayList idList;
    private ArrayList missingIDReferences;
    private XmlNamespaceManager nsmgr;
    private string currentTextValue;
    private string constructingTextValue;
    private bool shouldResetCurrentTextValue;
    private bool isSignificantWhitespace;
    private bool isWhitespace;
    private bool isText;
    private Stack attributeValueEntityStack = new Stack();
    private StringBuilder valueBuilder;
    private char[] whitespaceChars = new char[1]{ ' ' };

    public DTDValidatingReader(XmlReader reader)
      : this(reader, (XmlValidatingReader) null)
    {
    }

    internal DTDValidatingReader(XmlReader reader, XmlValidatingReader validatingReader)
    {
      this.reader = new EntityResolvingXmlReader(reader);
      this.sourceTextReader = reader as System.Xml.XmlTextReader;
      this.elementStack = new Stack();
      this.automataStack = new Stack();
      this.attributes = new DTDValidatingReader.AttributeSlot[10];
      this.nsmgr = new XmlNamespaceManager(reader.NameTable);
      this.validatingReader = validatingReader;
      this.valueBuilder = new StringBuilder();
      this.idList = new ArrayList();
      this.missingIDReferences = new ArrayList();
      if (reader is System.Xml.XmlTextReader xmlTextReader)
        this.resolver = xmlTextReader.Resolver;
      else
        this.resolver = (XmlResolver) new XmlUrlResolver();
    }

    IDictionary<string, string> IXmlNamespaceResolver.GetNamespacesInScope(XmlNamespaceScope scope)
    {
      IXmlNamespaceResolver reader = (IXmlNamespaceResolver) this.reader;
      return reader != null ? reader.GetNamespacesInScope(scope) : (IDictionary<string, string>) new Dictionary<string, string>();
    }

    bool IXmlLineInfo.HasLineInfo()
    {
      IXmlLineInfo reader = (IXmlLineInfo) this.reader;
      return reader != null && reader.HasLineInfo();
    }

    string IXmlNamespaceResolver.LookupPrefix(string ns) => ((IXmlNamespaceResolver) this.reader)?.LookupPrefix(ns);

    internal EntityResolvingXmlReader Source => this.reader;

    public DTDObjectModel DTD => this.dtd;

    public EntityHandling EntityHandling
    {
      get => this.reader.EntityHandling;
      set => this.reader.EntityHandling = value;
    }

    public override void Close() => this.reader.Close();

    private int GetAttributeIndex(string name)
    {
      for (int attributeIndex = 0; attributeIndex < this.attributeCount; ++attributeIndex)
      {
        if (this.attributes[attributeIndex].Name == name)
          return attributeIndex;
      }
      return -1;
    }

    private int GetAttributeIndex(string localName, string ns)
    {
      for (int attributeIndex = 0; attributeIndex < this.attributeCount; ++attributeIndex)
      {
        if (this.attributes[attributeIndex].LocalName == localName && this.attributes[attributeIndex].NS == ns)
          return attributeIndex;
      }
      return -1;
    }

    public override string GetAttribute(int i)
    {
      if (this.currentTextValue != null)
        throw new IndexOutOfRangeException("Specified index is out of range: " + (object) i);
      if (this.attributeCount <= i)
        throw new IndexOutOfRangeException("Specified index is out of range: " + (object) i);
      return this.attributes[i].Value;
    }

    public override string GetAttribute(string name)
    {
      if (this.currentTextValue != null)
        return (string) null;
      int attributeIndex = this.GetAttributeIndex(name);
      return attributeIndex < 0 ? (string) null : this.attributes[attributeIndex].Value;
    }

    public override string GetAttribute(string name, string ns)
    {
      if (this.currentTextValue != null)
        return (string) null;
      int attributeIndex = this.GetAttributeIndex(name, ns);
      return attributeIndex < 0 ? (string) null : this.attributes[attributeIndex].Value;
    }

    public override string LookupNamespace(string prefix)
    {
      string str = this.nsmgr.LookupNamespace(this.NameTable.Get(prefix));
      return str == string.Empty ? (string) null : str;
    }

    public override void MoveToAttribute(int i)
    {
      if (this.currentTextValue != null)
        throw new IndexOutOfRangeException("The index is out of range.");
      if (this.attributeCount <= i)
        throw new IndexOutOfRangeException("The index is out of range.");
      if (i < this.reader.AttributeCount)
        this.reader.MoveToAttribute(i);
      this.currentAttribute = i;
      this.consumedAttribute = false;
    }

    public override bool MoveToAttribute(string name)
    {
      if (this.currentTextValue != null)
        return false;
      int attributeIndex = this.GetAttributeIndex(name);
      if (attributeIndex < 0)
        return false;
      if (attributeIndex < this.reader.AttributeCount)
        this.reader.MoveToAttribute(attributeIndex);
      this.currentAttribute = attributeIndex;
      this.consumedAttribute = false;
      return true;
    }

    public override bool MoveToAttribute(string name, string ns)
    {
      if (this.currentTextValue != null)
        return false;
      int attributeIndex = this.GetAttributeIndex(name, ns);
      if (attributeIndex < 0)
        return false;
      if (attributeIndex < this.reader.AttributeCount)
        this.reader.MoveToAttribute(attributeIndex);
      this.currentAttribute = attributeIndex;
      this.consumedAttribute = false;
      return true;
    }

    public override bool MoveToElement()
    {
      if (this.currentTextValue != null || !this.reader.MoveToElement() && !this.IsDefault)
        return false;
      this.currentAttribute = -1;
      this.consumedAttribute = false;
      return true;
    }

    public override bool MoveToFirstAttribute()
    {
      if (this.currentTextValue != null || this.attributeCount == 0)
        return false;
      this.currentAttribute = 0;
      this.reader.MoveToFirstAttribute();
      this.consumedAttribute = false;
      return true;
    }

    public override bool MoveToNextAttribute()
    {
      if (this.currentTextValue != null)
        return false;
      if (this.currentAttribute == -1)
        return this.MoveToFirstAttribute();
      if (++this.currentAttribute == this.attributeCount)
      {
        --this.currentAttribute;
        return false;
      }
      if (this.currentAttribute < this.reader.AttributeCount)
        this.reader.MoveToAttribute(this.currentAttribute);
      this.consumedAttribute = false;
      return true;
    }

    public override bool Read()
    {
      if (this.currentTextValue != null)
        this.shouldResetCurrentTextValue = true;
      if (this.currentAttribute >= 0)
        this.MoveToElement();
      this.currentElement = (string) null;
      this.currentAttribute = -1;
      this.consumedAttribute = false;
      this.attributeCount = 0;
      this.isWhitespace = false;
      this.isSignificantWhitespace = false;
      this.isText = false;
      bool flag = this.ReadContent() || this.currentTextValue != null;
      if (!flag && (this.Settings == null || (this.Settings.ValidationFlags & XmlSchemaValidationFlags.ProcessIdentityConstraints) == XmlSchemaValidationFlags.None) && this.missingIDReferences.Count > 0)
      {
        this.HandleError("Missing ID reference was found: " + string.Join(",", this.missingIDReferences.ToArray(typeof (string)) as string[]), XmlSeverityType.Error);
        this.missingIDReferences.Clear();
      }
      if (this.validatingReader != null)
        this.EntityHandling = this.validatingReader.EntityHandling;
      return flag;
    }

    private bool ReadContent()
    {
      switch (this.reader.ReadState)
      {
        case ReadState.Error:
        case ReadState.EndOfFile:
        case ReadState.Closed:
          return false;
        default:
          if (this.popScope)
          {
            this.nsmgr.PopScope();
            this.popScope = false;
            if (this.elementStack.Count == 0)
              this.currentAutomata = (DTDAutomata) null;
          }
          bool flag = !this.reader.EOF;
          if (this.shouldResetCurrentTextValue)
          {
            this.currentTextValue = (string) null;
            this.shouldResetCurrentTextValue = false;
          }
          else
            flag = this.reader.Read();
          if (flag)
            return this.ProcessContent();
          if (this.elementStack.Count != 0)
            throw new InvalidOperationException("Unexpected end of XmlReader.");
          return false;
      }
    }

    private bool ProcessContent()
    {
      switch (this.reader.NodeType)
      {
        case XmlNodeType.Element:
          if (this.constructingTextValue != null)
          {
            this.currentTextValue = this.constructingTextValue;
            this.constructingTextValue = (string) null;
            if (this.isWhitespace)
              this.ValidateWhitespaceNode();
            return true;
          }
          this.ProcessStartElement();
          break;
        case XmlNodeType.Text:
          this.isWhitespace = this.isSignificantWhitespace = false;
          this.isText = true;
          goto case XmlNodeType.DocumentFragment;
        case XmlNodeType.CDATA:
          this.isSignificantWhitespace = this.isWhitespace = false;
          this.isText = true;
          this.ValidateText();
          if (this.currentTextValue != null)
          {
            this.currentTextValue = this.constructingTextValue;
            this.constructingTextValue = (string) null;
            return true;
          }
          break;
        case XmlNodeType.DocumentType:
          this.ReadDoctype();
          break;
        case XmlNodeType.DocumentFragment:
          if (this.reader.NodeType != XmlNodeType.DocumentFragment)
          {
            this.ValidateText();
            break;
          }
          break;
        case XmlNodeType.Whitespace:
          if (!this.isText && !this.isSignificantWhitespace)
          {
            this.isWhitespace = true;
            goto case XmlNodeType.DocumentFragment;
          }
          else
            goto case XmlNodeType.DocumentFragment;
        case XmlNodeType.SignificantWhitespace:
          if (!this.isText)
            this.isSignificantWhitespace = true;
          this.isWhitespace = false;
          goto case XmlNodeType.DocumentFragment;
        case XmlNodeType.EndElement:
          if (this.constructingTextValue != null)
          {
            this.currentTextValue = this.constructingTextValue;
            this.constructingTextValue = (string) null;
            return true;
          }
          this.ProcessEndElement();
          break;
        case XmlNodeType.XmlDeclaration:
          this.FillAttributes();
          if (this.GetAttribute("standalone") == "yes")
          {
            this.isStandalone = true;
            break;
          }
          break;
      }
      if (this.isWhitespace)
        this.ValidateWhitespaceNode();
      this.currentTextValue = this.constructingTextValue;
      this.constructingTextValue = (string) null;
      return true;
    }

    private void FillAttributes()
    {
      if (!this.reader.MoveToFirstAttribute())
        return;
      do
      {
        DTDValidatingReader.AttributeSlot attributeSlot = this.GetAttributeSlot();
        attributeSlot.Name = this.reader.Name;
        attributeSlot.LocalName = this.reader.LocalName;
        attributeSlot.Prefix = this.reader.Prefix;
        attributeSlot.NS = this.reader.NamespaceURI;
        attributeSlot.Value = this.reader.Value;
      }
      while (this.reader.MoveToNextAttribute());
      this.reader.MoveToElement();
    }

    private void ValidateText()
    {
      if (this.currentAutomata == null)
        return;
      DTDElementDeclaration elementDeclaration = (DTDElementDeclaration) null;
      if (this.elementStack.Count > 0)
        elementDeclaration = this.dtd.ElementDecls[this.elementStack.Peek() as string];
      if (elementDeclaration == null || elementDeclaration.IsMixedContent || elementDeclaration.IsAny || this.isWhitespace)
        return;
      this.HandleError(string.Format("Current element {0} does not allow character data content.", (object) (this.elementStack.Peek() as string)), XmlSeverityType.Error);
      this.currentAutomata = this.previousAutomata;
    }

    private void ValidateWhitespaceNode()
    {
      if (!this.isStandalone || this.DTD == null || this.elementStack.Count <= 0)
        return;
      DTDElementDeclaration elementDecl = this.DTD.ElementDecls[this.elementStack.Peek() as string];
      if (elementDecl == null || elementDecl.IsInternalSubset || elementDecl.IsMixedContent || elementDecl.IsAny || elementDecl.IsEmpty)
        return;
      this.HandleError("In a standalone document, whitespace cannot appear in an element which is declared to contain only element children.", XmlSeverityType.Error);
    }

    private void HandleError(string message, XmlSeverityType severity)
    {
      if (this.validatingReader != null && this.validatingReader.ValidationType == ValidationType.None)
        return;
      IXmlLineInfo xmlLineInfo = (IXmlLineInfo) this;
      bool flag = xmlLineInfo.HasLineInfo();
      this.HandleError(new XmlSchemaException(message, !flag ? 0 : xmlLineInfo.LineNumber, !flag ? 0 : xmlLineInfo.LinePosition, (XmlSchemaObject) null, this.BaseURI, (Exception) null), severity);
    }

    private void HandleError(XmlSchemaException ex, XmlSeverityType severity)
    {
      if (this.validatingReader != null && this.validatingReader.ValidationType == ValidationType.None)
        return;
      if (this.validatingReader != null)
        this.validatingReader.OnValidationEvent((object) this, new ValidationEventArgs(ex, ex.Message, severity));
      else if (severity == XmlSeverityType.Error)
        throw ex;
    }

    private void ValidateAttributes(DTDAttListDeclaration decl, bool validate)
    {
      this.DtdValidateAttributes(decl, validate);
      for (int index = 0; index < this.attributeCount; ++index)
      {
        DTDValidatingReader.AttributeSlot attribute = this.attributes[index];
        if (attribute.Name == "xmlns" || attribute.Prefix == "xmlns")
          this.nsmgr.AddNamespace(!(attribute.Prefix == "xmlns") ? string.Empty : attribute.LocalName, attribute.Value);
      }
      for (int index = 0; index < this.attributeCount; ++index)
      {
        DTDValidatingReader.AttributeSlot attribute = this.attributes[index];
        attribute.NS = !(attribute.Name == "xmlns") ? (attribute.Prefix.Length <= 0 ? string.Empty : this.LookupNamespace(attribute.Prefix)) : "http://www.w3.org/2000/xmlns/";
      }
    }

    private DTDValidatingReader.AttributeSlot GetAttributeSlot()
    {
      if (this.attributeCount == this.attributes.Length)
      {
        DTDValidatingReader.AttributeSlot[] destinationArray = new DTDValidatingReader.AttributeSlot[this.attributeCount << 1];
        Array.Copy((Array) this.attributes, (Array) destinationArray, this.attributeCount);
        this.attributes = destinationArray;
      }
      if (this.attributes[this.attributeCount] == null)
        this.attributes[this.attributeCount] = new DTDValidatingReader.AttributeSlot();
      DTDValidatingReader.AttributeSlot attribute = this.attributes[this.attributeCount];
      attribute.Clear();
      ++this.attributeCount;
      return attribute;
    }

    private void DtdValidateAttributes(DTDAttListDeclaration decl, bool validate)
    {
      while (this.reader.MoveToNextAttribute())
      {
        string name = this.reader.Name;
        DTDValidatingReader.AttributeSlot attributeSlot = this.GetAttributeSlot();
        attributeSlot.Name = this.reader.Name;
        attributeSlot.LocalName = this.reader.LocalName;
        attributeSlot.Prefix = this.reader.Prefix;
        XmlReader xmlReader = (XmlReader) this.reader;
        string empty = string.Empty;
        while (this.attributeValueEntityStack.Count >= 0)
        {
          if (!xmlReader.ReadAttributeValue())
          {
            if (this.attributeValueEntityStack.Count > 0)
              xmlReader = this.attributeValueEntityStack.Pop() as XmlReader;
            else
              break;
          }
          else
          {
            switch (xmlReader.NodeType)
            {
              case XmlNodeType.EntityReference:
                DTDEntityDeclaration entityDecl1 = this.DTD.EntityDecls[xmlReader.Name];
                if (entityDecl1 == null)
                {
                  this.HandleError(string.Format("Referenced entity {0} is not declared.", (object) xmlReader.Name), XmlSeverityType.Error);
                  continue;
                }
                System.Xml.XmlTextReader xmlTextReader = new System.Xml.XmlTextReader(entityDecl1.EntityValue, XmlNodeType.Attribute, this.ParserContext);
                this.attributeValueEntityStack.Push((object) xmlReader);
                xmlReader = (XmlReader) xmlTextReader;
                continue;
              case XmlNodeType.EndEntity:
                continue;
              default:
                empty += xmlReader.Value;
                continue;
            }
          }
        }
        this.reader.MoveToElement();
        this.reader.MoveToAttribute(name);
        attributeSlot.Value = this.FilterNormalization(name, empty);
        if (validate)
        {
          DTDAttributeDefinition attributeDefinition = decl[this.reader.Name];
          if (attributeDefinition == null)
          {
            this.HandleError(string.Format("Attribute {0} is not declared.", (object) this.reader.Name), XmlSeverityType.Error);
          }
          else
          {
            if (attributeDefinition.EnumeratedAttributeDeclaration.Count > 0 && !attributeDefinition.EnumeratedAttributeDeclaration.Contains((object) attributeSlot.Value))
              this.HandleError(string.Format("Attribute enumeration constraint error in attribute {0}, value {1}.", (object) this.reader.Name, (object) empty), XmlSeverityType.Error);
            if (attributeDefinition.EnumeratedNotations.Count > 0 && !attributeDefinition.EnumeratedNotations.Contains((object) attributeSlot.Value))
              this.HandleError(string.Format("Attribute notation enumeration constraint error in attribute {0}, value {1}.", (object) this.reader.Name, (object) empty), XmlSeverityType.Error);
            string str1 = attributeDefinition.Datatype == null ? empty : this.FilterNormalization(attributeDefinition.Name, empty);
            string[] strArray = (string[]) null;
            switch (attributeDefinition.Datatype.TokenizedType)
            {
              case XmlTokenizedType.IDREFS:
              case XmlTokenizedType.ENTITIES:
              case XmlTokenizedType.NMTOKENS:
                try
                {
                  strArray = attributeDefinition.Datatype.ParseValue(str1, this.NameTable, (IXmlNamespaceResolver) null) as string[];
                  break;
                }
                catch (Exception ex)
                {
                  this.HandleError("Attribute value is invalid against its data type.", XmlSeverityType.Error);
                  strArray = new string[0];
                  break;
                }
              default:
                try
                {
                  attributeDefinition.Datatype.ParseValue(str1, this.NameTable, (IXmlNamespaceResolver) null);
                  break;
                }
                catch (Exception ex)
                {
                  this.HandleError(string.Format("Attribute value is invalid against its data type '{0}'. {1}", (object) attributeDefinition.Datatype, (object) ex.Message), XmlSeverityType.Error);
                  break;
                }
            }
            switch (attributeDefinition.Datatype.TokenizedType)
            {
              case XmlTokenizedType.ID:
                if (this.idList.Contains((object) str1))
                {
                  this.HandleError(string.Format("Node with ID {0} was already appeared.", (object) empty), XmlSeverityType.Error);
                  break;
                }
                if (this.missingIDReferences.Contains((object) str1))
                  this.missingIDReferences.Remove((object) str1);
                this.idList.Add((object) str1);
                break;
              case XmlTokenizedType.IDREF:
                if (!this.idList.Contains((object) str1))
                {
                  this.missingIDReferences.Add((object) str1);
                  break;
                }
                break;
              case XmlTokenizedType.IDREFS:
                for (int index = 0; index < strArray.Length; ++index)
                {
                  string str2 = strArray[index];
                  if (!this.idList.Contains((object) str2))
                    this.missingIDReferences.Add((object) str2);
                }
                break;
              case XmlTokenizedType.ENTITY:
                DTDEntityDeclaration entityDecl2 = this.dtd.EntityDecls[str1];
                if (entityDecl2 == null)
                {
                  this.HandleError("Reference to undeclared entity was found in attribute: " + this.reader.Name + ".", XmlSeverityType.Error);
                  break;
                }
                if (entityDecl2.NotationName == null)
                {
                  this.HandleError("The entity specified by entity type value must be an unparsed entity. The entity definition has no NDATA in attribute: " + this.reader.Name + ".", XmlSeverityType.Error);
                  break;
                }
                break;
              case XmlTokenizedType.ENTITIES:
                for (int index = 0; index < strArray.Length; ++index)
                {
                  DTDEntityDeclaration entityDecl3 = this.dtd.EntityDecls[this.FilterNormalization(this.reader.Name, strArray[index])];
                  if (entityDecl3 == null)
                    this.HandleError("Reference to undeclared entity was found in attribute: " + this.reader.Name + ".", XmlSeverityType.Error);
                  else if (entityDecl3.NotationName == null)
                    this.HandleError("The entity specified by ENTITIES type value must be an unparsed entity. The entity definition has no NDATA in attribute: " + this.reader.Name + ".", XmlSeverityType.Error);
                }
                break;
            }
            if (this.isStandalone && !attributeDefinition.IsInternalSubset && empty != str1)
              this.HandleError("In standalone document, attribute value characters must not be checked against external definition.", XmlSeverityType.Error);
            if (attributeDefinition.OccurenceType == DTDAttributeOccurenceType.Fixed && empty != attributeDefinition.DefaultValue)
              this.HandleError(string.Format("Fixed attribute {0} in element {1} has invalid value {2}.", (object) attributeDefinition.Name, (object) decl.Name, (object) empty), XmlSeverityType.Error);
          }
        }
      }
      if (validate)
        this.VerifyDeclaredAttributes(decl);
      this.MoveToElement();
    }

    private void ReadDoctype()
    {
      this.FillAttributes();
      IHasXmlParserContext reader = (IHasXmlParserContext) this.reader;
      if (reader != null)
        this.dtd = reader.ParserContext.Dtd;
      if (this.dtd == null)
      {
        Mono.Xml2.XmlTextReader xmlTextReader = new Mono.Xml2.XmlTextReader(string.Empty, XmlNodeType.Document, (XmlParserContext) null);
        xmlTextReader.XmlResolver = this.resolver;
        xmlTextReader.GenerateDTDObjectModel(this.reader.Name, this.reader["PUBLIC"], this.reader["SYSTEM"], this.reader.Value);
        this.dtd = xmlTextReader.DTD;
      }
      this.currentAutomata = this.dtd.RootAutomata;
      for (int index = 0; index < this.DTD.Errors.Length; ++index)
        this.HandleError(this.DTD.Errors[index].Message, XmlSeverityType.Error);
      foreach (DTDEntityDeclaration entityDeclaration in this.dtd.EntityDecls.Values)
      {
        if (entityDeclaration.NotationName != null && this.dtd.NotationDecls[entityDeclaration.NotationName] == null)
          this.HandleError("Target notation was not found for NData in entity declaration " + entityDeclaration.Name + ".", XmlSeverityType.Error);
      }
      foreach (DTDAttListDeclaration attListDeclaration in this.dtd.AttListDecls.Values)
      {
        foreach (DTDAttributeDefinition definition in (IEnumerable) attListDeclaration.Definitions)
        {
          if (definition.Datatype.TokenizedType == XmlTokenizedType.NOTATION)
          {
            foreach (string enumeratedNotation in definition.EnumeratedNotations)
            {
              if (this.dtd.NotationDecls[enumeratedNotation] == null)
                this.HandleError("Target notation was not found for NOTATION typed attribute default " + definition.Name + ".", XmlSeverityType.Error);
            }
          }
        }
      }
    }

    private void ProcessStartElement()
    {
      this.nsmgr.PushScope();
      this.popScope = this.reader.IsEmptyElement;
      this.elementStack.Push((object) this.reader.Name);
      this.currentElement = this.Name;
      if (this.currentAutomata == null)
      {
        this.ValidateAttributes((DTDAttListDeclaration) null, false);
        if (!this.reader.IsEmptyElement)
          return;
        this.ProcessEndElement();
      }
      else
      {
        this.previousAutomata = this.currentAutomata;
        this.currentAutomata = this.currentAutomata.TryStartElement(this.reader.Name);
        if (this.currentAutomata == this.DTD.Invalid)
        {
          this.HandleError(string.Format("Invalid start element found: {0}", (object) this.reader.Name), XmlSeverityType.Error);
          this.currentAutomata = this.previousAutomata;
        }
        DTDElementDeclaration elementDecl = this.DTD.ElementDecls[this.reader.Name];
        if (elementDecl == null)
        {
          this.HandleError(string.Format("Element {0} is not declared.", (object) this.reader.Name), XmlSeverityType.Error);
          this.currentAutomata = this.previousAutomata;
        }
        this.automataStack.Push((object) this.currentAutomata);
        if (elementDecl != null)
          this.currentAutomata = elementDecl.ContentModel.GetAutomata();
        DTDAttListDeclaration attListDecl = this.dtd.AttListDecls[this.currentElement];
        if (attListDecl != null)
        {
          this.ValidateAttributes(attListDecl, true);
          this.currentAttribute = -1;
        }
        else
        {
          if (this.reader.HasAttributes)
            this.HandleError(string.Format("Attributes are found on element {0} while it has no attribute definitions.", (object) this.currentElement), XmlSeverityType.Error);
          this.ValidateAttributes((DTDAttListDeclaration) null, false);
        }
        if (!this.reader.IsEmptyElement)
          return;
        this.ProcessEndElement();
      }
    }

    private void ProcessEndElement()
    {
      this.popScope = true;
      this.elementStack.Pop();
      if (this.currentAutomata == null)
        return;
      if (this.DTD.ElementDecls[this.reader.Name] == null)
        this.HandleError(string.Format("Element {0} is not declared.", (object) this.reader.Name), XmlSeverityType.Error);
      this.previousAutomata = this.currentAutomata;
      if (this.currentAutomata.TryEndElement() == this.DTD.Invalid)
      {
        this.HandleError(string.Format("Invalid end element found: {0}", (object) this.reader.Name), XmlSeverityType.Error);
        this.currentAutomata = this.previousAutomata;
      }
      this.currentAutomata = this.automataStack.Pop() as DTDAutomata;
    }

    private void VerifyDeclaredAttributes(DTDAttListDeclaration decl)
    {
      for (int index1 = 0; index1 < decl.Definitions.Count; ++index1)
      {
        DTDAttributeDefinition definition = (DTDAttributeDefinition) decl.Definitions[index1];
        bool flag = false;
        for (int index2 = 0; index2 < this.attributeCount; ++index2)
        {
          if (this.attributes[index2].Name == definition.Name)
          {
            flag = true;
            break;
          }
        }
        if (!flag)
        {
          if (definition.OccurenceType == DTDAttributeOccurenceType.Required)
            this.HandleError(string.Format("Required attribute {0} in element {1} not found .", (object) definition.Name, (object) decl.Name), XmlSeverityType.Error);
          else if (definition.DefaultValue != null)
          {
            if (this.isStandalone && !definition.IsInternalSubset)
              this.HandleError("In standalone document, external default value definition must not be applied.", XmlSeverityType.Error);
            switch (this.validatingReader.ValidationType)
            {
              case ValidationType.None:
              case ValidationType.DTD:
                DTDValidatingReader.AttributeSlot attributeSlot = this.GetAttributeSlot();
                attributeSlot.Name = definition.Name;
                int length = definition.Name.IndexOf(':');
                attributeSlot.LocalName = length >= 0 ? definition.Name.Substring(length + 1) : definition.Name;
                string str = length >= 0 ? definition.Name.Substring(0, length) : string.Empty;
                attributeSlot.Prefix = str;
                attributeSlot.Value = definition.DefaultValue;
                attributeSlot.IsDefault = true;
                continue;
              case ValidationType.Auto:
                if (this.validatingReader.Schemas.Count != 0)
                  continue;
                goto case ValidationType.None;
              default:
                continue;
            }
          }
        }
      }
    }

    public override bool ReadAttributeValue()
    {
      if (this.consumedAttribute)
        return false;
      if (this.NodeType == XmlNodeType.Attribute && this.EntityHandling == EntityHandling.ExpandEntities)
      {
        this.consumedAttribute = true;
        return true;
      }
      if (!this.IsDefault)
        return this.reader.ReadAttributeValue();
      this.consumedAttribute = true;
      return true;
    }

    public override void ResolveEntity() => this.reader.ResolveEntity();

    public override int AttributeCount => this.currentTextValue != null ? 0 : this.attributeCount;

    public override string BaseURI => this.reader.BaseURI;

    public override bool CanResolveEntity => true;

    public override int Depth
    {
      get
      {
        int depth = this.reader.Depth;
        if (this.currentTextValue != null && this.reader.NodeType == XmlNodeType.EndElement)
          ++depth;
        return this.IsDefault ? depth + 1 : depth;
      }
    }

    public override bool EOF => this.reader.EOF;

    public override bool HasValue => this.currentAttribute >= 0 || this.currentTextValue != null || this.reader.HasValue;

    public override bool IsDefault => this.currentTextValue == null && this.currentAttribute != -1 && this.attributes[this.currentAttribute].IsDefault;

    public override bool IsEmptyElement => this.currentTextValue == null && this.reader.IsEmptyElement;

    public override string this[int i] => this.GetAttribute(i);

    public override string this[string name] => this.GetAttribute(name);

    public override string this[string name, string ns] => this.GetAttribute(name, ns);

    public int LineNumber
    {
      get
      {
        IXmlLineInfo reader = (IXmlLineInfo) this.reader;
        return reader != null ? reader.LineNumber : 0;
      }
    }

    public int LinePosition
    {
      get
      {
        IXmlLineInfo reader = (IXmlLineInfo) this.reader;
        return reader != null ? reader.LinePosition : 0;
      }
    }

    public override string LocalName
    {
      get
      {
        if (this.currentTextValue != null || this.consumedAttribute)
          return string.Empty;
        return this.NodeType == XmlNodeType.Attribute ? this.attributes[this.currentAttribute].LocalName : this.reader.LocalName;
      }
    }

    public override string Name
    {
      get
      {
        if (this.currentTextValue != null || this.consumedAttribute)
          return string.Empty;
        return this.NodeType == XmlNodeType.Attribute ? this.attributes[this.currentAttribute].Name : this.reader.Name;
      }
    }

    public override string NamespaceURI
    {
      get
      {
        if (this.currentTextValue != null || this.consumedAttribute)
          return string.Empty;
        switch (this.NodeType)
        {
          case XmlNodeType.Element:
          case XmlNodeType.EndElement:
            return this.nsmgr.LookupNamespace(this.Prefix);
          case XmlNodeType.Attribute:
            return this.attributes[this.currentAttribute].NS;
          default:
            return string.Empty;
        }
      }
    }

    public override XmlNameTable NameTable => this.reader.NameTable;

    public override XmlNodeType NodeType
    {
      get
      {
        if (this.currentTextValue != null)
        {
          if (this.isSignificantWhitespace)
            return XmlNodeType.SignificantWhitespace;
          return this.isWhitespace ? XmlNodeType.Whitespace : XmlNodeType.Text;
        }
        if (this.consumedAttribute)
          return XmlNodeType.Text;
        return this.IsDefault ? XmlNodeType.Attribute : this.reader.NodeType;
      }
    }

    public XmlParserContext ParserContext => XmlSchemaUtil.GetParserContext((XmlReader) this.reader);

    public override string Prefix
    {
      get
      {
        if (this.currentTextValue != null || this.consumedAttribute)
          return string.Empty;
        return this.NodeType == XmlNodeType.Attribute ? this.attributes[this.currentAttribute].Prefix : this.reader.Prefix;
      }
    }

    public override char QuoteChar => this.reader.QuoteChar;

    public override ReadState ReadState => this.reader.ReadState == ReadState.EndOfFile && this.currentTextValue != null ? ReadState.Interactive : this.reader.ReadState;

    public object SchemaType
    {
      get
      {
        if (this.DTD == null || this.currentAttribute == -1 || this.currentElement == null)
          return (object) null;
        DTDAttListDeclaration attListDecl = this.DTD.AttListDecls[this.currentElement];
        DTDAttributeDefinition attributeDefinition = attListDecl == null ? (DTDAttributeDefinition) null : attListDecl[this.attributes[this.currentAttribute].Name];
        return attributeDefinition != null ? (object) attributeDefinition.Datatype : (object) null;
      }
    }

    private string FilterNormalization(string attrName, string rawValue)
    {
      if (this.DTD == null || this.sourceTextReader == null || !this.sourceTextReader.Normalization)
        return rawValue;
      DTDAttributeDefinition attributeDefinition = this.dtd.AttListDecls[this.currentElement].Get(attrName);
      this.valueBuilder.Append(rawValue);
      this.valueBuilder.Replace('\r', ' ');
      this.valueBuilder.Replace('\n', ' ');
      this.valueBuilder.Replace('\t', ' ');
      try
      {
        if (attributeDefinition == null || attributeDefinition.Datatype.TokenizedType == XmlTokenizedType.CDATA)
          return this.valueBuilder.ToString();
        for (int index = 0; index < this.valueBuilder.Length; ++index)
        {
          if (this.valueBuilder[index] == ' ')
          {
            while (++index < this.valueBuilder.Length && this.valueBuilder[index] == ' ')
              this.valueBuilder.Remove(index, 1);
          }
        }
        return this.valueBuilder.ToString().Trim(this.whitespaceChars);
      }
      finally
      {
        this.valueBuilder.Length = 0;
      }
    }

    public override string Value
    {
      get
      {
        if (this.currentTextValue != null)
          return this.currentTextValue;
        return this.NodeType == XmlNodeType.Attribute || this.consumedAttribute ? this.attributes[this.currentAttribute].Value : this.reader.Value;
      }
    }

    public override string XmlLang => this["xml:lang"] ?? this.reader.XmlLang;

    internal XmlResolver Resolver => this.resolver;

    public XmlResolver XmlResolver
    {
      set
      {
        if (this.dtd != null)
          this.dtd.XmlResolver = value;
        this.resolver = value;
      }
    }

    public override XmlSpace XmlSpace
    {
      get
      {
        string key = this["xml:space"];
        if (key != null)
        {
          if (DTDValidatingReader.\u003C\u003Ef__switch\u0024map43 == null)
            DTDValidatingReader.\u003C\u003Ef__switch\u0024map43 = new Dictionary<string, int>(2)
            {
              {
                "preserve",
                0
              },
              {
                "default",
                1
              }
            };
          int num;
          if (DTDValidatingReader.\u003C\u003Ef__switch\u0024map43.TryGetValue(key, out num))
          {
            if (num == 0)
              return XmlSpace.Preserve;
            if (num == 1)
              return XmlSpace.Default;
          }
        }
        return this.reader.XmlSpace;
      }
    }

    private class AttributeSlot
    {
      public string Name;
      public string LocalName;
      public string NS;
      public string Prefix;
      public string Value;
      public bool IsDefault;

      public void Clear()
      {
        this.Prefix = string.Empty;
        this.LocalName = string.Empty;
        this.NS = string.Empty;
        this.Value = string.Empty;
        this.IsDefault = false;
      }
    }
  }
}
