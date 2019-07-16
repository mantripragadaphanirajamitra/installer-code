using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Deployment.WindowsInstaller;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Net;
using System.DirectoryServices;

namespace CustomAction1
{
    public class CustomActions
    {
        //static string httpsPortValue = string.Empty;
        //static string httpPortValue = string.Empty;

        [CustomAction]
        public static ActionResult ValidateForSpaceFromUserAgent(Session session)
        {
            session.Log("Begin ValidateForSpaceFromUserAgent");
            bool bVirDirVal = false, bPoolIdeName = false, bPoolIdePwd = false;
            string errMsg = string.Empty;
            string val = session["VIR_DIR_VAL"];
            string val1 = session["POOL_ID_NAME_VAL"];
            string val2 = session["POOL_ID_PWD_VAL"];

            session["RETERROR"] = "0";

            if (!string.IsNullOrEmpty(val))
            {
                if (Regex.IsMatch(val, @"(^\s)|(\s$)"))
                {
                    bVirDirVal = true;
                }
            }

            if (!string.IsNullOrEmpty(val1) && !string.IsNullOrEmpty(val2))
            {
                if (Regex.IsMatch(val1, @"(^\s)|(\s$)"))
                {
                    bPoolIdeName = true;
                }

                if (Regex.IsMatch(val2, @"(^\s)|(\s$)"))
                {
                    bPoolIdePwd = true;
                }
            }

            if (bVirDirVal && bPoolIdeName && bPoolIdePwd)
            {
                errMsg = "All the field values contains space(s). Please remove space(s).";
            }
            else if (bVirDirVal && bPoolIdeName)
            {
                errMsg = "Agent Name and Service Account Name field value contains space(s). Please remove space(s).";
            }
            else if (bVirDirVal && bPoolIdePwd)
            {
                errMsg = "Agent Name and Service Account Password field value contains space(s). Please remove space(s).";
            }
            else if (bPoolIdeName && bPoolIdePwd)
            {
                errMsg = "Service Account Name and Service Account Password field value contains space(s). Please remove space(s).";
            }
            else if (bVirDirVal)
            {
                errMsg = "Agent Name field value contains space(s). Please remove space(s).";
            }
            else if (bPoolIdeName)
            {
                errMsg = "Service Account Name field value contains space(s). Please remove space(s).";
            }
            else if (bPoolIdePwd)
            {
                errMsg = "Service Account Password field value contains space(s). Please remove space(s).";
            }
            if (!string.IsNullOrEmpty(errMsg))
            {
                session["ERRMESSAGE"] = errMsg;
                session["RETERROR"] = "1";
            }

            return ActionResult.Success;
        }

        [CustomAction]
        public static ActionResult ValidateForSpaceFromSXPUrl(Session session)
        {
            session.Log("Begin ValidateForSpaceFromSXPUrl");

            string errMsg = string.Empty;
            string val = session["DOMAINNAME"];
            string val1 = session["CONTEXTNAME"];
            string val2 = session["PORTNUMBER"];

            bool bDomainName = false, bContextName = false, bPortNum = false;

            session["RETERROR"] = "0";

            if (!string.IsNullOrEmpty(val))
            {
                if (Regex.IsMatch(val, @"(^\s)|(\s$)"))
                {
                    bDomainName = true;
                }
            }

            if (!string.IsNullOrEmpty(val1))
            {
                if (Regex.IsMatch(val1, @"(^\s)|(\s$)"))
                {
                    bContextName = true;
                }
            }
            //if (!string.IsNullOrEmpty(val1))
            //{
            //    if ((Regex.IsMatch(val1, @"\d")))
            //    {
            //        session["ERRMESSAGE"] = "Port Number only allow two or four-digit number";
            //    }
            //}

            if (!string.IsNullOrEmpty(val2))
            {
                if (Regex.IsMatch(val2, @"(^\s)|(\s$)"))
                {
                    bPortNum = true;
                }
            }

            if (bDomainName && bContextName && bPortNum)
            {
                errMsg = "All the field values contains space(s). Please remove space(s).";
            }
            else if (bDomainName && bContextName)
            {
                errMsg = "Domain Name and Context Name field value contains space(s). Please remove space(s).";
            }
            else if (bDomainName && bPortNum)
            {
                errMsg = "Domain Name and Port Number field value contains space(s). Please remove space(s).";
            }
            else if (bContextName && bPortNum)
            {
                errMsg = "Context Name and Port Number field value contains space(s). Please remove space(s).";
            }
            else if (bDomainName)
            {
                errMsg = "Domain Name field value contains space(s). Please remove space(s).";
            }
            else if (bContextName)
            {
                errMsg = "Context Name field value contains space(s). Please remove space(s).";
            }
            else if (bPortNum)
            {
                errMsg = "Port Number field value contains space(s). Please remove space(s).";
            }
            if (!string.IsNullOrEmpty(errMsg))
            {
                session["ERRMESSAGE"] = errMsg;
                session["RETERROR"] = "1";
            }

            return ActionResult.Success;
        }

        //[CustomAction]
        //public static ActionResult OnCheckHttps(Session session)
        //{
        //    session.Log("Begin OnCheckHttps");

        //    session["RETERROR"] = "0";
        //    string httpType = string.Empty;

