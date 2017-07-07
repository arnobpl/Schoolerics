using System.Data;
using System.Web;
using Schoolerics.AppCode.Controller.Account;
using Schoolerics.AppCode.DataConnector;
using Schoolerics.AppCode.Utility;

namespace Schoolerics.AppCode.Controller
{
    /// <summary>
    /// Login role for user account
    /// </summary>
    public enum LoginRole
    {
        Student = 1,
        Teacher,
        School
    }

    /// <summary>
    /// User authorization for signup, login, logout
    /// </summary>
    public class UserAuthorization
    {
        private const string LoginTable = "Login";
        private const string UsernameAccessString = "username";
        private const string CurrentInstanceAccessString = "currentInstance";


        /// <summary>
        /// user's username string if already logged in, otherwise null
        /// </summary>
        public string LoggedInUsername { get; private set; }

        /// <summary>
        /// user's name string if already logged in, otherwise null
        /// </summary>
        public string LoggedInUserName { get; private set; }

        /// <summary>
        /// user's user account if already logged in, otherwise null
        /// </summary>
        public UserAccount LoggedInUserAccount { get; private set; }

        /// <summary>
        /// user's login role if already logged in, otherwise null
        /// </summary>
        public LoginRole? LoggedInLoginRole { get; private set; }

        /// <summary>
        /// Accessing user account by a valid username
        /// </summary>
        /// <param name="username">valid username</param>
        /// <returns>user account if success, otherwise null</returns>
        private static UserAccount GetUserAccount(string username) {
            if (username == null) return null;

            LoginRole loginRole = (LoginRole) (int) (decimal)
                DataAccessLayer.SelectCommand(DataAccessLayer.SelectCommandString(
                    "Login_Role_ID", LoginTable, "Username = :username"),
                    new CommandParameter(":username", username)).Rows[0][0];

            switch (loginRole) {
                case LoginRole.Student:
                    return new Account.Student();
                case LoginRole.Teacher:
                    return new Account.Teacher();
                case LoginRole.School:
                    return new Account.School();
            }

            return null;
        }

        /// <summary>
        /// Check if the user has already logged in
        /// </summary>
        /// <returns>username string if success, otherwise null</returns>
        private static string GetLoggedInUsername() {
            // first check if 'username' variable is in session
            if (HttpContext.Current.Session[UsernameAccessString] != null) {
                string username = (string) HttpContext.Current.Session[UsernameAccessString];

                // now check for the 'username' in the database for additional check
                DataTable loginUserTable = DataAccessLayer.SelectCommand(DataAccessLayer.SelectCommandString(
                    "Username", LoginTable, "Username = :username"),
                    new CommandParameter(":username", username));

                if (1 == loginUserTable.Rows.Count) {
                    return username;
                }
                // clear session state because it is not available in the database
                HttpContext.Current.Session.Clear();
                CurrentInstance.Reset();
            }
            return null;
        }

        /// <summary>
        /// Reset the current logged-in user status in variables
        /// </summary>
        private void Reset() {
            LoggedInUsername = GetLoggedInUsername();
            LoggedInUserAccount = GetUserAccount(LoggedInUsername);
            LoggedInUserName = LoggedInUserAccount?.Name;
            LoggedInLoginRole = LoggedInUserAccount?.LoginRole;
        }

        /// <summary>
        /// Return the current instance of 'UserAuthorization' class
        /// </summary>
        public static UserAuthorization CurrentInstance {
            get {
                if (HttpContext.Current.Items[CurrentInstanceAccessString] == null) {
                    UserAuthorization userAuthorization = new UserAuthorization();
                    HttpContext.Current.Items[CurrentInstanceAccessString] = userAuthorization;
                    userAuthorization.Reset();
                }
                return HttpContext.Current.Items[CurrentInstanceAccessString] as UserAuthorization;
            }
        }

        /// <summary>
        /// Attempt to user signup
        /// </summary>
        /// <param name="name">login user's name</param>
        /// <param name="username">login username</param>
        /// <param name="emailAddress">login Email address</param>
        /// <param name="password">login password</param>
        /// <param name="loginRoleID">login role ID (zero based)</param>
        /// <returns>user account if success, otherwise null</returns>
        public static UserAccount SignupUser(string name, string username, string emailAddress, string password,
            int loginRoleID) {
            string userPassword = Encryption.Encrypt(password, emailAddress);
            int userLoginRoleID = loginRoleID + 1;

            if (0 == DataAccessLayer.ProcedureCommand("Signup_User",
                new CommandParameter("P_Name", name),
                new CommandParameter("P_Username", username),
                new CommandParameter("P_Email", emailAddress),
                new CommandParameter("P_Password", userPassword),
                new CommandParameter("P_Login_Role_ID", userLoginRoleID))) {
                // create a session for this user
                HttpContext.Current.Session[UsernameAccessString] = username;
                CurrentInstance.Reset();

                return GetUserAccount(username);
            }

            return null;
        }

        /// <summary>
        /// Attempt to user login
        /// </summary>
        /// <param name="emailAddress">login Email address</param>
        /// <param name="password">login password</param>
        /// <returns>user account if success, otherwise null</returns>
        public static UserAccount LoginUser(string emailAddress, string password) {
            string userPassword = Encryption.Encrypt(password, emailAddress);

            DataTable loginUserTable = DataAccessLayer.SelectCommand(DataAccessLayer.SelectCommandString(
                "Username", LoginTable, "Email = :emailAddress AND Password = :userPassword"),
                new CommandParameter(":emailAddress", emailAddress),
                new CommandParameter(":userPassword", userPassword));

            if (1 == loginUserTable.Rows.Count) {
                //TODO: Implement 'Remember me' feature

                string username = (string) loginUserTable.Rows[0][0];

                // create a session for this user
                HttpContext.Current.Session[UsernameAccessString] = username;
                CurrentInstance.Reset();

                return GetUserAccount(username);
            }
            return null;
        }

        /// <summary>
        /// Attempt to logout user account
        /// </summary>
        public static void Logout() {
            // clear session state to logout
            HttpContext.Current.Session.Clear();
            CurrentInstance.Reset();
        }
    }
}