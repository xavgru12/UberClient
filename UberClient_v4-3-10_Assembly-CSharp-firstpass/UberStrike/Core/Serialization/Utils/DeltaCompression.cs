// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.Utils.DeltaCompression
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System.IO;

namespace UberStrike.Core.Serialization.Utils
{
  public class DeltaCompression
  {
    public static byte[] Deflate(byte[] baseData, byte[] newData)
    {
      using (MemoryStream memoryStream = new MemoryStream())
      {
        byte num = 0;
        for (int index = 0; index < newData.Length; ++index)
        {
          if (index < baseData.Length)
          {
            if ((int) baseData[index] == (int) newData[index])
            {
              ++num;
            }
            else
            {
              memoryStream.WriteByte(num);
              memoryStream.WriteByte(newData[index]);
              num = (byte) 0;
            }
          }
          else
            memoryStream.WriteByte(newData[index]);
        }
        return memoryStream.ToArray();
      }
    }

    public static byte[] Inflate(byte[] baseData, byte[] delta)
    {
      if (delta.Length == 0)
        return baseData;
      using (MemoryStream memoryStream = new MemoryStream())
      {
        int index1 = 0;
        int index2 = 0;
        while (index2 < delta.Length)
        {
          if (index1 < baseData.Length)
          {
            int num = 0;
            while (num < (int) delta[index2])
            {
              memoryStream.WriteByte(baseData[index1]);
              ++num;
              ++index1;
            }
            memoryStream.WriteByte(delta[index2 + 1]);
            ++index1;
            index2 += 2;
          }
          else
          {
            memoryStream.WriteByte(delta[index2]);
            ++index2;
          }
        }
        return memoryStream.ToArray();
      }
    }
  }
}
