// Decompiled with JetBrains decompiler
// Type: Boo.Lang.Builtins
// Assembly: Boo.Lang, Version=2.0.9.5, Culture=neutral, PublicKeyToken=32c39770e9a21a67
// MVID: D4F47A63-3E02-45E3-BD62-EDD90EF56CC2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Boo.Lang.dll

using Boo.Lang.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Security.Policy;
using System.Text;

namespace Boo.Lang
{
  public class Builtins
  {
    public static Version BooVersion => new Version("0.9.5.5");

    public static void print(object o) => Console.WriteLine(o);

    public static string gets() => Console.ReadLine();

    public static string prompt(string message)
    {
      Console.Write(message);
      return Console.ReadLine();
    }

    public static string join(IEnumerable enumerable, string separator)
    {
      StringBuilder stringBuilder = new StringBuilder();
      IEnumerator enumerator = enumerable.GetEnumerator();
      using (enumerator as IDisposable)
      {
        if (enumerator.MoveNext())
        {
          stringBuilder.Append(enumerator.Current);
          while (enumerator.MoveNext())
          {
            stringBuilder.Append(separator);
            stringBuilder.Append(enumerator.Current);
          }
        }
      }
      return stringBuilder.ToString();
    }

    public static string join(IEnumerable enumerable, char separator)
    {
      StringBuilder stringBuilder = new StringBuilder();
      IEnumerator enumerator = enumerable.GetEnumerator();
      using (enumerator as IDisposable)
      {
        if (enumerator.MoveNext())
        {
          stringBuilder.Append(enumerator.Current);
          while (enumerator.MoveNext())
          {
            stringBuilder.Append(separator);
            stringBuilder.Append(enumerator.Current);
          }
        }
      }
      return stringBuilder.ToString();
    }

    public static string join(IEnumerable enumerable) => Builtins.join(enumerable, ' ');

    [DebuggerHidden]
    public static IEnumerable map(object enumerable, ICallable function)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      Builtins.\u003Cmap\u003Ec__Iterator0 mapCIterator0 = new Builtins.\u003Cmap\u003Ec__Iterator0()
      {
        enumerable = enumerable,
        function = function,
        \u003C\u0024\u003Eenumerable = enumerable,
        \u003C\u0024\u003Efunction = function
      };
      // ISSUE: reference to a compiler-generated field
      mapCIterator0.\u0024PC = -2;
      return (IEnumerable) mapCIterator0;
    }

    public static object[] array(IEnumerable enumerable) => new List(enumerable).ToArray();

    private static Array ArrayFromCollection(Type elementType, ICollection collection)
    {
      if (elementType == null)
        throw new ArgumentNullException(nameof (elementType));
      if (collection == null)
        throw new ArgumentNullException(nameof (collection));
      Array instance = Array.CreateInstance(elementType, collection.Count);
      if (RuntimeServices.IsPromotableNumeric(Type.GetTypeCode(elementType)))
      {
        int index = 0;
        foreach (object obj in (IEnumerable) collection)
        {
          object type = RuntimeServices.CheckNumericPromotion(obj).ToType(elementType, (IFormatProvider) null);
          instance.SetValue(type, index);
          ++index;
        }
      }
      else
        collection.CopyTo(instance, 0);
      return instance;
    }

    [TypeInferenceRule(TypeInferenceRules.ArrayOfTypeReferencedByFirstArgument)]
    public static Array array(Type elementType, IEnumerable enumerable)
    {
      if (elementType == null)
        throw new ArgumentNullException(nameof (elementType));
      if (enumerable == null)
        throw new ArgumentNullException(nameof (enumerable));
      if (enumerable is ICollection collection)
        return Builtins.ArrayFromCollection(elementType, collection);
      List list;
      if (RuntimeServices.IsPromotableNumeric(Type.GetTypeCode(elementType)))
      {
        list = new List();
        foreach (object obj in enumerable)
        {
          object type = RuntimeServices.CheckNumericPromotion(obj).ToType(elementType, (IFormatProvider) null);
          list.Add(type);
        }
      }
      else
        list = new List(enumerable);
      return list.ToArray(elementType);
    }

