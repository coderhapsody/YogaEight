using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Principal;
using GoGym.Data;
using GoGym.Providers.Base;
using GoGym.Providers.Configuration;
using GoGym.Providers.Constants;
using GoGym.Providers.ViewModels;

namespace GoGym.Providers
{
    public class CustomerProvider : BaseProvider
    {
        private ItemProvider itemProvider;

        public CustomerProvider(FitnessEntities context, IPrincipal principal, ItemProvider itemProvider) : base(context, principal)
        {
            this.itemProvider = itemProvider;
        }

        public void GetMinMaxCustomerJoinYear(out int minYear, out int maxYear)
        {
            minYear = context.Customers.Min(cust => cust.CreatedWhen).Year;
            maxYear = context.Customers.Max(cust => cust.CreatedWhen).Year;
        }

        public void AddCreditCardChangeHistory(int customerID, int creditCardTypeID, string creditCardNo, string creditCardHolderName, string creditCardIDNo, DateTime creditCardExpireDate)
        {
            var ccChangeHistory = new CreditCardChangeHistory();
            ccChangeHistory.CustomerID = customerID;
            ccChangeHistory.CreditCardTypeID = creditCardTypeID;
            ccChangeHistory.CreditCardNo = creditCardNo;
            ccChangeHistory.CreditCardHolderName = creditCardHolderName;
            ccChangeHistory.CreditCardExpiredDate = creditCardExpireDate;
            ccChangeHistory.CreditCardIDNo = creditCardIDNo;
            EntityHelper.SetAuditFieldForUpdate(ccChangeHistory, principal.Identity.Name);
            context.Add(ccChangeHistory);
            context.SaveChanges();
        }

        public void DeleteCreditCardChangeHistory(int creditCardChangeHistoryID)
        {            
            context.Delete(
                context.CreditCardChangeHistories.Single(cc => cc.ID == creditCardChangeHistoryID));
            context.SaveChanges();
        }        

        public IEnumerable<Customer> GetCustomersByBillingType(int billingTypeID)
        {
            return context.Customers.Where(cust => cust.BillingTypeID == billingTypeID).ToList();
        }


        public void Update(
            int id,
            string barcode,
            string firstName,
            string lastName,
            string surname,
            DateTime dateOfBirth,
            string address,
            string zipCode,
            string idCardNo,
            int? occupationID,
            string mailingAddress,
            string mailingZipCode,
            string email,
            string phone,
            string cellphone,
            string cellphone2,
            string workphone,
            int areaID,
            int partnerID,
            int billingTypeID,
            string cardNo,
            int creditCardTypeID,
            string cardHolderName,
            string cardHolderID,
            int bankID,
            DateTime cardExpiredDate,
            bool deletePhoto,
            string photo)
        {
            Customer cust = context.Customers.Single(row => row.ID == id);
            cust.Barcode = barcode;
            cust.FirstName = firstName;
            cust.LastName = lastName;
            cust.Surname = surname;
            cust.DateOfBirth = dateOfBirth;
            cust.Address = address;
            cust.ZipCode = zipCode;
            cust.IDCardNo = idCardNo;
            cust.OccupationID = occupationID;
            cust.MailingAddress = mailingAddress;
            cust.MailingZipCode = mailingZipCode;
            cust.Email = email;
            cust.HomePhone = phone;
            cust.CellPhone1 = cellphone;
            cust.CellPhone2 = cellphone2;
            cust.WorkPhone = workphone;
            cust.AreaID = areaID == 0 ? (int?)null : areaID;
            cust.PartnerID = partnerID == 0 ? (int?)null : partnerID;
            cust.BillingTypeID = billingTypeID;
            if (billingTypeID != 1) // manual payment
            {
                cust.CardNo = cardNo;
                cust.CardHolderName = cardHolderName;
                cust.CardHolderID = cardHolderID;
                cust.CreditCardTypeID = creditCardTypeID;
                cust.BankID = bankID == 0 ? (int?)null : bankID;
                cust.ExpiredDate = cardExpiredDate;
            }
            else
            {
                cust.CardNo = null;
                cust.CardHolderName = null;
                cust.CardHolderID = null;
                cust.CreditCardTypeID = null;
                cust.BankID = bankID == 0 ? (int?)null : bankID;
                cust.ExpiredDate = null;
            }
            cust.Photo = deletePhoto ? null : photo;
            EntityHelper.SetAuditFieldForUpdate(cust, principal.Identity.Name);
            context.SaveChanges();
        }

        public void Delete(int[] customersID)
        {
            context.Delete(
                context.Customers.Where(row => customersID.Contains(row.ID)));
            context.SaveChanges();
        }

