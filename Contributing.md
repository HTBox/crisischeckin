Getting started contributing
=============

These are the steps I followed to get crisischeckin up and running on my development machine. I was using Visual Studio 2013, but 2012 should work just as well.

* Clone the repo to your local machine
* Open the crisischeckin\crisischeckin.sln file in Visual Studio
* Right click on the solution item at the root of the solution explorer and choose "Manage NuGet packages for solution"
    * In the NuGet window, click on the "Restore" button at the top to install all the NuGet packages needed. This will also install Entity Framework cmdlets for PowerShell (package manager console) which you'll need for setting up the database
* Close Visual Studio
* Open the crisischeckin.sln file in Visual Studio again (this is to make sure the new cmdlets will be available in the package manager console)
* Run "Rebuild solution" from the Build menu (it should complete with no errors)
* Open up the Package Manager Console (Tools -> Library Package Manager -> Package Manager Console)
* Run the command `Update-Database -ProjectName models -StartupProject crisicheckinweb`  in the package manager console window. This should apply the code-based migrations and complete with no errors (but I did get yellow warnings that don't seem to have caused any problems)
* Run all the tests (Test -> Run -> All Tests), they should all pass without problems
* Set the start up project to crisicheckinweb
* Press F5 to run the site (or choose Debug -> Start Debugging)

When the site starts up, you should be presented with a log in page. You can use the Register link at the bottom to start making new accounts.

To log in as an administrator you can use "Administrator" and "P@$$w0rd" for the user name and password.

To get test data into your database:
* Open Solution Explorer in Visual Studio
* Find the file `Models/Migrations/DefaultData.sql.txt` and copy its contents into the buffer. 
* Open up Server Explorer tab in your Visual Studio and expand the connection to CrisisCheckin database.
* Right-click on any table and choose `New Query`. 
* Paste script into query window and run it.

This script will give you information on three people who volunteered to work in two different disaster areas each in several days, the list is visible under Administrator account.

What if something goes wrong?
====

The biggest problems I had were with broken databases. This can happen if you run the application before running the "Update-Database" cmdlet or you delete the database files from the App_Data folder without first removing them from SQL Express.

To fix that, I'd just throw away the stuff in the database and start again by

* Delete the .mdf and .ldf files inside `crisischeckin\crisischeckin\crisicheckinweb\App_Data` (if they're not already gone)
* Open a Visual Studio command prompt (as it says in [this StackOverflow answer](http://stackoverflow.com/questions/13275054/ef5-cannot-attach-the-file-0-as-database-1/16339164#16339164)) then run the commands
    * sqllocaldb.exe stop v11.0
    * sqllocaldb.exe delete v11.0

After doing that, you should be able to run the Update-Database command again to get back to a clean database. 

Jumping in
====

Once you've gotten the projet running in your local environment, check out these issues https://github.com/HTBox/crisischeckin/issues?labels=jump-in&state=open any of which would be a great place to get started helping our project.
