using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Xml.Linq;
using Kudu.Services;
using Librarian;
using RestSharp;
using Simian.Mvvm;

namespace Kudu.ViewModels
{
    public class SearchViewModel : ViewModelBase
    {
        public SearchViewModel()
        {
            this.SearchResults = new ObservableCollection<SearchResultViewModel>();
        }

        #region public string SearchQuery
        /// <summary>
        /// Collection of reviews of this book by users.
        /// </summary>
        private string _SearchQuery;
        public string SearchQuery
        {
            get
            {
                return _SearchQuery;
            }
            set
            {
                if (_SearchQuery != value)
                {
                    _SearchQuery = value;
                    OnPropertyChanged();
                }
            }
        }
        #endregion

        private bool _isSearching;
        public bool IsSearching
        {
            get { return _isSearching; }
            set
            {
                if (_isSearching != value)
                {
                    _isSearching = value;
                    UpdateSearchVisibility();
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<SearchResultViewModel> SearchResults { get; private set; }

        private SearchResultViewModel _selectedItem;
        public SearchResultViewModel SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (_selectedItem != value)
                {
                    _selectedItem = value;

                    if (_selectedItem != null)
                    {
                        var parameters = new Dictionary<string, string>
                        {
                            {"id", SelectedItem.BookId}
                        };

                        NavigationController.Current.NavigateTo(View.Book, parameters);
                    }
                }
            }
        }

        private string _statusMessage;
        public string StatusMessage
        {
            get { return _statusMessage; }
            set
            {
                if (_statusMessage != value)
                {
                    _statusMessage = value;
                    OnPropertyChanged("StatusMessage");
                }
            }
        }

        private Visibility _statusMessageVisibility = Visibility.Collapsed;
        public Visibility StatusMessageVisibility
        {
            get { return _statusMessageVisibility; }
            set
            {
                if (_statusMessageVisibility != value)
                {
                    _statusMessageVisibility = value;
                    OnPropertyChanged();
                }
            }
        }

        private Visibility _searchResultsVisibility = Visibility.Collapsed;
        public Visibility SearchResultsVisibility
        {
            get { return _searchResultsVisibility; }
            set
            {
                if (_searchResultsVisibility != value)
                {
                    _searchResultsVisibility = value;
                    OnPropertyChanged();
                }
            }
        }

        public void Search()
        {
            if (!String.IsNullOrEmpty(SearchQuery))
            {
                IsSearching = true;
                SearchResults.Clear();
                GoodreadsClient.Current.Search(SearchQuery, SearchCallback);
            }
        }

        private void UpdateSearchVisibility()
        {
            if (IsSearching)
            {
                StatusMessage = "Searching for " + SearchQuery + "...";
                StatusMessageVisibility = Visibility.Visible;
                SearchResultsVisibility = Visibility.Collapsed;
            }
            else
            {
                StatusMessageVisibility = Visibility.Collapsed;
                SearchResultsVisibility = Visibility.Visible;
            }
        }

        private void SearchCallback(IRestResponse response)
        {
            Deployment.Current.Dispatcher.BeginInvoke(
                () =>
                {
                    IsSearching = false;
                });

            XDocument doc = XDocument.Parse(response.Content);

            var resultList = (from work in doc.Descendants("work")
                              select new SearchResultViewModel
                              {
                                  BookTitle = (string)work.Element("best_book").Element("title"),
                                  Author = (string)work.Element("best_book").Element("author").Element("name"),
                                  BookId = (string)work.Element("best_book").Element("id")
                              }).ToList();

            foreach (var result in resultList)
            {
                var r = result;
                Deployment.Current.Dispatcher.BeginInvoke(
                () =>
                {
                    SearchResults.Add(r);
                });
            }
        }
    }
}
