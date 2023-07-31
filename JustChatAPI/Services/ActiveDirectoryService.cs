using System.DirectoryServices.AccountManagement;

namespace JustChatAPI.Services
{
    public class ActiveDirectoryService
    {
        private readonly string? _domain;
        private readonly string? _username;
        private readonly string? _password;

        public ActiveDirectoryService(IConfiguration configuration)
        {
            _domain = configuration["ActiveDirectorySettings:Domain"];
            _username = configuration["ActiveDirectorySettings:Username"];
            _password = configuration["ActiveDirectorySettings:Password"];
        }

        public void CreateUser(string username, string password)
        {
            using (var context = new PrincipalContext(ContextType.Domain, _domain, _username, _password))
            {
                using (var user = new UserPrincipal(context))
                {
                    user.SamAccountName = username;
                    user.SetPassword(password);
                    user.Enabled = true;
                    user.Save();
                }
            }
        }

        public bool AuthentificateUser(string username, string password)
        {
            using (var context = new PrincipalContext(ContextType.Domain, _domain))
            {
                return context.ValidateCredentials(username, password);
            }
        }

        public List<string> getContacts()
        {
            List<string> contacts = new List<string>();
            using (PrincipalContext context = new PrincipalContext(ContextType.Domain, _domain, _username, _password))
            {
                using (PrincipalSearcher searcher = new PrincipalSearcher(new UserPrincipal(context)))
                {
                    string ouName = "LDAP://OU=ecole,DC=justchat, DC=local";
                    //searcher.QueryFilter = new Contact
                    foreach (Principal result in searcher.FindAll())
                    {
                        if (result is UserPrincipal user)
                        {
                            contacts.Add(user.SamAccountName);
                        }

                    }
                }
            }

            return contacts;
        }


    }
}
