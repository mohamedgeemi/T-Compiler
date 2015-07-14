using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Compiler_Theory
{
    public partial class CompilerTheory : Form
    {
        public CompilerTheory()
        {
            InitializeComponent();
        }

        private void Phase1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Scanner NewScanner = new Scanner();
            NewScanner.ShowDialog();
            //this.Close();
            this.Show();
        }

        private void Phase2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Parser NewParser = new Parser();
            NewParser.ShowDialog();
            //this.Close();
            this.Show();
        }

        private void Phase3_Click(object sender, EventArgs e)
        {
            this.Hide();
            Semantic NewSemantic = new Semantic();
            NewSemantic.ShowDialog();
            //this.Close();
            this.Show();
        }

        private void CompilerTheory_Load(object sender, EventArgs e)
        {

        }

        
    }
}
