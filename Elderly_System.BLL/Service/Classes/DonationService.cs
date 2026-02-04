using Elderly_System.BLL.Service.Interface;
using Elderly_System.DAL.DTO.Request.Donation;
using Elderly_System.DAL.Enums;
using Elderly_System.DAL.Repositories.Interfaces;
using ElderlySystem.BLL.Helpers;
using ElderlySystem.DAL.Model;

namespace Elderly_System.BLL.Service.Classes
{
    public class DonationService : IDonationService
    {
        private readonly IDonationRepository _repository;
        public DonationService(IDonationRepository repository)
        {
            _repository = repository;
        }
        public async Task<ServiceResult> CreateDonationAsync(DonationCreateRequest request, string AdminId)
        {
            if (request.DonationType == DonationType.Cash)
            {
                if (request.MonetaryAmount is null || request.MonetaryAmount <= 0)
                    return ServiceResult.Failure("قيمة التبرع النقدي مطلوبة ويجب أن تكون أكبر من صفر.");

                if (string.IsNullOrWhiteSpace(request.Currency))
                    return ServiceResult.Failure("العملة مطلوبة للتبرع النقدي.");

                if (request.Goods != null && request.Goods.Count > 0)
                    return ServiceResult.Failure("التبرع النقدي لا يحتوي عناصر عينية.");
            }
            else
            {
                if (request.Goods == null || request.Goods.Count == 0)
                    return ServiceResult.Failure("يجب إضافة عنصر واحد على الأقل في التبرع العيني.");

                if (request.MonetaryAmount != null)
                    return ServiceResult.Failure("التبرع العيني لا يحتوي على مبلغ نقدي.");

                if (!string.IsNullOrWhiteSpace(request.Currency))
                    return ServiceResult.Failure("التبرع العيني لا يحتاج عملة.");
            }

            var donation = new Donation
            {
                DonorName = request.DonorName,
                DonationDate = request.DonationDate ?? DateTime.Now,
                DonationType = request.DonationType,
                MonetaryAmount = request.MonetaryAmount,
                Currency = request.Currency,
                AdminId = AdminId
            };

            if (request.Goods != null && request.Goods.Count > 0)
            {
                donation.Goods = request.Goods.Select(g => new Good
                {
                    NameGood = g.NameGood,
                    Quantity = g.Quantity
                }).ToList();
            }
            await _repository.AddDonationAsync(donation);
            return ServiceResult.SuccessMessage("تم إضافة التبرع بنجاح.");
        }
        public async Task<ServiceResult> DeleteDonationAsync(int donationId)
        {
            var donation = await _repository.GetDonationByIdAsync(donationId);
            if (donation == null)
                return ServiceResult.Failure("التبرع غير موجود.");

            await _repository.DeleteDonationAsync(donation);
            return ServiceResult.SuccessMessage("تم حذف التبرع بنجاح.");
        }
        /*public async Task<ServiceResult> UpdateDonationAsync(int donationId, DonationUpdateRequest request)
        {
            var donation = await _repository.GetDonationByIdAsync(donationId);
            if (donation == null)
                return ServiceResult.Failure("التبرع غير موجود.");

            // ===== Cash =====
            if (donation.DonationType == DonationType.Cash)
            {
                if (request.GoodId.HasValue || request.NameGood != null || request.Quantity.HasValue)
                    return ServiceResult.Failure("لا يمكن تعديل عناصر عينية لتبرع نقدي.");

                if (!request.MonetaryAmount.HasValue && request.Currency == null)
                    return ServiceResult.Failure("أرسل MonetaryAmount أو Currency للتعديل.");

                if (request.MonetaryAmount.HasValue)
                {
                    if (request.MonetaryAmount.Value <= 0)
                        return ServiceResult.Failure("قيمة التبرع يجب أن تكون أكبر من صفر.");
                    donation.MonetaryAmount = request.MonetaryAmount.Value;
                }

                if (request.Currency != null)
                {
                    if (string.IsNullOrWhiteSpace(request.Currency))
                        return ServiceResult.Failure("العملة لا يمكن أن تكون فارغة.");
                    donation.Currency = request.Currency.Trim().ToUpper();
                }

                await _repository.SaveChangesAsync();
                return ServiceResult.SuccessMessage("تم تعديل التبرع النقدي بنجاح.");
            }

            // ===== InKind =====
            else
            {
                if (request.MonetaryAmount.HasValue || request.Currency != null)
                    return ServiceResult.Failure("لا يمكن تعديل مبلغ/عملة لتبرع عيني.");

                if (!request.GoodId.HasValue)
                    return ServiceResult.Failure("GoodId مطلوب لتعديل عنصر عيني.");

                var good = donation.Goods.FirstOrDefault(g => g.Id == request.GoodId.Value);
                if (good == null)
                    return ServiceResult.Failure("العنصر غير موجود ضمن هذا التبرع.");

                if (request.NameGood == null && !request.Quantity.HasValue)
                    return ServiceResult.Failure("أرسل NameGood أو Quantity للتعديل.");

                if (request.NameGood != null)
                {
                    if (string.IsNullOrWhiteSpace(request.NameGood))
                        return ServiceResult.Failure("اسم العنصر لا يمكن أن يكون فارغ.");
                    good.NameGood = request.NameGood.Trim();
                }

                if (request.Quantity.HasValue)
                {
                    if (request.Quantity.Value <= 0)
                        return ServiceResult.Failure("الكمية يجب أن تكون أكبر من صفر.");
                    good.Quantity = request.Quantity.Value;
                }

                await _repository.SaveChangesAsync();
                return ServiceResult.SuccessMessage("تم تعديل التبرع العيني بنجاح.");
            }
        }*/
    
        }

    }

