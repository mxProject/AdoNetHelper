
## Extension methods

Although the interfaces provided by ADO\.NET allow for implementation that is independent of any particular database provider, these interfaces only provide basic functionality. Provides frequently required operations as extension methods for ADO\.NET interfaces.

### IDbConnection interface

#### OpenIfClosed method

If the connection is closed it will be opened, but if it is already open nothing will be done.

```csharp
void Execute(IDbConnection connection)
{
    // Open the connection if it is closed
    bool opened = connection.OpenIfClosed();

    try
    {
        var command = connection.CreateCommand();
        command.CommandText = "update SAMPLE_TABLE set ...";
        command.ExecuteNonQuery();
    }
    finally
    {
    // Close connection
    if (opened) { connection.Close(); }
    }
}
```

#### Cast&lt;TConnection&gt; method

Casts itself to a concrete type and performs the specified operation.

* This method casts the wrapped connection if the connection type implements the IDbConnectionWrapper interface. Depending on the specification of the wrapper, it may not be desirable to manipulate the wrapped connection directly, in which case use of this method should be avoided.

```csharp
IDbConnection connection = CreateConnection();

connection.Cast<SqlConnection>(x => {
    // Sets the value of a SqlConnection specific property
    x.StatisticsEnabled = true;
});
```

---
### IDbTransaction interface

#### Cast&lt;TTransaction&gt; method

Casts itself to a concrete type and performs the specified operation.

* This method casts the wrapped transaction if the transaction type implements the IDbTransactionWrapper interface. Depending on the specification of the wrapper, it may not be desirable to manipulate the wrapped transaction directly, in which case use of this method should be avoided.

```csharp
IDbTransaction transaction = connection.BeginTransaction();

transaction.Cast<OracleTransaction>(x => {
    // Executes an OracleTransaction specific method
    x.Save();
});
```

---
### IDbCommand interface

#### Add{ValueType}Parameter method

Add a parameter to the command.

* This method returns a valuetuple of the added parameter's index and the parameters.

* In addition to the AddParameter method, which specifies the parameter values ​​as object types, methods are provided to specify the type of each value. Please note that SByte, UInt16, UInt32, UInt64, and Guid may throw exceptions depending on the database provider. Please refer to the reference of each database provider.
 
  * AddParameter
  * AddBooleanParameter
  * AddByteParameter
  * AddInt16Parameter
  * AddInt32Parameter
  * AddInt64Parameter
  * AddSByteParameter
  * AddUInt16Parameter
  * AddUInt32Parameter
  * AddUInt64Parameter
  * AddSingleParameter
  * AddDoubleParameter
  * AddDecimalParameter
  * AddDateTimeParameter
  * AddDateTimeOffsetParameter
  * AddTimeParameter
  * AddStringParameter
  * AddCharParameter
  * AddGuidParameter
  * AddBinaryParameter

```csharp
IDbCommand command = CreateCommand();

var (index1, parameter2) = command.AddBooleanParameter("param1", true);

var (index1, parameter2) = command.AddStringParameter("param2");
parameter2.Value = "abc";
```

#### GetParameter method

Gets the parameter that corresponds to the specified index or parameter name.

```csharp
IDbCommand command = CreateCommand();

command.AddBooleanParameter("param1", true);
command.AddStringParameter("param2", "abc");

IDbDataParameter param1 = command.GetParameter(0);
IDbDataParameter param2 = command.GetParameter("param2");
```

#### Set{値型}ParameterValue method

Sets the value of the specified parameter.

* In addition to the SetParameterValue method, which specifies the parameter values ​​as object types, methods are provided to specify the type of each value. Please note that SByte, UInt16, UInt32, UInt64, and Guid may throw exceptions depending on the database provider. Please refer to the reference of each database provider.

  * SetParameterValue
  * SetBooleanParameterValue
  * SetByteParameterValue
  * SetInt16ParameterValue
  * SetInt32ParameterValue
  * SetInt64ParameterValue
  * SetSByteParameterValue
  * SetUInt16ParameterValue
  * SetUInt32ParameterValue
  * SetUInt64ParameterValue
  * SetSingleParameterValue
  * SetDoubleParameterValue
  * SetDecimalParameterValue
  * SetDateTimeParameterValue
  * SetDateTimeOffsetParameterValue
  * SetTimeParameterValue
  * SetStringParameterValue
  * SetCharParameterValue
  * SetGuidParameterValue
  * SetBinaryParameterValue

```csharp
IDbCommand command = CreateCommand();

command.AddBooleanParameter("param1");
command.AddStringParameter("param2");

command.SetBooleanParameterValue(0, true);
command.SetStringParameterValue("param2", "abc");
```

#### Cast&lt;TCommand&gt; method

Casts itself to a concrete type and performs the specified operation.

* This method casts the wrapped command if the command type implements the IDbCommandWrapper interface. Depending on the specification of the wrapper, it may not be desirable to manipulate the wrapped command directly, in which case use of this method should be avoided.

