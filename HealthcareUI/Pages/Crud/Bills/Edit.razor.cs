using HealthcareUI.Models;
using HealthcareUI.Services;
using HealthcareUI.Services.RestService;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace HealthcareUI.Pages.Crud.Bills
{
    public partial class Edit : ComponentBase
    {
        private Bill bill = new();
        private EditContext editContext;
        [Inject]
        private IDataService<Bill> _dataService { get; set; }
        [Inject]
        private IDataService<Patient> _patientDataService { get; set; }
        [Parameter] public int SetHashCode { get; set; }
        [Inject]
        public StateContainer _state { get; set; }
        [Inject]
        private IDataService<Doctor> _doctorDataService { get; set; }
        [Inject]
        public NavigationManager _navigationManager { get; set; }
        private List<Patient> Patients = [];

        private List<Doctor> Doctors = [];
        private string _selectedDoctorId;
        private string _selectedPatientId;

        public string Alert;
        public bool Success;
        public bool displayError;
        protected override async Task OnInitializedAsync()
        {
            base.OnInitialized();
            bill = _state.GetRoutingObjectParameter<Bill>(SetHashCode);
            Patients.AddRange(await _patientDataService.GetRecordsAsync() ?? []);
            Doctors.AddRange(await _doctorDataService.GetAvailableDoctors() ?? []);
            _selectedDoctorId = bill.Doctor.Id;
            _selectedPatientId = bill.Patient.Id;
        }
        private async void HandleValidSubmit()
        {
            try
            {
                if (string.IsNullOrEmpty(_selectedPatientId)) return;
                bill.Patient = Patients?.Find(e => e.Id == _selectedPatientId);
                if (string.IsNullOrEmpty(_selectedDoctorId)) return;
                bill.Doctor = Doctors?.Find(e => e.Id == _selectedDoctorId);

                ResponseResult r = await _dataService.UpdateRecordAsync(bill);

                if (r.IsSuccess)
                {
                    Success = true;
                    Alert = "Success";
                    bill = new();

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
                ResponseResult r = await _dataService.DeleteRecordAsync(bill.Id);

                if (r.IsSuccess)
                {
                    _navigationManager.NavigateTo("/Bills", true);

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