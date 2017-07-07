DROP TABLE STUDENT CASCADE CONSTRAINTS;
DROP TABLE TEACHER CASCADE CONSTRAINTS;
DROP TABLE SCHOOL CASCADE CONSTRAINTS;
DROP TABLE CLASS CASCADE CONSTRAINTS;
DROP TABLE COURSE CASCADE CONSTRAINTS;
DROP TABLE RESULT CASCADE CONSTRAINTS;
DROP TABLE ENROLL_STUDENT CASCADE CONSTRAINTS;
DROP TABLE ENROLL_TEACHER CASCADE CONSTRAINTS;
DROP TABLE EXAM CASCADE CONSTRAINTS;
DROP TABLE COURSE_CLASS_SCHOOL CASCADE CONSTRAINTS;
DROP TABLE LOGIN CASCADE CONSTRAINTS;
DROP TABLE MCQ CASCADE CONSTRAINTS;
DROP TABLE QUESTION_SET CASCADE CONSTRAINTS;
DROP TABLE ANSWER_OPTION CASCADE CONSTRAINTS;
DROP TABLE ANSWER_SET CASCADE CONSTRAINTS;
DROP TABLE Login_ROLE CASCADE CONSTRAINTS;
DROP TABLE PROFILE_PICTURE CASCADE CONSTRAINTS;
DROP TABLE COURSE_CODE CASCADE CONSTRAINTS;

DROP VIEW ENROLL_COURSE_STUDENT;
DROP VIEW ENROLL_COURSE_TEACHER;
DROP VIEW ENROLL_EXAM_STUDENT;
DROP VIEW ENROLL_EXAM_TEACHER;
DROP VIEW COURSE_CLASS_SCHOOL_NAME;

CREATE TABLE STUDENT (
ID NUMBER NOT NULL,
Login_ID NUMBER NOT NULL,
Name VARCHAR2(255) NOT NULL,
Student_Roll NUMBER NULL,
School_ID NUMBER NULL,
Class_ID NUMBER NULL,
Address VARCHAR2(255) NULL,
Contact VARCHAR2(255) NULL,
Date_of_Birth DATE NULL,
Picture_ID NUMBER NULL,
PRIMARY KEY (ID)
);

ALTER TABLE STUDENT ADD CONSTRAINT Login_ID_Student_Unique UNIQUE (Login_ID);

CREATE TABLE TEACHER (
ID NUMBER NOT NULL,
Login_ID NUMBER NOT NULL,
Name VARCHAR2(255) NOT NULL,
School_ID NUMBER NULL,
Address VARCHAR2(255) NULL,
Contact VARCHAR2(255) NULL,
Picture_ID NUMBER NULL,
PRIMARY KEY (ID)
);

ALTER TABLE TEACHER ADD CONSTRAINT Login_ID_Teacher_Unique UNIQUE (Login_ID);

CREATE TABLE SCHOOL (
ID NUMBER NOT NULL,
Login_ID NUMBER NOT NULL,
Name VARCHAR2(255) NOT NULL,
Address VARCHAR2(255) NULL,
Contact VARCHAR2(255) NULL,
Description VARCHAR2(255) NULL,
Headmaster_ID NUMBER NULL,
Url VARCHAR2(255) NULL,
Picture_ID NUMBER NULL,
PRIMARY KEY (ID)
);

ALTER TABLE SCHOOL ADD CONSTRAINT Login_ID_School_Unique UNIQUE (Login_ID);

CREATE TABLE CLASS (
ID NUMBER NOT NULL,
Name VARCHAR2(255) NOT NULL,
PRIMARY KEY (ID)
);

ALTER TABLE CLASS ADD CONSTRAINT Name_Class UNIQUE (Name);

CREATE TABLE COURSE (
ID NUMBER NOT NULL,
Name VARCHAR2(255) NOT NULL,
PRIMARY KEY (ID)
);

ALTER TABLE COURSE ADD CONSTRAINT Name_Course UNIQUE (Name);

CREATE TABLE RESULT (
ID NUMBER NOT NULL,
Exam_ID NUMBER NOT NULL,
Student_ID NUMBER NOT NULL,
Obtained_Marks NUMBER NULL,
Answer_Set_ID NUMBER NULL,
PRIMARY KEY (ID)
);

