using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace mxProject.Data
{
    /// <summary>
    /// Defines the targets for a <see cref="IDataReader"/>.
    /// </summary>
    [Flags]
    public enum DataReaderFilterTargets
    {
        /// <summary>
        /// None
        /// </summary>
        None = 0,

        /// <summary>
        /// Dispose
        /// </summary>
        Dispose = 1,

        /// <summary>
        /// Read
        /// </summary>
        Read = 2,

        /// <summary>
        /// NextResult
        /// </summary>
        NextResult = 4,

        /// <summary>
        /// Close
        /// </summary>
        Close = 8,

        /// <summary>
        /// IsDBNull
        /// </summary>
        IsDBNull = 16,

        /// <summary>
        /// GetName
        /// </summary>
        GetName = 32,

        /// <summary>
        /// GetOrdinal
        /// </summary>
        GetOrdinal = 64,

        /// <summary>
        /// GetFieldType
        /// </summary>
        GetFieldType = 128,

        /// <summary>
        /// GetTypeName
        /// </summary>
        GetDataTypeName = 256,

        /// <summary>
        /// GetSchemaTable
        /// </summary>
        GetSchemaTable = 512,

        /// <summary>
        /// GetBoolean
        /// </summary>
        GetBoolean = 1024,

        /// <summary>
        /// GetByte
        /// </summary>
        GetByte = 2048,

        /// <summary>
        /// GetBytes
        /// </summary>
        GetBytes = 4096,

        /// <summary>
        /// GetChar
        /// </summary>
        GetChar = 8192,

        /// <summary>
        /// GetChars
        /// </summary>
        GetChars = 16384,

        /// <summary>
        /// GetData
        /// </summary>
        GetData = 32768,

        /// <summary>
        /// GetDateTime
        /// </summary>
        GetDateTime = 65536,

        /// <summary>
        /// GetDecimal
        /// </summary>
        GetDecimal = 131072,

        /// <summary>
        /// GetDouble
        /// </summary>
        GetDouble = 262144,

        /// <summary>
        /// GetFloat
        /// </summary>
        GetFloat = 524288,

        /// <summary>
        /// GetGuid
        /// </summary>
        GetGuid = 1048576,

        /// <summary>
        /// GetInt16
        /// </summary>
        GetInt16 = 2097152,

        /// <summary>
        /// GetInt32
        /// </summary>
        GetInt32 = 4194304,

        /// <summary>
        /// GetInt64
        /// </summary>
        GetInt64 = 8388608,

        /// <summary>
        /// GetString
        /// </summary>
        GetString = 16777216,

        /// <summary>
        /// GetValue
        /// </summary>
        GetValue = 33554432,

        /// <summary>
        /// GetValues
        /// </summary>
        GetValues = 67108864,

        /// <summary>
        /// All
        /// </summary>
        All = Dispose | Read | NextResult | Close | IsDBNull | GetName | GetOrdinal | GetFieldType | GetDataTypeName | GetSchemaTable | GetBoolean | GetByte | GetBytes | GetChar | GetChars | GetData | GetDateTime | GetDecimal | GetDouble | GetFloat | GetGuid | GetInt16 | GetInt32 | GetInt64 | GetString | GetValue | GetValues,
    }
}
