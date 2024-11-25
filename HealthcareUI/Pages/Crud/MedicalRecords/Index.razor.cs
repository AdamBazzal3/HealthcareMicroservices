using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using HealthcareUI.Models;
using HealthcareUI.Services;
using HealthcareUI.Services.RestService;
using Microsoft.AspNetCore.Components;
using Microsoft.CodeAnalysis.Differencing;


namespace HealthcareUI.Pages.Crud.MedicalRecords
{
    public partial class Index : ComponentBase
    {
        private bool isInitiating = true;
        [Inject]
        private IDataService<MedicalRecord> _dataService { get; set; }
        [Inject]
        public NavigationManager _navigationManager { get; set; }
        [Inject]
        public StateContainer _state { get; set; }
        public List<MedicalRecord> MedicalRecords { get;set; } = new ();

        protected async override Task OnInitializedAsync()
        {
   
            await base.OnInitializedAsync();
            var list = await _dataService.GetRecordsAsync();

            MedicalRecords.AddRange(list ??= []);
            isInitiating = false;
        }

        public void OnEdit(MedicalRecord medicalRecord)
        {
            _state.AddRoutingObjectParameter(medicalRecord);
            _navigationManager.NavigateTo($"/MedicalRecords/Edit/{medicalRecord.GetHashCode()}");
        }
    }
}
