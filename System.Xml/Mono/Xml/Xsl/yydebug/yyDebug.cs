// Decompiled with JetBrains decompiler
// Type: Mono.Xml.Xsl.yydebug.yyDebug
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

namespace Mono.Xml.Xsl.yydebug
{
  internal interface yyDebug
  {
    void push(int state, object value);

    void lex(int state, int token, string name, object value);

    void shift(int from, int to, int errorFlag);

    void pop(int state);

    void discard(int state, int token, string name, object value);

    void reduce(int from, int to, int rule, string text, int len);

    void shift(int from, int to);

    void accept(object value);

    void error(string message);

    void reject();
  }
}
