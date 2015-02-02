using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows;
using System.Data.SqlClient;


namespace Vague2Vivid1.OCR
{
    public class ImageProcessing1
    {
        Bitmap inputImage, imgB2W,characterBitmap;
        private double BWThresh = 0.5;
        private String characters=null,s="*",wordTemp="";
        private String[] words;
        private int wordCount = 0;
        
        private double[][] data;
        private double[][] deg_value;


        public ImageProcessing1(Bitmap inputImage)
        {
            try
            {
                this.inputImage = new Bitmap(inputImage);
           
            imgB2W = new Bitmap(this.inputImage);
            }
            catch (Exception) { MessageBox.Show("No image to dispaly."); }
            // finally { this.inputImage = new Bitmap("../image/no_img.png"); }
            characterBitmap = new Bitmap("../image/blank.jpg");

            //testing without segmentation
            //this.characterBitmap = new Bitmap(inputImage);
            //imgB2W = new Bitmap(this.characterBitmap);
           // characterRecognisation();

            words = new String[20];
            


        
            letterSeperation();



            
          
               }
        //seperation of image into number of letters and 
        public void letterSeperation()
        {
            int hlines;

            Seperation seperate = new Seperation(imgB2W);
            hlines = seperate.getnoHLines();
            int[] vlines = new int[hlines];
            vlines = seperate.getnoVLines();

    //Console.WriteLine("no of hlines"+hlines);

         int v = 0,check;
    //   Console.WriteLine("no of words"+ seperate.calWords());
        //  MessageBox.Show("no of words" + seperate.calWords());
          
         for (int i = 0; i < (hlines); i = i + 2)
         
         {
             for (int j = 0; j < vlines[v]; j = j + 2)
             {
                
                 check = seperate.LetterImage(i,v, j);
                 if (check == 1)
                 {

                     characterBitmap = seperate.getCharacter();
                     
                     s+=characterRecognisation();
                     wordTemp += characterRecognisation();
                     if (seperate.getWordSeperation() == 1)
                     {
         //Console.WriteLine("hurray word seperated");
                         s += "*";
                         words[wordCount++] = wordTemp;
                         wordTemp = "";

                     }

                     //view each character
                     //if (j == 0)
                     //    return;

                    // Console.WriteLine("check the no of character");
                 }//end of check if 
              
              

             }//end of for j
             v++;
         }//end of for i


       Console.WriteLine("the result is " + s);
        
       if (s == null) { MessageBox.Show("The character(s) cannot be recognized."); }
        }
        public Bitmap character()
        {
            return characterBitmap;
        }
        public char characterRecognisation()
        {
            PixelExtraction pixelExtraction = new PixelExtraction();
            imgB2W = pixelExtraction.Img2BW(characterBitmap, BWThresh);
            Normalization normalization = new Normalization(pixelExtraction.getAllPoints(), pixelExtraction.getCountPoints());




            PCA pca = new PCA(); // added for the test

            pca.setTotalCount(pixelExtraction.getCountPoints());

            double[][] data = new double[5][];
            for (int i = 0; i < 5; i++)
            {
                data[i] = new double[5];
            }
            deg_value = new double[5][];
            for (int i = 0; i < 5; i++)
            {
                deg_value[i] = new double[2];
            }
            // 0 degree rotation
 //           Console.WriteLine("0 degree rotation");
            
            pca.pcaCalculation(normalization.getNormPoints());
            data[0] = pca.getEigenVector();
            deg_value[0] = pca.getEigenValue();
   //         Console.WriteLine("data[0]" + data[0][0] + "dafds" + data[0][1] + "0 3" + data[0][3] );


   //         Console.WriteLine("22 degree rotation");

            Normalization normalization1 = new Normalization(normalization.getNormPointsProjection22(), pixelExtraction.getCountPoints());
            pca.pcaCalculation(normalization1.getNormPoints());
            data[1] = pca.getEigenVector();
            deg_value[1] = pca.getEigenValue();


  //          Console.WriteLine("45 degree rotation");

            Normalization normalization3 = new Normalization(normalization.getNormPointsProjection45(), pixelExtraction.getCountPoints());
            pca.pcaCalculation(normalization3.getNormPoints());
            data[2] = pca.getEigenVector();
            deg_value[2] = pca.getEigenValue();

//            Console.WriteLine("67 degree rotation");

            Normalization normalization4 = new Normalization(normalization.getNormPointsProjection67(), pixelExtraction.getCountPoints());
            pca.pcaCalculation(normalization4.getNormPoints());
            data[3] = pca.getEigenVector();
            deg_value[3] = pca.getEigenValue();


//            Console.WriteLine("90 degree rotation");


            Normalization normalization5 = new Normalization(normalization.getNormPointsProjection90(), pixelExtraction.getCountPoints());
            pca.pcaCalculation(normalization5.getNormPoints());
            data[4] = pca.getEigenVector();
            deg_value[4] = pca.getEigenValue();

            DatabaseConnection connection = new DatabaseConnection();
            connection.retrieveDatabase(data, deg_value);
            //characters += connection.getRecognisedCharacter();
            return connection.getRecognisedCharacter();
 //          Console.WriteLine(" the main window characters" + characters);
        }
        public string getRecogniseCharacter()
        {
            return s;
        }
        public void wordDisplay()
        {
            for (int i = 0; i < wordCount; i++)
                Console.WriteLine("words " + words[i]);
            
        }
        public int getnoWords()
        {
            return wordCount;
        }
        public String[] getWords()
        {
            return words;
        }
        
        

        }
    }