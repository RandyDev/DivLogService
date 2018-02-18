Imports System.Data
'Imports System.Data.Sql
'Imports System.Data.SqlClient

Public Class TimePuncheDAL
#Region "get TimePunche stuff"
    Public Function getTimePuncheByID(ByVal tpID As String) As TimePunche
        Dim tp As New TimePunche
        Dim dba As New DBAccess()
        dba.CommandText = "SELECT TOP 1 TimePunche.DateWorked, TimePUnche.LocationID, Department.Name AS DepartmentName, EmployeeID as empID, TimePunche.IsClosed,  " & _
            "TimePunche.ID as tpID, TimePunche.DepartmentID " & _
            "FROM TimePunche INNER JOIN " & _
            "Department ON TimePunche.DepartmentID = Department.ID " & _
            "WHERE (TimePunche.ID = @tpID) " & _
            "ORDER BY TimePunche.DateWorked DESC "
        dba.AddParameter("@tpID", tpID)
        Dim ds As DataSet = dba.ExecuteDataSet
        If ds.Tables(0).Rows.Count > 0 Then
            Dim rw As DataRow = ds.Tables(0).Rows(0)
            tp.ID = rw.Item("tpID")
            tp.EmployeeID = rw.Item("empID")
            tp.DateWorked = rw.Item("DateWorked")
            tp.DepartmentID = rw.Item("DepartmentID")
            tp.DepartmentName = rw.Item("DepartmentName")
            tp.LocationID = rw.Item("LocationID")
            tp.IsClosed = rw.Item("IsClosed")
            dba.CommandText = "SELECT ID AS tioID, TimeIn, TimeOut, HoursWorked, isHourly,JobDescriptionID FROM TimeInOut WHERE TimepuncheID = @tpID ORDER BY TimeIn ASC"
            dba.AddParameter("@tpID", tpID)
            Dim tcds As DataSet = dba.ExecuteDataSet
            If tcds.Tables(0).Rows.Count > 0 Then
                Dim tioList As New List(Of TimeInOut)
                For Each tc As DataRow In tcds.Tables(0).Rows
                    Dim tio As New TimeInOut
                    tio.TimeIn = tc.Item("TimeIn")
                    tio.TimeOut = tc.Item("TimeOut")
                    tio.TimepuncheID = tp.ID
                    tio.ID = tc.Item("tioID")
                    tio.HoursWorked = tc.Item("HoursWorked")
                    tio.isHourly = tc.Item("isHourly")
                    If IsDBNull(tc.Item("JobDescriptionID")) Then
                        tio.JobDescriptionID = Utilities.zeroGuid
                    Else
                        tio.JobDescriptionID = tc.Item("JobDescriptionID")
                    End If
                    tioList.Add(tio)
                Next
                tp.tpList = tioList
            End If
        End If
        Return tp
    End Function
    Public Function getTimeInOutByID(ByVal tioID As Guid) As TimeInOut
        Dim retTIO As New TimeInOut
        Dim dba As New DBAccess
        dba.CommandText = "SELECT ID AS tioID,TimepuncheID, TimeIn, TimeOut, HoursWorked, isHourly,JobDescriptionID FROM TimeInOut WHERE ID = @ID"
        dba.AddParameter("@ID", tioID)
        Dim dt As DataTable = dba.ExecuteDataSet.Tables(0)
        If dt.Rows.Count > 0 Then
            Dim tc As DataRow = dt.Rows(0)
            retTIO.TimeIn = tc.Item("TimeIn")
            retTIO.TimeOut = tc.Item("TimeOut")
            retTIO.TimepuncheID = tc.Item("TimepuncheID")
            retTIO.ID = tc.Item("tioID")
            retTIO.HoursWorked = tc.Item("HoursWorked")
            retTIO.isHourly = tc.Item("isHourly")

            If (IsDBNull(tc.Item("JobDescriptionID"))) Then
                retTIO.JobDescriptionID = Utilities.zeroGuid
            Else
                retTIO.JobDescriptionID = New Guid(tc.Item("JobDescriptionID").ToString)
            End If
        End If
        Return retTIO


    End Function
    Public Function getMostRecentTimePunchByEmpID(ByVal empid As Guid) As List(Of TimePunche)
        Dim tplist As New List(Of TimePunche)
        Dim dba As New DBAccess()
        dba.CommandText = "SELECT TOP 1 TimePunche.DateWorked, Department.Name AS DepartmentName, TimePunche.IsClosed,  " & _
            "TimePunche.ID as tpID, TimePunche.DepartmentID " & _
            "FROM TimePunche INNER JOIN " & _
            "Department ON TimePunche.DepartmentID = Department.ID " & _
            "WHERE (TimePunche.EmployeeID = @empID) " & _
            "ORDER BY TimePunche.DateWorked DESC, isClosed "

        dba.AddParameter("@empID", empid)
        Dim ds As DataSet = dba.ExecuteDataSet
        If ds.Tables(0).Rows.Count > 0 Then
            Dim rw As DataRow = ds.Tables(0).Rows(0)
            Dim tp As New TimePunche
            tp.ID = rw.Item("tpID")
            tp.EmployeeID = empid
            tp.DateWorked = rw.Item("DateWorked")
            tp.DepartmentID = rw.Item("DepartmentID")
            tp.DepartmentName = rw.Item("DepartmentName")
            tp.IsClosed = rw.Item("IsClosed")
            dba.CommandText = "SELECT ID AS tioID, TimeIn, TimeOut,isHourly FROM TimeInOut WHERE TimepuncheID = @tpID ORDER BY TimeIn ASC"
            dba.AddParameter("@tpID", tp.ID)
            Dim tcds As DataSet = dba.ExecuteDataSet
            If tcds.Tables(0).Rows.Count > 0 Then
                Dim tioList As New List(Of TimeInOut)
                For Each tc As DataRow In tcds.Tables(0).Rows
                    Dim tio As New TimeInOut
                    tio.TimeIn = tc.Item("TimeIn")
                    tio.TimeOut = tc.Item("TimeOut")
                    tio.TimepuncheID = tp.ID
                    tio.ID = tc.Item("tioID")
                    tio.isHourly = tc.Item("isHourly")
                    tioList.Add(tio)
                Next
                tp.tpList = tioList
            End If
            tplist.Add(tp)
        End If
        Return tplist
    End Function

    Public Function getTimePunchesByEmpIDandPayPeriod(ByVal empID As Guid, ByVal sDate As DateTime, Optional ByVal dayz As Integer = 14) As List(Of TimePunche)
        Dim tpList As List(Of TimePunche) = New List(Of TimePunche)
        Dim tpdal As New TimePuncheDAL
        sDate = tpdal.getPayStartDate(sDate)
        Dim eDate As Date = DateAdd(DateInterval.Day, dayz, sDate)
        ' OPTIONAL or LOCATION based, adjust sDate and eDate for offset
        Dim lDAL As New locaDAL()
        Dim locaOffset As Integer = lDAL.getLocaOffset(empID)
        If locaOffset <> 0 Then
            sDate = DateAdd(DateInterval.Hour, locaOffset, sDate)
            eDate = DateAdd(DateInterval.Hour, locaOffset, eDate)
        End If
        Dim dba As New DBAccess()
        dba.CommandText = "SELECT DISTINCT TP.ID AS tpID, TP.DateWorked, Department.Name AS DepartmentName, TP.IsClosed, TP.DepartmentID, TP.LocationID " & _
           "FROM TimePunche TP INNER JOIN " & _
            "Department ON TP.DepartmentID = Department.ID INNER JOIN " & _
            "TimeInOut ON TP.ID = TimeInOut.TimepuncheID " & _
            "WHERE (TP.EmployeeID = @empID) AND (TimeInOut.TimeIn < @eDate) AND (TimeInOut.TimeIn >= @sDate) " & _
            "ORDER BY TP.DateWorked DESC, TP.isClosed"
        '        dba.CommandText = "SELECT distinct TimePunche.ID AS tpID, TimeInOut.TimeIn, Department.Name AS DepartmentName, timepunche.isclosed, timepunche.DepartmentID " & _
        '            "FROM TimePunche INNER JOIN " & _
        '            "TimeInOut ON TimePunche.ID = TimeInOut.TimepuncheID inner join " & _
        '            "department on timepunche.departmentID = department.id  " & _
        '            "WHERE (TimePunche.DateWorked > @sdate) AND (TimePunche.DateWorked < @edate)  " & _
        '            "AND timepunche.employeeid=@empID " & _
        '            "ORDER BY TimeInOut.TimeIn DESC"
        dba.AddParameter("@empID", empID)
        dba.AddParameter("@sDate", sDate)
        dba.AddParameter("@eDate", eDate)
        Dim ds As DataSet = dba.ExecuteDataSet
        If ds.Tables(0).Rows.Count > 0 Then
            For Each rw As DataRow In ds.Tables(0).Rows
                Dim tp As New TimePunche()
                tp.ID = rw.Item("tpID")
                tp.EmployeeID = empID
                tp.DateWorked = rw.Item("DateWorked")
                'tp.DateWorked = FormatDateTime(rw.Item("TimeIn"), DateFormat.ShortDate)
                tp.DepartmentID = rw.Item("DepartmentID")
                tp.DepartmentName = rw.Item("DepartmentName")
                If Not IsDBNull(rw.Item("LocationID")) Then
                    tp.LocationID = rw.Item("LocationID")
                    tp.LocationName = lDAL.getLocationNameByID(tp.LocationID.ToString())
                End If
                tp.IsClosed = rw.Item("IsClosed")

                dba.CommandText = "SELECT ID AS tioID, TimeIn, TimeOut, HoursWorked, isHourly FROM TimeInOut WHERE TimepuncheID = @tpID ORDER BY TimeIn ASC"
                dba.AddParameter("@tpID", tp.ID)
                Dim tcds As DataSet = New DataSet
                Try
                    tcds = dba.ExecuteDataSet
                Catch ex As Exception
                    Dim err As String = ex.Message
                End Try
                If tcds.Tables(0).Rows.Count > 0 Then
                    Dim tioList As New List(Of TimeInOut)
                    For Each tc As DataRow In tcds.Tables(0).Rows
                        Dim tio As New TimeInOut
                        tio.TimeIn = tc.Item("TimeIn")
                        tio.TimeOut = tc.Item("TimeOut")
                        tio.TimepuncheID = tp.ID
                        tio.ID = tc.Item("tioID")
                        tio.HoursWorked = tc.Item("HoursWorked")
                        tio.isHourly = tc.Item("isHourly")
                        tioList.Add(tio)
                    Next
                    tp.tpList = tioList
                End If
                tpList.Add(tp)
            Next
        Else
            '            tpList = getMostRecentTimePunchByEmpID(empID)
        End If
        Return tpList
    End Function

    Public Function getTimePunchesByEmpIDandPayWeek(ByVal empID As Guid, ByVal sDate As Date, ByVal edate As Date) As List(Of TimePunche)
        Dim tpList As List(Of TimePunche) = New List(Of TimePunche)
        Dim tpdal As New TimePuncheDAL
        ' OPTIONAL or LOCATION based, adjust sDate and eDate for offset
        Dim lDAL As New locaDAL()
        '************* is this off if employee works in two separate timeoffset locations
        Dim locaOffset As Integer = lDAL.getLocaOffset(empID)
        If locaOffset <> 0 Then
            sDate = DateAdd(DateInterval.Hour, locaOffset, sDate)
            edate = DateAdd(DateInterval.Hour, locaOffset, edate)
        End If

        '        sDate = DateAdd(DateInterval.Day, -1, sDate)

        Dim dba As New DBAccess()
        'dba.CommandText = "SELECT DISTINCT TOP (100) PERCENT TP.ID AS tpID, TP.DateWorked, dbo.Department.Name AS DepartmentName, TP.IsClosed, TP.DepartmentID " & _
        '    "FROM dbo.TimePunche AS TP INNER JOIN " & _
        '    "dbo.Department ON TP.DepartmentID = dbo.Department.ID INNER JOIN " & _
        '    "dbo.TimeInOut ON TP.ID = dbo.TimeInOut.TimepuncheID " & _
        '    "WHERE (TP.EmployeeID = @empID) AND (dbo.TimeInOut.TimeIn < @eDate) AND (dbo.TimeInOut.TimeIn >= @sDate) OR " & _
        '    "(TP.EmployeeID = @empID) AND (NOT (TP.ID IN " & _
        '    "(SELECT TimepuncheID " & _
        '    "FROM dbo.TimeInOut AS TimeInOut_1))) " & _
        '    "ORDER BY TP.DateWorked DESC, TP.IsClosed "

        dba.CommandText = "SELECT DISTINCT TP.ID AS tpID, TP.DateWorked, Department.Name AS DepartmentName, TP.IsClosed, TP.DepartmentID, TP.LocationID " & _
           "FROM TimePunche TP INNER JOIN " & _
            "Department ON TP.DepartmentID = Department.ID INNER JOIN " & _
            "TimeInOut ON TP.ID = TimeInOut.TimepuncheID " & _
            "WHERE (TP.EmployeeID = @empID) AND (TimeInOut.TimeIn < @eDate) AND (TimeInOut.TimeIn >= @sDate) " & _
            "ORDER BY TP.DateWorked DESC, TP.isClosed"
        '        dba.CommandText = "SELECT distinct TimePunche.ID AS tpID, TimeInOut.TimeIn, Department.Name AS DepartmentName, timepunche.isclosed, timepunche.DepartmentID " & _
        '            "FROM TimePunche INNER JOIN " & _
        '            "TimeInOut ON TimePunche.ID = TimeInOut.TimepuncheID inner join " & _
        '            "department on timepunche.departmentID = department.id  " & _
        '            "WHERE (TimePunche.DateWorked > @sdate) AND (TimePunche.DateWorked < @edate)  " & _
        '            "AND timepunche.employeeid=@empID " & _
        '            "ORDER BY TimeInOut.TimeIn DESC"
        dba.AddParameter("@empID", empID)
        dba.AddParameter("@sDate", sDate)
        dba.AddParameter("@eDate", edate)
        Dim ds As DataSet = dba.ExecuteDataSet
        '  ds.Table(0) should contain a list of distinct TimePunche records if emp is in none offset, 
        '  but worked in offset location , some hours could be lost
        '*********************************************************
        If ds.Tables(0).Rows.Count > 0 Then

            ' the first day preceeds the payperiod start ... if this is not an offset location
            '            If Not ds.Tables(0).Rows(0).Item("LocationID") Then 'resolves to offset location 
            ' delete it 
            '            ds.Tables(0).Rows(0).Delete()
            '        End If


            For Each rw As DataRow In ds.Tables(0).Rows
                Dim tp As New TimePunche()
                tp.ID = rw.Item("tpID")
                tp.EmployeeID = empID
                tp.DateWorked = rw.Item("DateWorked")
                'tp.DateWorked = FormatDateTime(rw.Item("TimeIn"), DateFormat.ShortDate)
                tp.DepartmentID = rw.Item("DepartmentID")
                tp.DepartmentName = rw.Item("DepartmentName")
                ' ****** pick up location here
                tp.IsClosed = rw.Item("IsClosed")
                dba.CommandText = "SELECT ID AS tioID, TimeIn, TimeOut, HoursWorked, isHourly FROM TimeInOut WHERE TimepuncheID = @tpID ORDER BY TimeIn ASC"
                dba.AddParameter("@tpID", tp.ID)
                Dim tcds As DataSet = dba.ExecuteDataSet
                If tcds.Tables(0).Rows.Count > 0 Then
                    Dim tioList As New List(Of TimeInOut)
                    For Each tc As DataRow In tcds.Tables(0).Rows
                        Dim tio As New TimeInOut
                        tio.TimeIn = tc.Item("TimeIn")
                        tio.TimeOut = tc.Item("TimeOut")
                        tio.TimepuncheID = tp.ID
                        tio.ID = tc.Item("tioID")
                        tio.HoursWorked = tc.Item("HoursWorked")
                        tio.isHourly = tc.Item("isHourly")
                        tioList.Add(tio)
                    Next
                    tp.tpList = tioList
                End If
                tpList.Add(tp)
            Next
        Else
            '            tpList = getMostRecentTimePunchByEmpID(empID)
        End If
        Return tpList
    End Function



