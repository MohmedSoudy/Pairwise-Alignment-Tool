using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Excel_Accession
{
    public class Protein_Alignment
    {
        public static double[,] M;
        public static double[,] Ix;
        public static double[,] Iy;
        public const double NegativeInfinity = -1.0 / 0.0;
        public static double match = 2;
        public static double mismatch = -1;
        public static string Protein_Sequence1;
        public static string Protein_Sequence2;
        public static int Gap_Cost;
        public static int[] All_Optimal1 = new int[100];                 //Local Alignment 
        public static int Gap_opening_Cost;
        public static string Seq1_Align = "";
        public static string Seq2_Align = "";
        public static string hor = "";
        public static string ver = "";

        public static string Optimal1;
        public static string Optimal2;
        public static int score(char a, char b)
        {
            string str1 = "ARNDCQEGHILKMFPSTWYVBJZX*";
            string str2 = "ARNDCQEGHILKMFPSTWYVBJZX*";
            int x = str1.IndexOf(b);
            int y = str2.IndexOf(a);
            int[,] BLOSUM80_matrix = new int[,] { { 5, -2, -2, -2, -1, -1, -1, 0, -2, -2, -2, -1, -1, -3, -1, 1, 0, -3, -2, 0, -2, -2, -1, -1, -6 },
                                                  {-2,  6, -1, -2, -4, 1,-1, -3,  0, -3, -3,  2, -2, -4, -2, -1, -1, -4, -3, -3, -1, -3,  0, -1,-6},
                                                  {-2, -1,  6,  1, -3,  0, -1, -1,  0, -4, -4,  0, -3, -4, -3,  0, 0, -4, -3, -4, 5, -4, 0, -1, -6}, 
                                                  {-2, -2,  1,  6, -4, -1, 1, -2, -2, -4, -5, -1, -4, -4, -2, -1, -1, -6, -4, -4, 5, -5,  1, -1,-6},
                                                  {-1, -4, -3, -4, 9, -4, -5, -4,-4, -2, -2, -4, -2, -3, -4, -2, -1, -3, -3, -1, -4,-2, -4, -1, -6},
                                                  {-1,  1,  0, -1, -4,  6,  2, -2,  1, -3, -3,  1,  0, -4, -2,  0, -1, -3, -2, -3, 0, -3, 4, -1,-6},
                                                  {-1, -1, -1,  1, -5,  2,  6, -3,  0, -4, -4, 1, -2, -4, -2, 0, -1, -4, -3, -3, 1, -4,  5, -1, -6},
                                                  {0, -3, -1, -2, -4, -2,-3, 6, -3, -5, -4, -2, -4, -4, -3, -1, -2, -4, -4, -4, -1, -5, -3, -1, -6},
                                                  {-2,  0,  0, -2, -4,  1,  0, -3,  8, -4, -3, -1, -2, -2, -3, -1, -2, -3, 2, -4, -1, -4, 0, -1,-6},
                                                  {-2, -3, -4, -4, -2, -3, -4, -5, -4,  5,  1, -3, 1, -1, -4, -3, -1, -3, -2, 3, -4, 3, -4, -1, -6},
                                                  {-2, -3, -4, -5, -2, -3, -4, -4, -3,  1,  4, -3,  2,  0,-3, -3, -2, -2, -2, 1, -4, 3, -3, -1, -6},
                                                  {-1,  2,  0, -1, -4,  1, 1, -2, -1, -3, -3, 5, -2, -4, -1, -1, -1, -4, -3, -3, -1, -3, 1, -1, -6},
                                                  {-1, -2, -3, -4, -2,  0, -2, -4, -2,  1,  2, -2, 6, 0, -3, -2, -1, -2, -2, 1, -3,  2, -1, -1, -6},
                                                  {-3, -4, -4, -4, -3, -4, -4, -4, -2, -1,  0, -4,  0,  6, -4,-3,-2,  0,  3, -1, -4, 0, -4, -1, -6},
                                                  {-1, -2, -3, -2, -4, -2, -2, -3, -3, -4, -3, -1, -3, -4, 8, -1, -2, -5, -4, -3, -2, -4,-2, -1,-6},
                                                  { 1, -1,  0, -1, -2,  0,  0, -1, -1, -3, -3, -1, -2, -3, -1,  5, 1, -4, -2, -2,  0, -3, 0, -1,-6},
                                                  { 0, -1,  0, -1, -1, -1, -1, -2, -2, -1, -2, -1, -1, -2, -2, 1, 5, -4, -2, 0, -1, -1, -1, -1, -6},
                                                  {-3, -4, -4, -6, -3, -3, -4, -4, -3, -3,-2, -4, -2, 0, -5, -4, -4, 11, 2, -3, -5, -3, -3, -1, -6},
                                                  {-2, -3, -3, -4, -3, -2, -3, -4,  2, -2, -2, -3, -2, 3, -4, -2, -2,  2, 7, -2,-3, -2, -3, -1, -6},
                                                  { 0, -3, -4, -4, -1, -3, -3, -4, -4,  3,  1, -3, 1, -1, -3, -2, 0, -3, -2, 4, -4,  2, -3, -1, -6},
                                                  {-2, -1,  5,  5, -4, 0, 1, -1, -1, -4, -4, -1, -3, -4, -2, 0, -1, -5, -3, -4,  5, -4,  0, -1, -6},
                                                  {-2, -3, -4, -5, -2, -3, -4, -5, -4,  3, 3, -3, 2, 0, -4, -3, -1, -3, -2,  2, -4,  3, -3, -1, -6},
                                                  {-1,  0,  0,  1, -4,  4,  5, -3,  0, -4, -3,  1, -1, -4, -2, 0, -1, -3, -3, -3, 0, -3, 5, -1, -6},
                                                  {-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,-1,-1,-1, -6},
                                                  {-6, -6, -6, -6, -6, -6, -6, -6, -6, -6, -6, -6, -6, -6, -6, -6, -6, -6, -6, -6, -6, -6,-6,-6, 1},};
            x = BLOSUM80_matrix[x, y];
            return x;
        }
        static public void Reset()
        {
            Seq1_Align = "";
            Seq2_Align = "";
            All_Optimal1 = new int[100];                 //Local Alignment 

            hor = "";
            ver = "";
        }

        public static int[,] Fill_Matrix_local(string Sequence1, string Sequence2, int gab)
        {
            int m = Sequence1.Length;
            int n = Sequence2.Length;
            int[,] mat2 = new int[m + 1, n + 1];
            for (int i = 0; i <= m; i++)
            {
                mat2[i, 0] = 0;
            }
            for (int j = 0; j <= n; j++)
            {
                mat2[0, j] = 0;
            }
            for (int i = 1; i <= m; i++)
            {
                for (int j = 1; j <= n; j++)
                {
                    int MATCH = mat2[i - 1, j - 1] + score(Sequence1[i - 1], Sequence2[j - 1]);
                    int delete = mat2[i - 1, j] + gab;
                    int insert = mat2[i, j - 1] + gab;
                    int max = Math.Max(MATCH, insert);
                    int max2 = Math.Max(max, delete);
                    mat2[i, j] = Math.Max(max2, 0);
                }
            }
            return mat2;
        }
        public static string back_track(int[,] Mat, string sequence1, string sequence2, int i, int j, int gap)
        {
            //var Max = (from int Element in Mat select Element).Max();            // Trace back from maximum val in array 
            if (i == 0 || j == 0)
                return "";

            else if (Mat[i, j] == 0)
                return "";

            else if (Mat[i, j] == Mat[i - 1, j - 1] + score(sequence1[i - 1], sequence2[j - 1]))
            {
                Seq1_Align += sequence1[i - 1];
                Seq2_Align += sequence2[j - 1];
                return back_track(Mat, sequence1, sequence2, i - 1, j - 1, gap);
            }
            else
            {
                if (Mat[i, j] == Mat[i - 1, j] + gap)
                {
                    Seq1_Align += "-";
                    Seq2_Align += sequence2[j - 1];
                    return back_track(Mat, sequence1, sequence2, i - 1, j, gap);
                }
                else
                {
                    Seq1_Align += sequence1[i - 1];
                    Seq2_Align += "-";
                    return back_track(Mat, sequence1, sequence2, i, j - 1, gap);
                }

            }
        }
        public static void Get_index(int[,] mat, int max)           //Get indcies of max
        {
            int index1 = 0;
            int index2 = 1;
            int r = Protein_Sequence1.Length + 1;
            int c = Protein_Sequence2.Length + 1;
            for (int i = 0; i < r; i++)
            {
                for (int j = 0; j < r; j++)
                {
                    if (mat[i, j] == max)
                    {
                        if (index1 < i && index2 < j + 1)
                        {
                            All_Optimal1[index1] = i;
                            All_Optimal1[index2] = j;
                            index1 += 2;
                            index2 += 2;
                        }
                    }
                }
            }
        }                                                    //End Get index
        public static int[,] Fill_Matrix_glopal(string Sequence1, string Sequence2, int gab)
        {
            int m = Sequence1.Length;
            int n = Sequence2.Length;
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
                    int MATCH = mat[i - 1, j - 1] + score(Sequence1[i - 1], Sequence2[j - 1]);
                    int delete = mat[i - 1, j] + gab;
                    int insert = mat[i, j - 1] + gab;
                    int max = Math.Max(MATCH, insert);
                    mat[i, j] = Math.Max(max, delete);
                }
            }
            return mat;
        }
        public static double max3(double i1, double i2, double i3)
        {
            return Math.Max(i3, Math.Max(i2, i1));
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
                    M[i, j] = max3(M[i - 1, j - 1] + score(s1[i - 1], s2[j - 1]), Ix[i - 1, j - 1] + score(s1[i - 1], s2[j - 1]), Iy[i - 1, j - 1] + score(s1[i - 1], s2[j - 1]));
                    Ix[i, j] = Math.Max(M[i - 1, j] + h + g, Ix[i - 1, j] + g);
                    Iy[i, j] = Math.Max(M[i, j - 1] + h + g, Iy[i, j - 1] + g);
                }
            }
        }
        public static string traceBack_Affine_protien(double[,] g, string s1, string s2, int i, int j)
        {

            if (i == 0 || j == 0)
                return "";
            if (M[i, j] >= Ix[i, j] && M[i, j] >= Iy[i, j])
            {
                hor += s2[j - 1];
                ver += s1[i - 1];
                return traceBack_Affine_protien(M, s1, s2, i - 1, j - 1);
            }

            else if (Iy[i, j] >= Ix[i, j] && Iy[i, j] >= M[i, j])
            {
                hor += s2[j - 1];
                ver += "-";
                return traceBack_Affine_protien(Iy, s1, s2, i, j - 1);
            }
            else if (Ix[i, j] >= Iy[i, j] && Ix[i, j] >= M[i, j])
            {
                hor += "-";
                ver += s1[i - 1];
                return traceBack_Affine_protien(Ix, s1, s2, i - 1, j);
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
    }
}
