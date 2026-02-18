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
                .Select(ri => new RoomImageResponse
                {
                    Url = ri.Url,
                    PublicId = ri.PublicId
                })
                .ToList();

                roomDto.CurrentElderlies = result.ResidentStays
                .Where(rs => rs.Status == Status.Active)
                .Select(rs => new RoomElderlyResponse
                {
                    Id = rs.Elderly.Id,
                    Name = rs.Elderly.Name,
                })
                .ToList();

            return ServiceResult.SuccessWithData(roomDto, "تم جلب الغرفة بنجاح");
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
        public async Task<ServiceResult> UpdateRoomAsync(UpdateRoomRequest request, int id)
        {
            var room = await _repository.GetRoomByIdAsync(id);
            if (room is null)
                return ServiceResult.Failure("الغرفة غير متوفره ");

            if (request.Price is not null)
                room.Price = request.Price.Value;
            if (request.RoomType is not null)
                room.RoomType = request.RoomType.Value;
            if (request.Capacity is not null)
            {
                if (request.Capacity < room.CurrentCapacity)
                    return ServiceResult.Failure("السعة الجديدة أقل من السعة الحالية، غير مسموح.");

                room.Capacity = request.Capacity.Value;
            }
            if (!string.IsNullOrWhiteSpace(request.Description))
                room.Description = request.Description;
            await _repository.UpdateRoomAsync(room);
            return ServiceResult.SuccessMessage("تم تحديث بيانات الغرفة بنجاح.");
        }
        public async Task<ServiceResult> ChangeImageRoomAsync(int id, ImageRoomRequest request)
        {
            var room = await _repository.GetRoomByIdWithImagesAsync(id);
            if (room is null)
                return ServiceResult.Failure("الغرفة غير متوفرة.");

            var hasNewImages = request.Images != null && request.Images.Any();
            var hasDeletes = request.DeletedPublicIds != null && request.DeletedPublicIds.Any();

            if (!hasNewImages && !hasDeletes)
                return ServiceResult.Failure("لا يوجد تغييرات لإجراءها.");

            if (hasDeletes)
            {
                var toDelete = room.RoomImages
                    .Where(x => request.DeletedPublicIds!.Contains(x.PublicId))
                    .ToList();

                foreach (var img in toDelete)
                {
                    await _file.DeleteAsync(img.PublicId);
                    room.RoomImages.Remove(img); 
                }
            }

            if (hasNewImages)
            {
                var uploadedImages = await _file.UploadMultipleAsync(request.Images!, "rooms");


                foreach (var up in uploadedImages)
                {
                    room.RoomImages.Add(new RoomImage
                    {
                        Url = up.Url,
                        PublicId = up.PublicId
                    });
                }
            }
            if (!room.RoomImages.Any())
                return ServiceResult.Failure("يجب أن تحتوي الغرفة على صورة واحدة على الأقل.");

            var updated = await _repository.SaveChangesAsync();
            if (!updated)
                return ServiceResult.Failure("حدث خطأ أثناء تحديث صور الغرفة.");

            return ServiceResult.SuccessMessage("تم تحديث صور الغرفة بنجاح.");
        }


    }
}
