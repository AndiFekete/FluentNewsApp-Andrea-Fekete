using FluentNewsApp.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace FluentNewsApp.Views
{
    /// <summary>
    /// Interaction logic for CategoryControl.xaml
    /// </summary>
    public partial class CategoryControl : UserControl
    {
        public static DependencyProperty CategoryProperty =
            DependencyProperty.Register("Category", typeof(string), typeof(CategoryControl), new PropertyMetadata("Default category name"));
        public static DependencyProperty ArticlesProperty =
            DependencyProperty.Register("Articles", typeof(List<Article>), typeof(CategoryControl), new PropertyMetadata(new List<Article>()));
        public static DependencyProperty HasErrorProperty =
            DependencyProperty.Register("HasError", typeof(bool), typeof(CategoryControl), new PropertyMetadata(false));

        public string Category
        {
            get { return (string)GetValue(CategoryProperty); }
            set { SetValue(CategoryProperty, value); }
        }

        public List<Article> Articles
        {
            get { return (List<Article>)GetValue(ArticlesProperty); }
            set { SetValue(ArticlesProperty, value); }
        }

        public bool HasError
        {
            get { return (bool)GetValue(HasErrorProperty); }
            set { SetValue(HasErrorProperty, value); }
        }

        public CategoryControl()
        {
            InitializeComponent();
            Binding categoryBinding = new Binding("Category")
            {
                Source = this
            };
            Binding articlesBinding = new Binding("Articles")
            {
                Source = this
            };
            Binding hasErrorBinding = new Binding("HasError")
            {
                Source = this
            };
        }
    }
}
