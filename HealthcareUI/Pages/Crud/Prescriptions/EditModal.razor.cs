using HealthcareUI.Models;
using HealthcareUI.Services.RestService;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace HealthcareUI.Pages.Crud.Prescriptions
{
    public partial class EditModal
    {
        [Parameter] public string Title { get; set; }
        [Parameter] public bool IsVisible { get; set; }
        
        [Parameter] public EventCallback<Prescription> OnClose { get; set; }
        [Parameter] public EventCallback OnDismiss { get; set; }
        [Parameter] public EventCallback<Prescription> OnDeleteEvent { get; set; }
        private void Close()
        {
            if(Success)
                OnClose.InvokeAsync(prescription);
            else
                OnDismiss.InvokeAsync(prescription);
            prescription = new();
            prescription.DateIssued = DateTime.Now;
            StateHasChanged();
        }

        private void HandleBackgroundClick()
        {
            Close();
        }
        [Parameter]
        public Prescription prescription { get; set; } = new();
        private EditContext editContext;
        [Inject]
        private IDataService<Prescription> _dataService { get; set; }
        [Inject]
        public NavigationManager _navigationManager { get; set; }
        public string Alert;
        public bool Success;
        public bool displayError;
        protected override void OnInitialized()
        {
            base.OnInitialized();
        }
        private void HandleValidSubmit()
        {
            try
            {
                Success = true;
                Close();
            }
            catch (Exception ex)
            {

            }

        }
        public void OnDelete()
        {
            try
            {
                OnDeleteEvent.InvokeAsync(prescription);
                StateHasChanged();
            }
            catch (Exception ex)
            {

            }
        }
    }
}