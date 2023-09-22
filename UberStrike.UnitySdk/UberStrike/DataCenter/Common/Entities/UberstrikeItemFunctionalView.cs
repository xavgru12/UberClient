
using Cmune.DataCenter.Common.Entities;
using System.Text;

namespace UberStrike.DataCenter.Common.Entities
{
  public class UberstrikeItemFunctionalView : UberstrikeItemView
  {
    public UberstrikeFunctionalConfigView Config { get; set; }

    public UberstrikeItemFunctionalView()
    {
    }

    public UberstrikeItemFunctionalView(
      ItemView item,
      int levelRequired,
      UberstrikeFunctionalConfigView config)
      : base(item, levelRequired)
    {
      this.Config = config;
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("[UberstrikeFunctionalView: ");
      stringBuilder.Append(base.ToString());
      stringBuilder.Append((object) this.Config);
      stringBuilder.Append("]]");
      return stringBuilder.ToString();
    }
  }
}
