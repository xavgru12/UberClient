// Decompiled with JetBrains decompiler
// Type: CloneUtil
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using System.Reflection;
using UberStrike.Realtime.UnitySdk;

public static class CloneUtil
{
  public static T Clone<T>(T instance) where T : class
  {
    ConstructorInfo constructor = instance.GetType().GetConstructor(new Type[0]);
    if (constructor == null)
      return (T) null;
    T destination = constructor.Invoke(new object[0]) as T;
    CloneUtil.CopyAllFields<T>(destination, instance);
    return destination;
  }

  public static void CopyAllFields<T>(T destination, T source) where T : class
  {
    foreach (FieldInfo allField in ReflectionHelper.GetAllFields(source.GetType(), true))
      allField.SetValue((object) destination, allField.GetValue((object) source));
  }

  public static void CopyFields(object dst, object src)
  {
    foreach (PropertyInfo property in src.GetType().GetProperties())
    {
      if (property.CanWrite)
        dst.GetType().GetProperty(property.Name).SetValue(dst, property.GetValue(src, (object[]) null), (object[]) null);
    }
  }
}
