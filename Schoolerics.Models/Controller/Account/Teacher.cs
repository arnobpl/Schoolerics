using System;
using System.Collections.Generic;
using System.Data;
using Schoolerics.AppCode.DataConnector;

namespace Schoolerics.AppCode.Controller.Account
{
    /// <summary>
    /// Contains all necessary interactions for teacher entity
    /// </summary>
    public class Teacher : UserAccount
    {
        private const string EnrollCourseTableName = "Enroll_Course_Teacher";
        private const string ExamResultTableNameTeacher = "Enroll_Exam_Teacher";
        private const string ExamResultTableNameStudent = "Enroll_Exam_Student";

        public Teacher() : base(UserAuthorization.CurrentInstance.LoggedInUsername) {
            if (LoginRole != LoginRole.Teacher) throw new AccessViolationException(ErrorStringLoginRoleMismatch);
        }

        /// <summary>
        /// Get school name if any, otherwise "N/A"
        /// </summary>
        public string SchoolName {
            get {
                DataTable dataTable = DataAccessLayer.SelectCommand(
                    DataAccessLayer.SelectCommandString("School_ID", LoginRole.ToString(), "ID = :ID"),
                    new CommandParameter(":ID", ID));

                if (!dataTable.Rows[0].IsNull(0)) {
                    int schoolID = (int) (decimal) dataTable.Rows[0][0];
                    return (string) DataAccessLayer.SelectCommand(
                        DataAccessLayer.SelectCommandString("Name", "School", "ID = :schoolID"),
                        new CommandParameter(":schoolID", schoolID)).Rows[0][0];
                }

                return EmptyField;
            }
        }

        /// <summary>
        /// Get a list of course names of enrolled courses
        /// </summary>
        /// <returns>data of 'DataTable' type containing distinct course names of enrolled courses</returns>
        public DataTable GetCourseInfo_CourseName() {
            return DataAccessLayer.SelectCommand(DataAccessLayer.SelectCommandString(
                "DISTINCT Course_Name AS \"Course Name\"", EnrollCourseTableName, "Teacher_ID = :ID",
                orderByClause: "Course_Name"),
                new CommandParameter(":ID", ID));
        }

        /// <summary>
        /// Get a list of class names of enrolled courses
        /// </summary>
        /// <returns>data of 'DataTable' type containing distinct class names of enrolled courses</returns>
        public DataTable GetCourseInfo_ClassName() {
            return DataAccessLayer.SelectCommand(DataAccessLayer.SelectCommandString(
                "DISTINCT Class_Name AS \"Class Name\"", EnrollCourseTableName, "Teacher_ID = :ID",
                orderByClause: "Class_Name"),
                new CommandParameter(":ID", ID));
        }

