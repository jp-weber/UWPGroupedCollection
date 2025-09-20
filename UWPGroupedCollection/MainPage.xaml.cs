using CommunityToolkit.Mvvm.DependencyInjection;
using UWPGroupedCollection.Services;
using UWPGroupedCollection.ViewModel;
using Windows.UI.Xaml.Controls;

namespace UWPGroupedCollection
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a <see cref="Frame">.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public ContactsViewModel ViewModel { get; } = new ContactsViewModel();
        public MainPage()
        {
            InitializeComponent();
            //DataContext = Ioc.Default.GetRequiredService<ContactsViewModel>();
            this.Loaded += MainPage_Loaded;
        }

        private void MainPage_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            ViewModel.LoadContactsCommand.Execute(null);
        }
    }
}
