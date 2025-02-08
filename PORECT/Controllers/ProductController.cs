using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PORECT.Helper;
using Tes.Domain;

namespace PORECT.Controllers
{
    public class ProductController : ParentController
    {
        #region Form
        // GET: AppMasterItems
        public IActionResult Index()
        {
            try
            {
                if (HttpContext.Session.GetString("Username") == null)
                {
                    return RedirectToAction("Login", "Login");
                }

                ReturnToken jwtToken = GenerateJwtToken();
                List<ParamTaskViewModel> listParamHeader = new List<ParamTaskViewModel>
                {
                    new ParamTaskViewModel
                    {
                        colName = "Authorization",
                        value = string.Concat("Bearer ", jwtToken.Token)
                    }
                };
                var data = _api.Get<List<RoomResponse>>(new(), AppConfig.Config.ConfigAPI.Product.BaseUrl, AppConfig.Config.ConfigAPI.Product.List.Endpoint,
                    AppConfig.Config.ConfigAPI.Product.BaseUrl.Split('/')[0] == "https:", listParamHeader);
                return View(data);
            }
            catch (Exception ex)
            {
                logger.WriteErrorToLog(ex, "Product", "Form Index");
                throw;
            }
        }

        public IActionResult Detail(GeneralViewModel data)
        {
            try
            {
                if (HttpContext.Session.GetString("Username") == null)
                {
                    return RedirectToAction("Login", "Login");
                }

                ReturnToken jwtToken = GenerateJwtToken();
                List<ParamTaskViewModel> listParamHeader = new List<ParamTaskViewModel>
                {
                    new ParamTaskViewModel
                    {
                        colName = "Authorization",
                        value = string.Concat("Bearer ", jwtToken.Token)
                    }
                };
                var param = new List<ParamTaskViewModel>
                {
                    new ParamTaskViewModel
                    {
                        colName = "ID",
                        value = (data.ID ?? -1).ToString()
                    }
                };
                var list = _api.Get<List<RoomResponse>>(param, AppConfig.Config.ConfigAPI.Product.BaseUrl, AppConfig.Config.ConfigAPI.Product.List.Endpoint,
                    AppConfig.Config.ConfigAPI.Product.BaseUrl.Split('/')[0] == "https:", listParamHeader);
                MsRoomViewModel model = new MsRoomViewModel
                {
                    FormMode = data.FormMode,
                    Product = data.FormMode.ToLower() == "insert" ? new() : list.OrderByDescending(x => x.CreatedDtm).FirstOrDefault()
                };

                return View("_Detail", model);
            }
            catch (Exception ex)
            {
                logger.WriteErrorToLog(ex, "Product", "Form Detail");
                throw;
            }
        }
        #endregion Form

        #region Transaction
        [HttpPost]
        public IActionResult Submit(string? data)
        {
            try
            {
                var username = HttpContext.Session.GetString("Username");
                if (username == null)
                {
                    return RedirectToAction("Login", "Login");
                }

                TransactionResponse response = new TransactionResponse();
                if (string.IsNullOrEmpty(data))
                {
                    response.IsSuccess = false;
                    response.Message = "Parameter is empty";
                    return Json(response);
                }

                MsRoomRequest model = JsonConvert.DeserializeObject<MsRoomRequest>(data);
                if (!ModelState.IsValid)
                {
                    //throw new Exception("Please check your input");
                    response.IsSuccess = false;
                    response.Message = "Please check your input";
                    return Json(response);
                }
                model.CreatedBy = username;

                ReturnToken jwtToken = GenerateJwtToken();
                List<ParamTaskViewModel> listParamHeader = new List<ParamTaskViewModel>
                {
                    new ParamTaskViewModel
                    {
                        colName = "Authorization",
                        value = string.Concat("Bearer ", jwtToken.Token)
                    }
                };
                string json = JsonConvert.SerializeObject(model);
                string result = _api.PostString(json, AppConfig.Config.ConfigAPI.Product.BaseUrl, AppConfig.Config.ConfigAPI.Product.Submit.Endpoint, default, false,
                    AppConfig.Config.ConfigAPI.Product.BaseUrl.Split('/')[0] == "https:", listParamHeader);
                if (!string.IsNullOrEmpty(result))
                    response = JsonConvert.DeserializeObject<TransactionResponse>(result);
                else
                {
                    response.IsSuccess = true;
                    response.Message = "Transaction success but no response from api";
                }

                return Json(response);
            }
            catch (Exception ex)
            {
                logger.WriteErrorToLog(ex, "Product", "Submit");
                throw;
            }
        }
        #endregion Transaction

    }
}
