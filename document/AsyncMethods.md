

## Asynchronous Methods

.NET Standard 2.1 introduced support for asynchronous methods. This package brings the same functionality to .NET Standard 2.0.

```csharp
IDbConnection connection = SampleDatabase.CreateConnection();

try
{
    await connection.OpenAsync(cancellationToken);

    using var transaction = await connection.BeginTransactionAsync(cancellationToken: cancellationToken);

    using var command = connection.CreateCommand();

    command.CommandText = "insert into SAMPLE_TABLE values (1, 'abc')";
    command.Transaction = transaction;

    await command.ExecuteNonQueryAsync(cancellationToken);

    await transaction.CommitAsync(cancellationToken);
}
finally
{
    await connection.DisposeAsync();
}
```

The following methods have been added:

**IDbConnection**

* DisposeAsync
* OpenAsync
* CloseAsync
* ChangeDatabaseAsync
* BeginTransactionAsync

**IDbTransaction**

* DisposeAsync
* CommitAsync
* RollbackAsync

**IDbCommand**

* DisposeAsync
* PrepareAsync
* ExecuteReaderAsync
* ExecuteScalarAsync
* ExecuteNonQueryAsync

**IDataReader**

* DisposeAsync
* CloseAsync
* GetSchemaTableAsync
* ReadAsync
* NextResultAsync

**IDataRecord**

* IsDBNullAsync
* GetValueAsync
* GetValuesAsync
 