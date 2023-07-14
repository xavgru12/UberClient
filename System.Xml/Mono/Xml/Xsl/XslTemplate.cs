// Decompiled with JetBrains decompiler
// Type: Mono.Xml.Xsl.XslTemplate
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using Mono.Xml.XPath;
using Mono.Xml.Xsl.Operations;
using System;
using System.Collections;
using System.Globalization;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Mono.Xml.Xsl
{
  internal class XslTemplate
  {
    private XmlQualifiedName name;
    private Pattern match;
    private XmlQualifiedName mode;
    private double priority = double.NaN;
    private ArrayList parameters;
    private XslOperation content;
    private static int nextId;
    public readonly int Id = XslTemplate.nextId++;
    private XslStylesheet style;
    private int stackSize;

    public XslTemplate(Compiler c)
    {
      if (c == null)
        return;
      this.style = c.CurrentStylesheet;
      c.PushScope();
      if (c.Input.Name == "template" && c.Input.NamespaceURI == "http://www.w3.org/1999/XSL/Transform" && c.Input.MoveToAttribute(nameof (mode), string.Empty))
      {
        c.Input.MoveToParent();
        if (!c.Input.MoveToAttribute(nameof (match), string.Empty))
          throw new XsltCompileException("XSLT 'template' element must not have 'mode' attribute when it does not have 'match' attribute", (Exception) null, c.Input);
        c.Input.MoveToParent();
      }
      if (c.Input.NamespaceURI != "http://www.w3.org/1999/XSL/Transform")
      {
        this.name = XmlQualifiedName.Empty;
        this.match = c.CompilePattern("/", c.Input);
        this.mode = XmlQualifiedName.Empty;
      }
      else
      {
        this.name = c.ParseQNameAttribute(nameof (name));
        this.match = c.CompilePattern(c.GetAttribute(nameof (match)), c.Input);
        this.mode = c.ParseQNameAttribute(nameof (mode));
        string attribute = c.GetAttribute(nameof (priority));
        if (attribute != null)
        {
          try
          {
            this.priority = double.Parse(attribute, (IFormatProvider) CultureInfo.InvariantCulture);
          }
          catch (FormatException ex)
          {
            throw new XsltException("Invalid priority number format", (Exception) ex, c.Input);
          }
        }
      }
      this.Parse(c);
      this.stackSize = c.PopScope().VariableHighTide;
    }

    public XmlQualifiedName Name => this.name;

    public Pattern Match => this.match;

    public XmlQualifiedName Mode => this.mode;

    public double Priority => this.priority;

    public XslStylesheet Parent => this.style;

    private void Parse(Compiler c)
    {
      if (c.Input.NamespaceURI != "http://www.w3.org/1999/XSL/Transform")
      {
        this.content = c.CompileTemplateContent();
      }
      else
      {
        if (!c.Input.MoveToFirstChild())
          return;
        bool flag1 = true;
        XPathNavigator other = c.Input.Clone();
        bool flag2 = false;
        do
        {
          if (flag2)
          {
            flag2 = false;
            other.MoveTo(c.Input);
          }
          if (c.Input.NodeType == XPathNodeType.Text)
          {
            flag1 = false;
            break;
          }
          if (c.Input.NodeType == XPathNodeType.Element)
          {
            if (c.Input.NamespaceURI != "http://www.w3.org/1999/XSL/Transform")
            {
              flag1 = false;
              break;
            }
            if (c.Input.LocalName != "param")
            {
              flag1 = false;
              break;
            }
            if (this.parameters == null)
              this.parameters = new ArrayList();
            this.parameters.Add((object) new XslLocalParam(c));
            flag2 = true;
          }
        }
        while (c.Input.MoveToNext());
        if (!flag1)
        {
          c.Input.MoveTo(other);
          this.content = c.CompileTemplateContent();
        }
        c.Input.MoveToParent();
      }
    }

    private string LocationMessage
    {
      get
      {
        XslCompiledElementBase content = (XslCompiledElementBase) this.content;
        return string.Format(" from\nxsl:template {0} at {1} ({2},{3})", (object) this.Match, (object) this.style.BaseURI, (object) content.LineNumber, (object) content.LinePosition);
      }
    }

    private void AppendTemplateFrame(XsltException ex) => ex.AddTemplateFrame(this.LocationMessage);

    public virtual void Evaluate(XslTransformProcessor p, Hashtable withParams)
    {
      if (XslTransform.TemplateStackFrameError)
      {
        try
        {
          this.EvaluateCore(p, withParams);
        }
        catch (XsltException ex)
        {
          this.AppendTemplateFrame(ex);
          throw ex;
        }
        catch (Exception ex1)
        {
          XsltException ex2 = new XsltException("Error during XSLT processing: ", (Exception) null, p.CurrentNode);
          this.AppendTemplateFrame(ex2);
          throw ex2;
        }
      }
      else
        this.EvaluateCore(p, withParams);
    }

    private void EvaluateCore(XslTransformProcessor p, Hashtable withParams)
    {
      if (XslTransform.TemplateStackFrameOutput != null)
        XslTransform.TemplateStackFrameOutput.WriteLine(this.LocationMessage);
      p.PushStack(this.stackSize);
      if (this.parameters != null)
      {
        if (withParams == null)
        {
          int count = this.parameters.Count;
          for (int index = 0; index < count; ++index)
            ((XslLocalParam) this.parameters[index]).Evaluate(p);
        }
        else
        {
          int count = this.parameters.Count;
          for (int index = 0; index < count; ++index)
          {
            XslLocalParam parameter = (XslLocalParam) this.parameters[index];
            object withParam = withParams[(object) parameter.Name];
            if (withParam != null)
              parameter.Override(p, withParam);
            else
              parameter.Evaluate(p);
          }
        }
      }
      if (this.content != null)
        this.content.Evaluate(p);
      p.PopStack();
    }

    public void Evaluate(XslTransformProcessor p) => this.Evaluate(p, (Hashtable) null);
  }
}
