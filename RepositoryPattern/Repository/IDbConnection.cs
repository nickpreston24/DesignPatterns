namespace DesignPatterns
{
    /// <summary>
    /// Represents a connection type for injecting into a Unit of Work
    /// Can be SqlConnection, MongoDbConnection, EFCore, etc.
    /// One exists already: https://docs.microsoft.com/en-us/dotnet/api/system.data.idbconnection?view=netframework-4.8
    /// </summary>
    public interface IDbConnection
    {
    }

}