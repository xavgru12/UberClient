﻿// Decompiled with JetBrains decompiler
// Type: Mono.Xml.XPath.XmlDocumentEditableNavigator
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.XPath;

namespace Mono.Xml.XPath
{
  internal class XmlDocumentEditableNavigator : XPathNavigator, IHasXmlNode
  {
    private static readonly bool isXmlDocumentNavigatorImpl = typeof (XmlDocumentEditableNavigator).Assembly == typeof (XmlDocument).Assembly;
    private XPathEditableDocument document;
    private XPathNavigator navigator;

    public XmlDocumentEditableNavigator(XPathEditableDocument doc)
    {
      this.document = doc;
      if (XmlDocumentEditableNavigator.isXmlDocumentNavigatorImpl)
        this.navigator = (XPathNavigator) new XmlDocumentNavigator(doc.Node);
      else
        this.navigator = doc.CreateNavigator();
    }

    public XmlDocumentEditableNavigator(XmlDocumentEditableNavigator nav)
    {
      this.document = nav.document;
      this.navigator = nav.navigator.Clone();
    }

    public override string BaseURI => this.navigator.BaseURI;

    public override bool CanEdit => true;

    public override bool IsEmptyElement => this.navigator.IsEmptyElement;

    public override string LocalName => this.navigator.LocalName;

    public override XmlNameTable NameTable => this.navigator.NameTable;

    public override string Name => this.navigator.Name;

    public override string NamespaceURI => this.navigator.NamespaceURI;

    public override XPathNodeType NodeType => this.navigator.NodeType;

    public override string Prefix => this.navigator.Prefix;

    public override IXmlSchemaInfo SchemaInfo => this.navigator.SchemaInfo;

    public override object UnderlyingObject => this.navigator.UnderlyingObject;

    public override string Value => this.navigator.Value;

    public override string XmlLang => this.navigator.XmlLang;

    public override bool HasChildren => this.navigator.HasChildren;

    public override bool HasAttributes => this.navigator.HasAttributes;

    public override XPathNavigator Clone() => (XPathNavigator) new XmlDocumentEditableNavigator(this);

    public override XPathNavigator CreateNavigator() => this.navigator.Clone();

    public XmlNode GetNode() => ((IHasXmlNode) this.navigator).GetNode();

    public override bool IsSamePosition(XPathNavigator other) => other is XmlDocumentEditableNavigator other1 ? this.navigator.IsSamePosition(other1.navigator) : this.navigator.IsSamePosition((XPathNavigator) other1);

    public override bool MoveTo(XPathNavigator other) => other is XmlDocumentEditableNavigator other1 ? this.navigator.MoveTo(other1.navigator) : this.navigator.MoveTo((XPathNavigator) other1);

    public override bool MoveToFirstAttribute() => this.navigator.MoveToFirstAttribute();

    public override bool MoveToFirstChild() => this.navigator.MoveToFirstChild();

    public override bool MoveToFirstNamespace(XPathNamespaceScope scope) => this.navigator.MoveToFirstNamespace(scope);

    public override bool MoveToId(string id) => this.navigator.MoveToId(id);

    public override bool MoveToNext() => this.navigator.MoveToNext();

    public override bool MoveToNextAttribute() => this.navigator.MoveToNextAttribute();

    public override bool MoveToNextNamespace(XPathNamespaceScope scope) => this.navigator.MoveToNextNamespace(scope);

    public override bool MoveToParent() => this.navigator.MoveToParent();

    public override bool MoveToPrevious() => this.navigator.MoveToPrevious();

    public override XmlWriter AppendChild() => (XmlWriter) new XmlDocumentInsertionWriter(((IHasXmlNode) this.navigator).GetNode() ?? throw new InvalidOperationException("Should not happen."), (XmlNode) null);

    public override void DeleteRange(XPathNavigator lastSiblingToDelete)
    {
      if (lastSiblingToDelete == null)
        throw new ArgumentNullException();
      XmlNode node = ((IHasXmlNode) this.navigator).GetNode();
      XmlNode xmlNode = (XmlNode) null;
      if (lastSiblingToDelete is IHasXmlNode)
        xmlNode = ((IHasXmlNode) lastSiblingToDelete).GetNode();
      if (!this.navigator.MoveToParent())
        throw new InvalidOperationException("There is no parent to remove current node.");
      if (xmlNode == null || node.ParentNode != xmlNode.ParentNode)
        throw new InvalidOperationException("Argument XPathNavigator has different parent node.");
      XmlNode parentNode = node.ParentNode;
      bool flag = true;
      XmlNode oldChild = node;
      while (flag)
      {
        flag = oldChild != xmlNode;
        XmlNode nextSibling = oldChild.NextSibling;
        parentNode.RemoveChild(oldChild);
        oldChild = nextSibling;
      }
    }

