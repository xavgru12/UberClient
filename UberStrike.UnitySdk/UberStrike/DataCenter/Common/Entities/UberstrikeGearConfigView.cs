
using System.Text;

namespace UberStrike.DataCenter.Common.Entities
{
  public class UberstrikeGearConfigView
  {
    public int ArmorPoints { get; set; }

    public int ArmorAbsorptionPercent { get; set; }

    public int ArmorWeight { get; set; }

    public int LevelRequired { get; set; }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("[UberstrikeGearConfigView: [ArmorPoints: ");
      stringBuilder.Append(this.ArmorPoints);
      stringBuilder.Append("][ArmorAbsorptionPercent: ");
      stringBuilder.Append(this.ArmorAbsorptionPercent);
      stringBuilder.Append("][ArmorWeight: ");
      stringBuilder.Append(this.ArmorWeight);
      stringBuilder.Append("]]");
      return stringBuilder.ToString();
    }
  }
}
