using System.Windows.Forms;

namespace FolderBrowserEx
{
    public interface IFolderBrowserDialog

    {
        string InitialFolder { get; set; }
        string DefaultFolder { get; set; }
        string SelectedFolder { get; set; }
        DialogResult ShowDialog();
        DialogResult ShowDialog(IWin32Window owner);
        void Dispose();
    }
}
