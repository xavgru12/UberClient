
using Cmune.DataCenter.Common.Entities;
using System.Text;

namespace UberStrike.DataCenter.Common.Entities
{
  public class UberstrikeItemWeaponView : UberstrikeItemView
  {
    public UberstrikeWeaponConfigView Config { get; set; }

    public UberstrikeItemWeaponView()
    {
    }

    public UberstrikeItemWeaponView(
      ItemView item,
      int levelRequired,
      UberstrikeWeaponConfigView config)
      : base(item, levelRequired)
    {
      this.Config = config;
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("[UberstrikeWeaponView: ");
      stringBuilder.Append(base.ToString());
      stringBuilder.Append((object) this.Config);
      stringBuilder.Append("]]");
      return stringBuilder.ToString();
    }
  }
}
