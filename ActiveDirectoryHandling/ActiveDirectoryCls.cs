using System;
using System.Collections;
using System.DirectoryServices;
using System.Security.Principal;
using System.Text;


namespace ActiveDirectoryHandling
{
    public class ActiveDirectoryCls
    {

        public string ServerIp { get; set; }
        public string ObjectDn { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool? LDAP { get; set; }

        /// <summary>
        /// Method used to create an entry to the AD.
        /// Replace the path, username, and password.
        /// </summary>
        /// <returns>DirectoryEntry</returns>
        public DirectoryEntry GetDirectoryEntry()
        {
            try
            {
                string server = ServerIp;//"192.168.1.254";
                string objectDn = ObjectDn;//"Tafeel";
                string userName = UserName;//"administrator";
                string password = Password;//"P@ssw0rd";
                DirectoryEntry de = new DirectoryEntry("LDAP://" + server + "/" + objectDn, userName, password);
                return de;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public void CreateNewAccount()
        {
            try
            {
                DirectoryEntry localMachine = new DirectoryEntry("WinNT://" +
                Environment.MachineName);
                DirectoryEntry newUser = localMachine.Children.Add("Test", "user");
                newUser.Invoke("SetPassword", new object[] { "1234" });
                newUser.CommitChanges();
                Console.WriteLine(newUser.Guid.ToString());
                localMachine.Close();
                newUser.Close();
            }
            catch (Exception ex)
            {

            }
        }

        public ArrayList GetADUsers()
        {
            try
            {
                ArrayList UsersList = new ArrayList();
                DirectoryEntry localMachine = new DirectoryEntry("WinNT://" + Environment.MachineName + ",Computer");
                DirectoryEntry admGroup = localMachine.Children.Find("administrators", "group");
                object members = admGroup.Invoke("members", null);

                foreach (object groupMember in (IEnumerable)members)
                {
                    DirectoryEntry member = new DirectoryEntry(groupMember);
                    UsersList.Add(member.Name);
                }

                return UsersList;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public bool LogonValid(string userName, string password, string domain)
        {
            DirectoryEntry de = null;
            if (LDAP == true)
            {
                //for PME only below code
                de = new DirectoryEntry("LDAP://" + domain, userName, password);
            }
            else
            {
                de = new DirectoryEntry(null, domain + "\\" + userName, password);
            }
            try
            {
                DirectorySearcher ds = new DirectorySearcher(de);
                ds.Filter = "samaccountname=" + userName;
                ds.PropertiesToLoad.Add("cn");
                SearchResult sr = ds.FindOne();
                if (sr == null) throw new Exception();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public bool IsExistInAD(string loginName)
        {

            string userName = ExtractUserName(loginName);
            DirectorySearcher search = new DirectorySearcher();
            search.Filter = String.Format("(SAMAccountName={0})", userName);
            search.PropertiesToLoad.Add("cn");
            search.PropertiesToLoad.Add("samaccountname");
            search.PropertiesToLoad.Add("givenname");
            search.PropertiesToLoad.Add("sn");
            SearchResult result = search.FindOne();


            if (result == null)
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        public clsActiveDirectoryUser GetADUserData(string loginName)
        {

            clsActiveDirectoryUser ADUserObject = new clsActiveDirectoryUser();
            try
            {
                string userName = ExtractUserName(loginName);
                DirectorySearcher search = new DirectorySearcher();
                search.Filter = String.Format("(SAMAccountName={0})", userName);
                search.PropertiesToLoad.Add("cn");
                search.PropertiesToLoad.Add("samaccountname");
                search.PropertiesToLoad.Add("givenname");
                search.PropertiesToLoad.Add("sn");
                SearchResult result = search.FindOne();

                if (result != null)
                {
                    ADUserObject.SamAccountName = (string)result.Properties["samaccountname"][0];
                    ADUserObject.GivenName = (string)result.Properties["givenname"][0];
                    ADUserObject.SureName = (string)result.Properties["sn"][0];
                    ADUserObject.UserGroups = GetADUserGroups(userName);
                    return ADUserObject;

                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        //find object by SID
        public clsActiveDirectoryUser GetADUserDataByUserSID(string SID)
        {
            DirectoryEntry de = new DirectoryEntry(null, this.ObjectDn + "\\" + UserName, Password);
            clsActiveDirectoryUser ADUserObject = new clsActiveDirectoryUser();
            try
            {
                DirectorySearcher search = new DirectorySearcher(de);
                // retrieve the SID
                byte[] objSID = new byte[28];
                ////convert SID to Byte Array
                //byte[] objSID_Sid = new byte[SID.Length];
                //for (int i = 0; i < SID.Length; i++)
                //{
                //    byte current= Convert.ToByte(SID[i]);
                //    objSID_Sid[i] = current;
                //}
                SecurityIdentifier securityIdentifier = new SecurityIdentifier(SID);
                securityIdentifier.GetBinaryForm(objSID, 0);
                //encode it using APPENDFORMAT method of StringBuilder
                StringBuilder hexSID = new StringBuilder();
                for (int i = 0; i < objSID.Length; i++)
                {
                    hexSID.AppendFormat("\\{0:x2}", objSID[i]);
                }
                string filter = string.Format("(objectSid={0})", hexSID.ToString());
                search.Filter = filter;//"((=" + SID + ")(objectClass=user))";//String.Format("(SAMAccountName={0})", userName);

                SearchResult result = search.FindOne();
                var resutentry = result.GetDirectoryEntry();
                //var list = new List<KeyValuePair<string, string>>();
                //foreach (PropertyValueCollection property in resutentry.Properties)
                //{
                //    foreach (object o in property)
                //    {
                //        string value = o.ToString();
                //        list.Add(new KeyValuePair<string, string>(property.PropertyName, value));
                //    } 
                //}

                if (result != null)
                {
                    ADUserObject.SamAccountName = (string)result.Properties["samaccountname"][0];
                    ADUserObject.SureName = (string)result.Properties["cn"][0];
                    return ADUserObject;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }

        }



        string ExtractUserName(string path)
        {
            string[] userPath = path.Split(new char[] { '\\' });
            return userPath[userPath.Length - 1];
        }


        string GetADUserGroups(string userName)
        {
            DirectorySearcher search = new DirectorySearcher();
            search.Filter = String.Format("(SAMAccountName={0})", userName);
            search.PropertiesToLoad.Add("memberOf");
            StringBuilder groupsList = new StringBuilder();

            SearchResult result = search.FindOne();
            if (result != null)
            {
                int groupCount = result.Properties["memberOf"].Count;

                for (int counter = 0; counter < groupCount; counter++)
                {
                    groupsList.Append((string)result.Properties["memberOf"][counter]);
                    groupsList.Append("|");
                }
            }
            groupsList.Length -= 1; //remove the last '|' symbol
            return groupsList.ToString();
        }



        public ArrayList GetADGroupUsers(string groupName)
        {
            SearchResult result;
            DirectorySearcher search = new DirectorySearcher();
            search.Filter = String.Format("(cn={0})", groupName);
            search.PropertiesToLoad.Add("member");
            result = search.FindOne();

            ArrayList userNames = new ArrayList();
            if (result != null)
            {
                for (int counter = 0; counter <
                         result.Properties["member"].Count; counter++)
                {
                    string user = (string)result.Properties["member"][counter];
                    userNames.Add(user);
                }
            }
            return userNames;
        }



    }
}
