using System;
using System.Collections.Generic;
using System.Data;
using Schoolerics.AppCode.DataConnector;

namespace Schoolerics.AppCode.Controller.Account
{
    /// <summary>
    /// Contains all necessary interactions for student entity
    /// </summary>
    public class Student : UserAccount
    {
        private const string EnrollCourseTableName = "Enroll_Course_Student";
        private const string ExamResultTableName = "Enroll_Exam_Student";

        public Student() : base(UserAuthorization.CurrentInstance.LoggedInUsername) {
            if (LoginRole != LoginRole.Student) throw new AccessViolationException(ErrorStringLoginRoleMismatch);
        }

        /// <summary>
        /// Get student roll if any, otherwise "N/A"
        /// </summary>
        public string StudentRoll {
            get {
                DataTable dataTable = DataAccessLayer.SelectCommand(DataAccessLayer.SelectCommandString(
                    "Student_Roll", LoginRole.ToString(), "ID = :ID"),
                    new CommandParameter(":ID", ID));

                if (!dataTable.Rows[0].IsNull(0)) {
                    return ((int) (decimal) dataTable.Rows[0][0]).ToString();
                }

                return EmptyField;
            }
        }

        /// <summary>
        /// Get school name if any, otherwise "N/A"
        /// </summary>
        public string SchoolName {
            get {
                DataTable dataTable = DataAccessLayer.SelectCommand(DataAccessLayer.SelectCommandString(
                    "School_ID", LoginRole.ToString(), "ID = :ID"),
                    new CommandParameter(":ID", ID));

                if (!dataTable.Rows[0].IsNull(0)) {
                    int schoolID = (int) (decimal) dataTable.Rows[0][0];
                    return (string) DataAccessLayer.SelectCommand(DataAccessLayer.SelectCommandString(
                        "Name", "School", "ID = :schoolID"),
                        new CommandParameter(":schoolID", schoolID)).Rows[0][0];
                }

                return EmptyField;
            }
        }

        /// <summary>
        /// Get class name if any, otherwise "N/A"
        /// </summary>
        public string ClassName {
            get {
                DataTable dataTable = DataAccessLayer.SelectCommand(DataAccessLayer.SelectCommandString(
                    "Class_ID", LoginRole.ToString(), "ID = :ID"),
                    new CommandParameter(":ID", ID));

                if (!dataTable.Rows[0].IsNull(0)) {
                    int classID = (int) (decimal) dataTable.Rows[0][0];
                    return (string) DataAccessLayer.SelectCommand(DataAccessLayer.SelectCommandString(
                        "Name", "Class", "ID = :classID"),
                        new CommandParameter(":classID", classID)).Rows[0][0];
                }

                return EmptyField;
            }
        }

        /// <summary>
        /// Get date of birth string ("dd/MM/yyyy") if assigned, otherwise "N/A";
        /// Set date of birth as "dd/MM/yyyy" format.
        /// Null or blank or invalid string cannot be set.
        /// </summary>
        public string DateOfBirth {
            get {
                DataTable dataTable = DataAccessLayer.SelectCommand(DataAccessLayer.SelectCommandString(
                    "to_char(Date_of_Birth, :InputDateFormat)", LoginRole.ToString(), "ID = :ID"),
                    new CommandParameter(":InputDateFormat", InputDateFormat),
                    new CommandParameter(":ID", ID));

                if (!dataTable.Rows[0].IsNull(0)) {
                    return (string) dataTable.Rows[0][0];
                }

                return EmptyField;
            }

            set {
                DataAccessLayer.UpdateCommand(DataAccessLayer.UpdateCommandString(
                    LoginRole.ToString(), "Date_of_Birth = to_date(:value, :InputDateFormat)", "ID = :ID"),
                    new CommandParameter(":value", value), new CommandParameter(":InputDateFormat", InputDateFormat),
                    new CommandParameter(":ID", ID));
            }
        }

        /// <summary>
        /// Get a list of course names of enrolled courses
        /// </summary>
        /// <returns>data of 'DataTable' type containing distinct course names of enrolled courses</returns>
        public DataTable GetCourseInfo_CourseName() {
            return DataAccessLayer.SelectCommand(DataAccessLayer.SelectCommandString(
                "DISTINCT Course_Name AS \"Course Name\"", EnrollCourseTableName, "Student_ID = :ID",
                orderByClause: "Course_Name"), new CommandParameter(":ID", ID));
        }

