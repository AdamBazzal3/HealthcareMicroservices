using HealthcareUI.Models;
using HealthcareUI.Services;
using HealthcareUI.Services.RestService;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace HealthcareUI.Pages.Crud.Doctors
{
    public partial class Edit : ComponentBase
    {
        private Doctor doctor = new ();
        private EditContext editContext;
        [Inject]
        private IDataService<Doctor> _dataService { get; set; }
        [Inject]
        public StateContainer _state { get; set; }
        [Inject]
        public NavigationManager _navigationManager { get; set; }
        [Parameter] public int SetHashCode { get; set; }
        public string Alert;
        public bool Success;
        public bool displayError;
        protected override void OnInitialized()
        {
            base.OnInitialized();
            doctor = _state.GetRoutingObjectParameter<Doctor>(SetHashCode);


        }
        private async void HandleValidSubmit()
        {
            try
            {
                ResponseResult r = await _dataService.UpdateRecordAsync(doctor);

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
        public async void OnDelete()
        {
            try
            {
                ResponseResult r = await _dataService.DeleteRecordAsync(doctor.Id);

                if (r.IsSuccess)
                {
                    _navigationManager.NavigateTo("/Doctors", true);

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