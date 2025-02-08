using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
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
        // GET: api/<UserController>
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
                logger.WriteErrorToLog(ex, "Room", "GetList");
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }
        #endregion View

        #region Transaction
        /// <summary>
        /// This API for insert/update/delete product.
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
                logger.WriteErrorToLog(ex, "Room", "Submit");
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }
        /// <summary>
        /// This API for insert/update/delete product.
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
                logger.WriteErrorToLog(ex, "Room", "UploadRoom");
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
                logger.WriteErrorToLog(ex, "Room", "SubmitRoomBooking");
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }
        #endregion Transaction

    }
}
