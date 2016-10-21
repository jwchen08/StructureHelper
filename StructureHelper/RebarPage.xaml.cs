using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Controls.Primitives;
using Coding4Fun.Toolkit.Controls;
using System.IO.IsolatedStorage;
using System.IO;

namespace StructureHelper
{
    public partial class RebarPage : PhoneApplicationPage
    {
        const double PI = 3.14159265;
        int BeamWidth = 350;
        int BeamHeight = 700;
        int BeamBaohuceng = 30;
        string BeamConcrete = "C30";
        string BeamGangjin = "HRB400";
        double BeamMinPjl = 0.002;

        int PlateThickness = 120;
        int PlateBaohuceng = 20;
        string PlateConcrete = "C30";
        string PlateGangjin = "HRB400";
        double PlateMinPjl = 0.002;

        int ColumnLength = 600;
        int ColumnWidth = 600;
        int ColumnBaohuceng = 30;
        string ColumnConcrete = "C35";
        string ColumnGangjin = "HRB400";
        double ColumnMinPjl = 0.002;

        public RebarPage()
        {
            InitializeComponent();
            ReadDataFromFiles();
            SetMainPageContext();

            List<int> beamDiameterList = new List<int> { 6, 8, 10, 12, 14, 16, 18, 20, 22, 25, 28, 32, 36, 40, 50 };
            List<int> floorDiameterList = new List<int> { 6, 8, 10, 12, 14, 16, 18, 20, 22, 25, 28, 32, 36, 40, 50 };
            List<int> columnDiameterList = new List<int> { 6, 8, 10, 12, 14, 16, 18, 20, 22, 25, 28, 32, 36, 40, 50 };

            this.BeamNumberSelector.DataSource = new IntLoopingDataSource() { MinValue = 1, MaxValue = 30, SelectedItem = 4 };
            BeamNumberSelector.DataSource.SelectionChanged += DataSource_SelectionChanged;
            this.BeamDiameterSelector.DataSource = new ListLoopingDataSource<int>() { Items = beamDiameterList, SelectedItem = 20 };
            BeamDiameterSelector.DataSource.SelectionChanged += DataSource_SelectionChanged;

            this.floorDiameterSelector.DataSource = new ListLoopingDataSource<int>() { Items = floorDiameterList, SelectedItem = 10 };
            floorDiameterSelector.DataSource.SelectionChanged += DataSource_FloorSelectionChanged;
            this.floorSpaceSelector.DataSource = new IntLoopingDataSource() { MinValue = 50, MaxValue = 400, Increment = 50, SelectedItem = 150 };
            floorSpaceSelector.DataSource.SelectionChanged += DataSource_FloorSelectionChanged;

            this.ColumnNumberSelector.DataSource = new IntLoopingDataSource() { MinValue = 1, MaxValue = 30, SelectedItem = 4 };
            ColumnNumberSelector.DataSource.SelectionChanged += new EventHandler<SelectionChangedEventArgs>(DataSource_ColumnSelectionChanged);
            this.ColumnDiameterSelector.DataSource = new ListLoopingDataSource<int>() { Items = columnDiameterList, SelectedItem = 25 };
            ColumnDiameterSelector.DataSource.SelectionChanged += new EventHandler<SelectionChangedEventArgs>(DataSource_ColumnSelectionChanged);
            
        }

        //梁筋面积根数或直径变化
        void DataSource_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int num = int.Parse(BeamNumberSelector.DataSource.SelectedItem.ToString());
            int dia = int.Parse(BeamDiameterSelector.DataSource.SelectedItem.ToString());
            double result = num * PI * dia * dia / 4;
            double pjl = result / (BeamWidth * (BeamHeight - BeamBaohuceng));
            BeamResultText.Text = string.Format("{0:F}", result);
            BeamPeijinlvText.Text = string.Format("{0:F}", pjl * 100);
            if (pjl >= BeamMinPjl)
                BeamIsBiggerText.Text = ">";
            else
                BeamIsBiggerText.Text = "<";
        }

