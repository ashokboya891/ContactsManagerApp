namespace Exceptions
{
    public class InvalidPersonIDException:ArgumentException
    {
        public InvalidPersonIDException() :base()
        {
        }
        public InvalidPersonIDException(string? message) : base(message)
        {
            Console.Out.WriteLine("we are in invalid constu");
        }
        public InvalidPersonIDException(string? message,Exception? innerException) : base(message,innerException)
        {
        }
    }
}
