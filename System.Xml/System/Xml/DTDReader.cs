// Decompiled with JetBrains decompiler
// Type: System.Xml.DTDReader
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using Mono.Xml;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Text;

namespace System.Xml
{
  internal class DTDReader : IXmlLineInfo
  {
    private const int initialNameCapacity = 256;
    private XmlParserInput currentInput;
    private Stack parserInputStack;
    private char[] nameBuffer;
    private int nameLength;
    private int nameCapacity;
    private StringBuilder valueBuffer;
    private int currentLinkedNodeLineNumber;
    private int currentLinkedNodeLinePosition;
    private int dtdIncludeSect;
    private bool normalization;
    private bool processingInternalSubset;
    private string cachedPublicId;
    private string cachedSystemId;
    private DTDObjectModel DTD;

    public DTDReader(DTDObjectModel dtd, int startLineNumber, int startLinePosition)
    {
      this.DTD = dtd;
      this.currentLinkedNodeLineNumber = startLineNumber;
      this.currentLinkedNodeLinePosition = startLinePosition;
      this.Init();
    }

    public string BaseURI => this.currentInput.BaseURI;

    public bool Normalization
    {
      get => this.normalization;
      set => this.normalization = value;
    }

    public int LineNumber => this.currentInput.LineNumber;

    public int LinePosition => this.currentInput.LinePosition;

    public bool HasLineInfo() => true;

    private XmlException NotWFError(string message) => new XmlException((IXmlLineInfo) this, this.BaseURI, message);

    private void Init()
    {
      this.parserInputStack = new Stack();
      this.nameBuffer = new char[256];
      this.nameLength = 0;
      this.nameCapacity = 256;
      this.valueBuffer = new StringBuilder(512);
    }

    internal DTDObjectModel GenerateDTDObjectModel()
    {
      int count = this.parserInputStack.Count;
      if (this.DTD.InternalSubset != null && this.DTD.InternalSubset.Length > 0)
      {
        this.processingInternalSubset = true;
        XmlParserInput currentInput = this.currentInput;
        this.currentInput = new XmlParserInput((TextReader) new StringReader(this.DTD.InternalSubset), this.DTD.BaseURI, this.currentLinkedNodeLineNumber, this.currentLinkedNodeLinePosition);
        this.currentInput.AllowTextDecl = false;
        bool flag;
        do
        {
          flag = this.ProcessDTDSubset();
          if (this.PeekChar() == -1 && this.parserInputStack.Count > 0)
            this.PopParserInput();
        }
        while (flag || this.parserInputStack.Count > count);
        if (this.dtdIncludeSect != 0)
          throw this.NotWFError("INCLUDE section is not ended correctly.");
        this.currentInput = currentInput;
        this.processingInternalSubset = false;
      }
      if (this.DTD.SystemId != null && this.DTD.SystemId != string.Empty && this.DTD.Resolver != null)
      {
        this.PushParserInput(this.DTD.SystemId);
        bool flag;
        do
        {
          flag = this.ProcessDTDSubset();
          if (this.PeekChar() == -1 && this.parserInputStack.Count > 1)
            this.PopParserInput();
        }
        while (flag || this.parserInputStack.Count > count + 1);
        if (this.dtdIncludeSect != 0)
          throw this.NotWFError("INCLUDE section is not ended correctly.");
        this.PopParserInput();
      }
      ArrayList refs = new ArrayList();
      foreach (DTDEntityDeclaration entityDeclaration in this.DTD.EntityDecls.Values)
      {
        if (entityDeclaration.NotationName != null)
        {
          entityDeclaration.ScanEntityValue(refs);
          refs.Clear();
        }
      }
      this.DTD.ExternalResources.Clear();
      return this.DTD;
    }

    private bool ProcessDTDSubset()
    {
      this.SkipWhitespace();
      int num1 = this.ReadChar();
      switch (num1)
      {
        case -1:
          return false;
        case 37:
          if (this.processingInternalSubset)
            this.DTD.InternalSubsetHasPEReference = true;
          string peName = this.ReadName();
          this.Expect(59);
          DTDParameterEntityDeclaration peDecl = this.GetPEDecl(peName);
          if (peDecl != null)
          {
            this.currentInput.PushPEBuffer(peDecl);
            while (this.currentInput.HasPEBuffer)
              this.ProcessDTDSubset();
            this.SkipWhitespace();
            break;
          }
          break;
        case 60:
          int num2 = this.ReadChar();
          switch (num2)
          {
            case -1:
              throw this.NotWFError("Unexpected end of stream.");
            case 33:
              this.CompileDeclaration();
              break;
            case 63:
              this.ReadProcessingInstruction();
              break;
            default:
              throw this.NotWFError("Syntax Error after '<' character: " + (object) (char) num2);
          }
          break;
        case 93:
          if (this.dtdIncludeSect == 0)
            throw this.NotWFError("Unbalanced end of INCLUDE/IGNORE section.");
          this.Expect("]>");
          --this.dtdIncludeSect;
          this.SkipWhitespace();
          break;
        default:
          throw this.NotWFError(string.Format("Syntax Error inside doctypedecl markup : {0}({1})", (object) num1, (object) (char) num1));
      }
      this.currentInput.AllowTextDecl = false;
      return true;
    }

