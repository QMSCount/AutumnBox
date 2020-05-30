﻿/*

* ==============================================================================
*
* Filename: RegisteredExtensionInfo
* Description: 
*
* Version: 1.0
* Created: 2020/5/30 11:00:12
* Compiler: Visual Studio 2019
*
* Author: zsh2401
*
* ==============================================================================
*/
using AutumnBox.OpenFramework.Management.ExtInfo;
using AutumnBox.OpenFramework.Management.ExtLibrary;
using System;

namespace AutumnBox.OpenFramework.Management
{
    public interface IRegisteredExtensionInfo
    {
        IExtensionInfo ExtensionInfo { get; }
        ILibrarian? Librarian { get; }
    }
    public class RegisteredExtensionInfo : IRegisteredExtensionInfo
    {
        public RegisteredExtensionInfo(IExtensionInfo extensionInfo, ILibrarian? librarian)
        {
            ExtensionInfo = extensionInfo ?? throw new ArgumentNullException(nameof(extensionInfo));
            Librarian = librarian;
        }

        public IExtensionInfo ExtensionInfo { get; }
        public ILibrarian? Librarian { get; }
    }
}
