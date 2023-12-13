using System;
using System.Windows.Forms;

namespace NumberToWord
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void btnConvert_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtNumber.Text != "")
                {
                    txtArabicWord.Text = ToWord.ConvertToArabic(Convert.ToDecimal(txtNumber.Text));
                }
                else return;
            }
            catch (Exception ex)
            {
                txtArabicWord.Text = ex.Message;
            }
        }
    }
}
