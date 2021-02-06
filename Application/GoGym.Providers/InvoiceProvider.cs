using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using GoGym.Data;
using GoGym.Providers.Base;
using GoGym.Providers.Constants;
using GoGym.Providers.ViewModels;

namespace GoGym.Providers
{
    public class InvoiceProvider : BaseProvider
    {
        public static readonly int ITEM_ACCOUNT_ADMINISTRATION_FEE = 18;

        private readonly AutoNumberProvider autoNumberProvider;
        private readonly PaymentProvider paymentProvider;

        public InvoiceProvider(FitnessEntities context, IPrincipal principal, AutoNumberProvider autoNumberProvider, PaymentProvider paymentProvider)
            : base(context, principal)
        {
            this.autoNumberProvider = autoNumberProvider;
            this.paymentProvider = paymentProvider;
        }

        public string CreateFreshMemberInvoice(
            int branchID,
            string contractNo,
            DateTime purchasedate,
            int employeeID,
            string notes,
            decimal discountValue,
            IEnumerable<PackageDetailViewModel> detail,
            IEnumerable<PaymentDetailViewModel> paymentDetail)
        {
            Contract contract = context.Contracts.SingleOrDefault(con => con.ContractNo == contractNo);            
            if (contract != null)
            {               
                var header = new InvoiceHeader();
                header.InvoiceNo = autoNumberProvider.Generate(branchID, "OR", purchasedate.Month, purchasedate.Year);
                header.BranchID = branchID;
                header.Contract = contract;
                header.Date = purchasedate;
                header.EmployeeID = employeeID;
                header.InvoiceType = InvoiceConstants.FRESH_MEMBER_INVOICE;
                header.Notes = notes;
                header.DiscountValue = discountValue;
                header.CustomerID = contract.CustomerID;
                header.VoidDate = null;
                EntityHelper.SetAuditFieldForInsert(header, principal.Identity.Name);
                foreach (var model in detail)
                {
                    var d = new InvoiceDetail();
                    d.ItemID = model.ItemID;
                    d.Quantity = model.Quantity;
                    d.UnitPrice = model.UnitPrice;
                    d.UnitName = model.UnitName;
                    d.Discount = model.Discount;
                    d.IsTaxable = model.IsTaxed;
                    d.InvoiceHeader = header;

                    CreateTrainingSession(branchID, model.ItemID, contract.CustomerID, header, d);

                    context.Add(d);
                }

                var pay = new PaymentHeader();
                pay.Date = purchasedate;
                pay.InvoiceHeader = header;
                pay.PaymentNo = autoNumberProvider.Generate(branchID, "PM", purchasedate.Month, purchasedate.Year);
                pay.VoidDate = null;
                EntityHelper.SetAuditFieldForInsert(pay, principal.Identity.Name);

                foreach (var payDetail in paymentDetail)
                {
                    var payd = new PaymentDetail();
                    payd.Amount = payDetail.Amount;
                    payd.CreditCardTypeID = payDetail.CreditCardTypeID;
                    payd.PaymentTypeID = payDetail.PaymentTypeID;
                    payd.ApprovalCode = payDetail.ApprovalCode;
                    payd.Notes = payDetail.Notes;
                    payd.PaymentHeader = pay;
                    context.Add(payd);
                }
                
                context.Add(header);                
                context.Add(pay);


                //CustomerStatusHistory custStatusHist = new CustomerStatusHistory();
                //custStatusHist.Customer = cust;
                //custStatusHist.CustomerStatusID =

                contract.PurchaseDate = purchasedate;
                //contract.ActiveDate = DateTime.Now;
                contract.Status = ContractStatus.PAID;

                autoNumberProvider.Increment("OR", header.BranchID, purchasedate.Year);
                autoNumberProvider.Increment("PM", header.BranchID, purchasedate.Year);



                context.SaveChanges();

                return header.InvoiceNo;
            }

            return null;
        }

