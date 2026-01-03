using ReviewByWpf.Services;
using ReviewByWpf.ViewModles;
using ReviewByWpf.Views;
using System.Configuration;
using System.Data;
using System.Windows;

namespace ReviewByWpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // MySql
            IReviewRepository repo = new MySqlReviewRepository("Server=localhost;Database=db_review;Uid=db_review_user;Pwd=1234;");

            // SQL Server로 바꾸려면 아래 줄만 교체
            // IReviewRepository repo = new SqlServerReviewRepository("Server=.;Database=db_review;Integrated Security=True;");

            var mainWindow = new MainWindow();
            mainWindow.DataContext = new ReviewViewModel(repo);
            mainWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            mainWindow.Show();
        }
    }

}
