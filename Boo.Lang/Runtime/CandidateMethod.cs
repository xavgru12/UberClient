// Decompiled with JetBrains decompiler
// Type: Boo.Lang.Runtime.CandidateMethod
// Assembly: Boo.Lang, Version=2.0.9.5, Culture=neutral, PublicKeyToken=32c39770e9a21a67
// MVID: D4F47A63-3E02-45E3-BD62-EDD90EF56CC2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Boo.Lang.dll

using System;
using System.Reflection;

namespace Boo.Lang.Runtime
{
  public class CandidateMethod
  {
    public const int ExactMatchScore = 7;
    public const int UpCastScore = 6;
    public const int WideningPromotion = 5;
    public const int ImplicitConversionScore = 4;
    public const int NarrowingPromotion = 3;
    public const int DowncastScore = 2;
    private readonly MethodInfo _method;
    private readonly int[] _argumentScores;
    private readonly bool _varArgs;

    public CandidateMethod(MethodInfo method, int argumentCount, bool varArgs)
    {
      this._method = method;
      this._argumentScores = new int[argumentCount];
      this._varArgs = varArgs;
    }

    public static int CalculateArgumentScore(Type paramType, Type argType)
    {
      if (argType == null)
        return !paramType.IsValueType ? 7 : -1;
      if (paramType == argType)
        return 7;
      if (paramType.IsAssignableFrom(argType))
        return 6;
      if (argType.IsAssignableFrom(paramType))
        return 2;
      return CandidateMethod.IsNumericPromotion(paramType, argType) ? (NumericTypes.IsWideningPromotion(paramType, argType) ? 5 : 3) : (RuntimeServices.FindImplicitConversionOperator(argType, paramType) != null ? 4 : -1);
    }

    public MethodInfo Method => this._method;

    public int[] ArgumentScores => this._argumentScores;

    public bool VarArgs => this._varArgs;

    public int MinimumArgumentCount => this._varArgs ? this.Parameters.Length - 1 : this.Parameters.Length;

    public ParameterInfo[] Parameters => this._method.GetParameters();

    public Type VarArgsParameterType => this.GetParameterType(this.Parameters.Length - 1).GetElementType();

    public bool DoesNotRequireConversions => !this.RequiresConversions;

    private bool RequiresConversions => Array.Exists<int>(this._argumentScores, new Predicate<int>(CandidateMethod.RequiresConversion));

    private static bool RequiresConversion(int score) => score < 5;

    public Type GetParameterType(int i) => this.Parameters[i].ParameterType;

    public static bool IsNumericPromotion(Type paramType, Type argType) => RuntimeServices.IsPromotableNumeric(Type.GetTypeCode(paramType)) && RuntimeServices.IsPromotableNumeric(Type.GetTypeCode(argType));

    public object DynamicInvoke(object target, object[] args) => this._method.Invoke(target, this.AdjustArgumentsForInvocation(args));

    private object[] AdjustArgumentsForInvocation(object[] arguments)
    {
      if (this.VarArgs)
      {
        Type argsParameterType = this.VarArgsParameterType;
        int minimumArgumentCount = this.MinimumArgumentCount;
        object[] objArray = new object[minimumArgumentCount + 1];
        for (int i = 0; i < minimumArgumentCount; ++i)
          objArray[i] = !CandidateMethod.RequiresConversion(this.ArgumentScores[i]) ? arguments[i] : RuntimeServices.Coerce(arguments[i], this.GetParameterType(i));
        objArray[minimumArgumentCount] = (object) CandidateMethod.CreateVarArgsArray(arguments, minimumArgumentCount, argsParameterType);
        return objArray;
      }
      if (this.RequiresConversions)
      {
        for (int i = 0; i < arguments.Length; ++i)
          arguments[i] = !CandidateMethod.RequiresConversion(this.ArgumentScores[i]) ? arguments[i] : RuntimeServices.Coerce(arguments[i], this.GetParameterType(i));
      }
      return arguments;
    }

    private static Array CreateVarArgsArray(
      object[] arguments,
      int minimumArgumentCount,
      Type varArgsParameterType)
    {
      int length = arguments.Length - minimumArgumentCount;
      Array instance = Array.CreateInstance(varArgsParameterType, length);
      for (int index = 0; index < instance.Length; ++index)
        instance.SetValue(RuntimeServices.Coerce(arguments[minimumArgumentCount + index], varArgsParameterType), index);
      return instance;
    }
  }
}