        public bool IsExist(string barcode)
        {
            return context.Customers.Count(cust => cust.Barcode == barcode) > 0;
        }

        public Customer Get(int id)
        {
            return context.Customers.SingleOrDefault(row => row.ID == id);
        }

        public Customer Get(string barcode)
        {
            return context.Customers.SingleOrDefault(row => row.Barcode == barcode);
        }

        public void UpdatePhoto(int id, string fileName)
        {
            Customer cust = context.Customers.Single(row => row.ID == id);
            cust.Photo = fileName;
            EntityHelper.SetAuditFieldForUpdate(cust, principal.Identity.Name);
            context.SaveChanges();
        }

        public void DeletePhoto(int id)
        {
            Customer cust = context.Customers.Single(row => row.ID == id);
            cust.Photo = null;
            EntityHelper.SetAuditFieldForUpdate(cust, principal.Identity.Name);
            context.SaveChanges();
        }

        public IEnumerable<CustomerCheckInViewModel> GetCheckInHistory(int branchID)
        {
            var query = context.CheckInLogs
                               .Where(ck => ck.CheckInWhen.Date == DateTime.Today)
                               .OrderByDescending(ck => ck.CheckInWhen)
                               .Take(10).ToList();
            return query.Select(checkInLog => new CustomerCheckInViewModel
                                              {
                                                  CustomerBarcode = checkInLog.Customer.Barcode,
                                                  CustomerName =
                                                      checkInLog.Customer.FirstName.Trim() + " " + checkInLog.Customer.LastName.Trim(),
                                                  When = checkInLog.CheckInWhen,
                                                  AllowCheckIn = checkInLog.Allowed,
                                                  Messages = checkInLog.Messages.Split('|').ToList(),
                                                  ContractNo = checkInLog.ContractID.HasValue ? checkInLog.Contract.ContractNo : String.Empty
                                              });
        }