        public string CreateExistingMemberInvoice(
            int branchID,
            DateTime date,
            string customerCode,
            int employeeID,
            string notes,
            decimal discountValue,
            IEnumerable<InvoiceDetailViewModel> detail,
            IEnumerable<PaymentDetailViewModel> paymentDetail)
        {
            Customer cust = context.Customers.SingleOrDefault(c => c.Barcode == customerCode);            

            var header = new InvoiceHeader();
            header.InvoiceNo = autoNumberProvider.Generate(branchID, "OR", date.Month, date.Year);
            header.BranchID = branchID;
            header.Date = date;
            if (cust == null)
            {
                header.Customer = null;
                header.CustomerName = customerCode;
            }
            else
            {
                header.Customer = cust;
                header.CustomerName = null;
            }

            header.EmployeeID = employeeID;
            header.InvoiceType = InvoiceConstants.EXISTING_MEMBER_INVOICE;
            header.Notes = notes;
            header.DiscountValue = discountValue;
            EntityHelper.SetAuditFieldForInsert(header, principal.Identity.Name);
            foreach (var model in detail)
            {
                var d = new InvoiceDetail();
                d.InvoiceID = model.InvoiceID;
                d.ItemID = model.ItemID;
                d.Quantity = model.Quantity;
                d.UnitName = model.UnitName;
                d.UnitPrice = model.UnitPrice;
                d.Discount = model.Discount;
                d.IsTaxable = model.IsTaxable;
                d.InvoiceHeader = header;
                context.Add(d);

                if(cust != null)
                    CreateTrainingSession(branchID, model.ItemID, cust.ID, header, d);
            }

            var payments = paymentDetail.ToList();
            if (payments.Count > 0)
            {
                var pay = new PaymentHeader();
                pay.Date = date;
                pay.InvoiceHeader = header;
                pay.PaymentNo = autoNumberProvider.Generate(branchID, "PM", date.Month, date.Year);
                pay.VoidDate = null;
                EntityHelper.SetAuditFieldForInsert(pay, principal.Identity.Name);

                foreach (var payDetail in payments)
                {
                    var payd = new PaymentDetail();
                    payd.Amount = payDetail.Amount;
                    payd.CreditCardTypeID = payDetail.CreditCardTypeID;
                    payd.PaymentTypeID = payDetail.PaymentTypeID;
                    payd.ApprovalCode = payDetail.ApprovalCode;
                    payd.Notes = payDetail.Notes;
                    payd.PaymentHeader = pay;
                    context.Add(payd);
                }
                autoNumberProvider.Increment("PM", header.BranchID, date.Year);

                context.Add(pay);
            }

            context.Add(header);

            autoNumberProvider.Increment("OR", header.BranchID, date.Year);

            context.SaveChanges();

            return header.InvoiceNo;
        }

        private void CreateTrainingSession(int branchID,
                                           int itemID,
                                           int customerID,
                                           InvoiceHeader header,
                                           InvoiceDetail d)
        {            
            var item = context.Items.SingleOrDefault(it => it.ID == itemID);
            if (item != null)
            {
                if (item.SessionBalance.GetValueOrDefault(0) > 0)
                {
                    var employee = context.Employees.SingleOrDefault(emp => emp.UserName == principal.Identity.Name);
                    if (employee != null)
                    {
                        var sessionQuota = new SessionQuotaHeader();

                        sessionQuota.InvoiceHeader = header;
                        sessionQuota.ItemID = item.ID;
                        sessionQuota.TotalQuota = item.SessionBalance.GetValueOrDefault();

                        var sessionQuotaDetail = new SessionQuotaDetail();
                        sessionQuotaDetail.BranchID = branchID;
                        sessionQuotaDetail.ClerkID = employee.ID;
                        sessionQuotaDetail.TrainerID = null;
                        sessionQuotaDetail.Type = TrainingSessionConstants.TRAINING_SESSION_PURCHASE;
                        sessionQuotaDetail.Usage = sessionQuota.TotalQuota;
                        sessionQuotaDetail.SessionQuotaHeader = sessionQuota;
                        sessionQuotaDetail.Notes = header.Notes;
                        
                        sessionQuota.SessionQuotaDetails.Add(sessionQuotaDetail);

                        header.SessionQuotaHeaders.Add(sessionQuota);                        
                    }
                }
            }
        }