        /// <summary>
        /// Get a list of class names of enrolled courses
        /// </summary>
        /// <returns>data of 'DataTable' type containing distinct class names of enrolled courses</returns>
        public DataTable GetCourseInfo_ClassName() {
            return DataAccessLayer.SelectCommand(DataAccessLayer.SelectCommandString(
                "DISTINCT Class_Name AS \"Class Name\"", EnrollCourseTableName, "Student_ID = :ID",
                orderByClause: "Class_Name"), new CommandParameter(":ID", ID));
        }

        /// <summary>
        /// Get enrolled course info of a specific or all classes, in current school or all schools
        /// </summary>
        /// <param name="courseName">empty string for all courses, or name of the selected course</param>
        /// <param name="inCurrentClass">true for current class, or false for all or the selected class</param>
        /// <param name="className">(if 'inCurrentClass' false) empty string for all classes, or name of the selected class</param>
        /// <param name="inCurrentSchool">true for current school, or false for all schools</param>
        /// <returns>data of 'DataTable' type containing course name, course code, class name, school name, enrolled date</returns>
        public DataTable GetEnrolledCourseInfo(string courseName = "", bool inCurrentClass = true, string className = "",
            bool inCurrentSchool = true) {
            string selectClause = "";
            string whereClause = "Student_ID = :ID";
            List<CommandParameter> commandParameters = new List<CommandParameter>();
            commandParameters.Add(new CommandParameter(":ID", ID));

            if (courseName != "") {
                whereClause += " AND Course_Name = :courseName";
                commandParameters.Add(new CommandParameter(":courseName", courseName));
            }
            else {
                selectClause = "Course_Name AS \"Course Name\", ";
            }
            selectClause += "Course_Code AS \"Course Code\"";

            if (inCurrentClass) {
                DataTable dataTableClassID = DataAccessLayer.SelectCommand(DataAccessLayer.SelectCommandString(
                    "Class_ID", LoginRole.ToString(), "ID = :ID"),
                    new CommandParameter(":ID", ID));

                int classID = 0;
                if (!dataTableClassID.Rows[0].IsNull(0)) {
                    classID = (int) (decimal) dataTableClassID.Rows[0][0];
                }

                whereClause += " AND Class_ID = :classID";
                commandParameters.Add(new CommandParameter(":classID", classID));
            }
            else if (className != "") {
                whereClause += " AND Class_Name = :className";
                commandParameters.Add(new CommandParameter(":className", className));
            }
            else {
                selectClause += ", Class_Name AS \"Class Name\"";
            }

            if (inCurrentSchool) {
                DataTable dataTableSchoolID = DataAccessLayer.SelectCommand(DataAccessLayer.SelectCommandString(
                    "School_ID", LoginRole.ToString(), "ID = :ID"),
                    new CommandParameter(":ID", ID));

                int schoolID = 0;
                if (!dataTableSchoolID.Rows[0].IsNull(0)) {
                    schoolID = (int) (decimal) dataTableSchoolID.Rows[0][0];
                }

                whereClause += " AND School_ID = :schoolID";
                commandParameters.Add(new CommandParameter(":schoolID", schoolID));
            }
            else {
                selectClause += ", School_Name AS \"School Name\"";
            }

            selectClause += ", (to_char(Enroll_Date, :date_format)) AS \"Enroll Date\"";
            commandParameters.Insert(0, new CommandParameter(":date_format", OutputDateFormat));

            DataTable dataTable = DataAccessLayer.SelectCommand(DataAccessLayer.SelectCommandString(
                selectClause, EnrollCourseTableName, whereClause,
                orderByClause: "Course_Code, (to_char(Enroll_Date, 'yyyy/MM/dd'))"),
                commandParameters.ToArray());
            return dataTable;
        }

        /// <summary>
        /// Get a list of course names of enrolled exams of courses
        /// </summary>
        /// <returns>data of 'DataTable' type containing distinct course names of enrolled exams of courses</returns>
        public DataTable GetExamInfo_CourseName() {
            return DataAccessLayer.SelectCommand(DataAccessLayer.SelectCommandString(
                "DISTINCT Course_Name AS \"Course Name\"", ExamResultTableName, "Student_ID = :ID",
                orderByClause: "Course_Name"), new CommandParameter(":ID", ID));
        }

        /// <summary>
        /// Get a list of class names of enrolled exams of courses
        /// </summary>
        /// <returns>data of 'DataTable' type containing distinct class names of enrolled exams of courses</returns>
        public DataTable GetExamInfo_ClassName() {
            return DataAccessLayer.SelectCommand(DataAccessLayer.SelectCommandString(
                "DISTINCT Class_Name AS \"Class Name\"", ExamResultTableName, "Student_ID = :ID",
                orderByClause: "Class_Name"), new CommandParameter(":ID", ID));
        }

