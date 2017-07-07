using System;
using System.Collections.Generic;
using System.Data;
using Schoolerics.AppCode.DataConnector;

namespace Schoolerics.AppCode.Controller.Account
{
    /// <summary>
    /// Contains all necessary interactions for school entity
    /// </summary>
    public class School : UserAccount
    {
        public School() : base(UserAuthorization.CurrentInstance.LoggedInUsername) {
            if (LoginRole != LoginRole.School) throw new AccessViolationException(ErrorStringLoginRoleMismatch);
        }

        /// <summary>
        /// Get description if assigned, otherwise "N/A"
        /// </summary>
        public string Description {
            get {
                DataTable dataTable = DataAccessLayer.SelectCommand(DataAccessLayer.SelectCommandString(
                    "Description", LoginRole.ToString(), "ID = :ID"),
                    new CommandParameter(":ID", ID));

                if (!dataTable.Rows[0].IsNull(0)) {
                    return (string) dataTable.Rows[0][0];
                }

                return EmptyField;
            }
            set {
                DataAccessLayer.UpdateCommand(DataAccessLayer.UpdateCommandString(
                    LoginRole.ToString(), "Description = :value", "ID = :ID"),
                    new CommandParameter(":value", value), new CommandParameter(":ID", ID));
            }
        }

        /// <summary>
        /// Get url if assigned, otherwise "N/A"
        /// </summary>
        public string Url {
            get {
                DataTable dataTable = DataAccessLayer.SelectCommand(DataAccessLayer.SelectCommandString(
                    "Url", LoginRole.ToString(), "ID = :ID"),
                    new CommandParameter(":ID", ID));

                if (!dataTable.Rows[0].IsNull(0)) {
                    return (string) dataTable.Rows[0][0];
                }

                return EmptyField;
            }
            set {
                DataAccessLayer.UpdateCommand(DataAccessLayer.UpdateCommandString(
                    LoginRole.ToString(), "Url = :value", "ID = :ID"),
                    new CommandParameter(":value", value), new CommandParameter(":ID", ID));
            }
        }

        /// <summary>
        /// Get headmaster name if assigned, otherwise "N/A"
        /// </summary>
        /// <returns>headmaster name if assigned, otherwise "N/A"</returns>
        public string GetHeadmasterName() {
            DataTable dataTable = DataAccessLayer.SelectCommand(DataAccessLayer.SelectCommandString(
                "Headmaster_ID", LoginRole.ToString(), "ID = :ID"),
                new CommandParameter(":ID", ID));

            if (!dataTable.Rows[0].IsNull(0)) {
                int headmasterID = (int) (decimal) dataTable.Rows[0][0];
                return (string) DataAccessLayer.SelectCommand(DataAccessLayer.SelectCommandString(
                    "Name", "Teacher", "ID = :headmasterID"),
                    new CommandParameter(":headmasterID", headmasterID)).Rows[0][0];
            }

            return EmptyField;
        }

        //TODO: Add a method for viewing specific teachers by course, class
        /// <summary>
        /// Get a list of teachers in school
        /// </summary>
        /// <returns>data of 'DataTable' type containing teacher ID, name, address and contact</returns>
        public DataTable GetTeacherInfo() {
            return DataAccessLayer.SelectCommand(DataAccessLayer.SelectCommandString(
                "ID, Name AS \"Teacher Name\", Address AS \"Address\", Contact AS \"Contact\"",
                "Teacher", "School_ID = :ID", orderByClause: "Name"), new CommandParameter(":ID", ID));
        }

        /// <summary>
        /// Check if teacher ID is authorized in school (must be done for assigning as headmaster)
        /// </summary>
        /// <param name="teacherID">teacher ID</param>
        /// <returns>true if teacher ID is authorized in school, otherwise false</returns>
        private bool IsAuthorizedTeacherID(int teacherID) {
            DataTable dataTableTeacherID = DataAccessLayer.SelectCommand(DataAccessLayer.SelectCommandString(
                "ID AS \"Teacher ID\"", "Teacher", "School_ID = :ID AND ID = :teacherID"),
                new CommandParameter(":ID", ID), new CommandParameter(":teacherID", teacherID));

            return (1 == dataTableTeacherID.Rows.Count);
        }

