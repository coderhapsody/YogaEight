using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using GoGym.Data;
using GoGym.Providers.Base;
using GoGym.Providers.ViewModels;
using Ninject.Extensions.Logging;

namespace GoGym.Providers
{
    public class BillingProvider : BaseProvider
    {
        private ILogger logger;
        private readonly AutoNumberProvider autoNumberProvider;
        private readonly InvoiceProvider invoiceProvider;

        public BillingProvider(FitnessEntities context,
                               IPrincipal principal,
                               ILogger logger,
                               AutoNumberProvider autoNumberProvider,
                               InvoiceProvider invoiceProvider) : base(context, principal)
        {
            this.logger = logger;
            this.autoNumberProvider = autoNumberProvider;
            this.invoiceProvider = invoiceProvider;
        }

        public BillingHeader GetBillingInfo(string batchNo)
        {
            return context.BillingHeaders.SingleOrDefault(b => b.BatchNo == batchNo);
        }

        public void DeleteBillingHistoryByProcessDate(DateTime date)
        {
            foreach (var billing in context.BillingHeaders.Where(b => b.ProcessDate == date))
            {
                context.Delete(billing.BillingDetails);
                context.Delete(billing);
            }
            context.SaveChanges();
        }

        public IEnumerable<int> GetYears()
        {
            IList<int> years = context.BillingHeaders.Select(b => b.ProcessDate.Year).Distinct().ToList();
            if (years.Count == 0)
                years.Add(DateTime.Today.Year);

            return years;
        }

        public bool IsMerchantCodeValid(int branchID)
        {
            Branch branch = context.Branches.SingleOrDefault(br => br.ID == branchID);
            if (branch != null)
            {
                return !String.IsNullOrEmpty(branch.MerchantCode.Trim());
            }

            return false;
        }

