using ReviewByWpf.ViewModles;
using System.Windows;

namespace ReviewByWpf.Views
{
    /// <summary>
    /// InsertWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class InsertWindow : Window
    {
        public InsertWindow()
        {
            InitializeComponent();

            Loaded += (s, e) =>
            {
                if (DataContext is InsertViewModel vm)
                {
                    vm.Saved += () =>
                    {
                        // MainWindow에서 ShowDialog() == true로 확인 가능
                        DialogResult = true;
                        Close();
                    };
                }
            };
        }
    }
}
