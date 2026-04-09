using Elderly_System.BLL.Service.Interface;
using Elderly_System.DAL.DTO.Request.Doctor;
using Elderly_System.DAL.DTO.Request.Elderly;
using Elderly_System.DAL.DTO.Response.Doctor;
using Elderly_System.DAL.DTO.Response.Nurse;
using Elderly_System.DAL.DTO.Response.User;
using Elderly_System.DAL.Enums;
using Elderly_System.DAL.Model;
using Elderly_System.DAL.Repositories.Interfaces;
using ElderlySystem.BLL.Helpers;

namespace Elderly_System.BLL.Service.Classes
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _repository;
        private readonly IFileService _file;

        public DoctorService(IDoctorRepository repository , IFileService file)
        {
            _repository = repository;
            _file = file;
        }
        public async Task<ServiceResult> GetMyElderliesAsync(string doctorId)
        {
            if (string.IsNullOrWhiteSpace(doctorId))
                return ServiceResult.Failure("تعذر تحديد الدكتور من التوكن.");

            var elderlies = await _repository.GetMyElderliesAsync(doctorId);
            return ServiceResult.SuccessWithData(elderlies, "تم جلب المسنين التابعين للدكتور بنجاح.");
        }
        public async Task<ServiceResult> AddMedicalReportAsync(string doctorId, AddMedicalReportDto dto)
        {
            if (string.IsNullOrWhiteSpace(doctorId))
                return ServiceResult.Failure("تعذر تحديد الدكتور من التوكن.");

            if (dto == null)
                return ServiceResult.Failure("البيانات المرسلة غير صحيحة.");

            if (dto.DiagnosisFile == null || dto.DiagnosisFile.Length == 0)
                return ServiceResult.Failure("ملف التشخيص مطلوب.");

            var ownsElderly = await _repository.DoctorOwnsElderlyAsync(doctorId, dto.ElderlyId);
            if (!ownsElderly)
                return ServiceResult.Failure("لا يوجد صلاحية لإضافة تشخيص لهذا المسن.");

            var uploadResult = await _file.UploadAsync(dto.DiagnosisFile, "medical-reports");

            var report = new MedicalReport
            {
                ElderlyId = dto.ElderlyId,
                DoctorId = doctorId,
                Date = DateTime.Today,
                DiagnosisUrl = uploadResult.Url,
                DiagnosisPublicId = uploadResult.PublicId
            };

            await _repository.AddMedicalReportAsync(report);
            await _repository.SaveChangesAsync();

            return ServiceResult.SuccessWithData(report.Id, "تم إضافة التشخيص بنجاح.");
        }
        public async Task<ServiceResult> GetPendingDoctorRequestsAsync(string doctorId)
        {
            if (string.IsNullOrWhiteSpace(doctorId))
                return ServiceResult.Failure("تعذر تحديد الدكتور من التوكن.");

            var requests = await _repository.GetPendingDoctorRequestsAsync(doctorId);
            return ServiceResult.SuccessWithData(requests, "تم جلب الطلبات المعلقة بنجاح.");
        }

        public async Task<ServiceResult> UpdateDoctorRequestStatusAsync(string doctorId, int requestId, bool isApproved)
        {
            if (string.IsNullOrWhiteSpace(doctorId))
                return ServiceResult.Failure("تعذر تحديد الدكتور من التوكن.");

            var request = await _repository.GetDoctorRequestByIdAsync(requestId, doctorId);
            if (request == null)
                return ServiceResult.Failure("الطلب غير موجود أو لا يوجد صلاحية عليه.");

            if (isApproved)
            {
                request.RequestStatus = Status.Active;
                request.Elderly.DoctorId = doctorId;
            }
            else
            {
                request.RequestStatus = Status.InActive;
            }

            await _repository.SaveChangesAsync();

            return ServiceResult.SuccessWithData(
                request.Id,
                isApproved ? "تم قبول الطلب بنجاح." : "تم رفض الطلب بنجاح."
            );
        }
        public async Task<ServiceResult> GetDoctorProfileAsync(string doctorId)
        {
            if (string.IsNullOrWhiteSpace(doctorId))
                return ServiceResult.Failure("تعذر تحديد الدكتور من التوكن.");

            var doctor = await _repository.GetDoctorWithDetailsByIdAsync(doctorId);
            if (doctor == null)
                return ServiceResult.Failure("الدكتور غير موجود.");

            var response = new DoctorProfileResponse
            {
                FullName = doctor.FullName,
                Email = doctor.Email,
                PhoneNumber = doctor.PhoneNumber,
                City = doctor.City,
                NationalId = doctor.NationalId,
                Gender = UserDetailsResponse.ToArabic(doctor.Gender),
                MedicalRank = doctor.MedicalRank,
                YearsOfExperience = doctor.YearsOfExperience,
                NumberOfOperations = doctor.NumberOfOperations,
                BDate = doctor.BDate,

                Specializations = doctor.Specializations
                    .Select(x => x.Specialization)
                    .ToList(),

                Diseases = doctor.Diseases
                    .Select(x => x.Disease)
                    .ToList(),

                WorkPlaces = doctor.WorkPlaces
                    .Select(x => x.WorkPlace)
                    .ToList(),

                PreviousWorkPlaces = doctor.PreviousWorkPlaces
                    .Select(x => x.WorkPlace)
                    .ToList(),

                OperationTypes = doctor.OperationTypes
                    .Select(x => x.OperationType)
                    .ToList(),

                MedicalProcedures = doctor.MedicalProcedures
                    .Select(x => x.ProcedureName)
                    .ToList(),

                DiagnosticTests = doctor.DiagnosticTests
                    .Select(x => x.TestName)
                    .ToList(),

                Universities = doctor.Universities
                    .Select(x => new DoctorUniversityResponse
                    {
                        UniversityName = x.UniversityName,
                        Degree = x.Degree.ToString()
                    })
                    .ToList(),

                ElderliesCount = doctor.Elderlies.Count,
                MedicalReportsCount = doctor.MedicalReports.Count
            };

            return ServiceResult.SuccessWithData(response, "تم جلب بيانات الدكتور بنجاح.");
        }

        public async Task<ServiceResult> UpdateDoctorProfileAsync(string doctorId, UpdateDoctorProfileRequest request)
        {
            if (string.IsNullOrWhiteSpace(doctorId))
                return ServiceResult.Failure("تعذر تحديد الدكتور من التوكن.");

            var doctor = await _repository.GetDoctorWithDetailsByIdAsync(doctorId);
            if (doctor == null)
                return ServiceResult.Failure("الدكتور غير موجود.");

            if (request == null)
                return ServiceResult.Failure("البيانات المرسلة غير صحيحة.");

            if (!string.IsNullOrWhiteSpace(request.MedicalRank))
                doctor.MedicalRank = request.MedicalRank.Trim();

            if (!string.IsNullOrWhiteSpace(request.YearsOfExperience))
                doctor.YearsOfExperience = request.YearsOfExperience.Trim();

            if (request.NumberOfOperations.HasValue)
                doctor.NumberOfOperations = request.NumberOfOperations.Value;

           

            if (request.Specializations != null)
            {
                doctor.Specializations.Clear();

                foreach (var item in request.Specializations
                             .Where(x => !string.IsNullOrWhiteSpace(x))
                             .Select(x => x.Trim())
                             .Distinct())
                {
                    doctor.Specializations.Add(new DoctorSpecialization
                    {
                        DoctorId = doctorId,
                        Specialization = item
                    });
                }
            }

            if (request.Diseases != null)
            {
                doctor.Diseases.Clear();

                foreach (var item in request.Diseases
                             .Where(x => !string.IsNullOrWhiteSpace(x))
                             .Select(x => x.Trim())
                             .Distinct())
                {
                    doctor.Diseases.Add(new DoctorDisease
                    {
                        DoctorId = doctorId,
                        Disease = item
                    });
                }
            }
            doctor.PreviousWorkPlaces ??= new List<DoctorPreviousWorkPlace>();
            doctor.WorkPlaces ??= new List<DoctorWorkPlace>();
            if (request.WorkPlaces != null)
            {
                foreach (var existing in doctor.WorkPlaces)
                {
                    if (!doctor.PreviousWorkPlaces.Any(p => p.WorkPlace == existing.WorkPlace))
                    {
                        doctor.PreviousWorkPlaces.Add(new DoctorPreviousWorkPlace
                        {
                            DoctorId = doctorId,
                            WorkPlace = existing.WorkPlace
                        });
                    }
                }

                doctor.WorkPlaces.Clear();

                foreach (var item in request.WorkPlaces
                             .Where(x => !string.IsNullOrWhiteSpace(x))
                             .Select(x => x.Trim())
                             .Distinct())
                {
                    doctor.WorkPlaces.Add(new DoctorWorkPlace
                    {
                        DoctorId = doctorId,
                        WorkPlace = item
                    });
                }
            }


            if (request.OperationTypes != null)
            {
                doctor.OperationTypes.Clear();

                foreach (var item in request.OperationTypes
                             .Where(x => !string.IsNullOrWhiteSpace(x))
                             .Select(x => x.Trim())
                             .Distinct())
                {
                    doctor.OperationTypes.Add(new DoctorOperationType
                    {
                        DoctorId = doctorId,
                        OperationType = item
                    });
                }
            }

            if (request.MedicalProcedures != null)
            {
                doctor.MedicalProcedures.Clear();

                foreach (var item in request.MedicalProcedures
                             .Where(x => !string.IsNullOrWhiteSpace(x))
                             .Select(x => x.Trim())
                             .Distinct())
                {
                    doctor.MedicalProcedures.Add(new DoctorMedicalProcedure
                    {
                        DoctorId = doctorId,
                        ProcedureName = item
                    });
                }
            }

            if (request.DiagnosticTests != null)
            {
                doctor.DiagnosticTests.Clear();

                foreach (var item in request.DiagnosticTests
                             .Where(x => !string.IsNullOrWhiteSpace(x))
                             .Select(x => x.Trim())
                             .Distinct())
                {
                    doctor.DiagnosticTests.Add(new DoctorDiagnosticTest
                    {
                        DoctorId = doctorId,
                        TestName = item
                    });
                }
            }
            await _repository.SaveChangesAsync();

            return ServiceResult.SuccessWithData(doctor.Id, "تم تعديل بيانات الدكتور بنجاح.");
        }
        public async Task<ServiceResult> GetDoctorElderlyDetailsAsync(int elderlyId)
        {
            if (elderlyId <= 0)
                return ServiceResult.Failure("رقم المسن غير صحيح.");

            var dto = await _repository.GetElderlyDetailsAsync(elderlyId);

            if (dto == null)
                return ServiceResult.Failure("المسن غير موجود.");

            return ServiceResult.SuccessWithData(dto, "تم جلب تفاصيل المسن بنجاح");
        }

        public async Task<ServiceResult> GetDoctorMedicalReportDiagnosisAsync(int reportId)
        {
            if (reportId <= 0)
                return ServiceResult.Failure("رقم التقرير غير صحيح.");

            var report = await _repository.GetMedicalReportByIdAsync(reportId);
            if (report == null)
                return ServiceResult.Failure("التقرير غير موجود.");

            var dto = new NurseDiagnosisDto
            {
                ReportId = report.Id,
                Date = report.Date.ToString("yyyy-MM-dd"),
                DiagnosisUrl = report.DiagnosisUrl,
                DiagnosisPublicId = report.DiagnosisPublicId,
                Doctor = new DoctorInfoDto
                {
                    DoctorId = report.Doctor.Id,
                    Name = report.Doctor.FullName,
                    WorkPlace = report.Doctor.WorkPlaces
                        .OrderByDescending(wp => wp.Id)
                        .Select(wp => wp.WorkPlace)
                        .FirstOrDefault() ?? "",
                    Phone = report.Doctor.PhoneNumber!
                }
            };

            return ServiceResult.SuccessWithData(dto, "تم جلب التشخيص بنجاح");
        }

    }
}