        public InvoiceHeader CreateExistingMemberInvoiceForBilling(
            int branchID,
            DateTime date,
            string customerCode,
            int employeeID,
            string notes,
            decimal discountValue,
            IEnumerable<InvoiceDetailViewModel> detail,
            IEnumerable<PaymentDetailViewModel> paymentDetail)
        {
            Customer cust = context.Customers.SingleOrDefault(c => c.Barcode == customerCode);
            if (cust != null)
            {
                var header = new InvoiceHeader();
                header.InvoiceNo = autoNumberProvider.Generate(branchID, "OR", date.Month, date.Year);
                header.BranchID = branchID;
                header.Date = date;
                header.Customer = cust;
                header.EmployeeID = employeeID;
                header.InvoiceType = InvoiceConstants.EXISTING_MEMBER_INVOICE;
                header.Notes = notes;
                header.DiscountValue = discountValue;
                EntityHelper.SetAuditFieldForInsert(header, principal.Identity.Name);
                foreach (var model in detail)
                {
                    var d = new InvoiceDetail();
                    d.InvoiceID = model.InvoiceID;
                    d.ItemID = model.ItemID;
                    d.Quantity = model.Quantity;
                    d.UnitPrice = model.UnitPrice;
                    d.UnitName = model.UnitName;
                    d.Discount = model.Discount;
                    d.IsTaxable = model.IsTaxable;
                    d.InvoiceHeader = header;
                    context.Add(d);
                }

                var pay = new PaymentHeader();
                pay.Date = date;
                pay.InvoiceHeader = header;
                pay.PaymentNo = autoNumberProvider.Generate(branchID, "PM", date.Month, date.Year);
                pay.VoidDate = null;
                EntityHelper.SetAuditFieldForInsert(pay, principal.Identity.Name);

                foreach (var payDetail in paymentDetail)
                {
                    var payd = new PaymentDetail();
                    payd.Amount = payDetail.Amount;
                    payd.CreditCardTypeID = payDetail.CreditCardTypeID;
                    payd.PaymentTypeID = payDetail.PaymentTypeID;
                    payd.ApprovalCode = payDetail.ApprovalCode;
                    payd.Notes = payDetail.Notes;
                    payd.PaymentHeader = pay;
                    context.Add(payd);
                }

                context.Add(header);
                context.Add(pay);

                autoNumberProvider.Increment("OR", header.BranchID, date.Year);
                autoNumberProvider.Increment("PM", header.BranchID, date.Year);

                return header;
            }

            return null;
        }

        public void ProcessVoid(string invoiceNo, string reason, bool voidPaymentOnly)
        {
            var invoice = context.InvoiceHeaders.SingleOrDefault(inv => inv.InvoiceNo == invoiceNo);
            if (invoice != null)
            {
                if (!voidPaymentOnly)
                {
                    invoice.VoidDate = DateTime.Now;
                    invoice.VoidReason = reason;

                    if (invoice.InvoiceType == InvoiceConstants.FRESH_MEMBER_INVOICE)
                    {
                        invoice.Contract.Status = ContractStatus.UNPAID;
                        invoice.Contract.ActiveDate = null;
                    }

                    EntityHelper.SetAuditFieldForUpdate(invoice, principal.Identity.Name);
                }

                var payment = invoice.PaymentHeaders.FirstOrDefault(p => p.InvoiceID == invoice.ID && !p.VoidDate.HasValue);
                if (payment != null)
                {
                    payment.VoidDate = invoice.VoidDate;
                    payment.VoidReason = reason;
                    EntityHelper.SetAuditFieldForUpdate(payment, principal.Identity.Name);
                }

                context.SaveChanges();
            }
        }

