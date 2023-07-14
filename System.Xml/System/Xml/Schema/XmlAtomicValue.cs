// Decompiled with JetBrains decompiler
// Type: System.Xml.Schema.XmlAtomicValue
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections;
using System.Xml.XPath;

namespace System.Xml.Schema
{
  [MonoTODO]
  public sealed class XmlAtomicValue : XPathItem, ICloneable
  {
    private bool booleanValue;
    private DateTime dateTimeValue;
    private Decimal decimalValue;
    private double doubleValue;
    private int intValue;
    private long longValue;
    private object objectValue;
    private float floatValue;
    private string stringValue;
    private XmlSchemaType schemaType;
    private XmlTypeCode xmlTypeCode;

    internal XmlAtomicValue(bool value, XmlSchemaType xmlType) => this.Init(value, xmlType);

    internal XmlAtomicValue(DateTime value, XmlSchemaType xmlType) => this.Init(value, xmlType);

    internal XmlAtomicValue(Decimal value, XmlSchemaType xmlType) => this.Init(value, xmlType);

    internal XmlAtomicValue(double value, XmlSchemaType xmlType) => this.Init(value, xmlType);

    internal XmlAtomicValue(int value, XmlSchemaType xmlType) => this.Init(value, xmlType);

    internal XmlAtomicValue(long value, XmlSchemaType xmlType) => this.Init(value, xmlType);

    internal XmlAtomicValue(float value, XmlSchemaType xmlType) => this.Init(value, xmlType);

    internal XmlAtomicValue(string value, XmlSchemaType xmlType) => this.Init(value, xmlType);

    internal XmlAtomicValue(object value, XmlSchemaType xmlType) => this.Init(value, xmlType);

    object ICloneable.Clone() => (object) this.Clone();

    private void Init(bool value, XmlSchemaType xmlType)
    {
      if (xmlType == null)
        throw new ArgumentNullException(nameof (xmlType));
      this.xmlTypeCode = XmlTypeCode.Boolean;
      this.booleanValue = value;
      this.schemaType = xmlType;
    }

    private void Init(DateTime value, XmlSchemaType xmlType)
    {
      if (xmlType == null)
        throw new ArgumentNullException(nameof (xmlType));
      this.xmlTypeCode = XmlTypeCode.DateTime;
      this.dateTimeValue = value;
      this.schemaType = xmlType;
    }

    private void Init(Decimal value, XmlSchemaType xmlType)
    {
      if (xmlType == null)
        throw new ArgumentNullException(nameof (xmlType));
      this.xmlTypeCode = XmlTypeCode.Decimal;
      this.decimalValue = value;
      this.schemaType = xmlType;
    }

    private void Init(double value, XmlSchemaType xmlType)
    {
      if (xmlType == null)
        throw new ArgumentNullException(nameof (xmlType));
      this.xmlTypeCode = XmlTypeCode.Double;
      this.doubleValue = value;
      this.schemaType = xmlType;
    }

    private void Init(int value, XmlSchemaType xmlType)
    {
      if (xmlType == null)
        throw new ArgumentNullException(nameof (xmlType));
      this.xmlTypeCode = XmlTypeCode.Int;
      this.intValue = value;
      this.schemaType = xmlType;
    }

    private void Init(long value, XmlSchemaType xmlType)
    {
      if (xmlType == null)
        throw new ArgumentNullException(nameof (xmlType));
      this.xmlTypeCode = XmlTypeCode.Long;
      this.longValue = value;
      this.schemaType = xmlType;
    }

    private void Init(float value, XmlSchemaType xmlType)
    {
      if (xmlType == null)
        throw new ArgumentNullException(nameof (xmlType));
      this.xmlTypeCode = XmlTypeCode.Float;
      this.floatValue = value;
      this.schemaType = xmlType;
    }

    private void Init(string value, XmlSchemaType xmlType)
    {
      if (value == null)
        throw new ArgumentNullException(nameof (value));
      if (xmlType == null)
        throw new ArgumentNullException(nameof (xmlType));
      this.xmlTypeCode = XmlTypeCode.String;
      this.stringValue = value;
      this.schemaType = xmlType;
    }

