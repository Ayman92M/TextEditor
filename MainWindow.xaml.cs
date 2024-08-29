using Microsoft.Win32;
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
        private MyFile myfile;

        public MainWindow()
        {
            InitializeComponent();
            myfile = new MyFile(this);
            this.Title = myfile.FileName;
        }


        /*
         Handles menu commands (such as New, Open, Save, SaveAs, and Close.)
         Extracts the command's name to perform different actions based on the identified command's name.
         
        
        -   If changes have been made {if (myfile.HasChanged)} 
            open a dialog box that asks the user if they want to save the file before proceeding.

            If the user does not choose to cancel, save the changes. Then create/Open a new file.
            But if the user chooses to cancel, abort the command. 

        -   Otherwise, just create/open a file.

        Same logic for MainWindow_Closing()
         */
        private void MenuItem_Click(object sender, ExecutedRoutedEventArgs e)
        {

            if (e.Command is RoutedCommand routedCommand)
            {
                string commandName = routedCommand.Name;

                //MessageBox.Show($" '{commandName}' executed!");
                switch (commandName)
                {
                    case "New":
                        if (myfile.HasChanged)
                        {
                            MessageBoxResult userChoice = myfile.SaveChanges();
                            if (userChoice == MessageBoxResult.Cancel)
                                break;

                        }

                        myfile = new MyFile(this);
                        break;


                    case "Open":
                        if (myfile.HasChanged)
                        {
                            MessageBoxResult userChoice = myfile.SaveChanges();
                            if (userChoice == MessageBoxResult.Cancel)
                                break;
                        }

                        string? newPath = myfile.OpenFile();
                        if (newPath != null)
                            myfile = new MyFile(this, myfile.GetFileNameFromPath(newPath), newPath);

                        break;


                    case "Save":
                        myfile.SaveFile();
                        break;


                    case "SaveAs":
                        myfile.save_as_file();
                        break;


                    case "Help":
                        MessageBox.Show("Version 3!", "TextEditor Version",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                        break;


                    case "Close":
                        Close();
                        break;

                }
            }

        }

        

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (myfile.HasChanged)
            {
                MessageBoxResult userChoice = myfile.SaveChanges();
                if (userChoice == MessageBoxResult.Cancel)
                    e.Cancel = true;
            }
        }

        /*
        ActionListener for the TextBox.
        -   Calls "GetCountChar()" to get counts of characters, words, and lines
        -   Marks that the file has changed 
        -   Updates the title and status message for the file.
         */
        private void MyTextBoxHasChanged(object sender, TextChangedEventArgs e)
        {
            TextBox txt_box = (TextBox)sender;
            string[] result = myfile.GetCountChar(txt_box);

            CharCountExcludingWhitespace.Content = result[0];
            TeckenCountIncludeWhitespace.Content = result[1];
            WordCount.Content = result[2];
            LineCount.Content = result[3];


            myfile.HasChanged = true;
            myfile.UpdateTiltle("*" + myfile.FileName);
            myfile.UpdateStatusBar("Satus: Unsaved changes.", MyFile.StatusIcon.Attention);
        }

        private void ShowMessage(string message, string title)
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // Handles the event when a drag-and-drop operation starts.
        private void TextBox_DragEnter(object sender, DragEventArgs e)
        {
            e.Handled = true;

            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effects = DragDropEffects.Copy;

            else
                e.Effects = DragDropEffects.None;

            // Ställ in muspekaren baserat på den valda effekten. 
            Mouse.SetCursor(e.Effects == DragDropEffects.Copy ? Cursors.Cross : Cursors.No);

        }

        // Set the mouse cursor based on the selected effect.
        private void TextBox_Drop(object sender, DragEventArgs e)
        {

            // try to avoid crashes if the file format is invalid, contains something invalid,
            // or if the user doesn't have permissions to read it.
            try
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    string[] _files = (string[])e.Data.GetData(DataFormats.FileDrop);

                    // Ensure that it is only one file and that it is a ".txt"
                    if (_files.Length == 1 && _files[0].EndsWith(".txt"))
                    {
                        string fileContent = System.IO.File.ReadAllText(_files[0]);

                        // Case ControlKey
                        if (e.KeyStates == DragDropKeyStates.ControlKey)
                            MyTextBox.Text += fileContent;

                        // Case ShiftKey
                        else if (e.KeyStates == DragDropKeyStates.ShiftKey)
                        {
                            int insertionIndex = MyTextBox.CaretIndex;
                            MyTextBox.Text = MyTextBox.Text.Insert(insertionIndex, fileContent);
                            MyTextBox.CaretIndex = insertionIndex + fileContent.Length;
                        }

                        // Default Case
                        else
                        {
                            if (myfile.HasChanged)
                            {
                                MessageBoxResult userChoice = myfile.SaveChanges();
                                if (userChoice != MessageBoxResult.Cancel)
                                {
                                    MyTextBox.Text = fileContent;
                                    myfile = new MyFile(this, myfile.GetFileNameFromPath(_files[0]), _files[0]);
                                }
                            }
                            else
                            {
                                MyTextBox.Text = fileContent;
                                myfile = new MyFile(this, myfile.GetFileNameFromPath(_files[0]), _files[0]);
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            e.Handled = true;
        }
    }


    public class MyFile
    {
        private MainWindow mainWindow;
        private const string DEFAULT_FILE_NAME = "dok1.txt";

        public string FileName { get; set; }
        public string FileFullPath { get; set; }
        public bool HasChanged { get; set; }
        public bool Saved { get; set; }



        /*
       * Constructor for the MyFile class
       * -    initializes its properties and updates the title and status in the main window.
       *
       * {mainWindow}
       * -    Reference to the main window where the MyFile instance will be used. 
       *      To access its variables.
       */
        public MyFile(MainWindow mainWindow)
        {

            this.mainWindow = mainWindow;

            FileName = DEFAULT_FILE_NAME;
            FileFullPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), FileName);

            //Marks that the file is --not saved-- and has not changed. In other words, a new file.
            Saved = false;
            HasChanged = false;
            mainWindow.MyTextBox.Text = string.Empty;

            UpdateStatusBar("Status: Ready!", MyFile.StatusIcon.Ready);
            UpdateTiltle(FileName);

        }

        public MyFile(MainWindow mainWindow, string fileName, string fullPath)
        {
            this.mainWindow = mainWindow;

            FileName = fileName;
            FileFullPath = fullPath;

            //Marks that the file is --saved-- and has not changed since the last save. In other words, an existing file.
            Saved = true;
            HasChanged = false;
            UpdateTiltle(FileName);
            UpdateStatusBar("Status: Ready!", MyFile.StatusIcon.Ready);
        }


        public void UpdateTiltle(String _fileName)
        {
            mainWindow.Title = _fileName;
        }

        public enum StatusIcon
        {
            Ready,
            Attention,
            Check,
            Error
        }

        public void UpdateStatusBar(string status, StatusIcon icon)
        {
            mainWindow.Status.Content = status;
            string iconFileName = icon switch
            {
                StatusIcon.Ready => "ready2.png",
                StatusIcon.Attention => "Attention.png",
                StatusIcon.Check => "Check.png",
                StatusIcon.Error => "Error.png",
                _ => throw new ArgumentOutOfRangeException(nameof(icon), icon, null)
            };

            mainWindow.StatusIcon.Source = new BitmapImage(new Uri($"/my_icon/{iconFileName}", UriKind.Relative));
        }


        //Opens a dialog box to select and open a file.
        //Reads the content of the file and displays it in the TextBox.
        //Returns the path to the selected file if the user opens a file, to get the file name and address.
        //If {null} is returned => the command to open is canceled. If userChoice == Cancel OR Exception
        public string? OpenFile()
        {

            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Text files (*.txt)|*.txt";

                if (openFileDialog.ShowDialog() == true)
                {

                    string fileContent = System.IO.File.ReadAllText(openFileDialog.FileName);
                    mainWindow.MyTextBox.Text = fileContent;
                    return openFileDialog.FileName;

                }
                return null;
            }
            catch (Exception ex)
            {
                //If it failed to read from the file
                MessageBox.Show($"OpenFile(). An error occurred: {ex.Message}",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }

        }

        public MessageBoxResult SaveChanges()
        {

            MessageBoxResult result = MessageBox.Show("Do you want to save changes?",
                "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

            switch (result)
            {

                case MessageBoxResult.Yes:
                    SaveFile();
                    // SaveFile(); changes { Saved to True } if it succeeds.

                    if (Saved)
                        return result;

                    // If the user changed their mind and clicked cancel instead of saving.
                    // To abort the ongoing command, return Cancel.
                    else
                        return MessageBoxResult.Cancel;



                case MessageBoxResult.No:
                    return result;



                case MessageBoxResult.Cancel:
                    return result;



                default:
                    return result;
            }
        }

        public void SaveFile()
        {
            if (Saved)
                save_existing_file();

            else
                save_as_file();
        }

        public void save_as_file()
        {

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.OverwritePrompt = true;
            saveFileDialog.Filter = "Text files (*.txt)|*.txt";
            saveFileDialog.Title = "Save As";
            if (saveFileDialog.ShowDialog() == true)
            {
                // Update FileFullPath with the selected file's path
                FileFullPath = saveFileDialog.FileName;

                // Save the file with the new name/location.
                save_existing_file();
            }

        }

        public void save_existing_file()
        {
            // To avoid crashes during the save process
            // In case the user does not have permissions to write to the specified file or location.
            try
            {
                System.IO.File.WriteAllText(FileFullPath, mainWindow.MyTextBox.Text);

                // Update FileName with the new file name based on the path/location.
                FileName = GetFileNameFromPath(FileFullPath);

                // Mark that the file is saved and has not changed since the last save
                Saved = true;
                HasChanged = false;

                UpdateTiltle(FileName);
                UpdateStatusBar("Status: File saved successfully.", StatusIcon.Check);


            }
            catch (Exception e)
            {
                throw new Exception("An error occurred while saving the file.", e);
            }
        }


        /*
        Calculates the number of characters based on the TextBox content 
        and creates and returns an array with the result.
        */

        public string GetFileNameFromPath(string _fullPath)
        {
            return System.IO.Path.GetFileName(_fullPath);
        }

        /*
        Beräknar antalet tecken baserat på TextBox-innehållet 
        Samt skapar och returnerar en array med resultatet.
         */
        public string[] GetCountChar(TextBox txt_box)
        {

            int charCountWithoutSpaces = txt_box.Text.Count(c => char.IsLetterOrDigit(c)
                                            || (!char.IsWhiteSpace(c) && c != '\n'));

            int charCountWithSpaces = charCountWithoutSpaces + (txt_box.Text.Split(' ').Length - 1);

            int wordCount = txt_box.Text.Split(new char[] { ' ', '\t', '\n', '\r'
            }, StringSplitOptions.RemoveEmptyEntries).Length;

            int lineCount = 0;
            if (txt_box.Text.Length >= 1)
                lineCount = txt_box.Text.Split('\n').Length;

            string[] result =
                {   $"{charCountWithSpaces} Teckens include whitespace.",
                    $"{charCountWithoutSpaces} Teckens excluding whitespace.",
                    $"{wordCount} Words.",
                    $"{lineCount} Lines."
                };
            return result;
        }

    }
}