        public InvoiceHeader GetInvoice(string invoiceNo)
        {
            return context.InvoiceHeaders.SingleOrDefault(inv => inv.InvoiceNo == invoiceNo);
        }

        public IList<InvoiceDetailViewModel> GetDetail(string invoiceNo)
        {
            var invoice = context.InvoiceHeaders.SingleOrDefault(inv => inv.InvoiceNo == invoiceNo);
            if (invoice != null)
            {
                var query = from d in context.InvoiceDetails
                            join i in context.Items on d.ItemID equals i.ID
                            where d.InvoiceID == invoice.ID
                            select new InvoiceDetailViewModel
                                   {
                                       ID = d.ID,
                                       InvoiceID = d.InvoiceID,
                                       ItemID = d.ItemID,
                                       ItemBarcode = i.Barcode,
                                       ItemDescription = i.Description,
                                       Quantity = d.Quantity,
                                       UnitName = d.UnitName,
                                       UnitPrice = d.UnitPrice,
                                       IsTaxable = d.IsTaxable,
                                       Discount = d.Discount,
                                       NetAmount =
                                           ((d.Quantity * d.UnitPrice) - ((d.Quantity * d.UnitPrice) * d.Discount / 100)) /
                                           (d.IsTaxable ? 1.1M : 1M),
                                       Total = d.Quantity * d.UnitPrice - (d.Quantity * d.UnitPrice) * d.Discount / 100
                                   };
                return query.ToList();
            }

            return null;
        }

        public string UpdateInvoiceAndPayment(string invoiceNo, DateTime invoiceDate, DateTime date, List<InvoiceDetailViewModel> invoiceDetail, List<PaymentDetailViewModel> paymentDetail)
        {
            //PaymentProvider payment = new PaymentProvider(context);
            string paymentNo = String.Empty;
            InvoiceHeader invoice = context.InvoiceHeaders.SingleOrDefault(inv => inv.InvoiceNo == invoiceNo);
            if (invoice != null)
            {

                invoice.Date = invoiceDate;
                
                context.Delete(invoice.InvoiceDetails.AsEnumerable());
                foreach (var model in invoiceDetail)
                {
                    var d = new InvoiceDetail();
                    d.InvoiceID = invoice.ID;
                    d.ItemID = model.ItemID;
                    d.Quantity = model.Quantity;
                    d.UnitPrice = model.UnitPrice;
                    d.Discount = model.Discount;
                    d.IsTaxable = model.IsTaxable;
                    d.InvoiceHeader = invoice;
                    context.Add(d);

                    if(invoice.CustomerID.HasValue)
                        CreateTrainingSession(invoice.BranchID, d.ItemID, invoice.CustomerID.Value, invoice, d);
                }
                paymentNo = paymentProvider.Create(date, invoiceNo, paymentDetail);
                context.SaveChanges();
            }

            return paymentNo;
        }

        private decimal CalculateTotalInvoice(InvoiceHeader invoiceHeader)
        {
            decimal totalDetail =
                invoiceHeader.InvoiceDetails.Sum(
                    detail =>
                        (detail.Quantity * detail.UnitPrice) -
                        (detail.Discount / 100 * (detail.Quantity * detail.UnitPrice)));
            decimal total = totalDetail - invoiceHeader.DiscountValue;
            return total;
        }