ALTER TABLE RESULT ADD CONSTRAINT Answer_Set_ID_Result UNIQUE (Answer_Set_ID);
ALTER TABLE RESULT ADD CONSTRAINT Exam_ID_Student_ID_Result UNIQUE (Exam_ID, Student_ID);

CREATE TABLE ENROLL_STUDENT (
ID NUMBER NOT NULL,
Course_Class_School_ID NUMBER NOT NULL,
Student_ID NUMBER NOT NULL,
Enroll_Date DATE NULL,
PRIMARY KEY (ID)
);

CREATE TABLE ENROLL_TEACHER (
ID NUMBER NOT NULL,
Course_Class_School_ID NUMBER NOT NULL,
Teacher_ID NUMBER NOT NULL,
Enroll_Date DATE NULL,
End_Date DATE NULL,
PRIMARY KEY (ID)
);

CREATE TABLE EXAM (
ID NUMBER NOT NULL,
Course_Class_School_ID NUMBER NOT NULL,
Date_Time DATE NOT NULL,
Duration_Second NUMBER NULL,
Total_Marks VARCHAR2(255) NULL,
Syllabus VARCHAR2(255) NULL,
Question_Set_ID NUMBER NULL,
PRIMARY KEY (ID)
);

ALTER TABLE EXAM ADD CONSTRAINT Question_Set_ID_Exam UNIQUE (Question_Set_ID);

CREATE TABLE COURSE_CLASS_SCHOOL (
ID NUMBER NOT NULL,
Course_ID NUMBER NOT NULL,
Class_ID NUMBER NOT NULL,
School_ID NUMBER NOT NULL,
Course_Code_ID NUMBER NULL,
PRIMARY KEY (ID)
);

ALTER TABLE COURSE_CLASS_SCHOOL ADD CONSTRAINT Course_Class_School_ID UNIQUE (Course_ID, Class_ID, School_ID, Course_Code_ID);

CREATE TABLE LOGIN (
ID NUMBER NOT NULL,
Username VARCHAR2(255) NOT NULL,
Email VARCHAR2(255) NOT NULL,
Password VARCHAR2(255) NOT NULL,
Login_Role_ID NUMBER NOT NULL,
PRIMARY KEY (ID)
);

ALTER TABLE LOGIN ADD CONSTRAINT Login_Username UNIQUE (Username);
ALTER TABLE LOGIN ADD CONSTRAINT Login_Email UNIQUE (Email);

CREATE TABLE MCQ (
ID NUMBER NOT NULL,
Course_Class_School_ID NUMBER NOT NULL,
Question VARCHAR2(255) NOT NULL,
Correct_Ans_ID NUMBER NOT NULL,
Marks NUMBER NOT NULL,
Minus_Marks NUMBER NOT NULL,
PRIMARY KEY (ID)
);

CREATE TABLE QUESTION_SET (
ID NUMBER NOT NULL,
Set_ID NUMBER NOT NULL,
Question_ID NUMBER NOT NULL,
PRIMARY KEY (ID)
);

CREATE TABLE ANSWER_OPTION (
ID NUMBER NOT NULL,
Question_ID NUMBER NOT NULL,
Option_Answer VARCHAR2(255) NOT NULL,
PRIMARY KEY (ID)
);

CREATE TABLE ANSWER_SET (
ID NUMBER NOT NULL,
Set_ID NUMBER NOT NULL,
Question_ID NUMBER NOT NULL,
Given_Ans_ID NUMBER NOT NULL,
PRIMARY KEY (ID)
);

CREATE TABLE Login_ROLE (
ID NUMBER NOT NULL,
Name VARCHAR2(255) NOT NULL,
PRIMARY KEY (ID)
);

ALTER TABLE Login_ROLE ADD CONSTRAINT Name_Login_Role UNIQUE (Name);

CREATE TABLE PROFILE_PICTURE (
ID NUMBER NOT NULL,
File_Path VARCHAR2(255) NOT NULL,
PRIMARY KEY (ID)
);

ALTER TABLE PROFILE_PICTURE ADD CONSTRAINT File_Path_Profile_Picture UNIQUE (File_Path);

CREATE TABLE COURSE_CODE (
ID NUMBER NOT NULL,
Name VARCHAR2(255) NOT NULL,
PRIMARY KEY (ID)
);

ALTER TABLE COURSE_CODE ADD CONSTRAINT Name_Course_Code UNIQUE (Name);


