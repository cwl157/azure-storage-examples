# Azure Storage Examples
Example of different azure storage options with a CRUD app. There are 4 different storage examples available. Local json file, json file using azure files, azure sql, and azure cosmos db. The Constants.cs file contains placeholders for all the keys and connection strings required. To change which storage option is being used, replace the stubs in Constants.cs and change the repository variable in Program.cs.
# Setup
The MusicManager.Setup app will setup the necessary data store objects, files, containers, db tables, etc. in the azure resource you choose. The azure resources must already exist.
# The application
The example application stores a collection of music including artist, album title, year, format, etc. The example requires dotnet core 3.1. 