        //板筋面积根数或间距变化
        void DataSource_FloorSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int dia = int.Parse(floorDiameterSelector.DataSource.SelectedItem.ToString());
            int spa = int.Parse(floorSpaceSelector.DataSource.SelectedItem.ToString());
            double result = (1000.0 / spa) * PI * dia * dia / 4;
            PlateResultText.Text = string.Format("{0:F}", result);
            double pjl = result / (1000 * (PlateThickness - PlateBaohuceng));
            PlatePeijinlvText.Text = string.Format("{0:F}", pjl * 100);
            if (pjl >= PlateMinPjl)
                PlateIsBiggerText.Text = ">";
            else
                PlateIsBiggerText.Text = "<";


        }

        //柱筋面积根数或直径变化
        void DataSource_ColumnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int num = int.Parse(ColumnNumberSelector.DataSource.SelectedItem.ToString());
            int dia = int.Parse(ColumnDiameterSelector.DataSource.SelectedItem.ToString());
            double result = num * PI * dia * dia / 4;
            double pjl = result / (ColumnWidth * ColumnLength);
            ColumnResultText.Text = string.Format("{0:F}", result);
            ColumnPeijinlvText.Text = string.Format("{0:F}", pjl * 100);
            if (pjl >= ColumnMinPjl)
                ColumnIsBiggerText.Text = ">";
            else
                ColumnIsBiggerText.Text = "<";
        }

        //设置主页面数据
        public void SetMainPageContext()
        {
            BeamResultText.Text = "1256.64";
            BeamPeijinlvText.Text = "0.54";
            BeamSizeText.Text = BeamWidth + "x" + BeamHeight;
            BeamConcreteText.Text = BeamConcrete;
            BeamGangjinText.Text = BeamGangjin;
            BeamMinPeijinlvText.Text = string.Format("{0:F}", BeamMinPjl * 100);
            if (0.0054 > BeamMinPjl)
                BeamIsBiggerText.Text = ">";
            else
                BeamIsBiggerText.Text = "<";

            PlateResultText.Text = "523.60";
            PlatePeijinlvText.Text = "0.52";
            PlateThicknessText.Text = PlateThickness.ToString();
            PlateConcreteText.Text = PlateConcrete;
            PlateGangjinText.Text = PlateGangjin;
            PlateMinPeijinlvText.Text = string.Format("{0:F}", PlateMinPjl * 100);
            if (0.0052 > PlateMinPjl)
                PlateIsBiggerText.Text = ">";
            else
                PlateIsBiggerText.Text = "<";

            ColumnResultText.Text = "2123.72";
            ColumnPeijinlvText.Text = "0.62";
            ColumnSizeText.Text = ColumnLength + "x" + ColumnWidth;
            ColumnConcreteText.Text = ColumnConcrete;
            ColumnGangjinText.Text = ColumnGangjin;
            ColumnMinPeijinlvText.Text = string.Format("{0:F}", ColumnMinPjl * 100);
            if (0.0062 > ColumnMinPjl)
                ColumnIsBiggerText.Text = ">";
            else
                ColumnIsBiggerText.Text = "<";
        }

        //根据主页面滑动在第几项显示不同ApplicationBar
        private void mainPivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (mainPivot.SelectedIndex == 0)
                ApplicationBar = (Microsoft.Phone.Shell.ApplicationBar)Resources["appbar1"];
            else if (mainPivot.SelectedIndex == 1)
                ApplicationBar = (Microsoft.Phone.Shell.ApplicationBar)Resources["appbar2"];
            else if (mainPivot.SelectedIndex == 2)
                ApplicationBar = (Microsoft.Phone.Shell.ApplicationBar)Resources["appbar3"];
            else
                ApplicationBar = null;
        }

        //梁最小配筋率
        public double BeamMinPeijinlv(string ConIndex, string GjIndex)
        {
            double ft = FtOfConcrete(ConIndex);
            double fy = FyOfGangjin(GjIndex);
            double pjl = 0.002;
            if (0.45 * ft / fy > pjl)
                pjl = 0.45 * ft / fy;
            return pjl;
        }

        //板最小配筋率
        public double PlateMinPeijinlv(string ConIndex, string GjIndex)
        {
            double ft = FtOfConcrete(ConIndex);
            double fy = FyOfGangjin(GjIndex);
            double pjl = 0.002;
            //规范8.5.1注2
            if (fy >= 360.0)
            {
                pjl = 0.0015;
                if (0.45 * ft / fy > pjl)
                    pjl = 0.45 * ft / fy;
            }
            else
            {
                pjl = 0.002;
                if (0.45 * ft / fy > pjl)
                    pjl = 0.45 * ft / fy;
            }
            return pjl;
        }

        //柱单侧最小配筋率
        public double ColumnSingleMinPeijinlv(string ConIndex, string GjIndex)
        {
            return 0.002;
        }

        //柱全截面最小配筋率
        public double ColumnAllMinPeijinlv(string ConIndex, string GjIndex)
        {
            double pjl = 0.005;
            double ft = FtOfConcrete(ConIndex);
            double fy = FyOfGangjin(GjIndex);
            if (ft <= 2.04)
            {
                switch (GjIndex)
                {
                    case "HRB500":
                        pjl = 0.005;
                        break;
                    case "HRB400":
                        pjl = 0.0055;
                        break;
                    case "HRB335":
                        pjl = 0.006;
                        break;
                    case "HPB300":
                        pjl = 0.006;
                        break;
                    default:
                        pjl = 0.006;
                        break;
                }
            }
            else
            {
                switch (GjIndex)
                {
                    case "HRB500":
                        pjl = 0.006;
                        break;
                    case "HRB400":
                        pjl = 0.0065;
                        break;
                    case "HRB335":
                        pjl = 0.007;
                        break;
                    case "HPB300":
                        pjl = 0.007;
                        break;
                    default:
                        pjl = 0.007;
                        break;
                }
            }
            return pjl;
        }

        //根据钢筋标号返回钢筋强度设计值
        public double FyOfGangjin(string GjIndex)
        {
            double fy = 360.0;//默认为HRB400
            switch (GjIndex)
            {
                case "HPB235":
                    fy = 210.0;
                    break;
                case "HPB300":
                    fy = 270.0;
                    break;
                case "HRB335":
                    fy = 300.0;
                    break;
                case "HRB400":
                    fy = 360.0;
                    break;
                case "HRB500":
                    fy = 435.0;
                    break;
                default:
                    fy = 360.0;
                    break;
            }
            return fy;
        }

        //根据混凝土标号返回混凝土抗压强度设计值
        public double FcOfConcrete(string ConIndex)
        {
            double fc = 14.3;//默认为C30
            switch (ConIndex)
            {
                case "C15":
                    fc = 7.2;
                    break;
                case "C20":
                    fc = 9.6;
                    break;
                case "C25":
                    fc = 11.9;
                    break;
                case "C30":
                    fc = 14.3;
                    break;
                case "C35":
                    fc = 16.7;
                    break;
                case "C40":
                    fc = 19.1;
                    break;
                case "C45":
                    fc = 21.2;
                    break;
                case "C50":
                    fc = 23.1;
                    break;
                case "C55":
                    fc = 25.3;
                    break;
                case "C60":
                    fc = 27.5;
                    break;
                case "C65":
                    fc = 29.7;
                    break;
                case "C70":
                    fc = 31.8;
                    break;
                case "C75":
                    fc = 33.8;
                    break;
                case "C80":
                    fc = 35.9;
                    break;
                default:
                    fc = 14.3;
                    break;
            }
            return fc;
        }

        //根据混凝土标号返回混凝土抗拉强度设计值
        public double FtOfConcrete(string ConIndex)
        {
            double ft = 1.43;//默认为C30
            switch (ConIndex)
            {
                case "C15":
                    ft = 0.91;
                    break;
                case "C20":
                    ft = 1.10;
                    break;
                case "C25":
                    ft = 1.27;
                    break;
                case "C30":
                    ft = 1.43;
                    break;
                case "C35":
                    ft = 1.57;
                    break;
                case "C40":
                    ft = 1.71;
                    break;
                case "C45":
                    ft = 1.80;
                    break;
                case "C50":
                    ft = 1.89;
                    break;
                case "C55":
                    ft = 1.96;
                    break;
                case "C60":
                    ft = 2.04;
                    break;
                case "C65":
                    ft = 2.09;
                    break;
                case "C70":
                    ft = 2.14;
                    break;
                case "C75":
                    ft = 2.18;
                    break;
                case "C80":
                    ft = 2.22;
                    break;
                default:
                    ft = 1.43;
                    break;
            }
            return ft;
        }

        //梁信息设置页
        private void BeamSettingButton_Click(object sender, EventArgs e)
        {
            BeamSettingAppearStoryboard.Begin();
            ApplicationBar = null;
        }

        //板设置
        private void PlateSettingButton_Click(object sender, EventArgs e)
        {
            PlateSettingAppearStoryboard.Begin();
            ApplicationBar = null;
        }

        //柱设置
        private void ColumnSettingButton_Click(object sender, EventArgs e)
        {
            ColumnSettingAppearStoryboard.Begin();
            ApplicationBar = null;
        }

        //确认梁设置
        private void BeamSettingOKButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (IsBeamSettingAcceptable())
            {
                BeamWidth = int.Parse(BeamSettingWidthText.Text);
                BeamHeight = int.Parse(BeamSettingHeightText.Text);
                BeamConcrete = "C" + BeamSettingConcreteText.Text;
                int gj = int.Parse(BeamSettingGangjinText.Text);
                if (gj <= 300)
                    BeamGangjin = "HPB" + gj.ToString();
                else
                    BeamGangjin = "HRB" + gj.ToString();
                BeamMinPjl = BeamMinPeijinlv(BeamConcrete, BeamGangjin);

                BeamSizeText.Text = BeamWidth + "x" + BeamHeight;
                BeamConcreteText.Text = BeamConcrete;
                BeamGangjinText.Text = BeamGangjin;
                BeamMinPeijinlvText.Text = string.Format("{0:F}", BeamMinPjl * 100);

                BeamSettingDisapperStoryboard.Begin();
                ApplicationBar = (Microsoft.Phone.Shell.ApplicationBar)Resources["appbar1"];
            }
        }

        //取消梁设置
        private void BeamSettingCancelButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            BeamSettingDisapperStoryboard.Begin();
            ApplicationBar = (Microsoft.Phone.Shell.ApplicationBar)Resources["appbar1"];
        }

        //检查梁输入参数
        public bool IsBeamSettingAcceptable()
        {
            int width, height;
            if (int.TryParse(BeamSettingWidthText.Text, out width) && width > 0 && int.TryParse(BeamSettingHeightText.Text, out height) && height > 0)
            {
                if (CheckConcreteSetting(BeamSettingConcreteText.Text))
                {
                    if (CheckGangjinSetting(BeamSettingGangjinText.Text))
                    {
                        return true;
                    }
                    else
                        return false;
                }
                else
                    return false;
            }
            else
            {
                ToastPrompt tp = new ToastPrompt { Title = "提示：", Message = "梁尺寸输入有误！" };
                tp.Show();
                return false;
            }

        }

        //检查混凝土强度输入
        public bool CheckConcreteSetting(string cc)
        {
            if (cc == "15" || cc == "20" || cc == "25" || cc == "30" || cc == "35" || cc == "40" || cc == "45" || cc == "50" || cc == "55" || cc == "60" || cc == "65" || cc == "70" || cc == "75" || cc == "80")
                return true;
            else
            {
                ToastPrompt tp = new ToastPrompt { Title = "提示：", Message = "混凝土强度输入有误！" };
                tp.Show();
                return false;
            }
        }

        //检查钢筋强度输入
        public bool CheckGangjinSetting(string gj)
        {
            if (gj == "235" || gj == "300" || gj == "335" || gj == "400" || gj == "500")
                return true;
            else
            {
                ToastPrompt tp = new ToastPrompt { Title = "提示：", Message = "钢筋强度输入有误！" };
                tp.Show();
                return false;

            }

        }

        //确认板设置
        private void PlateSettingOKButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (IsPlateSettingAcceptable())
            {
                PlateThickness = int.Parse(PlateSettingThicknessText.Text);
                PlateConcrete = "C" + PlateSettingConcreteText.Text;
                int gj = int.Parse(PlateSettingGangjinText.Text);
                if (gj <= 300)
                    PlateGangjin = "HPB" + gj.ToString();
                else
                    PlateGangjin = "HRB" + gj.ToString();
                PlateMinPjl = PlateMinPeijinlv(PlateConcrete, PlateGangjin);

                PlateThicknessText.Text = PlateThickness.ToString();
                PlateConcreteText.Text = PlateConcrete;
                PlateGangjinText.Text = PlateGangjin;
                PlateMinPeijinlvText.Text = string.Format("{0:F}", PlateMinPjl * 100);

                PlateSettingDisappearStoryboard.Begin();
                ApplicationBar = (Microsoft.Phone.Shell.ApplicationBar)Resources["appbar2"];
            }
        }

        //取消板设置
        private void PlateSettingCancelButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            PlateSettingDisappearStoryboard.Begin();
            ApplicationBar = (Microsoft.Phone.Shell.ApplicationBar)Resources["appbar2"];
        }

        //检查板输入参数
        public bool IsPlateSettingAcceptable()
        {
            int thickness;
            if (int.TryParse(PlateSettingThicknessText.Text, out thickness) && thickness > 0)
            {
                if (CheckConcreteSetting(PlateSettingConcreteText.Text))
                {
                    if (CheckGangjinSetting(PlateSettingGangjinText.Text))
                    {
                        return true;
                    }
                    else
                        return false;
                }
                else
                    return false;
            }
            else
            {
                ToastPrompt tp = new ToastPrompt { Title = "提示：", Message = "板厚度输入有误！" };
                tp.Show();
                return false;
            }

        }

        //确认柱设置
        private void ColumnSettingOKButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (IsColumnSettingAcceptable())
            {
                ColumnLength = int.Parse(ColumnSettingLengthText.Text);
                ColumnWidth = int.Parse(ColumnSettingWidthText.Text);
                ColumnConcrete = "C" + ColumnSettingConcreteText.Text;
                int gj = int.Parse(ColumnSettingGangjinText.Text);
                if (gj <= 300)
                    ColumnGangjin = "HPB" + gj.ToString();
                else
                    ColumnGangjin = "HRB" + gj.ToString();
                ColumnMinPjl = ColumnSingleMinPeijinlv(ColumnConcrete, ColumnGangjin);

                ColumnSizeText.Text = ColumnLength + "x" + ColumnWidth;
                ColumnConcreteText.Text = ColumnConcrete;
                ColumnGangjinText.Text = ColumnGangjin;
                ColumnMinPeijinlvText.Text = string.Format("{0:F}", ColumnMinPjl * 100);

                ColumnSettingDisappearStoryboard.Begin();
                ApplicationBar = (Microsoft.Phone.Shell.ApplicationBar)Resources["appbar3"];
            }
        }

        //取消柱设置
        private void ColumnSettingCancelButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ColumnSettingDisappearStoryboard.Begin();
            ApplicationBar = (Microsoft.Phone.Shell.ApplicationBar)Resources["appbar3"];
        }

        //检查柱输入参数
        public bool IsColumnSettingAcceptable()
        {
            int length, width;
            if (int.TryParse(ColumnSettingLengthText.Text, out length) && length > 0 && int.TryParse(ColumnSettingWidthText.Text, out width) && width > 0)
            {
                if (CheckConcreteSetting(ColumnSettingConcreteText.Text))
                {
                    if (CheckGangjinSetting(ColumnSettingGangjinText.Text))
                    {
                        return true;
                    }
                    else
                        return false;
                }
                else
                    return false;
            }
            else
            {
                ToastPrompt tp = new ToastPrompt { Title = "提示：", Message = "柱尺寸输入有误！" };
                tp.Show();
                return false;
            }

        }

        //离开页面保存数据
        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            using (IsolatedStorageFile appStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (var file = appStorage.OpenFile("SettingsData.txt", System.IO.FileMode.OpenOrCreate))
                {
                    using (System.IO.StreamWriter sw = new System.IO.StreamWriter(file))
                    {
                        sw.WriteLine(BeamWidth);
                        sw.WriteLine(BeamHeight);
                        sw.WriteLine(BeamConcrete);
                        sw.WriteLine(BeamGangjin);
                        sw.WriteLine(BeamMinPjl);

                        sw.WriteLine(PlateThickness);
                        sw.WriteLine(PlateConcrete);
                        sw.WriteLine(PlateGangjin);
                        sw.WriteLine(PlateMinPjl);

                        sw.WriteLine(ColumnLength);
                        sw.WriteLine(ColumnWidth);
                        sw.WriteLine(ColumnConcrete);
                        sw.WriteLine(ColumnGangjin);
                        sw.WriteLine(ColumnMinPjl);
                    }
                }
            }
        }

        //启动时读取数据
        public void ReadDataFromFiles()
        {
            using (IsolatedStorageFile appStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (appStorage.FileExists("SettingsData.txt"))
                {
                    using (var file = appStorage.OpenFile("SettingsData.txt", FileMode.Open))
                    {
                        using (StreamReader sr = new StreamReader(file))
                        {
                            BeamWidth = int.Parse(sr.ReadLine());
                            BeamHeight = int.Parse(sr.ReadLine());
                            BeamConcrete = sr.ReadLine();
                            BeamGangjin = sr.ReadLine();
                            BeamMinPjl = double.Parse(sr.ReadLine());

                            PlateThickness = int.Parse(sr.ReadLine());
                            PlateConcrete = sr.ReadLine();
                            PlateGangjin = sr.ReadLine();
                            PlateMinPjl = double.Parse(sr.ReadLine());

                            ColumnLength = int.Parse(sr.ReadLine());
                            ColumnWidth = int.Parse(sr.ReadLine());
                            ColumnConcrete = sr.ReadLine();
                            ColumnGangjin = sr.ReadLine();
                            ColumnMinPjl = double.Parse(sr.ReadLine());
                        }
                    }
                }
            }

        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            if (BeamSettingBorder.Visibility == Visibility.Visible)
            {
                e.Cancel = true;
                BeamSettingDisapperStoryboard.Begin();
                ApplicationBar = (Microsoft.Phone.Shell.ApplicationBar)Resources["appbar1"];
            }
            if (PlateSettingBorder.Visibility == Visibility.Visible)
            {
                e.Cancel = true;
                PlateSettingDisappearStoryboard.Begin();
                ApplicationBar = (Microsoft.Phone.Shell.ApplicationBar)Resources["appbar2"];
            }
            if (ColumnSettingBorder.Visibility == Visibility.Visible)
            {
                e.Cancel = true;
                ColumnSettingDisappearStoryboard.Begin();
                ApplicationBar = (Microsoft.Phone.Shell.ApplicationBar)Resources["appbar3"];
            }

            base.OnBackKeyPress(e);
        }

    }
}