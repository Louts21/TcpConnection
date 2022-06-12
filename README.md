# Files:
ExporterProgram.cs - Client
TCPServer.cs - Server

# Requirements:
Windows 10, Visual Studio Code (VSC), DotNet, Unity 2021.3.3f1

# Steps:
1. Install DotNet (https://dotnet.microsoft.com/en-us/download)
2. Create a program in a folder through VSC terminal
	- dotnet new console -o sample1
	- cd sample1
3. Delete the default .cs file (don't delete sample1.csproj)
4. Drag and drop ExporterPrgram.cs into sample1 folder
5. Create a Unity project and drag and drop TCPServer.cs into the project
6. Run the unity program (you should see "Server is listening!" in unity console)
7. Write "dotnet build" and execute it into the terminal of VSC
8. After the program is build. Write "dotnet run" into the terminal
