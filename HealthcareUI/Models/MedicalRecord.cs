
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace HealthcareUI.Models
{



    public class MedicalRecord {


        public string Id
        {
            get;set;
        }
        private bool _IsActive;

        public MedicalRecord()
        {
        

        }
       


        public bool IsActive
        {
            get { return _IsActive; }
            set {
                if (value == null) return;
                if (value.Equals(_IsActive))
                    return;

                _IsActive = value;
            }
        }


        private Patient _Patient;


        public Patient Patient
        {
            get { return _Patient; }
            set
            {
                if (value == null) return;
                if (value.Equals(_Patient))
                    return;

                _Patient = value;
            }
        }

        private Doctor _Doctor;

        public Doctor Doctor
        {
            get { return _Doctor; }
            set {
                if (value == null) return;
                if (value.Equals(_Doctor))
                    return;

                _Doctor = value;
            }
        }

        private DateTime _DateOfVisit;

        public DateTime DateOfVisit
        {
            get { return _DateOfVisit; }
            set
            {
                if (value == null) return;
                if (value.Equals(_DateOfVisit))
                    return;

                _DateOfVisit = value;
            }
        }

        private string _ClinicalNotes;

        public string ClinicalNotes
        {
            get { return _ClinicalNotes; }
            set {
                if (value == null) return;
                if (value.Equals(_ClinicalNotes))
                    return;

                _ClinicalNotes = value;
            }
        }

        private string _TestResults;
        public string TestResults
        {
            get { return _TestResults; }
            set {
                if (value == null) return;
                if (value.Equals(_TestResults))
                    return;

                _TestResults = value;
            }
        }

        private List<Prescription> _Prescriptions = new();

        public List<Prescription> Prescriptions
        {
            get { return _Prescriptions; }
            set
            {
                if (value == null) return;
                if (value.Equals(_Prescriptions))
                    return;

                _Prescriptions = value;
            }
        }

    }

}
