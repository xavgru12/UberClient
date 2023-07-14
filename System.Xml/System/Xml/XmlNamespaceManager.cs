// Decompiled with JetBrains decompiler
// Type: System.Xml.XmlNamespaceManager
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections;
using System.Collections.Generic;

namespace System.Xml
{
  public class XmlNamespaceManager : IEnumerable, IXmlNamespaceResolver
  {
    internal const string XmlnsXml = "http://www.w3.org/XML/1998/namespace";
    internal const string XmlnsXmlns = "http://www.w3.org/2000/xmlns/";
    internal const string PrefixXml = "xml";
    internal const string PrefixXmlns = "xmlns";
    private XmlNamespaceManager.NsDecl[] decls;
    private int declPos = -1;
    private XmlNamespaceManager.NsScope[] scopes;
    private int scopePos = -1;
    private string defaultNamespace;
    private int count;
    private XmlNameTable nameTable;
    internal bool internalAtomizedNames;

    public XmlNamespaceManager(XmlNameTable nameTable)
    {
      this.nameTable = nameTable != null ? nameTable : throw new ArgumentNullException(nameof (nameTable));
      nameTable.Add("xmlns");
      nameTable.Add("xml");
      nameTable.Add(string.Empty);
      nameTable.Add("http://www.w3.org/2000/xmlns/");
      nameTable.Add("http://www.w3.org/XML/1998/namespace");
      this.InitData();
    }

    private void InitData()
    {
      this.decls = new XmlNamespaceManager.NsDecl[10];
      this.scopes = new XmlNamespaceManager.NsScope[40];
    }

    private void GrowDecls()
    {
      XmlNamespaceManager.NsDecl[] decls = this.decls;
      this.decls = new XmlNamespaceManager.NsDecl[this.declPos * 2 + 1];
      if (this.declPos <= 0)
        return;
      Array.Copy((Array) decls, 0, (Array) this.decls, 0, this.declPos);
    }

    private void GrowScopes()
    {
      XmlNamespaceManager.NsScope[] scopes = this.scopes;
      this.scopes = new XmlNamespaceManager.NsScope[this.scopePos * 2 + 1];
      if (this.scopePos <= 0)
        return;
      Array.Copy((Array) scopes, 0, (Array) this.scopes, 0, this.scopePos);
    }

    public virtual string DefaultNamespace => this.defaultNamespace == null ? string.Empty : this.defaultNamespace;

    public virtual XmlNameTable NameTable => this.nameTable;

    public virtual void AddNamespace(string prefix, string uri) => this.AddNamespace(prefix, uri, false);

    private void AddNamespace(string prefix, string uri, bool atomizedNames)
    {
      if (prefix == null)
        throw new ArgumentNullException(nameof (prefix), "Value cannot be null.");
      if (uri == null)
        throw new ArgumentNullException(nameof (uri), "Value cannot be null.");
      if (!atomizedNames)
      {
        prefix = this.nameTable.Add(prefix);
        uri = this.nameTable.Add(uri);
      }
      if (prefix == "xml" && uri == "http://www.w3.org/XML/1998/namespace")
        return;
      XmlNamespaceManager.IsValidDeclaration(prefix, uri, true);
      if (prefix.Length == 0)
        this.defaultNamespace = uri;
      for (int declPos = this.declPos; declPos > this.declPos - this.count; --declPos)
      {
        if (object.ReferenceEquals((object) this.decls[declPos].Prefix, (object) prefix))
        {
          this.decls[declPos].Uri = uri;
          return;
        }
      }
      ++this.declPos;
      ++this.count;
      if (this.declPos == this.decls.Length)
        this.GrowDecls();
      this.decls[this.declPos].Prefix = prefix;
      this.decls[this.declPos].Uri = uri;
    }

    private static string IsValidDeclaration(string prefix, string uri, bool throwException)
    {
      string message = (string) null;
      if (prefix == "xml" && uri != "http://www.w3.org/XML/1998/namespace")
        message = string.Format("Prefix \"xml\" can only be bound to the fixed namespace URI \"{0}\". \"{1}\" is invalid.", (object) "http://www.w3.org/XML/1998/namespace", (object) uri);
      else if (message == null && prefix == "xmlns")
        message = "Declaring prefix named \"xmlns\" is not allowed to any namespace.";
      else if (message == null && uri == "http://www.w3.org/2000/xmlns/")
        message = string.Format("Namespace URI \"{0}\" cannot be declared with any namespace.", (object) "http://www.w3.org/2000/xmlns/");
      return message == null || !throwException ? message : throw new ArgumentException(message);
    }

