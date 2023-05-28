// Decompiled with JetBrains decompiler
// Type: BorderShadow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using Cmune.DataCenter.Common.Entities;
using UnityEngine;

public class BorderShadow : MonoBehaviour
{
  [SerializeField]
  private Texture2D _pageShadowLeft;
  [SerializeField]
  private Texture2D _pageShadowRight;

  private void OnGUI()
  {
    if (Screen.fullScreen || ApplicationDataManager.Channel != ChannelType.WebPortal && ApplicationDataManager.Channel != ChannelType.WebFacebook && ApplicationDataManager.Channel != ChannelType.Kongregate)
      return;
    GUI.DrawTexture(new Rect(0.0f, 0.0f, 4f, (float) Screen.height), (Texture) this._pageShadowLeft);
    GUI.DrawTexture(new Rect((float) (Screen.width - 4), 0.0f, 4f, (float) Screen.height), (Texture) this._pageShadowRight);
  }
}