    private void CompileDeclaration()
    {
      switch (this.ReadChar())
      {
        case 45:
          this.Expect(45);
          this.ReadComment();
          break;
        case 65:
          this.Expect("TTLIST");
          DTDAttListDeclaration decl1 = this.ReadAttListDecl();
          this.DTD.AttListDecls.Add(decl1.Name, decl1);
          break;
        case 69:
          switch (this.ReadChar())
          {
            case 76:
              this.Expect("EMENT");
              DTDElementDeclaration decl2 = this.ReadElementDecl();
              this.DTD.ElementDecls.Add(decl2.Name, decl2);
              return;
            case 78:
              this.Expect("TITY");
              if (!this.SkipWhitespace())
                throw this.NotWFError("Whitespace is required after '<!ENTITY' in DTD entity declaration.");
              while (this.PeekChar() == 37)
              {
                this.ReadChar();
                if (!this.SkipWhitespace())
                {
                  this.ExpandPERef();
                }
                else
                {
                  this.TryExpandPERef();
                  if (!XmlChar.IsNameChar(this.PeekChar()))
                    throw this.NotWFError("expected name character");
                  this.ReadParameterEntityDecl();
                  return;
                }
              }
              DTDEntityDeclaration decl3 = this.ReadEntityDecl();
              if (this.DTD.EntityDecls[decl3.Name] != null)
                return;
              this.DTD.EntityDecls.Add(decl3.Name, decl3);
              return;
            default:
              throw this.NotWFError("Syntax Error after '<!E' (ELEMENT or ENTITY must be found)");
          }
        case 78:
          this.Expect("OTATION");
          DTDNotationDeclaration decl4 = this.ReadNotationDecl();
          this.DTD.NotationDecls.Add(decl4.Name, decl4);
          break;
        case 91:
          this.SkipWhitespace();
          this.TryExpandPERef();
          this.Expect(73);
          switch (this.ReadChar())
          {
            case 71:
              this.Expect("NORE");
              this.ReadIgnoreSect();
              return;
            case 78:
              this.Expect("CLUDE");
              this.ExpectAfterWhitespace('[');
              ++this.dtdIncludeSect;
              return;
            default:
              return;
          }
        default:
          throw this.NotWFError("Syntax Error after '<!' characters.");
      }
    }

    private void ReadIgnoreSect()
    {
      this.ExpectAfterWhitespace('[');
      int num = 1;
      while (num > 0)
      {
        switch (this.ReadChar())
        {
          case -1:
            throw this.NotWFError("Unexpected IGNORE section end.");
          case 60:
            if (this.PeekChar() == 33)
            {
              this.ReadChar();
              if (this.PeekChar() == 91)
              {
                this.ReadChar();
                ++num;
                continue;
              }
              continue;
            }
            continue;
          case 93:
            if (this.PeekChar() == 93)
            {
              this.ReadChar();
              if (this.PeekChar() == 62)
              {
                this.ReadChar();
                --num;
                continue;
              }
              continue;
            }
            continue;
          default:
            continue;
        }
      }
      if (num != 0)
        throw this.NotWFError("IGNORE section is not ended correctly.");
    }

    private DTDElementDeclaration ReadElementDecl()
    {
      DTDElementDeclaration decl = new DTDElementDeclaration(this.DTD);
      decl.IsInternalSubset = this.processingInternalSubset;
      if (!this.SkipWhitespace())
        throw this.NotWFError("Whitespace is required between '<!ELEMENT' and name in DTD element declaration.");
      this.TryExpandPERef();
      decl.Name = this.ReadName();
      if (!this.SkipWhitespace())
        throw this.NotWFError("Whitespace is required between name and content in DTD element declaration.");
      this.TryExpandPERef();
      this.ReadContentSpec(decl);
      this.SkipWhitespace();
      this.TryExpandPERef();
      this.Expect(62);
      return decl;
    }

    private void ReadContentSpec(DTDElementDeclaration decl)
    {
      this.TryExpandPERef();
      switch (this.ReadChar())
      {
        case 40:
          DTDContentModel contentModel = decl.ContentModel;
          this.SkipWhitespace();
          this.TryExpandPERef();
          if (this.PeekChar() == 35)
          {
            decl.IsMixedContent = true;
            contentModel.Occurence = DTDOccurence.ZeroOrMore;
            contentModel.OrderType = DTDContentOrderType.Or;
            this.Expect("#PCDATA");
            this.SkipWhitespace();
            this.TryExpandPERef();
            while (this.PeekChar() != 41)
            {
              this.SkipWhitespace();
              if (this.PeekChar() == 37)
              {
                this.TryExpandPERef();
              }
              else
              {
                this.Expect(124);
                this.SkipWhitespace();
                this.TryExpandPERef();
                this.AddContentModel(contentModel.ChildModels, new DTDContentModel(this.DTD, decl.Name)
                {
                  ElementName = this.ReadName()
                });
                this.SkipWhitespace();
                this.TryExpandPERef();
              }
            }
            this.Expect(41);
            if (contentModel.ChildModels.Count > 0)
              this.Expect(42);
            else if (this.PeekChar() == 42)
              this.Expect(42);
          }
          else
          {
            contentModel.ChildModels.Add(this.ReadCP(decl));
            this.SkipWhitespace();
            while (true)
            {
              while (this.PeekChar() != 37)
              {
                if (this.PeekChar() == 124)
                {
                  contentModel.OrderType = contentModel.OrderType != DTDContentOrderType.Seq ? DTDContentOrderType.Or : throw this.NotWFError("Inconsistent choice markup in sequence cp.");
                  this.ReadChar();
                  this.SkipWhitespace();
                  this.AddContentModel(contentModel.ChildModels, this.ReadCP(decl));
                  this.SkipWhitespace();
                }
                else if (this.PeekChar() == 44)
                {
                  contentModel.OrderType = contentModel.OrderType != DTDContentOrderType.Or ? DTDContentOrderType.Seq : throw this.NotWFError("Inconsistent sequence markup in choice cp.");
                  this.ReadChar();
                  this.SkipWhitespace();
                  contentModel.ChildModels.Add(this.ReadCP(decl));
                  this.SkipWhitespace();
                }
                else
                {
                  this.Expect(41);
                  switch (this.PeekChar())
                  {
                    case 42:
                      contentModel.Occurence = DTDOccurence.ZeroOrMore;
                      this.ReadChar();
                      break;
                    case 43:
                      contentModel.Occurence = DTDOccurence.OneOrMore;
                      this.ReadChar();
                      break;
                    case 63:
                      contentModel.Occurence = DTDOccurence.Optional;
                      this.ReadChar();
                      break;
                  }
                  this.SkipWhitespace();
                  goto label_29;
                }
              }
              this.TryExpandPERef();
            }
          }
label_29:
          this.SkipWhitespace();
          break;
        case 65:
          decl.IsAny = true;
          this.Expect("NY");
          break;
        case 69:
          decl.IsEmpty = true;
          this.Expect("MPTY");
          break;
        default:
          throw this.NotWFError("ContentSpec is missing.");
      }
    }

