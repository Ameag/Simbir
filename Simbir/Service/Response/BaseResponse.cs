using System.Net;

namespace Simbir.Service.Response
{
    public class BaseResponse<T>:IBaseResponse<T>
    {
        public string Description { get; set; }
        public T Data { get; set; }
        public HttpStatusCode Status { get; set; }

    }

    public interface IBaseResponse<T>
    {
        T Data { get; set; }
        HttpStatusCode Status { get; set; }
    }
}
