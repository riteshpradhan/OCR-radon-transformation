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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;

namespace ImageSearch
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string _InitialUrl = @"http://www.google.com/images?hl=en&source=imghp&q=";
        ObservableCollection<Image> Images;

        public MainWindow()
        {
            InitializeComponent();
            Images = new ObservableCollection<Image>();
            this.DataContext = Images;
        }
        GoogleImageSearchService service;
        private void button1_Click(object sender, RoutedEventArgs e)
        { 
           service =new GoogleImageSearchService();
           service.GetImagesAsync(textBox1.Text, ServiceCallback,30);    
        }

        private void ServiceCallback(IList<Image> result)
        {
            Images.Clear();
            foreach(Image img in result  ) 
            {
                img.Width = 50;
                img.Height = 50;
                img.ClipToBounds = true;
                Images.Add(img);     
            }
        }
    }
}
