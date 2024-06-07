using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using Domain;

namespace DataLayer
{
    public class DLEmployees
    {
        private static DLEmployees instance = null;

        public static DLEmployees Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DLEmployees();
                }
                return instance;
            }
        }

        SqlConnection sc = new SqlConnection();
        SqlDataAdapter daEmployees = new SqlDataAdapter();
        public DataTable dtEmployees = new DataTable();

        public DLEmployees()
        {
            var nw = ConfigurationManager.ConnectionStrings["TSQL"];
            sc.ConnectionString = nw.ConnectionString;
            sc.Open();

            LoadEmployees();
        }

        private void LoadEmployees()
        {
            dtEmployees.Clear();

            var cmd = sc.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM [TSQL].[HR].[Employees]";
            daEmployees.SelectCommand = cmd;
            SqlCommandBuilder cb = new SqlCommandBuilder(daEmployees);

            daEmployees.Fill(dtEmployees);
            daEmployees.UpdateCommand = cb.GetUpdateCommand();
            sc.Close();
        }

        public bool InsertData(string Firstname, string Lastname, string title, string titleofcourtesy, DateTime? birthdate, DateTime? hiredate, string address, string city, string region, string postalcode, string country, string phone)
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
                    cmd.CommandText = "INSERT INTO HR.Employees (firstname, lastname, title, titleofcourtesy, birthdate, hiredate, address, city, region, postalcode, country, phone) VALUES (@Name, @LastName, @Title, @TitleOfCourtesy, @BirthDate, @HireDate, @Address, @City, @Region, @PostalCode, @Country, @Phone)";

                    cmd.Parameters.AddWithValue("@Name", Firstname);
                    cmd.Parameters.AddWithValue("@LastName", Lastname);
                    cmd.Parameters.AddWithValue("@Title", title);
                    cmd.Parameters.AddWithValue("@TitleOfCourtesy", titleofcourtesy);
                    cmd.Parameters.AddWithValue("@BirthDate", birthdate ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@HireDate", hiredate ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Address", address);
                    cmd.Parameters.AddWithValue("@City", city);
                    cmd.Parameters.AddWithValue("@Region", region);
                    cmd.Parameters.AddWithValue("@PostalCode", postalcode);
                    cmd.Parameters.AddWithValue("@Country", country);
                    cmd.Parameters.AddWithValue("@Phone", phone);

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

        private Employee Convert(DataRow dr)
        {
            Employee e = new Employee();
            e.Empid = Int32.Parse(dr["empid"].ToString());
            e.Lastname = dr["lastname"].ToString();
            e.Firstname = dr["firstname"].ToString();
            e.Title = dr["title"].ToString();
            e.Titleofcourtesy = dr["titleofcourtesy"].ToString();
            e.Birthdate = DateTime.Parse(dr["birthdate"].ToString());
            e.Hiredate = DateTime.Parse(dr["hiredate"].ToString());
            e.Address = dr["address"].ToString();
            e.City = dr["city"].ToString();
            e.Region = dr["region"].ToString();
            e.Postalcode = dr["postalcode"].ToString();
            e.Country = dr["country"].ToString();
            e.Phone = dr["phone"].ToString();

            int mgrid;
            int.TryParse(dr["mgrid"].ToString(), out mgrid);
            e.Mgrid = mgrid;

            return e;
        }

        public Employee GetEmployee(int id)
        {
            return Convert(dtEmployees.Select("empid = " + id.ToString())[0]);
        }

        public List<Employee> GetEmployees()
        {
            List<Employee> l = new List<Employee>();
            foreach (DataRow dr in dtEmployees.Rows)
            {
                l.Add(Convert(dr));
            }
            return l;
        }

        //public List<Employee> GetEmployees(string ime)
        //{
        //    List<Employee> l = new List<Employee>();

        //    foreach(DataRow dr in e.dtEmployees.Rows)
        //    {
        //        if (dr["firstname"].ToString().ToLower().Contains(ime.ToLower()) ||
        //            dr["lastname"].ToString().ToLower().Contains(ime.ToLower()) ||
        //            dr["title"].ToString().ToLower().Contains(ime.ToLower()))
        //        {
        //            l.Add(Convert(dr));
        //        }
        //    }
        //    return l;
        //}

        public List<Employee> GetEmployees(string ime)
        {
            List<Employee> employees = new List<Employee>();

            foreach (DataRow dr in dtEmployees.Rows)
            {
                Employee emp = Convert(dr);

                if (emp.Firstname.ToLower().Contains(ime.ToLower()) ||
                    emp.Lastname.ToLower().Contains(ime.ToLower()) ||
                    emp.Title.ToLower().Contains(ime.ToLower()) ||
                    emp.Titleofcourtesy.ToLower().Contains(ime.ToLower()) ||
                    emp.Address.ToLower().Contains(ime.ToLower()) ||
                    emp.City.ToLower().Contains(ime.ToLower()) ||
                    emp.Region.ToLower().Contains(ime.ToLower()) ||
                    emp.Postalcode.ToLower().Contains(ime.ToLower()) ||
                    emp.Country.ToLower().Contains(ime.ToLower()) ||
                    emp.Phone.ToLower().Contains(ime.ToLower()))
                {
                    employees.Add(emp);
                }
            }

            return employees;
        }

        //public List<Employee> GetEmployees(string ime)
        //{
        //    return dtEmployees.AsEnumerable()
        //        .Select(dr => Convert(dr))
        //        .Where(emp => emp.Firstname.ToLower().Contains(ime.ToLower()) ||
        //                      emp.Lastname.ToLower().Contains(ime.ToLower()) ||
        //                      emp.Title.ToLower().Contains(ime.ToLower()) ||
        //                      emp.Titleofcourtesy.ToLower().Contains(ime.ToLower()) ||
        //                      emp.Address.ToLower().Contains(ime.ToLower()) ||
        //                      emp.City.ToLower().Contains(ime.ToLower()) ||
        //                      emp.Region.ToLower().Contains(ime.ToLower()) ||
        //                      emp.Postalcode.ToLower().Contains(ime.ToLower()) ||
        //                      emp.Country.ToLower().Contains(ime.ToLower()) ||
        //                      emp.Phone.ToLower().Contains(ime.ToLower()))
        //        .ToList();
        //}

        void Update()
        {
            daEmployees.Update(dtEmployees);
            LoadEmployees();
        }

        public bool Update(Employee em)
        {
            DataRow dr = dtEmployees.Select("empid =" + em.Empid.ToString())[0];
            dr["empid"] = em.Empid.ToString();
            dr["lastname"] = em.Lastname.ToString();
            dr["firstname"] = em.Firstname.ToString();
            dr["title"] = em.Title.ToString();
            dr["titleofcourtesy"] = em.Titleofcourtesy.ToString();
            dr["birthdate"] = em.Birthdate;
            dr["hiredate"] = em.Hiredate;
            dr["address"] = em.Address;
            dr["city"] = em.City;
            dr["region"] = em.Region;
            dr["postalcode"] = em.Postalcode;
            dr["country"] = em.Country;
            dr["phone"] = em.Phone;

            Update();
            return true;
        }

        public bool Insert(Employee em)
        {
            if(em != null)
            {
                DataRow dr = dtEmployees.NewRow();
                dr["empid"] = em.Empid.ToString();
                dr["lastname"] = em.Lastname?.ToString();
                dr["firstname"] = em.Firstname?.ToString();
                dr["title"] = em.Title?.ToString();
                dr["titleofcourtesy"] = em.Titleofcourtesy?.ToString();
                dr["birthdate"] = em.Birthdate ?? DateTime.Parse("1900-01-01");
                dr["hiredate"] = em.Hiredate ?? DateTime.Parse("1900-01-01");
                dr["address"] = em.Address?.ToString();
                dr["city"] = em.City?.ToString();
                dr["region"] = em.Region?.ToString();
                dr["postalcode"] = em.Postalcode?.ToString();
                dr["country"] = em.Country?.ToString();
                dr["phone"] = em.Phone?.ToString();

                dtEmployees.Rows.Add(dr);

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
            dtEmployees.Select("empid = " + id.ToString())[0].Delete();

            Update();

            return true;
        }
    }
}
