using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exceptions
{
    public class UserCreateFailedException : Exception
    {
        public IEnumerable<string> Errors { get; }

        public UserCreateFailedException() : base("An error occurred while creating the user.")
        {
            Errors = Enumerable.Empty<string>();
        }

        public UserCreateFailedException(IEnumerable<string> errors)
            : base("An error occurred while creating the user.")
        {
            Errors = errors;
        }

        public UserCreateFailedException(string? message, IEnumerable<string> errors)
            : base(message)
        {
            Errors = errors;
        }
    }

}
