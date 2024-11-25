using MongoDB.Bson.Serialization.Attributes;

using HealthcareAppointmentAPI.Models;

namespace HealthcareAppointmentAPI.Utils
{
    [BsonKnownTypes(typeof(Patient))]
    public class Person
    {
        private DateTime _DateOfBirth;

        [BsonElement(nameof(FirstName))]
        public string FirstName { get; set; }
        [BsonElement(nameof(LastName))]
        public string LastName { get; set; }
        [BsonElement(nameof(ContactName))]
        public string ContactName { get; set; }

        [BsonElement(nameof(MobilePhone))]
        public string MobilePhone { get; set; }
        [BsonElement(nameof(Fax))]
        public string Fax { get; set; }
        [BsonElement(nameof(Email))]
        public string Email { get; set; }
        [BsonElement(nameof(Location))]
        public Location Location { get; set; }
        public bool IsActive
        {
            get; set;
        } = true;
        public DateTime DateOfBirth
        {
            get { return _DateOfBirth; }
            set
            {

                if (_DateOfBirth.Equals(value))
                    return;

                _DateOfBirth = value;
            }
        }

    }
}
