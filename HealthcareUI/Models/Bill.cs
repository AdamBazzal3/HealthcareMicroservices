namespace HealthcareUI.Models
{

    public class Bill 
    {




        private bool _IsActive;

        public Bill()
            
        {


        }
        public string? Id
        {
            get; set;
        }

        //public override void AfterConstruction()
        //{
        //    base.AfterConstruction();
        //    IsActive = true;
        //    PaymentStatus = BillStatus.Initiated;
        //}


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
            set {
                if (value == null) return;
                if (value.Equals(_Doctor))
                    return;

                _Doctor = value;
            }
        }

        private string _ServiceDetails;

        public string ServiceDetails
        {
            get { return _ServiceDetails; }
            set
            {
                if (value == null) return;
                if (value.Equals(_ServiceDetails))
                    return;

                _ServiceDetails = value;
            }
        }

        private decimal _Amount;

        public decimal Amount
        {
            get { return _Amount; }
            set {
                if (value == null) return;
                if (value.Equals(_Amount))
                    return;

                _Amount = value;
            }
        }

        private BillStatus _PaymentStatus;
        public BillStatus PaymentStatus
        {
            get { return _PaymentStatus; }
            set {
                if (value == null) return;
                if (value.Equals(_PaymentStatus))
                    return;

                _PaymentStatus = value;
            }
        }

        private string _InsuranceClaims;
        public string InsuranceClaims
        {
            get { return _InsuranceClaims; }
            set {
                if (value == null) return;
                if (value.Equals(_InsuranceClaims))
                    return;

                _InsuranceClaims = value;
            }
        }

    }
}
