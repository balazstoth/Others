1. Run the Install.bat file as Administrator! 
      (Nothing will be displayed, it just creates the encryption key)
2. IMPORTANT - Delete the Install.bat because of security reasons!
3. Start the .exe file

Instructions:
Server field: 		Name of the server - Like: (localdb)\MSSQLLocalDB
UserName field:		Admin's username
Password field:		Admin's password
Period field:		Period time in minutes. Determines how often the program checks if there's a new database.
DB Name field:		It's a filter for select the approriate databases. There are 3 different ways how to use it:
				I.   "*keyWord": select each databases which name ends with "keyWord"
				II.  "keyWord*": select each databases which name starts with "keyWord"
				III. "keyWord" : select each databases which name contains "keyWord"
			If the fields is empty, program selects all the available databases!
Save button:		Saves the values of each textBoxes to the XML file.
Restore fields:		Resets each textBox fields to the currently stored values.
Restart:		Checks the server and restart the timer.