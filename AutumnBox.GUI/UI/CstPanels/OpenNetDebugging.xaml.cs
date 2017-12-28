﻿using AutumnBox.Basic.Connection;
using AutumnBox.Basic.Devices;
using AutumnBox.Basic.Flows;
using AutumnBox.Support.CstmDebug;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AutumnBox.GUI.UI.CstPanels
{
    /// <summary>
    /// OpenNetDebugging.xaml 的交互逻辑
    /// </summary>
    public partial class OpenNetDebugging : UserControl, ICommunicableWithFastGrid
    {
        private readonly Serial _serial;
        public OpenNetDebugging(Serial serial)
        {
            InitializeComponent();
            _serial = serial;
        }

        public event EventHandler CallFatherToClose;

        public void OnFatherClosed()
        {

        }
        private const string portPattern = @"\d";
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, portPattern);
        }
        private async void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            //检查输入的端口是否正确
            int port = int.Parse(TBoxPort.Text);
            if (port > 65535)//如果端口号不对
            {//告诉用户不对
                new FastGrid(this.GridMain,
                    new DevicesPanelMessageBox(
                        App.Current.Resources["msgPleaseInputAPort"].ToString()
                        )
                );
                //并且将端口输入框重置
                TBoxPort.Text = "5555";
                //离开当前方法
                return;
            }
            //如果进行到这里,说明检查通过了
            var opener = new NetDebuggingOpener();
            opener.Init(new NetDebuggingOpenerArgs()
            {
                DevBasicInfo = new DeviceBasicInfo()
                {
                    Serial = _serial,
                    State = DeviceState.Poweron,
                },
                Port = (uint)port
            });
            //异步开启该设备的网络调试
            var result = await Task.Run(() =>
            {
                return opener.Run();
            });
            //如果开启成功了
            if (result.ResultType == Basic.FlowFramework.ResultType.Successful)
            {
                try
                {
                    //尝试连接到刚才开启调试的设备
                    await Task.Run(() =>
                    {
                        Thread.Sleep(3000);
                        var IP = DeviceInfoHelper.GetLocationIP(_serial);
                        if (IP != null)
                        {
                            this.Dispatcher.Invoke(() =>
                            {
                                new FastGrid(this.GridMain,
                                             new DevicesPanelMessageBox(
                                             App.Current.Resources["msgGettedIP"].ToString() + Environment.NewLine
                                             + IP.ToString() + ":" + port
                                             )
                                );
                            });
                        }
                        var connecter = new NetDeviceAdder();
                        connecter.Init(new NetDeviceAdderArgs()
                        {
                            IPEndPoint = new IPEndPoint(IP, port)
                        });
                        return connecter.Run();
                    });
                }
                catch (Exception ex)
                {
                    Logger.T("auto connect failed....", ex);
                }
            }
            //无论如何,执行完后,都要关闭连接界面
            await Task.Run(() => Thread.Sleep(10000));
            CallFatherToClose?.Invoke(this, e);
        }
    }
}
