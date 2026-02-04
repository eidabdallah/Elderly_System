using Elderly_System.BLL.Service.Interface;
using Elderly_System.DAL.DTO.Request.Room;
using Elderly_System.DAL.DTO.Response.Room;
using Elderly_System.DAL.Enums;
using Elderly_System.DAL.Repositories.Interfaces;
using ElderlySystem.BLL.Helpers;
using ElderlySystem.DAL.Model;
using Mapster;

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
        public async Task<ServiceResult> GetAllRoomAsync()
        {
            var result = await _repository.GetAllRoomAsync();
            var rooms = result.Adapt<List<RoomResponse>>();
            return ServiceResult.SuccessWithData(rooms, "تم جلب جميع الغرف");
        }
        public async Task<ServiceResult> GetRoomByIdAsync(int id)
        {
            var result = await _repository.GetRoomByIdWithImagesAsync(id);
            if (result is null)
                return ServiceResult.Failure("الغرفة غير متوفرة.");
            var roomDto = result.Adapt<RoomDetailsResponse>();
            roomDto.Images = result.RoomImages
                .Select(ri => ri.Url)
                .ToList();
            return ServiceResult.SuccessWithData(roomDto, "تم جلب الغرف بنجاح");
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
        public async Task<ServiceResult> DeleteRoomAsync(int id)
        {
            var room = await _repository.GetRoomByIdWithImagesAsync(id);
            if (room is null)
                return ServiceResult.Failure("الغرفة غير متوفرة.");
            if (room.CurrentCapacity > 0)
                return ServiceResult.Failure("لا يمكن حذف الغرفة لانها تحتوي على مقيمين حاليا");
            foreach (var img in room.RoomImages.ToList())
            {
                await _file.DeleteAsync(img.PublicId);
            }
            var deleted = await _repository.DeleteRoomAsync(room);
            if (!deleted)
                return ServiceResult.Failure("حدث خطأ أثناء حذف الغرفة.");
            return ServiceResult.SuccessMessage("تم حذف الغرفة بنجاح.");

        }
        public async Task<ServiceResult> ToggleRoomStatusAsync(int roomId)
        {
            var room = await _repository.GetRoomByIdAsync(roomId);
            if (room is null)
                return ServiceResult.Failure("الغرفة غير متوفرة.");

            if (room.Status == Status.Active)
            {
                if (room.CurrentCapacity > 0)
                    return ServiceResult.Failure("لا يمكن إيقاف الغرفة لأنها تحتوي على مقيمين حالياً.");
            }
            room.Status = room.Status == Status.Active
                ? Status.InActive
                : Status.Active;
            await _repository.UpdateRoomAsync(room);
            return ServiceResult.SuccessMessage("تم تغيير حالة الغرفة بنجاح.");
        }

    }
}
