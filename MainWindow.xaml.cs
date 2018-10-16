using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.IO;
using Microsoft.Win32;

namespace MyWordPad
{
    public partial class MainWindow : Window
    {
        bool TextChangedFlag;

        public MainWindow()
        {
            InitializeComponent();

            TextChangedFlag = false;

            this.Closing += MainWindowClosing;

            // Setting the default font (12)
            fontSizeComboBox.SelectedIndex = 4;

            // Setting user selected language option
            englishCheck.Opacity = MyWordPad.Properties.Settings.Default.DefaultOpacityEng;
            russianCheck.Opacity = MyWordPad.Properties.Settings.Default.DefaultOpacityRus;
            textBox.Language = System.Windows.Markup.XmlLanguage.GetLanguage(MyWordPad.Properties.Settings.Default.DefaultLanguage.IetfLanguageTag);
        }

        // Display information in StatusBar
        private void DefaultStatusText(object sender, MouseEventArgs e)
        {
            statusText.Text = this.FindResource("defaultStatusText").ToString();
        }
      
        private void OpenStatusText(object sender, MouseEventArgs e)
        {
            statusText.Text = this.FindResource("openStatusText").ToString();
        }

        private void SaveStatusText(object sender, MouseEventArgs e)
        {
            statusText.Text = this.FindResource("saveStatusText").ToString();
        }

        private void ExitStatusText(object sender, MouseEventArgs e)
        {
            statusText.Text = this.FindResource("exitStatusText").ToString();
        }

        private void CopyStatusText(object sender, MouseEventArgs e)
        {
            statusText.Text = this.FindResource("copyStatusText").ToString();
        }

        private void PasteStatusText(object sender, MouseEventArgs e)
        {
            statusText.Text = this.FindResource("pasteStatusText").ToString();
        }

        private void CutStatusText(object sender, MouseEventArgs e)
        {
            statusText.Text = this.FindResource("cutStatusText").ToString();
        }

        private void LanguageStatusText(object sender, MouseEventArgs e)
        {
            statusText.Text = this.FindResource("languageStatusText").ToString();
        }

        private void AboutStatusText(object sender, MouseEventArgs e)
        {
            statusText.Text = this.FindResource("aboutStatusText").ToString();
        }

        // Loading text from a file
        private void CanOpenExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void OpenExecute(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = this.FindResource("typeDocument").ToString() + " |*.txt";
            if (true == openFileDialog.ShowDialog())
            {
                string dataFromFile = File.ReadAllText(openFileDialog.FileName);
                textBox.Text = dataFromFile;
            }

            // Deactivating the text change flag when loading text from a file
            TextChangedFlag = false;
        }

        // Saving text to a file
        private void CanSaveExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void SaveExecute(object sender, ExecutedRoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = this.FindResource("typeDocument").ToString() + " |*.txt";
            if (true == saveFileDialog.ShowDialog())
            {
                File.WriteAllText(saveFileDialog.FileName, textBox.Text);
            }

            // Deactivating the text change flag when saving text to a file
            TextChangedFlag = false;
        }

        private void AppExit(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        // Spell Check Assistant
        private void CheckSpellingHints(object sender, RoutedEventArgs e)
        {
            string spellingHints = string.Empty;

            SpellingError error = textBox.GetSpellingError(textBox.CaretIndex);
            if (error != null)
                foreach (string s in error.Suggestions)
                {
                    spellingHints += string.Format("{0}\n", s);
                }
            variantSpellingHints.Content = spellingHints;
            expandr.IsExpanded = true;
        }

        // Opening information window about programm
        private void DisplayInformationWindow(object sender, RoutedEventArgs e)
        {
            InfoWindow infoWindow = new InfoWindow();
            infoWindow.Owner = this;
            infoWindow.ShowDialog();
        }

        // Handling event of a font selection for user text
        private void SelectFontSize(object sender, SelectionChangedEventArgs e)
        {
            textBox.FontSize = int.Parse((fontSizeComboBox.SelectedItem as ComboBoxItem).Content.ToString());
        }

        // Handling event of a selection English localization
        private void EnglishLanguage(object sender, RoutedEventArgs e)
        {
            // Visual demonstration of the selected application language
            englishCheck.Opacity = 1;
            russianCheck.Opacity = 0;
            MyWordPad.Properties.Settings.Default.DefaultOpacityEng = 1;
            MyWordPad.Properties.Settings.Default.DefaultOpacityRus = 0;

            // Change the current application language to English
            App.Language = App.Languages[0];
            textBox.Language = System.Windows.Markup.XmlLanguage.GetLanguage("en-US");

            // Changing localization of a default text of a status bar
            statusText.Text = "Ready";
        }

        // Handling event of a selection Russian localization
        private void RussianLanguage(object sender, RoutedEventArgs e)
        {
            englishCheck.Opacity = 0;
            russianCheck.Opacity = 1;
            MyWordPad.Properties.Settings.Default.DefaultOpacityEng = 0;
            MyWordPad.Properties.Settings.Default.DefaultOpacityRus = 1;

            // Change the current application language to Russian
            App.Language = App.Languages[1];
            textBox.Language = System.Windows.Markup.XmlLanguage.GetLanguage("ru-RU");

            statusText.Text = "Готово";
        }

        private void TextChanged(object sender, TextChangedEventArgs e)
        {
            TextChangedFlag = true;
        }

        // Warning message about unsaved data at the time of closing the application
        private void MainWindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (TextChangedFlag)
            {
                MessageBoxResult result = MessageBox.Show("Do you want to close without saving?", "MyWordPad", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.No)
                    e.Cancel = true;
            }
        }
    }
}
