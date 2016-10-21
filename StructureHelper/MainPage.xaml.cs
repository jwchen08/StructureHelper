using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using StructureHelper.Resources;
using Coding4Fun.Toolkit.Controls;
using Microsoft.Phone.Tasks;
using UmengSDK;
using System.Text;
using System.IO.IsolatedStorage;
using System.Threading.Tasks;
using System.IO;

namespace StructureHelper
{
    public partial class MainPage : PhoneApplicationPage
    {
        // 构造函数
        public MainPage()
        {
            InitializeComponent();
            this.Loaded += MainPage_Loaded;
            UpdateLiveTiles();

            //友盟在线参数
            UmengAnalytics.UpdateOnlineParamCompleted += UmengAnalytics_UpdateOnlineParamCompleted;
            UmengAnalytics.UpdateOnlineParamAsync("52d167dc56240be952186376");
            RateAndCommend();
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            //if (!(Application.Current as App).IsTrial)
            //{
            //    ToastPrompt tp = new ToastPrompt { Message = "正式版!" };
            //    tp.Show();
            //}
        }

        //启动5次后提示打分，点击确定后以后不再提示，否则每启动5次提示打分
        private void RateAndCommend()
        {
            int isRated = 0;
            int launchTimes = 0;
            using (IsolatedStorageFile appStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (appStorage.FileExists("RatedInfo.txt"))
                {
                    using (var file = appStorage.OpenFile("RatedInfo.txt", FileMode.Open))
                    {
                        using (StreamReader sr = new StreamReader(file))
                        {
                            //是否打过分
                            isRated = int.Parse(sr.ReadLine());
                            //启动次数
                            launchTimes = int.Parse(sr.ReadLine());

                        }
                    }
                }
            }
            //已经打过分了
            if (isRated == 1)
            {
                return;
            }
            //没打分，但是启动了五次
            else if (launchTimes == 5)
            {
                MessageBoxResult result = MessageBox.Show("亲，您已经使用该软件一段时间了，如果觉得不错麻烦给个好评哦！", "提醒", MessageBoxButton.OKCancel);
                if (result == MessageBoxResult.OK)
                {
                    using (IsolatedStorageFile appStorage = IsolatedStorageFile.GetUserStoreForApplication())
                    {
                        using (var file = appStorage.OpenFile("RatedInfo.txt", System.IO.FileMode.OpenOrCreate))
                        {
                            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(file))
                            {
                                sw.WriteLine("1");
                                sw.WriteLine("0");

                            }
                        }
                    }
                    MarketplaceReviewTask mrt = new MarketplaceReviewTask();
                    mrt.Show();
                }

                else if (result == MessageBoxResult.Cancel)
                {
                    using (IsolatedStorageFile appStorage = IsolatedStorageFile.GetUserStoreForApplication())
                    {
                        using (var file = appStorage.OpenFile("RatedInfo.txt", System.IO.FileMode.OpenOrCreate))
                        {
                            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(file))
                            {
                                sw.WriteLine("0");
                                sw.WriteLine("0");

                            }
                        }
                    }
                }
            }
            //没打分，启动次数不到五次
            else
            {
                launchTimes++;
                using (IsolatedStorageFile appStorage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    using (var file = appStorage.OpenFile("RatedInfo.txt", System.IO.FileMode.OpenOrCreate))
                    {
                        using (System.IO.StreamWriter sw = new System.IO.StreamWriter(file))
                        {
                            sw.WriteLine("0");
                            sw.WriteLine(launchTimes);

                        }
                    }
                }
            }

        }

        //获取友盟在线参数，每次仅获取一个
        void UmengAnalytics_UpdateOnlineParamCompleted(int statusCode, OnlineParamEventArgs e)
        {
            //statusCode返回状态码，0 标示有更新 
            //e OnlineConfigEventArgs 实例，包涵在线参数信息. 
            //e.Result 返回Dictionary对应线上配置的K-V格式在线参数信息
            if (statusCode == 0 && e.Result.Count > 0)
            {
                string ReadedInfo = "";
                //首先读取已查看过的消息列表
                using (IsolatedStorageFile appStorage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    if (appStorage.FileExists("UmengInfo.txt"))
                    {
                        using (var file = appStorage.OpenFile("UmengInfo.txt", FileMode.Open))
                        {
                            using (StreamReader sr = new StreamReader(file))
                            {
                                //读取全部信息
                                ReadedInfo = sr.ReadToEnd();

                            }
                        }
                    }
                }

                //判断服务器上的消息是否已读
                for (int i = 0; i < e.Result.Count; i++)
                {
                    var item = e.Result.ElementAt(i);
                    //存在未读消息
                    if (!ReadedInfo.Contains(item.Key + item.Value))
                    {
                        this.Dispatcher.BeginInvoke(delegate()
                        {
                            MessageBox.Show(item.Value, item.Key, MessageBoxButton.OK);

                        });
                        //将该消息加进已读列表
                        using (IsolatedStorageFile appStorage = IsolatedStorageFile.GetUserStoreForApplication())
                        {
                            using (var file = appStorage.OpenFile("UmengInfo.txt", System.IO.FileMode.Append))
                            {
                                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(file))
                                {
                                    //WriteLine其实是在字符串后面加上\r\n
                                    sw.WriteLine(item.Key + item.Value);

                                }
                            }
                        }
                        //此次不再获取新消息
                        break;
                    }
                }

            }
            UmengAnalytics.UpdateOnlineParamCompleted -= UmengAnalytics_UpdateOnlineParamCompleted;
        }

        //动态磁贴
        private void UpdateLiveTiles()
        {
            ShellTile mainTile = ShellTile.ActiveTiles.First();
            FlipTileData TileData = new FlipTileData()
            {
                Title = "结构小助手",
                BackTitle = "结构人生",
                BackContent = "我们构筑世界",
                WideBackContent = "我们构筑世界\nWe construct the world!",
                SmallBackgroundImage = new Uri("/Assets/Tiles/FlipCycleTileSmall.png", UriKind.Relative),
                BackgroundImage = new Uri("/Assets/Tiles/FlipCycleTileMedium.png", UriKind.Relative),
                BackBackgroundImage = new Uri("/Assets/Tiles/Back.png", UriKind.Relative),
                WideBackgroundImage = new Uri("/Assets/Tiles/FlipCycleTileLarge.png", UriKind.Relative),
                WideBackBackgroundImage = new Uri("/Assets/Tiles/WideBack.png", UriKind.Relative),
            };
            mainTile.Update(TileData);
        }

        //配筋查询
        private void RebarButton_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/RebarPage.xaml", UriKind.RelativeOrAbsolute));
        }

        //常用设计参数
        private void CanshuButton_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/CanshuPage.xaml", UriKind.Relative));
        }

        //结构求解
        private void SolverButton_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/SolverPage.xaml", UriKind.Relative));
        }

        //protected override void OnNavigatedTo(NavigationEventArgs e)
        //{
        //    IDictionary<string, string> queryStrings = NavigationContext.QueryString;
        //    if (queryStrings.ContainsKey("Msg")) MessageBox.Show("来自URI关联调用方信息：\n" + queryStrings["Msg"]);
        //    base.OnNavigatedTo(e);
        //}

        //应用推荐
        void appBarMenuItem1_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/RecommendPage.xaml", UriKind.Relative));
        }

        //给个好评
        void appBarMenuItem3_Click(object sender, EventArgs e)
        {
            MarketplaceReviewTask mrt = new MarketplaceReviewTask();
            mrt.Show();
        }

        private void SectionButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/SectionPage.xaml", UriKind.Relative));
        }

        private void BuyMenuItem_Click(object sender, EventArgs e)
        {
            MarketplaceDetailTask mdt = new MarketplaceDetailTask();
            mdt.Show();
        }

        private void EarthquakeButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/EarthquakePage.xaml", UriKind.Relative));
        }

        private void ApplicationBarMenuItem_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/AboutPage.xaml", UriKind.Relative));
        }

    }
}