        public CustomerCheckInViewModel DoCheckIn(int branchID, string customerBarcode, string userName, string path)
        {
            var checkInConfiguration = new CheckInConfiguration();

            var messages = new List<string>();
            var viewModel = new CustomerCheckInViewModel();
            viewModel.PickUpPersons = new List<string>();
            viewModel.PickUpPhotos = new List<string>();

            var customerStatusProvider = new CustomerStatusProvider(context, principal);

            Customer customer = context.Customers.SingleOrDefault(cust => cust.Barcode == customerBarcode);
            if (customer != null)
            {
                viewModel.CustomerBarcode = customerBarcode.ToUpper();
                viewModel.CustomerName = String.Format("{0} {1}", customer.FirstName.Trim().ToUpper(), customer.LastName.Trim().ToUpper());
                viewModel.When = DateTime.Now;
                viewModel.Photo = customer.Photo;
                viewModel.Age = customer.DateOfBirth.GetValueOrDefault().ToAgeString();
                viewModel.IsPhotoExist = File.Exists(path + customer.Photo);
                viewModel.AllowCheckIn = true;

                foreach (var person in customer.People.Where(p => p.Connection == 'P'))
                {
                    viewModel.PickUpPersons.Add(person.Name);
                    viewModel.PickUpPhotos.Add(person.Photo);
                }

                /* Get Messages */

                var contractProvider = new ContractProvider(context, principal);
                var activeContract = contractProvider.GetActiveContracts(customerBarcode).FirstOrDefault(contract => contract.EffectiveDate <= DateTime.Today);
                if (activeContract != null)
                {
                    if (!activeContract.PackageHeader.OpenEnd)
                    {
                        if (activeContract.ExpiredDate < DateTime.Today)
                            messages.Add("CONTRACT " + activeContract.ContractNo + " EXPIRED");
                        else if (activeContract.ExpiredDate.Subtract(DateTime.Today) <= TimeSpan.FromDays(30))
                            messages.Add("CONTRACT " + activeContract.ContractNo + " EXPIRING");
                    }
                    viewModel.PackageName = activeContract.PackageHeader.Name;
                }
                else
                {
                    messages.Add("UNAPPROVED CONTRACT");
                }

                if (checkInConfiguration.ContractNotActiveAlert)
                {
                    var inactiveContracts = customer.Contracts.Where(contract => !contract.ActiveDate.HasValue && !contract.VoidDate.HasValue && contract.EffectiveDate <= DateTime.Today).ToList();
                    if (inactiveContracts.Any())
                        messages.Add("CONTRACT NOT ACTIVE: " + String.Join(", ", inactiveContracts.Select(contract => contract.ContractNo).ToArray()));
                }

                if (checkInConfiguration.ContractNotPaid)
                {
                    var unpaidContracts = customer.Contracts.Where(contract => !contract.PurchaseDate.HasValue && !contract.VoidDate.HasValue && contract.EffectiveDate <= DateTime.Today).ToList();
                    if (unpaidContracts.Any())
                        messages.Add("CONTRACT NOT PAID: " + String.Join(", ", unpaidContracts.Select(contract => contract.ContractNo).ToArray()));
                }

                if (checkInConfiguration.BirthdayAlert)
                {
                    bool isBirthDay = customer.DateOfBirth.GetValueOrDefault().Month == DateTime.Today.Month &&
                                      customer.DateOfBirth.GetValueOrDefault().Day == DateTime.Today.Day;
                    if (isBirthDay)
                        messages.Add("HAPPY BIRTHDAY");
                }

                if (customer.BillingType.ID > BillingTypeConstants.MANUAL_PAYMENT)
                {
                    // alert for credit card is valid only for non-manual payment
                    if (DateTime.Today >= customer.ExpiredDate.GetValueOrDefault() && checkInConfiguration.CreditCardExpired)
                        messages.Add("CREDIT CARD IS EXPIRED");
                    else
                    {
                        if (checkInConfiguration.CreditCardExpiringAlert)
                        {
                            bool isCreditCardExpiring = customer.ExpiredDate.GetValueOrDefault().Subtract(DateTime.Today) <= TimeSpan.FromDays(30);
                            if (isCreditCardExpiring)
                                messages.Add("CREDIT CARD IS EXPIRING");
                        }
                    }
                }

                CustomerStatusHistory customerStatusHistory = customerStatusProvider.GetLatestStatus(customerBarcode);
                viewModel.CustomerStatus = customerStatusHistory == null ? "OK" : customerStatusHistory.CustomerStatus.Description;
                string color = customerStatusProvider.GetStatusColor(viewModel.CustomerStatus);
                viewModel.CustomerStatusColor = color.Split('|')[0];
                viewModel.CustomerStatusBackgroundColor = color.Split('|')[1];

                messages.AddRange(customer.CustomerNotes.Where(note => note.Priority == 1).Select(note => note.Notes));

                viewModel.Messages = messages;


                /* Save checkin history */
                var checkinlog = new CheckInLog();
                checkinlog.BranchID = branchID;
                checkinlog.CustomerID = customer.ID;

                checkinlog.CustomerStatusID = customerStatusHistory == null ? 1 : customerStatusHistory.CustomerStatusID;
                checkinlog.Employee = context.Employees.SingleOrDefault(emp => emp.UserName == userName);
                checkinlog.CheckInWhen = viewModel.When.Value;
                checkinlog.Messages = String.Join("|", messages.ToArray());
                checkinlog.Allowed = viewModel.AllowCheckIn;
                context.Add(checkinlog);
                context.SaveChanges();
            }
            else
            {
                viewModel.AllowCheckIn = false;
                messages.Add("INVALID CUSTOMER BARCODE");
                viewModel.Messages = messages;
            }

            return viewModel;
        }

        public void UpdateCreditCardInfo(string barcode, int creditCardTypeID, int bankID, string cardHolderName, string cardHolderIDNo, string creditCardNo, DateTime expiredDate, string reason)
        {
            Customer customer = context.Customers.SingleOrDefault(c => c.Barcode == barcode && c.BillingTypeID != 1);
            if (customer != null)
            {
// ReSharper disable once InconsistentNaming
                CreditCardChangeHistory lastCC = context.CreditCardChangeHistories.Where(cch => cch.CustomerID == customer.ID).OrderByDescending(cch => cch.ChangedWhen).Take(1).SingleOrDefault();
                if (lastCC != null)
                {
                    if (lastCC.CreditCardTypeID == creditCardTypeID &&
                       lastCC.BankID == bankID &&
                       lastCC.CreditCardHolderName == cardHolderName &&
                       lastCC.CreditCardIDNo == cardHolderIDNo &&
                       lastCC.CreditCardExpiredDate == expiredDate &&
                       lastCC.CreditCardNo == creditCardNo)
                    {
                        throw new Exception("Cannot found any change information since last information of credit card was saved.");
                    }
                }

                var cc = new CreditCardChangeHistory();
                cc.Customer = customer;
                cc.CreditCardTypeID = creditCardTypeID;
                cc.BankID = bankID; 
                cc.CreditCardHolderName = cardHolderName;
                cc.CreditCardIDNo = cardHolderIDNo;
                cc.CreditCardNo = creditCardNo;
                cc.CreditCardExpiredDate = expiredDate;
                cc.Reason = reason;
                cc.ChangedWhen = DateTime.Now;
                cc.ChangedWho = principal.Identity.Name;
                context.Add(cc);

                customer.BankID = bankID;
                customer.CreditCardTypeID = creditCardTypeID;
                customer.CardHolderID = cardHolderIDNo;
                customer.CardHolderName = cardHolderName;
                customer.CardNo = creditCardNo;
                customer.ExpiredDate = expiredDate;

                context.SaveChanges();
            }
        }

