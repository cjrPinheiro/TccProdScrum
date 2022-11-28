using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PS.Connector.Base.Interface
{
    public interface IBaseApiClient
    {
        Task<HttpResponseMessage> PostAsync(object body, string endpoint);
        Task<HttpResponseMessage> GetAsync(string endpoint, Dictionary<string, string> @params = null);
    }
}
