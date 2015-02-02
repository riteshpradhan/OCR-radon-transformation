using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Windows;

namespace Vague2Vivid1.OCR
{
    class DatabaseConnection
    {
        System.Data.SqlClient.SqlConnection con = null;
        SqlDataReader reader;
        char[] character;
        double[][] data ,cData;
        double eud = 0;
        char recognisedChar;
        private double[] eigen_diff;
        double[][] deg_val;
        double[][] eigen;
        char[] recog_char;
        int[] char_id;

        public DatabaseConnection()
        {
            ///DATABASE connection starts here

            con = new System.Data.SqlClient.SqlConnection();
            con.ConnectionString = "Data Source=.\\SQLEXPRESS;AttachDbFilename=|DataDirectory|\\Feature_database.mdf;Integrated Security=True;Connect Timeout=30;User Instance=True";

            //Data Source=.\\SQLEXPRESS;AttachDbFilename=C:\\Users\\RITESH\\Desktop\\OCR Radon PCA 3\\Vague2Vivid1\\Vague2Vivid1\\bin\\Debug\\Feature_database.mdf;Integrated Security=True;Connect Timeout=30;User Instance=True
            character = new char[] { 'A', 'B' ,'C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z'};
           

            data = new double[5][];
            cData = new double[5][];
            for (int i = 0; i < 5; i++)
            {
                data[i] = new double[5];
            }
            for (int i = 0; i < 5; i++)
            {
                cData[i] = new double[5];
            }

            deg_val = new double[5][];
            for (int i = 0; i < 5; i++)
            {
                deg_val[i] = new double[2];
            }

            eigen_diff = new double[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }; //// changing top numbers

            eigen = new double[5][];
            for (int i = 0; i < 5; i++)
            {
                eigen[i] = new double[2];
            }

            recog_char = new char[10];          // changing top numbers

            char_id = new int[10];              // changing top numbers

        }
        public void retrieveDatabase(double[][] calData, double[][] deg_value )
        {
            System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
            cmd.CommandType = System.Data.CommandType.Text;

            cData = calData;
            deg_val = deg_value;
           
           for (int i = 1; i <= 78; i++)
        
            {


                #region retrieve rows from TblEigen
                con.Open();
                cmd.CommandText = "SELECT * FROM TblEigen " +
                                        " WHERE id = " + i + "";
                cmd.Connection = con;
                reader = cmd.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        data[0][0] = Convert.ToDouble(reader["eigen_0_0_0"]);
                        data[0][1] = Convert.ToDouble(reader["eigen_0_0_1"]);
                        data[0][2] = Convert.ToDouble(reader["eigen_0_1_0"]);
                        data[0][3] = Convert.ToDouble(reader["eigen_0_1_1"]);

                        data[1][0] = Convert.ToDouble(reader["eigen_22_0_0"]);
                        data[1][1] = Convert.ToDouble(reader["eigen_22_0_1"]);
                        data[1][2] = Convert.ToDouble(reader["eigen_22_1_0"]);
                        data[1][3] = Convert.ToDouble(reader["eigen_22_1_1"]);

                        data[2][0] = Convert.ToDouble(reader["eigen_45_0_0"]);
                        data[2][1] = Convert.ToDouble(reader["eigen_45_0_1"]);
                        data[2][2] = Convert.ToDouble(reader["eigen_45_1_0"]);
                        data[2][3] = Convert.ToDouble(reader["eigen_45_1_1"]);

                        data[3][0] = Convert.ToDouble(reader["eigen_67_0_0"]);
                        data[3][1] = Convert.ToDouble(reader["eigen_67_0_1"]);
                        data[3][2] = Convert.ToDouble(reader["eigen_67_1_0"]);
                        data[3][3] = Convert.ToDouble(reader["eigen_67_1_1"]);

                        data[4][0] = Convert.ToDouble(reader["eigen_90_0_0"]);
                        data[4][1] = Convert.ToDouble(reader["eigen_90_0_1"]);
                        data[4][2] = Convert.ToDouble(reader["eigen_90_1_0"]);
                        data[4][3] = Convert.ToDouble(reader["eigen_90_1_1"]);

                        //                      Console.WriteLine("checking data from database: " + data[0][0]);
                    }
                }
                catch (Exception s) { MessageBox.Show("Error " + s); }
                finally
                {
                    con.Close();
                }
                #endregion // retrieve rows finished



                #region // calculating euclidean distance
                //calculating euclidian distance
                for (int k = 0; k < 5; k++)
                    for (int j = 0; j < 4; j++)
                    {
                        eud += Math.Pow((data[k][j] - cData[k][j]), 2);
                        //Console.WriteLine("euud" + eud);
                        //Console.WriteLine("data[" + k + "][" + j + "=" + data[k][j]);
                        //Console.WriteLine("cdata[" + k + "][" + j + "=" + cData[k][j]);

                    }

                //           Console.WriteLine(" The euclidian distance with {0} is: {1}", character[i-1], eud);
                #endregion
                //end of calc euclidian distance


                #region // updating vector error to the TBlEigen

                con.Open();

                cmd.CommandText = "UPDATE TblEigen SET " +
                                       " error = " + eud +
                                        " WHERE id = " + i + "";
                cmd.Connection = con;

                try
                {
                    cmd.ExecuteNonQuery();

                }
                catch (Exception e)
                {
                    MessageBox.Show("eud update error \n" + e);
                }

                finally
                {
                    con.Close();
                }
                #endregion

                eud = 0;  


            }//end of for loop

           #region getting top 10rows on error basis
           ///getting top 10 rows with minimmum error in next table by SQL