        public bool IsBillingTypeAutoPayment(string custBarcode)
        {
            Customer customer = context.Customers.SingleOrDefault(c => c.Barcode == custBarcode);
            if (customer != null)
            {
                return customer.BillingTypeID == 3;
            }

            return false;
        }

        public CustomerCheckInViewModel DoCheckInByContract(int branchID, string contractNo, string userName)
        {
            var messages = new List<string>();
            var model = new CustomerCheckInViewModel();
            var checkInConfiguration = new CheckInConfiguration();
            Contract contract = context.Contracts.FirstOrDefault(con => con.ContractNo == contractNo);
            var customerStatusProvider = new CustomerStatusProvider(context, principal);
            if (contract != null)
            {
                var customer = contract.Customer;
                var package = contract.PackageHeader;

                model.CustomerBarcode = customer.Barcode;
                model.CustomerName = String.Format("{0} {1}", customer.FirstName, customer.LastName);
                model.PackageName = package.Name;
                model.ContractNo = contractNo;
                model.ExpiredDate = contract.ExpiredDate.ToString("dddd, dd MMMM yyyy");
                model.MemberSince = customer.CreatedWhen.ToString("dddd, dd MMMM yyyy");
                model.Photo = customer.Photo;
                model.IsPhotoExist = !String.IsNullOrEmpty(model.Photo);
                model.Age = customer.DateOfBirth.GetValueOrDefault().ToAgeString();

                if (contract.ExpiredDate <= DateTime.Today)
                {
                    messages.Add("CONTRACT IS EXPIRED");
                }

                if (contract.ActiveDate.HasValue)
                {
                    messages.Add(String.Format("Contract has been activated since {0}",
                        contract.ActiveDate.Value.ToString("dddd, dd MMMM yyyy")));
                }
                else
                {
                    messages.Add("CONTRACT IS NOT ACTIVE");
                }

                CustomerStatusHistory customerStatusHistory = customerStatusProvider.GetLatestStatus(customer.Barcode);
                model.CustomerStatus = customerStatusHistory == null
                    ? "OK"
                    : customerStatusHistory.CustomerStatus.Description;
                if (checkInConfiguration.BirthdayAlert)
                {
                    bool isBirthDay = customer.DateOfBirth.GetValueOrDefault().Month == DateTime.Today.Month &&
                                      customer.DateOfBirth.GetValueOrDefault().Day == DateTime.Today.Day;
                    if (isBirthDay)
                        messages.Add("HAPPY BIRTHDAY");
                }
                model.Messages = messages;

                /* Save checkin history */
                var checkinlog = new CheckInLog();
                checkinlog.BranchID = branchID;
                checkinlog.CustomerID = customer.ID;

                checkinlog.CustomerStatusID = customerStatusHistory == null ? 1 : customerStatusHistory.CustomerStatusID;
                checkinlog.Employee = context.Employees.SingleOrDefault(emp => emp.UserName == userName);
                checkinlog.CheckInWhen = DateTime.Now;
                checkinlog.Messages = String.Join("|", messages.ToArray());
                checkinlog.Allowed = model.AllowCheckIn;
                checkinlog.ContractID = contract.ID;
                context.Add(checkinlog);
                context.SaveChanges();
            }
            else
            {
                model.AllowCheckIn = false;
                messages.Add("INVALID CONTRACT NUMBER");
                model.Messages = messages;
            }

            return model;
        }

        #region Potong sesi - OLD

        //public short GetBalanceTrainingSession(string customerBarcode,                                               
        //                                       int itemID,
        //                                       DateTime date)
        //{
        //    var customer = context.Customers.SingleOrDefault(cust => cust.Barcode == customerBarcode);
        //    if (customer != null)
        //    {
        //        var query = (from trn in context.TrainingSessions
        //                     where trn.IsActive
        //                           && trn.CustomerID == customer.ID                                   
        //                           && trn.ItemID == itemID
        //                           && trn.BalanceDate <= date
        //                     select trn.Balance).Sum(trn => trn);

        //        return Convert.ToInt16(query);
        //    }

        //    return 0;
        //}