        /// <summary>
        /// Assign headmaster by teacher ID
        /// </summary>
        /// <param name="teacherID">teacher ID</param>
        /// <returns>0 if success, otherwise the error code</returns>
        public int AssignHeadmaster(int teacherID) {
            if (!IsAuthorizedTeacherID(teacherID)) return -1;

            return DataAccessLayer.UpdateCommand(DataAccessLayer.UpdateCommandString(
                "School", "Headmaster_ID = :teacherID", "ID = :ID"),
                new CommandParameter(":teacherID", teacherID), new CommandParameter(":ID", ID));
        }

        //TODO: Add a method for viewing specific students by course, class
        /// <summary>
        /// Get a list of students in school
        /// </summary>
        /// <returns>data of 'DataTable' type containing student ID, name, address and contact</returns>
        public DataTable GetStudentInfo() {
            return DataAccessLayer.SelectCommand(DataAccessLayer.SelectCommandString(
                "ID AS \"Student ID\", Name AS \"Student Name\", Address AS \"Address\", Contact AS \"Contact\"",
                "Student", "School_ID = :ID", orderByClause: "Name"), new CommandParameter(":ID", ID));
        }

        /// <summary>
        /// Add student by student ID
        /// </summary>
        /// <param name="studentID">student ID</param>
        /// <param name="studentRoll">student's roll</param>
        /// <param name="className">student's class name</param>
        /// <returns>0 if success, otherwise the error code</returns>
        public int AddStudentByStudentID(int studentID, string studentRoll, string className) {
            DataTable dataTable = DataAccessLayer.SelectCommand(DataAccessLayer.SelectCommandString(
                "ID", "Class", "Name = :className"), new CommandParameter(":className", className));

            if (1 != dataTable.Rows.Count) return -1;

            int classID = (int) (decimal) dataTable.Rows[0][0];

            return DataAccessLayer.UpdateCommand(DataAccessLayer.UpdateCommandString(
                "Student", "School_ID = :ID, Student_Roll = :studentRoll, Class_ID = :classID", "ID = :studentID"),
                new CommandParameter(":ID", ID), new CommandParameter(":studentRoll", studentRoll),
                new CommandParameter(":classID", classID), new CommandParameter(":studentID", studentID));
        }

        /// <summary>
        /// Add student by student's username
        /// </summary>
        /// <param name="username">student's username</param>
        /// <param name="studentRoll">student's roll</param>
        /// <param name="className">student's class name</param>
        /// <returns>0 if success, otherwise the error code</returns>
        public int AddStudentByUsername(string username, string studentRoll, string className) {
            DataTable dataTable = DataAccessLayer.SelectCommand(DataAccessLayer.SelectCommandString(
                "Get_Student_ID_By_Username(:username)"), new CommandParameter(":username", username));

            if (1 != dataTable.Rows.Count) return -1;

            int studentID = (int) (decimal) dataTable.Rows[0][0];

            dataTable = DataAccessLayer.SelectCommand(DataAccessLayer.SelectCommandString(
                "ID", "Class", "Name = :className"), new CommandParameter(":className", className));

            if (1 != dataTable.Rows.Count) return -1;

            int classID = (int) (decimal) dataTable.Rows[0][0];

            return DataAccessLayer.UpdateCommand(DataAccessLayer.UpdateCommandString(
                "Student", "School_ID = :ID, Student_Roll = :studentRoll, Class_ID = :classID", "ID = :studentID"),
                new CommandParameter(":ID", ID), new CommandParameter(":studentRoll", studentRoll),
                new CommandParameter(":classID", classID), new CommandParameter(":studentID", studentID));
        }

