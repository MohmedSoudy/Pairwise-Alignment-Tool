using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace Excel_Accession
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        static int[,] Matrix1;
        public static string File_Path = "Score.txt";
       
        public static DateTime Date { get; set; }

        public static void Save_AlignmentData(string S1, string S2, string S3, int Score)
        {
            using (StreamWriter St = File.AppendText(File_Path))
            {
                St.WriteLine(S1);
                St.WriteLine(S2);
                St.WriteLine(S3);
                St.WriteLine("Alignment Score: " + Score);
                Form2.Date = DateTime.Now;
                St.WriteLine("Date of Alignmemnt is: " + Form2.Date);
                St.WriteLine(Environment.NewLine);
            }
        }
        public void DNALocal(List<List<string>> SpeciesData, string ExtintGap)
        {
            DNA_ALignment.DNA_Sequence1 = SpeciesData[0][2];
            DNA_ALignment.DNA_Sequence2 = SpeciesData[1][2];
            Matrix1 = DNA_ALignment.Fill_Matrix(SpeciesData[0][2], SpeciesData[1][2], Convert.ToInt32(Convert.ToDouble(ExtintGap)));
            var Max = (from int Element in Matrix1 select Element).Max();
            DNA_ALignment.Get_index(Matrix1, Max);
            for (int i = 0; i < DNA_ALignment.All_Optimal.Length; i++)
            {
                if (DNA_ALignment.All_Optimal[i] == 0)
                {
                    break;
                }

                DNA_ALignment.back_track(Matrix1, DNA_ALignment.DNA_Sequence1, DNA_ALignment.DNA_Sequence2, DNA_ALignment.All_Optimal[i], DNA_ALignment.All_Optimal[i]);
                i += 1;
                DNA_ALignment.Optimal1 = DNA_ALignment.Reverse_String(DNA_ALignment.Seq1_Align);
                textBox1.AppendText(DNA_ALignment.Optimal1 + Environment.NewLine);
                DNA_ALignment.Seq1_Align = "";
                DNA_ALignment.Optimal2 = DNA_ALignment.Reverse_String(DNA_ALignment.Seq2_Align);
                DNA_ALignment.Matches(DNA_ALignment.Optimal1, DNA_ALignment.Optimal2);
                textBox1.AppendText(DNA_ALignment.Matches_1 + Environment.NewLine);
                textBox1.AppendText(DNA_ALignment.Optimal2 + Environment.NewLine + Environment.NewLine + Environment.NewLine);
                DNA_ALignment.Seq2_Align = "";
                Form2.Save_AlignmentData(DNA_ALignment.Optimal1, DNA_ALignment.Matches_1, DNA_ALignment.Optimal2, Max);
            }
            label3.Text = Max.ToString();

            label4.Text = "DNA - Local alignment";
        }


        public void ProteinLocal(List<List<string>> SpeciesData, string ExtintGap)
        {
            Protein_Alignment.Protein_Sequence1 = SpeciesData[0][2];
            Protein_Alignment.Protein_Sequence2 = SpeciesData[1][2];
            Matrix1 = Protein_Alignment.Fill_Matrix_local(SpeciesData[0][2], SpeciesData[1][2], Convert.ToInt32(Convert.ToDouble(ExtintGap)));
            var Max = (from int Element in Matrix1 select Element).Max();
            Protein_Alignment.Get_index(Matrix1, Max);
            for (int i = 0; i < Protein_Alignment.All_Optimal1.Length; i++)
            {
                if (Protein_Alignment.All_Optimal1[i] == 0)
                {
                    break;
                }

                Protein_Alignment.back_track(Matrix1, Protein_Alignment.Protein_Sequence1, Protein_Alignment.Protein_Sequence2, Protein_Alignment.All_Optimal1[i], Protein_Alignment.All_Optimal1[i], Convert.ToInt32(Convert.ToDouble(ExtintGap)));
                i += 1;
                Protein_Alignment.Optimal1 = Protein_Alignment.Reverse_String(Protein_Alignment.Seq1_Align);
                textBox1.AppendText(Protein_Alignment.Optimal1 + Environment.NewLine);
                Protein_Alignment.Seq1_Align = "";
                Protein_Alignment.Optimal2 = Protein_Alignment.Reverse_String(Protein_Alignment.Seq2_Align);
                DNA_ALignment.Matches(Protein_Alignment.Optimal1, Protein_Alignment.Optimal2);
                textBox1.AppendText(DNA_ALignment.Matches_1 + Environment.NewLine);
                textBox1.AppendText(Protein_Alignment.Optimal2 + Environment.NewLine + Environment.NewLine + Environment.NewLine);
                Protein_Alignment.Seq2_Align = "";
                Form2.Save_AlignmentData(Protein_Alignment.Optimal1, DNA_ALignment.Matches_1, Protein_Alignment.Optimal2, Max);

            }

            label3.Text = Max.ToString();

            label4.Text = "Protein - Local alignment";
        }

        public void DNAGlobalLinear(List<List<string>> SpeciesData, string ExtintGap)
        {
            List<List<string>> Aligns = new List<List<string>>();
            int[,] mat1 = new int[SpeciesData[0][2].Length, SpeciesData[1][2].Length];
            mat1 = DNA_ALignment.LCS(SpeciesData[0][2], SpeciesData[1][2], Convert.ToInt32(Convert.ToDouble(ExtintGap)));
            Global.backtrack_GLOBAL_DNA(mat1, SpeciesData[0][2], SpeciesData[1][2], SpeciesData[0][2].Length, SpeciesData[1][2].Length, Convert.ToInt32(Convert.ToDouble(ExtintGap)), Aligns);

            for (int i = 0; i < Aligns.Count; i++)
            {
                Draw(Aligns[i][0], Aligns[i][1]);
                Form2.Save_AlignmentData(Aligns[i][0], DNA_ALignment.Matches_1, Aligns[i][1], mat1[SpeciesData[0][2].Length - 1, SpeciesData[1][2].Length - 1]);
            }
            label3.Text = mat1[SpeciesData[0][2].Length - 1, SpeciesData[1][2].Length - 1].ToString();

            label4.Text = "DNA - Global Linear";
        }
        public void Draw(string S1, string S2)
        {
            textBox1.AppendText(S1 + Environment.NewLine);
            DNA_ALignment.Matches(S1, S2);
            textBox1.AppendText(DNA_ALignment.Matches_1 + Environment.NewLine);
            textBox1.AppendText(S2 + Environment.NewLine + Environment.NewLine + Environment.NewLine);
        }

        public void ProteinGlobalLinear(List<List<string>> SpeciesData, string ExtintGap)
        {
            List<List<string>> Aligns = new List<List<string>>();
            int[,] mat1 = new int[SpeciesData[0][2].Length, SpeciesData[1][2].Length];
            mat1 = Protein_Alignment.Fill_Matrix_glopal(SpeciesData[0][2], SpeciesData[1][2], Convert.ToInt32(Convert.ToDouble(ExtintGap)));
            Global.backtrack_GLOBAL_protein(mat1, SpeciesData[0][2], SpeciesData[1][2], SpeciesData[0][2].Length, SpeciesData[1][2].Length, Convert.ToInt32(Convert.ToDouble(ExtintGap)), Aligns);

            for (int i = 0; i < Aligns.Count; i++)
            {
                Draw(Aligns[i][0], Aligns[i][1]);
                Form2.Save_AlignmentData(Aligns[i][0], DNA_ALignment.Matches_1, Aligns[i][1], mat1[SpeciesData[0][2].Length - 1, SpeciesData[1][2].Length - 1]);

            }
            label3.Text = mat1[SpeciesData[0][2].Length - 1, SpeciesData[1][2].Length - 1].ToString();

            label4.Text = "Protein - Global Linear";
        }

        public void DNAAffine(List<List<string>> SpeciesData, string OpenGap, string ExtintGap)
        {
            DNA_ALignment.InitializeAffine(SpeciesData[0][2], SpeciesData[1][2], Convert.ToInt32(Convert.ToDouble(OpenGap)), Convert.ToInt32(Convert.ToDouble(ExtintGap)));
            DNA_ALignment.Fill_Affine(SpeciesData[0][2], SpeciesData[1][2], Convert.ToInt32(Convert.ToDouble(OpenGap)), Convert.ToInt32(Convert.ToDouble(ExtintGap)));
            DNA_ALignment.traceBack_Affine_DNA(Protein_Alignment.M, SpeciesData[0][2], SpeciesData[1][2], SpeciesData[0][2].Length, SpeciesData[1][2].Length);
            Draw(DNA_ALignment.hor, DNA_ALignment.ver);
            double Max = DNA_ALignment.max3(DNA_ALignment.M[SpeciesData[0][2].Length - 1, SpeciesData[1][2].Length - 1], DNA_ALignment.Ix[SpeciesData[0][2].Length - 1, SpeciesData[1][2].Length - 1], DNA_ALignment.Iy[SpeciesData[0][2].Length - 1, SpeciesData[1][2].Length - 1]);
            int max = Convert.ToInt32(Max);
            Form2.Save_AlignmentData(DNA_ALignment.hor, DNA_ALignment.Matches_1, DNA_ALignment.ver, max);
            label3.Text = max.ToString();
        }

        public void ProteinAffine(List<List<string>> SpeciesData, string OpenGap, string ExtintGap)
        {
            Protein_Alignment.InitializeAffine(SpeciesData[0][2], SpeciesData[1][2], Convert.ToInt32(Convert.ToDouble(OpenGap)), Convert.ToInt32(Convert.ToDouble(ExtintGap)));
            Protein_Alignment.Fill_Affine(SpeciesData[0][2], SpeciesData[1][2], Convert.ToInt32(Convert.ToDouble(OpenGap)), Convert.ToInt32(Convert.ToDouble(ExtintGap)));
            Protein_Alignment.traceBack_Affine_protien(Protein_Alignment.M, SpeciesData[0][2], SpeciesData[1][2], SpeciesData[0][2].Length, SpeciesData[1][2].Length);
            Protein_Alignment.hor = Protein_Alignment.Reverse_String(DNA_ALignment.hor);
            Protein_Alignment.ver = Protein_Alignment.Reverse_String(DNA_ALignment.ver);
            Draw(Protein_Alignment.hor, Protein_Alignment.ver);
            double Max = Protein_Alignment.max3(Protein_Alignment.M[SpeciesData[0][2].Length - 1, SpeciesData[1][2].Length - 1], Protein_Alignment.Ix[SpeciesData[0][2].Length - 1, SpeciesData[1][2].Length - 1], Protein_Alignment.Iy[SpeciesData[0][2].Length - 1, SpeciesData[1][2].Length - 1]);
           /* if (Max == Protein_Alignment.M[SpeciesData[0][2].Length - 1, SpeciesData[1][2].Length - 1] )
            {
                Protein_Alignment.traceBack_Affine_protien(Protein_Alignment.M, SpeciesData[0][2], SpeciesData[1][2], SpeciesData[0][2].Length, SpeciesData[1][2].Length);
                Protein_Alignment.hor = Protein_Alignment.Reverse_String(DNA_ALignment.hor);
                Protein_Alignment.ver = Protein_Alignment.Reverse_String(DNA_ALignment.ver);
                Draw(Protein_Alignment.hor, Protein_Alignment.ver);
            }
            else if (Max == Protein_Alignment.Ix[SpeciesData[0][2].Length - 1, SpeciesData[1][2].Length - 1])
            {
                Protein_Alignment.traceBack_Affine_protien(Protein_Alignment.Ix, SpeciesData[0][2], SpeciesData[1][2], SpeciesData[0][2].Length, SpeciesData[1][2].Length);
                Protein_Alignment.hor = Protein_Alignment.Reverse_String(DNA_ALignment.hor);
                Protein_Alignment.ver = Protein_Alignment.Reverse_String(DNA_ALignment.ver);
                Draw(Protein_Alignment.hor, Protein_Alignment.ver);
            }
            else 
            {
                Protein_Alignment.traceBack_Affine_protien(Protein_Alignment.Iy, SpeciesData[0][2], SpeciesData[1][2], SpeciesData[0][2].Length, SpeciesData[1][2].Length);
                Protein_Alignment.hor = Protein_Alignment.Reverse_String(DNA_ALignment.hor);
                Protein_Alignment.ver = Protein_Alignment.Reverse_String(DNA_ALignment.ver);
                Draw(Protein_Alignment.hor, Protein_Alignment.ver);
            }*/
            int max = Convert.ToInt32(Max);
            Form2.Save_AlignmentData(Protein_Alignment.hor, DNA_ALignment.Matches_1, Protein_Alignment.ver, max);
            label3.Text = max.ToString();
        }
    }
}
