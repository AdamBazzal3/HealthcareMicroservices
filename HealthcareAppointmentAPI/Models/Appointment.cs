
using HealthcareAppointmentAPI.Attributes;


using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


using System.Text.Json.Serialization;

namespace HealthcareAppointmentAPI.Models
{

    [BsonCollection("appointments")]
    public class Appointment 
    {
        [BsonId]
        [JsonIgnore]
        public ObjectId _id { get; set; }
        private bool _IsActive;

        public Appointment()
        {
            _id = ObjectId.GenerateNewId();
        }

        public string? Id
        {
            set
            {
                if (value == null) return;

                _id = ObjectId.Parse(value);
            }
            get => _id.ToString();
        }
        public bool IsActive
        {
            get { return _IsActive; }
            set
            {
                if (value == null) return;
                if (value.Equals(_IsActive))
                    return;

                _IsActive = value;
            }
        }


        public void Deactivate()
        {
            IsActive = false;
        }

        //[DevExpress.Xpo.Aggregated]
        //[Association("Passenger-Rides")]
        //[IgnoreConvertToJson]
        //public XPCollection<Ride> PassengerRides
        //{
        //    get { return GetCollection<Ride>(nameof(PassengerRides)); }
        //}

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
            set
            {
                if (value == null) return;
                if (value.Equals(_Doctor))
                    return;

                _Doctor = value;
            }
        }

        private DateTime _DateTime;

        public DateTime DateTime
        {
            get { return _DateTime; }
            set
            {
                if (value == null) return;
                if (value.Equals(_DateTime))
                    return;

                _DateTime = value;
            }
        }
        private AppointmentStatus _Status;

        public AppointmentStatus Status
        {
            get { return _Status; }
            set
            {
                if (value == null) return;
                if (value.Equals(_Status))
                    return;

                _Status = value;
            }
        }
        private string _Location;

        public string Location
        {
            get { return _Location; }
            set
            {
                if (value == null) return;
                if (value.Equals(_Location))
                    return;

                _Location = value;
            }
        }

        private string _Reason;

        public string Reason
        {
            get { return _Reason; }
            set
            {
                if (value == null) return;
                if (value.Equals(_Reason))
                    return;

                _Reason = value;
            }
        }

    }

}
