using System;

namespace UFW.Net
{
    /// <summary>
    /// Represents errors that occur during run command.
    /// </summary>

    public class UfwException : Exception
    {
        public UfwException(string message) : base(message)
        {
        }
    }
}
