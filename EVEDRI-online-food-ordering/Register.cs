using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Tracing;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace EVEDRI_online_food_ordering
{
    public partial class Register : Form
    {
        public Register()
        {
            InitializeComponent();
            this.CenterToScreen();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // previous button
            this.Hide();
            var mainHomePage = new MainHomePage();
            mainHomePage.Show();
        }

        bool readyToConfirm = false;
        QuickBitesDataContext db = new QuickBitesDataContext();

        private void button1_Click(object sender, EventArgs e)
        {
            // confirm register button
            int incorrectDetails = 0;
            long contactNumber;
            // handles textbox functionality and errors
            object[] registerTextBoxArray = {
                this.textBox1, // [0] customer name
                this.textBox2, // [1] address
                this.textBox3, // [2] user email
                this.textBox4, // [3] user name
                this.textBox5, // [4] user password
                this.textBox6  // [5] user contact number
            };
            foreach (System.Windows.Forms.TextBox textBox in registerTextBoxArray)
            {
                textBox.ForeColor = System.Drawing.Color.Green;
                if (textBox.Text == "" || textBox.Text == "Field required!")
                {
                    textBox.ForeColor = System.Drawing.Color.Red;
                    textBox.Text = "Field required!";
                    readyToConfirm = false;
                    incorrectDetails += 1;
                    this.button1.Text = "Confirm";
                    this.button1.Width = 136;
                    continue;
                }
                else if (textBox.Text.Contains(" "))
                {
                    if (!(textBox == this.textBox1 || textBox == this.textBox2 ||
                        textBox.Text == "Enter an email address" || textBox.Text == "Must be numbers only"))
                    {
                        textBox.ForeColor = System.Drawing.Color.Red;
                        textBox.Text = "Must not have spaces";
                        readyToConfirm = false;
                        incorrectDetails += 1;
                        this.button1.Text = "Confirm";
                        this.button1.Width = 136;
                        continue;
                    }
                }
                if (textBox == this.textBox6)
                {
                    if (!Int64.TryParse(textBox.Text, out contactNumber))
                    {
                        textBox.ForeColor = System.Drawing.Color.Red;
                        textBox.Text = "Must be numbers only";
                        readyToConfirm = false;
                        incorrectDetails += 1;
                        this.button1.Text = "Confirm";
                        this.button1.Width = 136;
                    }
                }
                if (textBox == this.textBox3)
                {
                    if (!this.textBox3.Text.Contains('@') || !this.textBox3.Text.Contains(".com"))
                    {
                        textBox3.ForeColor = System.Drawing.Color.Red;
                        textBox3.Text = "Enter an email address";
                        readyToConfirm = false;
                        incorrectDetails += 1;
                        this.button1.Text = "Confirm";
                        this.button1.Width = 136;
                    }
                }
            }
            // handles gender radio buttons 
            object[] registerRadioButtonsArray = {
                this.radioButton1,
                this.radioButton2
            };
            object activeRadioButton;
            string activeRadioButtonText = "???";
            int uncheckedRadioButton = 0;
            foreach (System.Windows.Forms.RadioButton radioButton in registerRadioButtonsArray)
            {
                if (radioButton.Checked)
                {
                    activeRadioButton = radioButton;
                    activeRadioButtonText = radioButton.Text;
                }
                else // this else statement is for fail safe when somehow no radio button is checked
                {
                    radioButton.ForeColor = System.Drawing.Color.Red;
                    uncheckedRadioButton += 1;
                    this.button1.Text = "Confirm";
                    this.button1.Width = 136;
                }
            }
            if (uncheckedRadioButton == 1)
            {
                radioButton1.ForeColor = System.Drawing.Color.Black;
                radioButton2.ForeColor = System.Drawing.Color.Black;
            } else
            {
                incorrectDetails += 1;
                readyToConfirm = false;
            }
            //handles date time
            System.Windows.Forms.DateTimePicker dateOfBirth = this.dateTimePicker1;
            string userBirthDate = dateOfBirth.Text;
            string[] currentDate = DateTime.Now.ToString().Split();
            string[] currMonthDayYear = currentDate[0].Split('/');
            string[] userMonthDayYear = userBirthDate.Split('/');
            int yearDifference = Convert.ToInt32(currMonthDayYear[2]) - Convert.ToInt32(userMonthDayYear[2]);
            bool isOfAge = false;
            if (yearDifference > 18)
            {
                this.label9.ForeColor = System.Drawing.Color.Green;
                isOfAge = true;
            }
            else if (yearDifference == 18)
            {
                if (Convert.ToInt32(currMonthDayYear[0]) >= Convert.ToInt32(userMonthDayYear[0]))
                {
                    if (Convert.ToInt32(currMonthDayYear[1]) >= Convert.ToInt32(userMonthDayYear[1]))
                    {
                        this.label9.ForeColor = System.Drawing.Color.Green;
                        isOfAge = true;
                    }
                }
            }
            if (!isOfAge)
            {
                this.label9.ForeColor = System.Drawing.Color.Red;
                incorrectDetails += 1;
                readyToConfirm = false;
            }
            // confirmation
            if (readyToConfirm)
            {
                // sending to database
                UserAccount ua = new UserAccount
                {
                    username = this.textBox4.Text,
                    pass = this.textBox5.Text
                };
                UserDetail ud = new UserDetail
                {
                    customername = this.textBox1.Text,
                    gender = activeRadioButtonText,
                    dateofbirth = DateTime.Parse(userMonthDayYear[2] + "-" + userMonthDayYear[0] + "-" + userMonthDayYear[1]),
                    homeaddress = this.textBox2.Text,
                    contactnumber = this.textBox6.Text,
                    email = this.textBox3.Text
                };
                db.UserAccounts.InsertOnSubmit(ua);
                db.UserDetails.InsertOnSubmit(ud);
                db.SubmitChanges();
                this.Hide();
                var mainHomePage = new MainHomePage();
                mainHomePage.Show();
            }
            if (incorrectDetails == 0)
            {
                this.button1.Text = "Click to Confirm";
                this.button1.Width = 175;
                readyToConfirm = true;
            }
        }
    }
}
