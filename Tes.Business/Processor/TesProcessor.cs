using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PORECT.Helper;
using Tes.Data;
using Tes.Domain;

namespace Tes.Business
{
    public class UserProcessor : IUserRepository//TesDataAccessControl
    {
        private readonly TesDataAccessControl _access = new TesDataAccessControl();

        #region View
        public List<MsUserResponse> GetListUser(SearchUserRequest data)
        {
            try
            {
                var result = _access.GetListUserAsync(data).Result;
                //untuk case sederhana ini, abstraksi hanya ini karena semua operasi ada di repository

                return result;
            }
            catch
            {
                throw;
            }
        }
        #endregion View

        #region Transaction
        public TransactionResponse SubmitUser(MsUserRequest data)
        {
            try
            {
                var result = new TransactionResponse();

                #region Check Mandatory
                if (string.IsNullOrEmpty(data.TransactionType) || (data.TransactionType.ToLower() != "insert" &&
                    data.TransactionType.ToLower() != "update" && data.TransactionType.ToLower() != "disable" &&
                    data.TransactionType.ToLower() != "enable"))
                {
                    result.IsSuccess = false;
                    result.Message = "Invalid transaction type";
                    result.Data = JsonConvert.SerializeObject(data);
                    return result;
                }

                if (data.ID == null)
                {
                    if (data.TransactionType.ToLower() != "insert")
                    {
                        result.IsSuccess = false;
                        result.Message = "ID is required";
                        result.Data = JsonConvert.SerializeObject(data);
                        return result;
                    }
                }
                if (string.IsNullOrEmpty(data.Username))
                {
                    result.IsSuccess = false;
                    result.Message = "Username is required";
                    result.Data = JsonConvert.SerializeObject(data);
                    return result;
                }
                if (string.IsNullOrEmpty(data.Password))
                {
                    if (data.TransactionType.ToLower() != "disable" && data.TransactionType.ToLower() != "enable")
                    {
                        result.IsSuccess = false;
                        result.Message = "Password is required";
                        result.Data = JsonConvert.SerializeObject(data);
                        return result;
                    }
                }
                if (string.IsNullOrEmpty(data.FirstName))
                {
                    if (data.TransactionType.ToLower() == "insert")
                    {
                        result.IsSuccess = false;
                        result.Message = "First Name is required";
                        result.Data = JsonConvert.SerializeObject(data);
                        return result;
                    }
                }
                #endregion Check Mandatory

                result = _access.SubmitUserAsync(data).Result;

                return result;
            }
            catch
            {
                throw;
            }
        }
        #endregion Transaction

    }
    public class RoomProcessor : IRoomRepository//TesDataAccessControl
    {
        private readonly TesDataAccessControl _access = new TesDataAccessControl();
        private readonly ExcelHelper excel = new ExcelHelper();
        private readonly PORECTLog logger = new PORECTLog();

        #region View
        public List<MsRoomResponse> GetListRoom(SearchRoomRequest dto)
        {
            try
            {
                var result = _access.GetListRoomAsync(dto).Result;
                //untuk case sederhana ini, abstraksi hanya ini karena semua operasi ada di repository

                return result;
            }
            catch
            {
                throw;
            }
        }
        public List<BookingResponse> GetListBooking(SearchRoomRequest dto)
        {
            try
            {
                var result = _access.GetListBookingAsync(dto).Result;
                //untuk case sederhana ini, abstraksi hanya ini karena semua operasi ada di repository

                return result;
            }
            catch
            {
                throw;
            }
        }
        #endregion View

