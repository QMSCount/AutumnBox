﻿using AutumnBox.OpenFramework.Extension;
using AutumnBox.OpenFramework.LeafExtension;
using AutumnBox.OpenFramework.LeafExtension.Attributes;
using AutumnBox.OpenFramework.LeafExtension.Fast;
using AutumnBox.OpenFramework.LeafExtension.Kit;

namespace AutumnBox.CoreModules.Extensions.Poweron.Dpm
{
    [ExtName("激活DPM软件")]
    [ExtAuth("zsh2401")]
    [ExtVersion(0,0,5)]
    [ExtPriority(ExtPriority.HIGH)]
    [ExtOfficial]
    [ExtDeveloperMode]
    [ExtRequiredDeviceStates(Basic.Device.DeviceState.Poweron)]
    class GreateActivator : LeafExtensionBase
    {
        [LMain]
        public void EntryPoint(ILeafUI ui) {
            using (ui) {
                ui.Title = this.GetName();
                ui.Icon = this.GetIconBytes();
                ui.Show();
                var selectResult = ui.SelectFrom("选择一个","冰箱","Fuck");
                ui.ShowMessage(selectResult.ToString());
                ui.Finish();
            }
        }
    }
}
