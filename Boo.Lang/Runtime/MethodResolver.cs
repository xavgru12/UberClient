// Decompiled with JetBrains decompiler
// Type: Boo.Lang.Runtime.MethodResolver
// Assembly: Boo.Lang, Version=2.0.9.5, Culture=neutral, PublicKeyToken=32c39770e9a21a67
// MVID: D4F47A63-3E02-45E3-BD62-EDD90EF56CC2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Boo.Lang.dll

using System;
using System.Collections.Generic;
using System.Reflection;

namespace Boo.Lang.Runtime
{
  public class MethodResolver
  {
    private readonly Type[] _arguments;

    public MethodResolver(params Type[] argumentTypes) => this._arguments = argumentTypes;

    public static Type[] GetArgumentTypes(object[] arguments)
    {
      if (arguments.Length == 0)
        return Type.EmptyTypes;
      Type[] argumentTypes = new Type[arguments.Length];
      for (int index = 0; index < argumentTypes.Length; ++index)
        argumentTypes[index] = MethodResolver.GetObjectTypeOrNull(arguments[index]);
      return argumentTypes;
    }

    private static Type GetObjectTypeOrNull(object arg) => arg?.GetType();

    public CandidateMethod ResolveMethod(IEnumerable<MethodInfo> candidates)
    {
      Boo.Lang.List<CandidateMethod> applicableMethods = this.FindApplicableMethods(candidates);
      if (applicableMethods.Count == 0)
        return (CandidateMethod) null;
      if (applicableMethods.Count == 1)
        return applicableMethods[0];
      Boo.Lang.List<CandidateMethod> all = applicableMethods.FindAll(new Predicate<CandidateMethod>(MethodResolver.DoesNotRequireConversions));
      return all.Count > 0 ? this.BestMethod(all) : this.BestMethod(applicableMethods);
    }

    private static bool DoesNotRequireConversions(CandidateMethod candidate) => candidate.DoesNotRequireConversions;

    private CandidateMethod BestMethod(Boo.Lang.List<CandidateMethod> applicable)
    {
      applicable.Sort(new Comparison<CandidateMethod>(this.BetterCandidate));
      return applicable[applicable.Count - 1];
    }

    private static int TotalScore(CandidateMethod c1)
    {
      int num = 0;
      foreach (int argumentScore in c1.ArgumentScores)
        num += argumentScore;
      return num;
    }

    private int BetterCandidate(CandidateMethod c1, CandidateMethod c2)
    {
      int num1 = Math.Sign(MethodResolver.TotalScore(c1) - MethodResolver.TotalScore(c2));
      if (num1 != 0)
        return num1;
      if (c1.VarArgs && !c2.VarArgs)
        return -1;
      if (c2.VarArgs && !c1.VarArgs)
        return 1;
      int num2 = Math.Min(c1.MinimumArgumentCount, c2.MinimumArgumentCount);
      for (int i = 0; i < num2; ++i)
        num1 += MethodResolver.MoreSpecificType(c1.GetParameterType(i), c2.GetParameterType(i));
      if (num1 != 0)
        return num1;
      return c1.VarArgs && c2.VarArgs ? MethodResolver.MoreSpecificType(c1.VarArgsParameterType, c2.VarArgsParameterType) : 0;
    }

    private static int MoreSpecificType(Type t1, Type t2)
    {
      int num = MethodResolver.GetTypeGenerity(t2) - MethodResolver.GetTypeGenerity(t1);
      return num != 0 ? num : MethodResolver.GetLogicalTypeDepth(t1) - MethodResolver.GetLogicalTypeDepth(t2);
    }

    private static int GetTypeGenerity(Type type) => !type.ContainsGenericParameters ? 0 : type.GetGenericArguments().Length;

    private static int GetLogicalTypeDepth(Type type)
    {
      int typeDepth = MethodResolver.GetTypeDepth(type);
      return type.IsValueType ? typeDepth - 1 : typeDepth;
    }

    private static int GetTypeDepth(Type type)
    {
      if (type.IsByRef)
        return MethodResolver.GetTypeDepth(type.GetElementType());
      return type.IsInterface ? MethodResolver.GetInterfaceDepth(type) : MethodResolver.GetClassDepth(type);
    }

    private static int GetClassDepth(Type type)
    {
      int classDepth = 0;
      Type type1 = typeof (object);
      while (type != null && type != type1)
      {
        type = type.BaseType;
        ++classDepth;
      }
      return classDepth;
    }

    private static int GetInterfaceDepth(Type type)
    {
      Type[] interfaces = type.GetInterfaces();
      if (interfaces.Length <= 0)
        return 1;
      int num = 0;
      foreach (Type type1 in interfaces)
      {
        int interfaceDepth = MethodResolver.GetInterfaceDepth(type1);
        if (interfaceDepth > num)
          num = interfaceDepth;
      }
      return 1 + num;
    }

    private Boo.Lang.List<CandidateMethod> FindApplicableMethods(IEnumerable<MethodInfo> candidates)
    {
      Boo.Lang.List<CandidateMethod> applicableMethods = new Boo.Lang.List<CandidateMethod>();
      foreach (MethodInfo candidate in candidates)
      {
        CandidateMethod candidateMethod = this.IsApplicableMethod(candidate);
        if (candidateMethod != null)
          applicableMethods.Add(candidateMethod);
      }
      return applicableMethods;
    }

    private CandidateMethod IsApplicableMethod(MethodInfo method)
    {
      ParameterInfo[] parameters = method.GetParameters();
      bool flag = this.IsVarArgs(parameters);
      if (!this.ValidArgumentCount(parameters, flag))
        return (CandidateMethod) null;
      CandidateMethod candidateMethod = new CandidateMethod(method, this._arguments.Length, flag);
      return this.CalculateCandidateScore(candidateMethod) ? candidateMethod : (CandidateMethod) null;
    }

    private bool ValidArgumentCount(ParameterInfo[] parameters, bool varargs) => varargs ? this._arguments.Length >= parameters.Length - 1 : this._arguments.Length == parameters.Length;

    private bool IsVarArgs(ParameterInfo[] parameters) => parameters.Length != 0 && this.HasParamArrayAttribute(parameters[parameters.Length - 1]);

    private bool HasParamArrayAttribute(ParameterInfo info) => info.IsDefined(typeof (ParamArrayAttribute), true);

    private bool CalculateCandidateScore(CandidateMethod candidateMethod)
    {
      ParameterInfo[] parameters = candidateMethod.Parameters;
      for (int argumentIndex = 0; argumentIndex < candidateMethod.MinimumArgumentCount; ++argumentIndex)
      {
        if (parameters[argumentIndex].IsOut || !this.CalculateCandidateArgumentScore(candidateMethod, argumentIndex, parameters[argumentIndex].ParameterType))
          return false;
      }
      if (candidateMethod.VarArgs)
      {
        Type argsParameterType = candidateMethod.VarArgsParameterType;
        for (int minimumArgumentCount = candidateMethod.MinimumArgumentCount; minimumArgumentCount < this._arguments.Length; ++minimumArgumentCount)
        {
          if (!this.CalculateCandidateArgumentScore(candidateMethod, minimumArgumentCount, argsParameterType))
            return false;
        }
      }
      return true;
    }

    private bool CalculateCandidateArgumentScore(
      CandidateMethod candidateMethod,
      int argumentIndex,
      Type paramType)
    {
      int argumentScore = CandidateMethod.CalculateArgumentScore(paramType, this._arguments[argumentIndex]);
      if (argumentScore < 0)
        return false;
      candidateMethod.ArgumentScores[argumentIndex] = argumentScore;
      return true;
    }
  }
}
