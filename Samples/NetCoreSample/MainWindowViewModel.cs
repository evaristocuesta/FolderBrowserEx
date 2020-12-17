using FolderBrowserEx;
using MVVMBase;
using System.Windows.Forms;
using System.Windows.Input;

namespace NetCoreSample
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly IFolderBrowserDialog _folderBrowserDialog;
        private string _result;

        public MainWindowViewModel(IFolderBrowserDialog folderBrowserDialog)
        {
            _folderBrowserDialog = folderBrowserDialog;
            ShowFolderBrowserSingleSelectionCommand = new Command(
                ShowFolderBrowserSingleSelectionCommandExecute,
                ShowFolderBrowserSingleSelectionCommandCanExecute);

            ShowFolderBrowserMultipleSelectionCommand = new Command(
                ShowFolderBrowserMultipleSelectionCommandExecute,
                ShowFolderBrowserMultipleSelectionCommandCanExecute);
        }

        public ICommand ShowFolderBrowserSingleSelectionCommand { get; private set; }

        public ICommand ShowFolderBrowserMultipleSelectionCommand { get; private set; }

        public string Result
        {
            get { return _result; }
            set { _result = value; OnPropertyChanged(); }
        }

        private bool ShowFolderBrowserSingleSelectionCommandCanExecute()
        {
            return true;
        }

        private void ShowFolderBrowserSingleSelectionCommandExecute()
        {
            _folderBrowserDialog.Title = "Select a folder";
            _folderBrowserDialog.InitialFolder = @"C:\";
            _folderBrowserDialog.AllowMultiSelect = false;
            if (_folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Result += $"{_folderBrowserDialog.SelectedFolder}\n";
            }
        }

        private bool ShowFolderBrowserMultipleSelectionCommandCanExecute()
        {
            return true;
        }

        private void ShowFolderBrowserMultipleSelectionCommandExecute()
        {
            _folderBrowserDialog.Title = "Select multiple folders";
            _folderBrowserDialog.InitialFolder = @"C:\";
            _folderBrowserDialog.AllowMultiSelect = true;
            if (_folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                foreach (var folder in _folderBrowserDialog.SelectedFolders)
                    Result += $"{folder}\n";
            }
        }
    }
}
