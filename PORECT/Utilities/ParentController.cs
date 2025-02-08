using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PORECT.Helper;
using Tes.Domain;

namespace PORECT
{
    public class ParentController : Controller
    {
        protected readonly PORECTLog logger = new PORECTLog();
        protected readonly ApiProcessHelper _api = new ApiProcessHelper();
        protected readonly IHttpContextAccessor _contextAccessor;

        public ParentController()
        {
            
        }
        public ParentController(IServiceProvider? serviceProvider, IHttpContextAccessor? contextAccessor = null)
        {
            if (contextAccessor != null)
                _contextAccessor = contextAccessor;
        }

        protected ReturnToken GenerateJwtToken()
        {
            try
            {
                ReturnToken jwtToken = new ReturnToken();
                //string key = _api.Get<TransactionResponse>(new(), AppConfig.Config.ConfigAPI.Auth.GenerateKey.BaseUrl, AppConfig.Config.ConfigAPI.Auth.GenerateKey.Endpoint,
                //    AppConfig.Config.ConfigAPI.Auth.GenerateKey.BaseUrl.Split('/')[0] == "https:").Data?.ToString();
                //if (string.IsNullOrEmpty(key))
                //    throw new Exception("Token key is empty");

                var param = new AppUserRequest
                {
                    Username = AppConfig.Config.ConfigJwt.Username,
                    Password = AppConfig.Config.ConfigJwt.Password
                };
                var json = JsonConvert.SerializeObject(param);
                var result = _api.PostString(json, AppConfig.Config.ConfigAPI.Auth.BaseUrl, AppConfig.Config.ConfigAPI.Auth.Login.Endpoint, default, false,
                    AppConfig.Config.ConfigAPI.Auth.BaseUrl.Split('/')[0] == "https:");
                if (string.IsNullOrEmpty(result))
                    throw new Exception("Fail to get JWT Token");
                jwtToken = JsonConvert.DeserializeObject<ReturnToken>(result);

                return jwtToken;
            }
            catch
            {
                throw;
            }
        }

    }
}