        #region Transaction
        public TransactionResponse SubmitMsRoom(MsRoomRequest data)
        {
            try
            {
                var result = new TransactionResponse();

                #region Check Mandatory
                if (string.IsNullOrEmpty(data.TransactionType) || (data.TransactionType.ToLower() != "insert" &&
                    data.TransactionType.ToLower() != "update" && data.TransactionType.ToLower() != "disable" &&
                    data.TransactionType.ToLower() != "enable"))
                {
                    result.IsSuccess = false;
                    result.Message = "Invalid transaction type";
                    result.Data = JsonConvert.SerializeObject(data);
                    return result;
                }

                if (data.ID == null)
                {
                    if (data.TransactionType.ToLower() != "insert")
                    {
                        result.IsSuccess = false;
                        result.Message = "ID is required";
                        result.Data = JsonConvert.SerializeObject(data);
                        return result;
                    }
                }
                if (string.IsNullOrEmpty(data.Code))
                {
                    result.IsSuccess = false;
                    result.Message = "Code is required";
                    result.Data = JsonConvert.SerializeObject(data);
                    return result;
                }
                if (string.IsNullOrEmpty(data.Name))
                {
                    if (data.TransactionType.ToLower() == "insert")
                    {
                        result.IsSuccess = false;
                        result.Message = "Name is required";
                        result.Data = JsonConvert.SerializeObject(data);
                        return result;
                    }
                }
                if (data.Capacity == null)
                {
                    if (data.TransactionType.ToLower() == "insert")
                    {
                        result.IsSuccess = false;
                        result.Message = "Capacity is required";
                        result.Data = JsonConvert.SerializeObject(data);
                        return result;
                    }
                }
                if (data.Price == null)
                {
                    if (data.TransactionType.ToLower() == "insert")
                    {
                        result.IsSuccess = false;
                        result.Message = "Price is required";
                        result.Data = JsonConvert.SerializeObject(data);
                        return result;
                    }
                }
                else if (data.Price < 10000)
                {
                    if (data.TransactionType.ToLower() == "insert" || data.TransactionType.ToLower() == "update")
                    {
                        result.IsSuccess = false;
                        result.Message = "Price must be higher than 10000";
                        result.Data = JsonConvert.SerializeObject(data);
                        return result;
                    }
                }
                #endregion Check Mandatory

                result = _access.SubmitRoomAsync(data).Result;

                return result;
            }
            catch
            {
                throw;
            }
        }
        public TransactionResponse SubmitRoomBooking(BookingRequest data)
        {
            try
            {
                var result = new TransactionResponse();

                #region Check Mandatory
                if (string.IsNullOrEmpty(data.TransactionType) || (data.TransactionType.ToLower() != "insert" &&
                    data.TransactionType.ToLower() != "update" && data.TransactionType.ToLower() != "delete"))
                {
                    result.IsSuccess = false;
                    result.Message = "Invalid transaction type";
                    result.Data = JsonConvert.SerializeObject(data);
                    return result;
                }

                if (data.ID == null)
                {
                    if (data.TransactionType.ToLower() == "delete")
                    {
                        result.IsSuccess = false;
                        result.Message = "ID is required";
                        result.Data = JsonConvert.SerializeObject(data);
                        return result;
                    }
                }
                //if (data.ObjectID == null)
                //{
                //    result.IsSuccess = false;
                //    result.Message = "ObjectID is required";
                //    result.Data = JsonConvert.SerializeObject(data);
                //    return result;
                //}
                if (string.IsNullOrEmpty(data.Code))
                {
                    result.IsSuccess = false;
                    result.Message = "Code is required";
                    result.Data = JsonConvert.SerializeObject(data);
                    return result;
                }
                if (string.IsNullOrEmpty(data.Username))
                {
                    if (data.TransactionType.ToLower() == "insert")
                    {
                        result.IsSuccess = false;
                        result.Message = "User not registered";
                        result.Data = JsonConvert.SerializeObject(data);
                        return result;
                    }
                }
                if (data.Duration == null)
                {
                    if (data.TransactionType.ToLower() == "insert")
                    {
                        result.IsSuccess = false;
                        result.Message = "Duration is required";
                        result.Data = JsonConvert.SerializeObject(data);
                        return result;
                    }
                }
                if (data.CheckInDate == null)
                {
                    if (data.TransactionType.ToLower() == "insert")
                    {
                        result.IsSuccess = false;
                        result.Message = "CheckInDate is required";
                        result.Data = JsonConvert.SerializeObject(data);
                        return result;
                    }
                }
                if (data.CheckOutDate == null)
                {
                    if (data.TransactionType.ToLower() == "insert")
                    {
                        result.IsSuccess = false;
                        result.Message = "CheckOutDate is required";
                        result.Data = JsonConvert.SerializeObject(data);
                        return result;
                    }
                }
                if (data.Status == null)
                {
                    if (data.TransactionType.ToLower() == "update")
                    {
                        result.IsSuccess = false;
                        result.Message = "Status is required";
                        result.Data = JsonConvert.SerializeObject(data);
                        return result;
                    }
                }
                #endregion Check Mandatory

                result = _access.SubmitRoomBookingAsync(data).Result;

                return result;
            }
            catch
            {
                throw;
            }
        }
        #endregion Transaction

