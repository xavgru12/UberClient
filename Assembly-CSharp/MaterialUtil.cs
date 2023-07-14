// Decompiled with JetBrains decompiler
// Type: MaterialUtil
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public static class MaterialUtil
{
  private static Dictionary<Material, MaterialUtil.MaterialCache> _cache = new Dictionary<Material, MaterialUtil.MaterialCache>();

  public static void SetFloat(Material m, string propertyName, float value)
  {
    if ((bool) (Object) m && m.HasProperty(propertyName))
    {
      MaterialUtil.MaterialCache materialCache;
      if (!MaterialUtil._cache.TryGetValue(m, out materialCache))
      {
        materialCache = new MaterialUtil.MaterialCache();
        MaterialUtil._cache[m] = materialCache;
      }
      if (!materialCache.Floats.ContainsKey(propertyName))
        materialCache.Floats[propertyName] = m.GetFloat(propertyName);
      m.SetFloat(propertyName, value);
    }
    else
      Debug.LogError((object) string.Format("Property<float> '{0}' not found in Material {1}", (object) propertyName, !(bool) (Object) m ? (object) "NULL" : (object) m.name));
  }

  public static void SetColor(Material m, string propertyName, Color value)
  {
    if ((bool) (Object) m && m.HasProperty(propertyName))
    {
      MaterialUtil.MaterialCache materialCache;
      if (!MaterialUtil._cache.TryGetValue(m, out materialCache))
      {
        materialCache = new MaterialUtil.MaterialCache();
        MaterialUtil._cache[m] = materialCache;
      }
      if (!materialCache.Colors.ContainsKey(propertyName))
        materialCache.Colors[propertyName] = m.GetColor(propertyName);
      m.SetColor(propertyName, value);
    }
    else
      Debug.LogError((object) string.Format("Property<Color> '{0}' not found in Material {1}", (object) propertyName, !(bool) (Object) m ? (object) "NULL" : (object) m.name));
  }

  public static void SetTextureOffset(Material m, string propertyName, Vector2 value)
  {
    if ((bool) (Object) m && m.HasProperty(propertyName))
    {
      MaterialUtil.MaterialCache materialCache;
      if (!MaterialUtil._cache.TryGetValue(m, out materialCache))
      {
        materialCache = new MaterialUtil.MaterialCache();
        MaterialUtil._cache[m] = materialCache;
      }
      if (!materialCache.TextureOffset.ContainsKey(propertyName))
        materialCache.TextureOffset[propertyName] = m.GetTextureOffset(propertyName);
      m.SetTextureOffset(propertyName, value);
    }
    else
      Debug.LogError((object) string.Format("Property<Vector2> '{0}' not found in Material {1}", (object) propertyName, !(bool) (Object) m ? (object) "NULL" : (object) m.name));
  }

  public static void SetTexture(Material m, string propertyName, Texture value)
  {
    if ((bool) (Object) m && m.HasProperty(propertyName))
    {
      MaterialUtil.MaterialCache materialCache;
      if (!MaterialUtil._cache.TryGetValue(m, out materialCache))
      {
        materialCache = new MaterialUtil.MaterialCache();
        MaterialUtil._cache[m] = materialCache;
      }
      if (!materialCache.Texture.ContainsKey(propertyName))
        materialCache.Texture[propertyName] = m.GetTexture(propertyName);
      m.SetTexture(propertyName, value);
    }
    else
      Debug.LogError((object) string.Format("Property<Texture> '{0}' not found in Material {1}", (object) propertyName, !(bool) (Object) m ? (object) "NULL" : (object) m.name));
  }

  public static void Reset(Material m)
  {
    MaterialUtil.MaterialCache materialCache;
    if (!MaterialUtil._cache.TryGetValue(m, out materialCache))
      return;
    foreach (KeyValuePair<string, Color> color in materialCache.Colors)
      m.SetColor(color.Key, color.Value);
    foreach (KeyValuePair<string, float> keyValuePair in materialCache.Floats)
      m.SetFloat(keyValuePair.Key, keyValuePair.Value);
    foreach (KeyValuePair<string, Vector2> keyValuePair in materialCache.TextureOffset)
      m.SetTextureOffset(keyValuePair.Key, keyValuePair.Value);
  }

  private class MaterialCache
  {
    public Dictionary<string, Color> Colors = new Dictionary<string, Color>();
    public Dictionary<string, float> Floats = new Dictionary<string, float>();
    public Dictionary<string, Vector2> TextureOffset = new Dictionary<string, Vector2>();
    public Dictionary<string, Texture> Texture = new Dictionary<string, Texture>();
  }
}
