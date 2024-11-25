
using HealthcareUI.Utils;
using System.Security.Cryptography;
using System.Text.Json.Serialization;

namespace HealthcareUI.Models
{

    public class Patient : Person
    {


        
        public string Id { set; get; }
        private Gender _Gender;

        public Gender Gender
        {
            get { return _Gender; }
            set
            {
                if (value == null) return;
                if (value.Equals(_Gender))
                    return;

                _Gender = value;
            }
        }

        //private XPCollection<MedicalRecord> _MedicalHistory;
        //[Association("Patient-MedicalRecords")]
        //public XPCollection<MedicalRecord> MedicalHistory
        //{
        //    get
        //    {
        //        return GetCollection<MedicalRecord>(nameof(MedicalHistory));
        //    }
        //}

        //private string _CurrentMedications;
        //public string CurrentMedications
        //{
        //    get { return _CurrentMedications; }
        //    set { SetPropertyValue(nameof(CurrentMedications), ref _CurrentMedications, value); }
        //}

        private string _InsuranceDetails;
        public string InsuranceDetails
        {
            get { return _InsuranceDetails; }
            set
            {
                if (value == null) return;
                if (value.Equals(_InsuranceDetails))
                    return;

                _InsuranceDetails = value;
            }
        }

    }

}