        public int ProcessFirstAccrualInvoices(int branchID, int[] invoiceIDs, DateTime processDate)
        {
            int processedInvoices = 0;
            foreach (var invoiceID in invoiceIDs)
            {
                var invoiceHeader = context.InvoiceHeaders.SingleOrDefault(inv => inv.ID == invoiceID);
                if (invoiceHeader != null)
                {
                    // invoice
                    int duesInMonth = invoiceHeader.Contract.DuesInMonth;
                    var iah = new InvoiceAccrualHeader();
                    iah.InvoiceID = invoiceHeader.ID;
                    iah.InvoiceNo = autoNumberProvider.Generate(branchID, "OR", processDate.Month, processDate.Year);
                    autoNumberProvider.Increment("OR", branchID, processDate.Year);
                    iah.TotalAmount = CalculateTotalInvoice(invoiceHeader);
                    iah.AccrualDate = processDate;
                    iah.AccrualAmount = iah.TotalAmount / duesInMonth;
                    iah.CreatedWhen = DateTime.Now;
                    iah.CreatedWho = principal.Identity.Name;
                    iah.SumAccrualPeriod = 1;
                    iah.TotalAccrualPeriod = duesInMonth;

                    var details = invoiceHeader.InvoiceDetails;
                    var total = 0M;
                    foreach (var detail in details)
                    {
                        var iad = new InvoiceAccrualDetail
                        {
                            Discount =
                                detail.Discount / (detail.Item.ItemAccountID == ITEM_ACCOUNT_ADMINISTRATION_FEE
                                    ? 1
                                    : duesInMonth),
                            IsTaxable = detail.IsTaxable,
                            ItemID = detail.ItemID,
                            Quantity = detail.Quantity,
                            UnitPrice =
                                detail.UnitPrice / (detail.Item.ItemAccountID == ITEM_ACCOUNT_ADMINISTRATION_FEE
                                    ? 1
                                    : duesInMonth)
                        };
                        total += iad.UnitPrice * iad.Quantity - (iad.Discount / 100 * iad.UnitPrice * iad.Quantity) -
                                 (invoiceHeader.DiscountValue / duesInMonth);
                        iad.InvoiceAccrualHeader = iah;
                        context.Add(iad);
                    }
                    iah.AccrualAmount = total;
                    iah.SumAccrualAmount = iah.AccrualAmount;
                    context.Add(iah);

                    // payment
                    var paymentHeader = invoiceHeader.PaymentHeaders.FirstOrDefault(pay => !pay.VoidDate.HasValue);
                    if (paymentHeader != null)
                    {
                        var payAccHeader = new PaymentAccrualHeader();
                        payAccHeader.InvoiceAccrualHeader = iah;
                        payAccHeader.PaymentNo = autoNumberProvider.Generate(branchID,
                            "PM",
                            processDate.Month,
                            processDate.Year);
                        autoNumberProvider.Increment("PM", branchID, processDate.Year);
                        payAccHeader.Date = processDate;
                        payAccHeader.CreatedWhen = DateTime.Now;
                        payAccHeader.CreatedWho = principal.Identity.Name;

                        var paymentDetails = paymentHeader.PaymentDetails.ToList();
                        foreach (var paymentDetail in paymentDetails)
                        {
                            var payAccDetail = new PaymentAccrualDetail();
                            payAccDetail.Amount = total / paymentDetails.Count;
                            payAccDetail.ApprovalCode = paymentDetail.ApprovalCode;
                            payAccDetail.CreditCardTypeID = paymentDetail.CreditCardTypeID;
                            payAccDetail.Notes = paymentDetail.Notes;
                            payAccDetail.PaymentTypeID = paymentDetail.PaymentTypeID;
                            payAccDetail.PaymentAccrualHeader = payAccHeader;
                            context.Add(payAccDetail);
                        }
                        context.Add(payAccHeader);
                    }

                    processedInvoices++;
                }
            }
            context.SaveChanges();
            return processedInvoices;

        }

