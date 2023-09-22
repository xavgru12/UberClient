
using System;

namespace Cmune.Realtime.Common
{
  [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
  public class CMUNESYNC : Attribute
  {
    public CMUNESYNC(int tagId) => this.TagId = tagId;

    public int TagId { private set; get; }

    public bool IsTagged => this.TagId > 0;
  }
}
