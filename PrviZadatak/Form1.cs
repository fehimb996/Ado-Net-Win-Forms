using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Domain;
using BusinessLogic;

namespace PrviZadatak
{
    public partial class Form1 : Form
    {
        BLEmployee BLemployee = BLEmployee.Instance;
        BLSupplier BLsupplier = BLSupplier.Instance;
        private string parametarPretrage;

        public Form1()
        {
            InitializeComponent();

            dgvPodaci.DataSource = BLemployee.GetEmployees();
            dgvPodaci.DataSource = BLsupplier.GetSuppliers();
            dgvPodaci.SelectionChanged += dgvPodaci_SelectionChanged;

            txtPretraga.TextChanged += txtPretraga_TextChanged;
        }

        private void btnIzmeni_Click(object sender, EventArgs e)
        {
            if (dgvPodaci.SelectedRows.Count > 0)
            {
                string selectedTable = comboBox1.SelectedItem.ToString();
                if (selectedTable == "Employees")
                {
                    string id = dgvPodaci.SelectedRows[0].Cells[0].Value.ToString();
                    int empId;
                    if (int.TryParse(id, out empId))
                    {
                        Employee selectedEmployee = BLemployee.GetEmployee(empId);
                        if (selectedEmployee != null)
                        {
                            Form2 frm = new Form2();
                            frm.e = selectedEmployee;
                            if (frm.ShowDialog() == DialogResult.OK)
                            {
                                BLemployee.UpdateEmployee(selectedEmployee);
                                dgvPodaci.DataSource = BLemployee.GetEmployees();
                            }
                        }
                    }
                }
                else if (selectedTable == "Suppliers")
                {
                    string id = dgvPodaci.SelectedRows[0].Cells[0].Value.ToString();
                    int supplierId;
                    if (int.TryParse(id, out supplierId))
                    {
                        Supplier selectedSupplier = BLsupplier.GetSupplier(supplierId);
                        if (selectedSupplier != null)
                        {
                            Form3 frm = new Form3();
                            frm.s = selectedSupplier;
                            if (frm.ShowDialog() == DialogResult.OK)
                            {
                                BLsupplier.UpdateSupplier(selectedSupplier);
                                dgvPodaci.DataSource = BLsupplier.GetSuppliers();
                            }
                        }
                    }
                }
            }
            RefreshDataGridView();
        }

        private void btnNovi_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem.ToString() == "Employees")
            {
                Form2 frm = new Form2();
                Employee em = new Employee();
                frm.e = em;

                if (frm.ShowDialog() == DialogResult.OK)
                {
                    BLemployee.InsertEmployee(em);
                    dgvPodaci.DataSource = BLemployee.GetEmployees();
                }
            }
            else if (comboBox1.SelectedItem.ToString() == "Suppliers")
            {
                Form3 frm = new Form3();
                Supplier sup = new Supplier();
                frm.s = sup;

                if (frm.ShowDialog() == DialogResult.OK)
                {
                    BLsupplier.InsertSupplier(sup);
                    dgvPodaci.DataSource = BLsupplier.GetSuppliers();
                }
            }
        }

        private void btnObrisi_Click(object sender, EventArgs e)
        {
            if (dgvPodaci.SelectedRows.Count > 0)
            {
                string selectedTable = comboBox1.SelectedItem.ToString();
                if (selectedTable == "Employees")
                {
                    int id;
                    if (Int32.TryParse(dgvPodaci.SelectedRows[0].Cells[0].Value.ToString(), out id))
                    {
                        BLemployee.DeleteEmployee(id);
                        dgvPodaci.DataSource = BLemployee.GetEmployees();
                    }
                }
                else if (selectedTable == "Suppliers")
                {
                    int id;
                    if (Int32.TryParse(dgvPodaci.SelectedRows[0].Cells[0].Value.ToString(), out id))
                    {
                        BLsupplier.DeleteSupplier(id);
                        dgvPodaci.DataSource = BLsupplier.GetSuppliers();
                    }
                }
            }
            //RefreshDataGridView();
        }

        private void btnPretrazi_Click(object sender, EventArgs e)
        {
            // string ime = txtPretraga.Text;
            //parametarPretrage = txtPretraga.Text;

            //string selectedTable = comboBox1.SelectedItem.ToString();
            //if (selectedTable == "Employees")
            //{
            //    dgvPodaci.DataSource = BLemployee.GetEmployees(parametarPretrage);
            //}
            //else if (selectedTable == "Suppliers")
            //{
            //    dgvPodaci.DataSource = BLsupplier.GetSuppliers(parametarPretrage);
            //}

            //RefreshDataGridView();
        }

        private void txtPretraga_TextChanged(object sender, EventArgs e)
        {
            parametarPretrage = txtPretraga.Text;

            string selectedTable = comboBox1.SelectedItem.ToString();
            if (selectedTable == "Employees")
            {
                dgvPodaci.DataSource = BLemployee.GetEmployees(parametarPretrage);
            }
            else if (selectedTable == "Suppliers")
            {
                dgvPodaci.DataSource = BLsupplier.GetSuppliers(parametarPretrage);
            }

            RefreshDataGridView();
        }

        private void dgvPodaci_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvPodaci.SelectedRows.Count > 0)
            {
                btnIzmeni.Enabled = true;
                btnObrisi.Enabled = true;
            }
            else
            {
                btnIzmeni.Enabled = false;
                btnObrisi.Enabled = false;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowSelectedTable();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Add("Employees");
            comboBox1.Items.Add("Suppliers");

            comboBox1.SelectedIndex = 0;
            ShowSelectedTable();
        }

        private void ShowSelectedTable()
        {
            string selectedTable = comboBox1.SelectedItem.ToString();
            if (selectedTable == "Employees")
            {
                dgvPodaci.DataSource = BLemployee.GetEmployees();
            }
            else if (selectedTable == "Suppliers")
            {
               dgvPodaci.DataSource = BLsupplier.GetSuppliers();
            }
        }

        private void RefreshDataGridView()
        {
            if (comboBox1.SelectedItem != null)
            {
                string selectedTable = comboBox1.SelectedItem.ToString();
                if (selectedTable == "Employees" && BLemployee != null)
                {
                    if (parametarPretrage != null)
                    {
                        dgvPodaci.DataSource = BLemployee.GetEmployees(parametarPretrage);
                    }
                }
                else if (selectedTable == "Suppliers" && BLsupplier != null)
                {
                    if (parametarPretrage != null)
                    {
                        dgvPodaci.DataSource = BLsupplier.GetSuppliers(parametarPretrage);
                    }
                }
            }
        }
    }
}
