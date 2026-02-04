using Elderly_System.BLL.Service.Interface;
using Elderly_System.DAL.DTO.Request.Donation;
using Elderly_System.DAL.Enums;
using Elderly_System.DAL.Repositories.Interfaces;
using ElderlySystem.BLL.Helpers;
using ElderlySystem.DAL.Model;

namespace Elderly_System.BLL.Service.Classes
{
    public class DonationService :IDonationService
    {
        private readonly IDonationRepository _repository;
        public DonationService(IDonationRepository repository)
        {
            _repository = repository;
        }
        public async Task<ServiceResult> CreateDonationAsync(DonationCreateRequest request, string userId)
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
                AdminId = userId!
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
    }
}
