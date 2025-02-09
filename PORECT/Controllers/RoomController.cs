using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PORECT.Helper;
using Tes.Domain;

namespace PORECT.Controllers
{
    public class RoomController : ParentController
    {
        #region Form
        public IActionResult Index()
        {
            try
            {
                var data = new List<MsRoomResponse>();
                ReturnToken jwtToken = GenerateJwtToken();
                List<ParamTaskViewModel> listParamHeader = new List<ParamTaskViewModel>
                {
                    new ParamTaskViewModel
                    {
                        colName = "Authorization",
                        value = string.Concat("Bearer ", jwtToken.Token)
                    }
                };
                data = _api.Get<List<MsRoomResponse>>(new(), AppConfig.Config.ConfigAPI.Room.BaseUrl, AppConfig.Config.ConfigAPI.Room.List.Endpoint,
                    AppConfig.Config.ConfigAPI.Room.BaseUrl.Split('/')[0] == "https:", listParamHeader);
                return View(data);
            }
            catch (Exception ex)
            {
                logger.WriteErrorToLog(ex, "Room", "Form Index");
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
                var list = _api.Get<List<MsRoomResponse>>(param, AppConfig.Config.ConfigAPI.Room.BaseUrl, AppConfig.Config.ConfigAPI.Room.List.Endpoint,
                    AppConfig.Config.ConfigAPI.Room.BaseUrl.Split('/')[0] == "https:", listParamHeader);
                MsRoomViewModel model = new MsRoomViewModel
                {
                    FormMode = data.FormMode,
                    Room = data.FormMode.ToLower() == "insert" ? new() : list.OrderByDescending(x => x.CreatedDtm).FirstOrDefault()
                };

                return View("_Detail", model);
            }
            catch (Exception ex)
            {
                logger.WriteErrorToLog(ex, "Product", "Form Detail");
                throw;
            }
        }

        public IActionResult Download()
        {
            try
            {
                var roles = HttpContext.Session.GetString("Roles");
                if (HttpContext.Session.GetString("Fullname") == null || roles == null ||
                    !roles.Split(';').Any(x => x.ToLower() == "admin"))
                {
                    return RedirectToAction("Login", "Login");
                }

                return View("_Download");
            }
            catch (Exception ex)
            {
                logger.WriteErrorToLog(ex, "Room", "Form Download");
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
                string result = _api.PostString(json, AppConfig.Config.ConfigAPI.Room.BaseUrl, AppConfig.Config.ConfigAPI.Room.Submit.Endpoint, default, false,
                    AppConfig.Config.ConfigAPI.Room.BaseUrl.Split('/')[0] == "https:", listParamHeader);
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

        #region Upload/Download
        /// <summary>
        /// Download template for upload data
        /// </summary>
        /// <returns>Template file (excel)</returns>
        [HttpGet]
        public IActionResult DownloadTemplate(GeneralViewModel data)
        {
            try
            {
                var roles = HttpContext.Session.GetString("Roles");
                if (HttpContext.Session.GetString("Fullname") == null || roles == null ||
                    (data.Value == "Room" && !roles.Split(';').Any(x => x.ToLower() == "admin")))
                {
                    return RedirectToAction("Login", "Login");
                }

                List<ParamTaskViewModel> param = new List<ParamTaskViewModel>
                {
                    new ParamTaskViewModel
                    {
                        colName = "Value",
                        value = data.Value
                    },
                    new ParamTaskViewModel
                    {
                        colName = "Text",
                        value = "Download Template"
                    }
                };
                ReturnToken jwtToken = GenerateJwtToken();
                List<ParamTaskViewModel> listParamHeader = new List<ParamTaskViewModel>
                {
                    new ParamTaskViewModel
                    {
                        colName = "Authorization",
                        value = string.Concat("Bearer ", jwtToken.Token)
                    }
                };
                var response = _api.Get<TransactionResponse>(param, AppConfig.Config.ConfigAPI.Room.BaseUrl, AppConfig.Config.ConfigAPI.Room.DownloadTemplateStream.Endpoint,
                    AppConfig.Config.ConfigAPI.Room.BaseUrl.Split('/')[0] == "https:", listParamHeader);
                var result = Convert.FromBase64String(response.Data.ToString());
                if (response.IsSuccess == false || result.Length == 0)
                    return NotFound("File not found");

                return File(result,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    string.Format("Template_Upload_{0}.xlsx", data.Value)
                );
            }
            catch (Exception ex)
            {
                logger.WriteErrorToLog(ex, "Room", "DownloadTemplate");
                throw;
            }
        }
        /// <summary>
        /// Download Room Booking Report based on period
        /// </summary>
        /// <returns>Report file (excel)</returns>
        [HttpGet]
        public IActionResult DownloadReportBooking(ListChoiceWithId data)
        {
            try
            {
                var roles = HttpContext.Session.GetString("Roles");
                if (HttpContext.Session.GetString("Fullname") == null || roles == null ||
                    !roles.Split(';').Any(x => x.ToLower() == "admin"))
                {
                    return RedirectToAction("Login", "Login");
                }

                ReturnToken jwtToken = GenerateJwtToken();
                List<ParamTaskViewModel> param = new List<ParamTaskViewModel>
                {
                    new ParamTaskViewModel
                    {
                        colName = "Value",
                        value = data.Value.ToString()
                    },
                    new ParamTaskViewModel
                    {
                        colName = "Text",
                        value = !string.IsNullOrEmpty(data.Text) ? data.Text : "All"
                    }
                };
                List<ParamTaskViewModel> listParamHeader = new List<ParamTaskViewModel>
                {
                    new ParamTaskViewModel
                    {
                        colName = "Authorization",
                        value = string.Concat("Bearer ", jwtToken.Token)
                    }
                };
                var response = _api.Get<TransactionResponse>(param, AppConfig.Config.ConfigAPI.Room.BaseUrl, AppConfig.Config.ConfigAPI.Room.DownloadReportBookingStream.Endpoint,
                    AppConfig.Config.ConfigAPI.Room.BaseUrl.Split('/')[0] == "https:", listParamHeader);
                if(response.IsSuccess == false)
                    return NotFound(response.Message);
                var result = Convert.FromBase64String(response.Data.ToString());
                if (result.Length == 0)
                    return NotFound("Fail to generate report. Please try again.");

                return File(result,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    string.Format("Room_Booking_{0}.xlsx", //data.Text)
                        string.Format("{0}{1}", !string.IsNullOrEmpty(data.Text) ?
                            string.Concat(Convert.ToInt32(data.Text).ToMonthEnum().ToString(), " ") : string.Empty,
                            data.Value.ToString()
                        ))
                );
            }
            catch (Exception ex)
            {
                logger.WriteErrorToLog(ex, "Room", "DownloadReportBooking");
                throw;
            }
        }
        #endregion Upload/Download

    }
}
