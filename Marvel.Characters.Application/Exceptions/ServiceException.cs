namespace Marvel.Characters.Application.Exceptions
{
    [Serializable]
    public class ServiceException : Exception
    {
        public string Error { get; set; }
        public string DisplayMessage { get; set; }
        public string Details { get; set; }

        public ServiceException(string code, string description, Exception exception)
            : base(code, exception)
        {
            Error = "Internal Server Error";
            DisplayMessage = description;
            Details = $"{code} : {description} - {exception.Message}";
        }

        protected ServiceException(System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
