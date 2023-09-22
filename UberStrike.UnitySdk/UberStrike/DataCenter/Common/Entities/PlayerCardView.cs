
using System;

namespace UberStrike.DataCenter.Common.Entities
{
  [Serializable]
  public class PlayerCardView : IComparable
  {
    public string Name { get; set; }

    public int Cmid { get; set; }

    public int Splats { get; set; }

    public int Splatted { get; set; }

    public string Precision { get; set; }

    public int Ranking { get; set; }

    public long Shots { get; set; }

    public long Hits { get; set; }

    public string TagName { get; set; }

    public PlayerCardView()
    {
      this.Name = string.Empty;
      this.Precision = string.Empty;
      this.TagName = string.Empty;
    }

    public PlayerCardView(int cmid, int splats, int splatted, long shots, long hits)
    {
      this.Cmid = cmid;
      this.Splats = splats;
      this.Splatted = splatted;
      this.Shots = shots;
      this.Hits = hits;
    }

    public PlayerCardView(
      int cmid,
      string name,
      int splats,
      int splatted,
      string precision,
      int ranking,
      string tagName)
    {
      this.Cmid = cmid;
      this.Name = name;
      this.Splats = splats;
      this.Splatted = splatted;
      this.Precision = precision;
      this.Ranking = ranking;
      this.TagName = tagName;
    }

    public PlayerCardView(
      string name,
      int splats,
      int splatted,
      string precision,
      int ranking,
      string tagName)
    {
      this.Name = name;
      this.Splats = splats;
      this.Splatted = splatted;
      this.Precision = precision;
      this.Ranking = ranking;
      this.TagName = tagName;
    }

    public PlayerCardView(
      int cmid,
      string name,
      int splats,
      int splatted,
      string precision,
      int ranking,
      long shots,
      long hits,
      string tagName)
    {
      this.Cmid = cmid;
      this.Name = name;
      this.Splats = splats;
      this.Splatted = splatted;
      this.Precision = precision;
      this.Ranking = ranking;
      this.Shots = shots;
      this.Hits = hits;
      this.TagName = tagName;
    }

    public PlayerCardView(
      string name,
      int splats,
      int splatted,
      string precision,
      int ranking,
      long shots,
      long hits,
      string tagName)
    {
      this.Name = name;
      this.Splats = splats;
      this.Splatted = splatted;
      this.Precision = precision;
      this.Ranking = ranking;
      this.Shots = shots;
      this.Hits = hits;
      this.TagName = tagName;
    }

    public int CompareTo(object obj)
    {
      if (obj is PlayerCardView)
        return -((obj as PlayerCardView).Ranking - this.Ranking);
      throw new ArgumentOutOfRangeException("Parameter is not of the good type");
    }

    public override string ToString() => "[Player: [Name: " + this.Name + "][Cmid: " + (object) this.Cmid + "][Splats: " + (object) this.Splats + "][Shots: " + (object) this.Shots + "][Hits: " + (object) this.Hits + "][Splatted: " + (object) this.Splatted + "][Precision: " + this.Precision + "][Ranking: " + (object) this.Ranking + "][TagName: " + this.TagName + "]]";
  }
}
