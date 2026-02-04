using Elderly_System.BLL.Service.Interface;
using Elderly_System.DAL.DTO.Request.Room;
using Elderly_System.DAL.Repositories.Interfaces;
using ElderlySystem.BLL.Helpers;
using ElderlySystem.DAL.Model;
using EderlySystem.DAL.Enums;

namespace Elderly_System.BLL.Service.Classes
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _repository;
        private readonly IFileService _file;

        public RoomService(IRoomRepository Repository, IFileService file)
        {
            _repository = Repository;
            _file = file;
        }
        public async Task<ServiceResult> AddRoomAsync(RoomCreateRequest request)
        {
            var checkRoomNumber = await _repository.CheckRoomNumberAsync(request.RoomNumber);
            if (checkRoomNumber)
                return ServiceResult.Failure("رقم الغرفة مستخدم");
            var room = new Room
            {
                RoomNumber = request.RoomNumber,
                RoomType = request.RoomType,
                Capacity = request.Capacity,
                Price = request.Price,
                Description = request.Description,
                Status = Status.Active
            };
            if (request.Images != null && request.Images.Count > 0) 
            {
                var uploadedImages = await _file.UploadMultipleAsync(request.Images, "rooms");
                room.RoomImages = uploadedImages.Select(x => new RoomImage
                {
                    Url = x.Url,
                    PublicId = x.PublicId,
                }).ToList();
            }
            await _repository.AddRoomAsync(room);
            return ServiceResult.SuccessMessage("تم إضافة الغرفة بنجاح.");
        }
    }
}
