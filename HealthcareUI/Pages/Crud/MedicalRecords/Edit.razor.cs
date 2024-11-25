using HealthcareUI.Models;
using HealthcareUI.Services;
using HealthcareUI.Services.RestService;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace HealthcareUI.Pages.Crud.MedicalRecords
{
    public partial class Edit : ComponentBase
    {
        private MedicalRecord medicalRecord = new();
        private EditContext editContext;
        [Inject]
        private IDataService<MedicalRecord> _dataService { get; set; }
        [Inject]
        private IDataService<Patient> _patientDataService { get; set; }
        [Inject]
        public StateContainer _state { get; set; }
        [Inject]
        public NavigationManager _navigationManager { get; set; }
        [Parameter] public int SetHashCode { get; set; }
        [Inject]
        private IDataService<Doctor> _doctorDataService { get; set; }

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
            medicalRecord = _state.GetRoutingObjectParameter<MedicalRecord>(SetHashCode);
            Patients.AddRange(await _patientDataService.GetRecordsAsync() ?? []);
            Doctors.AddRange(await _doctorDataService.GetAvailableDoctors() ?? []);
            _selectedDoctorId = medicalRecord.Doctor.Id;
            _selectedPatientId = medicalRecord.Patient.Id;
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

                ResponseResult r = await _dataService.UpdateRecordAsync(medicalRecord);

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
        public async void OnDelete()
        {
            try
            {
                ResponseResult r = await _dataService.DeleteRecordAsync(medicalRecord.Id);

                if (r.IsSuccess)
                {
                    _navigationManager.NavigateTo("/MedicalRecords", true);

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