    public virtual IEnumerator GetEnumerator()
    {
      Hashtable hashtable = new Hashtable();
      for (int index = 0; index <= this.declPos; ++index)
      {
        if (this.decls[index].Prefix != string.Empty && this.decls[index].Uri != null)
          hashtable[(object) this.decls[index].Prefix] = (object) this.decls[index].Uri;
      }
      hashtable[(object) string.Empty] = (object) this.DefaultNamespace;
      hashtable[(object) "xml"] = (object) "http://www.w3.org/XML/1998/namespace";
      hashtable[(object) "xmlns"] = (object) "http://www.w3.org/2000/xmlns/";
      return hashtable.Keys.GetEnumerator();
    }

    public virtual IDictionary<string, string> GetNamespacesInScope(XmlNamespaceScope scope)
    {
      IDictionary namespacesInScopeImpl = this.GetNamespacesInScopeImpl(scope);
      IDictionary<string, string> namespacesInScope = (IDictionary<string, string>) new Dictionary<string, string>(namespacesInScopeImpl.Count);
      foreach (DictionaryEntry dictionaryEntry in namespacesInScopeImpl)
        namespacesInScope[(string) dictionaryEntry.Key] = (string) dictionaryEntry.Value;
      return namespacesInScope;
    }

    internal virtual IDictionary GetNamespacesInScopeImpl(XmlNamespaceScope scope)
    {
      Hashtable namespacesInScopeImpl = new Hashtable();
      if (scope == XmlNamespaceScope.Local)
      {
        for (int index = 0; index < this.count; ++index)
        {
          if (this.decls[this.declPos - index].Prefix == string.Empty && this.decls[this.declPos - index].Uri == string.Empty)
          {
            if (namespacesInScopeImpl.Contains((object) string.Empty))
              namespacesInScopeImpl.Remove((object) string.Empty);
          }
          else if (this.decls[this.declPos - index].Uri != null)
            namespacesInScopeImpl.Add((object) this.decls[this.declPos - index].Prefix, (object) this.decls[this.declPos - index].Uri);
        }
        return (IDictionary) namespacesInScopeImpl;
      }
      for (int index = 0; index <= this.declPos; ++index)
      {
        if (this.decls[index].Prefix == string.Empty && this.decls[index].Uri == string.Empty)
        {
          if (namespacesInScopeImpl.Contains((object) string.Empty))
            namespacesInScopeImpl.Remove((object) string.Empty);
        }
        else if (this.decls[index].Uri != null)
          namespacesInScopeImpl[(object) this.decls[index].Prefix] = (object) this.decls[index].Uri;
      }
      if (scope == XmlNamespaceScope.All)
        namespacesInScopeImpl.Add((object) "xml", (object) "http://www.w3.org/XML/1998/namespace");
      return (IDictionary) namespacesInScopeImpl;
    }

    public virtual bool HasNamespace(string prefix) => this.HasNamespace(prefix, false);

    private bool HasNamespace(string prefix, bool atomizedNames)
    {
      if (prefix == null || this.count == 0)
        return false;
      for (int declPos = this.declPos; declPos > this.declPos - this.count; --declPos)
      {
        if (this.decls[declPos].Prefix == prefix)
          return true;
      }
      return false;
    }

