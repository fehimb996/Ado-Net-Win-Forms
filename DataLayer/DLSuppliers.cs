using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Domain;

namespace DataLayer
{
    public class DLSuppliers
    {
        private static DLSuppliers instance = null;

        public static DLSuppliers Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DLSuppliers();
                }
                return instance;
            }
        }

        SqlConnection sc = new SqlConnection();
        SqlDataAdapter daSuppliers = new SqlDataAdapter();
        public DataTable dtSuppliers = new DataTable();

        public DLSuppliers()
        {
            var nw = ConfigurationManager.ConnectionStrings["TSQL"];
            sc.ConnectionString = nw.ConnectionString;
            sc.Open();

            LoadSuppliers();
        }

        private void LoadSuppliers()
        {
            dtSuppliers.Clear();

            var cmd = sc.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM [TSQL].[Production].[Suppliers]";
            daSuppliers.SelectCommand = cmd;
            SqlCommandBuilder cb = new SqlCommandBuilder(daSuppliers);

            daSuppliers.Fill(dtSuppliers);
            daSuppliers.UpdateCommand = cb.GetUpdateCommand();
            sc.Close();
        }

        public bool InsertData(string companyName, string contactName, string contactTitle, string Address, string City, string Region, string Postalcode, string Country, string Phone, string Fax)
        {
            if (sc.State == ConnectionState.Closed)
                sc.Open();

            using (SqlTransaction tran = sc.BeginTransaction())
            {
                try
                {
                    var cmd = sc.CreateCommand();
                    cmd.Transaction = tran;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "INSERT INTO Production.Suppliers (companyname, contactname, contacttitle, address, city, region, postalcode, country, phone, fax) VALUES (@companyName, @contactName, @contactTitle, @Address, @City, @Region, @Postalcode, @Country, @Phone, @Fax)";

                    cmd.Parameters.AddWithValue("@companyName", companyName);
                    cmd.Parameters.AddWithValue("@contactname", contactName);
                    cmd.Parameters.AddWithValue("@contacttitle", contactTitle);
                    cmd.Parameters.AddWithValue("@address", Address);
                    cmd.Parameters.AddWithValue("@city", City);
                    cmd.Parameters.AddWithValue("@region", Region);
                    cmd.Parameters.AddWithValue("@postalcode", Postalcode);
                    cmd.Parameters.AddWithValue("@country", Country);
                    cmd.Parameters.AddWithValue("@phone", Phone);
                    cmd.Parameters.AddWithValue("@fax", Fax);

                    cmd.ExecuteNonQuery();
                    tran.Commit();
                    Update();
                    return true;
                }
                catch
                {
                    tran.Rollback();
                    return false;
                }
            }
        }

        private Supplier Convert(DataRow dr)
        {
            Supplier s = new Supplier();
            s.SupplierID = Int32.Parse(dr["supplierid"].ToString());
            s.Companyname = dr["companyname"].ToString();
            s.Contactname = dr["contactname"].ToString();
            s.Contacttitle = dr["contacttitle"].ToString();
            s.Address = dr["address"].ToString();
            s.City = dr["city"].ToString();
            s.Region = dr["region"].ToString();
            s.Postalcode = dr["postalcode"].ToString();
            s.Country = dr["country"].ToString();
            s.Phone = dr["phone"].ToString();
            s.Fax = dr["fax"].ToString();

            return s;
        }

        public Supplier GetSupplier(int id)
        {
            return Convert(dtSuppliers.Select("supplierid = " + id.ToString())[0]);
        }

        public List<Supplier> GetSuppliers()
        {
            List<Supplier> l = new List<Supplier>();

            foreach (DataRow dr in dtSuppliers.Rows)
            {
                l.Add(Convert(dr));
            }
            return l;
        }

        //public List<Supplier> GetSuppliers(string naziv)
        //{
        //    List<Supplier> l = new List<Supplier>();

        //    foreach (DataRow dr in s.dtSuppliers.Rows)
        //    {
        //        if (dr["companyname"].ToString().ToLower().Contains(naziv.ToLower()) ||
        //            dr["contactname"].ToString().ToLower().Contains(naziv.ToLower()) ||
        //            dr["contacttitle"].ToString().ToLower().Contains(naziv.ToLower()))
        //        {
        //            l.Add(Convert(dr));
        //        }
        //    }
        //    return l;
        //}

        public List<Supplier> GetSuppliers(string naziv)
        {
            List<Supplier> suppliers = new List<Supplier>();

            foreach (DataRow dr in dtSuppliers.Rows)
            {
                Supplier supplier = Convert(dr);

                if (supplier.Companyname.ToLower().Contains(naziv.ToLower()) ||
                    supplier.Contactname.ToLower().Contains(naziv.ToLower()) ||
                    supplier.Contacttitle.ToLower().Contains(naziv.ToLower()) ||
                    supplier.Address.ToLower().Contains(naziv.ToLower()) ||
                    supplier.City.ToLower().Contains(naziv.ToLower()) ||
                    supplier.Region.ToLower().Contains(naziv.ToLower()) ||
                    supplier.Postalcode.ToLower().Contains(naziv.ToLower()) ||
                    supplier.Country.ToLower().Contains(naziv.ToLower()) ||
                    supplier.Phone.ToLower().Contains(naziv.ToLower()) ||
                    supplier.Fax.ToLower().Contains(naziv.ToLower()))
                {
                    suppliers.Add(supplier);
                }
            }

            return suppliers;
        }

        //public List<Supplier> GetSuppliers(string naziv)
        //{
        //    return dtSuppliers.AsEnumerable()
        //        .Select(dr => Convert(dr))
        //        .Where(supplier => supplier.Companyname.ToLower().Contains(naziv.ToLower()) ||
        //                           supplier.Contactname.ToLower().Contains(naziv.ToLower()) ||
        //                           supplier.Contacttitle.ToLower().Contains(naziv.ToLower()) ||
        //                           supplier.Address.ToLower().Contains(naziv.ToLower()) ||
        //                           supplier.City.ToLower().Contains(naziv.ToLower()) ||
        //                           supplier.Region.ToLower().Contains(naziv.ToLower()) ||
        //                           supplier.Postalcode.ToLower().Contains(naziv.ToLower()) ||
        //                           supplier.Country.ToLower().Contains(naziv.ToLower()) ||
        //                           supplier.Phone.ToLower().Contains(naziv.ToLower()) ||
        //                           supplier.Fax.ToLower().Contains(naziv.ToLower()))
        //        .ToList();
        //}


        void Update()
        {
            daSuppliers.Update(dtSuppliers);
            LoadSuppliers();
        }

        public bool Update(Supplier su)
        {
            DataRow dr = dtSuppliers.Select("supplierid = " + su.SupplierID.ToString())[0];
            dr["supplierid"] = su.SupplierID.ToString();
            dr["companyname"] = su.Companyname.ToString();
            dr["contactname"] = su.Contactname.ToString();
            dr["contacttitle"] = su.Contacttitle.ToString();
            dr["address"] = su.Address.ToString();
            dr["city"] = su.City.ToString();
            dr["region"] = su.Region.ToString();
            dr["postalcode"] = su.Postalcode.ToString();
            dr["country"] = su.Country.ToString();
            dr["phone"] = su.Phone.ToString();
            dr["fax"] = su.Fax.ToString();

            Update();
            return true;
        }

        public bool Insert(Supplier su)
        {
            if (su != null)
            {
                DataRow dr = dtSuppliers.NewRow();
                dr["supplierid"] = su.SupplierID.ToString();
                dr["companyname"] = su.Companyname?.ToString();
                dr["contactname"] = su.Contactname?.ToString();
                dr["contacttitle"] = su.Contacttitle?.ToString();
                dr["address"] = su.Address?.ToString();
                dr["city"] = su.City?.ToString();
                dr["region"] = su.Region?.ToString();
                dr["postalcode"] = su.Postalcode?.ToString();
                dr["country"] = su.Country?.ToString();
                dr["phone"] = su.Phone?.ToString();
                dr["fax"] = su.Fax?.ToString();

                dtSuppliers.Rows.Add(dr);

                Update();
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Delete(int id)
        {
            dtSuppliers.Select("supplierid = " + id.ToString())[0].Delete();

            Update();

            return true;
        }
    }
}
