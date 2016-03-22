using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using MVVMTest.Model;

namespace MVVMTest.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
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


        // Model 
        private Person _customer;

        // Presentation data
        private string _firstName;
        private string _lastName;
        private string _fullName;
        private string _welcomeMessage;

        // Presentation commands
        public DelegateCommand SaveCommand { get; private set; }
        
        /// <summary>
        /// Default Constructor for ViewModel
        /// </summary>
        public MainViewModel()
        {
            _customer = new Person();
            bindData();
            _customer.initialize("Hermione", "Granger");

            SaveCommand = new DelegateCommand(SaveExecute, SaveCanExecute);
        }

        #region Getters/Setters
        /// <summary>
        /// Get/Set Customer
        /// </summary>
        public Person Customer
        {
            get
            {
                return _customer;
            }
            set
            {
                if (_customer != value)
                {
                    Console.WriteLine("ViewModel's Customer changed");
                    _customer = value;
                    NotifyPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Get/Set FirstName
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
                    Console.WriteLine("ViewModel's FirstName changed");
                    _firstName = value;
                    _customer.FirstName = value;                  
                    NotifyPropertyChanged();
                  
                    if (SaveCommand != null)
                    {
                        // Reevaluate if save command can execute
                        Console.WriteLine("SaveCommand canExecute changed");
                        SaveCommand.RaiseCanExecuteChanged();
                    }
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
                    Console.WriteLine("ViewModel's LastName changed");
                    _lastName = value;
                    _customer.LastName = value;
                    NotifyPropertyChanged();

                    if (SaveCommand != null)
                    {
                        // Reevaluate if save command can execute
                        Console.WriteLine("SaveCommand canExecute changed");
                        SaveCommand.RaiseCanExecuteChanged();
                    }
                }
            }
        }

        /// <summary>
        /// Get/Set LastName
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
                    Console.WriteLine("ViewModel's FullName changed");
                    _fullName = value;
                    _customer.FullName = value;
                    NotifyPropertyChanged();
                }
            }
        }


        /// <summary>
        /// Get/Set WelcomeMessage
        /// </summary>
        public string WelcomeMessage
        {
            get
            {
                return _welcomeMessage;
            }
            set
            {
                if (_welcomeMessage != value)
                {
                    Console.WriteLine("ViewModel's WelcomeMessage changed");
                    _welcomeMessage = value;
                    NotifyPropertyChanged();
                }
            }
        }

        #endregion


        private void updateProperties(object sender, PropertyChangedEventArgs arg)
        {
            switch (arg.PropertyName)
            {
                case "FirstName":
                    FirstName = _customer.FirstName;
                    break;
                case "LastName":
                    LastName = _customer.LastName;
                    break;
                case "FullName":
                    FullName = _customer.FullName;
                    WelcomeMessage = "Welcome " + FullName;
                    break;
            }
        }

        private void bindData()
        {
            _customer.PropertyChanged += updateProperties;
        }

        private void SaveExecute(object param)
        {
            Console.WriteLine("Save customer info");
        }

        private bool SaveCanExecute(object param)
        {
            return (!string.IsNullOrWhiteSpace(FirstName)) && (!string.IsNullOrWhiteSpace(LastName));
        }
    }
}