ALTER TABLE ENROLL_TEACHER ADD CONSTRAINT Course_Class_School_ID_Teacher FOREIGN KEY (Course_Class_School_ID) REFERENCES COURSE_CLASS_SCHOOL (ID) ON DELETE CASCADE;
ALTER TABLE ENROLL_TEACHER ADD CONSTRAINT Teacher_ID_Enroll FOREIGN KEY (Teacher_ID) REFERENCES TEACHER (ID) ON DELETE CASCADE;
ALTER TABLE ENROLL_STUDENT ADD CONSTRAINT Course_Class_School_ID_Student FOREIGN KEY (Course_Class_School_ID) REFERENCES COURSE_CLASS_SCHOOL (ID) ON DELETE CASCADE;
ALTER TABLE COURSE_CLASS_SCHOOL ADD CONSTRAINT Course_ID_Course FOREIGN KEY (Course_ID) REFERENCES COURSE (ID) ON DELETE CASCADE;
ALTER TABLE COURSE_CLASS_SCHOOL ADD CONSTRAINT Class_ID_Course FOREIGN KEY (Class_ID) REFERENCES CLASS (ID) ON DELETE CASCADE;
ALTER TABLE RESULT ADD CONSTRAINT Exam_ID_Result FOREIGN KEY (Exam_ID) REFERENCES EXAM (ID) ON DELETE CASCADE;
ALTER TABLE EXAM ADD CONSTRAINT Course_Class_School_ID_Exam FOREIGN KEY (Course_Class_School_ID) REFERENCES COURSE_CLASS_SCHOOL (ID) ON DELETE CASCADE;
ALTER TABLE STUDENT ADD CONSTRAINT Login_ID_Student FOREIGN KEY (Login_ID) REFERENCES LOGIN (ID) ON DELETE CASCADE;
ALTER TABLE SCHOOL ADD CONSTRAINT Login_ID_School FOREIGN KEY (Login_ID) REFERENCES LOGIN (ID) ON DELETE CASCADE;
ALTER TABLE TEACHER ADD CONSTRAINT Login_ID_Teacher FOREIGN KEY (Login_ID) REFERENCES LOGIN (ID) ON DELETE CASCADE;
ALTER TABLE SCHOOL ADD CONSTRAINT Headmaster_ID_School FOREIGN KEY (Headmaster_ID) REFERENCES TEACHER (ID) ON DELETE CASCADE;
ALTER TABLE STUDENT ADD CONSTRAINT Class_ID_Student FOREIGN KEY (Class_ID) REFERENCES CLASS (ID) ON DELETE CASCADE;
ALTER TABLE QUESTION_SET ADD CONSTRAINT Question_ID_Set FOREIGN KEY (Question_ID) REFERENCES MCQ (ID) ON DELETE CASCADE;
ALTER TABLE MCQ ADD CONSTRAINT Correct_Ans_ID_MCQ FOREIGN KEY (Correct_Ans_ID) REFERENCES ANSWER_OPTION (ID) ON DELETE CASCADE;
ALTER TABLE ANSWER_OPTION ADD CONSTRAINT Question_ID_Answer_Option FOREIGN KEY (Question_ID) REFERENCES MCQ (ID) ON DELETE CASCADE;
ALTER TABLE ANSWER_SET ADD CONSTRAINT Question_ID_Answer_Set FOREIGN KEY (Question_ID) REFERENCES MCQ (ID) ON DELETE CASCADE;
ALTER TABLE MCQ ADD CONSTRAINT Course_Class_School_ID_MCQ FOREIGN KEY (Course_Class_School_ID) REFERENCES COURSE_CLASS_SCHOOL (ID) ON DELETE CASCADE;
ALTER TABLE LOGIN ADD CONSTRAINT Login_Role_ID_Login FOREIGN KEY (Login_Role_ID) REFERENCES Login_ROLE (ID) ON DELETE CASCADE;
ALTER TABLE STUDENT ADD CONSTRAINT Picture_ID_Student FOREIGN KEY (Picture_ID) REFERENCES PROFILE_PICTURE (ID) ON DELETE CASCADE;
ALTER TABLE SCHOOL ADD CONSTRAINT Picture_ID_School FOREIGN KEY (Picture_ID) REFERENCES PROFILE_PICTURE (ID) ON DELETE CASCADE;
ALTER TABLE TEACHER ADD CONSTRAINT Picture_ID_Teacher FOREIGN KEY (Picture_ID) REFERENCES PROFILE_PICTURE (ID) ON DELETE CASCADE;
ALTER TABLE ANSWER_SET ADD CONSTRAINT Answer_Option_ID_Answer_Set FOREIGN KEY (Given_Ans_ID) REFERENCES ANSWER_OPTION (ID) ON DELETE CASCADE;
ALTER TABLE QUESTION_SET ADD CONSTRAINT Question_Set_ID_Question_Set FOREIGN KEY (Set_ID) REFERENCES EXAM (Question_Set_ID) ON DELETE CASCADE;
ALTER TABLE ANSWER_SET ADD CONSTRAINT Answer_Set_ID_Answer_Set FOREIGN KEY (Set_ID) REFERENCES RESULT (Answer_Set_ID) ON DELETE CASCADE;
ALTER TABLE COURSE_CLASS_SCHOOL ADD CONSTRAINT School_ID_Course FOREIGN KEY (School_ID) REFERENCES SCHOOL (ID) ON DELETE CASCADE;
ALTER TABLE TEACHER ADD CONSTRAINT School_ID_Teacher FOREIGN KEY (School_ID) REFERENCES SCHOOL (ID) ON DELETE CASCADE;
ALTER TABLE STUDENT ADD CONSTRAINT School_ID_Student FOREIGN KEY (School_ID) REFERENCES SCHOOL (ID) ON DELETE CASCADE;
ALTER TABLE ENROLL_STUDENT ADD CONSTRAINT Student_ID_Enroll FOREIGN KEY (Student_ID) REFERENCES STUDENT (ID) ON DELETE CASCADE;
ALTER TABLE RESULT ADD CONSTRAINT Student_ID_Result FOREIGN KEY (Student_ID) REFERENCES STUDENT (ID) ON DELETE CASCADE;
ALTER TABLE COURSE_CLASS_SCHOOL ADD CONSTRAINT Course_Code_ID_Course FOREIGN KEY (Course_Code_ID) REFERENCES COURSE_CODE (ID) ON DELETE CASCADE;

