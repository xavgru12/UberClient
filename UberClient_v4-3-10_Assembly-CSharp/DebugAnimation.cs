// Decompiled with JetBrains decompiler
// Type: DebugAnimation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class DebugAnimation : IDebugPage
{
  private CharacterConfig config;

  public string Title => "Animation";

  public void Draw()
  {
    if (GameState.HasCurrentGame)
    {
      GUILayout.BeginHorizontal();
      foreach (CharacterConfig allCharacter in GameState.CurrentGame.AllCharacters)
      {
        if (GUILayout.Button(allCharacter.name))
          this.config = allCharacter;
      }
      GUILayout.EndHorizontal();
      if ((Object) this.config == (Object) null)
        GUILayout.Label("Select a player");
      else if ((Object) this.config.Decorator == (Object) null)
        GUILayout.Label("Missing Decorator");
      else if (this.config.Decorator.AnimationController == null)
        GUILayout.Label("Missing Animation");
      else
        GUILayout.Label(this.config.Decorator.AnimationController.GetDebugInfo());
    }
    else
      GUILayout.Label("No Game Running");
  }
}