#End Region



#Region "utils"
    Public Function getEmployeeTime(ByVal empID As Guid, ByVal sdate As Date, Optional ByVal edate As Date = Nothing) As Array
        Dim arPay(6) As Decimal

        Dim dp As String = String.Empty
        Dim empTime As Decimal = 0
        Dim tpDAL As New TimePuncheDAL
        Dim TimeSheet As List(Of TimePunche) = tpDAL.getTimePunchesByEmpIDandPayWeek(empID, sdate, edate)
        'make sure counter starts on day one of payperiod
        sdate = getPayStartDate(sdate)
        Dim thisTIO As Decimal = 0
        Dim thisTIOHourly As Decimal = 0
        Dim overtime As Decimal = 0
        For Each timeCard As TimePunche In TimeSheet
            '            Dim surrogate As Date = "1/1/1900"
            For Each tio As TimeInOut In timeCard.tpList
                thisTIO += tio.HoursWorked
                If tio.isHourly Then
                    thisTIOHourly += tio.HoursWorked
                End If
            Next    'tio
            If timeCard.DateWorked < DateAdd(DateInterval.Day, 7, sdate) Then 'first week
                arPay(0) += thisTIO
                arPay(4) += thisTIOHourly
                thisTIO = 0
                thisTIOHourly = 0
            Else
                arPay(2) += thisTIO
                arPay(5) += thisTIOHourly
                thisTIO = 0
                thisTIOHourly = 0
            End If
        Next    'timeCard
        If arPay(0) > 40 Then arPay(1) = arPay(0) - 40
        If arPay(2) > 40 Then arPay(3) = arPay(2) - 40
        Return arPay
        '0 = reg hrs wk 1 : 1 = ot hrs wk 1 : 2 reg hrs wk 2 : 3 = ot hrs wk 2 : 4 = hrly hours wk 1 : 5 = hrly hours wk 2

    End Function

    Public Function getPayStartDate(ByVal vdate As Date) As Date
        Dim BenchMarkDate As Date = "1/09/2000"
        vdate = FormatDateTime(vdate, DateFormat.ShortDate) ' "3/2/2011"
        Dim diff As Long = DateDiff(DateInterval.Day, BenchMarkDate, vdate)
        Dim modulus As Integer = diff Mod 14
        Dim startDate As Date = DateAdd(DateInterval.Day, -modulus, vdate)
        Dim endDate As Date = DateAdd(DateInterval.Day, 13, startDate)
        Return startDate
    End Function