        /// <summary>
        /// Get enrolled course info of a specific or all courses, a specific or all classes,
        /// in current school or all schools
        /// </summary>
        /// <param name="courseName">empty string for all courses, or name of the selected course</param>
        /// <param name="className">empty string for all classes, or name of the selected class</param>
        /// <param name="inCurrentSchool">true for current school, or false for all schools</param>
        /// <param name="onlyCurrentlyActive">true for showing only currently active courses, or false for all courses</param>
        /// <returns>data of 'DataTable' type containing course name, course code, class name, school name, enrolled date</returns>
        public DataTable GetEnrolledCourseInfo(string courseName = "", string className = "",
            bool inCurrentSchool = true, bool onlyCurrentlyActive = true) {
            string selectClause = "";
            string whereClause = "Teacher_ID = :ID";
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

            if (className != "") {
                whereClause += " AND Class_Name = :className";
                commandParameters.Add(new CommandParameter(":className", className));
            }
            else {
                selectClause += ", Class_Name AS \"Class Name\"";
            }

            if (inCurrentSchool) {
                DataTable dataTableSchoolID = DataAccessLayer.SelectCommand(
                    DataAccessLayer.SelectCommandString("School_ID", LoginRole.ToString(), "ID = :ID"),
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

            if (onlyCurrentlyActive) {
                whereClause += " AND End_Date IS NULL";
            }
            else {
                selectClause += ", (to_char(End_Date, :date_format)) AS \"End Date\"";
                commandParameters.Insert(1, new CommandParameter(":date_format", OutputDateFormat));
            }

            DataTable dataTable = DataAccessLayer.SelectCommand(DataAccessLayer.SelectCommandString(
                selectClause, EnrollCourseTableName, whereClause,
                orderByClause: "Course_Code, (to_char(Enroll_Date, 'yyyy/MM/dd'))"),
                commandParameters.ToArray());
            return dataTable;
        }

        /// <summary>
        /// Get a list of course names of enrolled active courses
        /// </summary>
        /// <param name="className">class name, or empty string for all classes</param>
        /// <param name="courseCode">course code, or empty string for all course codes</param>
        /// <returns>data of 'DataTable' type containing distinct course names of enrolled active courses</returns>
        public DataTable GetActiveCourseName(string className = "", string courseCode = "") {
            string whereClause = "Teacher_ID = :ID AND End_Date IS NULL";
            List<CommandParameter> commandParameters = new List<CommandParameter>();
            commandParameters.Add(new CommandParameter(":ID", ID));

            if (className != "") {
                whereClause += " AND Class_Name = :className";
                commandParameters.Add(new CommandParameter(":className", className));
            }

            if (courseCode != "") {
                whereClause += " AND Course_Code = :courseCode";
                commandParameters.Add(new CommandParameter(":courseCode", courseCode));
            }

            return DataAccessLayer.SelectCommand(DataAccessLayer.SelectCommandString(
                "DISTINCT Course_Name AS \"Course Name\"", EnrollCourseTableName,
                whereClause, orderByClause: "Course_Name"), commandParameters.ToArray());
        }

        /// <summary>
        /// Get a list of class names of enrolled active courses
        /// </summary>
        /// <param name="courseName">class name, or empty string for all courses</param>
        /// <param name="courseCode">course code, or empty string for all course codes</param>
        /// <returns>data of 'DataTable' type containing distinct class names of enrolled active courses</returns>
        public DataTable GetActiveClassName(string courseName = "", string courseCode = "") {
            string whereClause = "Teacher_ID = :ID AND End_Date IS NULL";
            List<CommandParameter> commandParameters = new List<CommandParameter>();
            commandParameters.Add(new CommandParameter(":ID", ID));

            if (courseName != "") {
                whereClause += " AND Course_Name = :courseName";
                commandParameters.Add(new CommandParameter(":courseName", courseName));
            }

            if (courseCode != "") {
                whereClause += " AND Course_Code = :courseCode";
                commandParameters.Add(new CommandParameter(":courseCode", courseCode));
            }

            return DataAccessLayer.SelectCommand(DataAccessLayer.SelectCommandString(
                "DISTINCT Class_Name AS \"Class Name\"", EnrollCourseTableName,
                whereClause, orderByClause: "Class_Name"), commandParameters.ToArray());
        }

        /// <summary>
        /// Get a list of course codes of enrolled active courses
        /// </summary>
        /// <param name="courseName">class name, or empty string for all courses</param>
        /// <param name="className">class name, or empty string for all classes</param>
        /// <returns>data of 'DataTable' type containing distinct course codes of enrolled active courses</returns>
        public DataTable GetActiveCourseCode(string courseName = "", string className = "") {
            string whereClause = "Teacher_ID = :ID AND End_Date IS NULL";
            List<CommandParameter> commandParameters = new List<CommandParameter>();
            commandParameters.Add(new CommandParameter(":ID", ID));

            if (courseName != "") {
                whereClause += " AND Course_Name = :courseName";
                commandParameters.Add(new CommandParameter(":courseName", courseName));
            }

            if (className != "") {
                whereClause += " AND Class_Name = :className";
                commandParameters.Add(new CommandParameter(":className", className));
            }

            return DataAccessLayer.SelectCommand(DataAccessLayer.SelectCommandString(
                "DISTINCT Course_Code AS \"Course Code\"", EnrollCourseTableName,
                whereClause, orderByClause: "Course_Code"), commandParameters.ToArray());
        }

        /// <summary>
        /// Create an exam of specific course with date, time, duration, total marks, syllabus
        /// </summary>
        /// <param name="courseName">course name</param>
        /// <param name="className">class name</param>
        /// <param name="courseCode">course code or empty string</param>
        /// <param name="dateTime">date and time string in 'dd/MM/yyyy HH24:MI:SS' format</param>
        /// <param name="durationSecond">duration in second or zero</param>
        /// <param name="totalMarks">total marks or zero</param>
        /// <param name="syllabus">syllabus or empty string</param>
        /// <returns>0 if success, otherwise the error code</returns>
        public int CreateExam(string courseName, string className, string courseCode, string dateTime,
            int durationSecond = 0, int totalMarks = 0, string syllabus = "") {
            const string examTableName = "Exam";

            string whereClause = "Teacher_ID = :ID AND Course_Name = :courseName AND Class_Name = :className";
            List<CommandParameter> commandParameters = new List<CommandParameter>();
            commandParameters.Add(new CommandParameter(":ID", ID));
            commandParameters.Add(new CommandParameter(":courseName", courseName));
            commandParameters.Add(new CommandParameter(":className", className));

            if (courseCode != "") {
                whereClause += " AND Course_Code = :courseCode";
                commandParameters.Add(new CommandParameter(":courseCode", courseCode));
            }
            else {
                whereClause += " AND Course_Code IS NULL";
            }

            whereClause += " AND End_Date IS NULL";

            DataTable dataTable = DataAccessLayer.SelectCommand(DataAccessLayer.SelectCommandString(
                "Course_Class_School_ID", EnrollCourseTableName, whereClause), commandParameters.ToArray());

            commandParameters.Clear();

            if (1 != dataTable.Rows.Count) return -1; // teacher has no eligible courses to create exams

            int courseClassSchoolID = (int) (decimal) dataTable.Rows[0][0];

            string valuesClause = ":courseClassSchoolID";
            commandParameters.Add(new CommandParameter(":courseClassSchoolID", courseClassSchoolID));

            valuesClause += ", to_date(:dateTime, :inputDateTimeFormat)";
            commandParameters.Add(new CommandParameter(":dateTime", dateTime));
            commandParameters.Add(new CommandParameter(":inputDateTimeFormat", InputDateTimeFormat));

            if (durationSecond != 0) {
                valuesClause += ", :durationSecond";
                commandParameters.Add(new CommandParameter(":durationSecond", durationSecond));
            }
            else {
                valuesClause += ", NULL";
            }

            if (totalMarks != 0) {
                valuesClause += ", :totalMarks";
                commandParameters.Add(new CommandParameter(":totalMarks", totalMarks));
            }
            else {
                valuesClause += ", NULL";
            }

            if (syllabus != "") {
                valuesClause += ", :syllabus";
                commandParameters.Add(new CommandParameter(":syllabus", syllabus));
            }
            else {
                valuesClause += ", NULL";
            }

            valuesClause += ", NULL"; // online exam feature has not been currently implemented

            return DataAccessLayer.InsertCommand_AllColumnAutoID(examTableName, valuesClause,
                commandParameters.ToArray());
        }

        /// <summary>
        /// Get a list of course names of exams of enrolled active courses
        /// </summary>
        /// <returns>data of 'DataTable' type containing distinct course names of exams of enrolled active courses</returns>
        public DataTable GetExamRoutine_CourseName() {
            return DataAccessLayer.SelectCommand(DataAccessLayer.SelectCommandString(
                "DISTINCT Course_Name AS \"Course Name\"", ExamResultTableNameTeacher,
                "Teacher_ID = :ID AND End_Date IS NULL",
                orderByClause: "Course_Name"), new CommandParameter(":ID", ID));
        }

        /// <summary>
        /// Get a list of class names of exams of enrolled active courses
        /// </summary>
        /// <returns>data of 'DataTable' type containing distinct class names of exams of enrolled active courses</returns>
        public DataTable GetExamRoutine_ClassName() {
            return DataAccessLayer.SelectCommand(DataAccessLayer.SelectCommandString(
                "DISTINCT Class_Name AS \"Class Name\"", ExamResultTableNameTeacher,
                "Teacher_ID = :ID AND End_Date IS NULL",
                orderByClause: "Class_Name"), new CommandParameter(":ID", ID));
        }

        /// <summary>
        /// Get a list of course codes of exams of enrolled active courses
        /// </summary>
        /// <returns>data of 'DataTable' type containing distinct course codes of exams of enrolled active courses</returns>
        public DataTable GetExamRoutine_CourseCode() {
            return DataAccessLayer.SelectCommand(DataAccessLayer.SelectCommandString(
                "DISTINCT Course_Code AS \"Course Code\"", ExamResultTableNameTeacher,
                "Teacher_ID = :ID AND End_Date IS NULL",
                orderByClause: "Course_Code"), new CommandParameter(":ID", ID));
        }

        /// <summary>
        /// Get a list of exam IDs of exams selected by course name, class name, course code
        /// </summary>
        /// <param name="courseName">empty string for all exams, or course name of the selected exams</param>
        /// <param name="className">empty string for all exams, or class name of the selected exams</param>
        /// <param name="courseCode">empty string for all exams, or course code of the selected exams</param>
        /// <returns>data of 'DataTable' type containing distinct exam IDs of exams of enrolled active courses</returns>
        public DataTable GetExamRoutine_ExamID(string courseName = "", string className = "", string courseCode = "") {
            string whereClause = "Teacher_ID = :ID";
            List<CommandParameter> commandParameters = new List<CommandParameter>();
            commandParameters.Add(new CommandParameter(":ID", ID));

            if (courseName != "") {
                whereClause += " AND Course_Name = :courseName";
                commandParameters.Add(new CommandParameter(":courseName", courseName));
            }

            if (className != "") {
                whereClause += " AND Class_Name = :className";
                commandParameters.Add(new CommandParameter(":className", className));
            }

            if (courseCode != "") {
                whereClause += " AND Course_Code = :courseCode";
                commandParameters.Add(new CommandParameter(":courseCode", courseCode));
            }

            whereClause += " AND End_Date IS NULL";

            return DataAccessLayer.SelectCommand(DataAccessLayer.SelectCommandString(
                "DISTINCT Exam_ID AS \"Exam ID\"", ExamResultTableNameTeacher,
                whereClause, orderByClause: "Exam_ID"), commandParameters.ToArray());
        }

        /// <summary>
        /// Check if exam ID is authorized to access
        /// </summary>
        /// <param name="examID">exam ID</param>
        /// <returns>true if exam ID is authorized to access, otherwise false</returns>
        private bool IsAuthorizedExamID(int examID) {
            DataTable dataTableExamID = DataAccessLayer.SelectCommand(DataAccessLayer.SelectCommandString(
                "Exam_ID", ExamResultTableNameTeacher, "Teacher_ID = :ID AND Exam_ID = :examID AND End_Date IS NULL"),
                new CommandParameter(":ID", ID), new CommandParameter(":examID", examID));
            return (1 == dataTableExamID.Rows.Count);
        }

        /// <summary>
        /// Get a list of student IDs of exams selected by exam ID
        /// </summary>
        /// <param name="examID">exam ID</param>
        /// <returns>data of 'DataTable' type containing student IDs of exam of exam ID</returns>
        public DataTable GetExamResultInfo_StudentID(int examID) {
            if (!IsAuthorizedExamID(examID)) return new DataTable();
            return DataAccessLayer.SelectCommand(DataAccessLayer.SelectCommandString(
                "Student_ID AS \"Student ID\"", ExamResultTableNameStudent,
                "Exam_ID = :examID", orderByClause: "Student_ID"), new CommandParameter(":examID", examID));
        }

        /// <summary>
        /// Set exam result of specific exam ID and student ID
        /// </summary>
        /// <param name="examID">exam ID</param>
        /// <param name="studentID">student ID</param>
        /// <param name="obtainedMarks">obtained marks</param>
        /// <returns>0 if success, otherwise the error code</returns>
        public int SubmitExamResult(int examID, int studentID, int obtainedMarks) {
            if (!IsAuthorizedExamID(examID)) return -1; // teacher is not authorized for the exam ID

            return DataAccessLayer.UpdateCommand(DataAccessLayer.UpdateCommandString(
                "Result", "Obtained_Marks = :obtainedMarks", "Exam_ID = :examID AND Student_ID = :studentID"),
                new CommandParameter(":obtainedMarks", obtainedMarks), new CommandParameter(":examID", examID),
                new CommandParameter(":studentID", studentID));
        }
    }
}