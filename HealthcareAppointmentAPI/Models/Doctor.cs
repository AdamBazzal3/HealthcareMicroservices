
using HealthcareAppointmentAPI.Utils;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace HealthcareAppointmentAPI.Models
{

    

    public class Doctor : Person
    {
    

        public Doctor()
            
        {
         
        }
        //[Association("SelectedRides-SelectedDrivers")]
        //[IgnoreConvertToJson]
        //public XPCollection<Ride> SelectedRides
        //{
        //    get { return GetCollection<Ride>(nameof(SelectedRides)); }
        //}


        

        public string Id
        {
            get; set;
        }


        private string _Specialty;
        public string Specialty
        {
            get { return _Specialty; }
            set
            {
                if (value == null) return;
                if (value.Equals(_Specialty))
                    return;

                _Specialty = value;
            }
        }

        private string _ContactInformation;
        public string ContactInformation
        {
            get { return _ContactInformation; }
            set
            {
                if (value == null) return;
                if (value.Equals(_ContactInformation))
                    return;

                _ContactInformation = value;
            }
        }

        private string _EmploymentDetails;
        public string EmploymentDetails
        {
            get { return _EmploymentDetails; }
            set
            {
                if (value == null) return;
                if (value.Equals(_EmploymentDetails))
                    return;

                _EmploymentDetails = value;
            }
        }

        private string _Schedule;
        public string Schedule
        {
            get { return _Schedule; }
            set
            {
                if (value == null) return;
                if (value.Equals(_Schedule))
                    return;

                _Schedule = value;
            }
        }


        private bool _IsAvailable;
        public bool IsAvailable
        {
            get { return _IsAvailable; }
            set
            {
                if (value == null) return;
                if (value.Equals(_IsAvailable))
                    return;

                _IsAvailable = value;
            }
        }
        private string _LicenseNumber;
        public string LicenseNumber
        {
            get { return _LicenseNumber; }
            set
            {
                if (value == null) return;
                if (value.Equals(_LicenseNumber))
                    return;

                _LicenseNumber = value;
            }
        }



    }


}
