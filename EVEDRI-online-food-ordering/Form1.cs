using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EVEDRI_online_food_ordering
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.CenterToScreen();
        }

        QuickBitesDataContext db = new QuickBitesDataContext();

        private void button1_Click(object sender, EventArgs e)
        {
            // login button
            string usernameInput = this.textBox1.Text;
            string passwordInput = this.textBox2.Text;
            var user = db.UserAccounts.Where(u => u.username == usernameInput).FirstOrDefault();
            //  ^^^ the first row that has matching username, is null if none
            if (user != null)
            {
                if (user.pass == passwordInput)
                {
                    this.Hide();
                    var productsPage = new Homepage(user.id, user.username);
                    productsPage.Show();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // previous button
            this.Hide();
            var mainHomePage = new MainHomePage();
            mainHomePage.Show();
        }
    }
}
