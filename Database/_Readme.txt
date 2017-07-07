Installation:

	- Open 'SQL Plus' app and run the following commands:

		/ as sysdba
		alter session set container = pdborcl;
		create user Schoolerics identified by 1;
		grant all privileges to Schoolerics;
		conn Schoolerics/1@pdborcl;

	- Now open 'SQL Developer' app, create a new connection to Schoolerics, and run all the scripts serially from "Scripts" folder.


Uninstallation:

	- Open 'SQL Plus' app and run the following commands:

		/ as sysdba
		alter session set container = pdborcl;
		drop user Schoolerics cascade;

