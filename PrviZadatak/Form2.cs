using Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrviZadatak
{
    public partial class Form2 : Form
    {
        public Employee e;
        public bool textChanged = false;

        public Form2(Employee employee)
        {
            InitializeComponent();
            this.e = employee;
            TextBoxIzmenjenTekst();
        }

        public Form2()
        {
            InitializeComponent();
            TextBoxIzmenjenTekst();
        }

        private void btnIzmeni2_Click(object sender, EventArgs e)
        {
            if (this.e != null)
            {
                if (this.e.Empid != 0)
                {
                    this.e.Firstname = txtIme.Text;
                    this.e.Lastname = txtPrezime.Text;
                    this.e.Title = txtTitula.Text;
                    this.e.Titleofcourtesy = txtTitleofcourtesy.Text;
                    this.e.Birthdate = dateBirthdate.Value;
                    this.e.Hiredate = dateHiredate.Value;
                    this.e.Address = txtAddress.Text;
                    this.e.City = txtCity.Text;
                    this.e.Region = txtRegion.Text;
                    this.e.Postalcode = txtPostalcode.Text;
                    this.e.Country = txtCountry.Text;
                    this.e.Phone = txtPhone.Text;
                }
                else
                {
                    this.e.Firstname = txtIme.Text;
                    this.e.Lastname = txtPrezime.Text;
                    this.e.Title = txtTitula.Text;
                    this.e.Titleofcourtesy = txtTitleofcourtesy.Text;
                    this.e.Birthdate = dateBirthdate.Value;
                    this.e.Hiredate = dateHiredate.Value;
                    this.e.Address = txtAddress.Text;
                    this.e.City = txtCity.Text;
                    this.e.Region = txtRegion.Text;
                    this.e.Postalcode = txtPostalcode.Text;
                    this.e.Country = txtCountry.Text;
                    this.e.Phone = txtPhone.Text;
                    MessageBox.Show("Uspešno unet red.");
                }
            }
            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            if (this.e != null)
            {
                if (this.e.Empid != 0)
                {
                    txtIme.Text = this.e.Firstname;
                    txtPrezime.Text = this.e.Lastname;
                    txtTitula.Text = this.e.Title;
                    txtTitleofcourtesy.Text = this.e.Titleofcourtesy;
                    dateBirthdate.Value = this.e.Birthdate.Value;
                    dateHiredate.Value = this.e.Hiredate.Value;
                    txtAddress.Text = this.e.Address;
                    txtCity.Text = this.e.City;
                    txtRegion.Text = this.e.Region;
                    txtPostalcode.Text = this.e.Postalcode;
                    txtCountry.Text = this.e.Country;
                    txtPhone.Text = this.e.Phone;
                    btnIzmeni2.Text = "Izmeni";
                }
                else
                {
                    btnIzmeni2.Text = "Sačuvaj";
                }
            }
        }

        private void TextBoxIzmenjenTekst()
        {
            txtIme.TextChanged += TextBox_TextChanged;
            txtPrezime.TextChanged += TextBox_TextChanged;
            txtTitula.TextChanged += TextBox_TextChanged;
            txtTitleofcourtesy.TextChanged += TextBox_TextChanged;
            txtAddress.TextChanged += TextBox_TextChanged;
            txtCity.TextChanged += TextBox_TextChanged;
            txtRegion.TextChanged += TextBox_TextChanged;
            txtPostalcode.TextChanged += TextBox_TextChanged;
            txtCountry.TextChanged += TextBox_TextChanged;
            txtPhone.TextChanged += TextBox_TextChanged;
        }

        public void TextBox_TextChanged(object sender, EventArgs e)
        {
            textChanged = true;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);


            if (textChanged && FormFieldsChanged())
            {
                DialogResult dialogResult = MessageBox.Show("Da li želite da izađete bez čuvanja podataka?", "Izlazak", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }
        }

        private bool FormFieldsChanged()
        {
            if (e == null)
                return false;

            return txtIme.Text != e.Firstname ||
                   txtPrezime.Text != e.Lastname ||
                   txtTitula.Text != e.Title ||
                   txtTitleofcourtesy.Text != e.Titleofcourtesy ||
                   dateBirthdate.Value != e.Birthdate ||
                   dateHiredate.Value != e.Hiredate ||
                   txtAddress.Text != e.Address ||
                   txtCity.Text != e.City ||
                   txtRegion.Text != e.Region ||
                   txtPostalcode.Text != e.Postalcode ||
                   txtCountry.Text != e.Country ||
                   txtPhone.Text != e.Phone;
        }
    }
}