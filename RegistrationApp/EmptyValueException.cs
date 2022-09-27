using System;

namespace RegistrationApp
{
    public class EmptyValueException : Exception
    {
        public string Field { get; set; }
        public EmptyValueException() { }
        public EmptyValueException(string message) : base(message) { }
        public EmptyValueException(string message, Exception innerException) : base(message, innerException) { }
        public EmptyValueException(string message, string field) :this(message)
        {
            Field = field;
        }

    }
}