           //MessageBox.Show("this next databse table open");
           con.Open();
           cmd.CommandText = "SELECT DISTINCT " +
                                    " TOP (10) id, Character, eigen_val_0_1, eigen_val_0_2, " +             // changing top numbers
                                    " eigen_val_22_1, eigen_val_22_2, " +
                                    " eigen_val_45_1, eigen_val_45_2, " +
                                    " eigen_val_67_1, eigen_val_67_2, " +
                                    " eigen_val_90_1, eigen_val_90_2, error " +
                                    " FROM            TblEigen " +
               //" WHERE        (error < 10) " +
                                    " ORDER BY error ";
           reader = cmd.ExecuteReader();
           try
           {
               int k = 0;
               while (reader.Read())
               {
                   eigen[0][0] = Convert.ToInt32(reader["eigen_val_0_1"]); // character[0] vaneko haamile paaune character
                   eigen[0][1] = Convert.ToInt32(reader["eigen_val_0_2"]);
                   eigen[1][0] = Convert.ToInt32(reader["eigen_val_22_1"]);
                   eigen[1][1] = Convert.ToInt32(reader["eigen_val_22_2"]);
                   eigen[2][0] = Convert.ToInt32(reader["eigen_val_45_1"]);
                   eigen[2][1] = Convert.ToInt32(reader["eigen_val_45_2"]);
                   eigen[3][0] = Convert.ToInt32(reader["eigen_val_67_1"]);
                   eigen[3][1] = Convert.ToInt32(reader["eigen_val_67_2"]);
                   eigen[4][0] = Convert.ToInt32(reader["eigen_val_90_1"]);
                   eigen[4][1] = Convert.ToInt32(reader["eigen_val_90_2"]);


                   recog_char[k] = Convert.ToChar(reader["Character"]);
                   //recognisedChar = Convert.ToChar(reader["Character"]);
                   char_id[k] = Convert.ToInt32(reader["id"]);

                   eigen_diff[k] = Math.Pow((eigen[0][0] - deg_val[0][0]), 2) + Math.Pow((eigen[0][1] - deg_val[0][1]), 2);
                   eigen_diff[k] += Math.Pow((eigen[1][0] - deg_val[1][0]), 2) + Math.Pow((eigen[1][1] - deg_val[1][1]), 2);
                   eigen_diff[k] += Math.Pow((eigen[2][0] - deg_val[2][0]), 2) + Math.Pow((eigen[2][1] - deg_val[2][1]), 2);
                   eigen_diff[k] += Math.Pow((eigen[3][0] - deg_val[3][0]), 2) + Math.Pow((eigen[3][1] - deg_val[3][1]), 2);
                   eigen_diff[k] += Math.Pow((eigen[4][0] - deg_val[4][0]), 2) + Math.Pow((eigen[4][1] - deg_val[4][1]), 2);
                   eigen_diff[k] = Math.Sqrt(eigen_diff[k]);
                   //Console.WriteLine("the eigen_diff for : {0} is : {1}", recog_char[k], eigen_diff[k]);
                   k++;
               }
           }
           catch (Exception e)
           {
               MessageBox.Show("getting eigen value from table \n" + e);
           }
           finally
           {
               con.Close();
           }

           #endregion // end of getting top 10 rows with min vector error

           #region  //update TblEigenError


           int length = 10;                 // changing top numbers
           Console.WriteLine();
           for (int i = 0; i < length; i++)
           {
               con.Open();
               Console.WriteLine("char: {0}   dist: {1}", recog_char[i], eigen_diff[i]);
               int j = i + 1;
               cmd.CommandText = "UPDATE TblEigenError SET " +
                                   " Char = '" + recog_char[i] + "' " +
                                   " , eigen_dist = " + eigen_diff[i] + " " +
                                   " WHERE id = " + j + " ";
               cmd.Connection = con;
               try
               {
                   cmd.ExecuteNonQuery();
               }
               catch (Exception e)
               {
                   MessageBox.Show("Msg update eigenvalue distance " + e);
               }
               finally
               {
                   con.Close();
               }
               //if ((cmd.ExecuteNonQuery()) != 0) { Console.WriteLine("eigen value ko update vayo ni ..."); }

           }

           #endregion

           #region // getting char with minimum eigenvector distance
           //con.Open();
           //cmd.CommandText = "SELECT Character FROM TblEigen " +
           //                           " WHERE error = (SELECT MIN(error) FROM TblEigen)";
           //cmd.Connection = con;


           //reader = cmd.ExecuteReader();

           //try
           //{

           //    while (reader.Read())
           //    {
           //        recognisedChar = Convert.ToChar(reader["Character"]);
           //    }
           //}
           //catch (Exception s) { MessageBox.Show("min eud char error \n " + s); }
           //finally
           //{
           //    con.Close();
           //}

           //Console.WriteLine(" This character is {0}:",recognisedChar);
           #endregion //end of the getting char with MIN eigenvector err

           #region// getting char with minimum value and vector distance
           //minimum error selection
           con.Open();
           cmd.CommandText = "SELECT Char FROM TblEigenError " +
                                     " WHERE eigen_dist = (SELECT MIN(eigen_dist) FROM TblEigenError)";
           cmd.Connection = con;
           reader = cmd.ExecuteReader();

           try
           {

               while (reader.Read())
               {
                   character[0] = Convert.ToChar(reader["Char"]); // character[0] vaneko haamile paaune character
                   recognisedChar = Convert.ToChar(reader["Char"]);
               }
           }
           catch (Exception s) { MessageBox.Show("minimum eigen dist error \n " + s); }
           finally
           {
               con.Close();
           }

           Console.WriteLine(" This character is {0}: {1}: {2} ", character[0], recognisedChar, character[0]);
            //////
           #endregion


        }
        public char getRecognisedCharacter()
        {
             return recognisedChar;
        }
    }
}