#End Region

#Region "TimePunche CRUD"
    Public Function insertTimePunche(ByVal tp As TimePunche) As String
        Dim retstr As String = String.Empty
        Dim dba As New DBAccess()
        tp.IsClosed = False
        ''setup
        dba.CommandText = "INSERT INTO TimePunche (EmployeeID, DepartmentID, LocationID, DateWorked, IsClosed, ID) " & _
            "VALUES (@EmployeeID, @DepartmentID, @LocationID, @DateWorked, @IsClosed, @ID)"
        dba.AddParameter("@EmployeeID", tp.EmployeeID)
        dba.AddParameter("@DepartmentID", tp.DepartmentID)
        dba.AddParameter("@LocationID", tp.LocationID)
        dba.AddParameter("@DateWorked", tp.DateWorked)
        dba.AddParameter("@IsClosed", tp.IsClosed)
        dba.AddParameter("@ID", tp.ID)
        Try
            dba.ExecuteNonQuery()
            retstr = tp.ID.ToString
        Catch ex As Exception
            retstr = ex.Message
        End Try
        Dim ardal As New AuditRepLogDAL()
        If FormatDateTime(tp.DateWorked, DateFormat.ShortDate) = FormatDateTime(Date.Now(), DateFormat.ShortDate) Then
            ardal.doTimePuncheRepLog(tp)
        End If
        Dim edal As New empDAL()
        ardal.UpdateAudit("TimePunche", "n/a", tp.DateWorked & " - " & edal.getEmployeeName(tp.EmployeeID), "NEW RECORD", tp.ID)
        Return retstr
    End Function

    Public Function updateTimePunche(ByVal tp As TimePunche) As String
        Dim errStr As String = String.Empty
        Dim dba As New DBAccess()
        dba.CommandText = "UPDATE TimePunche SET EmployeeID=@EmployeeID, DepartmentID=@DepartmentID, DateWorked=@DateWorked, IsClosed=@IsClosed WHERE ID=@ID"
        dba.AddParameter("@EmployeeID", tp.EmployeeID)
        dba.AddParameter("@DepartmentID", tp.DepartmentID)
        dba.AddParameter("@DateWorked", tp.DateWorked)
        dba.AddParameter("@IsClosed", tp.IsClosed)
        dba.AddParameter("@ID", tp.ID)
        Try
            dba.ExecuteNonQuery()
        Catch ex As Exception
            errStr = ex.Message
        End Try
        Dim ardal As New AuditRepLogDAL()
        ardal.doTimePuncheRepLog(tp)
        Return errStr
    End Function

    Public Function updateTimePuncheDepartment(ByVal tp As TimePunche) As String
        Dim errStr As String = String.Empty
        Dim dba As New DBAccess()
        dba.CommandText = "UPDATE TimePunche SET DepartmentID=@DepartmentID WHERE ID=@ID"
        dba.AddParameter("@DepartmentID", tp.DepartmentID)
        dba.AddParameter("@ID", tp.ID)
        Try
            dba.ExecuteNonQuery()
        Catch ex As Exception
            errStr = ex.Message
        End Try
        Dim ardal As New AuditRepLogDAL()
        ardal.doTimePuncheRepLog(tp)
        Return errStr
    End Function

    Public Function updateTimePuncheIsClosed(ByVal tpID As Guid, ByVal isClosed As Boolean) As String
        Dim errStr As String = String.Empty
        Dim dba As New DBAccess()
        dba.CommandText = "UPDATE TimePunche SET isClosed=@isClosed WHERE ID=@ID"
        dba.AddParameter("@isClosed", isClosed)
        dba.AddParameter("@ID", tpID)
        Try
            dba.ExecuteNonQuery()
        Catch ex As Exception
            errStr = ex.Message
        End Try

        Dim tp As TimePunche = getTimePuncheByID(tpID.ToString)
        Dim ardal As New AuditRepLogDAL()
        ardal.doTimePuncheRepLog(tp)
        Return errStr
    End Function


    Public Function deleteTimePunche(ByVal tpID As Guid) As String
        Dim errStr As String = Nothing
        Dim ardal As New AuditRepLogDAL
        Dim edal As New empDAL()
        Dim dba As New DBAccess()
        'delete dependencies
        dba.CommandText = "SELECT ID FROM TimeInOut WHERE TimePuncheID=@ID"
        dba.AddParameter("@ID", tpID)
        Dim dt As DataTable = dba.ExecuteDataSet.Tables(0)
        If dt.Rows.Count > 0 Then
            For Each rw As DataRow In dt.Rows
                errStr = deleteTIO(rw.Item(0))
            Next
        End If
        dba.CommandText = "SELECT EmployeeID, DepartmentID, DateWorked, IsClosed FROM TimePunche WHERE ID=@ID"
        dba.AddParameter("@ID", tpID)
        dt = dba.ExecuteDataSet.Tables(0)
        Dim tp As New TimePunche
        If dt.Rows.Count > 0 Then
            For Each rw As DataRow In dt.Rows
                tp.EmployeeID = rw.Item(0)
                tp.DepartmentID = rw.Item(1)
                tp.DateWorked = rw.Item(2)
                tp.IsClosed = rw.Item(3)
                tp.ID = tpID
                ardal.doTimePuncheRepLog(tp, 1)
            Next
        End If
        If errStr IsNot Nothing Then
            dba.CommandText = "Delete FROM TimePunche WHERE ID=@ID"
            dba.AddParameter("@ID", tpID)
            dba.ExecuteNonQuery()
        End If
        ardal.UpdateAudit("TimePunche", tp.DateWorked & " - " & edal.getEmployeeName(tp.EmployeeID), "DELETE RECORD", "n/a", tp.ID)
        Return errStr
    End Function
