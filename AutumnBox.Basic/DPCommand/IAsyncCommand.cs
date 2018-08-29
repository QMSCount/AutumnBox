﻿/*************************************************
** auth： zsh2401@163.com
** date:  2018/8/29 2:33:19 (UTC +8:00)
** desc： ...
*************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutumnBox.Basic.DPCommand
{
    public interface IAsyncCommand : ICommand
    {
        Task<ICommandResult> ExecuteAsync();
        void Cancel();
    }
}