    private DTDContentModel ReadCP(DTDElementDeclaration elem)
    {
      this.TryExpandPERef();
      DTDContentModel dtdContentModel;
      if (this.PeekChar() == 40)
      {
        dtdContentModel = new DTDContentModel(this.DTD, elem.Name);
        this.ReadChar();
        this.SkipWhitespace();
        dtdContentModel.ChildModels.Add(this.ReadCP(elem));
        this.SkipWhitespace();
        while (true)
        {
          while (this.PeekChar() != 37)
          {
            if (this.PeekChar() == 124)
            {
              dtdContentModel.OrderType = dtdContentModel.OrderType != DTDContentOrderType.Seq ? DTDContentOrderType.Or : throw this.NotWFError("Inconsistent choice markup in sequence cp.");
              this.ReadChar();
              this.SkipWhitespace();
              this.AddContentModel(dtdContentModel.ChildModels, this.ReadCP(elem));
              this.SkipWhitespace();
            }
            else if (this.PeekChar() == 44)
            {
              dtdContentModel.OrderType = dtdContentModel.OrderType != DTDContentOrderType.Or ? DTDContentOrderType.Seq : throw this.NotWFError("Inconsistent sequence markup in choice cp.");
              this.ReadChar();
              this.SkipWhitespace();
              dtdContentModel.ChildModels.Add(this.ReadCP(elem));
              this.SkipWhitespace();
            }
            else
            {
              this.ExpectAfterWhitespace(')');
              goto label_14;
            }
          }
          this.TryExpandPERef();
        }
      }
      else
      {
        this.TryExpandPERef();
        dtdContentModel = new DTDContentModel(this.DTD, elem.Name);
        dtdContentModel.ElementName = this.ReadName();
      }
label_14:
      switch (this.PeekChar())
      {
        case 42:
          dtdContentModel.Occurence = DTDOccurence.ZeroOrMore;
          this.ReadChar();
          break;
        case 43:
          dtdContentModel.Occurence = DTDOccurence.OneOrMore;
          this.ReadChar();
          break;
        case 63:
          dtdContentModel.Occurence = DTDOccurence.Optional;
          this.ReadChar();
          break;
      }
      return dtdContentModel;
    }

    private void AddContentModel(DTDContentModelCollection cmc, DTDContentModel cm)
    {
      if (cm.ElementName != null)
      {
        for (int i = 0; i < cmc.Count; ++i)
        {
          if (cmc[i].ElementName == cm.ElementName)
          {
            this.HandleError(new XmlException("Element content must be unique inside mixed content model.", this.LineNumber, this.LinePosition, (object) null, this.BaseURI, (Exception) null));
            return;
          }
        }
      }
      cmc.Add(cm);
    }

    private void ReadParameterEntityDecl()
    {
      DTDParameterEntityDeclaration decl = new DTDParameterEntityDeclaration(this.DTD);
      decl.BaseURI = this.BaseURI;
      decl.XmlResolver = this.DTD.Resolver;
      decl.Name = this.ReadName();
      if (!this.SkipWhitespace())
        throw this.NotWFError("Whitespace is required after name in DTD parameter entity declaration.");
      if (this.PeekChar() == 83 || this.PeekChar() == 80)
      {
        this.ReadExternalID();
        decl.PublicId = this.cachedPublicId;
        decl.SystemId = this.cachedSystemId;
        this.SkipWhitespace();
        decl.Resolve();
        this.ResolveExternalEntityReplacementText((DTDEntityBase) decl);
      }
      else
      {
        this.TryExpandPERef();
        int num = this.ReadChar();
        switch (num)
        {
          case 34:
          case 39:
            this.ClearValueBuffer();
            bool flag = true;
            while (flag)
            {
              int ch = this.ReadChar();
              switch (ch)
              {
                case -1:
                  throw this.NotWFError("unexpected end of stream in entity value definition.");
                case 34:
                  if (num == 34)
                  {
                    flag = false;
                    continue;
                  }
                  this.AppendValueChar(34);
                  continue;
                case 39:
                  if (num == 39)
                  {
                    flag = false;
                    continue;
                  }
                  this.AppendValueChar(39);
                  continue;
                default:
                  if (XmlChar.IsInvalid(ch))
                    throw this.NotWFError("Invalid character was used to define parameter entity.");
                  this.AppendValueChar(ch);
                  continue;
              }
            }
            decl.LiteralEntityValue = this.CreateValueString();
            this.ClearValueBuffer();
            this.ResolveInternalEntityReplacementText((DTDEntityBase) decl);
            break;
          default:
            throw this.NotWFError("quotation char was expected.");
        }
      }
      this.ExpectAfterWhitespace('>');
      if (this.DTD.PEDecls[decl.Name] != null)
        return;
      this.DTD.PEDecls.Add(decl.Name, decl);
    }

