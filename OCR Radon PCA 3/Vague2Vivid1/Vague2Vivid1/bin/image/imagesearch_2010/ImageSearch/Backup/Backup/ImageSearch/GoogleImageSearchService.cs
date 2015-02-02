using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using Wintellect.Threading.AsyncProgModel;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Media.Imaging;

namespace ImageSearch
{
    class GoogleImageSearchService
    {
        private string _searchString;
        private Int32 _numberOfImages;
        private Int32 _totalImages;
        private const string _GoogleImageURIPattern = ":\",\"http://[^\"]*\"";
        private const string _GoogleImageSearchBaseUrl = @"http://images.google.com/images?q={0}&start={1}&ndsp={2}";
        private Action<IList<Image>> _callback;
        private  IList<Image> _GoogledImages;
        private Int32 _numberOfImagesInAPage = 20;
        

        public void  GetImagesAsync(string searchString,Action<IList<Image>> callback, int count = 5)
        {
            if (String.IsNullOrEmpty( searchString))
            {
                throw new ArgumentNullException("searchString"); 
            }
            _searchString = searchString;
            _totalImages  = count;
            _callback = callback;
            AsyncEnumerator asyncEnumerator = new AsyncEnumerator();
            asyncEnumerator.BeginExecute(GetAllImages(asyncEnumerator), EnumeratorExecutionComplete);
              
        }

        private IEnumerator<Int32> GetAllImages(AsyncEnumerator ae)
        {
            Int32 CurrentPage=0;
           Int32 _totalImagesTemp=_totalImages ;
           _GoogledImages = new List<Image>();
            while(_totalImagesTemp>0)
            {
                AsyncEnumerator asyncE = new AsyncEnumerator();
                asyncE.BeginExecute(GetImagesFromGoogle(asyncE,
                                  GetGoogleSearchUrl(CurrentPage++),
                                  _totalImagesTemp > _numberOfImagesInAPage ? _numberOfImagesInAPage : _totalImagesTemp), ae.EndVoid(0, DiscardWebRequest));
                _totalImagesTemp = _totalImagesTemp - _numberOfImagesInAPage;
                yield return 1;   
            }
            
        }


        private void EnumeratorExecutionComplete(IAsyncResult result)
        {
            _callback(_GoogledImages);
        }

        private string GetGoogleSearchUrl(Int32 CurrentPage)
        {
            return String.Format(_GoogleImageSearchBaseUrl,
                _searchString, CurrentPage * _numberOfImagesInAPage,
                      _numberOfImagesInAPage   );
        }

        private IEnumerator<Int32> GetImagesFromGoogle(AsyncEnumerator ae,string searchUrl,Int32 imageNumber)
        {
            InitiateWebRequest(ae,searchUrl);
            yield return 1;
            String resultPage;
            using (WebResponse response = GetWebResponse(ae.DequeueAsyncResult()))
            {
                 resultPage = GetSearchResutlPage(response);  
            }
                IEnumerable<String> imageURIs = GetImageURIFromResultPage(resultPage) ;
                 int currentURI=0;
               
                 Int32 _downloaded=0;
                 while (_downloaded < imageNumber && currentURI < imageURIs.Count())
                 {
                     InitiateWebRequest(ae, imageURIs.ElementAt(currentURI++));
                     yield return 1;
                     try
                     {
                         using (WebResponse response = GetWebResponse(ae.DequeueAsyncResult()))
                         {
                             _GoogledImages.Add(GetImageFromResponse(response));
                             _downloaded++;
                         }
                     }
                     catch (Exception e)
                     {
                     }
                 }
            yield  break;
           }

        private static Image GetImageFromResponse(WebResponse response)
        {
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = GetMemoryStreamFromWebResponse(response);
            image.EndInit();
            Image result = new Image { Source = image };
            return result; 
        }

        private static MemoryStream GetMemoryStreamFromWebResponse(WebResponse response)
        {
            Stream stream = response.GetResponseStream();
            MemoryStream memStream = new MemoryStream();
            Byte[] bytes = new Byte[1024];
            int ReadBytes;
            do
            {
                ReadBytes = stream.Read(bytes, 0, bytes.Length);
                memStream.SetLength(memStream.Length + ReadBytes);
                memStream.Write(bytes, 0, ReadBytes);
            } while (ReadBytes > 0);
            memStream.Seek(0, SeekOrigin.Begin);
            return memStream;
        }

        private static WebResponse GetWebResponse(IAsyncResult result)
        {
            
            WebResponse response;
            WebRequest  request = result.AsyncState as WebRequest;
            response = request.EndGetResponse(result);
            return response;
 
        }

        private void InitiateWebRequest(AsyncEnumerator ae, string requestString)
        {
            WebRequest request = WebRequest.Create(requestString) as HttpWebRequest;
            request.BeginGetResponse(ae.EndVoid(0,DiscardWebRequest)  , request);  
        }

        private static string GetSearchResutlPage(WebResponse response)
        {
            StreamReader reader = new StreamReader(response.GetResponseStream());
            return reader.ReadToEnd();   
        }

        private static IEnumerable<String> GetImageURIFromResultPage(string resultPage)
        {
            Regex reg = new Regex(_GoogleImageURIPattern );
            MatchCollection matches = reg.Matches(resultPage);
            foreach (Match match in matches)
            {
                yield return GetImageURIFromMatch(match); 
            }
        }

        private static string GetImageURIFromMatch(Match match)
        {
            string result;
            int indexOfHttp = match.Value.IndexOf("http"); 
            result = match.Value.Remove(0, indexOfHttp);
            result = result.Remove(result.Length - 1, 1);
            return result;
        }

        private void DiscardWebRequest(IAsyncResult result)
        {
        }
    }
}
