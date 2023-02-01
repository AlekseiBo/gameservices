using Framework;

namespace GameServices
{
    public interface ISampleService : IService
    {
        public string Message { get; set; }
    }
}