using System;
using System.Data;
using Schoolerics.AppCode.DataConnector;

namespace Schoolerics.AppCode.Controller.Account
{
    /// <summary>
    /// Base class for user account
    /// </summary>
    public class UserAccount
    {
        private const string LoginTable = "Login";

        public readonly LoginRole LoginRole;
        public readonly string Name;

        protected readonly int ID;
        protected readonly int LoginID;

        protected const string EmptyField = "N/A";

        protected const string InputDateFormat = "dd/MM/yyyy";
        protected const string OutputDateFormat = "dd fmMonth, yyyy";
        protected const string InputTimeFormat = "HH24:MI:SS";
        protected const string OutputTimeFormat = "HH:MI:SS AM";
        protected const string InputDateTimeFormat = "dd/MM/yyyy HH24:MI:SS";
        protected const string OutputDateTimeFormat = "dd/MM/yyyy HH:MI:SS AM";

        protected const string ErrorStringNoLogin =
            "ERROR: Unauthorized access to a user account.\nLogin first before accessing the account.";

        protected const string ErrorStringLoginRoleMismatch =
            "ERROR: Unauthorized access to a user account.\nLogin role does not match with the class instance.";

        protected UserAccount(string username) {
            if (username == null) throw new AccessViolationException(ErrorStringNoLogin);

            LoginRole = (LoginRole) (int) (decimal) DataAccessLayer.SelectCommand(DataAccessLayer.SelectCommandString(
                "Login_Role_ID", LoginTable, "Username = :username"),
                new CommandParameter(":username", username)).Rows[0][0];

            LoginID = (int) (decimal) DataAccessLayer.SelectCommand(DataAccessLayer.SelectCommandString(
                "ID", LoginTable, "Username = :username"),
                new CommandParameter(":username", username)).Rows[0][0];

            ID = (int) (decimal) DataAccessLayer.SelectCommand(DataAccessLayer.SelectCommandString(
                "ID", LoginRole.ToString(), "Login_ID = :LoginID"),
                new CommandParameter(":LoginID", LoginID)).Rows[0][0];

            Name = (string) DataAccessLayer.SelectCommand(DataAccessLayer.SelectCommandString(
                "Name", LoginRole.ToString(), "ID = :ID"),
                new CommandParameter(":ID", ID)).Rows[0][0];
        }

        //TODO: Add method for profile picture

        /// <summary>
        /// Get address if assigned, otherwise "N/A"
        /// </summary>
        public string Address {
            get {
                DataTable dataTable = DataAccessLayer.SelectCommand(DataAccessLayer.SelectCommandString(
                    "Address", LoginRole.ToString(), "ID = :ID"),
                    new CommandParameter(":ID", ID));

                if (!dataTable.Rows[0].IsNull(0)) {
                    return (string) dataTable.Rows[0][0];
                }

                return EmptyField;
            }
            set {
                DataAccessLayer.UpdateCommand(DataAccessLayer.UpdateCommandString(
                    LoginRole.ToString(), "Address = :value", "ID = :ID"),
                    new CommandParameter(":value", value), new CommandParameter(":ID", ID));
            }
        }

        /// <summary>
        /// Get contact if assigned, otherwise "N/A"
        /// </summary>
        public string Contact {
            get {
                DataTable dataTable = DataAccessLayer.SelectCommand(DataAccessLayer.SelectCommandString(
                    "Contact", LoginRole.ToString(), "ID = :ID"),
                    new CommandParameter(":ID", ID));

                if (!dataTable.Rows[0].IsNull(0)) {
                    return (string) dataTable.Rows[0][0];
                }

                return EmptyField;
            }
            set {
                DataAccessLayer.UpdateCommand(DataAccessLayer.UpdateCommandString(
                    LoginRole.ToString(), "Contact = :value", "ID = :ID"),
                    new CommandParameter(":value", value), new CommandParameter(":ID", ID));
            }
        }
    }
}