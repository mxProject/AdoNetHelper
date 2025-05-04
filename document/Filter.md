
## Filter pattern

Filters are a mechanism for suspending any process. Filters can be applied to the following interface methods:

* IDbConnection
* IDbTransaction
* IDbCommand
* IDataParameterCollection
* IDataReader

```csharp
// Creates filters
var connectionFilters = new IDbConnectionFilter[]{ new SampleConnectionFilter() };
var commandFilters = new IDbCommandFilter[]{ new SampleCommandFilter() };

// Applies filters to connection
using var connection = CreateConnection().WithFilter(
    connectionFilters: connectionFilters,
    commandFilters: commandFilters
    );
```

The SampleConnectionFilter class is a filter that sets default values ​​for generated commands. The CreateCommand method on a connection with this filter applied will return a command with the CommandTimeout property set to 60.

```csharp
internal class SampleConnectionFilter : DbConnectionFilterBase
{
    internal SampleConnectionFilter()
    {
    }

    public override DbConnectionFilterTargets TargetMethods =>
        DbConnectionFilterTargets.CreateCommand;

    public override IDbCommand CreateCommand(IDbConnection connection, Func<IDbConnection, IDbCommand> continuation)
    {
        var command = continuation(connection);
        command.CommandTimeout = 60;
        return command;
    }
}
```

The SampleCommandFilter class is a filter that outputs logs when a command is executed. Commands with this filter applied will output logs before and after execution.

```csharp
internal class SampleCommandFilter : DbCommandFilterBase
{
    internal SampleCommandFilter()
    {
    }

    public override DbCommandFilterTargets TargetMethods =>
        DbCommandFilterTargets.ExecuteNonQuery | DbCommandFilterTargets.ExecuteReader | DbCommandFilterTargets.ExecuteScalar;

    public override int ExecuteNonQuery(IDbCommand command, Func<IDbCommand, int> continuation)
    {
        WriteExecutingLog(command);
        try
        {
            var result = continuation(command);
            WriteExecutedLog(command);
            return result;
        }
        catch (Exception ex)
        {
            WriteExceptionLog(command, ex);
            throw;
        }
    }

    public override IDataReader ExecuteReader(IDbCommand command, CommandBehavior behavior, Func<IDbCommand, CommandBehavior, IDataReader> continuation)
    {
        WriteExecutingLog(command);
        try
        {
            var result = continuation(command, behavior);
            WriteExecutedLog(command);
            return result;
        }
        catch (Exception ex)
        {
            WriteExceptionLog(command, ex);
            throw;
        }
    }

    public override object ExecuteScalar(IDbCommand command, Func<IDbCommand, object> continuation)
    {
        WriteExecutingLog(command);
        try
        {
            var result = continuation(command);
            WriteExecutedLog(command);
            return result;
        }
        catch (Exception ex)
        {
            WriteExceptionLog(command, ex);
            throw;
        }
    }

    private void WriteExecutingLog(IDbCommand command)
    {
        // Outputting log
    }

    private void WriteExecutedLog(IDbCommand command)
    {
        // Outputting log
    }

    private void WriteExceptionLog(IDbCommand command, Exception exception)
    {
        // Outputting log
    }
}
```

### DbConnectionFactory class

The DbConnectionFactory class is a factory class for creating connections. Use this factory class if you want to create a connection on which a filter will be applied multiple times, thereby avoiding the overhead of initializing the filter.

```csharp
// Creates filters
var connectionFilters = new IDbConnectionFilter[]{ new SampleConnectionFilter() };
var commandFilters = new IDbCommandFilter[]{ new SampleCommandFilter() };

// Creating a factory with filters
var factory = new DbConnectionFactory(
    connectionFilters: connectionFilters,
    commandFilters: commandFilters
);

// Creates connection
using var connection = factory.CreateConnection();
```