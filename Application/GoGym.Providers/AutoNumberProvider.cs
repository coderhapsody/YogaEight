using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Principal;
using GoGym.Data;
using GoGym.Providers.Base;

namespace GoGym.Providers
{
    public class AutoNumberProvider : BaseProvider
    {
        public AutoNumberProvider(FitnessEntities context, IPrincipal principal) : base(context, principal)
        {            
        }

        public void Update(string formCode, int branchID, int year, int newLastNumber)
        {
            Create(formCode, branchID, year);

            var autoNumber =
                context.AutoNumbers.SingleOrDefault(
                    row => row.FormCode == formCode && row.BranchID == branchID && row.Year == year);
            if (autoNumber != null)
            {
                autoNumber.LastNumber = newLastNumber;
                context.SaveChanges();
            }
        }

        public IEnumerable<AutoNumber> GetByBranch(int branchID)
        {
            return context.AutoNumbers.Where(an => an.BranchID == branchID).ToList();
        }

        private void Create(string formCode, int branchID, int year)
        {
            AutoNumber lastYearAutoNumber = context.AutoNumbers.SingleOrDefault(
                row => row.FormCode == formCode &&
                       row.BranchID == branchID &&
                       row.Year == year - 1);

            AutoNumber autoNumber = context.AutoNumbers.SingleOrDefault(
                row => row.FormCode == formCode &&
                       row.BranchID == branchID &&
                       row.Year == year);


            string prefix = lastYearAutoNumber == null ? formCode : lastYearAutoNumber.Prefix;

            if (autoNumber == null)
            {
                var paramFormCode = new SqlParameter();
                paramFormCode.ParameterName = "@FormCode";
                paramFormCode.DbType = DbType.String;
                paramFormCode.Value = formCode;

                var paramBranchID = new SqlParameter();
                paramBranchID.ParameterName = "@BranchID";
                paramBranchID.DbType = DbType.String;
                paramBranchID.Value = branchID;

                var paramYear = new SqlParameter();
                paramYear.ParameterName = "@Year";
                paramYear.DbType = DbType.Int32;
                paramYear.Value = year;

                var paramPrefix = new SqlParameter();
                paramPrefix.ParameterName = "@Prefix";
                paramPrefix.DbType = DbType.String;
                paramPrefix.Value = prefix;

                var paramLastNumber = new SqlParameter();
                paramLastNumber.ParameterName = "@LastNumber";
                paramLastNumber.DbType = DbType.Int32;
                paramLastNumber.Value = 0;

                context.ExecuteNonQuery(
                    "INSERT INTO AutoNumber(FormCode, BranchID, [Year], Prefix, LastNumber) VALUES(@FormCode, @BranchID, @Year, @Prefix, @LastNumber)",
                    paramFormCode,
                    paramBranchID,
                    paramYear,
                    paramPrefix,
                    paramLastNumber);
                //using (var ctx = new FitnessEntities())
                //{
                //    autoNumber = new AutoNumber();
                //    autoNumber.BranchID = branchID;
                //    autoNumber.FormCode = formCode;
                //    autoNumber.Year = year;
                //    autoNumber.LastNumber = 0;
                //    autoNumber.Prefix = prefix;
                //    ctx.Add(autoNumber);
                //    ctx.SaveChanges();    
                //}                
            }
        }

        public AutoNumber GetCurrentAutoNumber(string formCode, int branchID, int year)
        {
            Create(formCode, branchID, year);
            return
                context.AutoNumbers.Single(
                    row => row.FormCode == formCode && row.BranchID == branchID && row.Year == year);
        }

        public void Increment(string formCode, int branchID, int year)
        {
            var autoNumber =
                context.AutoNumbers.Single(
                    row => row.FormCode == formCode && row.BranchID == branchID && row.Year == year);
            autoNumber.LastNumber++;
            //context.SubmitChanges();
        }

        public string Generate(int branchID, string formCode, int month, int year)
        {
            string result;
            var currentAutoNumber = GetCurrentAutoNumber(formCode, branchID, year);
            int currentNumber = currentAutoNumber.LastNumber;
            string prefix = currentAutoNumber.Prefix;
            string branchCode = context.Branches.Single(row => row.ID == branchID).Code;

            currentNumber++;

            if (formCode == "CO")
            {
                // contract
                result = String.Format("{0}{1}/{2}/{3}",
                    branchCode.ToUpper(),
                    month.ToString("00"),
                    Convert.ToString(year).Substring(2, 2),
                    currentNumber.ToString("00000"));
            }
            else if (formCode == "CU")
            {
                result = String.Format("{0}{1}",
                    branchCode.ToUpper(),
                    currentNumber.ToString("00000"));
            }
            else if (formCode == "OR")
            {
                result = String.Format("{0}/{1}{2}/{3}",
                    branchCode.ToUpper(),
                    month.ToString("00"),
                    Convert.ToString(year).Substring(2, 2),
                    currentNumber.ToString("00000"));
            }
            else if (formCode == "BL")
            {
                result = String.Format("{0}-{1}{2}-{3}",
                    branchCode.ToUpper(),
                    month.ToString("00"),
                    Convert.ToString(year).Substring(2, 2),
                    currentNumber.ToString("00000"));
            }
            else
            {
                result = String.Format("{0}/{1}{2}{3}/{4}",
                    formCode,
                    branchCode.ToUpper(),
                    month.ToString("00"),
                    Convert.ToString(year).Substring(2, 2),
                    currentNumber.ToString("000"));
            }

            return result;
        }
    }
}