    [TypeInferenceRule(TypeInferenceRules.ArrayOfTypeReferencedByFirstArgument)]
    public static Array array(Type elementType, int length) => length >= 0 ? Builtins.matrix(elementType, length) : throw new ArgumentException("`length' cannot be negative", nameof (length));

    public static Array matrix(Type elementType, params int[] lengths)
    {
      if (elementType == null)
        throw new ArgumentNullException(nameof (elementType));
      return lengths != null && lengths.Length != 0 ? Array.CreateInstance(elementType, lengths) : throw new ArgumentException("A matrix must have at least one dimension", nameof (lengths));
    }

    public static T[] array<T>(int length) => throw new NotSupportedException("Operation should have been optimized away by the compiler!");

    public static T[,] matrix<T>(int length0, int length1) => throw new NotSupportedException("Operation should have been optimized away by the compiler!");

    public static T[,,] matrix<T>(int length0, int length1, int length2) => throw new NotSupportedException("Operation should have been optimized away by the compiler!");

    public static T[,,,] matrix<T>(int length0, int length1, int length2, int length3) => throw new NotSupportedException("Operation should have been optimized away by the compiler!");

    public static IEnumerable iterator(object enumerable) => RuntimeServices.GetEnumerable(enumerable);

    public static Process shellp(string filename, string arguments)
    {
      Process process = new Process();
      process.StartInfo.Arguments = arguments;
      process.StartInfo.CreateNoWindow = true;
      process.StartInfo.UseShellExecute = false;
      process.StartInfo.RedirectStandardOutput = true;
      process.StartInfo.RedirectStandardInput = true;
      process.StartInfo.RedirectStandardError = true;
      process.StartInfo.FileName = filename;
      process.Start();
      return process;
    }

    public static string shell(string filename, string arguments)
    {
      Process process = Builtins.shellp(filename, arguments);
      string end = process.StandardOutput.ReadToEnd();
      process.WaitForExit();
      return end;
    }

    public static string shellm(string filename, params string[] arguments)
    {
      AppDomain domain = AppDomain.CreateDomain(nameof (shellm), (Evidence) null, new AppDomainSetup()
      {
        ApplicationBase = Path.GetDirectoryName(Path.GetFullPath(filename))
      });
      try
      {
        Builtins.AssemblyExecutor assemblyExecutor = new Builtins.AssemblyExecutor(filename, arguments);
        domain.DoCallBack(new CrossAppDomainDelegate(assemblyExecutor.Execute));
        return assemblyExecutor.CapturedOutput;
      }
      finally
      {
        AppDomain.Unload(domain);
      }
    }

