using System;
using System.Collections.Generic;

using System.Text;

using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
//using System.Windows.Media;
using System.Drawing;

using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;



using System.Collections;

namespace Vague2Vivid1.OCR
{
    class PixelExtraction
    {
	    private Bitmap OriginalImage;
        private Bitmap imgOut;
        private double BWThresh = 0.5;
        private System.Drawing.Color cblack = System.Drawing.Color.FromArgb(255, 0, 0, 0);
        private System.Drawing.Color cwhite = System.Drawing.Color.FromArgb(255, 255, 255, 255);
        private ArrayList outList;
        private List<Vertex> vertices = new List<Vertex>();
         private Point [] allPoints;
         private int countPoints=0;


        
        public PixelExtraction()
        {
          
            ShowVertexList(vertices);
        }


        /* Convert the image into binary image*/
        public Bitmap Img2BW(Bitmap imgSrc, double threshold)
        {
            int width = imgSrc.Width;
            int height = imgSrc.Height;
            int px; double br;
            Color pixel;
            Bitmap imgOut = new Bitmap(imgSrc);
            allPoints = new Point[5000000];

            for (int row = 0; row < height - 1; row++)
            {
                for (int col = 0; col < width - 1; col++)
                {
                    pixel = imgSrc.GetPixel(col, row);
                    px = pixel.ToArgb();
                    br = pixel.GetBrightness();
                    if (pixel.GetBrightness() < threshold)
                    {
                        imgOut.SetPixel(col, row, cblack);
                        vertices.Add(new Vertex(col, row));
                        allPoints[countPoints].X = col;
                        allPoints[countPoints].Y = row;
                        countPoints++;
                    }
                    else
                        imgOut.SetPixel(col, row, cwhite);
                }
            }
            
            return imgOut;
        }

        public Point[] getAllPoints()
        {
            return allPoints;
        }
        public int getCountPoints()
        {
            return countPoints;
        }

        class Vertex
        {
            private int x;
            private int y;
            public Vertex(int i, int j)
            {
                this.X = i;
                this.Y = j;
            }
            public int X
            {
                get { return x; }
                set { x = value; }
            }
            public int Y
            {
                get { return y; }
                set { y = value; }
            }
        }


        private void ShowVertexList(List<Vertex> vertices)
        {
            /*
             * Show the vertex in ListBox
             */
            //foreach (Vertex vert in vertices)
            //{
            //    Console.WriteLine("{" + vert.X.ToString() + "," + vert.Y.ToString() + "}");
            //}

        }

    }
}
