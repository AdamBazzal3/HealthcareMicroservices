
using HealthcareClinicAPI.Utils;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using HealthcareClinicAPI.Attributes;
using System.Text.Json.Serialization;

namespace HealthcareClinicAPI.Models
{


    [BsonCollection("doctors")]
    public class Doctor : Person
    {
        [BsonId]
        [JsonIgnore]
        public ObjectId _id { get; set; }



        public Doctor()
            
        {
            _id = ObjectId.GenerateNewId();
        }
        //[Association("SelectedRides-SelectedDrivers")]
        //[IgnoreConvertToJson]
        //public XPCollection<Ride> SelectedRides
        //{
        //    get { return GetCollection<Ride>(nameof(SelectedRides)); }
        //}




        public string? Id
        {
            set
            {
                if (value == null) return;

                _id = ObjectId.Parse(value);
            }
            get => _id.ToString();
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