    private void ResolveExternalEntityReplacementText(DTDEntityBase decl)
    {
      if (decl.SystemId != null && decl.SystemId.Length > 0)
      {
        XmlTextReader xmlTextReader = new XmlTextReader(decl.LiteralEntityValue, XmlNodeType.Element, (XmlParserContext) null);
        xmlTextReader.SkipTextDeclaration();
        if (decl is DTDEntityDeclaration && this.DTD.EntityDecls[decl.Name] == null)
        {
          StringBuilder stringBuilder = new StringBuilder();
          xmlTextReader.Normalization = this.Normalization;
          xmlTextReader.Read();
          while (!xmlTextReader.EOF)
            stringBuilder.Append(xmlTextReader.ReadOuterXml());
          decl.ReplacementText = stringBuilder.ToString();
        }
        else
          decl.ReplacementText = xmlTextReader.GetRemainder().ReadToEnd();
      }
      else
        decl.ReplacementText = decl.LiteralEntityValue;
    }

    private void ResolveInternalEntityReplacementText(DTDEntityBase decl)
    {
      string literalEntityValue = decl.LiteralEntityValue;
      int length = literalEntityValue.Length;
      this.ClearValueBuffer();
      for (int index = 0; index < length; ++index)
      {
        int ch = (int) literalEntityValue[index];
        switch (ch)
        {
          case 37:
            ++index;
            int num = literalEntityValue.IndexOf(';', index);
            if (num < index + 1)
              throw new XmlException((IXmlLineInfo) decl, decl.BaseURI, "Invalid reference markup.");
            this.valueBuffer.Append(this.GetPEValue(literalEntityValue.Substring(index, num - index)));
            index = num;
            break;
          case 38:
            ++index;
            int end = literalEntityValue.IndexOf(';', index);
            if (end < index + 1)
              throw new XmlException((IXmlLineInfo) decl, decl.BaseURI, "Invalid reference markup.");
            if (literalEntityValue[index] == '#')
            {
              ++index;
              int characterReference = this.GetCharacterReference(decl, literalEntityValue, ref index, end);
              if (XmlChar.IsInvalid(characterReference))
                throw this.NotWFError("Invalid character was used to define parameter entity.");
              if (XmlChar.IsInvalid(characterReference))
                throw new XmlException((IXmlLineInfo) decl, decl.BaseURI, "Invalid character was found in the entity declaration.");
              this.AppendValueChar(characterReference);
              break;
            }
            string str = literalEntityValue.Substring(index, end - index);
            if (!XmlChar.IsName(str))
              throw this.NotWFError(string.Format("'{0}' is not a valid entity reference name.", (object) str));
            this.AppendValueChar(38);
            this.valueBuffer.Append(str);
            this.AppendValueChar(59);
            index = end;
            break;
          default:
            this.AppendValueChar(ch);
            break;
        }
      }
      decl.ReplacementText = this.CreateValueString();
      this.ClearValueBuffer();
    }

    private int GetCharacterReference(DTDEntityBase li, string value, ref int index, int end)
    {
      int characterReference;
      if (value[index] == 'x')
      {
        try
        {
          characterReference = int.Parse(value.Substring(index + 1, end - index - 1), NumberStyles.HexNumber, (IFormatProvider) CultureInfo.InvariantCulture);
        }
        catch (FormatException ex)
        {
          throw new XmlException((IXmlLineInfo) li, li.BaseURI, "Invalid number for a character reference.");
        }
      }
      else
      {
        try
        {
          characterReference = int.Parse(value.Substring(index, end - index), (IFormatProvider) CultureInfo.InvariantCulture);
        }
        catch (FormatException ex)
        {
          throw new XmlException((IXmlLineInfo) li, li.BaseURI, "Invalid number for a character reference.");
        }
      }
      index = end;
      return characterReference;
    }

    private string GetPEValue(string peName)
    {
      DTDParameterEntityDeclaration peDecl = this.GetPEDecl(peName);
      return peDecl != null ? peDecl.ReplacementText : string.Empty;
    }

    private DTDParameterEntityDeclaration GetPEDecl(string peName)
    {
      DTDParameterEntityDeclaration peDecl = this.DTD.PEDecls[peName];
      if (peDecl != null)
        return !peDecl.IsInternalSubset ? peDecl : throw this.NotWFError("Parameter entity is not allowed in internal subset entity '" + peName + "'");
      if (this.DTD.SystemId == null && !this.DTD.InternalSubsetHasPEReference || this.DTD.IsStandalone)
        throw this.NotWFError(string.Format("Parameter entity '{0}' not found.", (object) peName));
      this.HandleError(new XmlException("Parameter entity " + peName + " not found.", (Exception) null));
      return (DTDParameterEntityDeclaration) null;
    }

    private bool TryExpandPERef()
    {
      if (this.PeekChar() != 37)
        return false;
      while (this.PeekChar() == 37)
      {
        this.TryExpandPERefSpaceKeep();
        this.SkipWhitespace();
      }
      return true;
    }

    private bool TryExpandPERefSpaceKeep()
    {
      if (this.PeekChar() != 37)
        return false;
      if (this.processingInternalSubset)
        throw this.NotWFError("Parameter entity reference is not allowed inside internal subset.");
      this.ReadChar();
      this.ExpandPERef();
      return true;
    }

