using Newtonsoft.Json;
using PostgisUltilities;
using System;
using System.Windows.Forms;

namespace ApiTester
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private async void ExecuteBtn_Click(object sender, EventArgs e)
        {
            ModelItemApiHelper helper = new ModelItemApiHelper("https://localhost:" + PortBox.Text);
            object item = null;

            // item = await helper.Get<List<ModelItemDB>>("modelitem", Guid.Parse("08f54e0f-c3a1-42ea-8943-f58cd74a2c75"));

            //item = await helper.GetById<ModelItemDB>("modelitem", Guid.Parse("08f54e0f-c3a1-42ea-8943-f58cd74a2c75"), Guid.Parse("08f54e0f-c3a1-42ea-8943-f58cd74a2c75"));

            //ModelItemDB newModelItem = null; 
            //item = await helper.Post<ModelItemDB>("modelitem", Guid.Parse("08f54e0f-c3a1-42ea-8943-f58cd74a2c75"), newModelItem);

            OutputBox.Text = JsonConvert.SerializeObject(item);
        }
    }
}
