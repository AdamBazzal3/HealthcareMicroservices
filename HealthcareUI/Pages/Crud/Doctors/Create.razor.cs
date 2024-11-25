using HealthcareUI.Models;
using HealthcareUI.Services.RestService;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace HealthcareUI.Pages.Crud.Doctors
{
    public partial class Create : ComponentBase
    {
        private Doctor doctor = new Doctor();
        private EditContext editContext;
        [Inject]
        private IDataService<Doctor> _dataService { get; set; }
        [Inject]
        public NavigationManager _navigationManager { get; set; }

        public string Alert;
        public bool Success;
        public bool displayError;
        protected override void OnInitialized()
        {
            base.OnInitialized();
            doctor.DateOfBirth = DateTime.Now;
        }
        private async void HandleValidSubmit()
        {
            try
            {
                ResponseResult r = await _dataService.InsertRecordAsync(doctor);

                if (r.IsSuccess)
                {
                    Success = true;
                    Alert = "Success";
                    doctor = new();
                   
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