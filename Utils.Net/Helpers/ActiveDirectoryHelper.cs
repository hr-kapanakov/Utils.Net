using System.Collections;
using System.DirectoryServices.AccountManagement;

namespace Utils.Net.Helpers
{
    /// <summary>
    /// Contains methods for work with Active Directory.
    /// </summary>
    public static class ActiveDirectoryHelper
    {
        #region Validate Methods

        /// <summary>
        /// Validates the username and password of a given user.
        /// </summary>
        /// <param name="userName">The username to validate.</param>
        /// <param name="password">The password of the username to validate.</param>
        /// <returns>Returns True of user is valid</returns>
        public static bool ValidateCredentials(string userName, string password)
        {
            var principalContext = GetPrincipalContext();
            return principalContext.ValidateCredentials(userName, password);
        }

        /// <summary>
        /// Checks if the User Account is Expired.
        /// </summary>
        /// <param name="userName">The username to check.</param>
        /// <returns>Returns true if Expired.</returns>
        public static bool IsUserExpired(string userName)
        {
            var userPrincipal = GetUser(userName);
            if (userPrincipal.AccountExpirationDate != null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Checks if user exists on AD.
        /// </summary>
        /// <param name="userName">The username to check.</param>
        /// <returns>Returns true if username Exists.</returns>
        public static bool IsUserExisiting(string userName)
        {
            if (GetUser(userName) == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Checks if user account is locked.
        /// </summary>
        /// <param name="userName">The username to check.</param>
        /// <returns>Returns true of Account is locked.</returns>
        public static bool IsAccountLocked(string userName)
        {
            var userPrincipal = GetUser(userName);
            return userPrincipal.IsAccountLockedOut();
        }

        #endregion

        #region Search Methods

        /// <summary>
        /// Gets a certain user on Active Directory.
        /// </summary>
        /// <param name="userName">The username to get.</param>
        /// <returns>Returns the UserPrincipal Object.</returns>
        public static UserPrincipal GetUser(string userName)
        {
            var principalContext = GetPrincipalContext();

            var userPrincipal = UserPrincipal.FindByIdentity(principalContext, userName);
            return userPrincipal;
        }

        /// <summary>
        /// Gets a certain group on Active Directory.
        /// </summary>
        /// <param name="groupName">The group to get.</param>
        /// <returns>Returns the GroupPrincipal Object.</returns>
        public static GroupPrincipal GetGroup(string groupName)
        {
            var principalContext = GetPrincipalContext();

            var groupPrincipal = GroupPrincipal.FindByIdentity(principalContext, groupName);
            return groupPrincipal;
        }

        #endregion

        #region Group Methods

        /// <summary>
        /// Checks if user is a member of a given group.
        /// </summary>
        /// <param name="userName">The user you want to validate.</param>
        /// <param name="groupName">The group you want to check the membership of the user.</param>
        /// <returns>Returns true if user is a group member.</returns>
        public static bool IsUserGroupMember(string userName, string groupName)
        {
            var userPrincipal = GetUser(userName);
            var groupPrincipal = GetGroup(groupName);

            if (userPrincipal != null && groupPrincipal != null)
            {
                foreach (var member in groupPrincipal.GetMembers(true))
                {
                    if (userName == member.Name)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Gets a list of the users group memberships.
        /// </summary>
        /// <param name="userName">The user you want to get the group memberships.</param>
        /// <returns>Returns an ArrayList of group memberships.</returns>
        public static ArrayList GetUserGroups(string userName)
        {
            var myItems = new ArrayList();
            var userPrincipal = GetUser(userName);

            var principalSearchResult = userPrincipal.GetGroups();
            foreach (var result in principalSearchResult)
            {
                myItems.Add(result.Name);
            }

            return myItems;
        }

        /// <summary>
        /// Gets a list of the users authorization groups.
        /// </summary>
        /// <param name="userName">The user you want to get authorization groups.</param>
        /// <returns>Returns an ArrayList of group authorization memberships.</returns>
        public static ArrayList GetUserAuthorizationGroups(string userName)
        {
            var myItems = new ArrayList();
            var userPrincipal = GetUser(userName);

            var principalSearchResult = userPrincipal.GetAuthorizationGroups();
            foreach (var result in principalSearchResult)
            {
                myItems.Add(result.Name);
            }

            return myItems;
        }

        #endregion

        #region Helper Methods

        private static PrincipalContext GetPrincipalContext()
        {
            return new PrincipalContext(ContextType.Domain);
        }

        #endregion
    }
}
