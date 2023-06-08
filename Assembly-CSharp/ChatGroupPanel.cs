using System.Collections.Generic;
using UnityEngine;

public class ChatGroupPanel
{
	private readonly List<ChatGroup> chatGroups;

	public Vector2 Scroll
	{
		get;
		set;
	}

	public string SearchText
	{
		get;
		set;
	}

	public float ContentHeight
	{
		get;
		set;
	}

	public float WindowHeight
	{
		get;
		set;
	}

	public IEnumerable<ChatGroup> Groups => chatGroups;

	public ChatGroupPanel()
	{
		SearchText = string.Empty;
		chatGroups = new List<ChatGroup>();
	}

	public void AddGroup(UserGroups group, string name, ICollection<CommUser> users)
	{
		chatGroups.Add(new ChatGroup(group, name, users));
	}

	public void ScrollToUser(int cmid)
	{
		float total = 0f;
		int num = 0;
		foreach (ChatGroup chatGroup in chatGroups)
		{
			foreach (CommUser player in chatGroup.Players)
			{
				num++;
				if (player.Cmid == cmid)
				{
					break;
				}
			}
		}
		chatGroups.ForEach(delegate(ChatGroup g)
		{
			total += g.Players.Count;
		});
		float y = ContentHeight * (float)num / total;
		Vector2 scroll = Scroll;
		Scroll = new Vector2(scroll.x, y);
	}
}
