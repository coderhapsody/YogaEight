using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using GoGym.Data;
using GoGym.Providers.Base;

namespace GoGym.Providers
{
    public static class ContractStatus
    {
        public static readonly string UNPAID = "P";
        public static readonly string VOID = "V";
        public static readonly string PAID = "A";
        public static readonly string CLOSED = "C";
    }


    public class ContractProvider : BaseProvider
    {
        public ContractProvider(FitnessEntities context, IPrincipal principal) : base(context, principal)
        {            
        }

        public void Add(
            DateTime date,
            bool useExistingBarcode,
            string barcode,
            bool isTransfer,
            string firstName,
            string LastName,
            DateTime dateOfBirth,
            string identityCardNo,
            int? occupationID,
            int branchID,
            int packageID,
            DateTime? purchaseDate,
            DateTime effectiveDate,
            int billingTypeID,
            int billingCardTypeID,
            int billingBankID,
            string billingCardNo,
            string billingCardHolderName,
            string billingCardHolderID,
            DateTime cardExpiredDate,
            char status,
            int billingItemID,
            decimal duesAmount,
            decimal additionalDuesAmount,
            DateTime? nextDuesDate,
            DateTime expiredDate,
            string homePhone,
            string cellphone,
            string workphone,
            string mailingAddress,
            string zipCodeMailingAddress,
            string address,
            string zipCode,
            int areaID,
            bool fatherIsExist,
            string fatherName,
            string fatherIDCardNo,
            DateTime? fatherBirthDate,
            string fatherCellPhone,
            string fatherEmail,
            bool motherIsExist,
            string motherName,
            string motherIDCardNo,
            DateTime? motherBirthDate,
            string motherCellPhone,
            string motherEmail,
            string notes,
            string contractType)
        {
            PackageHeader package = context.PackageHeaders.SingleOrDefault(pkg => pkg.ID == packageID);
            if (package != null)
            {
                //nextDuesDate = effectiveDate.AddMonths(package.PackageDuesInMonth);
                var autoNumberProvider = new AutoNumberProvider(context, principal);
                Customer cust = null;
                if (useExistingBarcode)
                {
                    cust = context.Customers.Single(c => c.Barcode == barcode);
                }
                else
                {
                    cust = new Customer();
                    cust.Barcode = autoNumberProvider.Generate(branchID, "CU", 0, 0);
                    cust.FirstName = firstName;
                    cust.LastName = LastName;
                    cust.DateOfBirth = dateOfBirth;
                    cust.IDCardNo = identityCardNo;
                    cust.OccupationID = occupationID;
                    cust.HomeBranchID = branchID;
                    cust.HomePhone = homePhone;
                    cust.CellPhone1 = cellphone;
                    cust.WorkPhone = workphone;
                    cust.BillingTypeID = billingTypeID;
                    cust.AreaID = areaID == 0 ? (int?)null : areaID;
                    cust.MailingAddress = mailingAddress;
                    cust.MailingZipCode = zipCodeMailingAddress;
                    cust.Address = address;
                    cust.ZipCode = zipCode;
                    if (billingTypeID != 1) // auto payment
                    {
                        cust.BankID = billingBankID;
                        cust.CreditCardTypeID = billingCardTypeID;
                        cust.CardHolderName = billingCardHolderName;
                        cust.CardHolderID = billingCardHolderID;
                        cust.CardNo = billingCardNo;
                        cust.ExpiredDate = cardExpiredDate;
                    }
                    else
                    {
                        cust.BankID = (int?)null;
                        cust.CreditCardTypeID = (int?)null;
                        cust.CardHolderName = null;
                        cust.CardHolderID = null;
                        cust.CardNo = null;
                        cust.ExpiredDate = (DateTime?)null;
                    }
                    EntityHelper.SetAuditFieldForInsert(cust, principal.Identity.Name);
                    context.Add(cust);

                    autoNumberProvider.Increment("CU", branchID, 0);
                }

                var contract = new Contract();
                contract.ContractNo = autoNumberProvider.Generate(branchID, "CO", date.Month, date.Year);
                contract.Date = date;
                contract.Customer = cust;
                contract.BranchID = branchID;
                contract.PackageHeader = package;
                //contract.PurchaseDate = purchaseDate;
                contract.VoidDate = null;
                contract.EffectiveDate = effectiveDate;
                contract.BillingItemID = billingItemID == 0 ? (int?)null : billingItemID;
                contract.DuesAmount = duesAmount;
                contract.AdditionalDuesAmount = additionalDuesAmount;
                contract.NextDuesDate = nextDuesDate.GetValueOrDefault();
                //contract.ExpiredDate = expiredDate;                    
                contract.ExpiredDate = effectiveDate.AddMonths(package.PackageDuesInMonth);
                contract.BillingTypeID = billingTypeID;
                contract.Status = status.ToString();
                contract.Notes = notes;
                contract.DuesInMonth = package.PackageDuesInMonth;
                if(isTransfer)
                    contract.ContractType = 'T';
                else
                    contract.ContractType = useExistingBarcode ? Convert.ToChar(contractType) : (char?)null;
                EntityHelper.SetAuditFieldForInsert(contract, principal.Identity.Name);
                context.Add(contract);
                

                if (!useExistingBarcode)
                {
                    if (fatherIsExist)
                    {
                        Person person = new Person();
                        person.Connection = 'F';
                        person.Name = fatherName;
                        person.IDCardNo = fatherIDCardNo;
                        person.Phone1 = fatherCellPhone;
                        person.BirthDate = fatherBirthDate;
                        person.Email = fatherEmail;
                        person.Customer = cust;
                        EntityHelper.SetAuditFieldForInsert(person, principal.Identity.Name);
                        context.Add(person);
                    }

                    if (motherIsExist)
                    {
                        Person person = new Person();
                        person.Connection = 'M';
                        person.Name = motherName;
                        person.IDCardNo = motherIDCardNo;
                        person.Phone1 = motherCellPhone;
                        person.BirthDate = motherBirthDate;
                        person.Email = motherEmail;
                        person.Customer = cust;
                        EntityHelper.SetAuditFieldForInsert(person, principal.Identity.Name);
                        context.Add(person);
                    }
                }               

                autoNumberProvider.Increment("CO", branchID, date.Year);
                context.SaveChanges();

                //contract.VoidDate = null;
                //context.SaveChanges();
            }


        }