        /// <summary>
        /// Add student by student's email address
        /// </summary>
        /// <param name="email">student's email address</param>
        /// <param name="studentRoll">student's roll</param>
        /// <param name="className">student's class name</param>
        /// <returns>0 if success, otherwise the error code</returns>
        public int AddStudentByEmail(string email, string studentRoll, string className) {
            DataTable dataTable = DataAccessLayer.SelectCommand(DataAccessLayer.SelectCommandString(
                "Get_Student_ID_By_Email(:email)"), new CommandParameter(":email", email));

            if (1 != dataTable.Rows.Count) return -1;

            int studentID = (int) (decimal) dataTable.Rows[0][0];

            dataTable = DataAccessLayer.SelectCommand(DataAccessLayer.SelectCommandString(
                "ID", "Class", "Name = :className"), new CommandParameter(":className", className));

            if (1 != dataTable.Rows.Count) return -1;

            int classID = (int) (decimal) dataTable.Rows[0][0];

            return DataAccessLayer.UpdateCommand(DataAccessLayer.UpdateCommandString(
                "Student", "School_ID = :ID, Student_Roll = :studentRoll, Class_ID = :classID", "ID = :studentID"),
                new CommandParameter(":ID", ID), new CommandParameter(":studentRoll", studentRoll),
                new CommandParameter(":classID", classID), new CommandParameter(":studentID", studentID));
        }

        /// <summary>
        /// Add teacher by teacher ID
        /// </summary>
        /// <param name="teacherID">teacher ID</param>
        /// <returns>0 if success, otherwise the error code</returns>
        public int AddTeacherByTeacherID(int teacherID) {
            return DataAccessLayer.UpdateCommand(DataAccessLayer.UpdateCommandString(
                "Teacher", "School_ID = :ID", "ID = :teacherID"),
                new CommandParameter(":ID", ID), new CommandParameter(":teacherID", teacherID));
        }

        /// <summary>
        /// Add teacher by teacher's username
        /// </summary>
        /// <param name="username">teacher's username</param>
        /// <returns>0 if success, otherwise the error code</returns>
        public int AddTeacherByUsername(string username) {
            DataTable dataTableTeacherID = DataAccessLayer.SelectCommand(DataAccessLayer.SelectCommandString(
                "Get_Teacher_ID_By_Username(:username)"), new CommandParameter(":username", username));

            if (1 != dataTableTeacherID.Rows.Count) return -1;

            int teacherID = (int) (decimal) dataTableTeacherID.Rows[0][0];
            return DataAccessLayer.UpdateCommand(DataAccessLayer.UpdateCommandString(
                "Teacher", "School_ID = :ID", "ID = :teacherID"),
                new CommandParameter(":ID", ID), new CommandParameter(":teacherID", teacherID));
        }

        /// <summary>
        /// Add teacher by teacher's email address
        /// </summary>
        /// <param name="email">teacher's email address</param>
        /// <returns>0 if success, otherwise the error code</returns>
        public int AddTeacherByEmail(string email) {
            DataTable dataTableTeacherID = DataAccessLayer.SelectCommand(DataAccessLayer.SelectCommandString(
                "Get_Teacher_ID_By_Email(:email)"), new CommandParameter(":email", email));

            if (1 != dataTableTeacherID.Rows.Count) return -1;

            int teacherID = (int) (decimal) dataTableTeacherID.Rows[0][0];
            return DataAccessLayer.UpdateCommand(DataAccessLayer.UpdateCommandString(
                "Teacher", "School_ID = :ID", "ID = :teacherID"),
                new CommandParameter(":ID", ID), new CommandParameter(":teacherID", teacherID));
        }

        //TODO: Add feature to view course class info by catagory (make it suitable for dynamic dropdown list)
        /// <summary>
        /// Get course class info of school
        /// </summary>
        /// <returns>data of 'DataTable' type containing course name, class name and course code</returns>
        public DataTable GetCourseClassInfo() {
            return DataAccessLayer.SelectCommand(DataAccessLayer.SelectCommandString(
                "Class_Name AS \"Class Name\", Course_Name AS \"Course Name\", Course_Code AS \"Course Code\"",
                "Course_Class_School_Name", "School_ID = :ID", orderByClause: "Class_Name, Course_Name, Course_Code"),
                new CommandParameter("School_ID", ID));
        }