INSERT INTO Login_ROLE VALUES (1, 'Student');
INSERT INTO Login_ROLE VALUES (2, 'Teacher');
INSERT INTO Login_ROLE VALUES (3, 'School');

CREATE VIEW ENROLL_COURSE_STUDENT AS
SELECT
CCS.ID AS Course_Class_School_ID,
CRS.ID AS Course_ID,
CRS.Name AS Course_Name,
CRSCD.ID AS Course_Code_ID,
CRSCD.Name AS Course_Code,
CLSS.ID AS Class_ID,
CLSS.Name AS Class_Name,
SCH.ID AS School_ID,
SCH.Name AS School_Name,
STU.ID AS Student_ID,
STU.Name AS Student_Name,
STU.Student_Roll,
STU.Class_ID AS Current_Class_ID,
CLSS_STU.Name AS Current_Class_Name,
STU.School_ID AS Current_School_ID,
SCH_STU.Name AS Current_School_Name,
ESTU.ID AS Enroll_Student_ID,
ESTU.Enroll_Date
FROM
COURSE_CLASS_SCHOOL CCS,
COURSE CRS,
COURSE_CODE CRSCD,
CLASS CLSS,
SCHOOL SCH,
STUDENT STU,
CLASS CLSS_STU,
SCHOOL SCH_STU,
ENROLL_STUDENT ESTU
WHERE
CCS.Course_ID = CRS.ID
AND CCS.Course_Code_ID = CRSCD.ID
AND CCS.Class_ID = CLSS.ID
AND CCS.School_ID = SCH.ID
AND STU.Class_ID = CLSS_STU.ID
AND STU.School_ID = SCH_STU.ID
AND ESTU.Course_Class_School_ID = CCS.ID
AND ESTU.Student_ID = STU.ID;

