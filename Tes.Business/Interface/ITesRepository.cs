using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tes.Domain;

namespace Tes.Business
{
    public interface ITesRepository
    {
        #region View
        public List<MsUserResponse> GetListUser();
        public List<MsRoomResponse> GetListRoom(SearchRoomRequest dto);
        #endregion View

        #region Transaction
        public TransactionResponse SubmitUser(MsUserRequest data);
        public TransactionResponse SubmitRoom(MsRoomRequest data);
        #endregion Transaction
    }
    public interface IUserRepository
    {
        #region View
        public List<MsUserResponse> GetListUser(SearchUserRequest data);
        #endregion View

        #region Transaction
        public TransactionResponse SubmitUser(MsUserRequest data);
        #endregion Transaction
    }
    public interface IRoomRepository
    {
        #region View
        public List<MsRoomResponse> GetListRoom(SearchRoomRequest dto);
        #endregion View

        #region Transaction
        public TransactionResponse SubmitMsRoom(MsRoomRequest data);
        public TransactionResponse SubmitRoomBooking(BookingRequest data);
        #endregion Transaction

        #region Upload/Download
        /// <summary>
        /// Upload Room data with provided template
        /// </summary>
        /// <param name="data">File</param>
        /// <returns>Model class</returns>
        public TransactionResponse UploadRoomFile(UploadViewModel data);
        #endregion Upload/Download

    }
}