        public string ProcessBilling(int branchID, int billingTypeID, string[] contractNumbers, DateTime processDate, string processedByUserName, DateTime nextDueDateFrom, DateTime nextDueDateTo, string path)
        {

            string billingFile = String.Empty;
            try
            {
                Employee employee = context.Employees.SingleOrDefault(emp => emp.UserName == processedByUserName);
                Branch branch = context.Branches.SingleOrDefault(b => b.ID == branchID);
                if (employee != null && branch != null)
                {
                    billingFile = String.Format("{0}{1}.txt", branch.Code, processDate.ToString("ddMMyy"));

                    IEnumerable<BillingViewModel> billings = GenerateBillingData(branchID, contractNumbers, nextDueDateFrom, nextDueDateTo);
                    IList<BillingViewModel> bills = billings.ToList();
                    bills = CreateBillingInvoices(branchID, billingTypeID, bills, processDate, employee.ID, billingFile);
                    CreateBillingFile(branchID, bills, processDate, path);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw;
            }

            return billingFile;
        }

        public string GenerateBillingUnpaidInvoice(int branchID, string[] invoicesNo, string path)
        {
            var list = new List<BillingViewModel>();
            Branch branch = context.Branches.SingleOrDefault(b => b.ID == branchID);
            if (branch != null)
            {
                foreach (string invoiceNo in invoicesNo)
                {
                    InvoiceHeader invoice = context.InvoiceHeaders.SingleOrDefault(inv => inv.InvoiceNo == invoiceNo);
                    if (invoice != null)
                    {
                        var billing = new BillingViewModel();
                        billing.MerchantCode = branch.MerchantCode;
                        billing.CustomerBarcode = invoice.Customer.Barcode;
                        billing.CreditCardNo = invoice.Customer.CardNo;
                        billing.CreditCardExpiredDate = invoice.Customer.ExpiredDate.Value;
                        billing.DuesAmount = invoice.InvoiceDetails.Sum(inv => (inv.Quantity * inv.UnitPrice) - (inv.Discount / 100 * inv.Quantity * inv.UnitPrice));
                        billing.CreditCardName = invoice.Customer.CardHolderName;
                        billing.Note = "THE GYM MONTHLY PAYMENT," + invoice.InvoiceNo;
                        list.Add(billing);
                    }
                }
            }
            string fileName = CreateBillingFile(branchID, list, DateTime.Today, path, true);
            return fileName;
        }

        private string CreateBillingFile(int branchID, IEnumerable<BillingViewModel> billings, DateTime processDate, string path, bool resend = false)
        {
            Branch branch = context.Branches.SingleOrDefault(b => b.ID == branchID);
            string branchCode = branch.Code;
            string fileName = String.Format("{0}{1}.txt", branchCode, processDate.ToString("ddMMyy"));
            IList<string> lines = new List<string>();
            foreach (BillingViewModel billing in billings)
            {
                StringBuilder billingLine = new StringBuilder();
                billingLine.Append(billing.MerchantCode);
                billingLine.Append(billing.CustomerBarcode.PadRight(25));
                billingLine.Append(billing.CreditCardNo);
                billingLine.Append(billing.CreditCardExpiredDate.ToString("MMyy"));
                billingLine.Append("000");
                billingLine.Append(Convert.ToInt32(billing.DuesAmount));
                billingLine.Append("00");
                billingLine.Append(billing.CreditCardName.PadRight(26));
                billingLine.Append(billing.Note.PadRight(71));
                billingLine.Append("0");
                lines.Add(billingLine.ToString());
            }
            if (resend)
                fileName = "RESEND_" + fileName;
            File.WriteAllLines(path + fileName, lines.ToArray());

            return fileName;
        }

        private IEnumerable<BillingViewModel> GenerateBillingData(int branchID, string[] contractNumbers, DateTime nextDueDateFrom, DateTime nextDueDateTo)
        {
            Branch branch = context.Branches.SingleOrDefault(b => b.ID == branchID);
            if (branch != null)
            {
                string merchantCode = branch.MerchantCode;
                foreach (string contractNo in contractNumbers)
                {

                    Contract contract = context.Contracts.SingleOrDefault(c => c.ContractNo == contractNo);
                    Customer customer = contract.Customer;
                    decimal duesAmount = 0;

                    if (contract != null && customer != null)
                    {
                        var query = (from data in context.AllCustomersDetails
                                     where data.CustomerID == customer.ID
                                       && data.NextDuesDate.Value >= nextDueDateFrom
                                       && data.NextDuesDate.Value <= nextDueDateTo
                                     select data).ToList();
                        if (query != null)
                        {
                            duesAmount = query.Last().CustomerStatusID == 6
                                ? contract.PackageHeader.FreezeFee
                                : contract.DuesAmount + contract.AdditionalDuesAmount;
                        }

                        string customerBarcode = customer.Barcode;
                        string creditCardNo = customer.CardNo;
                        DateTime creditCardExpiredDate = customer.ExpiredDate.Value;
                        string creditCardName = customer.CardHolderName;
                        //decimal duesAmount = duesAmount;
                        string note = "THE GYM MONTHLY PAYMENT"; // contractNo;
                        yield return new BillingViewModel
                        {
                            MerchantCode = merchantCode,
                            CreditCardExpiredDate = creditCardExpiredDate,
                            CreditCardName = creditCardName,
                            CreditCardNo = creditCardNo,
                            CustomerBarcode = customerBarcode,
                            Note = note,
                            DuesAmount = duesAmount,
                            ContractNo = contractNo
                        };
                    }

                }
            }
        }

        private IList<BillingViewModel> CreateBillingInvoices(int branchID, int billingTypeID, IEnumerable<BillingViewModel> billings, DateTime processDate, int processedByEmployeeID, string billingFileName)
        {
            List<BillingViewModel> list = new List<BillingViewModel>();            
            string autoNumber = autoNumberProvider.Generate(branchID, "BL", processDate.Month, processDate.Year);

            BillingHeader billingHeader = new BillingHeader();
            billingHeader.BatchNo = autoNumber;
            billingHeader.BillingTypeID = billingTypeID;
            billingHeader.BranchID = branchID;
            billingHeader.UserName = context.Employees.Single(emp => emp.ID == processedByEmployeeID).UserName;
            billingHeader.ProcessDate = processDate;
            billingHeader.FileName = billingFileName;

            foreach (BillingViewModel billing in billings)
            {
                Contract contract = context.Contracts.SingleOrDefault(c => c.ContractNo == billing.ContractNo);                
                Customer customer = contract.Customer;
                Item item = context.Items.SingleOrDefault(it => it.ID == contract.BillingItemID.Value);
                PackageHeader package = contract.PackageHeader;
                if (contract != null && package != null && customer != null && item != null)
                {
                    var invoiceDetail = new InvoiceDetailViewModel();
                    invoiceDetail.InvoiceID = 0;
                    invoiceDetail.ItemID = contract.BillingItemID.Value;
                    invoiceDetail.Quantity = 1;
                    invoiceDetail.UnitName = item.UnitName1;
                    invoiceDetail.UnitPrice = billing.DuesAmount;
                    invoiceDetail.Discount = 0;
                    invoiceDetail.IsTaxable = true;

                    var paymentDetail = new PaymentDetailViewModel();
                    paymentDetail.PaymentTypeID = context.PaymentTypes.SingleOrDefault(p => p.Description == "Credit Card").ID;
                    paymentDetail.CreditCardTypeID = customer.CreditCardTypeID.HasValue ? customer.CreditCardTypeID.Value : context.CreditCardTypes.SingleOrDefault(cc => cc.Description == "Visa").ID;
                    paymentDetail.ApprovalCode = String.Empty;
                    paymentDetail.Amount = billing.DuesAmount;
                    paymentDetail.Notes = "Auto Pay";
                    paymentDetail.PaymentID = 0;

                    InvoiceHeader invoiceHeader = invoiceProvider.CreateExistingMemberInvoiceForBilling(branchID,
                        processDate,
                        customer.Barcode,
                        processedByEmployeeID,
                        "Auto pay " + contract.ContractNo,
                        0,
                        new List<InvoiceDetailViewModel>() { invoiceDetail },
                        new List<PaymentDetailViewModel>() { paymentDetail });


                    var billingDetail = new BillingDetail();
                    billingDetail.Amount = billing.DuesAmount;
                    billingDetail.Contract = contract;
                    billingDetail.Customer = customer;
                    billingDetail.InvoiceHeader = invoiceHeader;
                    billingDetail.PackageHeader = package;
                    billingHeader.BillingDetails.Add(billingDetail);

                    context.Add(billingHeader);

                    billing.Note += "," + invoiceHeader.InvoiceNo;
                    list.Add(billing);
                }

                if (contract.NextDuesDate.HasValue)
                {
                    contract.NextDuesDate = contract.NextDuesDate.Value.AddMonths(contract.DuesInMonth);
                }
            }


            autoNumberProvider.Increment("BL", branchID, processDate.Year);
            context.SaveChanges();

            return list;
        }

        public IEnumerable<AllCustomersDetail> GetBillingData(
            int branchID,
            int billingTypeID,
            int[] customerStatusID,
            int[] packagesID,
            DateTime nextDueDateFrom,
            DateTime nextDueDateTo)
        {
            return from data in context.AllCustomersDetails
                   where packagesID.Contains(data.PackageID.Value)
                      && customerStatusID.Contains(data.CustomerStatusID)
                      && data.HomeBranchID == branchID
                      && data.NextDuesDate.HasValue
                      && data.BillingTypeID.HasValue
                      && data.BillingTypeID.Value == billingTypeID
                      && data.NextDuesDate.Value >= nextDueDateFrom
                      && data.NextDuesDate.Value <= nextDueDateTo
                   select data;
        }


        public int ProcessBillingResult(string batchNo, string fileName)
        {
            List<BillingRejectionViewModel> rejections = ReadBillingResultFile(fileName).ToList();
            foreach (BillingRejectionViewModel rejection in rejections)
            {
                InvoiceHeader invoice = context.InvoiceHeaders.SingleOrDefault(ih => ih.InvoiceNo == rejection.InvoiceNo && !ih.VoidDate.HasValue);
                if (invoice != null)
                {
                    var payments = invoice.PaymentHeaders.Where(ph => !ph.VoidDate.HasValue);
                    foreach (var payment in payments)
                    {
                        payment.VoidDate = DateTime.Today;
                        payment.VoidReason = "AUTO PAY DECLINED: " + rejection.DeclineCode;
                    }

                    var status = new CustomerStatusHistory();
                    status.Customer = invoice.Customer;
                    status.Date = DateTime.Today;
                    status.CustomerStatusID = 4; // billing problem
                    status.Notes = "AUTO PAY DECLINED OF INVOICE " + invoice.InvoiceNo + ", REJECTION CODE: " + rejection.DeclineCode;
                    status.StartDate = DateTime.Today;
                    EntityHelper.SetAuditFieldForInsert(status, principal.Identity.Name);
                }
            }

            BillingHeader billing = context.BillingHeaders.SingleOrDefault(b => b.BatchNo == batchNo);
            if (billing != null)
            {
                billing.ResultProcessDate = DateTime.Now;
                foreach (var detail in billing.BillingDetails)
                {
                    var rejection =
                        rejections.SingleOrDefault(reject => reject.InvoiceNo == detail.InvoiceHeader.InvoiceNo);
                    if (rejection != null)
                    {
                        detail.BillingResult = rejection.DeclineCode;
                    }
                }
            }

            context.SaveChanges();

            return rejections.Count;

            //foreach (var billingDetail in billing.BillingDetails)
            //{                
            //    InvoiceHeader invoice = billingDetail.InvoiceHeader;
            //    if (invoice != null)
            //    {
            //        PaymentHeader payment = invoice.PaymentHeaders.SingleOrDefault(ph => !ph.VoidDate.HasValue);
            //        if (payment != null)
            //        {
            //            payment.VoidDate = DateTime.Today;
            //            payment.VoidReason = "AUTO PAY DECLINED: " + rejection.DeclineCode;
            //        }
            //    }
            //}
        }

        public IEnumerable<BillingAcceptedViewModel> ReadBillingUnpaidAcceptedFile(string fileName)
        {
            IList<BillingAcceptedViewModel> list = new List<BillingAcceptedViewModel>();
            string[] lines = File.ReadAllLines(fileName);
            foreach (string line in lines)
            {
                var model = new BillingAcceptedViewModel();

                model.MerchantCode = line.Substring(0, 8).Trim();
                model.CustomerBarcode = line.Substring(9, 25).Trim();
                model.CreditCardNo = line.Substring(34, 16).Trim();
                model.CreditCardExpiredDate = new DateTime(Convert.ToInt32("20" + line.Substring(52, 2)), Convert.ToInt32(line.Substring(50, 2)), 1);
                model.CreditCardName = line.Substring(66, 26).Trim();
                model.Note = line.Substring(91, 71).Trim();

                try
                {
                    string[] notes = model.Note.Split(',');
                    //model.ContractNo = notes[0];
                    model.InvoiceNo = notes[1];
                    model.InvoiceNo = model.InvoiceNo.Replace(".", "X");
                }
                catch
                {
                }

                model.VerificationCode = line.Substring(161, 10).Trim();

                yield return model;
            }
        }


        public IEnumerable<BillingRejectionViewModel> ReadBillingResultFile(string fileName)
        {
            IList<BillingRejectionViewModel> list = new List<BillingRejectionViewModel>();
            string[] lines = File.ReadAllLines(fileName);
            foreach (string line in lines)
            {
                var model = new BillingRejectionViewModel();

                model.MerchantCode = line.Substring(0, 8).Trim();
                model.CustomerBarcode = line.Substring(9, 25).Trim();
                model.CreditCardNo = line.Substring(34, 16).Trim();
                model.CreditCardExpiredDate = new DateTime(Convert.ToInt32("20" + line.Substring(52, 2)), Convert.ToInt32(line.Substring(50, 2)), 1);
                model.CreditCardName = line.Substring(66, 26).Trim();
                model.Note = line.Substring(91, 71).Trim();

                try
                {
                    string[] notes = model.Note.Split(',');
                    //model.ContractNo = notes[0];
                    model.InvoiceNo = notes[1];
                    model.InvoiceNo = model.InvoiceNo.Replace(".", "X");
                }
// ReSharper disable once EmptyGeneralCatchClause
                catch
                {
                }

                model.DeclineCode = line.Substring(169, 2).Trim();

                yield return model;
            }
        }

        public int ProcessBillingUnpaidInvoice(string fileName)
        {
            // accepted
            List<BillingAcceptedViewModel> acceptedInvoices = ReadBillingUnpaidAcceptedFile(fileName).ToList();
            foreach (BillingAcceptedViewModel acceptedInvoice in acceptedInvoices)
            {
                InvoiceHeader invoice = context.InvoiceHeaders.SingleOrDefault(ih => ih.InvoiceNo == acceptedInvoice.InvoiceNo);
                if (invoice != null)
                {
                    Customer cust = invoice.Customer;
                               
                    var payments = invoice.PaymentHeaders.Where(ph => !ph.VoidDate.HasValue);
                    foreach (var payment in payments)
                    {
                        payment.VoidDate = DateTime.Today;
                        //payment.VoidReason = "AUTO PAY DECLINED: " + acceptedInvoice.DeclineCode;
                    }

                    string paymentNo = autoNumberProvider.Generate(invoice.BranchID, "PM", DateTime.Today.Month, DateTime.Today.Year);

                    var h = new PaymentHeader();
                    h.Date = DateTime.Today;
                    h.InvoiceID = invoice.ID;
                    h.PaymentNo = paymentNo;
                    h.VoidDate = null;
                    EntityHelper.SetAuditFieldForInsert(h, principal.Identity.Name);

                    var d = new PaymentDetail();
                    d.CreditCardTypeID = cust.CreditCardTypeID;
                    d.PaymentTypeID = 4; // credit  card
                    d.Amount = invoice.InvoiceDetails.Sum(inv => (inv.Quantity * inv.UnitPrice) - (inv.Discount / 100 * (inv.Quantity * inv.UnitPrice)));
                    d.ApprovalCode = acceptedInvoice.VerificationCode;
                    h.PaymentDetails.Add(d);
                    context.Add(h);

                    autoNumberProvider.Increment("PM", invoice.BranchID, DateTime.Today.Year);

                    var status = new CustomerStatusHistory();
                    status.Customer = invoice.Customer;
                    status.Date = DateTime.Today;
                    status.CustomerStatusID = 1; // OK
                    status.Notes = "AUTO PAY ACCEPTED FOR INVOICE " + invoice.InvoiceNo;
                    status.StartDate = DateTime.Today;
                    EntityHelper.SetAuditFieldForInsert(status, principal.Identity.Name);
                }
            }

            context.SaveChanges();

            return acceptedInvoices.Count;
        }
 
    }
}
