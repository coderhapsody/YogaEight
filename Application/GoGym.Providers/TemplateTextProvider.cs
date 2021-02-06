using System;
using System.IO;
using System.Security.Principal;
using GoGym.Data;
using GoGym.Providers.Base;

namespace GoGym.Providers
{
    /// <summary>
    /// Summary description for TemplateTextProvider
    /// </summary>
    public class TemplateTextProvider : BaseProvider
    {
        public TemplateTextProvider(FitnessEntities context, IPrincipal principal) : base(context, principal)
        {
        }

        public string GetTermsConditionsText(string pathToTemplate)
        {
            return File.ReadAllText(pathToTemplate + "TermsConditions.txt");
        }

        public string GetPresigningNotice(string pathToTemplate)
        {
            return File.ReadAllText(pathToTemplate + "PresigningNotice.txt");
        }

        public string GetReceiptFooterText(string pathToTemplate)
        {
            return File.ReadAllText(pathToTemplate + "ReceiptFooterText.txt");
        }

        public void UpdateTermsConditions(string pathToTemplate, string text)
        {
            File.WriteAllText(pathToTemplate + "TermsConditions.txt", text);
        }

        public void UpdatePresigningNotice(string pathToTemplate, string text)
        {
            File.WriteAllText(pathToTemplate + "PresigningNotice.txt", text);
        }

        public void UpdateReceiptFooterText(string pathToTemplate, string text)
        {
            File.WriteAllText(pathToTemplate + "ReceiptFooterText.txt", text);
        }
    }
}