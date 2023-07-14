// Decompiled with JetBrains decompiler
// Type: System.Xml.Serialization.XmlSerializerImplementation
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections;

namespace System.Xml.Serialization
{
  public abstract class XmlSerializerImplementation
  {
    public virtual XmlSerializationReader Reader => throw new NotSupportedException();

    public virtual Hashtable ReadMethods => throw new NotSupportedException();

    public virtual Hashtable TypedSerializers => throw new NotSupportedException();

    public virtual Hashtable WriteMethods => throw new NotSupportedException();

    public virtual XmlSerializationWriter Writer => throw new NotSupportedException();

    public virtual bool CanSerialize(Type type) => throw new NotSupportedException();

    public virtual XmlSerializer GetSerializer(Type type) => throw new NotSupportedException();
  }
}