        public void Update(
            int id,
            int packageID,
            DateTime date,
            DateTime effectiveDate,
            string identityCardNo,
            int? occupationID,
            int billingTypeID,
            int billingCardTypeID,
            int billingBankID,
            string billingCardNo,
            string billingCardHolderName,
            string billingCardHolderID,
            DateTime cardExpiredDate,
            string homePhone,
            string cellphone,
            string workphone,
            string mailingAddress,
            string zipCodeMailingAddress,
            string address,
            string zipCode,
            int areaID,
            int billingItemID,
            decimal duesAmount,
            decimal additionalDuesAmount,
            DateTime nextDuesDate,
            DateTime expiredDate,
            bool fatherIsExist,
            string fatherName,
            string fatherIDCardNo,
            DateTime? fatherBirthDate,
            string fatherCellPhone,
            string fatherEmail,
            bool motherIsExist,
            string motherName,
            string motherIDCardNo,
            DateTime? motherBirthDate,
            string motherCellPhone,
            string motherEmail,
            string notes)            
        {
            //DateTime nextDuesDate;
            PackageHeader package = context.PackageHeaders.SingleOrDefault(pkg => pkg.ID == packageID);
            if (package != null)
            {
                //nextDuesDate = effectiveDate.AddMonths(package.PackageDuesInMonth);

                Contract contract = context.Contracts.Single(row => row.ID == id);
                contract.PackageID = packageID;
                contract.Date = date;
                contract.EffectiveDate = effectiveDate;
                contract.BillingTypeID = billingTypeID;
                contract.BillingItemID = billingItemID == 0 ? (int?)null : billingItemID;
                contract.NextDuesDate = nextDuesDate;
                contract.ExpiredDate = expiredDate; //effectiveDate.AddMonths(package.PackageDuesInMonth);
                contract.Notes = notes;
                contract.DuesAmount = duesAmount;
                contract.AdditionalDuesAmount = additionalDuesAmount;
                contract.DuesInMonth = package.PackageDuesInMonth;

                Customer customer = contract.Customer;
                customer.HomePhone = homePhone;
                customer.CellPhone1 = cellphone;
                customer.WorkPhone = workphone;
                customer.IDCardNo = identityCardNo;
                customer.OccupationID = occupationID;
                customer.MailingAddress = mailingAddress;
                customer.MailingZipCode = zipCodeMailingAddress;
                customer.BillingTypeID = billingTypeID;
                customer.Address = address;
                customer.ZipCode = zipCode;
                customer.AreaID = areaID == 0 ? (int?)null : areaID;
                if (billingTypeID != 1) // manual payment
                {
                    customer.BankID = billingBankID;
                    customer.CreditCardTypeID = billingCardTypeID;
                    customer.CardHolderName = billingCardHolderName;
                    customer.CardHolderID = billingCardHolderID;
                    customer.CardNo = billingCardNo;
                    customer.ExpiredDate = cardExpiredDate;
                }
                else
                {
                    customer.BankID = (int?)null;
                    customer.CreditCardTypeID = (int?)null;
                    customer.CardHolderName = null;
                    customer.CardHolderID = null;
                    customer.CardNo = null;
                    customer.ExpiredDate = (DateTime?)null;
                }
                EntityHelper.SetAuditFieldForUpdate(customer, principal.Identity.Name);

                if (fatherIsExist)
                {
                    Person person = context.People.Any(p => p.Connection == 'F' && p.CustomerID == contract.CustomerID) ? context.People.Single(p => p.Connection == 'F' && p.CustomerID == contract.CustomerID) : new Person();
                    person.Connection = 'F';
                    person.Name = fatherName;
                    person.IDCardNo = fatherIDCardNo;
                    person.Phone1 = fatherCellPhone;
                    person.BirthDate = fatherBirthDate;
                    person.Email = fatherEmail;
                    if (person.ID == 0)
                    {
                        person.Customer = customer;
                        EntityHelper.SetAuditFieldForInsert(person, principal.Identity.Name);
                        context.Add(person);
                    }
                    else
                        EntityHelper.SetAuditFieldForUpdate(person, principal.Identity.Name);

                }

                if (motherIsExist)
                {
                    Person person = context.People.Any(p => p.Connection == 'M' && p.CustomerID == contract.CustomerID) ? context.People.Single(p => p.Connection == 'M' && p.CustomerID == contract.CustomerID) : new Person();
                    person.Connection = 'M';
                    person.Name = motherName;
                    person.IDCardNo = motherIDCardNo;
                    person.Phone1 = motherCellPhone;
                    person.BirthDate = motherBirthDate;
                    person.Email = motherEmail;
                    if (person.ID == 0)
                    {
                        person.Customer = customer;
                        EntityHelper.SetAuditFieldForInsert(person, principal.Identity.Name);
                        context.Add(person);
                    }
                    else
                        EntityHelper.SetAuditFieldForUpdate(person, principal.Identity.Name);

                }                

                EntityHelper.SetAuditFieldForUpdate(contract, principal.Identity.Name);
                context.SaveChanges();
            }
        }

