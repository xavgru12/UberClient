// Decompiled with JetBrains decompiler
// Type: HudPopup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class HudPopup : Singleton<HudPopup>
{
  private const int ShowTime = 3;
  private const int FadeTime = 1;
  private Queue<HudPopup.HudMessage> queue = new Queue<HudPopup.HudMessage>();

  private HudPopup()
  {
  }

  public void Show(string text, Texture2D icon)
  {
    AutoMonoBehaviour<UnityRuntime>.Instance.OnGui += new Action(this.OnGUI);
    this.queue.Enqueue(new HudPopup.HudMessage()
    {
      Text = text,
      Texture = icon,
      Time = Time.time + 3f
    });
  }

  private void OnGUI()
  {
    if (this.queue.Count > 0)
    {
      if ((double) this.queue.Peek().Time > (double) Time.time)
      {
        HudPopup.HudMessage hudMessage = this.queue.Peek();
        Vector2 vector2 = new Vector2(260f, 50f);
        GUI.color = hudMessage.Color;
        GUI.BeginGroup(new Rect(0.0f, (float) Screen.height * 0.5f, vector2.x, vector2.y), BlueStonez.window);
        GUI.Label(new Rect(10f, 5f, (float) ((double) vector2.x - (double) hudMessage.IconWidth(vector2.y) - 20.0), vector2.y - 10f), hudMessage.Text, BlueStonez.label_interparkbold_13pt_left);
        GUI.DrawTexture(new Rect(vector2.x - hudMessage.IconWidth(vector2.y), 0.0f, hudMessage.IconWidth(vector2.y), vector2.y), (Texture) hudMessage.Texture);
        GUI.EndGroup();
        GUI.color = Color.white;
      }
      else
      {
        this.queue.Dequeue();
        if (this.queue.Count <= 0)
          return;
        this.queue.Peek().Time = Time.time + 3f;
      }
    }
    else
      AutoMonoBehaviour<UnityRuntime>.Instance.OnGui -= new Action(this.OnGUI);
  }

  private class HudMessage
  {
    public Texture2D Texture;
    public string Text;
    public float Time;

    public float Alpha => Mathf.Lerp(1f, 0.0f, (float) ((double) Time.time - (double) this.Time + 1.0));

    public Color Color => new Color(1f, 1f, 1f, this.Alpha);

    public float IconWidth(float height) => (UnityEngine.Object) this.Texture != (UnityEngine.Object) null ? (float) this.Texture.width / (float) this.Texture.height * height : 0.0f;
  }
}
