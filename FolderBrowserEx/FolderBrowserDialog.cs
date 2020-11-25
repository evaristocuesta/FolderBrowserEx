using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using Application = System.Windows.Application;
using IWin32Window = System.Windows.Forms.IWin32Window;

namespace FolderBrowserEx
{
    public class FolderBrowserDialog : IFolderBrowserDialog
    {
        /// <summary>
        /// Gets/sets the title of the dialog
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets/sets folder in which dialog will be open.
        /// </summary>
        public string InitialFolder { get; set; }

        /// <summary>
        /// Gets/sets directory in which dialog will be open 
        /// if there is no recent directory available.
        /// </summary>
        public string DefaultFolder { get; set; }

        /// <summary>
        /// Gets selected folder.
        /// </summary>
        public string SelectedFolder { get; private set; }

        public bool AllowMultiSelect { get; set; }

        /// <summary>
        /// Shows the folder browser dialog with a the default owner
        /// </summary>
        /// System.Windows.Forms.DialogResult.OK if the user clicks OK in the dialog box;
        /// otherwise, System.Windows.Forms.DialogResult.Cancel.
        /// </returns>
        public DialogResult ShowDialog()
        {
            return ShowDialog(owner: new WindowWrapper(GetHandleFromWindow(GetDefaultOwnerWindow())));
        }

        /// <summary>
        /// Shows the folder browser dialog with a the specified owner
        /// </summary>
        /// <param name="owner">Any object that implements IWin32Window to own the folder browser dialog</param>
        /// <returns>
        /// System.Windows.Forms.DialogResult.OK if the user clicks OK in the dialog box;
        /// otherwise, System.Windows.Forms.DialogResult.Cancel.
        /// </returns>
        public DialogResult ShowDialog(IWin32Window owner)
        {
            if (Environment.OSVersion.Version.Major >= 6)
            {
                return ShowVistaDialog(owner);
            }
            else
            {
                return ShowLegacyDialog(owner);
            }
        }
        private DialogResult ShowVistaDialog(IWin32Window owner)
        {
            var frm = (NativeMethods.IFileDialog)(new NativeMethods.FileOpenDialogRCW());
            frm.GetOptions(out uint options);
            options |= NativeMethods.FOS_PICKFOLDERS |
                       NativeMethods.FOS_FORCEFILESYSTEM |
                       NativeMethods.FOS_NOVALIDATE |
                       NativeMethods.FOS_NOTESTFILECREATE |
                       NativeMethods.FOS_DONTADDTORECENT;

            if (AllowMultiSelect)
                options |= NativeMethods.FOS_ALLOWMULTISELECT;

            frm.SetOptions(options);
            if (this.InitialFolder != null)
            {
                var riid = new Guid("43826D1E-E718-42EE-BC55-A1E261C37BFE"); //IShellItem
                if (NativeMethods.SHCreateItemFromParsingName
                   (this.InitialFolder, IntPtr.Zero, ref riid,
                    out NativeMethods.IShellItem directoryShellItem) == NativeMethods.S_OK)
                {
                    frm.SetFolder(directoryShellItem);
                }
            }
            if (this.DefaultFolder != null)
            {
                var riid = new Guid("43826D1E-E718-42EE-BC55-A1E261C37BFE"); //IShellItem
                if (NativeMethods.SHCreateItemFromParsingName
                   (this.DefaultFolder, IntPtr.Zero, ref riid,
                    out NativeMethods.IShellItem directoryShellItem) == NativeMethods.S_OK)
                {
                    frm.SetDefaultFolder(directoryShellItem);
                }
            }
            if (this.Title != null)
            {
                frm.SetTitle(this.Title);
            }

            if (frm.Show(owner.Handle) == NativeMethods.S_OK)
            {
                if (frm.GetResult(out NativeMethods.IShellItem shellItem) == NativeMethods.S_OK)
                {
                    if (shellItem.GetDisplayName(NativeMethods.SIGDN_FILESYSPATH,
                        out IntPtr pszString) == NativeMethods.S_OK)
                    {
                        if (pszString != IntPtr.Zero)
                        {
                            try
                            {
                                this.SelectedFolder = Marshal.PtrToStringAuto(pszString);
                                return DialogResult.OK;
                            }
                            finally
                            {
                                Marshal.FreeCoTaskMem(pszString);
                            }
                        }
                    }
                }
            }
            return DialogResult.Cancel;
        }
        private DialogResult ShowLegacyDialog(IWin32Window owner)
        {
            System.Windows.Forms.FolderBrowserDialog folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog(owner) == DialogResult.OK)
            {
                SelectedFolder = folderBrowserDialog.SelectedPath;
                return DialogResult.OK;
            }

            return DialogResult.Cancel;
        }

        private IntPtr GetHandleFromWindow(Window window)
        {
            if (window == null)
                return IntPtr.Zero;
            return new WindowInteropHelper(window).Handle;
        }

        private Window GetDefaultOwnerWindow()
        {
            Window defaultWindow = null;

            // TODO: Detect active window and change to that instead
            if (Application.Current != null && Application.Current.MainWindow != null)
            {
                defaultWindow = Application.Current.MainWindow;
            }
            return defaultWindow;
        }

        public void Dispose() { } //just to have possibility of Using statement.
    }

    
}
