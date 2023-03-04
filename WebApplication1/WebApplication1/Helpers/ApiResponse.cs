﻿namespace WebApplication1.Helpers
{
    public class ApiResponse<T>
    {
        public bool Succeeded { get; set; }

        public string Message { get; set; }

        public List<string> Errors { get; set; }

        public T Data { get; set; }

        public ApiResponse()
        {
            Succeeded = false;
            Errors = new List<string>();
        }
    }
}