        #region Upload/Download
        #region MsRoom
        public TransactionResponse UploadRoomFile(UploadViewModel data)
        {
            try
            {
                TransactionResponse response = new TransactionResponse();

                using (Attachment attachment = new Attachment(data.File))
                {
                    #region Check Mandatory
                    if (!attachment.HasContent)
                    {
                        response.IsSuccess = false;
                        response.Message = "File is corrupted.";
                        return response;
                    }

                    if (//attachment.MimeType != "text/csv" && 
                        attachment.MimeType != "application/vnd.ms-excel" &&
                        attachment.MimeType != "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        response.IsSuccess = false;
                        response.Message = "Only .xls and .xlsx file allowed.";
                        return response;
                    }
                    #endregion Check Mandatory

                    var list = excel.ExtractData(attachment);
                    if (list.Count < 3)
                    {
                        response.IsSuccess = false;
                        response.Message = "Excel contains no data";
                        return response;
                    }
                    logger.LogInfo("RoomProcessor", "UploadRoomFile", "Check extracted data", JsonConvert.SerializeObject(list));

                    var upload = UploadRoom(list, data.CreatedBy);
                    var failed = upload.Where(x => x.IsSuccess == false).ToList();
                    response.IsSuccess = failed.Count < upload.Count;
                    response.Message = response.IsSuccess == true ?
                                        string.Format("{0} of {1} data uploaded successfully",
                                            upload.Count - failed.Count, upload.Count
                                        ) :
                                        string.Format("Fail to upload data{0}",
                                            failed.Where(x => x.Message.ToLower().Contains("already exist")).ToList().Count ==
                                            failed.Count ? ". All data already exist." :
                                            ". Please check your file."
                                        );

                    return response;
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion MsRoom

        #region Booking
        public TransactionResponse UploadBookingFile(UploadViewModel data)
        {
            try
            {
                TransactionResponse response = new TransactionResponse();

                using (Attachment attachment = new Attachment(data.File))
                {
                    #region Check Mandatory
                    if (!attachment.HasContent)
                    {
                        response.IsSuccess = false;
                        response.Message = "File is corrupted.";
                        return response;
                    }

                    if (//attachment.MimeType != "text/csv" && 
                        attachment.MimeType != "application/vnd.ms-excel" &&
                        attachment.MimeType != "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        response.IsSuccess = false;
                        response.Message = "Only .xls and .xlsx file allowed.";
                        return response;
                    }
                    #endregion Check Mandatory

                    var list = excel.ExtractData(attachment);
                    if (list.Count < 3)
                    {
                        response.IsSuccess = false;
                        response.Message = "Excel contains no data";
                        return response;
                    }
                    logger.LogInfo("RoomProcessor", "UploadBookingFile", "Check extracted data", JsonConvert.SerializeObject(list));

                    var upload = UploadBooking(list, data.CreatedBy);
                    var failed = upload.Where(x => x.IsSuccess == false).ToList();
                    response.IsSuccess = failed.Count < upload.Count;
                    response.Message = response.IsSuccess == true ?
                                        string.Format("{0} of {1} data uploaded successfully",
                                            upload.Count - failed.Count, upload.Count
                                        ) :
                                        string.Format("Fail to upload data{0}",
                                            failed.Where(x => x.Message.ToLower().Contains("already exist")).ToList().Count ==
                                                failed.Count ? ". All data already exist." :
                                            failed.Where(x => x.Message.ToLower().Contains("already exist")).ToList().Count == 0 ?
                                                string.Format(". {0}.", failed.FirstOrDefault()?.Message) :
                                            ". Please check your file."
                                        );

                    return response;
                }
            }
            catch
            {
                throw;
            }
        }

        public byte[] DownloadReportBooking(ListChoiceWithId data)
        {
            try
            {
                int month = !string.IsNullOrEmpty(data.Text) ? Convert.ToInt32(data.Text) : -1;
                int year = data.Value;
                string filePath = AppConfig.Config.MsRoomTemplate.Download;
                if (!System.IO.File.Exists(filePath))
                    //return NotFound("Template not found");
                    throw new Exception("Template not found");

                var list = _access.GetReportBookingAsync(data).Result;
                if (list.Count < 1)
                {
                    //return NotFound("Data not found");
                    throw new Exception("Data not found");
                    //return new TransactionResponse
                    //{
                    //    IsSuccess = false,
                    //    Message = "Data not found"
                    //});
                }

                byte[] result = excel.DownloadRoomReport(list, filePath, month, year);
                return result;
            }
            catch
            {
                throw;
            }
        }
        #endregion Booking

        #endregion Upload/Download

        #region Private Method
        #region MsRoom
        private List<TransactionResponse> UploadRoom(List<object[]> listData, string uploadedBy)
        {
            try
            {
                List<TransactionResponse> response = new List<TransactionResponse>();

                for (int i = 2; i < listData.Count; i++)
                {
                    var data = listData[i];
                    var model = GetDataRoomFromObject(data, uploadedBy);
                    model.TransactionType = "Insert";
                    response.Add(SubmitMsRoom(model));
                }

                return response;
            }
            catch
            {
                throw;
            }
        }

        private MsRoomRequest GetDataRoomFromObject(object[] data, string uploadedBy)
        {
            try
            {
                double? price = !string.IsNullOrEmpty(data[5].ToString()) ?
                                double.Parse(data[5].ToString()) : null;
                byte? capacity = !string.IsNullOrEmpty(data[4].ToString()) ? 
                                byte.Parse(data[4].ToString()) : null;

                MsRoomRequest result = new MsRoomRequest
                {
                    Code = data[2].ToString(),
                    Name = data[3].ToString(),
                    Capacity = capacity,
                    Price = Convert.ToDecimal((price ?? 0).ToString("F")),
                    CreatedBy = uploadedBy
                };

                return result;
            }
            catch
            {
                throw;
            }
        }
        #endregion MsRoom

        #region Booking
        private List<TransactionResponse> UploadBooking(List<object[]> listData, string uploadedBy)
        {
            try
            {
                List<TransactionResponse> response = new List<TransactionResponse>();

                #region Get User Data
                UserProcessor _user = new UserProcessor();
                var user = _user.GetListUser(new())
                            .Select(x => new
                            {
                                x.Username,
                                Fullname = string.Format("{0}{1}", x.FirstName,
                                            !string.IsNullOrEmpty(x.LastName) ?
                                            string.Concat(" ", x.LastName) : string.Empty
                                ),
                                x.CreatedDtm
                            }).ToList();
                #endregion Get User Data
                for (int i = 2; i < listData.Count; i++)
                {
                    string objectId = Guid.NewGuid().GetObjectID();
                    var data = listData[i];
                    var model = GetDataBookingFromObject(data, objectId, uploadedBy);
                    model.TransactionType = "Insert";
                    model.Username = user.OrderByDescending(x => x.CreatedDtm)
                                         .FirstOrDefault(x => x.Fullname.Trim().ToLower() == model.Username)?.Username;
                    response.Add(SubmitRoomBooking(model));
                }

                return response;
            }
            catch
            {
                throw;
            }
        }

        private BookingRequest GetDataBookingFromObject(object[] data, string objectId, string uploadedBy)
        {
            try
            {
                DateTime? checkIn = !string.IsNullOrEmpty(data[5].ToString()) ?
                                Convert.ToDateTime(data[4].ToString()) : null;
                DateTime? checkOut = !string.IsNullOrEmpty(data[5].ToString()) ?
                                Convert.ToDateTime(data[5].ToString()) : null;
                byte? duration = checkIn != null && checkOut != null ?
                                byte.Parse(checkOut.Value.Date.Subtract(checkIn.Value.Date).Days.ToString()) : null;

                BookingRequest result = new BookingRequest
                {
                    ObjectID = objectId,
                    Code = data[2].ToString().Trim().ToUpper(),
                    Username = data[3].ToString().Trim().ToLower(), //fullname
                    Duration = duration,
                    CheckInDate = checkIn,
                    CheckOutDate = checkOut,
                    CreatedBy = uploadedBy
                };

                return result;
            }
            catch
            {
                throw;
            }
        }
        #endregion Booking

        #endregion Private Method

    }
}