    private void Init(object value, XmlSchemaType xmlType)
    {
      if (value == null)
        throw new ArgumentNullException(nameof (value));
      if (xmlType == null)
        throw new ArgumentNullException(nameof (xmlType));
      switch (Type.GetTypeCode(value.GetType()))
      {
        case TypeCode.Boolean:
          this.Init((bool) value, xmlType);
          break;
        case TypeCode.Int16:
        case TypeCode.UInt16:
        case TypeCode.Int32:
          this.Init((int) value, xmlType);
          break;
        case TypeCode.UInt32:
        case TypeCode.Int64:
          this.Init((long) value, xmlType);
          break;
        case TypeCode.Single:
          this.Init((float) value, xmlType);
          break;
        case TypeCode.Double:
          this.Init((double) value, xmlType);
          break;
        case TypeCode.Decimal:
          this.Init((Decimal) value, xmlType);
          break;
        case TypeCode.DateTime:
          this.Init((DateTime) value, xmlType);
          break;
        case TypeCode.String:
          this.Init((string) value, xmlType);
          break;
        default:
          switch (value)
          {
            case ICollection collection when collection.Count == 1:
              if (collection is IList)
              {
                this.Init(((IList) collection)[0], xmlType);
                return;
              }
              IEnumerator enumerator = collection.GetEnumerator();
              if (!enumerator.MoveNext())
                return;
              if (enumerator.Current is DictionaryEntry)
              {
                this.Init(((DictionaryEntry) enumerator.Current).Value, xmlType);
                return;
              }
              this.Init(enumerator.Current, xmlType);
              return;
            case XmlAtomicValue xmlAtomicValue:
              XmlTypeCode xmlTypeCode = xmlAtomicValue.xmlTypeCode;
              switch (xmlTypeCode)
              {
                case XmlTypeCode.String:
                  this.Init(xmlAtomicValue.stringValue, xmlType);
                  return;
                case XmlTypeCode.Boolean:
                  this.Init(xmlAtomicValue.booleanValue, xmlType);
                  return;
                case XmlTypeCode.Decimal:
                  this.Init(xmlAtomicValue.decimalValue, xmlType);
                  return;
                case XmlTypeCode.Float:
                  this.Init(xmlAtomicValue.floatValue, xmlType);
                  return;
                case XmlTypeCode.Double:
                  this.Init(xmlAtomicValue.doubleValue, xmlType);
                  return;
                case XmlTypeCode.DateTime:
                  this.Init(xmlAtomicValue.dateTimeValue, xmlType);
                  return;
                default:
                  if (xmlTypeCode != XmlTypeCode.Long)
                  {
                    if (xmlTypeCode == XmlTypeCode.Int)
                    {
                      this.Init(xmlAtomicValue.intValue, xmlType);
                      return;
                    }
                    this.objectValue = xmlAtomicValue.objectValue;
                    break;
                  }
                  this.Init(xmlAtomicValue.longValue, xmlType);
                  return;
              }
              break;
          }
          this.objectValue = value;
          this.schemaType = xmlType;
          break;
      }
    }

    public XmlAtomicValue Clone() => new XmlAtomicValue((object) this, this.schemaType);

    public override object ValueAs(Type type, IXmlNamespaceResolver nsResolver)
    {
      XmlTypeCode xmlTypeCode = XmlAtomicValue.XmlTypeCodeFromRuntimeType(type, false);
      switch (xmlTypeCode)
      {
        case XmlTypeCode.Long:
        case XmlTypeCode.UnsignedInt:
          return (object) this.ValueAsLong;
        case XmlTypeCode.Int:
        case XmlTypeCode.Short:
        case XmlTypeCode.UnsignedShort:
          return (object) this.ValueAsInt;
        default:
          switch (xmlTypeCode - 12)
          {
            case XmlTypeCode.None:
              return (object) this.Value;
            case XmlTypeCode.Item:
              return (object) this.ValueAsBoolean;
            case XmlTypeCode.Document:
            case XmlTypeCode.Element:
              return (object) this.ValueAsDouble;
            case XmlTypeCode.Namespace:
              return (object) this.ValueAsDateTime;
            default:
              if (xmlTypeCode == XmlTypeCode.Item)
                return this.TypedValue;
              if (xmlTypeCode == XmlTypeCode.QName)
                return (object) XmlQualifiedName.Parse(this.Value, nsResolver, true);
              throw new NotImplementedException();
          }
      }
    }

    public override string ToString() => this.Value;

    public override bool IsNode => false;

    internal XmlTypeCode ResolvedTypeCode => this.schemaType != XmlSchemaComplexType.AnyType ? this.schemaType.TypeCode : this.xmlTypeCode;

