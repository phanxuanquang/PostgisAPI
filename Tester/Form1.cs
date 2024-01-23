using iTextSharp.text.pdf;
using Newtonsoft.Json;
using System;
using System.Windows.Forms;

namespace Tester
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PdfReader reader = new PdfReader(@"C:\Users\phanxuanquang\Downloads\SOC통합관리센터_DSM(4차 배포).pdf");
            string ok = "";
            for (int pageNum = 1; pageNum <= reader.NumberOfPages; pageNum++)
            {
                PdfDictionary pageDict = reader.GetPageN(pageNum);
                PdfArray annots = pageDict.GetAsArray(PdfName.ANNOTS);

                if (annots != null)
                {
                    ok += JsonConvert.SerializeObject(annots) + "\n";
                    //foreach (var obj in annots.ArrayList)
                    //{
                    //    ok += JsonConvert.SerializeObject(obj) + "\n";
                    //}

                }
            }
            richTextBox1.Text = ok;

        }

        private void Extract3DData(PdfDictionary annotDict)
        {
            richTextBox1.Text = JsonConvert.SerializeObject(annotDict);
        }

        private bool Is3DAnnotation(PdfDictionary annotDict)
        {
            PdfName subtype = annotDict.GetAsName(PdfName.SUBTYPE);
            return subtype == PdfName._3D || annotDict.Contains(PdfName._3D);
        }
    }
}