CREATE VIEW ENROLL_COURSE_TEACHER AS
SELECT
CCS.ID AS Course_Class_School_ID,
CRS.ID AS Course_ID,
CRS.Name AS Course_Name,
CRSCD.ID AS Course_Code_ID,
CRSCD.Name AS Course_Code,
CLSS.ID AS Class_ID,
CLSS.Name AS Class_Name,
SCH.ID AS School_ID,
SCH.Name AS School_Name,
TCHR.ID AS Teacher_ID,
TCHR.Name AS Teacher_Name,
TCHR.School_ID AS Current_School_ID,
SCH_TCHR.Name AS Current_School_Name,
ETCHR.ID AS Enroll_Teacher_ID,
ETCHR.Enroll_Date,
ETCHR.End_Date
FROM
COURSE_CLASS_SCHOOL CCS,
COURSE CRS,
COURSE_CODE CRSCD,
CLASS CLSS,
SCHOOL SCH,
TEACHER TCHR,
SCHOOL SCH_TCHR,
ENROLL_TEACHER ETCHR
WHERE
CCS.Course_ID = CRS.ID
AND CCS.Course_Code_ID = CRSCD.ID
AND CCS.Class_ID = CLSS.ID
AND CCS.School_ID = SCH.ID
AND TCHR.School_ID = SCH_TCHR.ID
AND ETCHR.Course_Class_School_ID = CCS.ID
AND ETCHR.Teacher_ID = TCHR.ID;

CREATE VIEW ENROLL_EXAM_STUDENT AS
SELECT
CCS.ID AS Course_Class_School_ID,
CRS.ID AS Course_ID,
CRS.Name AS Course_Name,
CRSCD.ID AS Course_Code_ID,
CRSCD.Name AS Course_Code,
CLSS.ID AS Class_ID,
CLSS.Name AS Class_Name,
SCH.ID AS School_ID,
SCH.Name AS School_Name,
STU.ID AS Student_ID,
STU.Name AS Student_Name,
STU.Student_Roll,
STU.Class_ID AS Current_Class_ID,
CLSS_STU.Name AS Current_Class_Name,
STU.School_ID AS Current_School_ID,
SCH_STU.Name AS Current_School_Name,
EXM.ID AS Exam_ID,
EXM.Total_Marks,
EXM.Date_Time,
EXM.Duration_Second AS Duration,
EXM.Syllabus,
RSLT.ID AS Result_ID,
RSLT.Obtained_Marks
FROM
COURSE_CLASS_SCHOOL CCS,
COURSE CRS,
COURSE_CODE CRSCD,
CLASS CLSS,
SCHOOL SCH,
STUDENT STU,
CLASS CLSS_STU,
SCHOOL SCH_STU,
EXAM EXM,
RESULT RSLT
WHERE
CCS.Course_ID = CRS.ID
AND CCS.Course_Code_ID = CRSCD.ID
AND CCS.Class_ID = CLSS.ID
AND CCS.School_ID = SCH.ID
AND STU.Class_ID = CLSS_STU.ID
AND STU.School_ID = SCH_STU.ID
AND EXM.Course_Class_School_ID = CCS.ID
AND RSLT.Exam_ID = EXM.ID
AND RSLT.Student_ID = STU.ID;

CREATE VIEW ENROLL_EXAM_TEACHER AS
SELECT
CCS.ID AS Course_Class_School_ID,
CRS.ID AS Course_ID,
CRS.Name AS Course_Name,
CRSCD.ID AS Course_Code_ID,
CRSCD.Name AS Course_Code,
CLSS.ID AS Class_ID,
CLSS.Name AS Class_Name,
SCH.ID AS School_ID,
SCH.Name AS School_Name,
TCHR.ID AS Teacher_ID,
TCHR.Name AS Teacher_Name,
TCHR.School_ID AS Current_School_ID,
SCH_TCHR.Name AS Current_School_Name,
ETCHR.ID AS Enroll_Teacher_ID,
ETCHR.Enroll_Date,
ETCHR.End_Date,
EXM.ID AS Exam_ID,
EXM.Total_Marks,
EXM.Date_Time,
EXM.Duration_Second AS Duration,
EXM.Syllabus
FROM
COURSE_CLASS_SCHOOL CCS,
COURSE CRS,
COURSE_CODE CRSCD,
CLASS CLSS,
SCHOOL SCH,
TEACHER TCHR,
SCHOOL SCH_TCHR,
ENROLL_TEACHER ETCHR,
EXAM EXM
WHERE
CCS.Course_ID = CRS.ID
AND CCS.Course_Code_ID = CRSCD.ID
AND CCS.Class_ID = CLSS.ID
AND CCS.School_ID = SCH.ID
AND TCHR.School_ID = SCH_TCHR.ID
AND ETCHR.Course_Class_School_ID = CCS.ID
AND ETCHR.Teacher_ID = TCHR.ID
AND EXM.Course_Class_School_ID = CCS.ID;

