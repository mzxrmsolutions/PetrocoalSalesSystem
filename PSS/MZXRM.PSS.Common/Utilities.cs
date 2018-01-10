using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Configuration;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace MZXRM.PSS.Common
{
    public class Utilities
    {
        #region " SortDirection Enumeration "
        public enum SortDirection
        {
            ASCECDING = 0,
            DESCENDING = 1,
        }
        #endregion

        #region " RemoveDuplicateWords Function "
        public static string RemoveDuplicateWords(string v)
        {
            var dic = new Dictionary<string, bool>();
            StringBuilder strbuilder = new StringBuilder();
            string[] a = v.Split(',');
            foreach (string current in a)
            {
                string lower = current.ToLower();
                if (!dic.ContainsKey(lower))
                {
                    strbuilder.Append(current).Append(',');
                    dic.Add(lower, true);
                }
            }
            return strbuilder.ToString().Trim().Substring(0, strbuilder.Length - 1);
        }
        #endregion

        #region " Sort Function "
        public static List<T> Sort<T>(IEnumerable<T> list, string sortField, SortDirection sortDirection)
        {
            var param = Expression.Parameter(typeof(T), string.Empty);
            PropertyInfo propertyInfo = typeof(T).GetProperty(sortField, BindingFlags.IgnoreCase | BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public);
            var property = Expression.Property(param, propertyInfo);
            var lambda = Expression.Lambda<Func<T, object>>(Expression.Convert(property, typeof(object)), param);
            var returnList = new System.Collections.Generic.List<T>();
            if (sortDirection == SortDirection.ASCECDING)
                returnList = list.AsQueryable().OrderBy(lambda).ToList();
            else
                returnList = list.AsQueryable().OrderByDescending(lambda).ToList();
            return returnList;
        }
        #endregion

        #region " GetListForCommaSeparatedString Function "
        public static List<string> GetListForCommaSeparatedString(string ConcatenatedIDs)
        {
            List<string> objList = new List<string>();
            string[] splitPrinterIDs = ConcatenatedIDs.Split(',');
            if (splitPrinterIDs.Length > 0)
            {
                for (int i = 0; i < splitPrinterIDs.Length; i++)
                {
                    objList.Add(splitPrinterIDs[i]);
                }
            }
            return objList;
        }
        #endregion

        #region " SendMailMessage Function Added by kashif Abbas on 27th February 2017 to send email for Contact Us page. Ref JIRA: https://ateaintern.atlassian.net/browse/DBS-595 "
        public static void SendMailMessageForContactUs(string from, string to, string subject, string body, string FromEmailDisplayName, string bcc = null, string cc = null)
        {
            var mMailMessage = new MailMessage { };
            mMailMessage.From = new MailAddress(from, FromEmailDisplayName);


            #region " Adding ToEmail "
            if (!string.IsNullOrEmpty(to))
            {
                string[] ToEmailArray = to.Split(",;".ToCharArray());
                if (ToEmailArray.Length > 0)
                {
                    foreach (string ToEmail in ToEmailArray)
                    {
                        if (!String.IsNullOrEmpty(ToEmail))
                        {
                            mMailMessage.To.Add(new MailAddress(ToEmail));
                        }
                    }
                }
            }
            #endregion

            #region "Adding CC Email "
            if (!string.IsNullOrEmpty(cc))
            {
                string[] CCEmailArray = cc.Split(",;".ToCharArray());
                if (CCEmailArray.Length > 0)
                {
                    foreach (string CCEmail in CCEmailArray)
                    {
                        if (!String.IsNullOrEmpty(CCEmail))
                        {
                            mMailMessage.CC.Add(new MailAddress(CCEmail));
                        }
                    }
                }
            }
            #endregion

            #region " Adding BCC Email "
            if (!string.IsNullOrEmpty(bcc))
            {
                var bccEmailArray = bcc.Split(",;".ToCharArray());
                if (bccEmailArray.Length > 0)
                {
                    foreach (var bccEmail in bccEmailArray)
                    {
                        mMailMessage.Bcc.Add(new MailAddress(bccEmail));
                    }
                }
            }
            #endregion

            #region " Setting MailMessage properties values "
            mMailMessage.Subject = subject;
            mMailMessage.Body = body;
            mMailMessage.IsBodyHtml = true;
            mMailMessage.Priority = MailPriority.Normal;
            #endregion

            using (var mSmtpClient = GettSmtpClientObject())
            {
                mSmtpClient.Send(mMailMessage);
            }
        }
        #endregion

        #region " GettSmtpClientObject Fucntion "
        public static SmtpClient GettSmtpClientObject()
        {
            var mSmtpClient = new SmtpClient();
            var smtpSection = WebConfigurationManager.GetSection("system.net/mailSettings/smtp") as SmtpSection;
            mSmtpClient.Host = smtpSection.Network.Host;

            mSmtpClient.EnableSsl = smtpSection.Network.EnableSsl;

            #region " NetworkCredential Section "
            if (!string.IsNullOrEmpty(smtpSection.Network.UserName) && !string.IsNullOrEmpty(smtpSection.Network.Password))
            {
                System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential();
                NetworkCred.UserName = smtpSection.Network.UserName;
                NetworkCred.Password = smtpSection.Network.Password;
                mSmtpClient.Credentials = NetworkCred;
            }
            #endregion

            mSmtpClient.UseDefaultCredentials = smtpSection.Network.DefaultCredentials;
            mSmtpClient.Port = smtpSection.Network.Port;
            return mSmtpClient;
        }
        #endregion
    }
}