        //private IEnumerable<Tuple<int, int>> GetBalanceTrainingSession(string customerBarcode,
        //                                                                       DateTime date)
        //{
        //    var customer = context.Customers.SingleOrDefault(cust => cust.Barcode == customerBarcode);
        //    if (customer != null)
        //    {
        //        var query = (from trn in context.TrainingSessions
        //                     where trn.IsActive
        //                           && trn.CustomerID == customer.ID
        //                           && trn.BalanceDate <= date
        //                     group trn by trn.ItemID 
        //                     into g
        //                     select new
        //                            {                         
        //                                ItemID = g.Key, // item1
        //                                Balance = g.Sum(sess => sess.Balance) // item2
        //                            }).ToList();


        //        return query.Select(item => new Tuple<int, int>(item.ItemID, item.Balance)).ToList();
        //    }

        //    return null;
        //}

        public TrainingSessionInfoViewModel QueryTrainingSession(string customerBarcode, DateTime date)
        {
            var model = new TrainingSessionInfoViewModel();
            var customer = context.Customers.SingleOrDefault(cust => cust.Barcode == customerBarcode);

            if (customer != null)
            {
                model.CustomerBarcode = customerBarcode;
                model.CustomerName = String.Format("{0} {1}", customer.FirstName, customer.LastName);
                model.CustomerHomeBranchID = customer.HomeBranchID.GetValueOrDefault();
                model.Invoices = new List<TrainingSessionInvoiceViewModel>();

                //var trainingSessionBalances = (customerBarcode, date);

                var queryQuota = from quota in context.SessionQuotaHeaders
                                 join inv in context.InvoiceHeaders on quota.InvoiceID equals inv.ID
                                 where inv.CustomerID == customer.ID 
                                    && (quota.TotalQuota - quota.TotalUsage) > 0 
                                    && !inv.VoidDate.HasValue
                                 select quota;

                foreach (var trainingSession in queryQuota)
                {
                    var invoiceInfo = new TrainingSessionInvoiceViewModel();
                    invoiceInfo.QuotaID = trainingSession.ID;
                    invoiceInfo.Balance = (short) (trainingSession.TotalQuota - trainingSession.TotalUsage);
                    invoiceInfo.ItemID = trainingSession.ItemID;
                    var item = itemProvider.Get(invoiceInfo.ItemID);
                    invoiceInfo.ItemBarcode = item.Barcode;
                    invoiceInfo.ItemDescription = item.Description;

                    model.Invoices.Add(invoiceInfo);
                }
            }

            return model;
        }

        public void ProcessCutTrainingSession(int quotaID, int branchID, int trainerID, string customerBarcode, short qty, string notes)
        {            
            AdjustTrainingSession(quotaID, branchID, trainerID, customerBarcode, DateTime.Today, qty, notes, Convert.ToString(TrainingSessionConstants.TRAINING_SESSION_CUT));
        }

        public void AdjustTrainingSession(int quotaID, int branchID, int trainerID, string customerBarcode, DateTime date, short qty, string notes, string type = null)
        {
            var customer = context.Customers.SingleOrDefault(cust => cust.Barcode == customerBarcode);
            var employee = context.Employees.SingleOrDefault(emp => emp.UserName == principal.Identity.Name);
            var quotaHeader = context.SessionQuotaHeaders.SingleOrDefault(quota => quota.ID == quotaID);
            type = type ?? Convert.ToString(TrainingSessionConstants.TRAINING_SESSION_ADJUSTMENT);

            if (customer != null && employee != null && quotaHeader != null)
            {
                var quotaDetail = new SessionQuotaDetail();
                quotaDetail.Date = date;
                quotaDetail.When = DateTime.Now;
                quotaDetail.BranchID = branchID;
                quotaDetail.ClerkID = employee.ID;
                quotaDetail.TrainerID = trainerID;
                quotaDetail.Type = Convert.ToChar(type);
                quotaDetail.Usage = qty;
                quotaDetail.Notes = notes;                
                quotaDetail.SessionQuotaHeader = quotaHeader;

                if (Convert.ToChar(type) == TrainingSessionConstants.TRAINING_SESSION_CUT)
                    quotaHeader.TotalUsage += qty;
                else if(Convert.ToChar(type) == TrainingSessionConstants.TRAINING_SESSION_ADJUSTMENT)
                    quotaHeader.TotalUsage -= qty;

                quotaHeader.SessionQuotaDetails.Add(quotaDetail);
                context.SaveChanges();
            }

        }

        public SessionQuotaHeader GetQuotaInfo(int quotaID)
        {
            return context.SessionQuotaHeaders.SingleOrDefault(quota => quota.ID == quotaID);
        }
        #endregion
    }
}
