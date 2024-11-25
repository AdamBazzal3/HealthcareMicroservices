
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace HealthcareClinicAPI.Models
{


    public class Prescription 
    {
        [BsonId]
        [JsonIgnore]
        public ObjectId _id { get; set; }
        public string? Id
        {
            set
            {
                if (value == null) return;

                _id = ObjectId.Parse(value);
            }
            get => _id.ToString();
        }

        private bool _IsActive;

        public Prescription()

        {
            _id = ObjectId.GenerateNewId();

        }
       



        [DefaultValue(true)]
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


        public void Deactivate()
        {
            IsActive = false;
        }



        private DateTime _DateIssued;
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