    private void ExpandPERef()
    {
      string name = this.ReadName();
      this.Expect(59);
      DTDParameterEntityDeclaration peDecl = this.DTD.PEDecls[name];
      if (peDecl == null)
        this.HandleError(new XmlException("Parameter entity " + name + " not found.", (Exception) null));
      else
        this.currentInput.PushPEBuffer(peDecl);
    }

    private DTDEntityDeclaration ReadEntityDecl()
    {
      DTDEntityDeclaration decl = new DTDEntityDeclaration(this.DTD);
      decl.BaseURI = this.BaseURI;
      decl.XmlResolver = this.DTD.Resolver;
      decl.IsInternalSubset = this.processingInternalSubset;
      this.TryExpandPERef();
      decl.Name = this.ReadName();
      if (!this.SkipWhitespace())
        throw this.NotWFError("Whitespace is required between name and content in DTD entity declaration.");
      this.TryExpandPERef();
      if (this.PeekChar() == 83 || this.PeekChar() == 80)
      {
        this.ReadExternalID();
        decl.PublicId = this.cachedPublicId;
        decl.SystemId = this.cachedSystemId;
        if (this.SkipWhitespace() && this.PeekChar() == 78)
        {
          this.Expect("NDATA");
          if (!this.SkipWhitespace())
            throw this.NotWFError("Whitespace is required after NDATA.");
          decl.NotationName = this.ReadName();
        }
        if (decl.NotationName == null)
        {
          decl.Resolve();
          this.ResolveExternalEntityReplacementText((DTDEntityBase) decl);
        }
        else
        {
          decl.LiteralEntityValue = string.Empty;
          decl.ReplacementText = string.Empty;
        }
      }
      else
      {
        this.ReadEntityValueDecl(decl);
        this.ResolveInternalEntityReplacementText((DTDEntityBase) decl);
      }
      this.SkipWhitespace();
      this.TryExpandPERef();
      this.Expect(62);
      return decl;
    }

    private void ReadEntityValueDecl(DTDEntityDeclaration decl)
    {
      this.SkipWhitespace();
      int expected = this.ReadChar();
      switch (expected)
      {
        case 34:
        case 39:
          this.ClearValueBuffer();
          while (this.PeekChar() != expected)
          {
            int ch = this.ReadChar();
            switch (ch)
            {
              case -1:
                throw this.NotWFError("unexpected end of stream.");
              case 37:
                string peName = this.ReadName();
                this.Expect(59);
                if (decl.IsInternalSubset)
                  throw this.NotWFError(string.Format("Parameter entity is not allowed in internal subset entity '{0}'", (object) peName));
                this.valueBuffer.Append(this.GetPEValue(peName));
                continue;
              default:
                if (this.normalization && XmlChar.IsInvalid(ch))
                  throw this.NotWFError("Invalid character was found in the entity declaration.");
                this.AppendValueChar(ch);
                continue;
            }
          }
          string valueString = this.CreateValueString();
          this.ClearValueBuffer();
          this.Expect(expected);
          decl.LiteralEntityValue = valueString;
          break;
        default:
          throw this.NotWFError("quotation char was expected.");
      }
    }

    private DTDAttListDeclaration ReadAttListDecl()
    {
      this.TryExpandPERefSpaceKeep();
      if (!this.SkipWhitespace())
        throw this.NotWFError("Whitespace is required between ATTLIST and name in DTD attlist declaration.");
      this.TryExpandPERef();
      string name = this.ReadName();
      DTDAttListDeclaration attListDeclaration = this.DTD.AttListDecls[name] ?? new DTDAttListDeclaration(this.DTD);
      attListDeclaration.IsInternalSubset = this.processingInternalSubset;
      attListDeclaration.Name = name;
      if (!this.SkipWhitespace() && this.PeekChar() != 62)
        throw this.NotWFError("Whitespace is required between name and content in non-empty DTD attlist declaration.");
      this.TryExpandPERef();
      while (XmlChar.IsNameChar(this.PeekChar()))
      {
        DTDAttributeDefinition def = this.ReadAttributeDefinition();
        if (def.Datatype.TokenizedType == XmlTokenizedType.ID)
        {
          for (int i = 0; i < attListDeclaration.Definitions.Count; ++i)
          {
            if (attListDeclaration[i].Datatype.TokenizedType == XmlTokenizedType.ID)
            {
              this.HandleError(new XmlException("AttList declaration must not contain two or more ID attributes.", def.LineNumber, def.LinePosition, (object) null, def.BaseURI, (Exception) null));
              break;
            }
          }
        }
        if (attListDeclaration[def.Name] == null)
          attListDeclaration.Add(def);
        this.SkipWhitespace();
        this.TryExpandPERef();
      }
      this.SkipWhitespace();
      this.TryExpandPERef();
      this.Expect(62);
      return attListDeclaration;
    }

    private DTDAttributeDefinition ReadAttributeDefinition() => throw new NotImplementedException();

