
using System.Collections.Generic;

namespace Cmune.Util
{
  public sealed class ReverseComparer<T> : IComparer<T>
  {
    private readonly IComparer<T> inner;

    public ReverseComparer()
      : this((IComparer<T>) null)
    {
    }

    public ReverseComparer(IComparer<T> inner) => this.inner = inner ?? (IComparer<T>) Comparer<T>.Default;

    int IComparer<T>.Compare(T x, T y) => this.inner.Compare(y, x);
  }
}
