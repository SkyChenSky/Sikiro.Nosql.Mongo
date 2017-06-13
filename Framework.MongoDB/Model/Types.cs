using System;
using System.Collections;
using System.Collections.Generic;

namespace Framework.MongoDB.Model
{
    internal static class Types
    {
        /// <summary>
        /// Object 类型
        /// </summary>
        public static readonly Type Object = typeof(Object);

        /// <summary>
        /// Type 类型
        /// </summary>
        public static readonly Type Type = typeof(Type);

        /// <summary>
        /// Type 类型
        /// </summary>
        public static readonly Type IList = typeof(IList);

        /// <summary>
        /// Type 类型
        /// </summary>
        public static readonly Type Long = typeof(long);

        /// <summary>
        /// Type 类型
        /// </summary>
        public static readonly Type Float = typeof(float);

        /// <summary>
        /// Stirng 类型
        /// </summary>
        public static readonly Type String = typeof(String);

        /// <summary>
        /// Char 类型
        /// </summary>
        public static readonly Type Char = typeof(Char);

        /// <summary>
        /// Boolean 类型
        /// </summary>
        public static readonly Type Boolean = typeof(Boolean);

        /// <summary>
        /// Byte 类型
        /// </summary>
        public static readonly Type Byte = typeof(Byte);


        /// <summary>
        /// Byte 数组类型
        /// </summary>
        public static readonly Type ByteArray = typeof(Byte[]);

        /// <summary>
        /// SByte 类型
        /// </summary>
        public static readonly Type SByte = typeof(SByte);

        /// <summary>
        /// UInt16 类型
        /// </summary>
        public static readonly Type UInt16 = typeof(UInt16);

        /// <summary>
        /// Int32 类型
        /// </summary>
        public static readonly Type Int = typeof(int);

        /// <summary>
        /// UInt32 类型
        /// </summary>
        public static readonly Type UInt32 = typeof(UInt32);

        /// <summary>
        /// UInt64 类型
        /// </summary>
        public static readonly Type UInt64 = typeof(UInt64);

        /// <summary>
        /// Double 类型
        /// </summary>
        public static readonly Type Double = typeof(Double);

        /// <summary>
        /// Single 类型
        /// </summary>
        public static readonly Type Single = typeof(Single);

        /// <summary>
        /// Decimal 类型
        /// </summary>
        public static readonly Type Decimal = typeof(Decimal);

        /// <summary>
        /// Guid 类型
        /// </summary>
        public static readonly Type Guid = typeof(Guid);

        /// <summary>
        /// DateTime 类型
        /// </summary>
        public static readonly Type DateTime = typeof(DateTime);

        /// <summary>
        /// TimeSpan 类型
        /// </summary>
        public static readonly Type TimeSpan = typeof(TimeSpan);

        /// <summary>
        /// Nullable 类型
        /// </summary>
        public static readonly Type Nullable = typeof(Nullable<>);

        /// <summary>
        /// ValueType 类型
        /// </summary>
        public static readonly Type ValueType = typeof(ValueType);

        /// <summary>
        /// void 类型
        /// </summary>
        public static readonly Type Void = typeof(void);

        /// <summary>
        /// DBNull 类型
        /// </summary>
        public static readonly Type DBNull = typeof(DBNull);

        /// <summary>
        /// Delegate 类型
        /// </summary>
        public static readonly Type Delegate = typeof(Delegate);

        /// <summary>
        /// ByteEnumerable 类型
        /// </summary>
        public static readonly Type ByteEnumerable = typeof(IEnumerable<Byte>);

        /// <summary>
        /// IEnumerable 类型
        /// </summary>
        public static readonly Type IEnumerableOfT = typeof(IEnumerable<>);

        /// <summary>
        /// IEnumerable 类型
        /// </summary>
        public static readonly Type IEnumerable = typeof(IEnumerable);

        /// <summary>
        /// IListSource 类型
        /// </summary>
        public static readonly Type IListSource = typeof(System.ComponentModel.IListSource);

        /// <summary>
        /// IDictionary 类型
        /// </summary>
        public static readonly Type IDictionary = typeof(IDictionary);

        /// <summary>
        /// IDictionary 类型
        /// </summary>
        public static readonly Type IDictionaryOfT = typeof(IDictionary<,>);

        /// <summary>
        /// Dictionary 类型
        /// </summary>
        public static readonly Type DictionaryOfT = typeof(Dictionary<,>);

        /// <summary>
        /// IDataReader 类型
        /// </summary>
        public static readonly Type IDataReader = typeof(System.Data.IDataReader);

        /// <summary>
        /// DataTable 类型
        /// </summary>
        public static readonly Type DataTable = typeof(System.Data.DataTable);

        /// <summary>
        /// DataRow 类型
        /// </summary>
        public static readonly Type DataRow = typeof(System.Data.DataRow);

        /// <summary>
        /// IDictionary 类型
        /// </summary>
        public static readonly Type IDictionaryOfStringAndObject = typeof(IDictionary<string, object>);

        /// <summary>
        /// IDictionary 类型
        /// </summary>
        public static readonly Type IDictionaryOfStringAndString = typeof(IDictionary<string, string>);
    }
}
