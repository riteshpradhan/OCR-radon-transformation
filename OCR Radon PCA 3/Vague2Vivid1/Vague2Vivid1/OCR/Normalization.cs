using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Vague2Vivid1.OCR
{
    class Normalization
    {
         int stdpoints = 50;
         int numPixels;
        float normcoeff;
        private Point[] edgePoints,normPoints;
        float radius;
        float[] centroid;
        //private Point edgePoint;
       
        //private int count;  

        private float cos45, sin45, cos22, sin22, sec22, cos67, sin67, sqrt2, halfsqrt2, halfsqrt2divsin22, doublesqrt2;

        public Normalization(Point[] edgePoints, int numPixels)
        {
            //initialize math variables once	
            sqrt2 = (float)Math.Sqrt(2);
            halfsqrt2 = (float)0.5 * sqrt2;
            halfsqrt2divsin22 = (float)(0.5 * sqrt2 / Math.Sin(Math.PI / 8));
            doublesqrt2 = 2 * sqrt2;
            cos45 = sqrt2 / 2;
            sin45 = cos45;
            cos22 = (float)Math.Cos(Math.PI / 8);
            sin22 = (float)Math.Sin(Math.PI / 8);
            sec22 = 1 / cos22;
            cos67 = (float)Math.Cos(3 * Math.PI / 8);
            sin67 = (float)Math.Sin(3 * Math.PI / 8);


            this.edgePoints = new Point[5000000];
            normPoints = new Point[5000000];

            this.edgePoints = edgePoints;
            this.numPixels = numPixels;
            
           
            getRadius();
        
            normalize();


            
        }

        public void normalize()
        {
            
            

            normcoeff = stdpoints / radius;
            for (int i = 0; i < numPixels; i++)
            {
                normPoints[i].X = (int)((edgePoints[i].X - centroid[0]) * normcoeff);
                normPoints[i].Y = (int)((edgePoints[i].Y -centroid[1]) * normcoeff);
            }
        }


        public Point[] getNormPoints()
        {
            return normPoints;
        }

        //rotation 
        public Point[] getNormPointsProjection22()
        {
            Point[] edgeProjection = new Point[numPixels];


            for (int i = 0; i < numPixels; i++)
            {
                edgeProjection[i].X = (int)((edgePoints[i].X - centroid[0]) * cos22 + (edgePoints[i].Y - centroid[1]) * sin22);
                edgeProjection[i].Y = (int)(-(edgePoints[i].X - centroid[0]) * sin22 + (edgePoints[i].Y - centroid[1]) * cos22);
            }
            return edgeProjection;
        }
          public Point[] getNormPointsProjection45()
        {
            Point[] edgeProjection = new Point[numPixels];
            
            for (int i = 0; i < numPixels; i++)
            {
                edgeProjection[i].X = (int)((edgePoints[i].X - centroid[0]) * cos45 + (edgePoints[i].Y - centroid[1]) * sin45);
                edgeProjection[i].Y = (int)(-(edgePoints[i].X - centroid[0]) * sin45 + (edgePoints[i].Y - centroid[1]) * cos45);
            }
            return edgeProjection;
        }
          public Point[] getNormPointsProjection67()
          {
              Point[] edgeProjection = new Point[numPixels];

              for (int i = 0; i < numPixels; i++)
              {
                  edgeProjection[i].X = (int)((edgePoints[i].X - centroid[0]) * cos67 + (edgePoints[i].Y - centroid[1]) * sin67);
                  edgeProjection[i].Y = (int)(-(edgePoints[i].X - centroid[0]) * sin67 + (edgePoints[i].Y - centroid[1]) * cos67);
              }
              return edgeProjection;
          }
          public Point[] getNormPointsProjection90()
          {
              Point[] edgeProjection = new Point[numPixels];

              for (int i = 0; i < numPixels; i++)
              {
                  edgeProjection[i].X = (int)(+(edgePoints[i].Y - centroid[1]));
                  edgeProjection[i].Y = (int)(-(edgePoints[i].X - centroid[0]));
              }
              return edgeProjection;
          }




        public void getRadius()
        {

            //set radius to 0
            radius = 0;
            float sqrt2 = (float)Math.Sqrt(2);
            float halfsqrt2 = (float)0.5 * sqrt2;
          
           Point[] objects = edgePoints;
           centroid = new float[2];
           getCentroid();
            //find the max length from centroid to a pixel
            for (int i = 0; i < numPixels; i++)
            {
                //euclidian distance from pixel to centroid (no sqrt for performance reasons)
                double len = Math.Pow((double)(objects[i].X - centroid[0]), 2) + Math.Pow((double)(objects[i].Y - centroid[1]), 2);

                //set radius to longest distance found
                radius = Math.Max(radius, (float)len);
            }

            //calculate radius (include missing sqrt from above) to a midpoint of a pixel
            //Note: since distance is to midpoint add sqrt2/2 (approximation)
            radius = (float)(Math.Sqrt((float)radius) + halfsqrt2);
            //return radius;

        }

        public void getCentroid()
        {
            
           
            centroid[0] = 0;
            centroid[1] = 0;
            //Console.WriteLine("the no of pixel " + numPixels);
            for (int i = 0; i < numPixels; i++)
            {
                // Console.WriteLine("Edgepoints: {0} ", objects[i]);
                centroid[0] += edgePoints[i].X;
                centroid[1] += edgePoints[i].Y;


            }
            centroid[0] = centroid[0] / numPixels;
            centroid[1] = centroid[1] / numPixels;
            Console.WriteLine("Before Normalization Centroid: " + centroid[0] + ":" + centroid[1]);
           // return centroid;
        }
            
    }
}
