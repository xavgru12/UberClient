
using Cmune.DataCenter.Common.Entities;
using System.Text;

namespace UberStrike.DataCenter.Common.Entities
{
  public class UberstrikeItemGearView : UberstrikeItemView
  {
    public UberstrikeGearConfigView Config { get; set; }

    public UberstrikeItemGearView()
    {
    }

    public UberstrikeItemGearView(
      ItemView item,
      int levelRequired,
      UberstrikeGearConfigView config)
      : base(item, levelRequired)
    {
      this.Config = config;
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("[UberstrikeGearView: ");
      stringBuilder.Append(base.ToString());
      stringBuilder.Append((object) this.Config);
      stringBuilder.Append("]]");
      return stringBuilder.ToString();
    }
  }
}
