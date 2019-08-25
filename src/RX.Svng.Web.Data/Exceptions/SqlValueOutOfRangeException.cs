using System;

namespace RX.Svng.Web.Data.Exceptions
{
    public class SqlValueOutOfRangeException : Exception
    {
        public SqlValueOutOfRangeException(string message): base(message)
        {
        }
    }
}
