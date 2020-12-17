# FolderBrowserEx
[![NuGet](https://img.shields.io/nuget/v/FolderBrowserEx)](https://nuget.org/packages/FolderBrowserEx/) [![.NET Core](https://github.com/evaristocuesta/FolderBrowserEx/workflows/.NET%20Core/badge.svg)](https://github.com/evaristocuesta/FolderBrowserEx/actions)

**FolderBrowserEx** is a library to use the Windows Vista/7 Folder Browser in your .NET Framework and .NET Core Applications. 

Supporting .NET Framework (4.5+) and .NET Core (3.0 and 3.1)

## Table of contents

  - [Introduction](#introduction)
  - [Getting Started](#getting-started)
  - [License](#license)
  - [Credits](#credits)

## Introduction

In both .NET Framework and .NET Core applications we can use the control FolderBrowserDialog from System.Windows.Form. The problem is that the style of this controls looks very old and is very difficult to use, especially when it is compared to the new folder selection dialog which is used in Windows Vista. Unfortunately, it has not been included to .NET. 

The aim of this project is to offer a Windows Vista look and feel folder browser dialog to easily give a more modern look to our .NET applications. 

## Getting Started

To use this library, there are a few options:

  - Download this repository
  - Use the [FolderBrowserEx Nuget Package](https://nuget.org/packages/FolderBrowserEx/)

The FolderBrowserDialog uses the IFolderBrowserDialog interface.
```csharp
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
```
To use in an application, you can follow this example code. There are others examples in the directory Samples of the solution. 

```csharp
FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog()
folderBrowserDialog.Title = "Select a folder";
folderBrowserDialog.InitialFolder = @"C:\";
folderBrowserDialog.AllowMultiSelect = false;
if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
{
  string result += folderBrowserDialog.SelectedFolder;
}
```

If you want to use the FolderBrowserEx library from a View Model, follow this example code.

```csharp
using FolderBrowserEx;
using MVVMBase;
using System.Windows.Forms;
using System.Windows.Input;

namespace NetFrameworkSample
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly IFolderBrowserDialog _folderBrowserDialog;
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
            _folderBrowserDialog.Title = "Select multiple folders";
            _folderBrowserDialog.InitialFolder = @"C:\";
            _folderBrowserDialog.AllowMultiSelect = true;
            if (_folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                Result += $"{_folderBrowserDialog.SelectedFolder}\n";
            }
        }
    }
}
```

## License

Copyright © 2020 Evaristo Cuesta 

**FolderBrowserEx** is provided as-is under the MIT license. For more information see [LICENSE](./LICENSE).

## Credits

This project was adapted from the code from [CodeProject](https://www.codeproject.com/Articles/5255769/Csharp-Select-FolderDialog-for-NET-Core-3-0) writen by [ftwnate917](https://www.codeproject.com/Members/ftwnate917) and improved with new features. 

