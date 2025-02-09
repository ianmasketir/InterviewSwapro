using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PORECT.Helper;
//using Tes.BusinessRules;
using Tes.Data.Models;
using Tes.Domain;

namespace Tes.Data
{
    public class TesDataAccessControl
    {
        private DateTime Timestamp { get; set; } = DateTime.Now;
        //private readonly CompareValues compareValues = new CompareValues();

        #region View
        public async Task<List<MsUserResponse>> GetListUserAsync(SearchUserRequest dto)
        {
            using (var context = new DataContext())
            {
                if (context == null || context.MsUsers == null || 
                    context.MsRoles == null || context.UserRoles == null)
                    throw new ArgumentNullException("Db context not found");

                try
                {
                    var result = await (from u in context.MsUsers
                                            //join _ur in context.UserRoles on u.Username equals _ur.Username
                                            //  into TblUr
                                            //from ur in TblUr.DefaultIfEmpty()
                                        where (dto.ID == null || u.Id == dto.ID) &&
                                              (string.IsNullOrEmpty(dto.Username) || u.Username.ToLower() == dto.Username.ToLower().Trim())// &&
                                              //(string.IsNullOrEmpty(dto.Role) || ur.Role.ToLower() == dto.Role.ToLower().Trim())
                                        select new MsUserResponse
                                        {
                                            ID = u.Id,
                                            Username = u.Username,
                                            Password = u.Password.Decrypt(),
                                            FirstName = u.FirstName,
                                            LastName = u.LastName,
                                            Roles = context.UserRoles.Where(x => x.Username.ToLower() == u.Username.ToLower() && x.IsActive == true)
                                                           .Select(x => x.Role).ToList(),
                                            IsActive = u.IsActive ?? true,
                                            CreatedBy = u.CreatedBy,
                                            CreatedDtm = u.CreatedDtm,
                                            UpdatedBy = u.UpdatedBy,
                                            UpdatedDtm = u.UpdatedDtm
                                        }).ToListAsync();

                    if (!string.IsNullOrEmpty(dto.Role))
                    {
                        result = result.Where(x => x.Roles.Count > 0 && 
                                    x.Roles.Any(y => y.ToLower() == dto.Role.ToLower().Trim())).ToList();
                    }
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        public async Task<List<BookingResponse>> GetListBookingAsync(SearchRoomRequest dto)
        {
            using (var context = new DataContext())
            {
                if (context == null || context.Bookings == null)
                    throw new ArgumentNullException("Db context not found");

                try
                {
                    var result = await (from b in context.Bookings
                                        where (string.IsNullOrEmpty(dto.Code) || b.RoomCode.ToLower() == dto.Code.ToLower().Trim()) &&
                                              (string.IsNullOrEmpty(dto.Name) || b.Username.ToLower() == dto.Name.ToLower().Trim())
                                        select new BookingResponse
                                        {
                                            ObjectID = b.ObjectId,
                                            Code = b.RoomCode,
                                            Username = b.Username,
                                            Duration = b.Duration,
                                            CheckInDate = b.CheckInDate,
                                            CheckOutDate = b.CheckOutDate,
                                            Status = b.Status,
                                            CreatedBy = b.CreatedBy,
                                            CreatedDtm = b.CreatedDtm,
                                            UpdatedBy = b.UpdatedBy,
                                            UpdatedDtm = b.UpdatedDtm
                                        }).ToListAsync();
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        public async Task<List<MsRoomResponse>> GetListRoomAsync(SearchRoomRequest dto)
        {
            using (var context = new DataContext())
            {
                if (context == null || context.MsRooms == null || context.Bookings == null)
                    throw new ArgumentNullException("Db context not found");

                try
                {
                    //var booking = await context.Bookings.GroupBy(x => x.RoomCode)
                    //                    .Select(y => y.OrderByDescending(x => x.CheckOutDate).FirstOrDefault())
                    //                    .Select(x => new
                    //                    {
                    //                        Code = x.RoomCode,
                    //                        CheckOutDate = x.CheckOutDate,
                    //                        Status = x.Status
                    //                    }).ToListAsync();
                    var booking = (from b in context.Bookings
                                    join l in context.Bookings.GroupBy(x => x.RoomCode)
                                            .Select(x => new
                                            {
                                                RoomCode = x.Key,
                                                LatestCheckOutDate = x.Max(y => y.CheckOutDate)
                                            }) on b.RoomCode equals l.RoomCode
                                    where b.CheckOutDate == l.LatestCheckOutDate
                                    select new BookingResponse
                                    {
                                        Code = b.RoomCode,
                                        CheckOutDate = b.CheckOutDate,
                                        Status = b.Status
                                    });//.ToListAsync();
                    //var a = booking.ToList();
                    //var b0 = (from r in context.MsRooms
                    //         join _b in context.Bookings on r.Code equals _b.RoomCode
                    //         select r).ToList();

                     var result = booking.Any() ? 
                                 await (from r in context.MsRooms
                                        join _b in booking on r.Code equals _b.Code
                                          into TblB
                                        from b in TblB.DefaultIfEmpty()
                                        where (dto.ID == null || r.Id == dto.ID) &&
                                              (string.IsNullOrEmpty(dto.Code) || r.Code.ToLower() == dto.Code.ToLower().Trim()) &&
                                              (string.IsNullOrEmpty(dto.Name) || r.Name.ToLower().Contains(dto.Name.ToLower().Trim())) &&
                                              (dto.Capacity == null || r.Capacity == dto.Capacity) &&
                                              (string.IsNullOrEmpty(dto.BookingStatus) ||
                                              (b.Status != null && b.Status.ToLower() == dto.BookingStatus.ToLower().Trim())) &&

                                              ((dto.PriceFrom == null && dto.PriceTo == null) ||
                                               (dto.PriceFrom != null && dto.PriceTo == null && dto.PriceFrom <= r.Price) ||
                                               (dto.PriceFrom == null && dto.PriceTo != null && r.Price <= dto.PriceTo) ||
                                               (dto.PriceFrom != null && dto.PriceTo != null &&
                                                dto.PriceFrom <= r.Price && r.Price <= dto.PriceTo)
                                              ) &&
                                              ((dto.AvailableFrom == null && dto.AvailableTo == null) ||
                                               //(b != null && compareValues.Compare(b.CheckOutDate.Value, dto.AvailableFrom, dto.AvailableTo, "<=", ">=")
                                               //                                 == true))
                                               (dto.AvailableFrom != null && dto.AvailableTo == null &&
                                                (b.Code == null || 
                                                (b.CheckOutDate != null && dto.AvailableFrom.Value.Date <= b.CheckOutDate.Value.Date))
                                               ) ||
                                               (dto.AvailableFrom == null && dto.AvailableTo != null &&
                                                (b.Code == null || 
                                                (b.CheckOutDate != null && b.CheckOutDate.Value.Date <= dto.AvailableTo.Value.Date))
                                               ) ||
                                               (dto.AvailableFrom != null && dto.AvailableTo != null &&
                                                (b.Code == null || (b.CheckOutDate != null &&
                                                 dto.AvailableFrom.Value.Date <= b.CheckOutDate.Value.Date &&
                                                 b.CheckOutDate.Value.Date <= dto.AvailableTo.Value.Date))
                                              ))
                                        select new MsRoomResponse
                                        {
                                            ID = r.Id,
                                            Code = r.Code,
                                            Name = r.Name,
                                            Capacity = r.Capacity,
                                            Price = r.Price,
                                            BookingStatus = b.Code == null ? null : b.Status,
                                            //Booking = b,
                                            IsAvailable = b.Code == null || b.Status == RoomStatusEnum.CheckedOut.ToString(),
                                            AvailableDate = b.Code != null ? 
                                                Timestamp.Date < b.CheckOutDate.Value.Date && b.Status == RoomStatusEnum.CheckedOut.ToString() ? 
                                                    Timestamp.Date : b.CheckOutDate :
                                                dto.AvailableFrom ?? (new DateTime(2000, 1, 1)).Date,
                                            IsActive = r.IsActive ?? true,
                                            CreatedBy = r.CreatedBy,
                                            CreatedDtm = r.CreatedDtm,
                                            UpdatedBy = r.UpdatedBy,
                                            UpdatedDtm = r.UpdatedDtm
                                        }).Distinct().ToListAsync() :
                                 await (from r in context.MsRooms
                                        where (dto.ID == null || r.Id == dto.ID) &&
                                              (string.IsNullOrEmpty(dto.Code) || r.Code.ToLower() == dto.Code.ToLower().Trim()) &&
                                              (string.IsNullOrEmpty(dto.Name) || r.Name.ToLower().Contains(dto.Name.ToLower().Trim())) &&
                                              (dto.Capacity == null || r.Capacity == dto.Capacity) &&

                                              ((dto.PriceFrom == null && dto.PriceTo == null) ||
                                               (dto.PriceFrom != null && dto.PriceTo == null && dto.PriceFrom <= r.Price) ||
                                               (dto.PriceFrom == null && dto.PriceTo != null && r.Price <= dto.PriceTo) ||
                                               (dto.PriceFrom != null && dto.PriceTo != null &&
                                                dto.PriceFrom <= r.Price && r.Price <= dto.PriceTo)
                                              )
                                        select new MsRoomResponse
                                        {
                                            ID = r.Id,
                                            Code = r.Code,
                                            Name = r.Name,
                                            Capacity = r.Capacity,
                                            Price = r.Price,
                                            //Booking = new(),
                                            IsAvailable = true,
                                            AvailableDate = dto.AvailableFrom ?? (new DateTime(2000, 1, 1)).Date,
                                            IsActive = r.IsActive ?? true,
                                            CreatedBy = r.CreatedBy,
                                            CreatedDtm = r.CreatedDtm,
                                            UpdatedBy = r.UpdatedBy,
                                            UpdatedDtm = r.UpdatedDtm
                                        }).ToListAsync();

                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        public async Task<List<PORECT.Helper.ReportBookingResponse>> GetReportBookingAsync(ListChoiceWithId dto)
        {
            using (var context = new DataContext())
            {
                if (context == null || context.MsRooms == null || context.Bookings == null)
                    throw new ArgumentNullException("Db context not found");

                try
                {
                    var result = new List<PORECT.Helper.ReportBookingResponse>();

                    int? month = string.IsNullOrEmpty(dto.Text) ? null :
                                    Convert.ToInt32(dto.Text);
                    //var qry = await context.MsRooms.Where(x => x.IsActive == true).ToListAsync();
                    var qry = await (from r in context.MsRooms
                                     join _b in context.Bookings on r.Code equals _b.RoomCode
                                             into TblB
                                     from b in TblB.DefaultIfEmpty()
                                     where r.IsActive == true &&
                                           (month == null || b.CheckInDate.Month == month) &&
                                           b.CheckInDate.Year == dto.Value
                                     select new
                                     {
                                         r.Code,
                                         r.Name,
                                         r.Price,
                                         b
                                     }).ToListAsync();
                    for (int i = 0; i < qry.Count; i++)
                    {
                        if (!result.Any(x => x.RoomCode == qry[i].Code))
                        {
                            var data = new PORECT.Helper.ReportBookingResponse
                            {
                                No = i + 1,
                                RoomCode = qry[i].Code,
                                RoomName = !string.IsNullOrEmpty(qry[i].Name) ? qry[i].Name : qry[i].Code,
                                BookedQty = qry.Where(x => x.b?.RoomCode != null && x.b?.RoomCode == qry[i].Code).ToList().Count,
                                TotalDuration = qry.Where(x => x.b?.RoomCode != null && x.b?.RoomCode == qry[i].Code).ToList().Sum(x => x.b.Duration),
                                TotalPrice = qry.Where(x => x.b?.RoomCode != null && x.b?.RoomCode == qry[i].Code).ToList().Sum(x => x.Price),
                            };
                            result.Add(data);
                        }
                    }

                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion View

        #region Transaction
        public async Task<TransactionResponse> SubmitUserAsync(MsUserRequest data)
        {
            using (var context = new DataContext())
            {
                if (context == null || context.MsUsers == null || context.UserRoles == null)
                    throw new Exception("Db context not found");

                var transaction = context.Database.BeginTransaction();
                try
                {
                    TransactionResponse result = new TransactionResponse();

                    if (data.TransactionType.ToLower() == "insert")
                    {
                        #region Check Existing
                        var qry = await context.MsUsers.FirstOrDefaultAsync(x => x.Username.ToLower().Trim() == data.Username.ToLower().Trim());
                        if (qry != null)
                        {
                            await transaction.RollbackAsync();
                            result.IsSuccess = false;
                            result.Message = "User already exist";
                            result.Data = JsonConvert.SerializeObject(data);
                            return result;
                        }
                        #endregion Check Existing

                        #region MsUser
                        MsUser dto = new MsUser
                        {
                            Username = data.Username.Trim().Substring(0,
                                        data.Username.Trim().Length < 50 ? data.Username.Trim().Length : 50),
                            Password = data.Password.Encrypt(),
                            FirstName = data.FirstName.Trim().Substring(0,
                                        data.FirstName.Trim().Length < 100 ? data.FirstName.Trim().Length : 100),
                            LastName = data.LastName?.Trim().Substring(0,
                                        data.LastName.Trim().Length < 100 ? data.LastName.Trim().Length : 100),
                            CreatedBy = string.IsNullOrEmpty(data.CreatedBy) ? "System" : data.CreatedBy
                        };
                        context.Add(dto);
                        await context.SaveChangesAsync();
                        #endregion MsUser

                        #region Role
                        if (data.Roles != null)
                        {
                            foreach (var item in data.Roles)
                            {
                                var qryRole = await context.MsRoles.FirstOrDefaultAsync(x => x.Role.ToLower() == item.value.ToLower().Trim());
                                if (qryRole == null)
                                {
                                    await transaction.RollbackAsync();
                                    result.IsSuccess = false;
                                    result.Message = string.Format("Role {0} not exist", item);
                                    result.Data = JsonConvert.SerializeObject(data);
                                    return result;
                                }

                                UserRole dtoRole = new UserRole
                                {
                                    Username = data.Username.Trim(),
                                    Role = item.value.Trim(),
                                    CreatedBy = string.IsNullOrEmpty(data.CreatedBy) ? "System" : data.CreatedBy,
                                };
                                context.Add(dtoRole);
                                await context.SaveChangesAsync();
                            }
                        }
                        #endregion Role
                    }
                    else
                    {
                        #region Get Data
                        var qry = await context.MsUsers.FirstOrDefaultAsync(x => x.Id == data.ID);
                        if (qry == null)
                        {
                            await transaction.RollbackAsync();
                            result.IsSuccess = false;
                            result.Message = "Data not found";
                            result.Data = JsonConvert.SerializeObject(data);
                            return result;
                        }
                        #endregion Get Data

                        #region MsUser
                        if (data.TransactionType.ToLower() == "update")
                        {
                            #region Check Existing
                            if (qry.Username.ToLower().Trim() != data.Username.ToLower().Trim())
                            {
                                var check = await context.MsUsers.FirstOrDefaultAsync(x => x.Username.ToLower().Trim() == data.Username.ToLower().Trim());
                                if (check != null)
                                {
                                    await transaction.RollbackAsync();
                                    result.IsSuccess = false;
                                    result.Message = "User already exist";
                                    result.Data = JsonConvert.SerializeObject(data);
                                    return result;
                                }
                            }
                            #endregion Check Existing

                            qry.Password = !string.IsNullOrEmpty(data.Password?.Trim()) ? data.Password.Trim().Encrypt() :
                                                qry.Password;
                            qry.FirstName = !string.IsNullOrEmpty(data.FirstName?.Trim()) ? 
                                                data.FirstName.Trim().Substring(0,
                                                    data.FirstName.Trim().Length < 100 ? data.FirstName.Trim().Length : 100) :
                                                qry.FirstName;
                            qry.LastName = data.LastName?.Trim().Substring(0,
                                            data.LastName.Trim().Length < 100 ? data.LastName.Trim().Length : 100);
                        }
                        else
                            qry.IsActive = data.TransactionType.ToLower() == "enable";

                        qry.UpdatedBy = data.CreatedBy;
                        qry.UpdatedDtm = Timestamp;
                        context.Entry(qry).State = EntityState.Detached;
                        context.Update(qry);
                        await context.SaveChangesAsync();
                        #endregion MsUser

                        #region Role
                        if (data.Roles != null)
                        {
                            foreach (var item in data.Roles)
                            {
                                if (item.colName.ToLower() != "add" && item.colName.ToLower() != "disable" && item.colName.ToLower() != "enable")
                                {
                                    await transaction.RollbackAsync();
                                    result.IsSuccess = false;
                                    result.Message = "Invalid role operation";
                                    result.Data = JsonConvert.SerializeObject(data);
                                    return result;
                                }

                                var qryRole = await context.MsRoles.FirstOrDefaultAsync(x => x.Role.ToLower() == item.value.ToLower().Trim());
                                if (qryRole == null)
                                {
                                    await transaction.RollbackAsync();
                                    result.IsSuccess = false;
                                    result.Message = string.Format("Role {0} not exist", item.value);
                                    result.Data = JsonConvert.SerializeObject(data);
                                    return result;
                                }

                                var qryUserRole = await context.UserRoles.Where(x => x.Username.ToLower() == data.Username.Trim()).ToListAsync();
                                if (item.colName.ToLower() == "add")
                                {
                                    if (!qryUserRole.Any(x => x.Role.ToLower() == item.value.ToLower()))
                                    {
                                        UserRole dtoRole = new UserRole
                                        {
                                            Username = data.Username.Trim(),
                                            Role = qryRole.Role.Trim(),
                                            CreatedBy = string.IsNullOrEmpty(data.CreatedBy) ? "System" : data.CreatedBy,
                                        };
                                        context.Add(dtoRole);
                                        await context.SaveChangesAsync();
                                    }
                                }
                                else
                                {
                                    if (!qryUserRole.Any(x => x.Role.ToLower() == item.value.ToLower()) && item.colName.ToLower() == "enable")
                                    {
                                        UserRole dtoRole = new UserRole
                                        {
                                            Username = data.Username.Trim(),
                                            Role = qryRole.Role.Trim(),
                                            CreatedBy = string.IsNullOrEmpty(data.CreatedBy) ? "System" : data.CreatedBy,
                                        };
                                        context.Add(dtoRole);
                                    }
                                    else if (qryUserRole.Any(x => x.Role.ToLower() == item.value.ToLower()))
                                    {
                                        var dtoRole = qryUserRole.OrderByDescending(x => x.CreatedDtm).FirstOrDefault(x => x.Role == qryRole.Role);
                                        dtoRole.IsActive = item.colName.ToLower() == "enable";
                                        dtoRole.UpdatedBy = data.CreatedBy;
                                        dtoRole.UpdatedDtm = Timestamp;
                                        context.Entry(dtoRole).State = EntityState.Detached;
                                        context.Update(dtoRole);
                                    }
                                    await context.SaveChangesAsync();
                                }
                            }
                        }
                        #endregion Role
                    }

                    bool ready = true;
                    if (ready)
                    {
                        await transaction.CommitAsync();
                        result.Message = string.Format("User {0} {1} successfully.", 
                                            string.Format("{0}{1}", data.FirstName,
                                                !string.IsNullOrEmpty(data.LastName) ? 
                                                    string.Concat(" ", data.LastName) :
                                                    string.Empty
                                            ),
                                            data.TransactionType.ToLower() == "disable" ? "disabled" :
                                            data.TransactionType.ToLower() == "enable" ? "enabled" : "submitted");
                        //result.Data = JsonConvert.SerializeObject(data);
                    }
                    else
                    {
                        await transaction.RollbackAsync();
                        result.IsSuccess = false;
                        result.Message = string.Format("Fail to {0} user {1}.",
                                            data.TransactionType.ToLower() == "disable" ? "disable" :
                                            data.TransactionType.ToLower() == "enable" ? "enable" : "submit",
                                            string.Format("{0}{1}", data.FirstName,
                                                !string.IsNullOrEmpty(data.LastName) ?
                                                    string.Concat(" ", data.LastName) :
                                                    string.Empty
                                            ));
                        result.Data = JsonConvert.SerializeObject(new { Data = data });
                    }

                    return result;
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }
        public async Task<TransactionResponse> SubmitRoomAsync(MsRoomRequest data)
        {
            using (var context = new DataContext())
            {
                if (context == null || context.MsRooms == null)
                    throw new Exception("Db context not found");

                var transaction = context.Database.BeginTransaction();
                try
                {
                    TransactionResponse result = new TransactionResponse();

                    if (data.TransactionType.ToLower() == "insert")
                    {
                        #region Check Existing
                        var qry = await context.MsRooms.FirstOrDefaultAsync(x =>
                                    x.Name.ToLower().Trim() == data.Name.ToLower().Trim() &&
                                    x.Code.ToLower().Trim() == data.Code.ToLower().Trim()
                                  );
                        if (qry != null)
                        {
                            await transaction.RollbackAsync();
                            result.IsSuccess = false;
                            result.Message = string.Format("Room {0} with code {1} already exist", data.Name, data.Code);
                            result.Data = JsonConvert.SerializeObject(data);
                            return result;
                        }
                        #endregion Check Existing

                        MsRoom dto = new MsRoom
                        {
                            Code = data.Code.Trim().Substring(0,
                                    data.Code.Trim().Length < 5 ? data.Code.Trim().Length : 5).ToUpper(),
                            Name = data.Name.Trim().Substring(0,
                                    data.Name.Trim().Length < 100 ? data.Name.Trim().Length : 100),
                            Capacity = data.Capacity ?? 0,
                            Price = data.Price ?? 0,
                            CreatedBy = !string.IsNullOrEmpty(data.CreatedBy) ? data.CreatedBy : "System"//,
                            //CreatedDtm = data.CreatedDtm ?? Timestamp
                        };
                        context.Add(dto);
                    }
                    else
                    {
                        #region Get Data
                        var qry = await context.MsRooms.OrderByDescending(x => x.CreatedDtm)
                                        .FirstOrDefaultAsync(x => x.Code.ToUpper() == data.Code.ToUpper());
                        if (qry == null)
                        {
                            await transaction.RollbackAsync();
                            result.IsSuccess = false;
                            result.Message = "Data not found";
                            result.Data = JsonConvert.SerializeObject(data);
                            return result;
                        }
                        #endregion Get Data

                        if (data.TransactionType.ToLower() == "update")
                        {
                            qry.Name = !string.IsNullOrEmpty(data.Name?.Trim()) ? 
                                            data.Name.Trim().Substring(0,
                                                data.Name.Trim().Length < 100 ? data.Name.Trim().Length : 100) :
                                            qry.Name;
                            qry.Capacity = data.Capacity ?? qry.Capacity;
                            qry.Price = data.Price ?? qry.Price;
                        }
                        else
                            qry.IsActive = data.TransactionType.ToLower() == "enable";

                        qry.UpdatedBy = data.CreatedBy;
                        qry.UpdatedDtm = Timestamp;
                        context.Entry(qry).State = EntityState.Detached;
                        context.Update(qry);
                    }
                    await context.SaveChangesAsync();

                    bool ready = true;
                    if (ready)
                    {
                        await transaction.CommitAsync();
                        result.Message = string.Format("Room {0} {1} successfully.", data.Name,
                                            data.TransactionType.ToLower() == "insert" ||
                                            data.TransactionType.ToLower() == "update" ? "submitted" :
                                            string.Concat(data.TransactionType.ToLower(), "d"));
                        //result.Data = JsonConvert.SerializeObject(data);
                    }
                    else
                    {
                        await transaction.RollbackAsync();
                        result.IsSuccess = false;
                        result.Message = string.Format("Fail to {0} room {1}.",
                                            data.TransactionType.ToLower() == "insert" ||
                                            data.TransactionType.ToLower() == "update" ? "submit" : data.TransactionType.ToLower(),
                                            data.Name);
                        result.Data = JsonConvert.SerializeObject(data);
                    }

                    return result;
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }
        public async Task<TransactionResponse> SubmitRoomBookingAsync(BookingRequest data)
        {
            using (var context = new DataContext())
            {
                if (context == null || context.MsUsers == null ||
                    context.MsRooms == null || context.Bookings == null)
                    throw new Exception("Db context not found");

                var transaction = context.Database.BeginTransaction();
                try
                {
                    TransactionResponse result = new TransactionResponse();

                    var qryUser = await context.MsUsers.FirstOrDefaultAsync(x => x.Username.ToLower() == data.Username.ToLower());
                    if(qryUser == null)
                    {
                        await transaction.RollbackAsync();
                        result.IsSuccess = false;
                        result.Message = string.Format("User {0} not registered", data.Username);
                        result.Data = JsonConvert.SerializeObject(data);
                        return result;
                    }
                    var qryRoom = await context.MsRooms.FirstOrDefaultAsync(x => x.Code.ToUpper() == data.Code);
                    if(qryRoom == null)
                    {
                        await transaction.RollbackAsync();
                        result.IsSuccess = false;
                        result.Message = string.Format("Room {0} not exist", data.Code);
                        result.Data = JsonConvert.SerializeObject(data);
                        return result;
                    }

                    if (data.TransactionType.ToLower() == "insert")
                    {
                        #region Check Existing
                        var qry = await context.Bookings.FirstOrDefaultAsync(x =>
                                    x.Username.ToLower().Trim() == data.Username.ToLower().Trim() &&
                                    x.RoomCode.ToLower().Trim() == data.Code.ToLower().Trim()
                                  );
                        if (qry != null)
                        {
                            await transaction.RollbackAsync();
                            result.IsSuccess = false;
                            result.Message = string.Format("Room booking {0} from {1} already already exist", data.Code, data.Username);
                            result.Data = JsonConvert.SerializeObject(data);
                            return result;
                        }
                        #endregion Check Existing

                        #region Check Availability
                        var booking = await (from b in context.Bookings
                                             join l in context.Bookings.GroupBy(x => x.RoomCode)
                                                   .Select(x => new
                                                   {
                                                       RoomCode = x.Key,
                                                       LatestCheckOutDate = x.Max(y => y.CheckOutDate)
                                                   }) on b.RoomCode equals l.RoomCode
                                             where b.CheckOutDate == l.LatestCheckOutDate
                                             select new BookingResponse
                                             {
                                                 Code = b.RoomCode,
                                                 CheckOutDate = b.CheckOutDate,
                                                 Status = b.Status
                                             }).ToListAsync();
                        var check = booking.FirstOrDefault(x => x.Code.ToUpper() == data.Code.ToUpper().Trim());
                        if(check != null && check.Status != RoomStatusEnum.CheckedOut.ToString())
                        {
                            await transaction.RollbackAsync();
                            result.IsSuccess = false;
                            result.Message = string.Format("Room booking for {0} already exist", data.Code);
                            result.Data = JsonConvert.SerializeObject(data);
                            return result;
                        }
                        #endregion Check Availability

                        Booking dto = new Booking
                        {
                            ObjectId = data.ObjectID,
                            RoomCode = data.Code.Trim().Substring(0,
                                    data.Code.Trim().Length < 5 ? data.Code.Trim().Length : 5).ToUpper(),
                            Username = data.Username.Trim().Substring(0,
                                    data.Username.Trim().Length < 50 ? data.Username.Trim().Length : 50),
                            Duration = data.Duration ?? 0,
                            CheckInDate = data.CheckInDate ?? DateTime.MinValue,
                            CheckOutDate = data.CheckOutDate ?? DateTime.MaxValue,
                            Status = RoomStatusEnum.Booked.ToString(),
                            CreatedBy = !string.IsNullOrEmpty(data.CreatedBy) ? data.CreatedBy : "System"//,
                            //CreatedDtm = data.CreatedDtm ?? Timestamp
                        };
                        context.Add(dto);
                    }
                    else
                    {
                        #region Get Data
                        var qry = await context.Bookings.OrderByDescending(x => x.CreatedDtm)
                                        .Where(x => 
                                            //x.ObjectId.ToUpper() == data.ObjectID.ToUpper() &&
                                            x.RoomCode.ToUpper() == data.Code.ToUpper()
                                        ).ToListAsync();
                        if (qry.Count == 0)
                        {
                            await transaction.RollbackAsync();
                            result.IsSuccess = false;
                            result.Message = "Data not found";
                            result.Data = JsonConvert.SerializeObject(data);
                            return result;
                        }
                        #endregion Get Data

                        if (data.TransactionType.ToLower() == "update")
                        {
                            foreach (var item in qry)
                            {
                                item.Status = !string.IsNullOrEmpty(data.Status) ?
                                                data.Status.ToLower() == RoomStatusEnum.Booked.ToString().ToLower() ?
                                                    RoomStatusEnum.Booked.ToString() :
                                                data.Status.ToLower() == RoomStatusEnum.CheckedIn.ToString().ToLower() ?
                                                    RoomStatusEnum.CheckedIn.ToString() : RoomStatusEnum.CheckedOut.ToString() :
                                             item.Status;

                                item.UpdatedBy = data.CreatedBy;
                                item.UpdatedDtm = Timestamp;
                                context.Entry(item).State = EntityState.Detached;
                                context.Update(item);
                                await context.SaveChangesAsync();
                            }
                        }
                        else
                        {
                            var dto = qry.FirstOrDefault(x => x.Id == data.ID);
                            if (dto == null)
                            {
                                await transaction.RollbackAsync();
                                result.IsSuccess = false;
                                result.Message = "Data not found";
                                result.Data = JsonConvert.SerializeObject(data);
                                return result;
                            }
                            
                            context.Remove(dto);
                            await context.SaveChangesAsync();
                        }
                    }

                    bool ready = true;
                    if (ready)
                    {
                        await transaction.CommitAsync();
                        result.Message = string.Format("Room Booking {0} {1} successfully.", qryRoom.Name,
                                            data.TransactionType.ToLower() == "insert" ||
                                            data.TransactionType.ToLower() == "update" ? "submitted" :
                                            string.Concat(data.TransactionType.ToLower(), "d"));
                        //result.Data = JsonConvert.SerializeObject(data);
                    }
                    else
                    {
                        await transaction.RollbackAsync();
                        result.IsSuccess = false;
                        result.Message = string.Format("Fail to {0} room booking {1}.",
                                            data.TransactionType.ToLower() == "insert" ||
                                            data.TransactionType.ToLower() == "update" ? "submit" : data.TransactionType.ToLower(),
                                            qryRoom.Name);
                        result.Data = JsonConvert.SerializeObject(data);
                    }

                    return result;
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }
        #endregion Transaction

    }
}
