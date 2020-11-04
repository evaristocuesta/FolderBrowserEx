using FolderBrowserEx;
using MVVMBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NetFrameworkSample
{
    public class MainWindowViewModel : ViewModelBase
    {
        private IFolderBrowserDialog _folderBrowserDialog;
        private string _result;

        public MainWindowViewModel(IFolderBrowserDialog folderBrowserDialog)
        {
            _folderBrowserDialog = folderBrowserDialog;
            ShowFolderBrowserCommand = new Command(ShowFolderBrowserCommandExecute, ShowFolderBrowserCommandCanExecute);
        }

        public ICommand ShowFolderBrowserCommand { get; private set; }

        public string Result
        { 
            get { return _result; }
            set { _result = value; OnPropertyChanged(); }
        }

        private bool ShowFolderBrowserCommandCanExecute()
        {
            return true;
        }

        private void ShowFolderBrowserCommandExecute()
        {
            if (_folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Result += $"{_folderBrowserDialog.SelectedFolder}\n";
            }
        }
    }
}
