NuCRUD.NET
==========

Generic Database Library (Multiple Connection Type) For .NET

# How To #

## Reference NuCRUD.NET library to project ##
- Download required library at https://github.com/nubuntu/NuCRUD.NET/blob/master/bin/Debug/NuCRUD.NET.dll
- Add reference NuCRUD.NET.dll to your project
- Ready to Work with NuCRUD.NET Library

## Connect to Database ##
- See connection string example in http://www.connectionstrings.com/
- Specify your Database Driver, in this case I use mysql connection http://www.connectionstrings.com/mysql/
- Test Your DB Connection

        Dim connstring = "Server=localhost;Database=nucrud;Uid=root;Pwd=root;"
        Dim db As New NuDB(NuDB.MYSQL, connstring)
        If db.connect Then
            MsgBox("Succesfully Connected")
        End If

