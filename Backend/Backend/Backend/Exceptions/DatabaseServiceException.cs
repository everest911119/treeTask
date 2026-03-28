namespace Backend.Exceptions
{
    public class DatabaseServiceException : Exception
    {
        public DatabaseServiceException(string message) : base(message)
        {
        }

        public string? Details { get; set; }
        public DatabaseServiceException(string message, string details) : base(message)
        {
            Details = details;
        }
    }
}
