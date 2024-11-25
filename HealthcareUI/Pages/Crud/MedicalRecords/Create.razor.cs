using Consul.Filtering;
using HealthcareUI.Models;
using HealthcareUI.Services.RestService;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Security.Cryptography;

namespace HealthcareUI.Pages.Crud.MedicalRecords
{
    public partial class Create : ComponentBase
    {
        private MedicalRecord medicalRecord = new();
        private EditContext editContext;
        [Inject]
        private IDataService<MedicalRecord> _dataService { get; set; }
        [Inject]
        private IDataService<Patient> _patientDataService { get; set; }
        [Inject]
        private IDataService<Doctor> _doctorDataService { get; set; }
        [Inject]
        public NavigationManager _navigationManager { get; set; }
        // Sample data for Patients and Doctors
        private List<Patient> Patients = [];

        private List<Doctor> Doctors = [];
        private string _selectedDoctorId;
        private string _selectedPatientId;
        public string Alert;
        public bool Success;
        public bool displayError;
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Patients.AddRange(await _patientDataService.GetRecordsAsync() ?? []);
            Doctors.AddRange(await _doctorDataService.GetAvailableDoctors() ?? []);
            medicalRecord.DateOfVisit = DateTime.Now;
        }

        private async void HandleValidSubmit()
        {
            try
            {
                if (string.IsNullOrEmpty(_selectedPatientId)) return;
                medicalRecord.Patient = Patients?.Find(e => e.Id == _selectedPatientId);
                if (string.IsNullOrEmpty(_selectedDoctorId)) return;
                medicalRecord.Doctor = Doctors?.Find(e => e.Id == _selectedDoctorId);

                ResponseResult r = await _dataService.InsertRecordAsync(medicalRecord);

                if (r.IsSuccess)
                {
                    Success = true;
                    Alert = "Success";
                    medicalRecord = new();
                   
                }
                else
                {
                    Success = false;
                    Alert = r.Message;
                }

                displayError = true;
                StateHasChanged();
            }
            catch (Exception ex)
            {

            }

        }
    }
}