CREATE VIEW COURSE_CLASS_SCHOOL_NAME AS
SELECT
CCS.ID AS Course_Class_School_ID,
CRS.ID AS Course_ID,
CRS.Name AS Course_Name,
CRSCD.ID AS Course_Code_ID,
CRSCD.Name AS Course_Code,
CLSS.ID AS Class_ID,
CLSS.Name AS Class_Name,
SCH.ID AS School_ID,
SCH.Name AS School_Name
FROM
COURSE_CLASS_SCHOOL CCS,
COURSE CRS,
COURSE_CODE CRSCD,
CLASS CLSS,
SCHOOL SCH
WHERE
CCS.Course_ID = CRS.ID
AND CCS.Course_Code_ID = CRSCD.ID
AND CCS.Class_ID = CLSS.ID
AND CCS.School_ID = SCH.ID;

CREATE OR REPLACE PROCEDURE Signup_User
(
  P_Name IN VARCHAR2
, P_Username IN VARCHAR2
, P_Email IN VARCHAR2
, P_Password IN VARCHAR2
, P_Login_Role_ID IN NUMBER
) AS
  V_Insert_ID NUMBER;
  V_Login_ID NUMBER;
BEGIN
  IF P_Login_Role_ID < 1 OR P_Login_Role_ID > 3 THEN
    Raise_Application_Error(-20001, 'Invalid Parameter: Login_Role_ID cannot be less than 1 or greater than 3.');
  END IF;
  SELECT (NVL(MAX(ID), 0) + 1) INTO V_Insert_ID FROM LOGIN;
  INSERT INTO LOGIN VALUES (V_Insert_ID, P_Username, P_Email, P_Password, P_Login_Role_ID);
  V_Login_ID := V_Insert_ID;
  IF P_Login_Role_ID = 1 THEN
    SELECT (NVL(MAX(ID), 0) + 1) INTO V_Insert_ID FROM STUDENT;
    INSERT INTO STUDENT (ID, Login_ID, Name) VALUES (V_Insert_ID, V_Login_ID, P_Name);
  ELSIF P_Login_Role_ID = 2 THEN
    SELECT (NVL(MAX(ID), 0) + 1) INTO V_Insert_ID FROM TEACHER;
    INSERT INTO TEACHER (ID, Login_ID, Name) VALUES (V_Insert_ID, V_Login_ID, P_Name);
  ELSE
    SELECT (NVL(MAX(ID), 0) + 1) INTO V_Insert_ID FROM SCHOOL;
    INSERT INTO SCHOOL (ID, Login_ID, Name) VALUES (V_Insert_ID, V_Login_ID, P_Name);
  END IF;
END;
/

ALTER SESSION SET PLSCOPE_SETTINGS = 'IDENTIFIERS:NONE';
CREATE OR REPLACE TRIGGER Create_Exam
AFTER INSERT ON EXAM
FOR EACH ROW
DECLARE
  V_Insert_ID NUMBER;
  V_Exam_ID NUMBER;
BEGIN
  V_Exam_ID := :NEW.ID;
  SELECT (NVL(MAX(ID), 0) + 1) INTO V_Insert_ID FROM RESULT;
  FOR C_Student IN
  (
    SELECT
      ECS.Student_ID
    FROM
      ENROLL_COURSE_STUDENT ECS
    WHERE
      ECS.Course_Class_School_ID = :NEW.Course_Class_School_ID
      AND ECS.Class_ID = (SELECT S.Class_ID FROM STUDENT S WHERE S.ID = ECS.Student_ID)
      AND ECS.School_ID = (SELECT S.School_ID FROM STUDENT S WHERE S.ID = ECS.Student_ID)
  )
  LOOP
    INSERT INTO RESULT VALUES (V_Insert_ID, V_Exam_ID, C_Student.STUDENT_ID, NULL, NULL);
    V_Insert_ID := V_Insert_ID + 1;
  END LOOP;
END;
/
ALTER SESSION SET PLSCOPE_SETTINGS = 'IDENTIFIERS:ALL';

CREATE OR REPLACE TRIGGER School_Change_Teacher
AFTER UPDATE OF School_ID ON TEACHER
FOR EACH ROW
WHEN (OLD.School_ID IS NOT NULL)
BEGIN
  IF :OLD.School_ID = :NEW.School_ID THEN
    RETURN;
  END IF;
  UPDATE
    ENROLL_TEACHER
  SET
    End_Date = SYSDATE
  WHERE
   TEACHER_ID = :OLD.ID
   AND End_Date IS NULL;
