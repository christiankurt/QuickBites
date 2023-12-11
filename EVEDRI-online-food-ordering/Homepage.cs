using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace EVEDRI_online_food_ordering
{
    public partial class Homepage : Form
    {
        int currentUserID = 0;
        string currentUserName = "";
        QuickBitesDataContext db = new QuickBitesDataContext();
        public Homepage(int user_id, string user_name)
        {
            InitializeComponent();
            this.CenterToScreen();
            currentUserID = user_id;
            currentUserName = user_name;
            this.label19.Text = user_name;
            List<string> courseMealList = new List<string>();
            List<string> drinksList = new List<string>();
            List<string> specialtiesList = new List<string>();
            var courseMeal = from item in db.Products.ToList()
                             where item.category == "Course Meal"
                             select item.item;
            var drinks = from item in db.Products.ToList()
                         where item.category == "Drinks"
                         select item.item;
            var specialties = from item in db.Products.ToList()
                              where item.category == "Specialties"
                              select item.item;
            courseMealList.AddRange(courseMeal);
            drinksList.AddRange(drinks);
            specialtiesList.AddRange(specialties);
            // Prices
            var courseMealPrice = from item in db.Products.ToList()
                                  where item.category == "Course Meal"
                                  select item.price;
            var drinksPrice = from item in db.Products.ToList()
                              where item.category == "Drinks"
                              select item.price;
            var specialtiesPrice = from item in db.Products.ToList()
                                   where item.category == "Specialties"
                                   select item.price;
            try
            {
                List<double?> courseMealListPrice = new List<double?>();
                List<double?> drinksListPrice = new List<double?>();
                List<double?> specialtiesListPrice = new List<double?>();
                courseMealListPrice.AddRange((IEnumerable<double?>)courseMealPrice);
                drinksListPrice.AddRange((IEnumerable<double?>)drinksPrice);
                specialtiesListPrice.AddRange((IEnumerable<double?>)specialtiesPrice);
            } catch
            {
                List<double> courseMealListPrice = new List<double>();
                List<double> drinksListPrice = new List<double>();
                List<double> specialtiesListPrice = new List<double>();
                courseMealListPrice.AddRange((IEnumerable<double>)courseMealPrice);
                drinksListPrice.AddRange((IEnumerable<double>)drinksPrice);
                specialtiesListPrice.AddRange((IEnumerable<double>)specialtiesPrice);
            }
            // dynamic products page/food menu
            int groupBoxLabelIndex = 0;
            foreach (Control c in groupBox1.Controls)
            {
                if (c.GetType() == typeof(System.Windows.Forms.Panel))
                {
                    foreach (Control d in c.Controls)
                    {
                        if (d.Text == "Food Item")
                        {
                            d.Text = courseMealList.ElementAt(groupBoxLabelIndex);
                            groupBoxLabelIndex++;
                        } else
                        {
                            d.Text = $"₱{courseMealPrice.ElementAt(groupBoxLabelIndex).ToString()}.00";
                        }
                    }
                }
            }
            groupBoxLabelIndex = 0;
            foreach (Control c in groupBox2.Controls)
            {
                if (c.GetType() == typeof(System.Windows.Forms.Panel))
                {
                    foreach (Control d in c.Controls)
                    {
                        if (d.Text == "Food Item")
                        {
                            d.Text = drinksList.ElementAt(groupBoxLabelIndex);
                            groupBoxLabelIndex++;
                        }
                        else
                        {
                            d.Text = $"₱{drinksPrice.ElementAt(groupBoxLabelIndex).ToString()}.00";
                        }
                    }
                }
            }
            groupBoxLabelIndex = 0;
            foreach (Control c in groupBox3.Controls)
            {
                if (c.GetType() == typeof(System.Windows.Forms.Panel))
                {
                    foreach (Control d in c.Controls)
                    {
                        if (d.Text == "Food Item")
                        {
                            d.Text = specialtiesList.ElementAt(groupBoxLabelIndex);
                            groupBoxLabelIndex++;
                        }
                        else
                        {
                            d.Text = $"₱{specialtiesPrice.ElementAt(groupBoxLabelIndex).ToString()}.00";
                        }
                    }
                }
            }
            var courseMealImages = (from item in db.Products
                                   where item.category == "Course Meal"
                                   select item.img.ToArray()).ToArray();
            var drinksImages = (from item in db.Products
                                   where item.category == "Drinks"
                                   select item.img.ToArray()).ToArray();
            var specialtiesImages = (from item in db.Products
                                   where item.category == "Specialties"
                                   select item.img.ToArray()).ToArray();
            int imageIndex = 0;
            foreach (PictureBox c in groupBox1.Controls.Cast<Control>().Where(c => c.GetType() == typeof(PictureBox))) // referenced PsychoCoder from stackoverflow
            {
                c.Image = ByteArrayToImage(courseMealImages.ElementAt(imageIndex));
                imageIndex++;
            }
            imageIndex = 0;
            foreach (PictureBox c in groupBox2.Controls.Cast<Control>().Where(c => c.GetType() == typeof(PictureBox)))
            {
                c.Image = ByteArrayToImage(drinksImages.ElementAt(imageIndex));
                imageIndex++;
            }
            imageIndex = 0;
            foreach (PictureBox c in groupBox3.Controls.Cast<Control>().Where(c => c.GetType() == typeof(PictureBox)))
            {
                c.Image = ByteArrayToImage(specialtiesImages.ElementAt(imageIndex));
                imageIndex++;
            }
        }

        public static Image ByteArrayToImage(byte[] byteArray) // Arigatou chatGPT sensei!!! >.< (prolly web scraped from stackoverflow)
        {
            using (MemoryStream ms = new MemoryStream(byteArray))
            {
                Image image = Image.FromStream(ms);
                return image;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // start ordering button
            this.Hide();
            var orderForm = new OrderingForm(currentUserID, currentUserName);
            orderForm.Show();
        }

        private void label19_Click(object sender, EventArgs e)
        {
            this.Hide();
            var mainHomePage = new MainHomePage();
            mainHomePage.Show();
        }
    }
}