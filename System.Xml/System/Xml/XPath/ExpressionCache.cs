// Decompiled with JetBrains decompiler
// Type: System.Xml.XPath.ExpressionCache
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections;
using System.Xml.Xsl;

namespace System.Xml.XPath
{
  internal static class ExpressionCache
  {
    private static readonly Hashtable table_per_ctx = new Hashtable();
    private static object dummy = new object();
    private static object cache_lock = new object();

    public static XPathExpression Get(string xpath, IStaticXsltContext ctx)
    {
      object key = ctx == null ? ExpressionCache.dummy : (object) ctx;
      lock (ExpressionCache.cache_lock)
      {
        if (!(ExpressionCache.table_per_ctx[key] is WeakReference weakReference1))
          return (XPathExpression) null;
        if (!(weakReference1.Target is Hashtable target1))
        {
          ExpressionCache.table_per_ctx[key] = (object) null;
          return (XPathExpression) null;
        }
        if (target1[(object) xpath] is WeakReference weakReference2)
        {
          if (weakReference2.Target is XPathExpression target2)
            return target2;
          target1[(object) xpath] = (object) null;
        }
      }
      return (XPathExpression) null;
    }

    public static void Set(string xpath, IStaticXsltContext ctx, XPathExpression exp)
    {
      object key = ctx == null ? ExpressionCache.dummy : (object) ctx;
      Hashtable target = (Hashtable) null;
      lock (ExpressionCache.cache_lock)
      {
        if (ExpressionCache.table_per_ctx[key] is WeakReference weakReference && weakReference.IsAlive)
          target = (Hashtable) weakReference.Target;
        if (target == null)
        {
          target = new Hashtable();
          ExpressionCache.table_per_ctx[key] = (object) new WeakReference((object) target);
        }
        target[(object) xpath] = (object) new WeakReference((object) exp);
      }
    }
  }
}
