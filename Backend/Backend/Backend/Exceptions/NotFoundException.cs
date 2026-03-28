namespace Backend.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message)
        {
        }

        public NotFoundException(string name,object entity) : base($"product \"{name}\" {entity} was not found.")
        {
        }
    }
}