        /// <summary>
        /// Get exam routine info of a specific or all courses, upcoming exams or all exams,
        /// a specific or all classes, in current school or all schools
        /// </summary>
        /// <param name="courseName">empty string for all exams, or course name of the selected exams</param>
        /// <param name="onlyUpcomingExam">true for only upcoming exam, or false for all exams</param>
        /// <param name="inCurrentClass">true for current class, or false for all or the selected class</param>
        /// <param name="className">(if 'inCurrentClass' false) empty string for all classes, or name of the selected class</param>
        /// <param name="inCurrentSchool">true for current school, or false for all schools</param>
        /// <returns>
        /// data of 'DataTable' type containing exam ID, course name, course code, class name, school name, date, time,
        /// duration, syllabus, total marks
        /// </returns>
        public DataTable GetExamRoutineInfo(string courseName = "", bool onlyUpcomingExam = true,
            bool inCurrentClass = true, string className = "", bool inCurrentSchool = true) {
            string selectClause = "Exam_ID AS \"Exam ID\"";
            string whereClause = "Student_ID = :ID";
            List<CommandParameter> commandParameters = new List<CommandParameter>();
            commandParameters.Add(new CommandParameter(":ID", ID));

            if (courseName != "") {
                whereClause += " AND Course_Name = :courseName";
                commandParameters.Add(new CommandParameter(":courseName", courseName));
            }
            else {
                selectClause += ", Course_Name AS \"Course Name\"";
            }

            selectClause += ", Course_Code AS \"Course Code\"";

            if (onlyUpcomingExam) {
                whereClause += " AND to_char(Date_Time, 'yyyy/MM/dd') >= to_char(sysdate, 'yyyy/MM/dd')";
            }

            if (inCurrentClass) {
                DataTable dataTableClassID = DataAccessLayer.SelectCommand(DataAccessLayer.SelectCommandString(
                    "Class_ID", LoginRole.ToString(), "ID = :ID"),
                    new CommandParameter(":ID", ID));

                int classID = 0;
                if (!dataTableClassID.Rows[0].IsNull(0)) {
                    classID = (int) (decimal) dataTableClassID.Rows[0][0];
                }

                whereClause += " AND Class_ID = :classID";
                commandParameters.Add(new CommandParameter(":classID", classID));
            }
            else if (className != "") {
                whereClause += " AND Class_Name = :className";
                commandParameters.Add(new CommandParameter(":className", className));
            }
            else {
                selectClause += ", Class_Name AS \"Class Name\"";
            }

            if (inCurrentSchool) {
                DataTable dataTableSchoolID = DataAccessLayer.SelectCommand(DataAccessLayer.SelectCommandString(
                    "School_ID", LoginRole.ToString(), "ID = :ID"),
                    new CommandParameter(":ID", ID));

                int schoolID = 0;
                if (!dataTableSchoolID.Rows[0].IsNull(0)) {
                    schoolID = (int) (decimal) dataTableSchoolID.Rows[0][0];
                }

                whereClause += " AND School_ID = :schoolID";
                commandParameters.Add(new CommandParameter(":schoolID", schoolID));
            }
            else {
                selectClause += ", School_Name AS \"School Name\"";
            }

            selectClause += ", (to_char(Date_Time, :date_format)) AS \"Date\", " +
                            "(to_char(Date_Time, :time_format)) AS \"Time\", " +
                            "to_char(to_date(Duration,'sssss'), 'hh24:mi:ss') AS \"Duration\", " +
                            "Syllabus AS \"Syllabus\", Total_Marks AS \"Total Marks\"";
            commandParameters.Insert(0, new CommandParameter(":date_format", OutputDateFormat));
            commandParameters.Insert(1, new CommandParameter(":time_format", OutputTimeFormat));

            DataTable dataTable = DataAccessLayer.SelectCommand(DataAccessLayer.SelectCommandString(
                selectClause, ExamResultTableName, whereClause,
                orderByClause: "Course_Code, (to_char(Date_Time, 'yyyy/MM/dd'))"),
                commandParameters.ToArray());
            return dataTable;
        }

