using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using Kudu.Common;
using Kudu.Services;
using Librarian;
using Simian.Mvvm;

namespace Kudu.ViewModels
{
    public class UpdateProgressViewModel : ViewModelBase
    {
        public string BookId { get; private set; }

        private string _pageNumber;
        public string PageNumber
        {
            get
            {
                return _pageNumber;
            }
            set
            {
                if (_pageNumber != value)
                {
                    _pageNumber = value;
                    OnPropertyChanged("PageNumber");
                }
            }
        }

        private string _comment;
        public string Comment
        {
            get
            {
                return _comment;
            }
            set
            {
                if (_comment != value)
                {
                    _comment = value;
                    OnPropertyChanged("Comment");
                }
            }
        }

        public override void Initialize(IDictionary<string, string> parameters)
        {
            string id;
            if (!parameters.TryGetValue("id", out id))
            {
                throw new ArgumentException("Book ID is missing from parameters", "id");
            }

            BookId = id;
        }
        
        private ICommand _saveCommand;
        public ICommand SaveCommand
        {
            get
            {
                if (_saveCommand == null)
                {
                    _saveCommand = new DelegateCommand(
                        p =>
                        {
                            GoodreadsClient.Current.UpdateBookPage(
                                BookId,
                                PageNumber,
                                Comment,
                                r =>
                                {
                                    MessageBox.Show("Status updated");
                                    NavigationController.Current.GoBack();
                                }
                                );
                        },
                        p =>
                        {
                            return true;
                        });
                }

                return _saveCommand;
            }
        }
    }
}
