using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HealthcareUI.Models
{


    public class Prescription 
    {

        public string Id
        {
            get; set;
        }

        private bool _IsActive;

        public Prescription()

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


  



        private DateTime _DateIssued;
        [Required]
        public DateTime DateIssued
        {
            get { return _DateIssued; }
            set {
                if (value == null) return;
                if (value.Equals(_DateIssued))
                    return;

                _DateIssued = value;
            }
        }

  
        private string _Dosage;
        [Required]
        public string Dosage
        {
            get { return _Dosage; }
            set {
                if (value == null) return;
                if (value.Equals(_Dosage))
                    return;

                _Dosage = value;
            }
        }



    }

}
