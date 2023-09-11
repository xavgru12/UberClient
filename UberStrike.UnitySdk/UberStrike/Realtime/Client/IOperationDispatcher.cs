
using System;
using System.Collections.Generic;

namespace Uberstrike.Realtime.Client
{
  public interface IOperationDispatcher
  {
    void SetSender(
      Func<byte, Dictionary<byte, object>, bool, bool> sender);
  }
}