    [DebuggerHidden]
    public static IEnumerable<object[]> enumerate(object enumerable)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      Builtins.\u003Cenumerate\u003Ec__Iterator1 enumerateCIterator1 = new Builtins.\u003Cenumerate\u003Ec__Iterator1()
      {
        enumerable = enumerable,
        \u003C\u0024\u003Eenumerable = enumerable
      };
      // ISSUE: reference to a compiler-generated field
      enumerateCIterator1.\u0024PC = -2;
      return (IEnumerable<object[]>) enumerateCIterator1;
    }

    public static IEnumerable<int> range(int max) => max >= 0 ? Builtins.range(0, max) : throw new ArgumentOutOfRangeException("max < 0");

    [DebuggerHidden]
    public static IEnumerable<int> range(int begin, int end)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      Builtins.\u003Crange\u003Ec__Iterator2 rangeCIterator2 = new Builtins.\u003Crange\u003Ec__Iterator2()
      {
        begin = begin,
        end = end,
        \u003C\u0024\u003Ebegin = begin,
        \u003C\u0024\u003Eend = end
      };
      // ISSUE: reference to a compiler-generated field
      rangeCIterator2.\u0024PC = -2;
      return (IEnumerable<int>) rangeCIterator2;
    }

    [DebuggerHidden]
    public static IEnumerable<int> range(int begin, int end, int step)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      Builtins.\u003Crange\u003Ec__Iterator3 rangeCIterator3 = new Builtins.\u003Crange\u003Ec__Iterator3()
      {
        step = step,
        begin = begin,
        end = end,
        \u003C\u0024\u003Estep = step,
        \u003C\u0024\u003Ebegin = begin,
        \u003C\u0024\u003Eend = end
      };
      // ISSUE: reference to a compiler-generated field
      rangeCIterator3.\u0024PC = -2;
      return (IEnumerable<int>) rangeCIterator3;
    }

    public static IEnumerable reversed(object enumerable) => (IEnumerable) new List(Builtins.iterator(enumerable)).Reversed;

    public static Builtins.ZipEnumerator zip(params object[] enumerables)
    {
      IEnumerator[] enumeratorArray = new IEnumerator[enumerables.Length];
      for (int index = 0; index < enumerables.Length; ++index)
        enumeratorArray[index] = Builtins.GetEnumerator(enumerables[index]);
      return new Builtins.ZipEnumerator(enumeratorArray);
    }

    [DebuggerHidden]
    public static IEnumerable<object> cat(params object[] args)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      Builtins.\u003Ccat\u003Ec__Iterator4 catCIterator4 = new Builtins.\u003Ccat\u003Ec__Iterator4()
      {
        args = args,
        \u003C\u0024\u003Eargs = args
      };
      // ISSUE: reference to a compiler-generated field
      catCIterator4.\u0024PC = -2;
      return (IEnumerable<object>) catCIterator4;
    }

    private static IEnumerator GetEnumerator(object enumerable) => RuntimeServices.GetEnumerable(enumerable).GetEnumerator();

    public class duck
    {
    }

    internal class AssemblyExecutor : MarshalByRefObject
    {
      private string _filename;
      private string[] _arguments;
      private string _capturedOutput = string.Empty;

      public AssemblyExecutor(string filename, string[] arguments)
      {
        this._filename = filename;
        this._arguments = arguments;
      }

      public string CapturedOutput => this._capturedOutput;

      public void Execute()
      {
        StringWriter newOut1 = new StringWriter();
        TextWriter newOut2 = Console.Out;
        try
        {
          Console.SetOut((TextWriter) newOut1);
          Assembly.LoadFrom(this._filename).EntryPoint.Invoke((object) null, new object[1]
          {
            (object) this._arguments
          });
        }
        finally
        {
          Console.SetOut(newOut2);
          this._capturedOutput = newOut1.ToString();
        }
      }
    }

    [EnumeratorItemType(typeof (object[]))]
    public class ZipEnumerator : IEnumerable, IEnumerator, IDisposable
    {
      private IEnumerator[] _enumerators;

      internal ZipEnumerator(params IEnumerator[] enumerators) => this._enumerators = enumerators;

      public void Dispose()
      {
        for (int index = 0; index < this._enumerators.Length; ++index)
        {
          if (this._enumerators[index] is IDisposable enumerator)
            enumerator.Dispose();
        }
      }

      public void Reset()
      {
        for (int index = 0; index < this._enumerators.Length; ++index)
          this._enumerators[index].Reset();
      }

      public bool MoveNext()
      {
        for (int index = 0; index < this._enumerators.Length; ++index)
        {
          if (!this._enumerators[index].MoveNext())
            return false;
        }
        return true;
      }

      public object Current
      {
        get
        {
          object[] current = new object[this._enumerators.Length];
          for (int index = 0; index < current.Length; ++index)
            current[index] = this._enumerators[index].Current;
          return (object) current;
        }
      }

      public IEnumerator GetEnumerator() => (IEnumerator) this;
    }
  }
}
