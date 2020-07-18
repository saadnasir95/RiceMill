using System;

namespace TheRiceMill.Application.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string name, object key)
            : base($"Entity \"{name}\" ({key}) was not found.")
        {
        }
    }

    public class AlreadyExistsException : Exception
    {
        public AlreadyExistsException(string name,string property, object key) : base($"Entity of Type \"{name}\" already exists with {property} ({key})")
        {
            
        }
    }

    public class CannotDeleteException : Exception
    {
        public CannotDeleteException(string name, object key) : base($"Entity of Type \"{name}\" ({key}) cannot be deleted as its already in use or doesn't exist")
        {

        }
    }
}