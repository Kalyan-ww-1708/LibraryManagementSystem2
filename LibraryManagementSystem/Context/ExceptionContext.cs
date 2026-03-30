namespace LibraryManagementSystem.Context
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message) { }
    }

    public class UnauthorizedException : Exception
    {
        public UnauthorizedException(string message) : base(message) { }
    }
    public class InValidException : Exception
    {
        public InValidException(string message) : base(message) { }
    }
    public class ConflictException : Exception
    {
        public ConflictException(string message) : base(message) { }
    }
}
