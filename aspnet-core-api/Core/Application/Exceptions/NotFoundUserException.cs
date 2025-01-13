using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exceptions
{
    internal class NotFoundUserException : Exception
    {
        public NotFoundUserException() : base("Authentication error: Invalid credentials.")
        {
        }
        public NotFoundUserException(string? message) : base(message)
        {
        }
        public NotFoundUserException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