    public override object TypedValue
    {
      get
      {
        XmlTypeCode resolvedTypeCode = this.ResolvedTypeCode;
        switch (resolvedTypeCode)
        {
          case XmlTypeCode.String:
            return (object) this.Value;
          case XmlTypeCode.Boolean:
            return (object) this.ValueAsBoolean;
          case XmlTypeCode.Float:
          case XmlTypeCode.Double:
            return (object) this.ValueAsDouble;
          case XmlTypeCode.DateTime:
            return (object) this.ValueAsDateTime;
          default:
            if (resolvedTypeCode == XmlTypeCode.Long)
              return (object) this.ValueAsLong;
            return resolvedTypeCode == XmlTypeCode.Int ? (object) this.ValueAsInt : this.objectValue;
        }
      }
    }

    public override string Value
    {
      get
      {
        XmlTypeCode resolvedTypeCode = this.ResolvedTypeCode;
        switch (resolvedTypeCode)
        {
          case XmlTypeCode.AnyAtomicType:
            XmlTypeCode xmlTypeCode = XmlAtomicValue.XmlTypeCodeFromRuntimeType(this.objectValue.GetType(), false);
            switch (xmlTypeCode)
            {
              case XmlTypeCode.String:
                this.stringValue = (string) this.objectValue;
                break;
              case XmlTypeCode.Boolean:
                this.stringValue = XQueryConvert.BooleanToString((bool) this.objectValue);
                break;
              case XmlTypeCode.Decimal:
                this.stringValue = XQueryConvert.DecimalToString((Decimal) this.objectValue);
                break;
              case XmlTypeCode.Float:
                this.stringValue = XQueryConvert.FloatToString((float) this.objectValue);
                break;
              case XmlTypeCode.Double:
                this.stringValue = XQueryConvert.DoubleToString((double) this.objectValue);
                break;
              case XmlTypeCode.DateTime:
                this.stringValue = XQueryConvert.DateTimeToString((DateTime) this.objectValue);
                break;
              default:
                if (xmlTypeCode != XmlTypeCode.Long)
                {
                  if (xmlTypeCode == XmlTypeCode.Int)
                  {
                    this.stringValue = XQueryConvert.IntToString((int) this.objectValue);
                    break;
                  }
                  break;
                }
                this.stringValue = XQueryConvert.IntegerToString((long) this.objectValue);
                break;
            }
            break;
          case XmlTypeCode.String:
            return this.stringValue;
          case XmlTypeCode.Boolean:
            this.stringValue = XQueryConvert.BooleanToString(this.ValueAsBoolean);
            break;
          case XmlTypeCode.Float:
          case XmlTypeCode.Double:
            this.stringValue = XQueryConvert.DoubleToString(this.ValueAsDouble);
            break;
          case XmlTypeCode.DateTime:
            this.stringValue = XQueryConvert.DateTimeToString(this.ValueAsDateTime);
            break;
          case XmlTypeCode.NonPositiveInteger:
          case XmlTypeCode.NegativeInteger:
          case XmlTypeCode.Long:
          case XmlTypeCode.NonNegativeInteger:
          case XmlTypeCode.UnsignedLong:
          case XmlTypeCode.PositiveInteger:
            this.stringValue = XQueryConvert.IntegerToString(this.ValueAsLong);
            break;
          case XmlTypeCode.Int:
          case XmlTypeCode.Short:
          case XmlTypeCode.Byte:
          case XmlTypeCode.UnsignedInt:
          case XmlTypeCode.UnsignedShort:
          case XmlTypeCode.UnsignedByte:
            this.stringValue = XQueryConvert.IntToString(this.ValueAsInt);
            break;
          default:
            if (resolvedTypeCode == XmlTypeCode.None || resolvedTypeCode == XmlTypeCode.Item)
              goto case XmlTypeCode.AnyAtomicType;
            else
              break;
        }
        if (this.stringValue != null)
          return this.stringValue;
        if (this.objectValue != null)
          throw new InvalidCastException(string.Format("Conversion from runtime type {0} to {1} is not supported", (object) this.objectValue.GetType(), (object) XmlTypeCode.String));
        throw new InvalidCastException(string.Format("Conversion from schema type {0} (type code {1}, resolved type code {2}) to {3} is not supported.", (object) this.schemaType.QualifiedName, (object) this.xmlTypeCode, (object) this.ResolvedTypeCode, (object) XmlTypeCode.String));
      }
    }

