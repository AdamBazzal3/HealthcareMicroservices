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


namespace HealthcareUI.Pages.Crud.Prescriptions
{
    public partial class Index : ComponentBase
    {

        [Inject]
        private IDataService<Prescription> _dataService { get; set; }
        [Inject]
        public NavigationManager _navigationManager { get; set; }
        [Inject]
        public StateContainer _state { get; set; }
        [Parameter]
        public List<Prescription> Prescriptions { get;set; } = new ();
        private Prescription assignedPrescription = new();
        private bool isEditModalVisible;
        private void OnClosingEditModal(Prescription p)
        {
            isEditModalVisible = false;
        }
        public void OnEdit(Prescription prescription)
        {
            assignedPrescription = prescription; isEditModalVisible = true;
        }
        public void OnAdd()
        {
            isModalVisible = true;
        }
        private void OnDismissingEditModal()
        {
            isEditModalVisible = false;
        }
        //OnClose="OnClosingEditModal" OnDismiss="OnDismissingEditModal"
        protected async override Task OnInitializedAsync()
        {
            /*
            * This can be call to anything like calling an api to load employees.
            * During execution 'LoadingTemplate' will be displayed.
            * If your api call returns empty result, then 'EmptyTemplate' will be displayed,
            * this way you have proper feedback, for when your datagrid is loading or empty.
            */

            

            await base.OnInitializedAsync();
           // Prescriptions.AddRange(await _dataService.GetRecordsAsync() ?? []);
        }
        public bool isModalVisible;
        private void OnClosingModal(Prescription p)
        {
            isModalVisible = false;
            Prescriptions.Add(p);
        }
        private void OnDismissingModal()
        {
            isModalVisible = false;
        } private void OnDeletePrescription(Prescription p)
        {
            Prescriptions.Remove(p);
            isModalVisible = false;
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
