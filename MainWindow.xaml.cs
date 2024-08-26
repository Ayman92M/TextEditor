using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TextEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MenuItem_Click(object sender, ExecutedRoutedEventArgs e)
        {

            if (e.Command is RoutedCommand routedCommand)
            {
                string commandName = routedCommand.Name;
                MessageBox.Show($" '{commandName}' executed!");

            }

        }

        private void TextBox_DragEnter(object sender, DragEventArgs e)
        {

        }

        private void TextBox_Drop(object sender, DragEventArgs e)
        {

        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void MyTextBoxHasChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void ShowMessage(string message, string title)
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }


    public class MyFile
    {

    }
}