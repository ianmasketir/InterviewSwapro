using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using NPOI.OpenXml4Net.OPC.Internal;
using PORECT.Helper;
using Tes.Business;
using Tes.Domain;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PORECT.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MsRoomController : ParentController
    {
        private readonly IRoomRepository _repository;
        public MsRoomController(IRoomRepository productRepository)
        {
            _repository = productRepository;
        }
        //public RoomController(IServiceProvider serviceProvider) : base(serviceProvider)
        //{

        //}

        #region View
        
        [HttpGet("List")]
        public IActionResult GetList([FromQuery] SearchRoomRequest dto)
        {
            try
            {
                var data = _repository.GetListRoom(dto);
                return Ok(data);
            }
            catch (System.Exception ex)
            {
                logger.WriteErrorToLog(ex, "MsRoom", "GetList");
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        /// <summary>
        /// Get List Room Booking
        /// </summary>
        /// <param name="data"></param>
        /// <returns>Response model.</returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET /Room/ListBooking
        ///     {
        ///         "Name": "Username",
        ///         "Code": "RoomCode"
        ///     }
        /// </remarks>
        [HttpGet("ListBooking")]
        public IActionResult GetListBooking([FromQuery] SearchRoomRequest dto)
        {
            try
            {
                var data = _repository.GetListBooking(dto);
                return Ok(data);
            }
            catch (System.Exception ex)
            {
                logger.WriteErrorToLog(ex, "MsRoom", "GetListBooking");
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }
        #endregion View

        #region Transaction
        /// <summary>
        /// This API for insert/update/delete Room data.
        /// </summary>
        /// <param name="data"></param>
        /// <returns>Response model.</returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /Room/Submit
        ///     {
        ///         "RoomCode": "Tes-001",
        ///         "TransactionType": "Insert/Update/Delete"
        ///     }
        /// </remarks>
        [HttpPost("Submit")]
        public IActionResult Submit([FromBody] MsRoomRequest data)
        {
            try
            {
                if (data == null || data == default)
                {
                    return BadRequest("Parameter is empty!");
                }

                var result = _repository.SubmitMsRoom(data);
                return Ok(result);
            }
            catch (System.Exception ex)
            {
                logger.WriteErrorToLog(ex, "MsRoom", "Submit");
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }
        /// <summary>
        /// This API for upload Room data.
        /// </summary>
        /// <param name="data"></param>
        /// <returns>Response model.</returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /Room/UploadRoom
        ///     {
        ///         "File": "excel file",
        ///         "CreatedBy": "v-taufiq"
        ///     }
        /// </remarks>
        [HttpPost("UploadRoom")]
        public IActionResult UploadRoom([FromForm] UploadViewModel data)
        {
            try
            {
                if (data == null || data == default || data.File == null)
                {
                    return BadRequest("Parameter is empty!");
                }

                var result = _repository.UploadRoomFile(data);
                return Ok(result);
            }
            catch (System.Exception ex)
            {
                logger.WriteErrorToLog(ex, "MsRoom", "UploadRoom");
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        /// <summary>
        /// This API for book a room or update status booking
        /// </summary>
        /// <param name="data"></param>
        /// <returns>Response model.</returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /Room/SubmitRoomBooking
        ///     {
        ///         "ObjectID": "8-digit guid",
        ///         "RoomCode": "Tes-001",
        ///         "TransactionType": "Insert/Update/Delete",
        ///         "Status": "Booked/CheckedIn/CheckedOut"
        ///     }
        /// </remarks>
        [HttpPost("SubmitRoomBooking")]
        public IActionResult SubmitRoomBooking([FromBody] BookingRequest data)
        {
            try
            {
                if (data == null || data == default)
                {
                    return BadRequest("Parameter is empty!");
                }

                var result = _repository.SubmitRoomBooking(data);
                return Ok(result);
            }
            catch (System.Exception ex)
            {
                logger.WriteErrorToLog(ex, "MsRoom", "SubmitRoomBooking");
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }
        /// <summary>
        /// This API for upload Booking data.
        /// </summary>
        /// <param name="data"></param>
        /// <returns>Response model.</returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /Room/UploadBooking
        ///     {
        ///         "File": "excel file",
        ///         "CreatedBy": "v-taufiq"
        ///     }
        /// </remarks>
        [HttpPost("UploadBooking")]
        public IActionResult UploadBooking([FromForm] UploadViewModel data)
        {
            try
            {
                if (data == null || data == default || data.File == null)
                {
                    return BadRequest("Parameter is empty!");
                }

                var result = _repository.UploadBookingFile(data);
                return Ok(result);
            }
            catch (System.Exception ex)
            {
                logger.WriteErrorToLog(ex, "MsRoom", "UploadBooking");
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }
        #endregion Transaction

        #region Download
        /// <summary>
        /// Download template for upload Room/Booking data
        /// </summary>
        /// <param name="data">Value = type template</param>
        /// <returns>Template file (excel)</returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET /Room/DownloadTemplate
        ///     {
        ///         "Value": "Room/Booking",
        ///         "Text": ""
        ///     }
        /// </remarks>
        [HttpGet("DownloadTemplate")]
        public IActionResult DownloadTemplate([FromQuery] ListChoiceWithString data)
        {
            try
            {
                if (string.IsNullOrEmpty(data.Value.Trim()) ||
                    (data.Value.ToLower().Trim() != UploadTemplateTypeEnum.Room.ToString().ToLower() &&
                    data.Value.ToLower().Trim() != UploadTemplateTypeEnum.Booking.ToString().ToLower()))
                    return BadRequest("Template not found");

                string type = data.Value.ToLower().Trim() == UploadTemplateTypeEnum.Room.ToString().ToLower() ?
                                UploadTemplateTypeEnum.Room.ToString() : UploadTemplateTypeEnum.Booking.ToString();
                string filePath = data.Value.ToLower() == UploadTemplateTypeEnum.Room.ToString().ToLower() ?
                                    AppConfig.Config.MsRoomTemplate.Upload : AppConfig.Config.RoomBookingTemplate.Upload;
                if (!System.IO.File.Exists(filePath))
                    return NotFound("Template not found");

                string[] split = filePath.ToLower().Split('.');
                string extension = split.Length > 1 ? split[1] : "xls";
                return File(Helper.FileHelper.DoDownload(filePath).FileByte,
                    extension == "xlsx" ? "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" :
                    "application/vnd.ms-excel",
                    string.Format("Template_Upload_{0}.{1}", type, extension)
                );
            }
            catch (Exception ex)
            {
                logger.WriteErrorToLog(ex, "MsRoom", "DownloadTemplate");
                throw;
            }
        }
        /// <summary>
        /// Download template for upload Room/Booking data
        /// </summary>
        /// <param name="data">Value = type template</param>
        /// <returns>Template file (excel) as byte array</returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET /Room/DownloadTemplateStream
        ///     {
        ///         "Value": "Room/Booking",
        ///         "Text": ""
        ///     }
        /// </remarks>
        [HttpGet("DownloadTemplateStream")]
        public IActionResult DownloadTemplateStream([FromQuery] ListChoiceWithString data)
        {
            try
            {
                if (string.IsNullOrEmpty(data.Value.Trim()) ||
                    (data.Value.ToLower().Trim() != UploadTemplateTypeEnum.Room.ToString().ToLower() &&
                    data.Value.ToLower().Trim() != UploadTemplateTypeEnum.Booking.ToString().ToLower()))
                    return BadRequest("Template not found");

                string type = data.Value.ToLower().Trim() == UploadTemplateTypeEnum.Room.ToString().ToLower() ?
                                UploadTemplateTypeEnum.Room.ToString() : UploadTemplateTypeEnum.Booking.ToString();
                string filePath = data.Value.ToLower() == UploadTemplateTypeEnum.Room.ToString().ToLower() ?
                                    AppConfig.Config.MsRoomTemplate.Upload : AppConfig.Config.RoomBookingTemplate.Upload;
                if (!System.IO.File.Exists(filePath))
                    return NotFound("Template not found");

                var result = new TransactionResponse
                {
                    Data = Helper.FileHelper.DoDownload(filePath).FileByte
                };
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.WriteErrorToLog(ex, "MsRoom", "DownloadTemplateStream");
                throw;
            }
        }

        /// <summary>
        /// Get Room Booking Report based on period
        /// </summary>
        /// <param name="data">Period to search. Text = period.Month.ToString(), Value = period.Year.</param>
        /// <returns>Excel file.</returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET /Room/DownloadReportBooking
        ///     {
        ///         "Value": period.Year,
        ///         "Text": "period.Month"
        ///     }
        /// </remarks>
        [HttpGet("DownloadReportBooking")]
        public IActionResult DownloadReportBooking([FromQuery] ListChoiceWithId data)
        {
            try
            {
                if (int.TryParse(data.Text, out int month) == false)
                    data.Text = string.Empty;

                var file = _repository.DownloadReportBooking(data);

                if (file.Length == 0)
                {
                    return Json(new TransactionResponse
                    {
                        IsSuccess = false,
                        Message = "Fail to write data to excel"
                    });
                }

                return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    string.Format("Room_Booking_{0}.xlsx", //data.Text)
                        string.Format("{0}{1}", !string.IsNullOrEmpty(data.Text) ?
                            string.Concat(Convert.ToInt32(data.Text).ToMonthEnum().ToString(), " ") : string.Empty, 
                            data.Value.ToString()
                        )
                ));
            }
            catch (System.Exception ex)
            {
                logger.WriteErrorToLog(ex, "MsRoom", "DownloadReportBooking");
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }
        /// <summary>
        /// Get Room Booking Report based on period
        /// </summary>
        /// <param name="data">Period to search. Text = period.Month.ToString(), Value = period.Year.</param>
        /// <returns>Excel file as byte array.</returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /Room/DownloadReportBookingStream
        ///     {
        ///         "Value": period.Year,
        ///         "Text": "period.Month"
        ///     }
        /// </remarks>
        [HttpGet("DownloadReportBookingStream")]
        public IActionResult DownloadReportBookingStream([FromQuery] ListChoiceWithId data)
        {
            try
            {
                if (int.TryParse(data.Text, out int month) == false)
                    data.Text = string.Empty;

                var file = _repository.DownloadReportBooking(data);

                if (file.Length == 0)
                {
                    return Ok(new TransactionResponse
                    {
                        IsSuccess = false,
                        Message = "Fail to write data to excel"
                    });
                }

                var result = new TransactionResponse
                {
                    Data = file
                };
                return Ok(result);
            }
            catch (System.Exception ex)
            {
                logger.WriteErrorToLog(ex, "MsRoom", "DownloadReportBookingStream");
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }
        #endregion Download

    }
}