        public PackageHeader GetPackageInContract(string contractNo)
        {
            return context.Contracts.Single(con => con.ContractNo == contractNo).PackageHeader;
        }

        public IEnumerable<InvoiceHeader> GetActiveInvoices(string contractNo)
        {
            var query = from con in context.Contracts
                        join inv in context.InvoiceHeaders on con.ID equals inv.ContractID
                        where !inv.VoidDate.HasValue
                              && con.ContractNo == contractNo
                        select inv;
            return query.ToList();
        }

        public void Delete(int[] contractsID)
        {
            context.Delete(context.Contracts.Where(row => contractsID.Contains(row.ID)));
            context.SaveChanges();
        }

        public void VoidContract(string contractNo)
        {
            Contract contract = context.Contracts.SingleOrDefault(row => row.ContractNo == contractNo);
            if (contract != null)
            {
                contract.VoidDate = DateTime.Now;
                contract.Status = ContractStatus.VOID;
                context.SaveChanges();
            }
        }

        public void DeActivateContract(string contractNo)
        {
            Contract contract = context.Contracts.SingleOrDefault(row => row.ContractNo == contractNo);
            if (contract != null)
            {
                if (contract.ActiveDate.HasValue)
                {

                    contract.ActiveDate = (DateTime?)null;
                    contract.ChangedWho = principal.Identity.Name;
                    contract.ChangedWhen = DateTime.Now;
                    context.SaveChanges();
                }
            }
        }

