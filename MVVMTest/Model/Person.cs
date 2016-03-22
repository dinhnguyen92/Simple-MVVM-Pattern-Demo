using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MVVMTest.Model
{
    public class Person : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        // Model data
        private string _firstName;
        private string _lastName;
        private string _fullName;      

        /// <summary>
        /// Default Constructor of Person.
        /// </summary>
        public Person()
        {
            // Subscribe the updateFullName method to the PropertyChanged event
            // Each time FirstName and/or LastName change, the PropertyChanged event is raised
            // And FullName is updated correspondingly
            PropertyChanged += new PropertyChangedEventHandler(updateFullName);
        }

        public void initialize(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }

        /// <summary>
        /// Get/Set FirstName.
        /// </summary>
        public string FirstName
        {
            get
            {
                return _firstName;
            }
            set
            {
                if (_firstName != value)
                {
                    Console.WriteLine("Model's FirstName changed");
                    _firstName = value;
                    NotifyPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Get/Set LastName
        /// </summary>
        public string LastName
        {
            get
            {
                return _lastName;
            }
            set
            {
                if (_lastName != value)
                {
                    Console.WriteLine("Model's LastName changed");
                    _lastName = value;
                    NotifyPropertyChanged();           
                }
            }
        }      

        /// <summary>
        /// Get/Set FullName.
        /// </summary>
        public string FullName
        {
            get
            {
                return _fullName;
            }
            set
            {
                if (_fullName != value)
                {
                    Console.WriteLine("Model's FullName changed");
                    _fullName = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private void updateFullName(object sender, PropertyChangedEventArgs arg)
        {
            // Update FullName
            FullName = _firstName + " " + _lastName;
        }
    }
}
