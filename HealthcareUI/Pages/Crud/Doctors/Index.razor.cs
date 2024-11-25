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


namespace HealthcareUI.Pages.Crud.Doctors
{
    public partial class Index : ComponentBase
    {
        private bool isInitiating = true;
        [Inject]
        private IDataService<Doctor> _dataService { get; set; }
        [Inject]
        public NavigationManager _navigationManager { get; set; }
        [Inject]
        public StateContainer _state { get; set; }
        public List<Doctor> Doctors { get;set; } = new ();

        protected async override Task OnInitializedAsync()
        {
            /*
        * This can be call to anything like calling an api to load employees.
        * During execution 'LoadingTemplate' will be displayed.
        * If your api call returns empty result, then 'EmptyTemplate' will be displayed,
        * this way you have proper feedback, for when your datagrid is loading or empty.
        */

            

            await base.OnInitializedAsync();
            Doctors.AddRange(await _dataService.GetRecordsAsync() ?? []);
            isInitiating = false;
        }

        public void OnEdit(Doctor doctor)
        {
            _state.AddRoutingObjectParameter(doctor);
            _navigationManager.NavigateTo($"/Doctors/Edit/{doctor.GetHashCode()}");
        }
        //public void NavigateToEditPage(Patient toEdit)
        //{
        //    _navigationManager.NavigateTo("/Trailers/Edit/" + toEdit.Id);
        //}

        //public void NavigateToDeletePage(Trailer toDelete)
        //{
        //    _navigationManager.NavigateTo("/Trailers/Delete/" + toDelete.Id);
        //}

        //public void NavigateToCreatePage()
        //{
        //    _navigationManager.NavigateTo("/Trailers/Create");
        //}
    }
}
