----- Student -----

INSERT INTO LOGIN VALUES (1, 'arnobpl', 'arnobpl@gmail.com', 'QCpNS4mrJFUAOuxbgmf5cg==', 1);
INSERT INTO STUDENT VALUES (1, 1, 'Arnob Paul', 3, NULL, NULL, 'Bashabo, Dhaka, Bangladesh', '+8801723841200', TO_DATE('1994-01-23', 'SYYYY-MM-DD'), NULL);

INSERT INTO LOGIN VALUES (2, 'shuhana102', 'shuhana102@gmail.com', 'kS0k8Gt0uyp//MAZsn8aow==', 1);
INSERT INTO STUDENT VALUES (2, 2, 'Shuha Nabila', 1, NULL, NULL, 'Siddheswari, Dhaka, Bangladesh', '+8801775377502', TO_DATE('1995-11-29', 'SYYYY-MM-DD'), NULL);

INSERT INTO LOGIN VALUES (3, 'fr.rahat', 'fr.rahat@gmail.com', 'NBRHPx6PyTE1Yt0HrcvRIg==', 1);
INSERT INTO STUDENT VALUES (3, 3, 'Fazle Rabbi Rahat', 2, NULL, NULL, 'Bashabo, Dhaka, Bangladesh', '+8801521439718', TO_DATE('1994-06-18', 'SYYYY-MM-DD'), NULL);

INSERT INTO LOGIN VALUES (4, 'mayisha.alam', 'mayisha.alam@gmail.com', 'K7sMXohFEiqk5Xnics0TVw==', 1);
INSERT INTO STUDENT VALUES (4, 4, 'Mayisha Alam', 4, NULL, NULL, 'Jigatola, Dhaka, Bangladesh', '+8801676735860', NULL, NULL);

INSERT INTO LOGIN VALUES (5, 'nazsa.9070', 'nazsa.9070@gmail.com', 'vE/68H9wXtMXu4xNMwdTKA==', 1);
INSERT INTO STUDENT VALUES (5, 5, 'Nazmus Sakib', 5, NULL, NULL, 'Khilgaon, Dhaka, Bangladesh', '+8801779435710', TO_DATE('1995-11-03', 'SYYYY-MM-DD'), NULL);

INSERT INTO LOGIN VALUES (6, 'amina.nisha', 'amina.nisha@hotmail.com', 'nOKRSJclffZgB9KB7iEMNw==', 1);
INSERT INTO STUDENT VALUES (6, 6, 'Amina Rahman Nisha', 6, NULL, NULL, 'Kalyanpur, Dhaka, Bangladesh', '+8801869505126', NULL, NULL);

----- Teacher -----

INSERT INTO LOGIN VALUES (7, 'monirul.islam', 'mmislam@cse.buet.ac.bd', 'RB0JfBK2KQgRUCyTLMHsRA==', 2);
INSERT INTO TEACHER VALUES (1, 7, 'Md. Monirul Islam', NULL, NULL, '+88029665650', NULL);

INSERT INTO LOGIN VALUES (8, 'anindya_iqbal', 'anindya_iqbal@yahoo.com', 'FaQed1y3tQoWYAz6v/eCnA==', 2);
INSERT INTO TEACHER VALUES (2, 8, 'Anindya Iqbal', NULL, NULL, '+8801857416849', NULL);

INSERT INTO LOGIN VALUES (9, 'asmlatiful', 'asmlatifulhoque@cse.buet.ac.bd', 'RY9g7nDpLUGQTowZ0opCbA==', 2);
INSERT INTO TEACHER VALUES (3, 9, 'Md. Latiful Hoque', NULL, NULL, '+8801556346357', NULL);

INSERT INTO LOGIN VALUES (10, 'takhandipu', 'takhandipu@gmail.com', 'dquJzR2MJDG+JLjjlHTcpg==', 2);
INSERT INTO TEACHER VALUES (4, 10, 'Tanvir Ahmed Khan', NULL, 'Jatrabari, Dhaka, Bangladesh', '+8801912090363', NULL);

INSERT INTO LOGIN VALUES (11, 'madhusudan08', 'madhusudan.buet@gmail.com', 'DtM9XJrT+P+PGjE1Ejt4rw==', 2);
INSERT INTO TEACHER VALUES (5, 11, 'Madhusudan Basak', NULL, NULL, '+8801710288910', NULL);

