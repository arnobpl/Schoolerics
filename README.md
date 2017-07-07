# Schoolerics

Introduction
============
This project contains a sample web application for the collaboration among students, teachers and schools. As a sample it only contains the backend part of the web application.


How to use
==========
## Prerequisites
The project code might be used in various environments after some sort of tweaking but it has been tested in a specific environment. The following tools has been used in the test environment:
  - **Programming IDE:** *Visual Studio 2015*
  - **Programming Framework:** *ASP.NET 4.5* (*MVC* is preferable but *Web Forms* should work just fine)
  - **Database Tool:** 
    - *Oracle Database 12c* with *Oracle Developer Tools for Visual Studio*
    - *Navicat* (optional, just needed to redesign the ERD of the database)

## Database Configuration
  - **Configure:** Please refer to `Database/_Readme.txt` file to install or uninstall database configuration.
  - **Edit:** You can directly edit the database scripts in `Database/Scripts` folder. But for convenience, please open `Database/Schoolerics.ndm` file with *Navicat* and edit the database within the app.

## Run Project
After performing the above, you can run easily the project. Please do the followings to run the project:
  - Open `Schoolerics.sln` with *Visual Studio 2015*.
  - Add a new *ASP.NET* project to the solution. You can use either *MVC* or *Web Forms* but *MVC* is preferable.
  - Add some code to integrate with `Schoolerics.Model`. Please refer to [Code Overview](#code-overview) section for more details.


Code Overview
=============
## Data Connector (`Schoolerics.Models/DataConnector/DataAccessLayer.cs`)
It connects the other parts of this project with Oracle database. Cool thing is: the data connector alone can be used in other projects for the same purpose. In this case, you just need to remove the references of `Schoolerics` and all should be fine.

## User Authorization (`Schoolerics.Models/Controller/UserAuthorization.cs`)
To sign up, login or logout, you need the code. All the methods of `UserAuthorization` have their own documentations, though method names are self-explanatory.

## Account (`Schoolerics.Models/Controller/Account`)
There are 3 types of account: (1) Student, (2) Teacher and (3) School. Most of the methods have their own documentations, though method names are self-explanatory.

## Encryption (`Schoolerics.Models/Utility/Encryption.cs`)
It has been used in store encrypted passwords in the database. But using it, you may encrypt other things.


Features
========
## Registration
  - First only basic login info and then details as the following:
    - As student (school, class info added by school account)
    - As teacher (school info added by school account)
    - As school/headmaster/admin

## For student
  - View own profile
  - Edit address, contact
  - View school, class info
  - View enrolled courses
  - View exam routine
  - View offline exam results

## For teacher
  - View own profile
  - Edit contact
  - View school info
  - View enrolled courses
  - Set exam routine
  - Set offline exam results

## For school/headmaster/admin
  - View own profile (school)
  - Edit address, contact, description, Url for school
  - Assign class course enrolment
  - Assign students in school
  - Assign teachers in school
  - Assign student, course, class enrolment
  - Assign teacher, course enrolment

