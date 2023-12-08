using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EVEDRI_online_food_ordering
{
    public partial class MainHomePage : Form
    {
        public MainHomePage()
        {
            InitializeComponent();
            this.CenterToScreen();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // login 
            this.Hide();
            Form1 loginForm = new Form1();
            loginForm.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // register button
            this.Hide();
            Register registerForm = new Register();
            registerForm.Show();
        }

        private void btnMin_Click(object sender, EventArgs e)
        {
            // minimize button
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnX_Click(object sender, EventArgs e)
        {
            // exit button
            System.Windows.Forms.Application.Exit();
        }
    }
}
