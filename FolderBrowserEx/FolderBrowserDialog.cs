using System;
using System.IO;
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
        public string SelectedFolder { get; set; }
        public DialogResult ShowDialog()
        {
            return ShowDialog(owner: new WindowWrapper(GetHandleFromWindow(GetDefaultOwnerWindow())));
        }
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
            uint options;
            frm.GetOptions(out options);
            options |= NativeMethods.FOS_PICKFOLDERS |
                       NativeMethods.FOS_FORCEFILESYSTEM |
                       NativeMethods.FOS_NOVALIDATE |
                       NativeMethods.FOS_NOTESTFILECREATE |
                       NativeMethods.FOS_DONTADDTORECENT;
            frm.SetOptions(options);
            if (this.InitialFolder != null)
            {
                NativeMethods.IShellItem directoryShellItem;
                var riid = new Guid("43826D1E-E718-42EE-BC55-A1E261C37BFE"); //IShellItem
                if (NativeMethods.SHCreateItemFromParsingName
                   (this.InitialFolder, IntPtr.Zero, ref riid,
                    out directoryShellItem) == NativeMethods.S_OK)
                {
                    frm.SetFolder(directoryShellItem);
                }
            }
            if (this.DefaultFolder != null)
            {
                NativeMethods.IShellItem directoryShellItem;
                var riid = new Guid("43826D1E-E718-42EE-BC55-A1E261C37BFE"); //IShellItem
                if (NativeMethods.SHCreateItemFromParsingName
                   (this.DefaultFolder, IntPtr.Zero, ref riid,
                    out directoryShellItem) == NativeMethods.S_OK)
                {
                    frm.SetDefaultFolder(directoryShellItem);
                }
            }

            if (frm.Show(owner.Handle) == NativeMethods.S_OK)
            {
                NativeMethods.IShellItem shellItem;
                if (frm.GetResult(out shellItem) == NativeMethods.S_OK)
                {
                    IntPtr pszString;
                    if (shellItem.GetDisplayName(NativeMethods.SIGDN_FILESYSPATH,
                        out pszString) == NativeMethods.S_OK)
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
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
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