    private void ReadAttributeDefaultValue(DTDAttributeDefinition def)
    {
      if (this.PeekChar() == 35)
      {
        this.ReadChar();
        int num = this.PeekChar();
        switch (num)
        {
          case 70:
            this.Expect("FIXED");
            def.OccurenceType = DTDAttributeOccurenceType.Fixed;
            if (!this.SkipWhitespace())
              throw this.NotWFError("Whitespace is required between FIXED and actual value in DTD attribute definition.");
            def.UnresolvedDefaultValue = this.ReadDefaultAttribute();
            break;
          case 73:
            this.Expect("IMPLIED");
            def.OccurenceType = DTDAttributeOccurenceType.Optional;
            break;
          default:
            if (num == 82)
            {
              this.Expect("REQUIRED");
              def.OccurenceType = DTDAttributeOccurenceType.Required;
              break;
            }
            break;
        }
      }
      else
      {
        this.SkipWhitespace();
        this.TryExpandPERef();
        def.UnresolvedDefaultValue = this.ReadDefaultAttribute();
      }
      if (def.DefaultValue != null)
      {
        string str = def.Datatype.Normalize(def.DefaultValue);
        bool flag = false;
        object obj = (object) null;
        if (def.EnumeratedAttributeDeclaration.Count > 0 && !def.EnumeratedAttributeDeclaration.Contains((object) str))
        {
          this.HandleError(new XmlException("Default value is not one of the enumerated values.", def.LineNumber, def.LinePosition, (object) null, def.BaseURI, (Exception) null));
          flag = true;
        }
        if (def.EnumeratedNotations.Count > 0 && !def.EnumeratedNotations.Contains((object) str))
        {
          this.HandleError(new XmlException("Default value is not one of the enumerated notation values.", def.LineNumber, def.LinePosition, (object) null, def.BaseURI, (Exception) null));
          flag = true;
        }
        if (!flag)
        {
          try
          {
            obj = def.Datatype.ParseValue(str, this.DTD.NameTable, (IXmlNamespaceResolver) null);
          }
          catch (Exception ex)
          {
            this.HandleError(new XmlException("Invalid default value for ENTITY type.", def.LineNumber, def.LinePosition, (object) null, def.BaseURI, ex));
            flag = true;
          }
        }
        if (!flag)
        {
          switch (def.Datatype.TokenizedType)
          {
            case XmlTokenizedType.ENTITY:
              if (this.DTD.EntityDecls[str] == null)
              {
                this.HandleError(new XmlException("Specified entity declaration used by default attribute value was not found.", def.LineNumber, def.LinePosition, (object) null, def.BaseURI, (Exception) null));
                break;
              }
              break;
            case XmlTokenizedType.ENTITIES:
              foreach (string name in obj as string[])
              {
                if (this.DTD.EntityDecls[name] == null)
                  this.HandleError(new XmlException("Specified entity declaration used by default attribute value was not found.", def.LineNumber, def.LinePosition, (object) null, def.BaseURI, (Exception) null));
              }
              break;
          }
        }
      }
      if (def.Datatype == null || def.Datatype.TokenizedType != XmlTokenizedType.ID || def.UnresolvedDefaultValue == null)
        return;
      this.HandleError(new XmlException("ID attribute must not have fixed value constraint.", def.LineNumber, def.LinePosition, (object) null, def.BaseURI, (Exception) null));
    }

    private DTDNotationDeclaration ReadNotationDecl()
    {
      DTDNotationDeclaration notationDeclaration = new DTDNotationDeclaration(this.DTD);
      if (!this.SkipWhitespace())
        throw this.NotWFError("Whitespace is required between NOTATION and name in DTD notation declaration.");
      this.TryExpandPERef();
      notationDeclaration.Name = this.ReadName();
      notationDeclaration.Prefix = string.Empty;
      notationDeclaration.LocalName = notationDeclaration.Name;
      this.SkipWhitespace();
      if (this.PeekChar() == 80)
      {
        notationDeclaration.PublicId = this.ReadPubidLiteral();
        bool flag = this.SkipWhitespace();
        if (this.PeekChar() == 39 || this.PeekChar() == 34)
        {
          if (!flag)
            throw this.NotWFError("Whitespace is required between public id and system id.");
          notationDeclaration.SystemId = this.ReadSystemLiteral(false);
          this.SkipWhitespace();
        }
      }
      else if (this.PeekChar() == 83)
      {
        notationDeclaration.SystemId = this.ReadSystemLiteral(true);
        this.SkipWhitespace();
      }
      if (notationDeclaration.PublicId == null && notationDeclaration.SystemId == null)
        throw this.NotWFError("public or system declaration required for \"NOTATION\" declaration.");
      this.TryExpandPERef();
      this.Expect(62);
      return notationDeclaration;
    }

    private void ReadExternalID()
    {
      switch (this.PeekChar())
      {
        case 80:
          this.cachedPublicId = this.ReadPubidLiteral();
          if (!this.SkipWhitespace())
            throw this.NotWFError("Whitespace is required between PUBLIC id and SYSTEM id.");
          this.cachedSystemId = this.ReadSystemLiteral(false);
          break;
        case 83:
          this.cachedSystemId = this.ReadSystemLiteral(true);
          break;
      }
    }

    private string ReadSystemLiteral(bool expectSYSTEM)
    {
      if (expectSYSTEM)
      {
        this.Expect("SYSTEM");
        if (!this.SkipWhitespace())
          throw this.NotWFError("Whitespace is required after 'SYSTEM'.");
      }
      else
        this.SkipWhitespace();
      int num = this.ReadChar();
      int ch = 0;
      this.ClearValueBuffer();
      while (ch != num)
      {
        ch = this.ReadChar();
        if (ch < 0)
          throw this.NotWFError("Unexpected end of stream in ExternalID.");
        if (ch != num)
          this.AppendValueChar(ch);
      }
      return this.CreateValueString();
    }