    public override bool ValueAsBoolean
    {
      get
      {
        XmlTypeCode xmlTypeCode = this.xmlTypeCode;
        switch (xmlTypeCode)
        {
          case XmlTypeCode.AnyAtomicType:
            if (this.objectValue is bool)
              return (bool) this.objectValue;
            break;
          case XmlTypeCode.String:
            return XQueryConvert.StringToBoolean(this.stringValue);
          case XmlTypeCode.Boolean:
            return this.booleanValue;
          case XmlTypeCode.Decimal:
            return XQueryConvert.DecimalToBoolean(this.decimalValue);
          case XmlTypeCode.Float:
            return XQueryConvert.FloatToBoolean(this.floatValue);
          case XmlTypeCode.Double:
            return XQueryConvert.DoubleToBoolean(this.doubleValue);
          default:
            if (xmlTypeCode != XmlTypeCode.None && xmlTypeCode != XmlTypeCode.Item)
            {
              if (xmlTypeCode == XmlTypeCode.Long)
                return XQueryConvert.IntegerToBoolean(this.longValue);
              if (xmlTypeCode == XmlTypeCode.Int)
                return XQueryConvert.IntToBoolean(this.intValue);
              break;
            }
            goto case XmlTypeCode.AnyAtomicType;
        }
        throw new InvalidCastException(string.Format("Conversion from {0} to {1} is not supported", (object) this.schemaType.QualifiedName, (object) XmlSchemaSimpleType.XsBoolean.QualifiedName));
      }
    }

    public override DateTime ValueAsDateTime
    {
      get
      {
        XmlTypeCode xmlTypeCode = this.xmlTypeCode;
        switch (xmlTypeCode)
        {
          case XmlTypeCode.AnyAtomicType:
            if (this.objectValue is DateTime)
              return (DateTime) this.objectValue;
            break;
          case XmlTypeCode.String:
            return XQueryConvert.StringToDateTime(this.stringValue);
          default:
            if (xmlTypeCode != XmlTypeCode.None && xmlTypeCode != XmlTypeCode.Item)
            {
              if (xmlTypeCode == XmlTypeCode.DateTime)
                return this.dateTimeValue;
              break;
            }
            goto case XmlTypeCode.AnyAtomicType;
        }
        throw new InvalidCastException(string.Format("Conversion from {0} to {1} is not supported", (object) this.schemaType.QualifiedName, (object) XmlSchemaSimpleType.XsDateTime.QualifiedName));
      }
    }

    public override double ValueAsDouble
    {
      get
      {
        XmlTypeCode xmlTypeCode = this.xmlTypeCode;
        switch (xmlTypeCode)
        {
          case XmlTypeCode.AnyAtomicType:
            if (this.objectValue is double)
              return (double) this.objectValue;
            break;
          case XmlTypeCode.String:
            return XQueryConvert.StringToDouble(this.stringValue);
          case XmlTypeCode.Boolean:
            return XQueryConvert.BooleanToDouble(this.booleanValue);
          case XmlTypeCode.Decimal:
            return XQueryConvert.DecimalToDouble(this.decimalValue);
          case XmlTypeCode.Float:
            return XQueryConvert.FloatToDouble(this.floatValue);
          case XmlTypeCode.Double:
            return this.doubleValue;
          default:
            if (xmlTypeCode != XmlTypeCode.None && xmlTypeCode != XmlTypeCode.Item)
            {
              if (xmlTypeCode == XmlTypeCode.Long)
                return XQueryConvert.IntegerToDouble(this.longValue);
              if (xmlTypeCode == XmlTypeCode.Int)
                return XQueryConvert.IntToDouble(this.intValue);
              break;
            }
            goto case XmlTypeCode.AnyAtomicType;
        }
        throw new InvalidCastException(string.Format("Conversion from {0} to {1} is not supported", (object) this.schemaType.QualifiedName, (object) XmlSchemaSimpleType.XsDouble.QualifiedName));
      }
    }

    public override int ValueAsInt
    {
      get
      {
        XmlTypeCode xmlTypeCode = this.xmlTypeCode;
        switch (xmlTypeCode)
        {
          case XmlTypeCode.AnyAtomicType:
            if (this.objectValue is int)
              return (int) this.objectValue;
            break;
          case XmlTypeCode.String:
            return XQueryConvert.StringToInt(this.stringValue);
          case XmlTypeCode.Boolean:
            return XQueryConvert.BooleanToInt(this.booleanValue);
          case XmlTypeCode.Decimal:
            return XQueryConvert.DecimalToInt(this.decimalValue);
          case XmlTypeCode.Float:
            return XQueryConvert.FloatToInt(this.floatValue);
          case XmlTypeCode.Double:
            return XQueryConvert.DoubleToInt(this.doubleValue);
          default:
            if (xmlTypeCode != XmlTypeCode.None && xmlTypeCode != XmlTypeCode.Item)
            {
              if (xmlTypeCode == XmlTypeCode.Long)
                return XQueryConvert.IntegerToInt(this.longValue);
              if (xmlTypeCode == XmlTypeCode.Int)
                return this.intValue;
              break;
            }
            goto case XmlTypeCode.AnyAtomicType;
        }
        throw new InvalidCastException(string.Format("Conversion from {0} to {1} is not supported", (object) this.schemaType.QualifiedName, (object) XmlSchemaSimpleType.XsInt.QualifiedName));
      }
    }

