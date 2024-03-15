# cs.ado.learn
repository for demonstrating tasks

# Test
The test is designed to work with PostgreSQL.

To test the result you need to edit the app.config file. 

In the connectionString line:
1. replace the Port value with the port of your PostgreSQL (by default 5432)
2. replace the Database value with the name of your Database (by default NameDatabase)

After launching the application, you will need to enter the database user logs and password

Then connect to the database and select the table to edit

The following actions with strings are allowed:
1. Addition. To do this, fill in the columns in the blank line and press the enter button or go to another line. If exceptions occur during the process, they should be taken into account and changes should be made to the columns.
2. Modification. To do this, you need to select any filled line and make changes to it. Changes will be written when you press the enter button or when you move to another line.
3. Removal. To do this, select any filled line and click the "Delete" button.