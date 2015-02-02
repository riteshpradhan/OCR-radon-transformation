using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Vague2Vivid1.OCR
{
  
        class Seperation
        {
            Bitmap imgOut,imgChar,letter;
            int[,] GreyImage;
            int[]  hHisto;
            /// <summary>
            /// c1=line no  c2=character no 
            /// </summary>
            int[,] vHisto,bottom;
            int[] top,word,noVlines;
            int width, height,noHLines=0;
            Color pixel;
            int wordSep=0;
        
            
        public Seperation(Bitmap imgSrc)
        {

    
            imgOut = new Bitmap(imgSrc);
            imgOut=imgSrc;
           
           
            width = imgSrc.Width;
            height = imgSrc.Height;
            GreyImage = new int[width, height];
            
            hHisto = new int[height];

           //top=horizontal line drawn to seperate lines 
            top = new int[200];

            //no of words in th image
            word = new int[200];
            
            //to calculate the histogram
            binarization(imgSrc);
           
            
           
              //calculation of horizontal histogram
               HHisto();

               //line Seperation
               lineSeperation();




               //calculation of vertical histogram
               vHisto = new int[noHLines, width];
               bottom = new int[noHLines, 200];
               noVlines = new int[100];

               VHisto();
             




               
         }

        #region binarization
        private void binarization(Bitmap imgSrc)
        {
           
            int px; double br;
            double threshold = 0.5;
            
            for (int row = 0; row < height - 1; row++)
            {
                for (int col = 0; col < width - 1; col++)
                {
                    pixel = imgSrc.GetPixel(col, row);
                    px = pixel.ToArgb();
                    br = pixel.GetBrightness();
                    if (pixel.GetBrightness() < threshold)
                    {

                        GreyImage[col, row] = 0;

                    }
                    else
                    {

                        GreyImage[col, row] = 1;
                    }

                }
            }
        }
        #endregion
        private int characterSeperation(int a)
        {
            //j=letter count in each line
            int j = 0, s = 0;
            
            
            for (int i = 1; i < width; i++)
            {
                
                if (vHisto[a,i - 1] <4)
                {
                    
                     s++;
                    if (vHisto[a, i - 1] < vHisto[a, i] && vHisto[a, i] > 3)
                    {
                        if (j != 0|| i == width - 1)
                        {
                            s = i - s;
                            bottom[a, j] = s + 1;

                            j++;

                        }
                        if (i != width - 1)
                        {
                            bottom[a, j] = i-1;
                            j++;
                        }
                       

                      
                        s = 0;
                        
                    }
                    
                }
              
                


            }
            return j;
        }
     

        private void lineSeperation()
        {
             
            int s = 0;


            for (int i = 1; i < height; i++)
            {
                

    //Console.WriteLine("horizontal histogram["+i+"]=" + hHisto[i]);
                if (hHisto[i - 1] <= 3)
                {
                    s++;
                 
                    if (hHisto[i - 1] < hHisto[i])
                    {
                        if (noHLines != 0 || i == height - 1)
                        {
                            top[noHLines] = i - s + 1;
                            Console.WriteLine("top[" + noHLines + "]=" + top[noHLines]);

                            noHLines++;
                        }
                        if (i != height - 1)
                        {

                            top[noHLines] = i-1;
                            Console.WriteLine("top[" + noHLines + "]=" + top[noHLines]);
                           
                            noHLines++;
                        }
                       
                        
                        
                        s = 0;


                    }// if (hHisto[i - 1] < hHisto[i])

                   
                }//if (hHisto[i - 1] <= 3)


            }

        }

        #region horizontal histogram
        public void HHisto()
        {




            int m, n;


            for (m = 0; m < height; m++)
            {
                for (n = 0; n < width; n++)
                {


                    if (GreyImage[n, m] == 0)
                    {
                        hHisto[m] = hHisto[m] + 1;
                    }


                }

            }

        }

#endregion


        public void VHisto()
        {
            int linePos=0;
            for(int i=0;i<(noHLines);i=i+2)
           {
                for (int m = top[i]; m < top[i + 1]; m++)
                    for (int n = 0; n < width; n++)
                        if (GreyImage[n, m] == 0)
                            vHisto[linePos, n] = vHisto[linePos, n] + 1;     //calculation of vertical histogram in each line 
                noVlines[linePos] = characterSeperation(linePos);
   Console.WriteLine("no of characters" + noVlines[linePos]);
               linePos++;

            }
        
        }
        
        #region bitmap of each letter
        public int LetterImage(int v,int i, int j)
        {

            wordSep = 0;
           
            if ((bottom[i, j + 1] - bottom[i, j]) < 6 || (top[v + 1] - top[v]) < 4)
                return 0;
            if ((j + 2) <= noVlines[i]-1)
            {
                if ((bottom[i, j + 2] - bottom[i, j + 1]) > 25)
                    wordSep = 1;
            }
            else
                wordSep = 1;
            letter = new Bitmap((bottom[i, j + 1] - bottom[i, j]), (top[v + 1] - top[v]), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
           Rectangle rect = new Rectangle(bottom[i, j], top[v], (bottom[i, j + 1] - bottom[i, j]), (top[v + 1] - top[v]));
           letter = imgOut.Clone(rect, imgOut.PixelFormat);



            return 1;
            
        }
        #endregion 
        public int getnoHLines()
        {
            return noHLines;
        }
        public int[] getnoVLines()
        {
            return noVlines;
        }
            //return 1 if word is seperated...
        public int getWordSeperation()
        {
            return wordSep;
        }
        public Bitmap getCharacter()
        {
            return letter;
        }

        //just a check to count no of words.....
        #region just a check to count no of words

        public int calWords()
         {
             int GrapWords = 0,temp=1;
             for (int i = 0; i < (noHLines/2); i++)
             {
                 if (temp != GrapWords)
                     GrapWords++;
                 temp = GrapWords;

                 for (int j = 1; j < (noVlines[i] - 2); j = j + 2)
                 {

                     Console.WriteLine("sep" + (bottom[i, j+1] - bottom[i, j ]));
                     if ((bottom[i, j+1] - bottom[i, j ]) > 25)
                     {
                         GrapWords++;
                         Console.WriteLine("word count"+GrapWords);
                         
                     }
                         
                 

                 }
               
             }
           
             return GrapWords;
         }
        #endregion 



        }
    }