        /// <summary>
        /// Assign course, class, course code into the school
        /// </summary>
        /// <param name="courseName">course name</param>
        /// <param name="className">class name</param>
        /// <param name="courseCode">course code</param>
        /// <returns>0 if success, otherwise the error code</returns>
        public int AssignCourseClass(string courseName, string className, string courseCode = "") {
            List<CommandParameter> commandParameters = new List<CommandParameter>();
            commandParameters.Add(new CommandParameter("P_School_ID", ID));
            commandParameters.Add(new CommandParameter("P_Course_Name", courseName));
            commandParameters.Add(new CommandParameter("P_Class_Name", className));
            if (courseCode != "") commandParameters.Add(new CommandParameter("Course_Code", courseCode));
            return DataAccessLayer.ProcedureCommand("Add_Course_Class", commandParameters.ToArray());
        }

        //TODO: Add a method for viewing specific student enrolment by course, class, student roll
        /// <summary>
        /// Get student course enrolment info of school
        /// </summary>
        /// <returns>
        /// data of 'DataTable' type containing student ID, name, roll, class, course, course code, enroll date, current
        /// class
        /// </returns>
        public DataTable GetStudentCourseInfo() {
            return DataAccessLayer.SelectCommand(DataAccessLayer.SelectCommandString(
                "Student_ID AS \"Student ID\", Student_Name AS \"Student Name\", Student_Roll AS \"Student Roll\", " +
                "Class_Name AS \"Class Name\", Course_Name AS \"Course Name\", Course_Code AS \"Course Code\", " +
                "Enroll_Date AS\"Enroll Date\", Current_Class_Name AS \"Current Class\"",
                "Enroll_Course_Student", "School_ID = :ID",
                orderByClause: "Student_ID, Class_Name, Course_Name, Course_Code"),
                new CommandParameter("School_ID", ID));
        }

        /// <summary>
        /// Assign student into course
        /// </summary>
        /// <param name="studentEmail">student's email address</param>
        /// <param name="courseName">course name</param>
        /// <param name="className">class name</param>
        /// <param name="courseCode">course code</param>
        /// <returns>0 if success, otherwise the error code</returns>
        public int AssignStudentCourse(string studentEmail, string courseName, string className, string courseCode = "") {
            DataTable dataTable = DataAccessLayer.SelectCommand(DataAccessLayer.SelectCommandString(
                "Get_Student_ID_By_Email(:teacherEmail)"), new CommandParameter(":teacherEmail", studentEmail));

            if (1 != dataTable.Rows.Count) return -1;

            int studentID = (int) (decimal) dataTable.Rows[0][0];

            dataTable = DataAccessLayer.SelectCommand(DataAccessLayer.SelectCommandString(
                "School_ID", "Student", "ID = :studentID"), new CommandParameter(":studentID", studentID));

            if (1 != dataTable.Rows.Count) return -1;

            int schoolID = (int) (decimal) dataTable.Rows[0][0];

            if (schoolID != ID) return -1;

            string whereClause = "Course_Name = :courseName AND Class_Name = :className AND School_ID = :ID";
            List<CommandParameter> commandParameters = new List<CommandParameter>();

            commandParameters.Add(new CommandParameter(":courseName", courseName));
            commandParameters.Add(new CommandParameter(":className", className));
            commandParameters.Add(new CommandParameter(":ID", ID));

            if (courseCode != "") {
                whereClause += " AND Course_Code = :courseCode";
                commandParameters.Add(new CommandParameter(":courseCode", courseCode));
            }
            else {
                whereClause += " AND Course_Code IS NULL";
            }

            dataTable = DataAccessLayer.SelectCommand(DataAccessLayer.SelectCommandString(
                "Course_Class_School_ID", "Course_Class_School_Name", whereClause),
                commandParameters.ToArray());

            if (1 != dataTable.Rows.Count) return -1;

            int courseClassSchoolID = (int) (decimal) dataTable.Rows[0][0];

            return DataAccessLayer.InsertCommand_AllColumnAutoID(
                "Enroll_Student", ":courseClassSchoolID, :studentID, sysdate",
                new CommandParameter(":courseClassSchoolID", courseClassSchoolID),
                new CommandParameter(":studentID", studentID));
        }

