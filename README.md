NuCRUD.NET
==========

Generic Database Library (Multiple Connection Type) For .NET

# How To #

## Reference NuCRUD.NET library to project ##
- Download required library at https://github.com/nubuntu/NuCRUD.NET/blob/master/bin/Debug/NuCRUD.NET.dll
- Add reference NuCRUD.NET.dll to your project
- Ready to Work with NuCRUD.NET Library

## Connect to Database ##
![Alt text](http://www.youtube.com/watch?v=OY1vv7hQQCg)
- See connection string example in http://www.connectionstrings.com/
- Specify your Database Driver, in this case I use mysql connection http://www.connectionstrings.com/mysql/
- Test Your DB Connection

        Dim connstring = "Server=localhost;Database=nucrud;Uid=root;Pwd=root;"
        Dim db As New NuDB(NuDB.MYSQL, connstring)
        If db.connect Then
            MsgBox("Succesfully Connected")
        End If

## CRUD your windows form ##
- Add tablelayoutpanel control to form
- write simple code like below

        Dim connstring = "Server=localhost;Database=nucrud;Uid=root;Pwd=root;"
        Dim student As New NuTable
        With student
            .table = "siswa" 'name of table in database
            .title = "Student List" 'title of form
            ' Adding fields
            .fields.Add(New NuField("id", "ID", False, True)) 'id is primary key of table siswa
            .fields.Add(New NuField("nama", "Name"))
            ' or you can add field by
            Dim field As New NuField("alamat", "Address")
            field.searchable = True
            field.update = False ' can't be updated/not editable
            .fields.Add(field)
        End With
        Dim crud As New NuCRUD(NuDB.MYSQL, connstring, TableLayoutPanel1, student)
        'done