```csharp
IDbCommand command = connection.CreateCommand();

command.Cast<OracleCommand>(x => {
    // Sets the value of an OracleCommand specific property
    x.BindByName = true;
});
```

---
### IDataParameterCollection interface

#### Cast&lt;TParameters&gt; method

Casts itself to a concrete type and performs the specified operation.

* This method casts the wrapped collection if the collection type implements the IDataParameterCollectionWrapper interface. Depending on the specification of the wrapper, it may not be desirable to manipulate the wrapped collection directly, in which case use of this method should be avoided.

```csharp
using var connection = SampleDatabase.CreateConnection();

var command = connection.CreateCommand();

command.CommandText = "select * from SAMPLE_TABLE where ID = @id";

command.Parameters.Cast<SqlParameterCollection>(x =>
{
    // Executing SqlParameterCollection specific methods
    x.AddWithValue("id", 1);
});
```

---
### IDbDataParameter interface

#### Set{ValueType}Value method

Sets the value of the specified parameter.

* In addition to the SetValue method, which specifies the parameter values ​​as object types, methods are provided to specify the type of each value. Please note that SByte, UInt16, UInt32, UInt64, and Guid may throw exceptions depending on the database provider. Please refer to the reference of each database provider.

  * SetValue
  * SetBooleanValue
  * SetByteValue
  * SetInt16Value
  * SetInt32Value
  * SetInt64Value
  * SetSByteValue
  * SetUInt16Value
  * SetUInt32Value
  * SetUInt64Value
  * SetSingleValue
  * SetDoubleValue
  * SetDecimalValue
  * SetDateTimeValue
  * SetDateTimeOffsetValue
  * SetTimeValue
  * SetStringValue
  * SetCharValue
  * SetGuidValue
  * SetBinaryValue

```csharp
IDbCommand command = CreateCommand();

var (_, param1) = command.AddBooleanParameter("param1");
var (_, param2) = command.AddStringParameter("param2");

param1.SetBooleanValue(true);
param2.SetStringValue("abc");
```

#### Cast&lt;TParameter&gt; method

Casts itself to a concrete type and performs the specified operation.

* This method casts the wrapped parameter if the parameter type implements the IDbDataParameterWrapper interface. Depending on the specification of the wrapper, it may not be desirable to manipulate the wrapped parameter directly, in which case use of this method should be avoided.

```csharp
IDbCommand command = CreateCommand();

var (_, param1) = command.AddBooleanParameter("param1");

param1.Cast<SqlParameter>(x => {
    // Set the value of a SqlParameter specific property
    x.ForceColumnEncryption = true;
});
```

---
### IDataReader interface

#### Cast&lt;TCommand&gt; method

Casts itself to a concrete type and performs the specified operation.

* This method casts the wrapped datareader if the datareader type implements the IDataReaderWrapper interface. Depending on the specification of the wrapper, it may not be desirable to manipulate the wrapped datareader directly, in which case use of this method should be avoided.

```csharp
using var connection = SampleDatabase.CreateConnection();

connection.Open();

using (var command = connection.CreateCommand())
{
    command.CommandText = "select 1 as ID, 'abc' as NAME";

    using var reader = command.ExecuteReader();

    reader.Cast<SqlDataReader>(x =>
    {
        // Executing SqlDataReader specific method
        var idType = x.GetProviderSpecificFieldType(0);
    });
}
```

---
### IDataRecord interface

#### IsDBNull(string name) method

This IsDBNull method takes the field name as an argument.

* Asynchronous methods are also provided.

```csharp
IDataReader reader = GetData();

var isnull = reader.IsDBNull("id");
```

#### Get{ValueType}(string name) method

This GetValue method takes the field name as an argument.

* Asynchronous methods are also provided.

```csharp
IDataReader reader = GetData();

var field1 = reader.GetValue("field1");
var field2 = reader.GetBoolean("field2");
var field3 = reader.GetString("field3");
```

#### Get{ValueType}OrNull method

Gets the value of the specified field, or null if the field's value is DBNull.

* The return type is Nullable&lt;T&gt;. 
* The return type of the GetStringOrNull method is String, but it is defined as a String?, indicating that it can be null.

* Asynchronous methods are also provided.

```csharp
IDataReader reader = GetData();

bool? field1 = reader.GetBooleanOrNull("field1");
string? field2 = reader.GetStringOrNull("field2");
```

#### Get{ValueType}OrDefault method

Gets the value of the specified field. If the field's value is DBNull, returns the default value for that value's type.

* Asynchronous methods are also provided.

```csharp
IDataReader reader = GetData();

bool field1 = reader.GetBooleanOrDefault("field1");
```

#### GetStringOrDefault method

Gets the value of the specified field. If the field's value is DBNull, returns String.Empty.

* Asynchronous methods are also provided.

```csharp
IDataReader reader = GetData();

string field2 = reader.GetStringOrEmpty("field2");
```
