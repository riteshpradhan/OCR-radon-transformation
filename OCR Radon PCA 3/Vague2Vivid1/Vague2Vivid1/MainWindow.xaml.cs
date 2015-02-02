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
using System.ComponentModel;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;

using System.Drawing;
using Vague2Vivid1.OCR;
using System.Collections.ObjectModel;
using ImageSearch;
using System.IO;
using Vague2Vivid1.WebCam1;


namespace Vague2Vivid1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Bitmap bitmap;  //input image

        public static BitmapSource bs;
        public static IntPtr ip;
        private string recogniseCharacters = null;
        private const string _InitialUrl = @"http://www.google.com/images?hl=en&source=imghp&q=";
        ObservableCollection<System.Windows.Controls.Image> Images;
        GoogleImageSearchService service;
        private int  count=0,wordCount;
        private char[] words;
        private String[] sepWords;
        WebCam webcam;
        public MainWindow()
        {
            InitializeComponent();
            //google image serach
            Images = new ObservableCollection<System.Windows.Controls.Image>();
            this.DataContext = Images;
            words = new char[30];
            sepWords = new String[20];


           
        }
        private void mainWindow_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            // TODO: Add event handler implementation here.
            webcam = new WebCam();
            webcam.InitializeWebCam(ref imgVideo);
        }
        private void btnCredits_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(" Kapila Rajbhandari    064-BCT-513\n Rajesh Raj Pandey     064-BCT-526\n Ritesh Pradhan          064-BCT-527\n Sunit Belbase             064-BCT-545\n");
            CreditsWindow win_credits = new CreditsWindow();
            win_credits.Show();
        }

        private void bntStart_Click(object sender, RoutedEventArgs e)
        {
            webcam.Start();

        }

        private void bntStop_Click(object sender, RoutedEventArgs e)
        {
            webcam.Stop();

        }

        private void bntContinue_Click(object sender, RoutedEventArgs e)
        {
            webcam.Continue();

        }

        private void bntCapture_Click(object sender, RoutedEventArgs e)
        {
            imgCapture.Source = imgVideo.Source;
            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create((BitmapSource) imgCapture.Source));
                enc.Save(outStream);
                bitmap = new Bitmap(outStream);

            }
            


        }

        private void bntSaveImage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                imgCapture.Source = imgVideo.Source;
            }
            catch { MessageBox.Show("k bhayo yo save button lai??"); }
            finally { }

        }

        private void bntResolution_Click(object sender, RoutedEventArgs e)
        {
            webcam.ResolutionSetting();

        }

        private void bntNext_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Image img;
            count ++;
            Images.Clear();
            try
            {
                if (count == 4)
                    count = 0;
                img = service._GoogledImages[count];


                img.Width = 400;
                img.Height = 250;
                img.ClipToBounds = true;
                Images.Add(img);
            }
            catch (Exception) 
            {
                MessageBox.Show("Please wait some time.");
            }


        }

        private void bntBrowse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Bitmap files (*.bmp)|*.bmp|PNG files (*.png)|*.png|TIFF files (*.tif)|*tif|JPEG files (*.jpg)|*.jpg |All files (*.*)|*.*";
            ofd.FilterIndex = 5;
            ofd.RestoreDirectory = true;
            if (ofd.ShowDialog().Equals(true))
            {
                bitmap = new Bitmap(Bitmap.FromFile(ofd.FileName));
                if (bitmap == null)
                    throw new ArgumentNullException("bitmap");

                
                    IntPtr hBitmap = bitmap.GetHbitmap();

                   
                        imgCapture.Source= System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                            hBitmap,
                            IntPtr.Zero,
                            Int32Rect.Empty,
                            BitmapSizeOptions.FromEmptyOptions());
                  
                

               

            }
        }

        private void bntExit_Click(object sender, RoutedEventArgs e)
        {
            WordTextBox.Children.Clear();
            Button txtNumber;
            ImageProcessing1 imgProcessing = new ImageProcessing1(bitmap);
            bitmap = imgProcessing.character();
      //imgRecognise.Source = LoadBitmap(bitmap);
            recogniseCharacters = imgProcessing.getRecogniseCharacter();

            //LoadingWindow load_win = new LoadingWindow(); // yeta baata loading window nikaaleko ho
            //load_win.Activate();
            //load_win.Show();
            //LoadingWindow load_win = new LoadingWindow(); // yeta baata loading window nikaaleko ho
            imgProcessing.wordDisplay();

           // load_win.Close(); // word aaye pachi close the loading window

            wordCount = imgProcessing.getnoWords();
            sepWords=imgProcessing.getWords();
            for (int i = 0; i < wordCount; i++)
            {
                txtNumber = new Button();
                txtNumber.Width = 80;
                txtNumber.Height = 50;

                txtNumber.Name=sepWords[i];
                txtNumber.Content = sepWords[i];
                WordTextBox.Children.Add(txtNumber);
                txtNumber.Click += new RoutedEventHandler(this.callToSearch);
                
                //load_win.Activate();
                //load_win.Show();

                
            }


            //load_win.Close();
            //load_win.ApplyAnimationClock(DependencyProperty dp,TimeSpan.FromSeconds(1));

         

            
           
        }
        public void callToSearch(object sender, RoutedEventArgs e)
        {
            string tempSearch = sender.ToString();
            tempSearch = tempSearch.Substring(31);
            Console.WriteLine("searching image" + sender.ToString());
           
            service = new GoogleImageSearchService();
            service.GetImagesAsync(tempSearch, ServiceCallback);
        }



        private void imgCapture_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {

        }
        //load  bitmap to display 
        public static BitmapSource LoadBitmap(System.Drawing.Bitmap source)
        {

            ip = source.GetHbitmap();

            bs = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(ip, IntPtr.Zero, System.Windows.Int32Rect.Empty,

                System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());


            return bs;

        }

        private void buttonSearch_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("the recognised word is " + recogniseCharacters);
            service = new GoogleImageSearchService();
            service.GetImagesAsync(recogniseCharacters, ServiceCallback);
         }
      
        private void ServiceCallback(System.Windows.Controls.Image result)
        {
            Images.Clear();
            //  foreach(Image img in result  ) 
            {
                result.Width = 400;
                result.Height = 250;
                result.ClipToBounds = true;
                Images.Add(result);
                Console.WriteLine("image ");
            }
        }
        private void listBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