INSERT INTO LOGIN VALUES (12, 'masattar.buet', 'masattar@cse.buet.ac.bd', 'uyB4+WNOThE1OtxLAcK7mQ==', 2);
INSERT INTO TEACHER VALUES (6, 12, 'Md. Abdus Sattar', NULL, NULL, '+88029665650', NULL);

----- School -----

INSERT INTO LOGIN VALUES (13, 'ndc.dhaka', 'ndc.dhaka@gmail.com', 'q8dYiYhxFEopOMVJEPkn1A==', 3);
INSERT INTO SCHOOL VALUES (1, 13, 'Notre Dame College', 'Azimpur, Dhaka, Bangladesh', NULL, NULL, NULL, NULL, NULL);

INSERT INTO LOGIN VALUES (14, 'vnc.dhaka', 'vnc.dhaka@gmail.com', 'nadnWtov18LSCWmGn7wPaQ==', 3);
INSERT INTO SCHOOL VALUES (2, 14, 'Viqarunnisa Noon School and College', 'New Baily Road, Dhaka, Bangladesh', NULL, NULL, NULL, NULL, NULL);

INSERT INTO LOGIN VALUES (15, 'ideal.school_dhaka', 'ideal.school_dhaka@gmail.com', '4M7Wxk0/u+RNAGm4syCoeQ==', 3);
INSERT INTO SCHOOL VALUES (3, 15, 'Ideal School and College', 'Motijheel, Dhaka, Bangladesh', NULL, NULL, NULL, NULL, NULL);

INSERT INTO LOGIN VALUES (16, 'hcsc.dhaka', 'hcsc.dhaka@gmail.com', 'ns1MjUuJ00TMYKkSYGLHBg==', 3);
INSERT INTO SCHOOL VALUES (4, 16, 'Holy Cross School and College', 'Tejgaon, Dhaka, Bangladesh', NULL, NULL, NULL, NULL, NULL);

INSERT INTO LOGIN VALUES (17, 'motijheel.govt.dhaka', 'motijheel.govt.dhaka@gmail.com', 'sqeHmHk9a0axkMRLfxZOYQ==', 3);
INSERT INTO SCHOOL VALUES (5, 17, 'Motijheel Govt. High School', 'Motijheel, Dhaka, Bangladesh', NULL, NULL, NULL, NULL, NULL);

INSERT INTO LOGIN VALUES (18, 'national.ideal.dhaka', 'national.ideal.dhaka@gmail.com', '8jMlzkssqbCZOElBez/acQ==', 3);
INSERT INTO SCHOOL VALUES (6, 18, 'National Ideal College', 'Khilgaon, Dhaka, Bangladesh', NULL, NULL, NULL, NULL, NULL);

----- Student School Assign -----

UPDATE STUDENT SET School_ID = 1 WHERE LOGIN_ID = 1;
UPDATE STUDENT SET School_ID = 2 WHERE LOGIN_ID = 2;
UPDATE STUDENT SET School_ID = 3 WHERE LOGIN_ID = 3;
UPDATE STUDENT SET School_ID = 4 WHERE LOGIN_ID = 4;
UPDATE STUDENT SET School_ID = 5 WHERE LOGIN_ID = 5;
UPDATE STUDENT SET School_ID = 6 WHERE LOGIN_ID = 6;

----- Teacher School Assign -----

UPDATE TEACHER SET School_ID = 1 WHERE LOGIN_ID = 7;
UPDATE TEACHER SET School_ID = 4 WHERE LOGIN_ID = 8;
UPDATE TEACHER SET School_ID = 6 WHERE LOGIN_ID = 9;
UPDATE TEACHER SET School_ID = 1 WHERE LOGIN_ID = 10;
UPDATE TEACHER SET School_ID = 2 WHERE LOGIN_ID = 11;
UPDATE TEACHER SET School_ID = 4 WHERE LOGIN_ID = 12;

----- Class -----

