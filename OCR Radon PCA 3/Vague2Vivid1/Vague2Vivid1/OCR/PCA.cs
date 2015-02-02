using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;


namespace Vague2Vivid1.OCR
{
    class PCA
    {
        private Point edgePoint;
        private Point[] edgePoints;
        private PointF[] intermediatePoints;
        private double[][] covMatrix,V,vchek;
        private double[] d;

        int totalCount;   
        float meanX=0,meanY=0,covtempX=0,covtempY=0;

        //Canny canny = new Canny();
        
        
        //public PCA() { }
        public PCA()
        {
            edgePoints = new Point[5000000];

           
            //edgePoint = new Point();
            //edgePoints = new Point[2000];
            //totalCount = canny.getCount();
           
        }
        public void pcaCalculation(Point[] edgePoints)
        {
            this.edgePoints=edgePoints;
            mean();
            subtractFromMean();
            covariance();
            
        }
        public void setTotalCount(int count)
        {
            totalCount = count;
        }
        public void mean()
        {
           
            //edgePoints = canny.getEdgePoints();
            //totalCount = canny.getCount();
            
          //  Console.WriteLine("this is the testing edge point total : X = {0}, Y = {1}", edgePoints[0].X, edgePoints[0].Y);

            for (int i = 0; i < totalCount; i++)
            {
                meanX += edgePoints[i].X;
                meanY += edgePoints[i].Y;

            }
            meanX = meanX / totalCount;
            meanY = meanY / totalCount;
            //check check
            Console.WriteLine("Check it MeanX: " + meanX);
            Console.WriteLine("Check it MeanY: " + meanY);

        }

        public void subtractFromMean()
        {
            intermediatePoints = new PointF[50000];
            for (int i = 0; i < totalCount; i++)
            {
                intermediatePoints[i].X = edgePoints[i].X - meanX;
                intermediatePoints[i].Y = edgePoints[i].Y - meanY;

            }
         //   Console.WriteLine("First intermediate Point: {0}", intermediatePoints[0]);
        }
        public double[] getEigenVector()
        {
            double[] a = new double[4];
            int k = 0;
            for (int i = 0; i < 2; i++)
                for (int j = 0; j < 2; j++)
                {
                    a[k] = V[i][j];
                    k++;
                }
            return a;
        }
        public double[] getEigenValue()
        {
            return d;
        }

        public void covariance()
        {
            covMatrix = new double[2][];
            for (int i = 0; i < 2; i++)
                covMatrix[i] = new double[2];
            V = new double[2][];

            for (int i = 0; i < 2; i++)
                V[i] = new double[2];
            d = new double[2];


        //    //covMatrix[0][0] = 0;
        //    //covMatrix[0][1] = 0;
        //    //covMatrix[1][0] = 0;
        //    //covMatrix[1][1] = 0;


            for (int i = 0; i < totalCount; i++)
            {
                covMatrix[0][0] += (intermediatePoints[i].X - meanX) * (intermediatePoints[i].X - meanX);
                covMatrix[0][1] += (intermediatePoints[i].X - meanX) * (intermediatePoints[i].Y - meanY);
                covMatrix[1][0] = covMatrix[0][1];
                covMatrix[1][1] += (intermediatePoints[i].Y - meanY) * (intermediatePoints[i].Y - meanY);
            }
            covMatrix[0][0] = covMatrix[0][0] / (totalCount - 1);
            covMatrix[0][1] = covMatrix[0][1] / (totalCount - 1);
            covMatrix[1][0] = covMatrix[1][0] / (totalCount - 1);
            covMatrix[1][1] = covMatrix[1][1] / (totalCount - 1);

           // Console.WriteLine(" testing covariance matrix: {0} {1} {2} {3}", covMatrix[0][0], covMatrix[0][1], covMatrix[1][0], covMatrix[1][1]);
          //  Console.WriteLine(" matrix testing : {0}", covMatrix);
           
            //yeta baata eigenvalue ra eigenvector niskincha

            GeneralMatrix generalMatrix = new GeneralMatrix(covMatrix);

            EigenvalueDecomposition eigenvalueDecomposition = new EigenvalueDecomposition(generalMatrix);

            d = eigenvalueDecomposition.getD(); // get eigen values

            V = eigenvalueDecomposition.getV();  // get eigen vectors
            //for (int i = 0; i < 2; i++)
                //Console.WriteLine("eigenvalue: " + d[i]);
            //for (int i = 0; i < 2; i++)
            //{
            //    for (int j = 0; j < 2; j++)
            //        Console.WriteLine("eigenvector " + V[i][j]);
            //}

        }            
    }
}
