
using System;

namespace Cmune.Util
{
  public class CmuneException : Exception
  {
    public CmuneException(string str, params object[] args) => CmuneDebug.LogError("EXCEPTION: " + string.Format(str, args), new object[0]);
  }
}