    private string ReadPubidLiteral()
    {
      this.Expect("PUBLIC");
      if (!this.SkipWhitespace())
        throw this.NotWFError("Whitespace is required after 'PUBLIC'.");
      int num = this.ReadChar();
      int ch = 0;
      this.ClearValueBuffer();
      while (ch != num)
      {
        ch = this.ReadChar();
        if (ch < 0)
          throw this.NotWFError("Unexpected end of stream in ExternalID.");
        if (ch != num && !XmlChar.IsPubidChar(ch))
          throw this.NotWFError(string.Format("character '{0}' not allowed for PUBLIC ID", (object) (char) ch));
        if (ch != num)
          this.AppendValueChar(ch);
      }
      return this.CreateValueString();
    }

    internal string ReadName() => this.ReadNameOrNmToken(false);

    private string ReadNmToken() => this.ReadNameOrNmToken(true);

    private string ReadNameOrNmToken(bool isNameToken)
    {
      int ch = this.PeekChar();
      if (isNameToken)
      {
        if (!XmlChar.IsNameChar(ch))
          throw this.NotWFError(string.Format("a nmtoken did not start with a legal character {0} ({1})", (object) ch, (object) (char) ch));
      }
      else if (!XmlChar.IsFirstNameChar(ch))
        throw this.NotWFError(string.Format("a name did not start with a legal character {0} ({1})", (object) ch, (object) (char) ch));
      this.nameLength = 0;
      this.AppendNameChar(this.ReadChar());
      while (XmlChar.IsNameChar(this.PeekChar()))
        this.AppendNameChar(this.ReadChar());
      return this.CreateNameString();
    }

    private void Expect(int expected)
    {
      int num = this.ReadChar();
      if (num != expected)
        throw this.NotWFError(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "expected '{0}' ({1:X}) but found '{2}' ({3:X})", (object) (char) expected, (object) expected, (object) (char) num, (object) num));
    }

    private void Expect(string expected)
    {
      int length = expected.Length;
      for (int index = 0; index < length; ++index)
        this.Expect((int) expected[index]);
    }

