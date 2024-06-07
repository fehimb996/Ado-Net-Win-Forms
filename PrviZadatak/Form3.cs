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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace PrviZadatak
{
    public partial class Form3 : Form
    {
        public Supplier s;
        public bool textChanged = false;

        public Form3()
        {
            InitializeComponent();
            TextBoxIzmenjenTekst();
        }

        public Form3(Supplier supplier)
        {
            InitializeComponent();
            this.s = supplier;
            TextBoxIzmenjenTekst();
        }

        private void btnIzmeni2_Click(object sender, EventArgs e)
        {
            if (this.s != null)
            {
                if (this.s.SupplierID != 0)
                {
                    this.s.Companyname = txtCompanyname.Text;
                    this.s.Contactname = txtContactname.Text;
                    this.s.Contacttitle = txtTitle.Text;
                    this.s.Address = txtAddress.Text;
                    this.s.City = txtCity.Text;
                    this.s.Region = txtRegion.Text;
                    this.s.Postalcode = txtPostalCode.Text;
                    this.s.Country = txtCountry.Text;
                    this.s.Phone = txtPhone.Text;
                    this.s.Fax = txtFax.Text;
                }
                else
                {
                    this.s.Companyname = txtCompanyname.Text;
                    this.s.Contactname = txtContactname.Text;
                    this.s.Contacttitle = txtTitle.Text;
                    this.s.Address = txtAddress.Text;
                    this.s.City = txtCity.Text;
                    this.s.Region = txtRegion.Text;
                    this.s.Postalcode = txtPostalCode.Text;
                    this.s.Country = txtCountry.Text;
                    this.s.Phone = txtPhone.Text;
                    this.s.Fax = txtFax.Text;
                    MessageBox.Show("Uspešno unet red.");
                }
            }
            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            if (this.s != null)
            {
                if (this.s.SupplierID != 0)
                {
                    txtCompanyname.Text = this.s.Companyname;
                    txtContactname.Text = this.s.Contactname;
                    txtTitle.Text = this.s.Contacttitle;
                    txtAddress.Text = this.s.Address;
                    txtCity.Text = this.s.City;
                    txtRegion.Text = this.s.Region;
                    txtPostalCode.Text = this.s.Postalcode;
                    txtCountry.Text = this.s.Country;
                    txtPhone.Text = this.s.Phone;
                    txtFax.Text = this.s.Fax;
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
            txtCompanyname.TextChanged += TextBox_TextChanged;
            txtContactname.TextChanged += TextBox_TextChanged;
            txtTitle.TextChanged += TextBox_TextChanged;
            txtAddress.TextChanged += TextBox_TextChanged;
            txtCity.TextChanged += TextBox_TextChanged;
            txtRegion.TextChanged += TextBox_TextChanged;
            txtPostalCode.TextChanged += TextBox_TextChanged;
            txtCountry.TextChanged += TextBox_TextChanged;
            txtPhone.TextChanged += TextBox_TextChanged;
            txtFax.TextChanged += TextBox_TextChanged;
        }

        public void TextBox_TextChanged(object sender, EventArgs e)
        {
            textChanged = true;
        }

        private bool FormFieldsChanged()
        {
            if (s == null)
                return false;

            return txtCompanyname.Text != s.Companyname ||
                   txtContactname.Text != s.Contactname ||
                   txtTitle.Text != s.Contacttitle ||
                   txtAddress.Text != s.Address ||
                   txtCity.Text != s.City ||
                   txtRegion.Text != s.Region ||
                   txtPostalCode.Text != s.Postalcode ||
                   txtCountry.Text != s.Country ||
                   txtPhone.Text != s.Phone ||
                   txtFax.Text != s.Fax;
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
    }
}
