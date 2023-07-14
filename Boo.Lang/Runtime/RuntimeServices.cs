// Decompiled with JetBrains decompiler
// Type: Boo.Lang.Runtime.RuntimeServices
// Assembly: Boo.Lang, Version=2.0.9.5, Culture=neutral, PublicKeyToken=32c39770e9a21a67
// MVID: D4F47A63-3E02-45E3-BD62-EDD90EF56CC2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Boo.Lang.dll

using Boo.Lang.Runtime.DynamicDispatching;
using Boo.Lang.Runtime.DynamicDispatching.Emitters;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Boo.Lang.Runtime
{
  public class RuntimeServices
  {
    internal const BindingFlags InstanceMemberFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
    internal const BindingFlags DefaultBindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy | BindingFlags.OptionalParamBinding;
    private const BindingFlags InvokeBindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy | BindingFlags.InvokeMethod | BindingFlags.OptionalParamBinding;
    private const BindingFlags SetPropertyBindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy | BindingFlags.SetField | BindingFlags.SetProperty | BindingFlags.OptionalParamBinding;
    private const BindingFlags GetPropertyBindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy | BindingFlags.GetField | BindingFlags.GetProperty | BindingFlags.OptionalParamBinding;
    private static readonly object[] NoArguments = new object[0];
    private static readonly Type RuntimeServicesType = typeof (RuntimeServices);
    private static readonly DispatcherCache _cache = new DispatcherCache();
    private static readonly ExtensionRegistry _extensions = new ExtensionRegistry();
    private static readonly object True = (object) true;

    public static void WithExtensions(Type extensions, RuntimeServices.CodeBlock block)
    {
      RuntimeServices.RegisterExtensions(extensions);
      try
      {
        block();
      }
      finally
      {
        RuntimeServices.UnRegisterExtensions(extensions);
      }
    }

    public static void RegisterExtensions(Type extensions) => RuntimeServices._extensions.Register(extensions);

    public static void UnRegisterExtensions(Type extensions) => RuntimeServices._extensions.UnRegister(extensions);

    public static object Invoke(object target, string name, object[] args) => RuntimeServices.GetDispatcher(target, args, name, (DispatcherCache.DispatcherFactory) (() => RuntimeServices.CreateMethodDispatcher(target, name, args)))(target, args);

    private static Dispatcher CreateMethodDispatcher(object target, string name, object[] args)
    {
      if (target is IQuackFu)
        return (Dispatcher) ((o, arguments) => ((IQuackFu) o).QuackInvoke(name, arguments));
      if (target is Type targetType)
        return RuntimeServices.DoCreateMethodDispatcher((object) null, targetType, name, args);
      Type type = target.GetType();
      return type.IsCOMObject ? (Dispatcher) ((o, arguments) => o.GetType().InvokeMember(name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy | BindingFlags.InvokeMethod | BindingFlags.OptionalParamBinding, (Binder) null, target, arguments)) : RuntimeServices.DoCreateMethodDispatcher(target, type, name, args);
    }

    private static Dispatcher DoCreateMethodDispatcher(
      object target,
      Type targetType,
      string name,
      object[] args)
    {
      return new MethodDispatcherFactory(RuntimeServices._extensions, target, targetType, name, args).Create();
    }

    private static Dispatcher GetDispatcher(
      object target,
      object[] args,
      string cacheKeyName,
      DispatcherCache.DispatcherFactory factory)
    {
      Type[] argumentTypes = MethodResolver.GetArgumentTypes(args);
      return RuntimeServices.GetDispatcher(target, cacheKeyName, argumentTypes, factory);
    }

    private static Dispatcher GetDispatcher(
      object target,
      string cacheKeyName,
      Type[] cacheKeyTypes,
      DispatcherCache.DispatcherFactory factory)
    {
      if (!(target is Type type))
        type = target.GetType();
      DispatcherKey key = new DispatcherKey(type, cacheKeyName, cacheKeyTypes);
      return RuntimeServices._cache.Get(key, factory);
    }

    public static object GetProperty(object target, string name) => RuntimeServices.GetDispatcher(target, RuntimeServices.NoArguments, name, (DispatcherCache.DispatcherFactory) (() => RuntimeServices.CreatePropGetDispatcher(target, name)))(target, RuntimeServices.NoArguments);

    private static Dispatcher CreatePropGetDispatcher(object target, string name)
    {
      if (target is IQuackFu)
        return (Dispatcher) ((o, args) => ((IQuackFu) o).QuackGet(name, (object[]) null));
      if (target is Type type)
        return RuntimeServices.DoCreatePropGetDispatcher((object) null, type, name);
      return target.GetType().IsCOMObject ? (Dispatcher) ((o, args) => o.GetType().InvokeMember(name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy | BindingFlags.GetField | BindingFlags.GetProperty | BindingFlags.OptionalParamBinding, (Binder) null, o, (object[]) null)) : RuntimeServices.DoCreatePropGetDispatcher(target, target.GetType(), name);
    }

    private static Dispatcher DoCreatePropGetDispatcher(object target, Type type, string name) => new PropertyDispatcherFactory(RuntimeServices._extensions, target, type, name, new object[0]).CreateGetter();

    public static object SetProperty(object target, string name, object value)
    {
      object[] args = new object[1]{ value };
      return RuntimeServices.GetDispatcher(target, args, name, (DispatcherCache.DispatcherFactory) (() => RuntimeServices.CreatePropSetDispatcher(target, name, value)))(target, args);
    }

    private static Dispatcher CreatePropSetDispatcher(object target, string name, object value)
    {
      switch (target)
      {
        case IQuackFu _:
          return (Dispatcher) ((o, args) => ((IQuackFu) o).QuackSet(name, (object[]) null, args[0]));
        case Type type2:
          return RuntimeServices.DoCreatePropSetDispatcher((object) null, type2, name, value);
        default:
          Type type1 = target.GetType();
          return type1.IsCOMObject ? (Dispatcher) ((o, args) => o.GetType().InvokeMember(name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy | BindingFlags.SetField | BindingFlags.SetProperty | BindingFlags.OptionalParamBinding, (Binder) null, o, args)) : RuntimeServices.DoCreatePropSetDispatcher(target, type1, name, value);
      }
    }

    private static Dispatcher DoCreatePropSetDispatcher(
      object target,
      Type type,
      string name,
      object value)
    {
      return new PropertyDispatcherFactory(RuntimeServices._extensions, target, type, name, new object[1]
      {
        value
      }).CreateSetter();
    }

    public static void PropagateValueTypeChanges(RuntimeServices.ValueTypeChange[] changes)
    {
      foreach (RuntimeServices.ValueTypeChange change in changes)
      {
        if (!(change.Value is ValueType))
          break;
        try
        {
          RuntimeServices.SetProperty(change.Target, change.Member, change.Value);
        }
        catch (MissingFieldException ex)
        {
          break;
        }
      }
    }

    public static object Coerce(object value, Type toType)
    {
      if (value == null)
        return (object) null;
      object[] args = new object[1]{ (object) toType };
      return RuntimeServices.GetDispatcher(value, "$Coerce$", new Type[1]
      {
        toType
      }, (DispatcherCache.DispatcherFactory) (() => RuntimeServices.CreateCoerceDispatcher(value, toType)))(value, args);
    }

    private static Dispatcher CreateCoerceDispatcher(object value, Type toType)
    {
      if (toType.IsInstanceOfType(value))
        return new Dispatcher(RuntimeServices.IdentityDispatcher);
      if (value is ICoercible)
        return new Dispatcher(RuntimeServices.CoercibleDispatcher);
      Type type = value.GetType();
      if (RuntimeServices.IsPromotableNumeric(type) && RuntimeServices.IsPromotableNumeric(toType))
        return RuntimeServices.EmitPromotionDispatcher(type, toType);
      MethodInfo conversionOperator = RuntimeServices.FindImplicitConversionOperator(type, toType);
      return conversionOperator == null ? new Dispatcher(RuntimeServices.IdentityDispatcher) : RuntimeServices.EmitImplicitConversionDispatcher(conversionOperator);
    }

    private static Dispatcher EmitPromotionDispatcher(Type fromType, Type toType) => (Dispatcher) Delegate.CreateDelegate(typeof (Dispatcher), typeof (NumericPromotions).GetMethod("From" + (object) Type.GetTypeCode(fromType) + "To" + (object) Type.GetTypeCode(toType)));

    private static bool IsPromotableNumeric(Type fromType) => RuntimeServices.IsPromotableNumeric(Type.GetTypeCode(fromType));

    private static Dispatcher EmitImplicitConversionDispatcher(MethodInfo method) => new ImplicitConversionEmitter(method).Emit();

    private static object CoercibleDispatcher(object o, object[] args) => ((ICoercible) o).Coerce((Type) args[0]);

    private static object IdentityDispatcher(object o, object[] args) => o;

    public static object GetSlice(object target, string name, object[] args) => RuntimeServices.GetDispatcher(target, args, name + "[]", (DispatcherCache.DispatcherFactory) (() => RuntimeServices.CreateGetSliceDispatcher(target, name, args)))(target, args);

    private static Dispatcher CreateGetSliceDispatcher(object target, string name, object[] args)
    {
      if (target is IQuackFu)
        return (Dispatcher) ((o, arguments) => ((IQuackFu) o).QuackGet(name, arguments));
      return string.Empty == name && args.Length == 1 && target is Array ? new Dispatcher(RuntimeServices.GetArraySlice) : new SliceDispatcherFactory(RuntimeServices._extensions, target, target.GetType(), name, args).CreateGetter();
    }

    private static object GetArraySlice(object target, object[] args)
    {
      IList list = (IList) target;
      return list[RuntimeServices.NormalizeIndex(list.Count, (int) args[0])];
    }

    public static object SetSlice(object target, string name, object[] args) => RuntimeServices.GetDispatcher(target, args, name + "[]=", (DispatcherCache.DispatcherFactory) (() => RuntimeServices.CreateSetSliceDispatcher(target, name, args)))(target, args);

    private static Dispatcher CreateSetSliceDispatcher(object target, string name, object[] args)
    {
      if (target is IQuackFu)
        return (Dispatcher) ((o, arguments) => ((IQuackFu) o).QuackSet(name, (object[]) RuntimeServices.GetRange2((Array) arguments, 0, arguments.Length - 1), arguments[arguments.Length - 1]));
      return string.Empty == name && args.Length == 2 && target is Array ? new Dispatcher(RuntimeServices.SetArraySlice) : new SliceDispatcherFactory(RuntimeServices._extensions, target, target.GetType(), name, args).CreateSetter();
    }

    private static object SetArraySlice(object target, object[] args)
    {
      IList list = (IList) target;
      list[RuntimeServices.NormalizeIndex(list.Count, (int) args[0])] = args[1];
      return args[1];
    }

    internal static string GetDefaultMemberName(Type type)
    {
      DefaultMemberAttribute customAttribute = (DefaultMemberAttribute) Attribute.GetCustomAttribute((MemberInfo) type, typeof (DefaultMemberAttribute));
      return customAttribute != null ? customAttribute.MemberName : string.Empty;
    }

    public static object InvokeCallable(object target, object[] args)
    {
      if (target == null)
        throw new ArgumentNullException(nameof (target));
      if (args == null)
        throw new ArgumentNullException(nameof (args));
      if (target is ICallable callable)
        return callable.Call(args);
      Delegate @delegate = target as Delegate;
      if ((object) @delegate != null)
        return @delegate.DynamicInvoke(args);
      return target is Type type ? Activator.CreateInstance(type, args) : ((MethodBase) target).Invoke((object) null, args);
    }

    private static bool IsNumeric(TypeCode code)
    {
      switch (code)
      {
        case TypeCode.SByte:
          return true;
        case TypeCode.Byte:
          return true;
        case TypeCode.Int16:
          return true;
        case TypeCode.UInt16:
          return true;
        case TypeCode.Int32:
          return true;
        case TypeCode.UInt32:
          return true;
        case TypeCode.Int64:
          return true;
        case TypeCode.UInt64:
          return true;
        case TypeCode.Single:
          return true;
        case TypeCode.Double:
          return true;
        case TypeCode.Decimal:
          return true;
        default:
          return false;
      }
    }

    public static object InvokeBinaryOperator(string operatorName, object lhs, object rhs)
    {
      Type type1 = lhs.GetType();
      Type type2 = rhs.GetType();
      TypeCode typeCode1 = Type.GetTypeCode(type1);
      TypeCode typeCode2 = Type.GetTypeCode(type2);
      if (RuntimeServices.IsNumeric(typeCode1) && RuntimeServices.IsNumeric(typeCode2))
      {
        int num = ((int) operatorName[3] << 8) + (int) operatorName[operatorName.Length - 1];
        switch (num)
        {
          case 18284:
            return (object) RuntimeServices.op_GreaterThanOrEqual(lhs, typeCode1, rhs, typeCode2);
          case 18286:
            return (object) RuntimeServices.op_GreaterThan(lhs, typeCode1, rhs, typeCode2);
          default:
            switch (num - 19564)
            {
              case 0:
                return (object) RuntimeServices.op_LessThanOrEqual(lhs, typeCode1, rhs, typeCode2);
              case 2:
                return (object) RuntimeServices.op_LessThan(lhs, typeCode1, rhs, typeCode2);
              default:
                if (num != 19826)
                {
                  if (num == 19827)
                    return RuntimeServices.op_Modulus(lhs, typeCode1, rhs, typeCode2);
                  if (num == 16750)
                    return RuntimeServices.op_Addition(lhs, typeCode1, rhs, typeCode2);
                  if (num == 16996)
                    return RuntimeServices.op_BitwiseAnd(lhs, typeCode1, rhs, typeCode2);
                  if (num == 17010)
                    return RuntimeServices.op_BitwiseOr(lhs, typeCode1, rhs, typeCode2);
                  if (num == 17518)
                    return RuntimeServices.op_Division(lhs, typeCode1, rhs, typeCode2);
                  if (num == 17774)
                    return (object) RuntimeServices.op_Exponentiation(lhs, typeCode1, rhs, typeCode2);
                  if (num == 17778)
                    return RuntimeServices.op_ExclusiveOr(lhs, typeCode1, rhs, typeCode2);
                  if (num != 19816)
                  {
                    if (num == 19833)
                      return RuntimeServices.op_Multiply(lhs, typeCode1, rhs, typeCode2);
                    if (num != 20072 && num != 20082)
                    {
                      if (num == 21358)
                        return RuntimeServices.op_Subtraction(lhs, typeCode1, rhs, typeCode2);
                      if (num == 21364)
                        return operatorName[8] == 'L' ? RuntimeServices.op_ShiftLeft(lhs, typeCode1, rhs, typeCode2) : RuntimeServices.op_ShiftRight(lhs, typeCode1, rhs, typeCode2);
                    }
                  }
                }
                throw new MissingMethodException(RuntimeServices.MissingOperatorMessageFor(operatorName, type1, type2));
            }
        }
      }
      else
      {
        object[] args = new object[2]{ lhs, rhs };
        if (lhs is IQuackFu quackFu1)
          return quackFu1.QuackInvoke(operatorName, args);
        if (rhs is IQuackFu quackFu2)
          return quackFu2.QuackInvoke(operatorName, args);
        try
        {
          return RuntimeServices.Invoke((object) type1, operatorName, args);
        }
        catch (MissingMethodException ex1)
        {
          try
          {
            return RuntimeServices.Invoke((object) type2, operatorName, args);
          }
          catch (MissingMethodException ex2)
          {
            try
            {
              return RuntimeServices.InvokeRuntimeServicesOperator(operatorName, args);
            }
            catch (MissingMethodException ex3)
            {
            }
          }
          throw new MissingMethodException(RuntimeServices.MissingOperatorMessageFor(operatorName, type1, type2), (Exception) ex1);
        }
      }
    }

    private static string MissingOperatorMessageFor(
      string operatorName,
      Type lhsType,
      Type rhsType)
    {
      return string.Format("{0} is not applicable to operands '{1}' and '{2}'.", (object) RuntimeServices.FormatOperatorName(operatorName), (object) lhsType, (object) rhsType);
    }

    private static string FormatOperatorName(string operatorName)
    {
      StringBuilder stringBuilder = new StringBuilder(operatorName.Length);
      stringBuilder.Append(operatorName[3]);
      foreach (char c in operatorName.Substring(4))
      {
        if (char.IsUpper(c))
        {
          stringBuilder.Append(" ");
          stringBuilder.Append(char.ToLower(c));
        }
        else
          stringBuilder.Append(c);
      }
      return stringBuilder.ToString();
    }

    public static object InvokeUnaryOperator(string operatorName, object operand)
    {
      Type type = operand.GetType();
      TypeCode typeCode = Type.GetTypeCode(type);
      if (RuntimeServices.IsNumeric(typeCode))
      {
        if (((int) operatorName[3] << 8) + (int) operatorName[operatorName.Length - 1] == 21870)
          return RuntimeServices.op_UnaryNegation(operand, typeCode);
        throw new ArgumentException(operatorName + " " + operand);
      }
      object[] args = new object[1]{ operand };
      if (operand is IQuackFu quackFu)
        return quackFu.QuackInvoke(operatorName, args);
      try
      {
        return RuntimeServices.Invoke((object) type, operatorName, args);
      }
      catch (MissingMethodException ex1)
      {
        try
        {
          return RuntimeServices.InvokeRuntimeServicesOperator(operatorName, args);
        }
        catch (MissingMethodException ex2)
        {
        }
        throw;
      }
    }

    private static object InvokeRuntimeServicesOperator(string operatorName, object[] args) => RuntimeServices.Invoke((object) RuntimeServices.RuntimeServicesType, operatorName, args);

    public static object MoveNext(IEnumerator enumerator)
    {
      if (enumerator == null)
        throw new ApplicationException("Cannot unpack null.");
      return enumerator.MoveNext() ? enumerator.Current : throw new ApplicationException("Unpack list of wrong size.");
    }

    public static int Len(object obj)
    {
      switch (obj)
      {
        case ICollection collection:
          return collection.Count;
        case string str:
          return str.Length;
        default:
          throw new ArgumentException();
      }
    }

    public static string Mid(string s, int begin, int end)
    {
      begin = RuntimeServices.NormalizeStringIndex(s, begin);
      end = RuntimeServices.NormalizeStringIndex(s, end);
      return s.Substring(begin, end - begin);
    }

    public static Array GetRange1(Array source, int begin) => RuntimeServices.GetRange2(source, begin, source.Length);

    public static Array GetRange2(Array source, int begin, int end)
    {
      int length1 = source.Length;
      begin = RuntimeServices.NormalizeIndex(length1, begin);
      end = RuntimeServices.NormalizeIndex(length1, end);
      int length2 = Math.Max(0, end - begin);
      Array instance = Array.CreateInstance(source.GetType().GetElementType(), length2);
      Array.Copy(source, begin, instance, 0, length2);
      return instance;
    }

    public static void SetMultiDimensionalRange1(
      Array source,
      Array dest,
      int[] ranges,
      bool[] collapse)
    {
      if (dest.Rank != ranges.Length / 2)
        throw new Exception("invalid range passed: " + (object) (ranges.Length / 2) + ", expected " + (object) (dest.Rank * 2));
      for (int dimension = 0; dimension < dest.Rank; ++dimension)
      {
        if (ranges[2 * dimension] > 0 || ranges[2 * dimension] > dest.GetLength(dimension) || ranges[2 * dimension + 1] > dest.GetLength(dimension) || ranges[2 * dimension + 1] < ranges[2 * dimension])
          throw new ApplicationException("Invalid array.");
      }
      int length = 0;
      foreach (bool flag in collapse)
      {
        if (!flag)
          ++length;
      }
      if (source.Rank != length)
        throw new ApplicationException("Invalid array.");
      int[] numArray1 = new int[dest.Rank];
      int[] numArray2 = new int[length];
      int dimension1 = 0;
      for (int index = 0; index < dest.Rank; ++index)
      {
        numArray1[index] = ranges[2 * index + 1] - ranges[2 * index];
        if (!collapse[index])
        {
          numArray2[dimension1] = numArray1[index] - ranges[2 * index];
          if (numArray2[dimension1] != source.GetLength(dimension1))
            throw new ApplicationException("Invalid array.");
          ++dimension1;
        }
      }
      int[] numArray3 = new int[dest.Rank];
      for (int index = 0; index < dest.Rank; ++index)
        numArray3[index] = index != 0 ? numArray3[index - 1] / numArray1[index - 1] : source.Length / numArray1[numArray1.Length - 1];
      int[] numArray4 = new int[dest.Rank];
      int[] numArray5 = new int[length];
      for (int index1 = 0; index1 < source.Length; ++index1)
      {
        int index2 = 0;
        for (int index3 = 0; index3 < dest.Rank; ++index3)
        {
          int num = index1 % numArray3[index3] / (numArray3[index3] / numArray1[index3]);
          numArray4[index3] = num;
          if (!collapse[index3])
          {
            numArray5[index2] = numArray4[index3] + ranges[2 * index3];
            ++index2;
          }
          dest.SetValue(source.GetValue(numArray5), numArray4);
        }
      }
    }

    public static Array GetMultiDimensionalRange1(Array source, int[] ranges, bool[] collapse)
    {
      int rank = source.Rank;
      int num1 = 0;
      foreach (bool flag in collapse)
      {
        if (flag)
          ++num1;
      }
      int length = rank - num1;
      int[] numArray1 = new int[length];
      int[] numArray2 = new int[rank];
      int index1 = 0;
      for (int dimension = 0; dimension < rank; ++dimension)
      {
        ranges[2 * dimension] = RuntimeServices.NormalizeIndex(source.GetLength(dimension), ranges[2 * dimension]);
        ranges[2 * dimension + 1] = RuntimeServices.NormalizeIndex(source.GetLength(dimension), ranges[2 * dimension + 1]);
        numArray2[dimension] = ranges[2 * dimension + 1] - ranges[2 * dimension];
        if (!collapse[dimension])
        {
          numArray1[index1] = ranges[2 * dimension + 1] - ranges[2 * dimension];
          ++index1;
        }
      }
      Array instance = Array.CreateInstance(source.GetType().GetElementType(), numArray1);
      int[] numArray3 = new int[rank];
      int[] numArray4 = new int[length];
      int[] numArray5 = new int[rank];
      for (int index2 = 0; index2 < rank; ++index2)
        numArray3[index2] = index2 != 0 ? numArray3[index2 - 1] / numArray2[index2 - 1] : instance.Length;
      for (int index3 = 0; index3 < instance.Length; ++index3)
      {
        int index4 = 0;
        for (int index5 = 0; index5 < rank; ++index5)
        {
          int num2 = index3 % numArray3[index5] / (numArray3[index5] / numArray2[index5]);
          numArray5[index5] = ranges[2 * index5] + num2;
          if (!collapse[index5])
          {
            numArray4[index4] = numArray5[index5] - ranges[2 * index5];
            ++index4;
          }
        }
        instance.SetValue(source.GetValue(numArray5), numArray4);
      }
      return instance;
    }

    public static void CheckArrayUnpack(Array array, int expected)
    {
      if (array == null)
        throw new ApplicationException("Cannot unpack null.");
      if (expected <= array.Length)
        return;
      RuntimeServices.Error("Unpack array of wrong size (expected={0}, actual={1}).", (object) expected, (object) array.Length);
    }

    public static int NormalizeIndex(int len, int index) => index < 0 ? Math.Max(0, index + len) : Math.Min(index, len);

    public static int NormalizeArrayIndex(Array array, int index) => index < 0 ? Math.Max(0, index + array.Length) : Math.Min(index, array.Length);

    public static int NormalizeStringIndex(string s, int index) => index < 0 ? Math.Max(0, index + s.Length) : Math.Min(index, s.Length);

    public static IEnumerable GetEnumerable(object enumerable)
    {
      switch (enumerable)
      {
        case null:
          throw new ApplicationException("Cannot enumerate null.");
        case IEnumerable enumerable1:
          return enumerable1;
        case TextReader reader:
          return (IEnumerable) TextReaderEnumerator.lines(reader);
        default:
          throw new ApplicationException("Argument is not enumerable (does not implement System.Collections.IEnumerable).");
      }
    }

    public static Array AddArrays(Type resultingElementType, Array lhs, Array rhs)
    {
      int length = lhs.Length + rhs.Length;
      Array instance = Array.CreateInstance(resultingElementType, length);
      Array.Copy(lhs, 0, instance, 0, lhs.Length);
      Array.Copy(rhs, 0, instance, lhs.Length, rhs.Length);
      return instance;
    }

    public static string operator +(string lhs, string rhs) => lhs + rhs;

    public static string operator +(string lhs, object rhs) => lhs + rhs;

    public static string operator +(object lhs, string rhs) => lhs.ToString() + rhs;

    public static Array operator *(Array lhs, int count)
    {
      if (count < 0)
        throw new ArgumentOutOfRangeException(nameof (count));
      Type type = lhs.GetType();
      if (type.GetArrayRank() != 1)
        throw new ArgumentException(nameof (lhs));
      int length = lhs.Length;
      Array instance = Array.CreateInstance(type.GetElementType(), length * count);
      int destinationIndex = 0;
      for (int index = 0; index < count; ++index)
      {
        Array.Copy(lhs, 0, instance, destinationIndex, length);
        destinationIndex += length;
      }
      return instance;
    }

    public static Array operator *(int count, Array rhs) => RuntimeServices.op_Multiply(rhs, count);

    public static string operator *(string lhs, int count)
    {
      if (count < 0)
        throw new ArgumentOutOfRangeException(nameof (count));
      string str = (string) null;
      if (lhs != null)
      {
        StringBuilder stringBuilder = new StringBuilder(lhs.Length * count);
        for (int index = 0; index < count; ++index)
          stringBuilder.Append(lhs);
        str = stringBuilder.ToString();
      }
      return str;
    }

    public static string operator *(int count, string rhs) => RuntimeServices.op_Multiply(rhs, count);

    public static bool op_NotMember(string lhs, string rhs) => !RuntimeServices.op_Member(lhs, rhs);

    public static bool op_Member(string lhs, string rhs) => lhs != null && rhs != null && rhs.IndexOf(lhs) > -1;

    public static bool op_Member(char lhs, string rhs) => rhs != null && rhs.IndexOf(lhs) > -1;

    public static bool op_Match(string input, Regex pattern) => pattern.IsMatch(input);

    public static bool op_Match(string input, string pattern) => Regex.IsMatch(input, pattern);

    public static bool op_NotMatch(string input, Regex pattern) => !RuntimeServices.op_Match(input, pattern);

    public static bool op_NotMatch(string input, string pattern) => !RuntimeServices.op_Match(input, pattern);

    public static string operator %(string lhs, IEnumerable rhs) => string.Format(lhs, Builtins.array(rhs));

    public static string operator %(string lhs, object[] rhs) => string.Format(lhs, rhs);

    public static bool op_Member(object lhs, IList rhs) => rhs != null && rhs.Contains(lhs);

    public static bool op_NotMember(object lhs, IList rhs) => !RuntimeServices.op_Member(lhs, rhs);

    public static bool op_Member(object lhs, IDictionary rhs) => rhs != null && rhs.Contains(lhs);

    public static bool op_NotMember(object lhs, IDictionary rhs) => !RuntimeServices.op_Member(lhs, rhs);

    public static bool op_Member(object lhs, IEnumerable rhs)
    {
      if (rhs == null)
        return false;
      foreach (object rh in rhs)
      {
        if (RuntimeServices.EqualityOperator(lhs, rh))
          return true;
      }
      return false;
    }

    public static bool op_NotMember(object lhs, IEnumerable rhs) => !RuntimeServices.op_Member(lhs, rhs);

    public static bool EqualityOperator(object lhs, object rhs)
    {
      if (lhs == rhs)
        return true;
      if (lhs == null)
        return rhs.Equals(lhs);
      if (rhs == null)
        return lhs.Equals(rhs);
      TypeCode typeCode1 = Type.GetTypeCode(lhs.GetType());
      TypeCode typeCode2 = Type.GetTypeCode(rhs.GetType());
      if (RuntimeServices.IsNumeric(typeCode1) && RuntimeServices.IsNumeric(typeCode2))
        return RuntimeServices.EqualityOperator(lhs, typeCode1, rhs, typeCode2);
      if (lhs is Array lhs1 && rhs is Array rhs1)
        return RuntimeServices.ArrayEqualityImpl(lhs1, rhs1);
      return lhs.Equals(rhs) || rhs.Equals(lhs);
    }

    public static bool operator ==(Array lhs, Array rhs)
    {
      if (lhs == rhs)
        return true;
      return lhs != null && rhs != null && RuntimeServices.ArrayEqualityImpl(lhs, rhs);
    }

    private static bool ArrayEqualityImpl(Array lhs, Array rhs)
    {
      if (lhs.Rank != 1 || rhs.Rank != 1)
        throw new ArgumentException("array rank must be 1");
      if (lhs.Length != rhs.Length)
        return false;
      for (int index = 0; index < lhs.Length; ++index)
      {
        if (!RuntimeServices.EqualityOperator(lhs.GetValue(index), rhs.GetValue(index)))
          return false;
      }
      return true;
    }

    private static TypeCode GetConvertTypeCode(TypeCode lhsTypeCode, TypeCode rhsTypeCode)
    {
      if (lhsTypeCode == TypeCode.Decimal || rhsTypeCode == TypeCode.Decimal)
        return TypeCode.Decimal;
      if (lhsTypeCode == TypeCode.Double || rhsTypeCode == TypeCode.Double)
        return TypeCode.Double;
      if (lhsTypeCode == TypeCode.Single || rhsTypeCode == TypeCode.Single)
        return TypeCode.Single;
      if (lhsTypeCode == TypeCode.UInt64)
        return rhsTypeCode == TypeCode.SByte || rhsTypeCode == TypeCode.Int16 || rhsTypeCode == TypeCode.Int32 || rhsTypeCode == TypeCode.Int64 ? TypeCode.Int64 : TypeCode.UInt64;
      if (rhsTypeCode == TypeCode.UInt64)
        return lhsTypeCode == TypeCode.SByte || lhsTypeCode == TypeCode.Int16 || lhsTypeCode == TypeCode.Int32 || lhsTypeCode == TypeCode.Int64 ? TypeCode.Int64 : TypeCode.UInt64;
      if (lhsTypeCode == TypeCode.Int64 || rhsTypeCode == TypeCode.Int64)
        return TypeCode.Int64;
      if (lhsTypeCode == TypeCode.UInt32)
        return rhsTypeCode == TypeCode.SByte || rhsTypeCode == TypeCode.Int16 || rhsTypeCode == TypeCode.Int32 ? TypeCode.Int64 : TypeCode.UInt32;
      if (rhsTypeCode != TypeCode.UInt32)
        return TypeCode.Int32;
      return lhsTypeCode == TypeCode.SByte || lhsTypeCode == TypeCode.Int16 || lhsTypeCode == TypeCode.Int32 ? TypeCode.Int64 : TypeCode.UInt32;
    }

    private static object operator *(
      object lhs,
      TypeCode lhsTypeCode,
      object rhs,
      TypeCode rhsTypeCode)
    {
      IConvertible convertible1 = (IConvertible) lhs;
      IConvertible convertible2 = (IConvertible) rhs;
      switch (RuntimeServices.GetConvertTypeCode(lhsTypeCode, rhsTypeCode))
      {
        case TypeCode.UInt32:
          return (object) (uint) ((int) convertible1.ToUInt32((IFormatProvider) null) * (int) convertible2.ToUInt32((IFormatProvider) null));
        case TypeCode.Int64:
          return (object) (convertible1.ToInt64((IFormatProvider) null) * convertible2.ToInt64((IFormatProvider) null));
        case TypeCode.UInt64:
          return (object) (ulong) ((long) convertible1.ToUInt64((IFormatProvider) null) * (long) convertible2.ToUInt64((IFormatProvider) null));
        case TypeCode.Single:
          return (object) (float) ((double) convertible1.ToSingle((IFormatProvider) null) * (double) convertible2.ToSingle((IFormatProvider) null));
        case TypeCode.Double:
          return (object) (convertible1.ToDouble((IFormatProvider) null) * convertible2.ToDouble((IFormatProvider) null));
        case TypeCode.Decimal:
          return (object) (convertible1.ToDecimal((IFormatProvider) null) * convertible2.ToDecimal((IFormatProvider) null));
        default:
          return (object) (convertible1.ToInt32((IFormatProvider) null) * convertible2.ToInt32((IFormatProvider) null));
      }
    }

    private static object operator /(
      object lhs,
      TypeCode lhsTypeCode,
      object rhs,
      TypeCode rhsTypeCode)
    {
      IConvertible convertible1 = (IConvertible) lhs;
      IConvertible convertible2 = (IConvertible) rhs;
      switch (RuntimeServices.GetConvertTypeCode(lhsTypeCode, rhsTypeCode))
      {
        case TypeCode.UInt32:
          return (object) (convertible1.ToUInt32((IFormatProvider) null) / convertible2.ToUInt32((IFormatProvider) null));
        case TypeCode.Int64:
          return (object) (convertible1.ToInt64((IFormatProvider) null) / convertible2.ToInt64((IFormatProvider) null));
        case TypeCode.UInt64:
          return (object) (convertible1.ToUInt64((IFormatProvider) null) / convertible2.ToUInt64((IFormatProvider) null));
        case TypeCode.Single:
          return (object) (float) ((double) convertible1.ToSingle((IFormatProvider) null) / (double) convertible2.ToSingle((IFormatProvider) null));
        case TypeCode.Double:
          return (object) (convertible1.ToDouble((IFormatProvider) null) / convertible2.ToDouble((IFormatProvider) null));
        case TypeCode.Decimal:
          return (object) (convertible1.ToDecimal((IFormatProvider) null) / convertible2.ToDecimal((IFormatProvider) null));
        default:
          return (object) (convertible1.ToInt32((IFormatProvider) null) / convertible2.ToInt32((IFormatProvider) null));
      }
    }

    private static object operator +(
      object lhs,
      TypeCode lhsTypeCode,
      object rhs,
      TypeCode rhsTypeCode)
    {
      IConvertible convertible1 = (IConvertible) lhs;
      IConvertible convertible2 = (IConvertible) rhs;
      switch (RuntimeServices.GetConvertTypeCode(lhsTypeCode, rhsTypeCode))
      {
        case TypeCode.UInt32:
          return (object) (uint) ((int) convertible1.ToUInt32((IFormatProvider) null) + (int) convertible2.ToUInt32((IFormatProvider) null));
        case TypeCode.Int64:
          return (object) (convertible1.ToInt64((IFormatProvider) null) + convertible2.ToInt64((IFormatProvider) null));
        case TypeCode.UInt64:
          return (object) (ulong) ((long) convertible1.ToUInt64((IFormatProvider) null) + (long) convertible2.ToUInt64((IFormatProvider) null));
        case TypeCode.Single:
          return (object) (float) ((double) convertible1.ToSingle((IFormatProvider) null) + (double) convertible2.ToSingle((IFormatProvider) null));
        case TypeCode.Double:
          return (object) (convertible1.ToDouble((IFormatProvider) null) + convertible2.ToDouble((IFormatProvider) null));
        case TypeCode.Decimal:
          return (object) (convertible1.ToDecimal((IFormatProvider) null) + convertible2.ToDecimal((IFormatProvider) null));
        default:
          return (object) (convertible1.ToInt32((IFormatProvider) null) + convertible2.ToInt32((IFormatProvider) null));
      }
    }

    private static object operator -(
      object lhs,
      TypeCode lhsTypeCode,
      object rhs,
      TypeCode rhsTypeCode)
    {
      IConvertible convertible1 = (IConvertible) lhs;
      IConvertible convertible2 = (IConvertible) rhs;
      switch (RuntimeServices.GetConvertTypeCode(lhsTypeCode, rhsTypeCode))
      {
        case TypeCode.UInt32:
          return (object) (uint) ((int) convertible1.ToUInt32((IFormatProvider) null) - (int) convertible2.ToUInt32((IFormatProvider) null));
        case TypeCode.Int64:
          return (object) (convertible1.ToInt64((IFormatProvider) null) - convertible2.ToInt64((IFormatProvider) null));
        case TypeCode.UInt64:
          return (object) (ulong) ((long) convertible1.ToUInt64((IFormatProvider) null) - (long) convertible2.ToUInt64((IFormatProvider) null));
        case TypeCode.Single:
          return (object) (float) ((double) convertible1.ToSingle((IFormatProvider) null) - (double) convertible2.ToSingle((IFormatProvider) null));
        case TypeCode.Double:
          return (object) (convertible1.ToDouble((IFormatProvider) null) - convertible2.ToDouble((IFormatProvider) null));
        case TypeCode.Decimal:
          return (object) (convertible1.ToDecimal((IFormatProvider) null) - convertible2.ToDecimal((IFormatProvider) null));
        default:
          return (object) (convertible1.ToInt32((IFormatProvider) null) - convertible2.ToInt32((IFormatProvider) null));
      }
    }

    private static bool EqualityOperator(
      object lhs,
      TypeCode lhsTypeCode,
      object rhs,
      TypeCode rhsTypeCode)
    {
      IConvertible convertible1 = (IConvertible) lhs;
      IConvertible convertible2 = (IConvertible) rhs;
      switch (RuntimeServices.GetConvertTypeCode(lhsTypeCode, rhsTypeCode))
      {
        case TypeCode.UInt32:
          return (int) convertible1.ToUInt32((IFormatProvider) null) == (int) convertible2.ToUInt32((IFormatProvider) null);
        case TypeCode.Int64:
          return convertible1.ToInt64((IFormatProvider) null) == convertible2.ToInt64((IFormatProvider) null);
        case TypeCode.UInt64:
          return (long) convertible1.ToUInt64((IFormatProvider) null) == (long) convertible2.ToUInt64((IFormatProvider) null);
        case TypeCode.Single:
          return (double) convertible1.ToSingle((IFormatProvider) null) == (double) convertible2.ToSingle((IFormatProvider) null);
        case TypeCode.Double:
          return convertible1.ToDouble((IFormatProvider) null) == convertible2.ToDouble((IFormatProvider) null);
        case TypeCode.Decimal:
          return convertible1.ToDecimal((IFormatProvider) null) == convertible2.ToDecimal((IFormatProvider) null);
        default:
          return convertible1.ToInt32((IFormatProvider) null) == convertible2.ToInt32((IFormatProvider) null);
      }
    }

    private static bool operator >(
      object lhs,
      TypeCode lhsTypeCode,
      object rhs,
      TypeCode rhsTypeCode)
    {
      IConvertible convertible1 = (IConvertible) lhs;
      IConvertible convertible2 = (IConvertible) rhs;
      switch (RuntimeServices.GetConvertTypeCode(lhsTypeCode, rhsTypeCode))
      {
        case TypeCode.UInt32:
          return convertible1.ToUInt32((IFormatProvider) null) > convertible2.ToUInt32((IFormatProvider) null);
        case TypeCode.Int64:
          return convertible1.ToInt64((IFormatProvider) null) > convertible2.ToInt64((IFormatProvider) null);
        case TypeCode.UInt64:
          return convertible1.ToUInt64((IFormatProvider) null) > convertible2.ToUInt64((IFormatProvider) null);
        case TypeCode.Single:
          return (double) convertible1.ToSingle((IFormatProvider) null) > (double) convertible2.ToSingle((IFormatProvider) null);
        case TypeCode.Double:
          return convertible1.ToDouble((IFormatProvider) null) > convertible2.ToDouble((IFormatProvider) null);
        case TypeCode.Decimal:
          return convertible1.ToDecimal((IFormatProvider) null) > convertible2.ToDecimal((IFormatProvider) null);
        default:
          return convertible1.ToInt32((IFormatProvider) null) > convertible2.ToInt32((IFormatProvider) null);
      }
    }

    private static bool operator >=(
      object lhs,
      TypeCode lhsTypeCode,
      object rhs,
      TypeCode rhsTypeCode)
    {
      IConvertible convertible1 = (IConvertible) lhs;
      IConvertible convertible2 = (IConvertible) rhs;
      switch (RuntimeServices.GetConvertTypeCode(lhsTypeCode, rhsTypeCode))
      {
        case TypeCode.UInt32:
          return convertible1.ToUInt32((IFormatProvider) null) >= convertible2.ToUInt32((IFormatProvider) null);
        case TypeCode.Int64:
          return convertible1.ToInt64((IFormatProvider) null) >= convertible2.ToInt64((IFormatProvider) null);
        case TypeCode.UInt64:
          return convertible1.ToUInt64((IFormatProvider) null) >= convertible2.ToUInt64((IFormatProvider) null);
        case TypeCode.Single:
          return (double) convertible1.ToSingle((IFormatProvider) null) >= (double) convertible2.ToSingle((IFormatProvider) null);
        case TypeCode.Double:
          return convertible1.ToDouble((IFormatProvider) null) >= convertible2.ToDouble((IFormatProvider) null);
        case TypeCode.Decimal:
          return convertible1.ToDecimal((IFormatProvider) null) >= convertible2.ToDecimal((IFormatProvider) null);
        default:
          return convertible1.ToInt32((IFormatProvider) null) >= convertible2.ToInt32((IFormatProvider) null);
      }
    }

    private static bool operator <(
      object lhs,
      TypeCode lhsTypeCode,
      object rhs,
      TypeCode rhsTypeCode)
    {
      IConvertible convertible1 = (IConvertible) lhs;
      IConvertible convertible2 = (IConvertible) rhs;
      switch (RuntimeServices.GetConvertTypeCode(lhsTypeCode, rhsTypeCode))
      {
        case TypeCode.UInt32:
          return convertible1.ToUInt32((IFormatProvider) null) < convertible2.ToUInt32((IFormatProvider) null);
        case TypeCode.Int64:
          return convertible1.ToInt64((IFormatProvider) null) < convertible2.ToInt64((IFormatProvider) null);
        case TypeCode.UInt64:
          return convertible1.ToUInt64((IFormatProvider) null) < convertible2.ToUInt64((IFormatProvider) null);
        case TypeCode.Single:
          return (double) convertible1.ToSingle((IFormatProvider) null) < (double) convertible2.ToSingle((IFormatProvider) null);
        case TypeCode.Double:
          return convertible1.ToDouble((IFormatProvider) null) < convertible2.ToDouble((IFormatProvider) null);
        case TypeCode.Decimal:
          return convertible1.ToDecimal((IFormatProvider) null) < convertible2.ToDecimal((IFormatProvider) null);
        default:
          return convertible1.ToInt32((IFormatProvider) null) < convertible2.ToInt32((IFormatProvider) null);
      }
    }

    private static bool operator <=(
      object lhs,
      TypeCode lhsTypeCode,
      object rhs,
      TypeCode rhsTypeCode)
    {
      IConvertible convertible1 = (IConvertible) lhs;
      IConvertible convertible2 = (IConvertible) rhs;
      switch (RuntimeServices.GetConvertTypeCode(lhsTypeCode, rhsTypeCode))
      {
        case TypeCode.UInt32:
          return convertible1.ToUInt32((IFormatProvider) null) <= convertible2.ToUInt32((IFormatProvider) null);
        case TypeCode.Int64:
          return convertible1.ToInt64((IFormatProvider) null) <= convertible2.ToInt64((IFormatProvider) null);
        case TypeCode.UInt64:
          return convertible1.ToUInt64((IFormatProvider) null) <= convertible2.ToUInt64((IFormatProvider) null);
        case TypeCode.Single:
          return (double) convertible1.ToSingle((IFormatProvider) null) <= (double) convertible2.ToSingle((IFormatProvider) null);
        case TypeCode.Double:
          return convertible1.ToDouble((IFormatProvider) null) <= convertible2.ToDouble((IFormatProvider) null);
        case TypeCode.Decimal:
          return convertible1.ToDecimal((IFormatProvider) null) <= convertible2.ToDecimal((IFormatProvider) null);
        default:
          return convertible1.ToInt32((IFormatProvider) null) <= convertible2.ToInt32((IFormatProvider) null);
      }
    }

    private static object operator %(
      object lhs,
      TypeCode lhsTypeCode,
      object rhs,
      TypeCode rhsTypeCode)
    {
      IConvertible convertible1 = (IConvertible) lhs;
      IConvertible convertible2 = (IConvertible) rhs;
      switch (RuntimeServices.GetConvertTypeCode(lhsTypeCode, rhsTypeCode))
      {
        case TypeCode.UInt32:
          return (object) (convertible1.ToUInt32((IFormatProvider) null) % convertible2.ToUInt32((IFormatProvider) null));
        case TypeCode.Int64:
          return (object) (convertible1.ToInt64((IFormatProvider) null) % convertible2.ToInt64((IFormatProvider) null));
        case TypeCode.UInt64:
          return (object) (convertible1.ToUInt64((IFormatProvider) null) % convertible2.ToUInt64((IFormatProvider) null));
        case TypeCode.Single:
          return (object) (float) ((double) convertible1.ToSingle((IFormatProvider) null) % (double) convertible2.ToSingle((IFormatProvider) null));
        case TypeCode.Double:
          return (object) (convertible1.ToDouble((IFormatProvider) null) % convertible2.ToDouble((IFormatProvider) null));
        case TypeCode.Decimal:
          return (object) (convertible1.ToDecimal((IFormatProvider) null) % convertible2.ToDecimal((IFormatProvider) null));
        default:
          return (object) (convertible1.ToInt32((IFormatProvider) null) % convertible2.ToInt32((IFormatProvider) null));
      }
    }

    private static double op_Exponentiation(
      object lhs,
      TypeCode lhsTypeCode,
      object rhs,
      TypeCode rhsTypeCode)
    {
      IConvertible convertible1 = (IConvertible) lhs;
      IConvertible convertible2 = (IConvertible) rhs;
      return Math.Pow(convertible1.ToDouble((IFormatProvider) null), convertible2.ToDouble((IFormatProvider) null));
    }

    private static object operator &(
      object lhs,
      TypeCode lhsTypeCode,
      object rhs,
      TypeCode rhsTypeCode)
    {
      IConvertible convertible1 = (IConvertible) lhs;
      IConvertible convertible2 = (IConvertible) rhs;
      switch (RuntimeServices.GetConvertTypeCode(lhsTypeCode, rhsTypeCode))
      {
        case TypeCode.UInt32:
          return (object) (uint) ((int) convertible1.ToUInt32((IFormatProvider) null) & (int) convertible2.ToUInt32((IFormatProvider) null));
        case TypeCode.Int64:
          return (object) (convertible1.ToInt64((IFormatProvider) null) & convertible2.ToInt64((IFormatProvider) null));
        case TypeCode.UInt64:
          return (object) (ulong) ((long) convertible1.ToUInt64((IFormatProvider) null) & (long) convertible2.ToUInt64((IFormatProvider) null));
        case TypeCode.Single:
        case TypeCode.Double:
        case TypeCode.Decimal:
          throw new ArgumentException(lhsTypeCode.ToString() + " & " + (object) rhsTypeCode);
        default:
          return (object) (convertible1.ToInt32((IFormatProvider) null) & convertible2.ToInt32((IFormatProvider) null));
      }
    }

    private static object operator |(
      object lhs,
      TypeCode lhsTypeCode,
      object rhs,
      TypeCode rhsTypeCode)
    {
      IConvertible convertible1 = (IConvertible) lhs;
      IConvertible convertible2 = (IConvertible) rhs;
      switch (RuntimeServices.GetConvertTypeCode(lhsTypeCode, rhsTypeCode))
      {
        case TypeCode.UInt32:
          return (object) (uint) ((int) convertible1.ToUInt32((IFormatProvider) null) | (int) convertible2.ToUInt32((IFormatProvider) null));
        case TypeCode.Int64:
          return (object) (convertible1.ToInt64((IFormatProvider) null) | convertible2.ToInt64((IFormatProvider) null));
        case TypeCode.UInt64:
          return (object) (ulong) ((long) convertible1.ToUInt64((IFormatProvider) null) | (long) convertible2.ToUInt64((IFormatProvider) null));
        case TypeCode.Single:
        case TypeCode.Double:
        case TypeCode.Decimal:
          throw new ArgumentException(lhsTypeCode.ToString() + " | " + (object) rhsTypeCode);
        default:
          return (object) (convertible1.ToInt32((IFormatProvider) null) | convertible2.ToInt32((IFormatProvider) null));
      }
    }

    private static object operator ^(
      object lhs,
      TypeCode lhsTypeCode,
      object rhs,
      TypeCode rhsTypeCode)
    {
      IConvertible convertible1 = (IConvertible) lhs;
      IConvertible convertible2 = (IConvertible) rhs;
      switch (RuntimeServices.GetConvertTypeCode(lhsTypeCode, rhsTypeCode))
      {
        case TypeCode.UInt32:
          return (object) (uint) ((int) convertible1.ToUInt32((IFormatProvider) null) ^ (int) convertible2.ToUInt32((IFormatProvider) null));
        case TypeCode.Int64:
          return (object) (convertible1.ToInt64((IFormatProvider) null) ^ convertible2.ToInt64((IFormatProvider) null));
        case TypeCode.UInt64:
          return (object) (ulong) ((long) convertible1.ToUInt64((IFormatProvider) null) ^ (long) convertible2.ToUInt64((IFormatProvider) null));
        case TypeCode.Single:
        case TypeCode.Double:
        case TypeCode.Decimal:
          throw new ArgumentException(lhsTypeCode.ToString() + " ^ " + (object) rhsTypeCode);
        default:
          return (object) (convertible1.ToInt32((IFormatProvider) null) ^ convertible2.ToInt32((IFormatProvider) null));
      }
    }

    private static object op_ShiftLeft(
      object lhs,
      TypeCode lhsTypeCode,
      object rhs,
      TypeCode rhsTypeCode)
    {
      IConvertible convertible1 = (IConvertible) lhs;
      IConvertible convertible2 = (IConvertible) rhs;
      switch (rhsTypeCode)
      {
        case TypeCode.Single:
        case TypeCode.Double:
        case TypeCode.Decimal:
          throw new ArgumentException(lhsTypeCode.ToString() + " << " + (object) rhsTypeCode);
        default:
          switch (lhsTypeCode)
          {
            case TypeCode.UInt32:
              return (object) (uint) ((int) convertible1.ToUInt32((IFormatProvider) null) << convertible2.ToInt32((IFormatProvider) null));
            case TypeCode.Int64:
              return (object) (convertible1.ToInt64((IFormatProvider) null) << convertible2.ToInt32((IFormatProvider) null));
            case TypeCode.UInt64:
              return (object) (ulong) ((long) convertible1.ToUInt64((IFormatProvider) null) << convertible2.ToInt32((IFormatProvider) null));
            case TypeCode.Single:
            case TypeCode.Double:
            case TypeCode.Decimal:
              throw new ArgumentException(lhsTypeCode.ToString() + " << " + (object) rhsTypeCode);
            default:
              return (object) (convertible1.ToInt32((IFormatProvider) null) << convertible2.ToInt32((IFormatProvider) null));
          }
      }
    }

    private static object op_ShiftRight(
      object lhs,
      TypeCode lhsTypeCode,
      object rhs,
      TypeCode rhsTypeCode)
    {
      IConvertible convertible1 = (IConvertible) lhs;
      IConvertible convertible2 = (IConvertible) rhs;
      switch (rhsTypeCode)
      {
        case TypeCode.Single:
        case TypeCode.Double:
        case TypeCode.Decimal:
          throw new ArgumentException(lhsTypeCode.ToString() + " >> " + (object) rhsTypeCode);
        default:
          switch (lhsTypeCode)
          {
            case TypeCode.UInt32:
              return (object) (convertible1.ToUInt32((IFormatProvider) null) >> convertible2.ToInt32((IFormatProvider) null));
            case TypeCode.Int64:
              return (object) (convertible1.ToInt64((IFormatProvider) null) >> convertible2.ToInt32((IFormatProvider) null));
            case TypeCode.UInt64:
              return (object) (convertible1.ToUInt64((IFormatProvider) null) >> convertible2.ToInt32((IFormatProvider) null));
            case TypeCode.Single:
            case TypeCode.Double:
            case TypeCode.Decimal:
              throw new ArgumentException(lhsTypeCode.ToString() + " >> " + (object) rhsTypeCode);
            default:
              return (object) (convertible1.ToInt32((IFormatProvider) null) >> convertible2.ToInt32((IFormatProvider) null));
          }
      }
    }

    private static object operator -(object operand, TypeCode operandTypeCode)
    {
      IConvertible convertible = (IConvertible) operand;
      switch (operandTypeCode)
      {
        case TypeCode.UInt32:
          return (object) -convertible.ToInt64((IFormatProvider) null);
        case TypeCode.Int64:
          return (object) -convertible.ToInt64((IFormatProvider) null);
        case TypeCode.UInt64:
          return (object) -convertible.ToInt64((IFormatProvider) null);
        case TypeCode.Single:
          return (object) (float) -(double) convertible.ToSingle((IFormatProvider) null);
        case TypeCode.Double:
          return (object) -convertible.ToDouble((IFormatProvider) null);
        case TypeCode.Decimal:
          return (object) -convertible.ToDecimal((IFormatProvider) null);
        default:
          return (object) -convertible.ToInt32((IFormatProvider) null);
      }
    }

    internal static bool IsPromotableNumeric(TypeCode code)
    {
      switch (code)
      {
        case TypeCode.Boolean:
          return true;
        case TypeCode.Char:
          return true;
        case TypeCode.SByte:
          return true;
        case TypeCode.Byte:
          return true;
        case TypeCode.Int16:
          return true;
        case TypeCode.UInt16:
          return true;
        case TypeCode.Int32:
          return true;
        case TypeCode.UInt32:
          return true;
        case TypeCode.Int64:
          return true;
        case TypeCode.UInt64:
          return true;
        case TypeCode.Single:
          return true;
        case TypeCode.Double:
          return true;
        case TypeCode.Decimal:
          return true;
        default:
          return false;
      }
    }

    public static IConvertible CheckNumericPromotion(object value) => RuntimeServices.CheckNumericPromotion((IConvertible) value);

    public static IConvertible CheckNumericPromotion(IConvertible convertible) => RuntimeServices.IsPromotableNumeric(convertible.GetTypeCode()) ? convertible : throw new InvalidCastException();

    public static byte UnboxByte(object value) => value is byte num ? num : RuntimeServices.CheckNumericPromotion(value).ToByte((IFormatProvider) null);

    public static sbyte UnboxSByte(object value) => value is sbyte num ? num : RuntimeServices.CheckNumericPromotion(value).ToSByte((IFormatProvider) null);

    public static char UnboxChar(object value) => value is char ch ? ch : RuntimeServices.CheckNumericPromotion(value).ToChar((IFormatProvider) null);

    public static short UnboxInt16(object value) => value is short num ? num : RuntimeServices.CheckNumericPromotion(value).ToInt16((IFormatProvider) null);

    public static ushort UnboxUInt16(object value) => value is ushort num ? num : RuntimeServices.CheckNumericPromotion(value).ToUInt16((IFormatProvider) null);

    public static int UnboxInt32(object value) => value is int num ? num : RuntimeServices.CheckNumericPromotion(value).ToInt32((IFormatProvider) null);

    public static uint UnboxUInt32(object value) => value is uint num ? num : RuntimeServices.CheckNumericPromotion(value).ToUInt32((IFormatProvider) null);

    public static long UnboxInt64(object value) => value is long num ? num : RuntimeServices.CheckNumericPromotion(value).ToInt64((IFormatProvider) null);

    public static ulong UnboxUInt64(object value) => value is ulong num ? num : RuntimeServices.CheckNumericPromotion(value).ToUInt64((IFormatProvider) null);

    public static float UnboxSingle(object value) => value is float num ? num : RuntimeServices.CheckNumericPromotion(value).ToSingle((IFormatProvider) null);

    public static double UnboxDouble(object value) => value is double num ? num : RuntimeServices.CheckNumericPromotion(value).ToDouble((IFormatProvider) null);

    public static Decimal UnboxDecimal(object value) => value is Decimal num ? num : RuntimeServices.CheckNumericPromotion(value).ToDecimal((IFormatProvider) null);

    public static bool UnboxBoolean(object value) => value is bool flag ? flag : RuntimeServices.CheckNumericPromotion(value).ToBoolean((IFormatProvider) null);

    public static bool ToBool(object value)
    {
      switch (value)
      {
        case null:
          return false;
        case bool flag:
          return flag;
        case string _:
          return !string.IsNullOrEmpty((string) value);
        default:
          Type type = value.GetType();
          return (bool) RuntimeServices.GetDispatcher(value, "$ToBool$", new Type[1]
          {
            type
          }, (DispatcherCache.DispatcherFactory) (() => RuntimeServices.CreateBoolConverter(type)))(value, new object[1]
          {
            value
          });
      }
    }

    public static bool ToBool(Decimal value) => 0M != value;

    public static bool ToBool(float value) => 0.0 != (double) value;

    public static bool ToBool(double value) => 0.0 != value;

    private static object ToBoolTrue(object value, object[] arguments) => RuntimeServices.True;

    private static object UnboxBooleanDispatcher(object value, object[] arguments) => (object) RuntimeServices.UnboxBoolean(value);

    private static Dispatcher CreateBoolConverter(Type type)
    {
      MethodInfo conversionOperator = RuntimeServices.FindImplicitConversionOperator(type, typeof (bool));
      if (conversionOperator != null)
        return RuntimeServices.EmitImplicitConversionDispatcher(conversionOperator);
      return type.IsValueType ? new Dispatcher(RuntimeServices.UnboxBooleanDispatcher) : new Dispatcher(RuntimeServices.ToBoolTrue);
    }

    internal static MethodInfo FindImplicitConversionOperator(Type from, Type to) => RuntimeServices.FindImplicitConversionMethod((IEnumerable<MethodInfo>) from.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy), from, to) ?? RuntimeServices.FindImplicitConversionMethod((IEnumerable<MethodInfo>) to.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy), from, to) ?? RuntimeServices.FindImplicitConversionMethod(RuntimeServices.GetExtensionMethods(), from, to);

    [DebuggerHidden]
    private static IEnumerable<MethodInfo> GetExtensionMethods()
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      RuntimeServices.\u003CGetExtensionMethods\u003Ec__IteratorC methodsCIteratorC = new RuntimeServices.\u003CGetExtensionMethods\u003Ec__IteratorC();
      // ISSUE: variable of a compiler-generated type
      RuntimeServices.\u003CGetExtensionMethods\u003Ec__IteratorC extensionMethods = methodsCIteratorC;
      // ISSUE: reference to a compiler-generated field
      extensionMethods.\u0024PC = -2;
      return (IEnumerable<MethodInfo>) extensionMethods;
    }

    private static MethodInfo FindImplicitConversionMethod(
      IEnumerable<MethodInfo> candidates,
      Type from,
      Type to)
    {
      foreach (MethodInfo candidate in candidates)
      {
        if (!(candidate.Name != "op_Implicit") && candidate.ReturnType == to)
        {
          ParameterInfo[] parameters = candidate.GetParameters();
          if (parameters.Length == 1 && parameters[0].ParameterType.IsAssignableFrom(from))
            return candidate;
        }
      }
      return (MethodInfo) null;
    }

    private static void Error(string format, params object[] args) => throw new ApplicationException(string.Format(format, args));

    public static string RuntimeDisplayName
    {
      get
      {
        Type type = Type.GetType("Mono.Runtime");
        return type != null ? (string) type.GetMethod("GetDisplayName", BindingFlags.Static | BindingFlags.NonPublic).Invoke((object) null, (object[]) null) : "CLR " + Environment.Version.ToString();
      }
    }

    public struct ValueTypeChange
    {
      public object Target;
      public string Member;
      public object Value;

      public ValueTypeChange(object target, string member, object value)
      {
        this.Target = target;
        this.Member = member;
        this.Value = value;
      }
    }

    public delegate void CodeBlock();
  }
}
