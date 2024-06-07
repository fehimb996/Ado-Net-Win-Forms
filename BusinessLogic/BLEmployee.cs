using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;
using Domain;

namespace BusinessLogic
{
    public class BLEmployee
    {
        private static BLEmployee instance = null;
        public static BLEmployee Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new BLEmployee();
                }
                return instance;
            }
        }

        public DLEmployees e = DLEmployees.Instance;

        public Employee GetEmployee(int id)
        {
            return e.GetEmployee(id);
        }

        public List<Employee> GetEmployees()
        {
            return e.GetEmployees();
        }

        public List<Employee> GetEmployees(string ime)
        {
            return e.GetEmployees(ime);
        }

        public bool UpdateEmployee(Employee em)
        {
            e.Update(em);
            return true;
        }

        public bool InsertEmployee(Employee em)
        {
            e.InsertData(em.Firstname, em.Lastname, em.Title, em.Titleofcourtesy, em.Birthdate, em.Hiredate, em.Address, em.City, em.Region, em.Postalcode, em.Country, em.Phone);
            return true;
        }

        public bool DeleteEmployee(int id)
        {
            e.Delete(id);
            return true;
        }
    }
}