END;
/

CREATE OR REPLACE FUNCTION Get_Student_ID_By_Username
(
  P_Username IN VARCHAR2
) RETURN NUMBER AS
  V_Login_ID NUMBER;
  V_Student_ID NUMBER;
BEGIN
  SELECT ID INTO V_Login_ID FROM LOGIN WHERE Username = P_Username;
  SELECT ID INTO V_Student_ID FROM STUDENT WHERE Login_ID = V_Login_ID;
  RETURN V_Student_ID;
END;
/

CREATE OR REPLACE FUNCTION Get_Student_ID_By_Email
(
  P_Email IN VARCHAR2
) RETURN NUMBER AS
  V_Login_ID NUMBER;
  V_Student_ID NUMBER;
BEGIN
  SELECT ID INTO V_Login_ID FROM LOGIN WHERE Email = P_Email;
  SELECT ID INTO V_Student_ID FROM STUDENT WHERE Login_ID = V_Login_ID;
  RETURN V_Student_ID;
END;
/

CREATE OR REPLACE FUNCTION Get_Teacher_ID_By_Username
(
  P_Username IN VARCHAR2
) RETURN NUMBER AS
  V_Login_ID NUMBER;
  V_Teacher_ID NUMBER;
BEGIN
  SELECT ID INTO V_Login_ID FROM LOGIN WHERE Username = P_Username;
  SELECT ID INTO V_Teacher_ID FROM TEACHER WHERE Login_ID = V_Login_ID;
  RETURN V_Teacher_ID;
END;
/

CREATE OR REPLACE FUNCTION Get_Teacher_ID_By_Email
(
  P_Email IN VARCHAR2
) RETURN NUMBER AS
  V_Login_ID NUMBER;
  V_Teacher_ID NUMBER;
BEGIN
  SELECT ID INTO V_Login_ID FROM LOGIN WHERE Email = P_Email;
  SELECT ID INTO V_Teacher_ID FROM TEACHER WHERE Login_ID = V_Login_ID;
  RETURN V_Teacher_ID;
END;
/

CREATE OR REPLACE PROCEDURE Add_Course_Class
(
  P_School_ID IN NUMBER
, P_Course_Name IN VARCHAR2
, P_Class_Name IN VARCHAR2
, P_Course_Code IN VARCHAR2 DEFAULT NULL
) AS
  V_Course_ID NUMBER;
  V_Class_ID NUMBER;
  V_Course_Code_ID NUMBER;
  V_Course_Class_School_ID NUMBER;
BEGIN
  BEGIN
    SELECT ID INTO V_Course_ID FROM COURSE WHERE Name = P_Course_Name;
  EXCEPTION
    WHEN No_Data_Found THEN
      SELECT (NVL(MAX(ID), 0) + 1) INTO V_Course_ID FROM COURSE;
      INSERT INTO COURSE VALUES (V_Course_ID, P_Course_Name);
  END;
  BEGIN
    SELECT ID INTO V_Class_ID FROM CLASS WHERE Name = P_Class_Name;
  EXCEPTION
    WHEN No_Data_Found THEN
      SELECT (NVL(MAX(ID), 0) + 1) INTO V_Class_ID FROM CLASS;
      INSERT INTO CLASS VALUES (V_Class_ID, P_Class_Name);
  END;
  IF P_Course_Code IS NOT NULL THEN
    BEGIN
      SELECT ID INTO V_Course_Code_ID FROM COURSE_CODE WHERE Name = P_Course_Code;
    EXCEPTION
      WHEN No_Data_Found THEN
        SELECT (NVL(MAX(ID), 0) + 1) INTO V_Course_Code_ID FROM COURSE_CODE;
        INSERT INTO COURSE_CODE VALUES (V_Course_Code_ID, P_Course_Code);
    END;
  ELSE
    V_Course_Code_ID := NULL;
  END IF;
  SELECT (NVL(MAX(ID), 0) + 1) INTO V_Course_Class_School_ID FROM COURSE_CLASS_SCHOOL;
  INSERT INTO COURSE_CLASS_SCHOOL VALUES (V_Course_Class_School_ID, V_Course_ID, V_Class_ID, P_School_ID, V_Course_Code_ID);
END;
/

