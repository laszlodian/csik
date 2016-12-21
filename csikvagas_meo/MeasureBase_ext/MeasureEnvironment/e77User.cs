using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.DirectoryServices.Protocols;
using e77.MeasureBase.Properties;

namespace e77.MeasureBase.MeasureEnvironment
{
    public class e77User
    {
        public e77User()
        {
            if(TheUser != null)
                throw new MeasureBaseException("Singleton");
            TheUser = this;

            Name = System.Environment.GetEnvironmentVariable("USERNAME"); //todo

            string accessRights = System.Environment.GetEnvironmentVariable("ACCESSRIGHTS");
            SetAccessRights(accessRights);
        }
        public static e77User TheUser { get; private set; }

        public string Name { get; internal set; }
        
        string _fullName;
        public string FullName 
        {
            get
            {
                if (_fullName == null)
                {
                    if (!LdapDenied)
                        _fullName = GetFullName(Name);
                    else
                        throw new InvalidOperationException("LdapDenied!");
                }
                return _fullName;
            }

            set
            {
                _fullName = value;
            }
        }
        
        static internal bool LdapDenied { get; private set; }

        /*Call it before construction  of User class*/
        [Obsolete("[WARNING] Using this property is dangerous. Do not use for real environment!")]
        public static void DenyLdap()
        {
            LdapDenied = true;
        }
        
        static public string GetFullName(string userName_in)
        {
            string fullName = null;
            try
            {
                Trace.TraceInformation("Felhasználónév lekérdezése: '{0}'", userName_in);

                //obsolete?: sometimes this function is slow. Temporally workaround for development:
                //if (!MeasureConfig.TheConfig.SqlReleaseDb)                return "Fast accessable fullname";

                LdapDirectoryIdentifier myLdapDirectoryIdentifier = new LdapDirectoryIdentifier("77ks", 389); //77ks.77el.hu
                LdapConnection ldapConn = new LdapConnection(myLdapDirectoryIdentifier);
                ldapConn.SessionOptions.ProtocolVersion = 3;
                ldapConn.AuthType = AuthType.Basic;

                SearchRequest findme = new SearchRequest();
                findme.DistinguishedName = string.Format("uid={0},ou=People,dc=77el,dc=hu", userName_in);
                findme.Scope = System.DirectoryServices.Protocols.SearchScope.Base;
                SearchResponse results = (SearchResponse)ldapConn.SendRequest(findme);

                SearchResultEntryCollection entries = results.Entries;

                if (entries.Count == 0)
                    throw new NotFoundException(string.Format("User {0} at LDAP", userName_in));
                else if (entries.Count > 1)
                    throw new MeasureBaseInternalException(string.Format("More user with id {0} at LDAP", userName_in));

                fullName = entries[0].Attributes["cn"][0].ToString();
                Trace.TraceInformation("User id {0}, full name = {1}", userName_in, fullName);
            }
            catch (Exception ex)
            {
                ex.ReportError();
                throw new Exception(string.Format(Resources.LDAP_ERROR, userName_in), ex);
            }

            return fullName;
        }
        
        #region AccessRight

        /// <summary>
        /// All access right of the user
        /// </summary>
        public string[] AccessRights { get; internal set; }

        internal void SetAccessRights(string accessRights)
        {
            if (accessRights == null || accessRights == string.Empty)
            {
                AccessRights = new string[] { };
            }
            else
            {
                AccessRights = accessRights.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < AccessRights.Length; i++)
                    AccessRights[i] = AccessRights[i].Trim();
            }

            _validAccessRights = null; //invalidate
        }

        public bool HasAccessRight(string accessRight_in)
        {
            //if (!IsAccessRightValid(accessRight_in)) /// ALL_ACCESS_RIGTS tartalmazza-e accessRigt_in-t?
            //    throw new ArgumentException(string.Format("Invalid AccessRight: '{0}'. Valid rights: '{1}'",
            //        accessRight_in, ALL_ACCESS_RIGHTS));

            //todo ? faster method:            Array.BinarySearch, Array.Sort

            return ALL_ACCESS_RIGHTS.Contains(accessRight_in);
            
            
            foreach (string ar in (_validAccessRights == null ? //if it is null, use lower ALL_ACCESS_RIGHTS, because counting ValidAccessRights needs HasAccessRight, leads to infinite recursion
                            AccessRights : ValidAccessRights)) 
            {
                if (ar.ToLower().StartsWith(accessRight_in.ToLower()))
                    return true;
            }
            return false;
        }

        [Obsolete("[WARNING] Using this property is dangerous. Can be used for turn AccessRight checking off!")]
        public void AddAccessRight(string accessRight_in)
        {
            Trace.TraceWarning("User.AddAccessRight('{0}') has been called", accessRight_in);

            if(!IsAccessRightValid(accessRight_in))
                throw new ArgumentException( string.Format("Invalid AccessRight: '{0}'. Valid rights: '{1}'", 
                    accessRight_in, ALL_ACCESS_RIGHTS));

            List<string> ar = new List<string>(AccessRights);
            ar.Add(accessRight_in);
            AccessRights = ar.ToArray();
            _validAccessRights = null; //invalidate
        }

        static string[] _validAccessRights = null;
        
        /// <summary>
        /// User's all AccessRights for this Appllication
        /// </summary>
        internal string[] ValidAccessRights
        {
            get
            {
                if (_validAccessRights == null)
                {
                    _validAccessRights = ALL_ACCESS_RIGHTS.Where(right => HasAccessRight(right)).ToArray();
                }

                return _validAccessRights;
            }
        }

        string[] ALL_ACCESS_RIGHTS 
        {
            get { return e77.MeasureBase.Sql.EnvironmentId.TheEnvironmentId.ALL_ACCESS_RIGHTS; } 
        }

        private bool IsAccessRightValid(string accessRight_in)
        {
            if (ALL_ACCESS_RIGHTS == null)
                throw new MeasureBaseException("User.ALL_ACCESS_RIGHTS has not been initialized.");

            return ALL_ACCESS_RIGHTS.Contains(accessRight_in);
        }
        
        #endregion
    }
}
