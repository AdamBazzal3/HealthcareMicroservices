using System.ComponentModel;

namespace HealthcareUI.Utils
{

    public class Person
    {
        private DateTime _DateOfBirth;


        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string ContactName { get; set; }


        public string MobilePhone { get; set; }

        public string Fax { get; set; }

        public string Email { get; set; }

        public Location Location { get; set; } = new();

        public bool IsActive
        {
            get; set;
        } = true;
        public DateTime DateOfBirth
        {
            get { return _DateOfBirth; }
            set {

                if (_DateOfBirth.Equals(value))
                    return;

                _DateOfBirth = value;
            }
        }

    }
}