    public override long ValueAsLong
    {
      get
      {
        XmlTypeCode xmlTypeCode = this.xmlTypeCode;
        switch (xmlTypeCode)
        {
          case XmlTypeCode.AnyAtomicType:
            if (this.objectValue is long)
              return (long) this.objectValue;
            break;
          case XmlTypeCode.String:
            return XQueryConvert.StringToInteger(this.stringValue);
          case XmlTypeCode.Boolean:
            return XQueryConvert.BooleanToInteger(this.booleanValue);
          case XmlTypeCode.Decimal:
            return XQueryConvert.DecimalToInteger(this.decimalValue);
          case XmlTypeCode.Float:
            return XQueryConvert.FloatToInteger(this.floatValue);
          case XmlTypeCode.Double:
            return XQueryConvert.DoubleToInteger(this.doubleValue);
          default:
            if (xmlTypeCode != XmlTypeCode.None && xmlTypeCode != XmlTypeCode.Item)
            {
              if (xmlTypeCode == XmlTypeCode.Long)
                return this.longValue;
              if (xmlTypeCode == XmlTypeCode.Int)
                return (long) XQueryConvert.IntegerToInt((long) this.intValue);
              break;
            }
            goto case XmlTypeCode.AnyAtomicType;
        }
        throw new InvalidCastException(string.Format("Conversion from {0} to {1} is not supported", (object) this.schemaType.QualifiedName, (object) XmlSchemaSimpleType.XsLong.QualifiedName));
      }
    }

    public override Type ValueType => this.schemaType.Datatype.ValueType;

    public override XmlSchemaType XmlType => this.schemaType;

    internal static Type RuntimeTypeFromXmlTypeCode(XmlTypeCode typeCode)
    {
      XmlTypeCode xmlTypeCode = typeCode;
      switch (xmlTypeCode)
      {
        case XmlTypeCode.Long:
          return typeof (long);
        case XmlTypeCode.Int:
          return typeof (int);
        case XmlTypeCode.Short:
          return typeof (short);
        case XmlTypeCode.UnsignedInt:
          return typeof (uint);
        case XmlTypeCode.UnsignedShort:
          return typeof (ushort);
        default:
          switch (xmlTypeCode - 12)
          {
            case XmlTypeCode.None:
              return typeof (string);
            case XmlTypeCode.Item:
              return typeof (bool);
            case XmlTypeCode.Node:
              return typeof (Decimal);
            case XmlTypeCode.Document:
              return typeof (float);
            case XmlTypeCode.Element:
              return typeof (double);
            case XmlTypeCode.Namespace:
              return typeof (DateTime);
            default:
              if (xmlTypeCode == XmlTypeCode.Item)
                return typeof (object);
              throw new NotSupportedException(string.Format("XQuery internal error: Cannot infer Runtime Type from XmlTypeCode {0}.", (object) typeCode));
          }
      }
    }

    internal static XmlTypeCode XmlTypeCodeFromRuntimeType(Type cliType, bool raiseError)
    {
      switch (Type.GetTypeCode(cliType))
      {
        case TypeCode.Object:
          return XmlTypeCode.Item;
        case TypeCode.Boolean:
          return XmlTypeCode.Boolean;
        case TypeCode.Int16:
          return XmlTypeCode.Short;
        case TypeCode.UInt16:
          return XmlTypeCode.UnsignedShort;
        case TypeCode.Int32:
          return XmlTypeCode.Int;
        case TypeCode.UInt32:
          return XmlTypeCode.UnsignedInt;
        case TypeCode.Int64:
          return XmlTypeCode.Long;
        case TypeCode.Single:
          return XmlTypeCode.Float;
        case TypeCode.Double:
          return XmlTypeCode.Double;
        case TypeCode.Decimal:
          return XmlTypeCode.Decimal;
        case TypeCode.DateTime:
          return XmlTypeCode.DateTime;
        case TypeCode.String:
          return XmlTypeCode.String;
        default:
          if (raiseError)
            throw new NotSupportedException(string.Format("XQuery internal error: Cannot infer XmlTypeCode from Runtime Type {0}", (object) cliType));
          return XmlTypeCode.None;
      }
    }
  }
}
