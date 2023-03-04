namespace WebApplication1.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException() : base()
        {
            Errors = new List<string>();
        }

        public NotFoundException(string message, string error) : base(message)
        {
            Errors = new List<string>();

            Errors.Add(error);
        }

        public NotFoundException(string message, List<string> errors) : base(message)
        {
            Errors = new List<string>();

            Errors.AddRange(errors);
        }

        public List<string> Errors { get; set; }
    }
}
