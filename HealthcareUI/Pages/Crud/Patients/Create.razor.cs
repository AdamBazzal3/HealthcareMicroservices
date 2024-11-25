using HealthcareUI.Models;
using HealthcareUI.Services.RestService;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace HealthcareUI.Pages.Crud.Patients
{
    public partial class Create : ComponentBase
    {
        private Patient patient = new Patient();
        private EditContext editContext;
        [Inject]
        private IDataService<Patient> _dataService { get; set; }
        [Inject]
        public NavigationManager _navigationManager { get; set; }

        public string Alert;
        public bool Success;
        public bool displayError;
        protected override void OnInitialized()
        {
            base.OnInitialized();
            patient.DateOfBirth = DateTime.Now;
        }
        private async void HandleValidSubmit()
        {
            try
            {
                ResponseResult r = await _dataService.InsertRecordAsync(patient);

                if (r.IsSuccess)
                {
                    Success = true;
                    Alert = "Success";
                    patient = new();
                   
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