        public void ActivateContract(string contractNo)
        {
            Contract contract = context.Contracts.SingleOrDefault(row => row.ContractNo == contractNo);
            if (contract != null)
            {
                if (!contract.ActiveDate.HasValue)
                {
                    contract.ActiveDate = DateTime.Now;
                    contract.ChangedWho = principal.Identity.Name;
                    contract.ChangedWhen = DateTime.Now;
                    context.SaveChanges();
                }
            }
        }

        public void CloseContract(string contractNo)
        {
            Contract contract = context.Contracts.SingleOrDefault(row => row.ContractNo == contractNo);
            if (contract != null)
            {
                contract.ClosedDate = DateTime.Now;
                contract.Status = ContractStatus.CLOSED;
                contract.ChangedWho = principal.Identity.Name;
                contract.ChangedWhen = DateTime.Now;
                context.SaveChanges();
            }
        }

        public Contract GetContractByInvoiceNo(string invoiceNo)
        {
            return (from con in context.Contracts
                    join inv in context.InvoiceHeaders on con.ID equals inv.ContractID
                    where inv.InvoiceNo == invoiceNo
                    select con).SingleOrDefault();
        }

        public bool IsValidContract(string contractNo)
        {
            return context.Contracts.Count(con => con.ContractNo == contractNo && !con.VoidDate.HasValue) > 0;
        }

        public Contract Get(string contractNo)
        {
            return context.Contracts.SingleOrDefault(row => row.ContractNo == contractNo);
        }

        public Contract Get(int contractID)
        {
            return context.Contracts.SingleOrDefault(row => row.ID == contractID);
        }

        public string DecodeStatus(char status)
        {
            switch (status)
            {
                case 'P': return "UnPaid";
                case 'A': return "Paid";
                case 'V': return "Void";
                case 'C': return "Closed";
            }

            return String.Empty;
        }

        public IEnumerable<Contract> GetActiveContracts(string customerBarcode)
        {
            var query = from con in context.Contracts
                        join cust in context.Customers on con.CustomerID equals cust.ID
                        where cust.Barcode == customerBarcode
                           && !con.VoidDate.HasValue
                           && !con.ClosedDate.HasValue
                        orderby con.ChangedWhen descending
                        select con;

            return query.ToList();
        }


        public void UpdateNextDuesDate(string contractNo, DateTime expiredDate, DateTime nextDuesDate)
        {
            var contract = context.Contracts.SingleOrDefault(con => con.ContractNo == contractNo);
            if (contract != null)
            {
                contract.NextDuesDate = nextDuesDate;
                contract.ExpiredDate = expiredDate;
                context.SaveChanges();
            }
        }
        
    }
}
