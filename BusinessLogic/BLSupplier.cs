using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using DataLayer;
using System.Data;

namespace BusinessLogic
{
    public class BLSupplier
    {
        private static BLSupplier instance = null;
        public static BLSupplier Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new BLSupplier();
                }
                return instance;
            }
        }

        public DLSuppliers s = DLSuppliers.Instance;

        public Supplier GetSupplier(int id)
        {
            return s.GetSupplier(id);
        }

        public List<Supplier> GetSuppliers()
        {
            return s.GetSuppliers();
        }

        public List<Supplier> GetSuppliers(string naziv)
        {
            return s.GetSuppliers(naziv);
        }

        public bool UpdateSupplier(Supplier su)
        {
            s.Update(su);
            return true;
        }

        public bool InsertSupplier(Supplier su)
        {
            s.InsertData(su.Companyname, su.Contactname, su.Contacttitle, su.Address, su.City, su.Region, su.Postalcode, su.Country, su.Phone, su.Fax);
            return true;
        }

        public bool DeleteSupplier(int id)
        {
            s.Delete(id);
            return true;
        }
    }
}
