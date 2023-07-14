// Decompiled with JetBrains decompiler
// Type: Mono.Xml.Xsl.XslEmptyTemplate
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections;

namespace Mono.Xml.Xsl
{
  internal class XslEmptyTemplate : XslTemplate
  {
    private static XslEmptyTemplate instance = new XslEmptyTemplate();

    private XslEmptyTemplate()
      : base((Compiler) null)
    {
    }

    public static XslTemplate Instance => (XslTemplate) XslEmptyTemplate.instance;

    public override void Evaluate(XslTransformProcessor p, Hashtable withParams)
    {
    }
  }
}