        //    httpType = session["FEATUREX_CHECKHTTP"];
        //    if (httpType.Equals("1"))
        //    {
        //        if (!string.IsNullOrEmpty(httpsPortValue))
        //        {
        //            httpsPortValue = "443";
        //        }

        //        session["PORTNUMBER"] = httpsPortValue;
        //        httpsPortValue = session["PORTNUMBER"];
        //    }
        //    else
        //    {
        //        session["PORTNUMBER"] = httpPortValue;
        //        httpPortValue = session["PORTNUMBER"];
        //    }

        //    return ActionResult.Success;
        //}

        [CustomAction]
        public static ActionResult GetCompAndDomName(Session session)
        {
            session.Log("Begin GetCompAndDomName");

            session["RETERROR"] = "0";

            string domainName = System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties().DomainName;
            string hostName = Dns.GetHostName();

            if (!hostName.Contains(domainName)) // if the hostname does not already include the domain name
            {
                hostName = hostName + "." + domainName;   // add the domain name part
            }

            session["DOMAINNAME"] = hostName;
            session["CONTEXTNAME"] = "sxp";
            session["PORTNUMBER"] = "5040";

            return ActionResult.Success;
        }
        //[CustomAction]
        //public static ActionResult GetCurrentDate(Session session)
        //{
        //    session.Log("Begin GetCurrentDate");

        //    session["RETERROR"] = "0";

        //    string currentdate = DateTime.Now.ToString("M/d/yyyy");


        //    session["DATELI"] = currentdate;
        //    return ActionResult.Success;

        //}

        [CustomAction]
        public static ActionResult ConstructSxpUrl(Session session)
        {
            session.Log("Begin ConstructSxpUrl");

            session["RETERROR"] = "0";
            
            string domainName = session["DOMAINNAME"];
            string contextName = session["CONTEXTNAME"];
            string portNum = session["PORTNUMBER"];
            string httpType = string.Empty;
            string sxpUrl = string.Empty;
            httpType = session["FEATUREX_CHECKHTTP"];
            if (httpType.Equals("1"))
            {
                httpType = "https://";
            }
            else
            {
                httpType = "http://";
            }
            //sxpUrl = httpType + domainName + ":" + portNum + "/" + contextName;
            sxpUrl = httpType + domainName + ":" + portNum + "/" + "{R:0}";

            session["SSOURLWITHPORT"] = sxpUrl;

            return ActionResult.Success;
        }
        

        [CustomAction]
        public static ActionResult GetCompAndDomNameWithoutPort(Session session)
        {
            session.Log("Begin GetCompAndDomName");

            session["RETERROR"] = "0";

            string domainName = session["DOMAINNAME"];
            string contextName = session["CONTEXTNAME"];
            string portNum = session["PORTNUMBER"];
            string httpType = string.Empty;
            string sxpUrl = string.Empty;
            httpType = session["FEATUREX_CHECKHTTP"];
            if (httpType.Equals("1"))
            {
                httpType = "https://";
            }
            else
            {
                httpType = "http://";
            }

            sxpUrl = httpType + domainName + "/" + contextName + "/iwaauth";

            session["SSOURLWITOUTPORT"] = sxpUrl;

            return ActionResult.Success;
        }

        [CustomAction]
        public static ActionResult GetContextname(Session session)
        {
            session.Log("Begin GetContextname");

            session["RETERROR"] = "0";

            
            string contextName = session["CONTEXTNAME"];
       
         
            string contextnam = string.Empty;


            contextnam =  "(" + contextName + "."+"*"+")";

            session["SXPCONTEXTNAME"] = contextnam;

            return ActionResult.Success;
        }

        [CustomAction]
        public static ActionResult ValidateAgentAccount(Session session)
        {
            session.Log("Begin ValidateAgentAccount");

            session["RETERROR"] = "0";
            string sSkip = session["FEATUREX_CHECKED"];
            if (!sSkip.Equals("1"))
            {
                string domainName = string.Empty;
                string userName = session["POOL_ID_NAME_VAL"];
                string password = session["POOL_ID_PWD_VAL"];
                string[] value = userName.Split('\\');

                if (value.Count() == 2)
                {
                    domainName = value[0];
                    userName = value[1];

                    DirectoryEntry entry = new DirectoryEntry("LDAP://" + domainName, userName, password);
                    try
                    {
                        object obj = entry.NativeObject;
                        DirectorySearcher search = new DirectorySearcher(entry);
                        search.Filter = "(SAMAccountName=" + userName + ")";
                        search.PropertiesToLoad.Add("cn");
                        SearchResult result = search.FindOne();

                        if (null == result)
                        {
                            session["ERRMESSAGE"] = "Invalid Username or Password.";
                            session["RETERROR"]   = "1";
                        }
                    }
                    catch (Exception ex)
                    {
                        session["ERRMESSAGE"] = "Invalid Username or Password. " + ex.Message;
                        session["RETERROR"]   = "1";
                    }
                }
                else
                {
                    session["ERRMESSAGE"] = "Inavlid Service Account Name. Please provide valid account name (Ex:domainname\\username).";
                    session["RETERROR"] = "1";
                }
            }

            return ActionResult.Success;
        }
    }
}