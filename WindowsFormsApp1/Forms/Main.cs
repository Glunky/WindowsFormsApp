using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using WindowsFormsApp1.Data.Models;

namespace WindowsFormsApp1
{
    public partial class Main : Form
    {
        private string workingDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
        private string countriesFilePath;
        private List<Country> countries;
        private BindingSource bs;

        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs args)
        {
            countriesFilePath = $@"{workingDirectory}\countries.dat";

            bs = new BindingSource();
            countries= File.Exists(countriesFilePath) ? GetCountriesFromFile() : GetDefaultCountries();
            bs.DataSource = countries;

            dataGridView.DataSource = bs;
            dataGridView.Columns[dataGridView.ColumnCount - 1].Visible = false;
            bindingNavigator.BindingSource = bs;

            chart.DataSource = bs;
            chart.Series[0].XValueMember = "Name";
            chart.Series[0].YValueMembers = "Population";
            bs.CurrentChanged += (o, e) => chart.DataBind();

            pictureBox.DataBindings.Add("ImageLocation", bs, "ImageFile", true);
            propertyGrid.DataBindings.Add("SelectedObject", bs, "");
            chart.Legends.Clear();
        }

        private void pictureBox_DoubleClick(object sender, EventArgs e)
        {
            OpenFileDialog opf = new OpenFileDialog();
            opf.InitialDirectory = workingDirectory;
            opf.Filter = "Картинка в формате(*.bmp;*.jpg)|*.bmp;*.jpg|Все файлы(*.*)|*.*";
            if (opf.ShowDialog() == DialogResult.OK)
            {
                (bs.Current as Country).ImageFile = opf.FileName;
                bs.ResetBindings(false);
            }
        }

        private void toolStripTextBox_TextChanged(object sender, EventArgs e)
        {
            bs.DataSource = countries.Where(
                x => Regex.IsMatch(x.Name, $"^{toolStripTextBox.Text}.*", RegexOptions.IgnoreCase)
            ).ToList();
        }

        private void toolStripSaveButton_Click(object sender, EventArgs e)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (Stream fs = new FileStream(countriesFilePath, FileMode.Create))
            {
                bf.Serialize(fs, (bs.DataSource));
            }
            MessageBox.Show("Сохранено");
        }

        private void toolStripLoadButton_Click(object sender, EventArgs e)
        {
            if (File.Exists(countriesFilePath))
            {
                countries = GetCountriesFromFile();
                bs.DataSource = countries;
            }
            else
            {
                MessageBox.Show("Ничего не сохранено");
            }
        }

        private List<Country> GetCountriesFromFile()
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (FileStream fs = new FileStream(countriesFilePath, FileMode.Open))
            {
                return (List<Country>)bf.Deserialize(fs);
            }
        }

        private List<Country> GetDefaultCountries()
        {
            return new List<Country>()
            {
                new Country()
                {
                    Name = "Германия",
                    Capital = "Берлин",
                    Population =  81f,
                    Area = 357021,
                    ImageFile = $@"{workingDirectory}\Resources\germany.png"
                },
                new Country()
                {
                    Name = "Россия",
                    Capital = "Москва",
                    Population = 146f,
                    Area = 17125187,
                    ImageFile = $@"{workingDirectory}\Resources\russia.png"
                },
                new Country()
                {
                    Name = "США",
                    Capital = "Вашингтон",
                    Population = 321f,
                    Area = 9519431,
                    ImageFile = $@"{workingDirectory}\Resources\usa.png"

                },
                new Country()
                {
                    Name = "Сербия",
                    Capital = "Белград",
                    Population = 7.2f,
                    Area = 88407,
                    ImageFile = $@"{workingDirectory}\Resources\serbia.png"
                },
                new Country()
                {
                    Name = "Франция",
                    Capital = "Париж",
                    Population = 66f,
                    Area = 674585,
                    ImageFile = $@"{workingDirectory}\Resources\france.png"
                },
                new Country()
                {
                    Name = "Бразилия",
                    Capital = "Бразилиа",
                    Population = 201f,
                    Area = 8514877,
                    ImageFile = $@"{workingDirectory}\Resources\brazil.png"
                },
            };
        }
    }
}
