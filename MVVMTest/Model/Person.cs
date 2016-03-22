using System;

namespace MVVMTest.Model
{
    public class Person
    {
        #region NotifyPropertiesChanged

        public delegate void NamePropertiesChangedEventHandler();
        public event NamePropertiesChangedEventHandler NamePropertiesChanged;

        private void NotifyNamePropertiesChanged()
        {
            if (NamePropertiesChanged != null)
            {
                NamePropertiesChanged();
            }
        }

        #endregion


        private string _firstName;
        private string _lastName;
        private string _fullName;
       

        /// <summary>
        /// Default Constructor of Person.
        /// </summary>
        public Person()
        {
        }

        public void vInitialize(string firstName, string lastName)
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
                    NotifyNamePropertiesChanged();

                    // Update FullName
                    FullName = _firstName + " " + _lastName;
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
                    NotifyNamePropertiesChanged();

                    // Update FullName
                    FullName = _firstName + " " + _lastName;
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
            private set
            {
                if (_fullName != value)
                {
                    Console.WriteLine("Model's FullName changed");
                    _fullName = value;
                    NotifyNamePropertiesChanged();
                }
            }
        }
    }
}
