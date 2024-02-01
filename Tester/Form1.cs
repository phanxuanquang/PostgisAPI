using PostgisUltilities;
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

        private async void TestBtn(object sender, EventArgs e)
        {
            ApiHelper apiHelper = new ApiHelper();
            apiHelper.baseUrl = $"https://localhost:7186";
            apiHelper.Post($"{endpointBox.Text}", bodyBox.Text);
        }
    }
}
