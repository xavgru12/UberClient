﻿
using System.Text;

namespace UberStrike.DataCenter.Common.Entities
{
  public class UberstrikeFunctionalConfigView
  {
    public int LevelRequired { get; set; }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("[UberstrikeFunctionalConfigView: ");
      stringBuilder.Append("]");
      return stringBuilder.ToString();
    }
  }
}