INSERT INTO CLASS VALUES (1, 'Nursery');
INSERT INTO CLASS VALUES (2, 'One');
INSERT INTO CLASS VALUES (3, 'Two');
INSERT INTO CLASS VALUES (4, 'Three');
INSERT INTO CLASS VALUES (5, 'Four');
INSERT INTO CLASS VALUES (6, 'Five');
INSERT INTO CLASS VALUES (7, 'Six');
INSERT INTO CLASS VALUES (8, 'Seven');
INSERT INTO CLASS VALUES (9, 'Eight');
INSERT INTO CLASS VALUES (10, 'Nine');
INSERT INTO CLASS VALUES (11, 'Ten');
INSERT INTO CLASS VALUES (12, 'Eleven');
INSERT INTO CLASS VALUES (13, 'Twelve');

----- Student Class Assign -----

UPDATE STUDENT SET Class_ID = 13 WHERE LOGIN_ID = 1;
UPDATE STUDENT SET Class_ID = 10 WHERE LOGIN_ID = 2;
UPDATE STUDENT SET Class_ID = 13 WHERE LOGIN_ID = 3;
UPDATE STUDENT SET Class_ID = 12 WHERE LOGIN_ID = 4;
UPDATE STUDENT SET Class_ID = 10 WHERE LOGIN_ID = 5;
UPDATE STUDENT SET Class_ID = 7 WHERE LOGIN_ID = 6;

----- Course -----

INSERT INTO COURSE VALUES (1, 'Bangla');
INSERT INTO COURSE VALUES (2, 'English');
INSERT INTO COURSE VALUES (3, 'General Math');
INSERT INTO COURSE VALUES (4, 'Higher Math');
INSERT INTO COURSE VALUES (5, 'Physics');
INSERT INTO COURSE VALUES (6, 'Chemistry');
INSERT INTO COURSE VALUES (7, 'Biology');
INSERT INTO COURSE VALUES (8, 'Computer');
INSERT INTO COURSE VALUES (9, 'General Science');
INSERT INTO COURSE VALUES (10, 'Social Science');

----- Course_Code -----

INSERT INTO COURSE_CODE VALUES (1, 'BAN 100');
INSERT INTO COURSE_CODE VALUES (2, 'ENG 200');
INSERT INTO COURSE_CODE VALUES (3, 'GM 300');
INSERT INTO COURSE_CODE VALUES (4, 'HM 400');
INSERT INTO COURSE_CODE VALUES (5, 'PHY 500');
INSERT INTO COURSE_CODE VALUES (6, 'CHEM 600');
INSERT INTO COURSE_CODE VALUES (7, 'BIO 700');
INSERT INTO COURSE_CODE VALUES (8, 'COM 800');
INSERT INTO COURSE_CODE VALUES (9, 'GS 900');
INSERT INTO COURSE_CODE VALUES (10, 'SS 110');
INSERT INTO COURSE_CODE VALUES (11, 'BAN 101');
INSERT INTO COURSE_CODE VALUES (12, 'ENG 201');
INSERT INTO COURSE_CODE VALUES (13, 'GM 301');
INSERT INTO COURSE_CODE VALUES (14, 'HM 401');
INSERT INTO COURSE_CODE VALUES (15, 'PHY 501');
INSERT INTO COURSE_CODE VALUES (16, 'CHEM 601');
INSERT INTO COURSE_CODE VALUES (17, 'BIO 701');
INSERT INTO COURSE_CODE VALUES (18, 'COM 801');
INSERT INTO COURSE_CODE VALUES (19, 'GS 901');
INSERT INTO COURSE_CODE VALUES (20, 'SS 111');

----- Course_Class_School -----

INSERT INTO COURSE_CLASS_SCHOOL VALUES (1, 1, 12, 1, 1);
INSERT INTO COURSE_CLASS_SCHOOL VALUES (2, 1, 13, 1, 11);
INSERT INTO COURSE_CLASS_SCHOOL VALUES (3, 3, 12, 1, 3);
INSERT INTO COURSE_CLASS_SCHOOL VALUES (4, 3, 13, 1, 13);
INSERT INTO COURSE_CLASS_SCHOOL VALUES (5, 2, 10, 2, 2);
INSERT INTO COURSE_CLASS_SCHOOL VALUES (6, 6, 11, 2, 6);
INSERT INTO COURSE_CLASS_SCHOOL VALUES (7, 4, 6, 3, 4);
INSERT INTO COURSE_CLASS_SCHOOL VALUES (8, 5, 7, 4, 5);
INSERT INTO COURSE_CLASS_SCHOOL VALUES (9, 7, 8, 5, 7);
INSERT INTO COURSE_CLASS_SCHOOL VALUES (10, 8, 9, 6, 8);

