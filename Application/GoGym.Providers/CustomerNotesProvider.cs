using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using GoGym.Data;
using GoGym.Providers.Base;

namespace GoGym.Providers
{
    public class CustomerNotesProvider : BaseProvider
    {
        public CustomerNotesProvider(FitnessEntities context, IPrincipal principal)
            : base(context, principal)
        {
        }

        public void Add(
            string customerBarcode,
            string notes,
            short priority
            )
        {
            int customerID = context.Customers.Single(c => c.Barcode == customerBarcode).ID;
            Add(customerID, notes, priority);
        }

        public void Delete(int id)
        {
            CustomerNote node = context.CustomerNotes.SingleOrDefault(note => note.ID == id);
            if (node != null)
            {
                context.Delete(node);
                context.SaveChanges();
            }
        }

        public void Add(
            int customerID,
            string notes,
            short priority
            )
        {
            CustomerNote note = new CustomerNote();
            note.CustomerID = customerID;
            note.Notes = notes;
            note.Priority = priority;
            EntityHelper.SetAuditFieldForInsert(note, principal.Identity.Name);
            context.Add(note);
            context.SaveChanges();
        }

        public void Update(
            int customerID,
            string notes,
            short priority
            )
        {
            CustomerNote note = new CustomerNote();
            note.CustomerID = customerID;
            note.Notes = notes;
            note.Priority = priority;
            EntityHelper.SetAuditFieldForInsert(note, principal.Identity.Name);
            context.SaveChanges();
        }




        public void ToggleNote(int id)
        {
            CustomerNote note = context.CustomerNotes.SingleOrDefault(n => n.ID == id);
            if (note != null)
            {
                note.Priority = Convert.ToInt16((note.Priority - (short)1) * (short)-1);
                context.SaveChanges();
            }
        }

        public IEnumerable<CustomerNote> GetTopNotes(string customerBarcode)
        {
            var query = (from note in context.CustomerNotes
                         join cust in context.Customers on note.CustomerID equals cust.ID
                         where cust.Barcode == customerBarcode
                         orderby note.CreatedWhen descending
                         select note).Take(3);
            return query.ToList();
        }
    }
}