    public override XmlWriter ReplaceRange(XPathNavigator nav)
    {
      if (nav == null)
        throw new ArgumentNullException();
      XmlNode start = ((IHasXmlNode) this.navigator).GetNode();
      XmlNode end = (XmlNode) null;
      if (nav is IHasXmlNode)
        end = ((IHasXmlNode) nav).GetNode();
      if (end == null || start.ParentNode != end.ParentNode)
        throw new InvalidOperationException("Argument XPathNavigator has different parent node.");
      XmlDocumentInsertionWriter documentInsertionWriter = (XmlDocumentInsertionWriter) this.InsertBefore();
      XPathNavigator prev = this.Clone();
      if (!prev.MoveToPrevious())
        prev = (XPathNavigator) null;
      XPathNavigator parentNav = this.Clone();
      parentNav.MoveToParent();
      documentInsertionWriter.Closed += (XmlWriterClosedEventHandler) (writer =>
      {
        XmlNode parentNode = start.ParentNode;
        bool flag = true;
        XmlNode oldChild = start;
        while (flag)
        {
          flag = oldChild != end;
          XmlNode nextSibling = oldChild.NextSibling;
          parentNode.RemoveChild(oldChild);
          oldChild = nextSibling;
        }
        if (prev != null)
        {
          this.MoveTo(prev);
          this.MoveToNext();
        }
        else
        {
          this.MoveTo(parentNav);
          this.MoveToFirstChild();
        }
      });
      return (XmlWriter) documentInsertionWriter;
    }

    public override XmlWriter InsertBefore()
    {
      XmlNode node = ((IHasXmlNode) this.navigator).GetNode();
      return (XmlWriter) new XmlDocumentInsertionWriter(node.ParentNode, node);
    }

    public override XmlWriter CreateAttributes() => (XmlWriter) new XmlDocumentAttributeWriter(((IHasXmlNode) this.navigator).GetNode());

    public override void DeleteSelf()
    {
      XmlNode node = ((IHasXmlNode) this.navigator).GetNode();
      if (node is XmlAttribute oldAttr)
      {
        if (oldAttr.OwnerElement == null)
          throw new InvalidOperationException("This attribute node cannot be removed since it has no owner element.");
        this.navigator.MoveToParent();
        oldAttr.OwnerElement.RemoveAttributeNode(oldAttr);
      }
      else
      {
        if (node.ParentNode == null)
          throw new InvalidOperationException("This node cannot be removed since it has no parent.");
        this.navigator.MoveToParent();
        node.ParentNode.RemoveChild(node);
      }
    }

    public override void ReplaceSelf(XmlReader reader)
    {
      XmlNode node = ((IHasXmlNode) this.navigator).GetNode();
      XmlNode parentNode = node.ParentNode;
      if (parentNode == null)
        throw new InvalidOperationException("This node cannot be removed since it has no parent.");
      bool flag1 = false;
      if (!this.MoveToPrevious())
        this.MoveToParent();
      else
        flag1 = true;
      XmlDocument xmlDocument = parentNode.NodeType != XmlNodeType.Document ? parentNode.OwnerDocument : parentNode as XmlDocument;
      bool flag2 = false;
      if (reader.ReadState == ReadState.Initial)
      {
        reader.Read();
        if (reader.EOF)
        {
          flag2 = true;
        }
        else
        {
          while (!reader.EOF)
            parentNode.AppendChild(xmlDocument.ReadNode(reader));
        }
      }
      else if (reader.EOF)
        flag2 = true;
      else
        parentNode.AppendChild(xmlDocument.ReadNode(reader));
      if (flag2)
        throw new InvalidOperationException("Content is required in argument XmlReader to replace current node.");
      parentNode.RemoveChild(node);
      if (flag1)
        this.MoveToNext();
      else
        this.MoveToFirstChild();
    }

    public override void SetValue(string value)
    {
      XmlNode node = ((IHasXmlNode) this.navigator).GetNode();
      while (node.FirstChild != null)
        node.RemoveChild(node.FirstChild);
      node.InnerText = value;
    }

    public override void MoveToRoot() => this.navigator.MoveToRoot();

    public override bool MoveToNamespace(string name) => this.navigator.MoveToNamespace(name);

    public override bool MoveToFirst() => this.navigator.MoveToFirst();

    public override bool MoveToAttribute(string localName, string namespaceURI) => this.navigator.MoveToAttribute(localName, namespaceURI);

    public override bool IsDescendant(XPathNavigator nav) => nav is XmlDocumentEditableNavigator editableNavigator ? this.navigator.IsDescendant(editableNavigator.navigator) : this.navigator.IsDescendant(nav);

    public override string GetNamespace(string name) => this.navigator.GetNamespace(name);

    public override string GetAttribute(string localName, string namespaceURI) => this.navigator.GetAttribute(localName, namespaceURI);

    public override XmlNodeOrder ComparePosition(XPathNavigator nav) => nav is XmlDocumentEditableNavigator editableNavigator ? this.navigator.ComparePosition(editableNavigator.navigator) : this.navigator.ComparePosition(nav);
  }
}
