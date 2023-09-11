
using Cmune.DataCenter.Common.Entities;
using System.Text;

namespace UberStrike.DataCenter.Common.Entities
{
  public class UberstrikeItemWeaponModView : UberstrikeItemView
  {
    public UberstrikeWeaponModConfigView Config { get; set; }

    public UberstrikeItemWeaponModView()
    {
    }

    public UberstrikeItemWeaponModView(
      ItemView item,
      int level,
      UberstrikeWeaponModConfigView config)
      : base(item, level)
    {
      this.Config = config;
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("[UberstrikeWeaponModView: ");
      stringBuilder.Append(base.ToString());
      stringBuilder.Append((object) this.Config);
      stringBuilder.Append("]]");
      return stringBuilder.ToString();
    }
  }
}
