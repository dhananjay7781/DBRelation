namespace WebApplication1.Exceptions
{
    public class ValidationException : Exception
    {
        public ValidationException() : base()
        {
            Errors = new List<string>();
        }

        public ValidationException(string message, List<string> errors) : base(message)
        {
            Errors = new List<string>();

            Errors.AddRange(errors);
        }

        public List<string> Errors { get; set; }
    }
}
