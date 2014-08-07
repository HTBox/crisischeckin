using System;

namespace Services.Api.Responses
{
    public abstract class ServiceResponse<TResult>
    {
        public bool Succeeded { get; set; }
        public Exception Exception { get; set; }
        public TResult Result { get; set; }
    }
}
