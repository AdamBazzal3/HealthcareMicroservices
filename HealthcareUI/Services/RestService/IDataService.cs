using HealthcareUI.Models;
using Microsoft.AspNetCore.Components.Forms;


namespace HealthcareUI.Services.RestService
{
    public interface IDataService<T> where T : class
    {
        Task<List<T>> GetRecordsAsync();
        Task<List<T>> GetAvailableDoctors();
        Task<(bool,List<T>)> GetPatientMedicalRecords(string patientId);
        Task<ResponseResult> InsertRecordAsync(T patient);
        Task<ResponseResult> UpdateRecordAsync(T patient);
        Task<ResponseResult> DeleteRecordAsync(string id);

    }
}