        public int ProcessAccrualInvoices(int branchID, int[] accrualInvoiceIDs, DateTime processDate)
        {
            int processedInvoices = 0;

            foreach (var accrualInvoiceID in accrualInvoiceIDs)
            {
                var iah = context.InvoiceAccrualHeaders.SingleOrDefault(inv => inv.ID == accrualInvoiceID);
                var invoiceHeader = context.InvoiceHeaders.SingleOrDefault(inv => inv.ID == iah.InvoiceID);

                if (invoiceHeader != null && iah != null && iah.TotalAccrualPeriod > iah.SumAccrualPeriod)
                {
                    // invoice
                    int duesInMonth = iah.TotalAccrualPeriod;
                    var invoiceDetails = invoiceHeader.InvoiceDetails;
                    var nextInvoiceAccrual = new InvoiceAccrualHeader();
                    nextInvoiceAccrual.InvoiceID = iah.InvoiceID;
                    nextInvoiceAccrual.InvoiceNo = autoNumberProvider.Generate(branchID, "OR", processDate.Month, processDate.Year);
                    autoNumberProvider.Increment("OR", branchID, processDate.Year);

                    nextInvoiceAccrual.AccrualDate = processDate;
                    nextInvoiceAccrual.TotalAmount = iah.TotalAmount;
                    nextInvoiceAccrual.TotalAccrualPeriod = iah.TotalAccrualPeriod;
                    //nextInvoiceAccrual.AccrualAmount = iah.AccrualAmount;
                    nextInvoiceAccrual.SumAccrualPeriod = iah.SumAccrualPeriod + 1;
                    //nextInvoiceAccrual.SumAccrualAmount = iah.SumAccrualAmount + iah.AccrualAmount;
                    nextInvoiceAccrual.CreatedWhen = DateTime.Now;
                    nextInvoiceAccrual.CreatedWho = principal.Identity.Name;


                    var total = 0M;
                    foreach (var detail in invoiceDetails)
                    {
                        if (detail.Item.ItemAccountID != ITEM_ACCOUNT_ADMINISTRATION_FEE)
                        {
                            var iad = new InvoiceAccrualDetail
                            {
                                Discount = detail.Discount / duesInMonth,
                                IsTaxable = detail.IsTaxable,
                                ItemID = detail.ItemID,
                                Quantity = detail.Quantity,
                                UnitPrice = detail.UnitPrice / duesInMonth
                            };
                            total += iad.UnitPrice * iad.Quantity - (iad.Discount / 100 * iad.UnitPrice * iad.Quantity) -
                                     (invoiceHeader.DiscountValue / duesInMonth);
                            iad.InvoiceAccrualHeader = iah;
                            context.Add(iad);
                        }
                    }
                    nextInvoiceAccrual.AccrualAmount = total;
                    nextInvoiceAccrual.SumAccrualAmount = iah.SumAccrualAmount + total;
                    
                    context.Add(nextInvoiceAccrual);


                    // payment
                    var paymentHeader = invoiceHeader.PaymentHeaders.FirstOrDefault(pay => !pay.VoidDate.HasValue);
                    if (paymentHeader != null)
                    {
                        var payAccHeader = new PaymentAccrualHeader();
                        payAccHeader.InvoiceAccrualHeader = nextInvoiceAccrual;
                        payAccHeader.PaymentNo = autoNumberProvider.Generate(branchID,
                            "PM",
                            processDate.Month,
                            processDate.Year);
                        autoNumberProvider.Increment("PM", branchID, processDate.Year);
                        payAccHeader.Date = processDate;
                        payAccHeader.CreatedWhen = DateTime.Now;
                        payAccHeader.CreatedWho = principal.Identity.Name;

                        var paymentDetails = paymentHeader.PaymentDetails.ToList();
                        foreach (var paymentDetail in paymentDetails)
                        {
                            var payAccDetail = new PaymentAccrualDetail();
                            payAccDetail.Amount = total / paymentDetails.Count;
                            payAccDetail.ApprovalCode = paymentDetail.ApprovalCode;
                            payAccDetail.CreditCardTypeID = paymentDetail.CreditCardTypeID;
                            payAccDetail.Notes = paymentDetail.Notes;
                            payAccDetail.PaymentTypeID = paymentDetail.PaymentTypeID;

                            payAccDetail.PaymentAccrualHeader = payAccHeader;
                            context.Add(payAccDetail);
                        }
                        context.Add(payAccHeader);
                    }

                    processedInvoices++;
                }
            }
            context.SaveChanges();

            return processedInvoices;
        }
    }
}