----- Enroll_Student -----

INSERT INTO ENROLL_STUDENT VALUES (1, 4, 1, TO_DATE('2014-01-01', 'SYYYY-MM-DD'));
INSERT INTO ENROLL_STUDENT VALUES (2, 2, 2, TO_DATE('2015-01-01', 'SYYYY-MM-DD'));
INSERT INTO ENROLL_STUDENT VALUES (3, 3, 3, TO_DATE('2015-01-01', 'SYYYY-MM-DD'));
INSERT INTO ENROLL_STUDENT VALUES (4, 4, 4, TO_DATE('2015-01-01', 'SYYYY-MM-DD'));
INSERT INTO ENROLL_STUDENT VALUES (5, 5, 5, TO_DATE('2015-01-01', 'SYYYY-MM-DD'));
INSERT INTO ENROLL_STUDENT VALUES (6, 6, 6, TO_DATE('2015-01-01', 'SYYYY-MM-DD'));
INSERT INTO ENROLL_STUDENT VALUES (7, 2, 1, TO_DATE('2014-01-01', 'SYYYY-MM-DD'));
INSERT INTO ENROLL_STUDENT VALUES (8, 6, 2, TO_DATE('2014-01-01', 'SYYYY-MM-DD'));
INSERT INTO ENROLL_STUDENT VALUES (9, 7, 3, TO_DATE('2015-01-01', 'SYYYY-MM-DD'));
INSERT INTO ENROLL_STUDENT VALUES (10, 8, 4, TO_DATE('2015-01-01', 'SYYYY-MM-DD'));
INSERT INTO ENROLL_STUDENT VALUES (11, 9, 5, TO_DATE('2015-01-01', 'SYYYY-MM-DD'));
INSERT INTO ENROLL_STUDENT VALUES (12, 10, 6, TO_DATE('2015-01-01', 'SYYYY-MM-DD'));

----- Enroll_Teacher -----

INSERT INTO ENROLL_TEACHER VALUES (1, 1, 1, TO_DATE('2012-01-02', 'SYYYY-MM-DD'), TO_DATE('2014-05-02', 'SYYYY-MM-DD'));
INSERT INTO ENROLL_TEACHER VALUES (2, 2, 1, TO_DATE('2013-02-01', 'SYYYY-MM-DD'), TO_DATE('2014-07-06', 'SYYYY-MM-DD'));
INSERT INTO ENROLL_TEACHER VALUES (3, 5, 2, TO_DATE('2013-01-02', 'SYYYY-MM-DD'), TO_DATE('2015-01-02', 'SYYYY-MM-DD'));
INSERT INTO ENROLL_TEACHER VALUES (4, 4, 3, TO_DATE('2013-03-01', 'SYYYY-MM-DD'), TO_DATE('2014-05-06', 'SYYYY-MM-DD'));
INSERT INTO ENROLL_TEACHER VALUES (5, 3, 4, TO_DATE('2013-01-01', 'SYYYY-MM-DD'), NULL);
INSERT INTO ENROLL_TEACHER VALUES (6, 5, 5, TO_DATE('2013-02-01', 'SYYYY-MM-DD'), NULL);
INSERT INTO ENROLL_TEACHER VALUES (7, 2, 4, TO_DATE('2012-01-02', 'SYYYY-MM-DD'), NULL);

----- Exam -----

INSERT INTO EXAM VALUES (1, 2, TO_DATE('27-12-2015','dd-MM-yyyy'), 1800, 50, '3rd part', NULL);
INSERT INTO EXAM VALUES (2, 3, TO_DATE('28-12-2015','dd-MM-yyyy'), 1800, 50, 'Chapters 5', NULL);
INSERT INTO EXAM VALUES (3, 5, TO_DATE('29-12-2015','dd-MM-yyyy'), 3600, 100, 'All chapters', NULL);