    public virtual string LookupNamespace(string prefix)
    {
      string key = prefix;
      if (key == null)
        return (string) null;
      // ISSUE: reference to a compiler-generated field
      if (XmlNamespaceManager.\u003C\u003Ef__switch\u0024map28 == null)
      {
        // ISSUE: reference to a compiler-generated field
        XmlNamespaceManager.\u003C\u003Ef__switch\u0024map28 = new Dictionary<string, int>(3)
        {
          {
            "xmlns",
            0
          },
          {
            "xml",
            1
          },
          {
            string.Empty,
            2
          }
        };
      }
      int num;
      // ISSUE: reference to a compiler-generated field
      if (XmlNamespaceManager.\u003C\u003Ef__switch\u0024map28.TryGetValue(key, out num))
      {
        switch (num)
        {
          case 0:
            return this.nameTable.Get("http://www.w3.org/2000/xmlns/");
          case 1:
            return this.nameTable.Get("http://www.w3.org/XML/1998/namespace");
          case 2:
            return this.DefaultNamespace;
        }
      }
      for (int declPos = this.declPos; declPos >= 0; --declPos)
      {
        if (this.CompareString(this.decls[declPos].Prefix, prefix, this.internalAtomizedNames) && this.decls[declPos].Uri != null)
          return this.decls[declPos].Uri;
      }
      return (string) null;
    }

    internal string LookupNamespace(string prefix, bool atomizedNames)
    {
      this.internalAtomizedNames = atomizedNames;
      string str = this.LookupNamespace(prefix);
      this.internalAtomizedNames = false;
      return str;
    }

    public virtual string LookupPrefix(string uri) => this.LookupPrefix(uri, false);

    private bool CompareString(string s1, string s2, bool atomizedNames) => atomizedNames ? object.ReferenceEquals((object) s1, (object) s2) : s1 == s2;

    internal string LookupPrefix(string uri, bool atomizedName) => this.LookupPrefixCore(uri, atomizedName, false);

    internal string LookupPrefixExclusive(string uri, bool atomizedName) => this.LookupPrefixCore(uri, atomizedName, true);

    private string LookupPrefixCore(string uri, bool atomizedName, bool excludeOverriden)
    {
      if (uri == null)
        return (string) null;
      if (this.CompareString(uri, this.DefaultNamespace, atomizedName))
        return string.Empty;
      if (this.CompareString(uri, "http://www.w3.org/XML/1998/namespace", atomizedName))
        return "xml";
      if (this.CompareString(uri, "http://www.w3.org/2000/xmlns/", atomizedName))
        return "xmlns";
      for (int declPos = this.declPos; declPos >= 0; --declPos)
      {
        if (this.CompareString(this.decls[declPos].Uri, uri, atomizedName) && this.decls[declPos].Prefix.Length > 0 && (!excludeOverriden || !this.IsOverriden(declPos)))
          return this.decls[declPos].Prefix;
      }
      return (string) null;
    }

    private bool IsOverriden(int idx)
    {
      if (idx == this.declPos)
        return false;
      string prefix = this.decls[idx + 1].Prefix;
      for (int index = idx + 1; index <= this.declPos; ++index)
      {
        if ((object) this.decls[idx].Prefix == (object) prefix)
          return true;
      }
      return false;
    }

    public virtual bool PopScope()
    {
      if (this.scopePos == -1)
        return false;
      this.declPos -= this.count;
      this.defaultNamespace = this.scopes[this.scopePos].DefaultNamespace;
      this.count = this.scopes[this.scopePos].DeclCount;
      --this.scopePos;
      return true;
    }

    public virtual void PushScope()
    {
      ++this.scopePos;
      if (this.scopePos == this.scopes.Length)
        this.GrowScopes();
      this.scopes[this.scopePos].DefaultNamespace = this.defaultNamespace;
      this.scopes[this.scopePos].DeclCount = this.count;
      this.count = 0;
    }

    public virtual void RemoveNamespace(string prefix, string uri) => this.RemoveNamespace(prefix, uri, false);

    private void RemoveNamespace(string prefix, string uri, bool atomizedNames)
    {
      if (prefix == null)
        throw new ArgumentNullException(nameof (prefix));
      if (uri == null)
        throw new ArgumentNullException(nameof (uri));
      if (this.count == 0)
        return;
      for (int declPos = this.declPos; declPos > this.declPos - this.count; --declPos)
      {
        if (this.CompareString(this.decls[declPos].Prefix, prefix, atomizedNames) && this.CompareString(this.decls[declPos].Uri, uri, atomizedNames))
          this.decls[declPos].Uri = (string) null;
      }
    }

    private struct NsDecl
    {
      public string Prefix;
      public string Uri;
    }

    private struct NsScope
    {
      public int DeclCount;
      public string DefaultNamespace;
    }
  }
}
