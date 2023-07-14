// Decompiled with JetBrains decompiler
// Type: SplashScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class SplashScreen : MonoBehaviour
{
  [SerializeField]
  private Texture2D initializingScreen;
  [SerializeField]
  private Texture2D initializingScreenBlurred;
  private Texture mainScreen;
  private Texture blurScreen;
  private Texture2D loadingBarBackground;
  private float _blurBackgroundAlpha = 1f;
  private Vector2 barSize = new Vector2(300f, 20f);

  private void Awake()
  {
    this.loadingBarBackground = new Texture2D(1, 1, TextureFormat.RGB24, false);
    this.loadingBarBackground.SetPixels(new Color[1]
    {
      Color.white
    });
    this.loadingBarBackground.Apply(false);
    this.mainScreen = (Texture) this.initializingScreen;
    this.blurScreen = (Texture) this.initializingScreenBlurred;
  }

  private void OnGUI()
  {
    GUI.skin = PopupSkin.Skin;
    GUIStyle labelLoading = PopupSkin.label_loading;
    float alpha = (float) (((double) Mathf.Sin(Time.time * 2f) + 1.2999999523162842) * 0.5);
    if (!GlobalSceneLoader.IsError)
    {
      labelLoading.normal.textColor = labelLoading.normal.textColor.SetAlpha(alpha);
      if (GlobalSceneLoader.IsGlobalSceneLoaded && GlobalSceneLoader.IsItemAssetBundleLoaded)
        this._blurBackgroundAlpha = Mathf.Lerp(this._blurBackgroundAlpha, 0.0f, Time.deltaTime);
      float num1 = (float) Screen.width / (float) this.mainScreen.width;
      float num2 = (float) Screen.height / (float) this.mainScreen.height;
      float num3 = (double) num1 <= (double) num2 ? num2 : num1;
      Rect position = new Rect((float) (Screen.width / 2) - (float) ((double) this.mainScreen.width * (double) num3 / 2.0), (float) (Screen.height / 2) - (float) ((double) this.mainScreen.height * (double) num3 / 2.0), (float) this.mainScreen.width * num3, (float) this.mainScreen.height * num3);
      GUI.depth = 100;
      GUI.color = new Color(1f, 1f, 1f, 1f);
      GUI.DrawTexture(position, this.mainScreen);
      GUI.color = new Color(1f, 1f, 1f, 1f - this._blurBackgroundAlpha);
      GUI.DrawTexture(position, this.blurScreen);
      GUI.color = Color.white;
      if (Application.internetReachability == NetworkReachability.NotReachable)
      {
        labelLoading.normal.textColor = labelLoading.normal.textColor.SetAlpha(1f);
        GUI.Label(new Rect(0.0f, 0.0f, (float) Screen.width, (float) Screen.height), "No internet connection available", labelLoading);
      }
      else if (!GlobalSceneLoader.IsGlobalSceneLoaded)
      {
        GUITools.LabelShadow(new Rect(0.0f, (float) (Screen.height - 150), (float) Screen.width, labelLoading.CalcSize(new GUIContent("Loading game. Please wait...")).Height()), "Loading game. Please wait...", labelLoading, labelLoading.normal.textColor);
        GUI.color = new Color(1f, 1f, 1f, 0.5f);
        GUI.DrawTexture(new Rect((float) ((double) Screen.width * 0.5 - (double) this.barSize.x * 0.5), (float) ((double) (Screen.height - 150) + (double) this.barSize.Height() + 8.0), this.barSize.x, 8f), (Texture) this.loadingBarBackground);
        GUI.color = Color.white;
        GUI.DrawTexture(new Rect((float) ((double) Screen.width * 0.5 - (double) this.barSize.x * 0.5), (float) ((double) (Screen.height - 150) + (double) this.barSize.Height() + 8.0), (float) Mathf.RoundToInt(this.TotalProgress * this.barSize.x), 8f), (Texture) this.loadingBarBackground);
      }
      else if (!GlobalSceneLoader.IsItemAssetBundleLoaded)
      {
        GUITools.LabelShadow(new Rect(0.0f, (float) (Screen.height - 150), (float) Screen.width, labelLoading.CalcSize(new GUIContent("Loading weapons. Please wait...")).Height()), "Loading weapons. Please wait...", labelLoading, labelLoading.normal.textColor);
        GUI.color = new Color(1f, 1f, 1f, 0.5f);
        GUI.DrawTexture(new Rect((float) ((double) Screen.width * 0.5 - (double) this.barSize.x * 0.5), (float) ((double) (Screen.height - 150) + (double) this.barSize.Height() + 8.0), this.barSize.x, 8f), (Texture) this.loadingBarBackground);
        GUI.color = Color.white;
        GUI.DrawTexture(new Rect((float) ((double) Screen.width * 0.5 - (double) this.barSize.x * 0.5), (float) ((double) (Screen.height - 150) + (double) this.barSize.Height() + 8.0), (float) Mathf.RoundToInt(this.TotalProgress * this.barSize.x), 8f), (Texture) this.loadingBarBackground);
      }
      else
      {
        if (GlobalSceneLoader.IsInitialised)
          return;
        GUITools.LabelShadow(new Rect(0.0f, (float) (Screen.height - 150), (float) Screen.width, labelLoading.CalcSize(new GUIContent("Connecting...")).Height()), "Connecting...", labelLoading, labelLoading.normal.textColor);
      }
    }
    else
    {
      if (PopupSystem.IsAnyPopupOpen)
        return;
      labelLoading.normal.textColor = labelLoading.normal.textColor.SetAlpha(1f);
      if (!GUI.Button(new Rect(0.0f, 0.0f, (float) Screen.width, (float) Screen.height), "There was a problem loading UberStrike. Try reloading or visit support.uberstrike.com if the problem persists.", labelLoading))
        return;
      Application.Quit();
    }
  }

  private float TotalProgress => (float) ((double) GlobalSceneLoader.GlobalSceneProgress * 0.5 + (double) GlobalSceneLoader.ItemAssetBundleProgress * 0.5);
}
