using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FluentNewsApp.Models
{
    public sealed class ArticleFeed : INotifyPropertyChanged
    {
        private string _category;
        private List<Article> _articles;
        private bool _isLoading;
        private bool _hasError;

        public string Category
        {
            get
            {
                return _category;
            }
            set
            {
                if (_category != value)
                {
                    _category = value;
                    OnPropertyChanged(nameof(Category));
                }
            }
        }

        public List<Article> Articles
        {
            get
            {
                return _articles;
            }
            set 
            {
                if (_articles != value)
                {
                    _articles = value;
                    OnPropertyChanged(nameof(Articles));
                }
            }
        }
        public bool IsLoading
        {
            get
            {
                return _isLoading;
            }
            set
            {
                if (_isLoading != value)
                {
                    _isLoading = value;
                    OnPropertyChanged(nameof(IsLoading));
                }
            }
        }
        public bool HasError
        {
            get
            {
                return _hasError;
            }
            set
            {
                if (_hasError != value)
                {
                    _hasError = value;
                    OnPropertyChanged(nameof(HasError));
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged; 
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
