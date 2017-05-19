using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Excel_Accession
{
    class Global
    {
        public static string seq_pre1 = "";
        public static string seq_pre2 = "";
        public static int num_of_optimals = 1;
        public static int Len;
        public static string frist_alig1 = "";
        public static string sec_alig2 = "";

        public static int get_match_mismatch(char a, char b)
        {
            if (a == b)
                return 2;
            else
                return -1;
        }
        public static void backtrack_GLOBAL_DNA(int[,] m, string s1, string s2, int i, int j, int gap, List<List<string>> Aligns)
        {
            int scoree = 0;
            int H, V, D;
            if (i == 0 && j == 0) //if two strings are finished
            {
                string aln = "";

                if (num_of_optimals == 1)                //the first alignment to know the length 
                {
                    Len = frist_alig1.Length;
                }
                if (frist_alig1.Length < Len && sec_alig2.Length < Len)   //if new alihnment <Length so get the whole string
                {
                    // int getl = L - ALIG_1.Length;
                    for (int y = frist_alig1.Length; y < Len; y++)
                    {
                        frist_alig1 += seq_pre1[y];
                        sec_alig2 += seq_pre2[y];
                    }
                }
                for (int ii = 0; ii <= frist_alig1.Length - 1; ii++)              //to know the similar||
                {
                    if (frist_alig1[ii] == sec_alig2[ii])
                    {
                        aln += "|";
                    }
                    else
                        aln += " ";
                }
                //  Console.WriteLine(frist_alig1);
                // Console.WriteLine(aln);
                // Console.WriteLine(sec_alig2);
                List<string> test = new List<string>();
                test.Add(frist_alig1);
                test.Add(sec_alig2);

                Aligns.Add(test);

                seq_pre1 = frist_alig1;
                seq_pre2 = sec_alig2;
                frist_alig1 = "";
                sec_alig2 = "";
                ////////////////////////
                num_of_optimals++;
                return;
            }

            else if (i == 0 && j > 0)  //if 1st string is finished but the 2nd isn't move until 2nd finish and not element =0
            {
                frist_alig1 = s2[j - 1] + frist_alig1;
                sec_alig2 = "-" + sec_alig2;
                backtrack_GLOBAL_DNA(m, s1, s2, i, j - 1, gap, Aligns);

            }
            else if (i > 0 && j == 0)     //if 2nd string is finished but the 1st isn't move until 1st finish and not element =0
            {
                sec_alig2 = s1[i - 1] + sec_alig2;
                frist_alig1 = "-" + frist_alig1;
                backtrack_GLOBAL_DNA(m, s1, s2, i - 1, j, gap, Aligns);
            }

            else
            {
                scoree = get_match_mismatch(s1[i - 1], s2[j - 1]);

                //scoree = get_index_Blosum(s1[i - 1], s2[j - 1]);
                // calculate the horizontal, vertical,diagonal elements
                V = m[i - 1, j] + gap;
                H = m[i, j - 1] + gap;
                D = m[i - 1, j - 1] + scoree;

                if (H == Math.Max(H, Math.Max(V, D)))
                {
                    // save horizontal char only and gap on vertical
                    frist_alig1 = s2[j - 1] + frist_alig1;
                    sec_alig2 = "-" + sec_alig2;

                    //move to horizontal element
                    backtrack_GLOBAL_DNA(m, s1, s2, i, j - 1, gap, Aligns);
                }
                if (V == Math.Max(H, Math.Max(V, D)))
                {
                    // save vertical char only and gap on horizontal
                    frist_alig1 = "-" + frist_alig1;
                    sec_alig2 = s1[i - 1] + sec_alig2;

                    //move to vertical element
                    backtrack_GLOBAL_DNA(m, s1, s2, i - 1, j, gap, Aligns);
                }
                if (D == Math.Max(H, Math.Max(V, D)))
                {
                    // save two chars
                    frist_alig1 = s2[j - 1] + frist_alig1;
                    sec_alig2 = s1[i - 1] + sec_alig2;

                    //move to diagonal element
                    backtrack_GLOBAL_DNA(m, s1, s2, i - 1, j - 1, gap, Aligns);
                }
            }
            // return arr;
        }
        public static void backtrack_GLOBAL_protein(int[,] m, string s1, string s2, int i, int j, int gap, List<List<string>> Aligns)
        {
            int scoree = 0;
            int H, V, D;

            if (i == 0 && j == 0) //if two strings are finished
            {
                string aln = "";
                if (num_of_optimals == 1)                //the first alignment to know the length 
                {
                    Len = frist_alig1.Length;
                }
                if (frist_alig1.Length < Len && sec_alig2.Length < Len)   //if new alihnment <Length so get the whole string
                {
                    // int getl = L - ALIG_1.Length;
                    for (int y = frist_alig1.Length; y < Len; y++)
                    {
                        frist_alig1 += seq_pre1[y];
                        sec_alig2 += seq_pre2[y];
                    }
                }
                for (int ii = 0; ii <= frist_alig1.Length - 1; ii++)              //to know the similar||
                {
                    if (frist_alig1[ii] == sec_alig2[ii])
                    {
                        aln += "|";
                    }
                    else
                        aln += " ";
                }
                //// هنا المفروض تعرض انت بقي يا سعودي
                //Console.WriteLine(frist_alig1);
                //Console.WriteLine(aln);
                //Console.WriteLine(sec_alig2);

                List<string> test = new List<string>();
                test.Add(frist_alig1);
                test.Add(sec_alig2);

                Aligns.Add(test);


                seq_pre1 = frist_alig1;
                seq_pre2 = sec_alig2;
                frist_alig1 = "";
                sec_alig2 = "";
                ////////////////////////
                num_of_optimals++;
                return;
            }
            else if (i == 0 && j > 0)  //if 1st string is finished but the 2nd isn't move until 2nd finish and not element =0
            {
                frist_alig1 = s2[j - 1] + frist_alig1;
                sec_alig2 = "-" + sec_alig2;
                backtrack_GLOBAL_protein(m, s1, s2, i, j - 1, gap, Aligns);
            }
            else if (i > 0 && j == 0)     //if 2nd string is finished but the 1st isn't move until 1st finish and not element =0
            {
                sec_alig2 = s1[i - 1] + sec_alig2;
                frist_alig1 = "-" + frist_alig1;
                backtrack_GLOBAL_protein(m, s1, s2, i - 1, j, gap, Aligns);
            }
            else
            {
                //get match w mismatch 
                scoree = get_match_mismatch(s1[i - 1], s2[j - 1]);
                // calculate the horizontal, vertical,diagonal elements
                V = m[i - 1, j] + gap;
                H = m[i, j - 1] + gap;
                D = m[i - 1, j - 1] + scoree;
                if (H == Math.Max(H, Math.Max(V, D)))
                {
                    // save horizontal char only and gap on vertical
                    frist_alig1 = s2[j - 1] + frist_alig1;
                    sec_alig2 = "-" + sec_alig2;
                    //move to horizontal element
                    backtrack_GLOBAL_protein(m, s1, s2, i, j - 1, gap, Aligns);
                }
                if (V == Math.Max(H, Math.Max(V, D)))
                {
                    // save vertical char only and gap on horizontal
                    frist_alig1 = "-" + frist_alig1;
                    sec_alig2 = s1[i - 1] + sec_alig2;
                    //move to vertical element
                    backtrack_GLOBAL_protein(m, s1, s2, i - 1, j, gap, Aligns);
                }
                if (D == Math.Max(H, Math.Max(V, D)))
                {
                    // save two chars
                    frist_alig1 = s2[j - 1] + frist_alig1;
                    sec_alig2 = s1[i - 1] + sec_alig2;
                    //move to diagonal element
                    backtrack_GLOBAL_protein(m, s1, s2, i - 1, j - 1, gap, Aligns);
                }
            }
            // return arr;
        }
    }
}