#End Region

    ' creates new TimePunche record w/ new TimeInOut record with start time of tp.dateworked:current time
    Public Function CreateTimeCard(ByVal tp As TimePunche) As String
        'create new timepunche
        Dim newTPid As String = insertTimePunche(tp)
        'If Utilities.IsValidGuid(newTPid) Then
        '    tp.ID = New Guid(newTPid)
        '    'else we have issues
        'End If

        'Dim tio As New TimeInOut
        'tio.TimepuncheID = tp.ID
        'tio.TimeIn = tp.DateWorked & " " & Date.Now().ToShortTimeString
        'tio.TimeOut = "1/1/1900"
        'Dim utl As New Utilities
        ''        tio.isHourly = utl.isHourly(JobDescriptionID)
        ''        tio.JobDescriptionID = JobDescriptionID
        'Dim errTIO As String = insertTIO(tio)

        'Dim ardal As New AuditRepLogDAL()
        'ardal.doTimePuncheRepLog(tp)
        'spin up an 'empDAL instance'
        Return tp.ID.ToString
    End Function


#Region "TimeInOut CRUD"
    Public Function insertTIO(ByVal tio As TimeInOut) As String
        Dim errStr As String = String.Empty
        Dim dba As New DBAccess()
        tio.ID = Guid.NewGuid()
        dba.CommandText = "INSERT INTO TimeInOut (TimeIn, TimeOut, TimePuncheID, ID, isHourly,JobDescriptionID) " &
            "VALUES (@TimeIn, @TimeOut, @TimePuncheID, @ID, @isHourly, @JobDescriptionID)"
        dba.AddParameter("@TimeIn", tio.TimeIn)
        dba.AddParameter("@TimeOut", tio.TimeOut)
        dba.AddParameter("@TimePuncheID", tio.TimepuncheID)
        dba.AddParameter("@ID", tio.ID)
        dba.AddParameter("@isHourly", tio.isHourly)
        dba.AddParameter("@JobDescriptionID", tio.JobDescriptionID)
        Try
            dba.ExecuteNonQuery()
            errStr = tio.ID.ToString
        Catch ex As Exception
            errStr = ex.Message
        End Try
        Dim ardal As New AuditRepLogDAL()
        ardal.doTimeInOutRepLog(tio)
        ardal.UpdateAudit("TimeInOut", "n/a", tio.TimeIn & " - " & tio.TimeOut & " - " & tio.isHourly, "TimeIn - TimeOut - isHourly", tio.ID)
        Return errStr
    End Function

    Public Function updateTIO(ByVal tio As TimeInOut) As String
        Dim errStr As String = String.Empty
        Dim ardal As New AuditRepLogDAL()
        Dim oldtio As New TimeInOut
        Dim dba As New DBAccess()
        dba.CommandText = "SELECT TimeIn, TimeOut, isHourly FROM TimeInOut WHERE ID = @ID"
        dba.AddParameter("@ID", tio.ID)
        Dim dt As DataTable = dba.ExecuteDataSet.Tables(0)
        oldtio.TimeIn = dt.Rows(0).Item(0)
        oldtio.TimeOut = dt.Rows(0).Item(1)
        oldtio.isHourly = dt.Rows(0).Item(2)
        If tio.TimeIn <> oldtio.TimeIn Then ardal.UpdateAudit("TimeInOut", oldtio.TimeIn.ToString, tio.TimeIn.ToString, "TimeIn", tio.ID)
        If tio.TimeOut <> oldtio.TimeOut Then ardal.UpdateAudit("TimeInOut", oldtio.TimeOut.ToString, tio.TimeOut.ToString, "TimeOut", tio.ID)
        If tio.isHourly <> oldtio.isHourly Then ardal.UpdateAudit("TimeInOut", oldtio.isHourly.ToString, tio.isHourly.ToString, "IsHourly", tio.ID)
        dba.CommandText = "UPDATE TimeInOut SET TimeIn=@TimeIn, TimeOut=@TimeOut, IsHourly=@IsHourly, JobDescriptionID= @JobDescriptionID WHERE ID=@ID "
        dba.AddParameter("@TimeIn", tio.TimeIn)
        dba.AddParameter("@TimeOut", tio.TimeOut)
        dba.AddParameter("@IsHourly", tio.isHourly)
        dba.AddParameter("@ID", tio.ID)
        dba.AddParameter("@JobDescriptionID", tio.JobDescriptionID)
        Try
            dba.ExecuteNonQuery()
        Catch ex As Exception
            errStr = ex.Message
        End Try
        ardal.doTimeInOutRepLog(tio)
        Return errStr
    End Function

    Public Function deleteTIO(ByVal tioID As Guid) As String
        Dim errStr As String = String.Empty
        Dim ardal As New AuditRepLogDAL()
        Dim tio As New TimeInOut
        Dim dba As New DBAccess()
        dba.CommandText = "SELECT TimePuncheID, TimeIn, TimeOut FROM TimeInOut WHERE ID = @ID"
        dba.AddParameter("@ID", tioID)
        Dim dt As DataTable = dba.ExecuteDataSet.Tables(0)
        If dt.Rows.Count > 0 Then
            For Each rw As DataRow In dt.Rows
                tio.TimepuncheID = rw.Item(0)
                tio.TimeIn = rw.Item(1)
                tio.TimeOut = rw.Item(2)
                tio.ID = tioID
                ardal.doTimeInOutRepLog(tio, 1)
                ardal.UpdateAudit("TimeInOut", tio.TimeIn & " - " & tio.TimeOut, "DELETE RECORD", "n/a", tioID)
            Next
        End If
        dba.CommandText = "DELETE FROM TimeInOut WHERE ID = @ID"
        dba.AddParameter("@ID", tioID)
        Try
            dba.ExecuteNonQuery()
        Catch ex As Exception
            errStr = ex.Message
        End Try
        Return errStr
    End Function
#End Region

    Public Function TimeOn(ByVal eid As String) As String
        Dim retstr As String = String.Empty
        Dim dba As New DBAccess
        dba.CommandText = "SELECT TimeInOut.TimeIn" & _
        " FROM Employee INNER JOIN" & _
        " TimePunche ON Employee.ID = TimePunche.EmployeeID INNER JOIN" & _
        " TimeInOut ON TimePunche.ID = TimeInOut.TimepuncheID" & _
        " WHERE (TimeInOut.TimeOut= '1/1/1900 12:00:00 AM' OR TimeInOut.TimeOut IS NULL) AND (Employee.ID = @eid)"
        dba.AddParameter("@eid", eid)
        Try
            retstr = dba.ExecuteScalar
        Catch ex As Exception
            retstr = ex.Message
        End Try
        Dim mat As String = String.Empty
        Return retstr
    End Function

End Class
