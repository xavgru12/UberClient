// Decompiled with JetBrains decompiler
// Type: Mono.Xml.Xsl.Operations.XslOperation
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.IO;

namespace Mono.Xml.Xsl.Operations
{
  internal abstract class XslOperation
  {
    public const string XsltNamespace = "http://www.w3.org/1999/XSL/Transform";

    public abstract void Evaluate(XslTransformProcessor p);

    public virtual string EvaluateAsString(XslTransformProcessor p)
    {
      StringWriter w = new StringWriter();
      Outputter newOutput = (Outputter) new TextOutputter((TextWriter) w, true);
      p.PushOutput(newOutput);
      this.Evaluate(p);
      p.PopOutput();
      newOutput.Done();
      return w.ToString();
    }
  }
}