        /// <summary>
        /// Get a list of exam IDs of exams of enrolled courses
        /// </summary>
        /// <returns>data of 'DataTable' type containing distinct exam IDs of exams of enrolled courses</returns>
        public DataTable GetExamResultInfo_ExamID() {
            return DataAccessLayer.SelectCommand(DataAccessLayer.SelectCommandString(
                "DISTINCT Exam_ID AS \"Exam ID\"", ExamResultTableName, "Student_ID = :ID",
                orderByClause: "Exam_ID"), new CommandParameter(":ID", ID));
        }

        /// <summary>
        /// Get exam result info of an exam ID or all exams, a specific or all courses,
        /// upcoming exams or all exams, a specific or all classes, in current school or all schools
        /// </summary>
        /// <param name="examID">0 for all exams, or exam ID of the selected exam</param>
        /// <param name="courseName">empty string for all exams, or course name of the selected exams</param>
        /// <param name="inCurrentClass">true for current class, or false for all or the selected class</param>
        /// <param name="className">(if 'inCurrentClass' false) empty string for all classes, or name of the selected class</param>
        /// <param name="inCurrentSchool">true for current school, or false for all schools</param>
        /// <returns>
        /// data of 'DataTable' type containing exam ID, course name, course code, class name, school name, date, time,
        /// total marks, obtained marks
        /// </returns>
        public DataTable GetExamResultInfo(int examID = 0, string courseName = "", bool inCurrentClass = true,
            string className = "", bool inCurrentSchool = true) {
            string selectClause = "Exam_ID AS \"Exam ID\"";
            string whereClause = "Student_ID = :ID";
            List<CommandParameter> commandParameters = new List<CommandParameter>();
            commandParameters.Add(new CommandParameter(":ID", ID));

            if (examID != 0) {
                whereClause += " AND Exam_ID = :examID";
                commandParameters.Add(new CommandParameter(":examID", examID));
            }

            if (courseName != "") {
                whereClause += " AND Course_Name = :courseName";
                commandParameters.Add(new CommandParameter(":courseName", courseName));
            }
            else {
                selectClause += ", Course_Name AS \"Course Name\"";
            }

            selectClause += ", Course_Code AS \"Course Code\"";

            whereClause += " AND to_char(Date_Time, 'yyyy/MM/dd') <= to_char(sysdate, 'yyyy/MM/dd')";

            if (inCurrentClass) {
                DataTable dataTableClassID = DataAccessLayer.SelectCommand(DataAccessLayer.SelectCommandString(
                    "Class_ID", LoginRole.ToString(), "ID = :ID"),
                    new CommandParameter(":ID", ID));

                int classID = 0;
                if (!dataTableClassID.Rows[0].IsNull(0)) {
                    classID = (int) (decimal) dataTableClassID.Rows[0][0];
                }

                whereClause += " AND Class_ID = :classID";
                commandParameters.Add(new CommandParameter(":classID", classID));
            }
            else if (className != "") {
                whereClause += " AND Class_Name = :className";
                commandParameters.Add(new CommandParameter(":className", className));
            }
            else {
                selectClause += ", Class_Name AS \"Class Name\"";
            }

            if (inCurrentSchool) {
                DataTable dataTableSchoolID = DataAccessLayer.SelectCommand(DataAccessLayer.SelectCommandString(
                    "School_ID", LoginRole.ToString(), "ID = :ID"),
                    new CommandParameter(":ID", ID));

                int schoolID = 0;
                if (!dataTableSchoolID.Rows[0].IsNull(0)) {
                    schoolID = (int) (decimal) dataTableSchoolID.Rows[0][0];
                }

                whereClause += " AND School_ID = :schoolID";
                commandParameters.Add(new CommandParameter(":schoolID", schoolID));
            }
            else {
                selectClause += ", School_Name AS \"School Name\"";
            }

            selectClause += ", (to_char(Date_Time, :date_format)) AS \"Date\", " +
                            "(to_char(Date_Time, :time_format)) AS \"Time\", " +
                            "Total_Marks AS \"Total Marks\", Obtained_Marks AS \"Obtained Marks\"";
            commandParameters.Insert(0, new CommandParameter(":date_format", OutputDateFormat));
            commandParameters.Insert(1, new CommandParameter(":time_format", OutputTimeFormat));

            DataTable dataTable = DataAccessLayer.SelectCommand(DataAccessLayer.SelectCommandString(
                selectClause, ExamResultTableName, whereClause,
                orderByClause: "Course_Code, (to_char(Date_Time, 'yyyy/MM/dd'))"),
                commandParameters.ToArray());
            return dataTable;
        }
    }
}