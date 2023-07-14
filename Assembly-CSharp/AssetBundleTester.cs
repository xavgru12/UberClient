// Decompiled with JetBrains decompiler
// Type: AssetBundleTester
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Xml;
using UnityEngine;

[ExecuteInEditMode]
public class AssetBundleTester : MonoBehaviour
{
  private const string BaseContentUrlProduction = "http://static.cmune.com/UberStrike/";
  private const string BaseContentUrlExternalQA = "http://static.cmune.com/UberStrike/Qa/";
  private const string BaseContentUrlInternalDev = "http://client.dev.uberstrike.com/";
  private bool _isReadingUrl;
  private bool _isLoadingBundle;
  private float _loadingProgress;
  private string _searchText = string.Empty;
  private string _bundlePath = "file://";
  private string _baseUrl = string.Empty;
  private string _errorMessage = string.Empty;
  private ItemCollection _itemCollection;
  private ItemCollectionGrid _itemsGrid;
  private string[] _bundleDirs = new string[3]
  {
    "DEV",
    "QA",
    "PROD"
  };
  private int _selectedBundleDir = -1;

  private void Awake() => this._itemCollection = new ItemCollection();

  private void OnGUI()
  {
    GUI.Label(new Rect(10f, 10f, 120f, 20f), "AssetBundle Test");
    GUI.Label(new Rect(10f, 40f, 120f, 20f), "AssetBundle Path:");
    GUI.enabled = !this._isReadingUrl && !this._isLoadingBundle;
    int num = GUI.SelectionGrid(new Rect(125f, 40f, 100f, 60f), this._selectedBundleDir, this._bundleDirs, 1, GUI.skin.toggle);
    if (num != this._selectedBundleDir)
    {
      this._selectedBundleDir = num;
      this.UpdateBundleDir();
    }
    this._bundlePath = GUI.TextField(new Rect(125f, 110f, 300f, 20f), this._bundlePath);
    if (GUI.Button(new Rect(430f, 110f, 100f, 20f), "Load"))
    {
      this._isLoadingBundle = true;
      if (this._itemsGrid != null)
        this._itemsGrid.Dispose();
      this.StartCoroutine(AssetBundleLoader.LoadAssetBundleNoCache(this._bundlePath, (Action<float>) (p => this._loadingProgress = p), new Action<AssetBundle>(this.OnAssetbundleLoaded)));
    }
    if ((double) this._loadingProgress > 0.0)
      GUI.Label(new Rect(540f, 110f, 100f, 20f), ((float) ((double) this._loadingProgress * 100.0)).ToString() + "%");
    GUI.Label(new Rect(10f, 140f, 100f, 20f), "Find item name: ");
    this._searchText = GUI.TextField(new Rect(125f, 140f, 100f, 20f), this._searchText);
    if (this._itemsGrid != null)
    {
      this._itemsGrid.SetFilter(this._searchText);
      this._itemsGrid.Draw(new Rect(0.0f, 160f, (float) Screen.width, (float) (Screen.height - 160)));
    }
    GUI.enabled = true;
  }

  private void UpdateBundleDir()
  {
    switch (this._selectedBundleDir)
    {
      case 0:
        this._baseUrl = "http://client.dev.uberstrike.com/";
        break;
      case 1:
        this._baseUrl = "http://static.cmune.com/UberStrike/Qa/";
        break;
      case 2:
        this._baseUrl = "http://static.cmune.com/UberStrike/";
        break;
    }
    this.StartCoroutine(this.StartGettingItemAssetBundleUrl(this._baseUrl));
  }

  private void OnAssetbundleLoaded(AssetBundle bundle)
  {
    this._loadingProgress = 0.0f;
    this.StartCoroutine(this.InstantiateItems(bundle));
  }

  [DebuggerHidden]
  private IEnumerator StartGettingItemAssetBundleUrl(string baseUrl) => (IEnumerator) new AssetBundleTester.\u003CStartGettingItemAssetBundleUrl\u003Ec__Iterator0()
  {
    baseUrl = baseUrl,
    \u003C\u0024\u003EbaseUrl = baseUrl,
    \u003C\u003Ef__this = this
  };

  private string GetItemAssetBundleName(string xml)
  {
    string itemAssetBundleName = string.Empty;
    XmlReader xmlReader = XmlReader.Create((TextReader) new StringReader(xml));
    while (xmlReader.Read())
    {
      if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name.Equals("Hash"))
      {
        itemAssetBundleName = xmlReader.ReadString() + string.Format("-SD.unity3d");
        break;
      }
    }
    return itemAssetBundleName;
  }

  [DebuggerHidden]
  private IEnumerator InstantiateItems(AssetBundle bundle) => (IEnumerator) new AssetBundleTester.\u003CInstantiateItems\u003Ec__Iterator1()
  {
    bundle = bundle,
    \u003C\u0024\u003Ebundle = bundle,
    \u003C\u003Ef__this = this
  };
}