        //TODO: Add a method for viewing specific teacher enrolment by course, class
        /// <summary>
        /// Get teacher course enrolment info of school
        /// </summary>
        /// <returns>data of 'DataTable' type containing teacher ID, name, class, course, course code, enroll date, end date</returns>
        public DataTable GetTeacherCourseInfo() {
            return DataAccessLayer.SelectCommand(DataAccessLayer.SelectCommandString(
                "Teacher_ID AS \"Teacher ID\", Teacher_Name AS \"Teacher Name\", Class_Name AS \"Class Name\", " +
                "Course_Name AS \"Course Name\", Course_Code AS \"Course Code\", Enroll_Date AS\"Enroll Date\", End_Date AS \"End Date\"",
                "Enroll_Course_Teacher", "School_ID = :ID",
                orderByClause: "Teacher_ID, Class_Name, Course_Name, Course_Code"),
                new CommandParameter("School_ID", ID));
        }

        /// <summary>
        /// Assign teacher into course
        /// </summary>
        /// <param name="teacherEmail">student's email address</param>
        /// <param name="courseName">course name</param>
        /// <param name="className">class name</param>
        /// <param name="courseCode">course code</param>
        /// <returns>0 if success, otherwise the error code</returns>
        public int AssignTeacherCourse(string teacherEmail, string courseName, string className, string courseCode = "") {
            DataTable dataTable = DataAccessLayer.SelectCommand(DataAccessLayer.SelectCommandString(
                "Get_Teacher_ID_By_Email(:teacherEmail)"), new CommandParameter(":teacherEmail", teacherEmail));

            if (1 != dataTable.Rows.Count) return -1;

            int teacherID = (int) (decimal) dataTable.Rows[0][0];

            dataTable = DataAccessLayer.SelectCommand(DataAccessLayer.SelectCommandString(
                "School_ID", "Teacher", "ID = :teacherID"), new CommandParameter(":teacherID", teacherID));

            if (1 != dataTable.Rows.Count) return -1;

            int schoolID = (int) (decimal) dataTable.Rows[0][0];

            if (schoolID != ID) return -1;

            string whereClause = "Course_Name = :courseName AND Class_Name = :className AND School_ID = :ID";
            List<CommandParameter> commandParameters = new List<CommandParameter>();

            commandParameters.Add(new CommandParameter(":courseName", courseName));
            commandParameters.Add(new CommandParameter(":className", className));
            commandParameters.Add(new CommandParameter(":ID", ID));

            if (courseCode != "") {
                whereClause += " AND Course_Code = :courseCode";
                commandParameters.Add(new CommandParameter(":courseCode", courseCode));
            }
            else {
                whereClause += " AND Course_Code IS NULL";
            }

            dataTable = DataAccessLayer.SelectCommand(DataAccessLayer.SelectCommandString(
                "Course_Class_School_ID", "Course_Class_School_Name", whereClause),
                commandParameters.ToArray());

            if (1 != dataTable.Rows.Count) return -1;

            int courseClassSchoolID = (int) (decimal) dataTable.Rows[0][0];

            return DataAccessLayer.InsertCommand_AllColumnAutoID(
                "Enroll_Teacher", ":courseClassSchoolID, :teacherID, sysdate",
                new CommandParameter(":courseClassSchoolID", courseClassSchoolID),
                new CommandParameter(":teacherID", teacherID));
        }
    }
}