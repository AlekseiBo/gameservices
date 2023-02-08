namespace GameServices
{
    public class SampleService : ISampleService
    {
        public SampleService(string message) => Message = message;
        public string Message { get; set; }
    }
}