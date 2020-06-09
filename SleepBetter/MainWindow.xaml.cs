using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using System.Windows.Threading;

namespace SleepBetter
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : SourceChord.FluentWPF.AcrylicWindow
    {
        Task<Config> config;

        public MainWindow()
        {
            InitializeComponent();

            config = new Task<Config>(() => {
                var cfg = new Config();

                Dispatcher.Invoke(() =>
                {
                    Display.Text = "你已经坚持" + cfg.Days + "天没有熬夜了";
                    SleepBetter.IsEnabled = true;
                    MotherFucker.IsEnabled = true;

                    var plan = global::SleepBetter.Personal.Plan.myPlan((int)cfg.Days);
                    SleepBetter.Content =
                        "在 " + plan.Item1.ToString("D2") + ":" + plan.Item2.ToString("D2") + " 之前睡觉";
                });

                return cfg;
            });

            Loaded += (_,_2) => config.Start();
        }

        private void SleepBetter_Click(object sender, RoutedEventArgs e)
        {
            SleepBetter.IsEnabled = false;
            MotherFucker.IsEnabled = false;
            CloseWindowBtn.IsEnabled = false;

            var c = config.Result;
            c.Days++;
            Display.FontSize = 50;
            Display.Text = "很好！你现在已经坚持" + c.Days + "天没有熬夜了！";

            new Task(() =>
            {
                c.Upload();
                Thread.Sleep(1000);
                Dispatcher.Invoke(() => Close());
            }).Start();
        }

        private void MotherFucker_Click(object sender, RoutedEventArgs e)
        {
            SleepBetter.IsEnabled = false;
            MotherFucker.IsEnabled = false;
            CloseWindowBtn.IsEnabled = false;

            var c = config.Result;
            c.Days = 0;
            Display.Text = "小心猝死";

            new Task(() =>
            {
                c.Upload();
                Thread.Sleep(1000);
                Dispatcher.Invoke(() => Close());
            }).Start();
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
