// Decompiled with JetBrains decompiler
// Type: UberStrike.DataCenter.Common.Entities.PlayerCardView
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

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

    public override string ToString() => "[Player: [Name: " + this.Name + "][Cmid: " + this.Cmid.ToString() + "][Splats: " + this.Splats.ToString() + "][Shots: " + this.Shots.ToString() + "][Hits: " + this.Hits.ToString() + "][Splatted: " + this.Splatted.ToString() + "][Precision: " + this.Precision + "][Ranking: " + this.Ranking.ToString() + "][TagName: " + this.TagName + "]]";
  }
}
