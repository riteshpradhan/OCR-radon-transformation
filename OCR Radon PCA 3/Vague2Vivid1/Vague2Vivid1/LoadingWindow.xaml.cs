using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using System.Threading;
using System.Windows.Threading;

namespace Vague2Vivid1
{
    /// <summary>
    /// Interaction logic for LoadingWindow.xaml
    /// </summary>
    public partial class LoadingWindow : Window
    {
        //Thread loadingThread;
        //Storyboard Showboard;
        //Storyboard Hideboard;
        //private delegate void ShowDelegate(string txt);
        //private delegate void HideDelegate();
        //ShowDelegate showDelegate;
        //HideDelegate hideDelegate;


        public LoadingWindow()
        {
            InitializeComponent();
            //new LoadingWindow().ShowDialog();
            //Showboard = this.Resources["showStoryboard"] as Storyboard;
            //Hideboard = this.Resources["HideStoryboard"] as Storyboard;
            

        }

        private void textBox1_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    //    private void showText(string txt)
    //    {
    //        txtLoading.Text = txt;
    //        BeginStoryboard(Showboard);
    //    }
    //    private void hideText()
    //    {
    //        BeginStoryboard(Hideboard);
    //    }
    //    private void load()
    //    {
    //        Thread.Sleep(1000);
    //        this.Dispatcher.Invoke(showDelegate, "first data to loading");
    //        Thread.Sleep(2000);
    //        //do some loading work
    //        this.Dispatcher.Invoke(hideDelegate);

    //        Thread.Sleep(2000);
    //        this.Dispatcher.Invoke(showDelegate, "second data loading");
    //        Thread.Sleep(2000);
    //        //do some loading work
    //        this.Dispatcher.Invoke(hideDelegate);

    //        Thread.Sleep(2000);
    //        this.Dispatcher.Invoke(showDelegate, "last data loading");
    //        Thread.Sleep(2000);
    //        //do some loading work 
    //        this.Dispatcher.Invoke(hideDelegate);

    //        //close the window
    //        Thread.Sleep(2000);
    //        this.Dispatcher.Invoke(DispatcherPriority.Normal,
    //    (Action)delegate() { Close(); });
    //    }
    }
}
