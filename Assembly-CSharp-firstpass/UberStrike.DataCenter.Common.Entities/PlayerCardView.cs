using System;

namespace UberStrike.DataCenter.Common.Entities
{
	[Serializable]
	public class PlayerCardView : IComparable
	{
		public string Name
		{
			get;
			set;
		}

		public int Cmid
		{
			get;
			set;
		}

		public int Splats
		{
			get;
			set;
		}

		public int Splatted
		{
			get;
			set;
		}

		public string Precision
		{
			get;
			set;
		}

		public int Ranking
		{
			get;
			set;
		}

		public long Shots
		{
			get;
			set;
		}

		public long Hits
		{
			get;
			set;
		}

		public string TagName
		{
			get;
			set;
		}

		public PlayerCardView()
		{
			Name = string.Empty;
			Precision = string.Empty;
			TagName = string.Empty;
		}

		public PlayerCardView(int cmid, int splats, int splatted, long shots, long hits)
		{
			Cmid = cmid;
			Splats = splats;
			Splatted = splatted;
			Shots = shots;
			Hits = hits;
		}

		public PlayerCardView(int cmid, string name, int splats, int splatted, string precision, int ranking, string tagName)
		{
			Cmid = cmid;
			Name = name;
			Splats = splats;
			Splatted = splatted;
			Precision = precision;
			Ranking = ranking;
			TagName = tagName;
		}

		public PlayerCardView(string name, int splats, int splatted, string precision, int ranking, string tagName)
		{
			Name = name;
			Splats = splats;
			Splatted = splatted;
			Precision = precision;
			Ranking = ranking;
			TagName = tagName;
		}

		public PlayerCardView(int cmid, string name, int splats, int splatted, string precision, int ranking, long shots, long hits, string tagName)
		{
			Cmid = cmid;
			Name = name;
			Splats = splats;
			Splatted = splatted;
			Precision = precision;
			Ranking = ranking;
			Shots = shots;
			Hits = hits;
			TagName = tagName;
		}

		public PlayerCardView(string name, int splats, int splatted, string precision, int ranking, long shots, long hits, string tagName)
		{
			Name = name;
			Splats = splats;
			Splatted = splatted;
			Precision = precision;
			Ranking = ranking;
			Shots = shots;
			Hits = hits;
			TagName = tagName;
		}

		public int CompareTo(object obj)
		{
			if (obj is PlayerCardView)
			{
				PlayerCardView playerCardView = obj as PlayerCardView;
				return -(playerCardView.Ranking - Ranking);
			}
			throw new ArgumentOutOfRangeException("Parameter is not of the good type");
		}

		public override string ToString()
		{
			return "[Player: [Name: " + Name + "][Cmid: " + Cmid.ToString() + "][Splats: " + Splats.ToString() + "][Shots: " + Shots.ToString() + "][Hits: " + Hits.ToString() + "][Splatted: " + Splatted.ToString() + "][Precision: " + Precision + "][Ranking: " + Ranking.ToString() + "][TagName: " + TagName + "]]";
		}
	}
}
