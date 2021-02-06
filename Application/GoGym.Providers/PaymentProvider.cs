using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using GoGym.Data;
using GoGym.Providers.Base;
using GoGym.Providers.ViewModels;

namespace GoGym.Providers
{
    public class PaymentProvider : BaseProvider
    {
        private readonly AutoNumberProvider autoNumberProvider;

        public PaymentProvider(FitnessEntities context, IPrincipal principal, AutoNumberProvider autoNumberProvider) 
            : base(context, principal)
        {
            this.autoNumberProvider = autoNumberProvider;
        }

        public PaymentHeader GetPayment(string paymentNo)
        {
            return context.PaymentHeaders.SingleOrDefault(row => row.PaymentNo == paymentNo && !row.VoidDate.HasValue);
        }

        public PaymentHeader GetPaymentOfInvoice(string invoiceNo)
        {
            return (from pay in context.PaymentHeaders
                    join inv in context.InvoiceHeaders on pay.InvoiceID equals inv.ID
                    where inv.InvoiceNo == invoiceNo && !pay.VoidDate.HasValue
                    select pay).SingleOrDefault();
        }

        public IList<PaymentDetailViewModel> GetDetail(string invoiceNo)
        {
            var invoice = context.InvoiceHeaders.SingleOrDefault(inv => inv.InvoiceNo == invoiceNo);
            if (invoice != null)
            {
                var query = from d in context.PaymentDetails
                            join h in context.PaymentHeaders on d.PaymentID equals h.ID
                            join cct in context.CreditCardTypes on d.CreditCardTypeID equals cct.ID into temp
                            from t in temp.DefaultIfEmpty()
                            join pt in context.PaymentTypes on d.PaymentTypeID equals pt.ID
                            where h.InvoiceID == invoice.ID
                                  && !h.VoidDate.HasValue
                            select new PaymentDetailViewModel
                                   {
                                       ID = d.ID,
                                       PaymentID = d.PaymentID,
                                       PaymentTypeID = d.PaymentTypeID,
                                       PaymentType = pt.Description,
                                       CreditCardTypeID = d.CreditCardTypeID,
                                       CreditCardType = t.Description,
                                       ApprovalCode = d.ApprovalCode,
                                       Amount = d.Amount
                                   };
                return query.ToList();
            }

            return null;
        }

        public string Create(
            DateTime date,
            string invoiceNo,
            IEnumerable<PaymentDetailViewModel> detail)
        {
            string paymentNo = String.Empty;

            var invoice = context.InvoiceHeaders.SingleOrDefault(inv => inv.InvoiceNo == invoiceNo);
            if (invoice != null)
            {
                paymentNo = autoNumberProvider.Generate(invoice.BranchID, "PM", date.Month, date.Year);

                PaymentHeader h = new PaymentHeader();
                h.Date = date;
                h.InvoiceID = invoice.ID;
                h.PaymentNo = paymentNo;
                h.VoidDate = (DateTime?)null;
                context.Add(h);

                EntityHelper.SetAuditFieldForInsert(h, principal.Identity.Name);
                foreach (var model in detail)
                {
                    PaymentDetail d = new PaymentDetail();
                    d.CreditCardTypeID = model.CreditCardTypeID;
                    d.PaymentTypeID = model.PaymentTypeID;
                    d.Amount = model.Amount;
                    d.ApprovalCode = model.ApprovalCode;
                    d.PaymentHeader = h;
                    context.Add(d);
                }

                autoNumberProvider.Increment("PM", invoice.BranchID, date.Year);
                context.SaveChanges();
            }

            return paymentNo;

        }

        public void Edit(
            DateTime date,
            string invoiceNo,
            IEnumerable<PaymentDetailViewModel> detail)
        {
            var invoice = context.InvoiceHeaders.SingleOrDefault(inv => inv.InvoiceNo == invoiceNo);
            if (invoice != null)
            {
                PaymentHeader h = context.PaymentHeaders.SingleOrDefault(pay => pay.InvoiceID == invoice.ID);
                if (h != null)
                {
                    context.Delete(
                        context.PaymentDetails.Where(pay => pay.PaymentID == h.ID));

                    h.Date = date;
                    h.InvoiceID = invoice.ID;
                    EntityHelper.SetAuditFieldForInsert(h, principal.Identity.Name);
                    foreach (var model in detail)
                    {
                        PaymentDetail d = new PaymentDetail();
                        d.CreditCardTypeID = model.CreditCardTypeID;
                        d.PaymentTypeID = model.PaymentTypeID;
                        d.Amount = model.Amount;
                        d.ApprovalCode = model.ApprovalCode;
                        d.PaymentHeader = h;
                        context.Add(d);
                    }

                    autoNumberProvider.Increment("PM", invoice.BranchID, date.Year);
                    context.SaveChanges();
                }
            }

        }

    }
}
