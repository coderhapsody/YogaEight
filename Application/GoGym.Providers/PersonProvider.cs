using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using GoGym.Data;
using GoGym.Providers.Base;

namespace GoGym.Providers
{
    public static class PersonConnection
    {
        public static readonly string Father = "F";
        public static readonly string Mother = "M";
        public static readonly string Guardian = "G";
        public static readonly string PickUp = "P";
    }


    public class PersonProvider : BaseProvider
    {
        public PersonProvider(FitnessEntities context, IPrincipal principal) : base(context, principal)
        {
        }

        public bool FatherIsExist(string customerCode)
        {
            return context.People.Any(p => p.Customer.Barcode == customerCode && p.Connection == 'F');
        }

        public bool MotherIsExist(string customerCode)
        {
            return context.People.Any(p => p.Customer.Barcode == customerCode && p.Connection == 'M');
        }

        public void Add(
            string customerCode,
            string connection,
            string name,
            DateTime? birthDate,
            string idCardNo,
            string email,
            string phone1,
            string phone2,
            string photo)
        {
            Customer customer = context.Customers.SingleOrDefault(cust => cust.Barcode == customerCode);
            if (customer != null)
            {
                Person obj = new Person();
                obj.Connection = Convert.ToChar(connection);
                obj.Name = name;
                obj.BirthDate = birthDate;
                obj.IDCardNo = idCardNo;
                obj.Phone1 = phone1;
                obj.Phone2 = phone2;
                obj.Photo = photo;
                obj.Customer = customer;
                EntityHelper.SetAuditFieldForInsert(obj, principal.Identity.Name);
                context.Add(obj);
                context.SaveChanges();
            }
        }


        public void Update(
            int id,
            string customerCode,
            string connection,
            string name,
            DateTime? birthDate,
            string idCardNo,
            string email,
            string phone1,
            string phone2,
            string photo
            )
        {
            Customer customer = context.Customers.SingleOrDefault(cust => cust.Barcode == customerCode);
            if (customer != null)
            {
                Person obj = context.People.Single(row => row.ID == id);
                obj.Connection = Convert.ToChar(connection);
                obj.Name = name;
                obj.BirthDate = birthDate;
                obj.IDCardNo = idCardNo;
                obj.Phone1 = phone1;
                obj.Phone2 = phone2;
                obj.Email = email;
                obj.Photo = photo;
                obj.Customer = customer;
                EntityHelper.SetAuditFieldForUpdate(obj, principal.Identity.Name);
                context.SaveChanges();
            }
        }

        public void Delete(int[] id)
        {
            context.Delete(
                context.People.Where(row => id.Contains(row.ID)));
            context.SaveChanges();
        }

        public Person Get(int id)
        {
            return context.People.SingleOrDefault(row => row.ID == id);
        }

        public IEnumerable<Person> GetParentsInformation(string customerCode)
        {
            return context.People.Where(row => row.Customer.Barcode == customerCode).ToList();
        }

        public void UpdatePhoto(int id, string fileName)
        {
            Person obj = context.People.Single(row => row.ID == id);
            obj.Photo = fileName;
            EntityHelper.SetAuditFieldForUpdate(obj, principal.Identity.Name);
            context.SaveChanges();
        }

        public void DeletePhoto(int id)
        {
            Person obj = context.People.Single(row => row.ID == id);
            obj.Photo = null;
            EntityHelper.SetAuditFieldForUpdate(obj, principal.Identity.Name);
            context.SaveChanges();
        }
    }
}
