using System.Collections.Generic;
using System.Windows.Forms;

namespace FolderBrowserEx
{
    public interface IFolderBrowserDialog
    {
        /// <summary>
        /// Gets/sets the title of the dialog
        /// </summary>
        string Title { get; set; }

        /// <summary>
        /// Gets/sets folder in which dialog will be open.
        /// </summary>
        string InitialFolder { get; set; }

        /// <summary>
        /// Gets/sets directory in which dialog will be open 
        /// if there is no recent directory available.
        /// </summary>
        string DefaultFolder { get; set; }

        /// <summary>
        /// Gets selected folder when AllowMultiSelect is false
        /// </summary>
        string SelectedFolder { get; }

        /// <summary>
        /// Gets selected folders when AllowMultiSelect is true.
        /// </summary>
        List<string> SelectedFolders { get; }

        bool AllowMultiSelect { get; set; }

        /// <summary>
        /// Shows the folder browser dialog with a the default owner
        /// </summary>
        /// <returns>
        /// System.Windows.Forms.DialogResult.OK if the user clicks OK in the dialog box;
        /// otherwise, System.Windows.Forms.DialogResult.Cancel.
        /// </returns>
        DialogResult ShowDialog();

        /// <summary>
        /// Shows the folder browser dialog with a the specified owner
        /// </summary>
        /// <param name="owner">Any object that implements IWin32Window to own the folder browser dialog</param>
        /// <returns>
        /// System.Windows.Forms.DialogResult.OK if the user clicks OK in the dialog box;
        /// otherwise, System.Windows.Forms.DialogResult.Cancel.
        /// </returns>
        DialogResult ShowDialog(IWin32Window owner);

        /// <summary>
        /// Dispose the object
        /// </summary>
        void Dispose();
    }
}