    private void ExpectAfterWhitespace(char c)
    {
      int ch;
      do
      {
        ch = this.ReadChar();
      }
      while (XmlChar.IsWhitespace(ch));
      if ((int) c != ch)
        throw this.NotWFError(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "Expected {0} but found {1} [{2}].", (object) c, (object) (char) ch, (object) ch));
    }

    private bool SkipWhitespace()
    {
      bool flag = XmlChar.IsWhitespace(this.PeekChar());
      while (XmlChar.IsWhitespace(this.PeekChar()))
        this.ReadChar();
      return flag;
    }

    private int PeekChar() => this.currentInput.PeekChar();

    private int ReadChar() => this.currentInput.ReadChar();

    private void ReadComment()
    {
      this.currentInput.AllowTextDecl = false;
      while (this.PeekChar() != -1)
      {
        int ch = this.ReadChar();
        if (ch == 45 && this.PeekChar() == 45)
        {
          this.ReadChar();
          if (this.PeekChar() != 62)
            throw this.NotWFError("comments cannot contain '--'");
          this.ReadChar();
          break;
        }
        if (XmlChar.IsInvalid(ch))
          throw this.NotWFError("Not allowed character was found.");
      }
    }

    private void ReadProcessingInstruction()
    {
      string string1 = this.ReadName();
      if (string1 == "xml")
      {
        this.ReadTextDeclaration();
      }
      else
      {
        if (CultureInfo.InvariantCulture.CompareInfo.Compare(string1, "xml", CompareOptions.IgnoreCase) == 0)
          throw this.NotWFError("Not allowed processing instruction name which starts with 'X', 'M', 'L' was found.");
        this.currentInput.AllowTextDecl = false;
        if (!this.SkipWhitespace() && this.PeekChar() != 63)
          throw this.NotWFError("Invalid processing instruction name was found.");
        while (this.PeekChar() != -1)
        {
          if (this.ReadChar() == 63 && this.PeekChar() == 62)
          {
            this.ReadChar();
            break;
          }
        }
      }
    }

    private void ReadTextDeclaration()
    {
      this.currentInput.AllowTextDecl = this.currentInput.AllowTextDecl ? false : throw this.NotWFError("Text declaration cannot appear in this state.");
      this.SkipWhitespace();
      if (this.PeekChar() == 118)
      {
        this.Expect("version");
        this.ExpectAfterWhitespace('=');
        this.SkipWhitespace();
        int num = this.ReadChar();
        char[] chArray = new char[3];
        int index = 0;
        switch (num)
        {
          case 34:
          case 39:
            while (this.PeekChar() != num)
            {
              if (this.PeekChar() == -1)
                throw this.NotWFError("Invalid version declaration inside text declaration.");
              if (index == 3)
                throw this.NotWFError("Invalid version number inside text declaration.");
              chArray[index] = (char) this.ReadChar();
              ++index;
              if (index == 3 && new string(chArray) != "1.0")
                throw this.NotWFError("Invalid version number inside text declaration.");
            }
            this.ReadChar();
            this.SkipWhitespace();
            break;
          default:
            throw this.NotWFError("Invalid version declaration inside text declaration.");
        }
      }
      if (this.PeekChar() != 101)
        throw this.NotWFError("Encoding declaration is mandatory in text declaration.");
      this.Expect("encoding");
      this.ExpectAfterWhitespace('=');
      this.SkipWhitespace();
      int num1 = this.ReadChar();
      switch (num1)
      {
        case 34:
        case 39:
          while (this.PeekChar() != num1)
          {
            if (this.ReadChar() == -1)
              throw this.NotWFError("Invalid encoding declaration inside text declaration.");
          }
          this.ReadChar();
          this.SkipWhitespace();
          this.Expect("?>");
          break;
        default:
          throw this.NotWFError("Invalid encoding declaration inside text declaration.");
      }
    }

    private void AppendNameChar(int ch)
    {
      this.CheckNameCapacity();
      if (ch < (int) ushort.MaxValue)
      {
        this.nameBuffer[this.nameLength++] = (char) ch;
      }
      else
      {
        this.nameBuffer[this.nameLength++] = (char) (ch / 65536 + 55296 - 1);
        this.CheckNameCapacity();
        this.nameBuffer[this.nameLength++] = (char) (ch % 65536 + 56320);
      }
    }

    private void CheckNameCapacity()
    {
      if (this.nameLength != this.nameCapacity)
        return;
      this.nameCapacity *= 2;
      char[] nameBuffer = this.nameBuffer;
      this.nameBuffer = new char[this.nameCapacity];
      Array.Copy((Array) nameBuffer, (Array) this.nameBuffer, this.nameLength);
    }

    private string CreateNameString() => this.DTD.NameTable.Add(this.nameBuffer, 0, this.nameLength);

    private void AppendValueChar(int ch)
    {
      if (ch < 65536)
      {
        this.valueBuffer.Append((char) ch);
      }
      else
      {
        if (ch > 1114111)
          throw new XmlException("The numeric entity value is too large", (Exception) null, this.LineNumber, this.LinePosition);
        int num = ch - 65536;
        this.valueBuffer.Append((char) ((num >> 10) + 55296));
        this.valueBuffer.Append((char) ((num & 1023) + 56320));
      }
    }

    private string CreateValueString() => this.valueBuffer.ToString();

    private void ClearValueBuffer() => this.valueBuffer.Length = 0;

    private string ReadDefaultAttribute()
    {
      this.ClearValueBuffer();
      this.TryExpandPERef();
      int ch1 = this.ReadChar();
      switch (ch1)
      {
        case 34:
        case 39:
          this.AppendValueChar(ch1);
          while (this.PeekChar() != ch1)
          {
            int ch2 = this.ReadChar();
            switch (ch2)
            {
              case -1:
                throw this.NotWFError("unexpected end of file in an attribute value");
              case 38:
                this.AppendValueChar(ch2);
                if (this.PeekChar() != 35)
                {
                  string name = this.ReadName();
                  this.Expect(59);
                  if (XmlChar.GetPredefinedEntity(name) < 0)
                  {
                    DTDEntityDeclaration entityDecl = this.DTD != null ? this.DTD.EntityDecls[name] : (DTDEntityDeclaration) null;
                    if ((entityDecl == null || entityDecl.SystemId != null) && (this.DTD.IsStandalone || this.DTD.SystemId == null && !this.DTD.InternalSubsetHasPEReference))
                      throw this.NotWFError("Reference to external entities is not allowed in attribute value.");
                  }
                  this.valueBuffer.Append(name);
                  this.AppendValueChar(59);
                  continue;
                }
                continue;
              case 60:
                throw this.NotWFError("attribute values cannot contain '<'");
              default:
                this.AppendValueChar(ch2);
                continue;
            }
          }
          this.ReadChar();
          this.AppendValueChar(ch1);
          return this.CreateValueString();
        default:
          throw this.NotWFError("an attribute value was not quoted");
      }
    }

    private void PushParserInput(string url)
    {
      Uri baseUri = (Uri) null;
      try
      {
        if (this.DTD.BaseURI != null)
        {
          if (this.DTD.BaseURI.Length > 0)
            baseUri = new Uri(this.DTD.BaseURI);
        }
      }
      catch (UriFormatException ex)
      {
      }
      Uri absoluteUri = url == null || url.Length <= 0 ? baseUri : this.DTD.Resolver.ResolveUri(baseUri, url);
      string baseURI = !(absoluteUri != (Uri) null) ? string.Empty : absoluteUri.ToString();
      foreach (XmlParserInput xmlParserInput in this.parserInputStack.ToArray())
      {
        if (xmlParserInput.BaseURI == baseURI)
          throw this.NotWFError("Nested inclusion is not allowed: " + url);
      }
      this.parserInputStack.Push((object) this.currentInput);
      Stream stream = (Stream) null;
      MemoryStream input = new MemoryStream();
      try
      {
        stream = this.DTD.Resolver.GetEntity(absoluteUri, (string) null, typeof (Stream)) as Stream;
        byte[] buffer = new byte[4096];
        int count;
        do
        {
          count = stream.Read(buffer, 0, buffer.Length);
          input.Write(buffer, 0, count);
        }
        while (count > 0);
        stream.Close();
        input.Position = 0L;
        this.currentInput = new XmlParserInput((TextReader) new XmlStreamReader((Stream) input), baseURI);
      }
      catch (Exception ex)
      {
        stream?.Close();
        int lineNumber = this.currentInput != null ? this.currentInput.LineNumber : 0;
        int linePosition = this.currentInput != null ? this.currentInput.LinePosition : 0;
        string sourceUri = this.currentInput != null ? this.currentInput.BaseURI : string.Empty;
        this.HandleError(new XmlException("Specified external entity not found. Target URL is " + url + " .", lineNumber, linePosition, (object) null, sourceUri, ex));
        this.currentInput = new XmlParserInput((TextReader) new StringReader(string.Empty), baseURI);
      }
    }

    private void PopParserInput()
    {
      this.currentInput.Close();
      this.currentInput = this.parserInputStack.Pop() as XmlParserInput;
    }

    private void HandleError(XmlException ex) => this.DTD.AddError(ex);
  }
}
