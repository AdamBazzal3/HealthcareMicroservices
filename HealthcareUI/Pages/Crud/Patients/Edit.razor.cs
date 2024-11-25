using HealthcareUI.Models;
using HealthcareUI.Services;
using HealthcareUI.Services.RestService;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Polly.CircuitBreaker;
using Polly.Timeout;

namespace HealthcareUI.Pages.Crud.Patients
{
    public partial class Edit : ComponentBase
    {
        private Patient patient = new ();
        private EditContext editContext;
        [Inject]
        private IDataService<Patient> _dataService { get; set; }
        [Inject]
        private IDataService<MedicalRecord> _medicaldataService { get; set; }
        [Inject]
        public StateContainer _state { get; set; }

        public List<MedicalRecord> MedicalRecords { get; set; } = [];
        [Inject]
        public NavigationManager _navigationManager { get; set; }
        [Parameter] public int SetHashCode { get; set; }
        public string Alert;
        public bool Success;
        public bool displayError;
        protected override void OnInitialized()
        {
            base.OnInitialized();
            patient = _state.GetRoutingObjectParameter<Patient>(SetHashCode);


        }
        private async void HandleValidSubmit()
        {
            try
            {
                ResponseResult r = await _dataService.UpdateRecordAsync(patient);

                if (r.IsSuccess)
                {
                    Success = true;
                    Alert = "Success";

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
        private string error;
        private bool IsSuccess = true;
        private bool IsFetching ;
        private async void FetchMedicalHistory()
        {
            try
            {
                IsFetching = true;
                (bool success,List<MedicalRecord> records) = await _medicaldataService.GetPatientMedicalRecords(patient.Id);

                if (success)
                {
                    MedicalRecords = records ?? [];
                    IsSuccess = true;
                }
                else
                {
                    IsSuccess = false;
                }

            }
            catch (BrokenCircuitException ex)
            {
                IsSuccess = false;
            }
            catch (Exception ex)
            {
                IsSuccess = false;
            }
            finally
            {
                IsFetching = false;
                StateHasChanged();
            }
            
        }
        public async void OnDelete()
        {
            try
            {
                ResponseResult r = await _dataService.DeleteRecordAsync(patient.Id);

                if (r.IsSuccess)
                {
                    _navigationManager.NavigateTo("/Patients", true);

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