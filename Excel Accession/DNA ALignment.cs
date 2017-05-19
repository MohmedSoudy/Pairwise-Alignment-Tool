using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Excel_Accession
{
    public class DNA_ALignment
    {
        public static double[,] M;
        public static double[,] Ix;
        public static double[,] Iy;
        public const double NegativeInfinity = -1.0 / 0.0;
        static double match = 2;
        static double mismatch = -1;
        public static string DNA_Sequence1;
        public static string DNA_Sequence2;
        public static string Matches_1;
        public static int Gap_Cost;
        public static int Gap_opening_Cost;
        public static string Seq1_Align = "";
        public static string Seq2_Align = "";
        public static string Optimal1;
        public static string Optimal2;
        public static int[] All_Optimal = new int[100];                 //Local Alignment 
        public static int[,] Matrix;
        public static string hor = "";
        public static string ver = "";
        static public void Reset()
        {
            Seq1_Align = "";
            Seq2_Align = "";
            All_Optimal = new int[100];                 //Local Alignment 
            hor = "";
            ver = "";
        }
        public static double max3(double i1, double i2, double i3)
        {
            return Math.Max(i3, Math.Max(i2, i1));
        }
        public static int[,] LCS(string str1, string str2, int gab)
        {
            int m = str1.Length;
            int n = str2.Length;

            int[,] mat = new int[m + 1, n + 1];
            for (int i = 0; i <= m; i++)
            {
                mat[i, 0] = gab * i;
            }
            for (int j = 0; j <= n; j++)
            {
                mat[0, j] = gab * j;
            }
            for (int i = 1; i <= m; i++)
            {
                for (int j = 1; j <= n; j++)
                {
                    int MATCH = mat[i - 1, j - 1] + Get_Score(str1[i - 1], str2[j - 1]);
                    int delete = mat[i - 1, j] + gab;
                    int insert = mat[i, j - 1] + gab;
                    int max = Math.Max(MATCH, insert);
                    mat[i, j] = Math.Max(max, delete);
                }
            }
            return mat;
        }
        public static void InitializeAffine(string s1, string s2, int h, int g)
        {
            int m = s1.Length;
            int n = s2.Length;
            M = new double[m + 1, n + 1];
            Ix = new double[m + 1, n + 1];
            Iy = new double[m + 1, n + 1];


            for (int i = 0; i <= m; i++)
            {
                for (int j = 0; j <= n; j++)
                {
                    M[i, j] = NegativeInfinity;
                    Ix[i, j] = NegativeInfinity;
                    Iy[i, j] = NegativeInfinity;
                }
            }
            if (m != 0 && n != 0)
                M[0, 0] = 0;
            for (int i = 0; i <= m; i++)
            {
                Ix[i, 0] = h + g * i;
            }

            for (int i = 0; i <= n; i++)
            {
                Iy[0, i] = h + g * i;
            }
        }
        public static void Fill_Affine(string s1, string s2, int h, int g)
        {
            int m = s1.Length;
            int n = s2.Length;
            M = new double[m + 1, n + 1];
            Ix = new double[m + 1, n + 1];
            Iy = new double[m + 1, n + 1];
            InitializeAffine(s1, s2, h, g);
            for (int i = 1; i <= m; i++)
            {
                for (int j = 1; j <= n; j++)
                {
                    if (s1[i - 1] == s2[j - 1])
                    {
                        M[i, j] = max3(M[i - 1, j - 1] + match, Ix[i - 1, j - 1] + mismatch, Iy[i - 1, j - 1] + mismatch);
                    }
                    else
                    {
                        M[i, j] = max3(M[i - 1, j - 1] + mismatch, Ix[i - 1, j - 1] + mismatch, Iy[i - 1, j - 1] + mismatch);          //e7tmal te7tag tazbit
                    }
                    Ix[i, j] = Math.Max(M[i - 1, j] + h + g, Ix[i - 1, j] + g);
                    Iy[i, j] = Math.Max(M[i, j - 1] + h + g, Iy[i, j - 1] + g);
                }
            }
        }
        public static string traceBack_Affine_DNA(double[,] g, string s1, string s2, int i, int j)
        {

            if (i == 0 || j == 0)
                return "";
            if (M[i, j] >= Ix[i, j] && M[i, j] >= Iy[i, j])
            {
                hor += s2[j - 1];
                ver += s1[i - 1];
                return traceBack_Affine_DNA(M, s1, s2, i - 1, j - 1);
            }

            else if (Iy[i, j] >= Ix[i, j] && Iy[i, j] >= M[i, j])
            {
                hor += s2[j - 1];
                ver += "-";
                return traceBack_Affine_DNA(Iy, s1, s2, i, j - 1);
            }
            else if (Ix[i, j] >= Iy[i, j] && Ix[i, j] >= M[i, j])
            {
                hor += "-";
                ver += s1[i - 1];
                return traceBack_Affine_DNA(Ix, s1, s2, i - 1, j);
            }
            else
                return "";
        }
        public static string Reverse_String(string Sequence)
        {
            char[] sequence = Sequence.ToCharArray();
            Array.Reverse(sequence);
            return new string(sequence);
        }
        public static int Get_Score(char Seq1, char Seq2)               //Matches and Mismatch
        {
            if (Seq1 == Seq2)
            {
                return 1;          //Match
            }
            return -1;             //DisMatch
        }
        public static int Value_or_Zero(int Value)    // Return the value or return zero 
        {
            if (Value > 0)
            {
                return Value;
            }
            return 0;
        }
        public static int Get_Max(int match, int insert, int delete)
        {
            if (match > insert)
            {
                if (match > delete)
                {
                    return match;
                }
                return delete;
            }
            if (insert > delete)
            {
                return insert;
            }
            return delete;
        }
        public static void Matches(string Sequence1, string Sequence2)
        {
            Matches_1 = "";
            for (int i = 0; i < Sequence1.Length; i++)
            {
                if (Sequence1[i] == Sequence2[i])
                {
                    Matches_1 += "|  ";
                }
                else
                    Matches_1 += ".  ";
            }
        }
        public static void Get_index(int[,] mat, int max)           //Get indcies of max
        {
            int index1 = 0;
            int index2 = 1;
            int r = DNA_Sequence1.Length + 1;
            int c = DNA_Sequence2.Length + 1;
            for (int i = 0; i < r; i++)
            {
                for (int j = 0; j < c; j++)
                {
                  if (j > r - 1 || i > c - 1)
                  {
                      break;
                  }
                    if (mat[i, j] == max)
                    {
                        if (index1 < i && index2 < j + 1)
                        {
                            All_Optimal[index1] = i;
                            All_Optimal[index2] = j;
                            index1 += 2;
                            index2 += 2;
                        }
                    }
                }
            }
        }                                                    //End Get index


        public static int[,] Fill_Matrix(string Sequence1, string Sequence2, int GAP)
        {
            int Match, Insert, Delete = 0;
            Matrix = new int[Sequence1.Length + 1, Sequence2.Length + 2];
            for (int i = 0; i < Sequence1.Length + 1; i++)          //Intialize Row 1 With Zeros
            {
                Matrix[i, 0] = 0;
            }//end for Rows
            for (int j = 0; j < Sequence2.Length + 1; j++)          //Intialize Col 1 With Zeros
            {
                Matrix[0, j] = 0;
            }//end for Col
            for (int i = 1; i < Sequence1.Length + 1; i++)
            {
                for (int j = 1; j < Sequence2.Length + 1; j++)
                {
                    //Match case 1
                    Match = Matrix[i - 1, j - 1] + Get_Score(Sequence1[i - 1], Sequence2[j - 1]);
                    Match = Value_or_Zero(Match);
                    //Insert case 2
                    Insert = Matrix[i - 1, j] + GAP;
                    Insert = Value_or_Zero(Insert);
                    //Delete case 3
                    Delete = Matrix[i, j - 1] + GAP;
                    Delete = Value_or_Zero(Delete);
                    Matrix[i, j] = Get_Max(Match, Insert, Delete);
                }                                   //End Col
            }                                       //End Rows

            return Matrix;
        }    //End FillMatrix

        public static string back_track(int[,] Mat, string sequence1, string sequence2, int i, int j)
        {
            //var Max = (from int Element in Mat select Element).Max();            // Trace back from maximum val in array 
             if (i > sequence1.Length || j > sequence2.Length|| i > sequence2.Length || j > sequence1.Length)
            {
                return "";
            }
            else if (Mat[i, j] == 0)
                return "";
            if (i == 0 || j == 0)
                return "";


             if (sequence1[i - 1] == sequence2[j - 1])
            {
                Seq1_Align += sequence1[i - 1];
                Seq2_Align += sequence1[i - 1];
                return back_track(Mat, sequence1, sequence2, i - 1, j - 1);
            }
            else
            {
                if (Mat[i - 1, j] > Mat[i, j - 1])
                {
                    Seq1_Align += "-";
                    Seq2_Align += sequence2[j - 1];
                    return back_track(Mat, sequence1, sequence2, i - 1, j);
                }
                else
                {
                    Seq1_Align += sequence1[i - 1];
                    Seq2_Align += "-";
                    return back_track(Mat, sequence1, sequence2, i, j - 1);
                }

            }
        }
    }
}
