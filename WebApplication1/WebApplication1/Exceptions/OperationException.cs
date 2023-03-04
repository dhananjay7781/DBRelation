namespace WebApplication1.Exceptions
{
    public class OperationException : Exception
    {
        public OperationException() : base()
        {
            Errors = new List<string>();
        }

        public OperationException(string message, string error) : base(message)
        {
            Errors = new List<string>();

            Errors.Add(error);
        }

        public OperationException(string message, List<string> errors) : base(message)
        {
            Errors = new List<string>();

            Errors.AddRange(errors);
        }

        public List<string> Errors { get; set; }
    }
}
