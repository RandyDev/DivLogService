'Imports DiversifiedLogistics.WorkOrder
Imports System.Data
Imports System.Data.SqlClient
Imports System
'Imports System.Net
'Imports System.Net.Mail
'Imports System.Net.Mime
'Imports System.Threading
'Imports System.ComponentModel
<DataContract([Namespace]:="")>
Public Class WorkOrderDAL
    <Flags()> _
    Enum LoadStatus     'track load status via bitwise operations
        is_done = finished
        todo = Undefined
        Undefined = 0
        CheckedIn = 1
        Assigned = 2
        Printed = 4
        AddDataChanged = 8
        Complete = 64
        finished = 128
    End Enum
    'Public Function GetActiveWorkOrders(ByVal locaID As Guid, ByVal HideCompleted As Boolean) As DataTable
    '    Dim todaysDate As DateTime = Date.Now.ToShortDateString
    '    Dim endDate As DateTime = DateAdd(DateInterval.Day, 1, todaysDate)
    '    Dim ldal As New locaDAL
    '    Dim bdOffSet As Integer = ldal.getLocaBDOffset(locaID)
    '    '        Dim getDate As DateTime = DateAdd(DateInterval.Day, -1, todaysDate)
    '    Dim sql As String = "SELECT W.ID, W.LogDate, W.DoorNumber AS 'DoorNum', V.Name AS Vendor, W.PurchaseOrder, C.Name AS Carrier, W.TruckNumber, W.TrailerNumber,  " & _
    '        "W.AppointmentTime, W.StartTime, W.CompTime, W.DockTime, D.Name AS Department, LT.Name AS LoadType " & _
    '        "FROM ParentCompany AS PC RIGHT OUTER JOIN " & _
    '        "Location AS L ON PC.ID = L.ParentCompanyID RIGHT OUTER JOIN " & _
    '        "WorkOrder AS W LEFT OUTER JOIN " & _
    '        "LoadType AS LT ON W.LoadTypeID = LT.ID LEFT OUTER JOIN " & _
    '        "Carrier AS C ON W.CarrierID = C.ID LEFT OUTER JOIN " & _
    '        "Vendor AS V ON W.CustomerID = V.ID ON L.ID = W.LocationID LEFT OUTER JOIN " & _
    '        "Department AS D ON W.DepartmentID = D.ID " & _
    '        "WHERE     (W.LocationID = @locaID) "
    '    If HideCompleted Then
    '        sql &= "AND DATEPART(year, w.Comptime) < DATEPART(year, GETDATE()) "
    '    End If
    '    If bdOffSet <> 0 Then
    '        todaysDate = DateAdd(DateInterval.Hour, bdOffSet, todaysDate)
    '        sql &= "AND ((W.DockTime > @dt) and (W.DockTime < @edt)) "
    '    Else
    '        sql &= "AND (CAST(CONVERT(char(10), W.LogDate, 101) AS DATETIME) = @dt) "
    '    End If
    '    sql &= "ORDER BY W.CompTime, W.DockTime DESC, W.AppointmentTime "
    '    'sql &= "ORDER BY W.DockTime DESC,D.Name, W.DoorNumber, W.AppointmentTime "

    '    Dim adapter As New SqlDataAdapter(Sql, ConfigurationManager.ConnectionStrings("rtdsConnectionString").ConnectionString)
    '    Dim param As New SqlParameter("locaID", locaID)
    '    Dim param2 As New SqlParameter("dt", todaysDate)
    '    adapter.SelectCommand.Parameters.Add(param)
    '    adapter.SelectCommand.Parameters.Add(param2)
    '    If bdOffSet <> 0 Then
    '        Dim param3 As New SqlParameter("edt", DateAdd(DateInterval.Hour, bdOffSet, EndDate))
    '        adapter.SelectCommand.Parameters.Add(param3)
    '    End If

    '    Dim dt As DataTable = New DataTable()
    '    adapter.Fill(dt)
    '    '        go get thu records
    '    Dim clm As DataColumn = New DataColumn()
    '    clm.ColumnName = "Unloaders"
    '    dt.Columns.Add(clm)
    '    If dt.Rows.Count > 1 Then
    '        For Each rw As DataRow In dt.Rows
    '            adapter.SelectCommand.CommandText = "SELECT Employee.FirstName, Employee.LastName " & _
    '                "FROM Employee INNER JOIN " & _
    '                "Unloader ON Employee.ID = Unloader.EmployeeID INNER JOIN " & _
    '                "WorkOrder ON Unloader.LoadID = WorkOrder.ID " & _
    '                "WHERE(WorkOrder.ID = @woid) "
    '            adapter.SelectCommand.Parameters.Clear()
    '            Dim xparam As SqlParameter = New SqlParameter("woid", rw.Item("ID"))
    '            adapter.SelectCommand.Parameters.Add(xparam)
    '            Dim dtunloaders As DataTable = New DataTable()
    '            adapter.Fill(dtunloaders)
    '            Dim uls As String = String.Empty
    '            For Each urw As DataRow In dtunloaders.Rows
    '                uls &= urw.Item("FirstName") & " " & urw.Item("LastName") & ", "
    '            Next
    '            If uls.Length > 3 Then uls = Left(uls, Len(uls) - 2)
    '            rw.Item("Unloaders") = uls
    '        Next
    '    End If
    '    'put records in list
    '    'iterate thru list and add employees

    '    Return dt
    'End Function
    'Public Function GetActiveWorkOrders2(ByVal locaID As Guid, ByVal HideCompleted As Boolean) As DataTable
    '    Dim ldal As New locaDAL
    '    Dim loca As Location = ldal.getLocationByID(locaID)
    '    Dim locationName As String = loca.Name
    '    Dim bdOffSet As Integer = ldal.getLocaBDOffset(locaID)
    '    Dim StartDate As DateTime = Date.Now.ToShortDateString
    '    Dim EndDate As DateTime = DateAdd(DateInterval.Day, 1, StartDate)
    '    Dim strSQL As String = "SELECT dbo.WorkOrder.DoorNumber, dbo.Vendor.Name AS Vendor, dbo.WorkOrder.PurchaseOrder AS PO, dbo.Carrier.Name AS Carrier,  " & _
    '        "dbo.WorkOrder.TrailerNumber AS Trailer, CASE WHEN CONVERT(VARCHAR(19), dbo.WorkOrder.AppointmentTime, 121)  " & _
    '        "= '1900-01-01 00:00:00' THEN '- - -' ELSE CONVERT(VARCHAR(5), dbo.WorkOrder.AppointmentTime, 8) END AS Appt,  " & _
    '        "CASE WHEN CONVERT(VARCHAR(19), dbo.WorkOrder.DockTime, 121) = '1900-01-01 00:00:00' THEN '- - -' ELSE CONVERT(VARCHAR(5),  " & _
    '        "dbo.WorkOrder.DockTime, 8) END AS DockTime, CASE WHEN CONVERT(VARCHAR(19), dbo.WorkOrder.StartTime, 121)  " & _
    '        "= '1900-01-01 00:00:00' THEN '- - -' ELSE CONVERT(VARCHAR(5), dbo.WorkOrder.StartTime, 8) END AS StartTime, CASE WHEN CONVERT(VARCHAR(19),  " & _
    '        "dbo.WorkOrder.CompTime, 121) = '1900-01-01 00:00:00' THEN '- - -' ELSE CONVERT(VARCHAR(5), dbo.WorkOrder.CompTime, 8) END AS Finish,  " & _
    '        "dbo.Department.Name AS Department, dbo.WorkOrder.ID, CASE WHEN dbo.ufn_UnloaderList(dbo.WorkOrder.ID) IS NULL  " & _
    '        "THEN '- - -' ELSE dbo.ufn_UnloaderList(dbo.WorkOrder.ID) END AS Unloaders, dbo.LoadType.Name " & _
    '        "FROM dbo.WorkOrder INNER JOIN " & _
    '        "dbo.LoadType ON dbo.WorkOrder.LoadTypeID = dbo.LoadType.ID LEFT OUTER JOIN " & _
    '        "dbo.Carrier ON dbo.WorkOrder.CarrierID = dbo.Carrier.ID LEFT OUTER JOIN " & _
    '        "dbo.Vendor ON dbo.WorkOrder.CustomerID = dbo.Vendor.ID LEFT OUTER JOIN " & _
    '        "dbo.Department ON dbo.WorkOrder.DepartmentID = dbo.Department.ID LEFT OUTER JOIN " & _
    '        "dbo.Location ON dbo.WorkOrder.LocationID = dbo.Location.ID " & _
    '        "WHERE (dbo.Location.Name = @location) "
    '    If HideCompleted Then
    '        strSQL &= "AND DATEPART(year, dbo.WorkOrder.Comptime) < DATEPART(year, GETDATE()) "
    '    End If
    '    If bdOffSet <> 0 Then
    '        StartDate = DateAdd(DateInterval.Hour, bdOffSet, StartDate)
    '        EndDate = DateAdd(DateInterval.Hour, bdOffSet, EndDate)
    '        strSQL &= "CASE WHEN W.AppointmentTime > '1900-01-01 00:00:00.000' "
    '        strSQL &= "THEN ((W.AppointmentTime >= @StartDate) and (W.AppointmentTime < @EndDate)) "
    '        strSQL &= "ELSE ((W.DockTime >= @StartDate) and (W.DockTime < @EndDate)) END "
    '    Else
    '        strSQL &= "AND (dbo.WorkOrder.LogDate >= @StartDate) AND (dbo.WorkOrder.LogDate < @EndDate) "
    '    End If
    '    strSQL &= "ORDER BY CASE WHEN CONVERT(VARCHAR(19), dbo.WorkOrder.CompTime, 121) = '1900-01-01 00:00:00' THEN '1' ELSE '0' END DESC, " & _
    '        "CASE WHEN dbo.ufn_UnloaderList(dbo.WorkOrder.ID) IS NULL THEN '1' ELSE '0' END, Department, dbo.WorkOrder.DoorNumber, DockTime DESC, Appt "
    '    Dim adapter As SqlDataAdapter = New SqlDataAdapter(strSQL, ConfigurationManager.ConnectionStrings("rtdsConnectionString").ConnectionString)
    '    Dim param As New SqlParameter("location", locationName)
    '    Dim param2 As New SqlParameter("StartDate", StartDate)
    '    Dim param3 As New SqlParameter("EndDate", DateAdd(DateInterval.Hour, bdOffSet, EndDate))
    '    adapter.SelectCommand.Parameters.Add(param)
    '    adapter.SelectCommand.Parameters.Add(param2)
    '    adapter.SelectCommand.Parameters.Add(param3)
    '    adapter.SelectCommand.CommandTimeout = 120
    '    Dim dt As DataTable = New DataTable()
    '    adapter.Fill(dt)
    '    Return dt
    'End Function
    'Public Function getLocationIDbyWorkOrderID(ByVal woid As String) As String
    '    Dim retstr As String = String.Empty
    '    Dim dba As New DBAccess
    '    dba.CommandText = "Select LocationID FROM WorkOrder WHERE ID = @woid"
    '    dba.AddParameter("@woid", woid)
    '    retstr = dba.ExecuteScalar.ToString
    '    Return retstr
    'End Function

    'Public Function isLoadLocked(ByVal loadID As String) As Boolean
    '    Dim retbool As Boolean = False
    '    Dim dba As New DBAccess()
    '    dba.CommandText = "SELECT COUNT (workOrderID) FROM VerifiedWorkOrders WHERE workOrderID = @workOrderID"
    '    dba.AddParameter("@workOrderID", loadID)
    '    retbool = dba.ExecuteScalar > 0
    '    Return retbool
    'End Function

    Public Function GetLoadByID(ByVal loadID As String) As WorkOrder
        Dim wo As New WorkOrder
        Dim dba As New DBAccess()
        dba.CommandText = "SELECT WO.Status, WO.LogDate, WO.LogNumber, WO.LoadNumber, WO.LocationID, WO.DepartmentID, Dept.Name AS Department, WO.LoadTypeID,  " &
            "LT.Name AS LoadType, WO.CustomerID, WO.VendorNumber, V.Name AS VendorName, WO.ReceiptNumber, WO.PurchaseOrder, WO.Amount,  " &
            "WO.IsCash, WO.LoadDescriptionID,WO.SplitPaymentAmount, Des.Name AS LoadDescription, WO.CarrierID, Carrier.Name AS CarrierName, WO.TruckNumber,  " &
            "WO.TrailerNumber, WO.AppointmentTime, WO.GateTime, WO.DockTime, WO.StartTime, WO.CompTime, WO.TTLTime, WO.PalletsUnloaded,  " &
            "WO.DoorNumber, WO.Pieces, WO.Weight, WO.Comments, WO.Restacks, WO.PalletsReceived, WO.BadPallets, WO.NumberOfItems,  " &
            "WO.CheckNumber, WO.BOL, WO.ID, VWO.userID, VWO.timeStamp, WO.CreatedBy " &
            "FROM WorkOrder AS WO LEFT OUTER JOIN " &
            "Vendor AS V ON WO.CustomerID = V.ID LEFT OUTER JOIN " &
            "Department AS Dept ON WO.DepartmentID = Dept.ID LEFT OUTER JOIN " &
            "Carrier ON WO.CarrierID = Carrier.ID LEFT OUTER JOIN " &
            "Description AS Des ON WO.LoadDescriptionID = Des.ID LEFT OUTER JOIN " &
            "LoadType AS LT ON WO.LoadTypeID = LT.ID LEFT OUTER JOIN " &
            "VerifiedWorkOrders AS VWO ON WO.ID = VWO.workOrderID " &
            "WHERE (WO.ID = @loadID) "
        dba.AddParameter("@loadID", loadID)
        Dim ds As DataSet = dba.ExecuteDataSet
        If ds.Tables(0).Rows.Count > 0 Then
            For Each rw As DataRow In ds.Tables(0).Rows
                wo.Status = IIf(IsDBNull(rw.Item("Status")), "", rw.Item("Status"))
                wo.LogDate = IIf(IsDBNull(rw.Item("LogDate")), "", rw.Item("LogDate"))
                wo.LogNumber = IIf(IsDBNull(rw.Item("LogNumber")), "", rw.Item("LogNumber"))
                wo.LoadNumber = IIf(IsDBNull(rw.Item("LoadNumber")), "", rw.Item("LoadNumber"))
                wo.LocationID = IIf(IsDBNull(rw.Item("LocationID")), "", rw.Item("LocationID"))
                wo.DepartmentID = IIf(IsDBNull(rw.Item("DepartmentID")), "", rw.Item("DepartmentID"))
                wo.Department = IIf(IsDBNull(rw.Item("Department")), "", rw.Item("Department"))
                wo.LoadTypeID = IIf(IsDBNull(rw.Item("LoadTypeID")), "", rw.Item("LoadTypeID"))
                wo.LoadType = IIf(IsDBNull(rw.Item("LoadType")), "", rw.Item("LoadType"))
                wo.CustomerID = IIf(IsDBNull(rw.Item("CustomerID")), "", rw.Item("CustomerID"))
                wo.VendorNumber = IIf(IsDBNull(rw.Item("VendorNumber")), "", rw.Item("VendorNumber"))
                wo.VendorName = IIf(IsDBNull(rw.Item("VendorName")), "", rw.Item("VendorName"))
                wo.ReceiptNumber = IIf(IsDBNull(rw.Item("ReceiptNumber")), "", rw.Item("ReceiptNumber"))
                wo.PurchaseOrder = IIf(IsDBNull(rw.Item("PurchaseOrder")), "", rw.Item("PurchaseOrder"))
                wo.Amount = IIf(IsDBNull(rw.Item("Amount")), "", rw.Item("Amount"))
                wo.SplitPaymentAmount = IIf(IsDBNull(rw.Item("SplitPaymentAmount")), 0, rw.Item("SplitPaymentAmount"))
                wo.IsCash = IIf(IsDBNull(rw.Item("IsCash")), "", rw.Item("IsCash"))
                wo.LoadDescriptionID = IIf(IsDBNull(rw.Item("LoadDescriptionID")), "", rw.Item("LoadDescriptionID"))
                wo.LoadDescription = IIf(IsDBNull(rw.Item("LoadDescription")), "", rw.Item("LoadDescription"))
                wo.CarrierID = IIf(IsDBNull(rw.Item("CarrierID")), "", rw.Item("CarrierID"))
                wo.CarrierName = IIf(IsDBNull(rw.Item("CarrierName")), "", rw.Item("CarrierName"))
                wo.TruckNumber = IIf(IsDBNull(rw.Item("TruckNumber")), "", rw.Item("TruckNumber"))
                wo.TrailerNumber = IIf(IsDBNull(rw.Item("TrailerNumber")), "", rw.Item("TrailerNumber"))
                wo.AppointmentTime = IIf(IsDBNull(rw.Item("AppointmentTime")), "", rw.Item("AppointmentTime"))
                wo.GateTime = IIf(IsDBNull(rw.Item("GateTime")), "", rw.Item("GateTime"))
                wo.DockTime = IIf(IsDBNull(rw.Item("DockTime")), "", rw.Item("DockTime"))
                wo.StartTime = IIf(IsDBNull(rw.Item("StartTime")), "1/1/1980", rw.Item("StartTime"))
                wo.CompTime = IIf(IsDBNull(rw.Item("CompTime")), "", rw.Item("CompTime"))
                wo.TTLTime = IIf(IsDBNull(rw.Item("TTLTime")), "0", rw.Item("TTLTime"))
                wo.PalletsUnloaded = IIf(IsDBNull(rw.Item("PalletsUnloaded")), "", rw.Item("PalletsUnloaded"))
                wo.DoorNumber = IIf(IsDBNull(rw.Item("DoorNumber")), "", rw.Item("DoorNumber"))
                wo.Pieces = IIf(IsDBNull(rw.Item("Pieces")), "", rw.Item("Pieces"))
                wo.Weight = IIf(IsDBNull(rw.Item("Weight")), "", rw.Item("Weight"))
                wo.Comments = IIf(IsDBNull(rw.Item("Comments")), "", rw.Item("Comments"))
                wo.Restacks = IIf(IsDBNull(rw.Item("Restacks")), "", rw.Item("Restacks"))
                wo.PalletsReceived = IIf(IsDBNull(rw.Item("PalletsReceived")), "", rw.Item("PalletsReceived"))
                wo.BadPallets = IIf(IsDBNull(rw.Item("BadPallets")), "", rw.Item("BadPallets"))
                wo.NumberOfItems = IIf(IsDBNull(rw.Item("NumberOfItems")), "", rw.Item("NumberOfItems"))
                wo.CheckNumber = IIf(IsDBNull(rw.Item("CheckNumber")), "", rw.Item("CheckNumber"))
                wo.BOL = IIf(IsDBNull(rw.Item("BOL")), "", rw.Item("BOL").ToString)
                wo.ID = IIf(IsDBNull(rw.Item("ID")), "", rw.Item("ID"))
                wo.isClosed = Not IsDBNull(rw.Item("userID"))
                wo.CreatedBy = IIf(IsDBNull(rw.Item("CreatedBy")), "", rw.Item("CreatedBy"))
            Next
        End If
        'Dim edal As New empDAL
        'Dim elst As List(Of String) = New List(Of String)

        'Dim dt As DataTable = edal.GetUnloadersByWOID(wo.ID.ToString)
        'If dt.Rows.Count > 0 Then
        '    For Each row As DataRow In dt.Rows
        '        elst.Add(row.Item("ID").ToString)
        '    Next
        '    wo.Employee = elst
        'End If
        Return wo
    End Function

    Public Function GetLoadsToBeVerified(ByVal locaID As String, ByVal sDate As DateTime, ByVal eDate As DateTime, Optional ByVal tfilter As String = "") As DataTable 'As List(Of Load)
        'Dim loadList As New List(Of Load)
        Dim dt As New DataTable()
        Dim dba As New DBAccess()
        eDate = DateAdd(DateInterval.Second, 86399, eDate)
        If locaID.Length < 6 Then
            locaID = Guid.NewGuid().ToString
        End If
        Dim cmd As String = "SELECT WO.Status, WO.LogDate, WO.LogNumber, WO.LoadNumber, WO.LocationID, WO.DepartmentID, WO.LoadTypeID, WO.CustomerID,  " & _
            "WO.VendorNumber, V.Name AS VendorName, WO.ReceiptNumber, WO.PurchaseOrder, WO.Amount,WO.SplitPaymentAmount, WO.IsCash, WO.LoadDescriptionID, WO.CarrierID,  " & _
            "Car.Name AS CarrierName, WO.TruckNumber, WO.TrailerNumber, WO.AppointmentTime, WO.GateTime, WO.DockTime, WO.StartTime, WO.CompTime,  " & _
            "WO.TTLTime, WO.PalletsUnloaded, WO.DoorNumber, WO.Pieces, WO.Weight, WO.Comments, WO.Restacks, WO.PalletsReceived, WO.BadPallets,  " & _
            "WO.NumberOfItems, WO.CheckNumber, WO.BOL, WO.ID, VWO.userID, VWO.timeStamp, Dept.Name AS DepartmentName,  " & _
            "Des.Name AS LoadDescription, LoadType.Name AS LoadTypeName, " & _
            "(SELECT COUNT(WorkOrderID) AS PicCount " & _
            "FROM dbo.LoadImages " & _
            "GROUP BY WorkOrderID " & _
            "HAVING (WorkOrderID = WO.ID)) AS PicCount " & _
            "FROM WorkOrder AS WO LEFT OUTER JOIN " & _
            "LoadType ON WO.LoadTypeID = LoadType.ID LEFT OUTER JOIN " & _
            "Vendor AS V ON WO.CustomerID = V.ID LEFT OUTER JOIN " & _
            "Carrier AS Car ON WO.CarrierID = Car.ID LEFT OUTER JOIN " & _
            "Department AS Dept ON WO.DepartmentID = Dept.ID LEFT OUTER JOIN " & _
            "Description AS Des ON WO.LoadDescriptionID = Des.ID LEFT OUTER JOIN " & _
            "VerifiedWorkOrders AS VWO ON WO.ID = VWO.workOrderID " & _
            "WHERE (WO.LocationID = @locaID) AND (WO.LogDate >= @sDate) AND (WO.LogDate <= @eDate)"
        If tfilter.Length > 2 Then
            Dim strFilter As String = String.Format("AND ((Dept.Name LIKE '%{0}%') OR (WO.PurchaseOrder LIKE '%{0}%') OR (WO.VendorNumber LIKE '%{0}%') OR (LoadType.Name LIKE '%{0}%') OR (WO.Comments LIKE '%{0}%')) ", tfilter)
            cmd &= strFilter
        End If
        cmd &= "ORDER BY VWO.workOrderID, WO.LogDate "
        dba.CommandText = cmd
        dba.AddParameter("@locaID", locaID)
        dba.AddParameter("@sDate", sDate)
        dba.AddParameter("@eDate", eDate)

        Dim ds As DataSet = dba.ExecuteDataSet
        dt = ds.Tables(0)

        Return dt   ' loadList
    End Function
    Public Function GetEditLoads(ByVal locaID As String, ByVal sDate As DateTime, ByVal eDate As DateTime, Optional ByVal tfilter As String = "") As DataTable 'As List(Of Load)
        'Dim loadList As New List(Of Load)
        Dim dt As New DataTable()
        Dim dba As New DBAccess()
        eDate = DateAdd(DateInterval.Second, 86399, eDate) 'advance enddate by 23hrs 59mins 59 secs
        If locaID.Length < 6 Then
            locaID = Guid.NewGuid().ToString
        End If
        Dim cmd As String = "SELECT WO.Status, WO.LogDate, WO.LogNumber, WO.LoadNumber, WO.LocationID, WO.DepartmentID, WO.LoadTypeID, WO.CustomerID,  " & _
            "WO.VendorNumber, V.Name AS VendorName, WO.ReceiptNumber, WO.PurchaseOrder, WO.Amount,WO.SplitPaymentAmount, WO.IsCash, WO.LoadDescriptionID, WO.CarrierID,  " & _
            "Car.Name AS CarrierName, WO.TruckNumber, WO.TrailerNumber, WO.AppointmentTime, WO.GateTime, WO.DockTime, WO.StartTime, WO.CompTime,  " & _
            "WO.TTLTime, WO.PalletsUnloaded, WO.DoorNumber, WO.Pieces, WO.Weight, WO.Comments, WO.Restacks, WO.PalletsReceived, WO.BadPallets,  " & _
            "WO.NumberOfItems, WO.CheckNumber, WO.BOL, WO.ID, VWO.userID, VWO.timeStamp, Dept.Name AS DepartmentName,  " & _
            "Des.Name AS LoadDescription, LoadType.Name AS LoadTypeName, " & _
            "(SELECT COUNT(WorkOrderID) AS PicCount " & _
            "FROM dbo.LoadImages " & _
            "GROUP BY WorkOrderID " & _
            "HAVING (WorkOrderID = WO.ID)) AS PicCount " & _
            "FROM WorkOrder AS WO LEFT OUTER JOIN " & _
            "LoadType ON WO.LoadTypeID = LoadType.ID LEFT OUTER JOIN " & _
            "Vendor AS V ON WO.CustomerID = V.ID LEFT OUTER JOIN " & _
            "Carrier AS Car ON WO.CarrierID = Car.ID LEFT OUTER JOIN " & _
            "Department AS Dept ON WO.DepartmentID = Dept.ID LEFT OUTER JOIN " & _
            "Description AS Des ON WO.LoadDescriptionID = Des.ID LEFT OUTER JOIN " & _
            "VerifiedWorkOrders AS VWO ON WO.ID = VWO.workOrderID " & _
            "WHERE (WO.LocationID = @locaID) AND (WO.LogDate >= @sDate) AND (WO.LogDate <= @eDate) AND (WO.Status < 79)"
        '*************************************************
        '******************TO DO**************************
        '*************************************************
        'include offset for time
        '*************************************************
        '*************************************************
        '*************************************************
        cmd &= "ORDER BY WO.Status "
        dba.CommandText = cmd
        dba.AddParameter("@locaID", locaID)
        dba.AddParameter("@sDate", sDate)
        dba.AddParameter("@eDate", eDate)

        Dim ds As DataSet = dba.ExecuteDataSet
        dt = ds.Tables(0)

        Return dt   ' loadList
    End Function

    'Public Function UpdateWorkOrder(ByVal wo As WorkOrder, Optional ByVal audit As Boolean = True, Optional repl As Boolean = True) As String
    '    Dim svd As String = String.Empty
    '    Dim arDAL As New AuditRepLogDAL()
    '    Dim wodal As New WorkOrderDAL()
    '    Dim dba As New DBAccess()
    '    Dim tblName As String = "WorkOrder"
    '    Dim oldWO As WorkOrder = wodal.GetLoadByID(wo.ID.ToString)
    '    If oldWO.Amount = Nothing Then oldWO.Amount = 0
    '    If audit Then
    '        If oldWO.Amount <> wo.Amount Then arDAL.UpdateAudit(tblName, oldWO.Amount.ToString, wo.Amount.ToString, "Amount", wo.ID)
    '        If oldWO.SplitPaymentAmount <> wo.SplitPaymentAmount Then arDAL.UpdateAudit(tblName, oldWO.SplitPaymentAmount.ToString, wo.SplitPaymentAmount.ToString, "SplitPaymentAmount", wo.ID)
    '        If oldWO.IsCash <> wo.IsCash Then arDAL.UpdateAudit(tblName, oldWO.IsCash.ToString, wo.IsCash.ToString, "IsCash", wo.ID)
    '        If oldWO.PurchaseOrder <> wo.PurchaseOrder Then arDAL.UpdateAudit(tblName, oldWO.PurchaseOrder, wo.PurchaseOrder, "PurchaseOrder", wo.ID)
    '        If oldWO.Pieces <> wo.Pieces Then arDAL.UpdateAudit(tblName, oldWO.Pieces.ToString, wo.Pieces.ToString, "Pieces", wo.ID)
    '        If oldWO.PalletsReceived <> wo.PalletsReceived Then arDAL.UpdateAudit(tblName, oldWO.PalletsReceived.ToString, wo.PalletsReceived.ToString, "PalletsReceived", wo.ID)
    '        If oldWO.PalletsUnloaded <> wo.PalletsUnloaded Then arDAL.UpdateAudit(tblName, oldWO.PalletsUnloaded.ToString, wo.PalletsUnloaded.ToString, "PalletsUnloaded", wo.ID)
    '        If oldWO.Weight <> wo.Weight Then arDAL.UpdateAudit(tblName, oldWO.Weight.ToString, wo.Weight.ToString, "Weight", wo.ID)
    '        If Left(wo.CreatedBy, 9) <> "Imported:" Then
    '            If oldWO.Restacks <> wo.Restacks Then arDAL.UpdateAudit(tblName, oldWO.Restacks.ToString, wo.Restacks.ToString, "Restacks", wo.ID)
    '            If oldWO.BadPallets <> wo.BadPallets Then arDAL.UpdateAudit(tblName, oldWO.BadPallets.ToString, wo.BadPallets.ToString, "BadPallets", wo.ID)
    '        End If
    '        If oldWO.LogDate.ToShortDateString <> wo.LogDate.ToShortDateString Then arDAL.UpdateAudit(tblName, oldWO.LogDate.ToShortDateString, wo.LogDate.ToShortDateString, "LogDate", wo.ID)
    '        If oldWO.DepartmentID <> wo.DepartmentID Then arDAL.UpdateAudit(tblName, oldWO.DepartmentID.ToString, wo.DepartmentID.ToString, "DepartmentID", wo.ID)
    '        If oldWO.Department <> wo.Department Then arDAL.UpdateAudit(tblName, oldWO.Department.ToString, wo.Department.ToString, "Department", wo.ID)
    '        If oldWO.LoadTypeID <> wo.LoadTypeID Then arDAL.UpdateAudit(tblName, oldWO.LoadTypeID.ToString, wo.LoadTypeID.ToString, "`LoadTypeID", wo.ID)
    '        If oldWO.LoadType <> wo.LoadType Then arDAL.UpdateAudit(tblName, oldWO.LoadType.ToString, wo.LoadType.ToString, "LoadType", wo.ID)
    '        If oldWO.CustomerID <> wo.CustomerID Then arDAL.UpdateAudit(tblName, oldWO.CustomerID.ToString, wo.CustomerID.ToString, "CustomerID", wo.ID)
    '        If oldWO.VendorNumber <> wo.VendorNumber Then arDAL.UpdateAudit(tblName, oldWO.VendorNumber.ToString, wo.VendorNumber.ToString, "VendorNumber", wo.ID)
    '        If oldWO.VendorName <> wo.VendorName Then arDAL.UpdateAudit(tblName, oldWO.VendorName.ToString, wo.VendorName.ToString, "VendorName", wo.ID)
    '        If oldWO.PurchaseOrder <> wo.PurchaseOrder Then arDAL.UpdateAudit(tblName, oldWO.PurchaseOrder.ToString, wo.PurchaseOrder.ToString, "PurchaseOrder", wo.ID)
    '        If oldWO.Amount <> wo.Amount Then arDAL.UpdateAudit(tblName, oldWO.Amount.ToString, wo.Amount.ToString, "Amount", wo.ID)
    '        If oldWO.IsCash <> wo.IsCash Then arDAL.UpdateAudit(tblName, oldWO.IsCash.ToString, wo.IsCash.ToString, "IsCash", wo.ID)
    '        If oldWO.LoadDescriptionID <> wo.LoadDescriptionID Then arDAL.UpdateAudit(tblName, oldWO.LoadDescriptionID.ToString, wo.LoadDescriptionID.ToString, "LoadDescriptionID", wo.ID)
    '        If oldWO.LoadDescription <> wo.LoadDescription Then arDAL.UpdateAudit(tblName, oldWO.LoadDescription.ToString, wo.LoadDescription.ToString, "LoadDescription", wo.ID)
    '        If oldWO.CarrierID <> wo.CarrierID Then arDAL.UpdateAudit(tblName, oldWO.CarrierID.ToString, wo.CarrierID.ToString, "CarrierID", wo.ID)
    '        If oldWO.CarrierName <> wo.CarrierName Then arDAL.UpdateAudit(tblName, oldWO.CarrierName.ToString, wo.CarrierName.ToString, "CarrierName", wo.ID)
    '        If oldWO.TruckNumber <> wo.TruckNumber Then arDAL.UpdateAudit(tblName, oldWO.TruckNumber.ToString, wo.TruckNumber.ToString, "TruckNumber", wo.ID)
    '        If oldWO.TrailerNumber <> wo.TrailerNumber Then arDAL.UpdateAudit(tblName, oldWO.TrailerNumber.ToString, wo.TrailerNumber.ToString, "TrailerNumber", wo.ID)
    '        If oldWO.AppointmentTime <> wo.AppointmentTime Then arDAL.UpdateAudit(tblName, oldWO.AppointmentTime.ToString, wo.AppointmentTime.ToString, "AppointmentTime", wo.ID)
    '        If oldWO.GateTime <> wo.GateTime Then arDAL.UpdateAudit(tblName, oldWO.GateTime.ToString, wo.GateTime.ToString, "GateTime", wo.ID)
    '        If oldWO.DockTime <> wo.DockTime Then arDAL.UpdateAudit(tblName, oldWO.DockTime.ToString, wo.DockTime.ToString, "DockTime", wo.ID)
    '        If oldWO.StartTime <> wo.StartTime Then arDAL.UpdateAudit(tblName, oldWO.StartTime.ToString, wo.StartTime.ToString, "StartTime", wo.ID)
    '        If oldWO.CompTime <> wo.CompTime Then arDAL.UpdateAudit(tblName, oldWO.CompTime.ToString, wo.CompTime.ToString, "CompTime", wo.ID)
    '        If oldWO.TTLTime <> wo.TTLTime Then arDAL.UpdateAudit(tblName, oldWO.TTLTime.ToString, wo.TTLTime.ToString, "TTLTime", wo.ID)
    '        If oldWO.DoorNumber <> wo.DoorNumber Then arDAL.UpdateAudit(tblName, oldWO.DoorNumber.ToString, wo.DoorNumber.ToString, "DoorNumber", wo.ID)
    '        If oldWO.Pieces <> wo.Pieces Then arDAL.UpdateAudit(tblName, oldWO.Pieces.ToString, wo.Pieces.ToString, "Pieces", wo.ID)
    '        If oldWO.Weight <> wo.Weight Then arDAL.UpdateAudit(tblName, oldWO.Weight.ToString, wo.Weight.ToString, "Weight", wo.ID)
    '        If oldWO.Comments.Trim() <> wo.Comments.Trim() Then arDAL.UpdateAudit(tblName, oldWO.Comments.ToString, wo.Comments.ToString, "Comments", wo.ID)
    '        If oldWO.Restacks <> wo.Restacks Then arDAL.UpdateAudit(tblName, oldWO.Restacks.ToString, wo.Restacks.ToString, "Restacks", wo.ID)
    '        If oldWO.PalletsReceived <> wo.PalletsReceived Then arDAL.UpdateAudit(tblName, oldWO.PalletsReceived.ToString, wo.PalletsReceived.ToString, "PalletsReceived", wo.ID)
    '        If oldWO.NumberOfItems <> wo.NumberOfItems Then arDAL.UpdateAudit(tblName, oldWO.ToString, wo.NumberOfItems.ToString, "NumberOfItems", wo.ID)
    '        If oldWO.CheckNumber <> wo.CheckNumber Then arDAL.UpdateAudit(tblName, oldWO.CheckNumber.ToString, wo.CheckNumber.ToString, "CheckNumber", wo.ID)
    '        If oldWO.BOL <> wo.BOL Then arDAL.UpdateAudit(tblName, oldWO.BOL.ToString, wo.BOL.ToString, "BOL", wo.ID)

    '        'this is a guid, show text version of changes 
    '        If oldWO.LoadTypeID <> wo.LoadTypeID Then
    '            Dim oloadTypeName As String = String.Empty
    '            Dim nloadTypeName As String = String.Empty

    '            dba.CommandText = "Select Name FROM LoadType WHERE ID = @id"
    '            dba.AddParameter("@id", oldWO.LoadTypeID)
    '            oloadTypeName = dba.ExecuteScalar
    '            If oloadTypeName Is Nothing Then oloadTypeName = "not selected"

    '            dba.CommandText = "Select Name FROM LoadType WHERE ID = @id"
    '            dba.AddParameter("@id", wo.LoadTypeID)
    '            nloadTypeName = dba.ExecuteScalar
    '            arDAL.UpdateAudit(tblName, oloadTypeName, nloadTypeName, "LoadType", wo.ID)
    '        End If
    '        If oldWO.LogDate.ToShortDateString <> wo.LogDate.ToShortDateString Then arDAL.UpdateAudit(tblName, oldWO.LogDate, wo.LogDate, "LogDate", wo.ID)
    '        If oldWO.StartTime > DateAdd(DateInterval.Minute, 1, wo.StartTime) Or oldWO.StartTime < DateAdd(DateInterval.Minute, -1, wo.StartTime) Then arDAL.UpdateAudit(tblName, oldWO.StartTime, wo.StartTime, "StartTime", wo.ID)
    '        If oldWO.CompTime > DateAdd(DateInterval.Minute, 1, wo.CompTime) Or oldWO.CompTime < DateAdd(DateInterval.Minute, -1, wo.CompTime) Then arDAL.UpdateAudit(tblName, oldWO.CompTime.ToString, wo.CompTime.ToString, "CompTime", wo.ID)
    '        If Not wo.Employee Is Nothing Then
    '            If wo.Employee.Count > 0 Then UpdateAuditUnloaderChanges(wo)
    '        End If
    '    End If
    '    Dim utls As New Utilities
    '    If Left(wo.CreatedBy, 8) = "Imported" Then
    '        wo.CreatedBy = utls.CreatedByToText(HttpContext.Current.Session("userID")) & ":Imported"
    '    End If
    '    If Left(wo.CreatedBy, 6) = "Excel_" Then
    '        wo.CreatedBy = utls.CreatedByToText(HttpContext.Current.Session("userID")) & ":Excel"
    '    End If


    '    If wo.PaymentType Is Nothing Then wo.PaymentType = ""
    '    dba.CommandText = "UPDATE WorkOrder SET Status=@Status, LogDate=@LogDate, LogNumber=@LogNumber, LoadNumber=@LoadNumber, LocationID=@LocationID, " & _
    '                    "DepartmentID=@DepartmentID, LoadTypeID=@LoadTypeID, PaymentType=@PaymentType, " & _
    '                    "CustomerID=@CustomerID, VendorNumber=@VendorNumber, ReceiptNumber=@ReceiptNumber, " & _
    '                    "PurchaseOrder=@PurchaseOrder, Amount=@Amount,SplitPaymentAmount=@SplitPaymentAmount, IsCash=@IsCash, LoadDescriptionID=@LoadDescriptionID, " & _
    '                    "CarrierID=@CarrierID, TruckNumber=@TruckNumber, " & _
    '                    "TrailerNumber=@TrailerNumber, AppointmentTime=@AppointmentTime, GateTime=@GateTime, DockTime=@DockTime, " & _
    '                    "StartTime=@StartTime, CompTime=@CompTime, TTLTime=@TTLTime, PalletsUnloaded=@PalletsUnloaded, DoorNumber=@DoorNumber, " & _
    '                    "Pieces=@Pieces, Weight=@Weight, Comments=@Comments, Restacks=@Restacks, PalletsReceived=@PalletsReceived, " & _
    '                    "BadPallets=@BadPallets, NumberOfItems=@NumberOfItems, CheckNumber=@CheckNumber, CreatedBy=@CreatedBy, BOL=@BOL " & _
    '                    "WHERE ID = @ID"
    '    dba.AddParameter("@Status", wo.Status)
    '    dba.AddParameter("@LogDate", wo.LogDate)
    '    dba.AddParameter("@LogNumber", wo.LogNumber)
    '    dba.AddParameter("@LoadNumber", wo.LoadNumber)
    '    dba.AddParameter("@LocationID", wo.LocationID)
    '    dba.AddParameter("@DepartmentID", wo.DepartmentID)
    '    dba.AddParameter("@LoadTypeID", wo.LoadTypeID)
    '    dba.AddParameter("@PaymentType", wo.PaymentType)
    '    dba.AddParameter("@CustomerID", wo.CustomerID)
    '    dba.AddParameter("@VendorNumber", wo.VendorNumber)
    '    dba.AddParameter("@ReceiptNumber", wo.ReceiptNumber)
    '    dba.AddParameter("@PurchaseOrder", wo.PurchaseOrder)
    '    dba.AddParameter("@Amount", wo.Amount)
    '    dba.AddParameter("@SplitPaymentAmount", wo.SplitPaymentAmount)
    '    dba.AddParameter("@IsCash", wo.IsCash)
    '    dba.AddParameter("@LoadDescriptionID", wo.LoadDescriptionID)
    '    dba.AddParameter("@CarrierID", wo.CarrierID)
    '    dba.AddParameter("@TruckNumber", wo.TruckNumber)
    '    dba.AddParameter("@TrailerNumber", wo.TrailerNumber)
    '    dba.AddParameter("@AppointmentTime", wo.AppointmentTime)
    '    dba.AddParameter("@GateTime", wo.GateTime)
    '    dba.AddParameter("@DockTime", wo.DockTime)
    '    dba.AddParameter("@StartTime", wo.StartTime)
    '    dba.AddParameter("@CompTime", wo.CompTime)
    '    dba.AddParameter("@TTLTime", wo.TTLTime)
    '    dba.AddParameter("@PalletsUnloaded", wo.PalletsUnloaded)
    '    dba.AddParameter("@DoorNumber", wo.DoorNumber)
    '    dba.AddParameter("@Pieces", wo.Pieces)
    '    dba.AddParameter("@Weight", wo.Weight)
    '    dba.AddParameter("@Comments", wo.Comments)
    '    dba.AddParameter("@Restacks", wo.Restacks)
    '    dba.AddParameter("@PalletsReceived", wo.PalletsReceived)
    '    dba.AddParameter("@BadPallets", wo.BadPallets)
    '    dba.AddParameter("@NumberOfItems", wo.NumberOfItems)
    '    dba.AddParameter("@CreatedBy", wo.CreatedBy)
    '    dba.AddParameter("@CheckNumber", wo.CheckNumber)
    '    dba.AddParameter("@BOL", wo.BOL)
    '    dba.AddParameter("@ID", wo.ID)
    '    Try
    '        Dim i As Integer = dba.ExecuteNonQuery()
    '    Catch ex As Exception
    '        svd = ex.Message
    '    End Try
    '    If FormatDateTime(Date.Now(), DateFormat.ShortDate) = FormatDateTime(oldWO.LogDate, DateFormat.ShortDate) Then
    '        If repl Then arDAL.doWorkOrderRepLog(wo)
    '    End If
    '    Return svd
    'End Function

    'Public Sub UpdateAuditUnloaderChanges(ByVal wo As WorkOrder)
    '    Dim dba As New DBAccess()
    '    Dim ds As DataSet = New DataSet
    '    Dim oestr As String = String.Empty
    '    Dim oestrCount As Integer = 0
    '    Dim nestr As String = String.Empty
    '    Dim nestrCount As Integer = 0
    '    dba.CommandText = "SELECT Employee.FirstName + ' ' + Employee.LastName AS Name " & _
    '        "FROM Employee INNER JOIN " & _
    '        "Unloader ON Employee.ID = Unloader.EmployeeID INNER JOIN " & _
    '        "WorkOrder ON Unloader.LoadID = WorkOrder.ID " & _
    '        "WHERE (WorkOrder.ID = @woid) "
    '    dba.AddParameter("@woid", wo.ID)
    '    ds = dba.ExecuteDataSet
    '    If ds.Tables(0).Rows.Count > 0 Then
    '        For Each rw As DataRow In ds.Tables(0).Rows
    '            oestrCount = oestrCount + 1
    '            oestr &= rw.Item("Name") & " - "
    '        Next
    '    End If
    '    If oestr.Length > 3 Then
    '        oestr = "[" & oestrCount.ToString & "] " & Left(oestr, oestr.Length - 3)
    '    Else
    '        oestr = "None Selected"
    '    End If

    '    If Not wo.Employee.Item(0) = "listcleared" Then
    '        For Each eStr As String In wo.Employee
    '            nestrCount = nestrCount + 1
    '            nestr &= Utilities.getRTDSuserNameByUserID(eStr) & " - "

    '        Next
    '        nestr = "[" & nestrCount.ToString & "] " & Left(nestr, nestr.Length - 3)
    '    Else
    '        nestrCount = 0
    '        nestr = "[" & nestrCount.ToString & "] " & "None Selected"
    '    End If


    '    Dim apdal As New AuditRepLogDAL()
    '    dba.CommandText = "SELECT LoadID, EmployeeID, ID FROM Unloader WHERE LoadID = @woid"
    '    dba.AddParameter("@woid", wo.ID)
    '    Dim dt As DataTable = dba.ExecuteDataSet.Tables(0)
    '    ' ******************** REMOVE Replication Log ****************************
    '    If FormatDateTime(Date.Now(), DateFormat.ShortDate) = FormatDateTime(wo.LogDate, DateFormat.ShortDate) Then
    '        If dt.Rows.Count > 0 Then
    '            For Each rw As DataRow In dt.Rows
    '                apdal.doUnloaderRepLog(rw.Item(0), rw.Item(1), rw.Item(2), 1)
    '            Next
    '        End If
    '    End If

    '    dba.CommandText = "DELETE FROM Unloader WHERE LoadID = @woid"
    '    dba.AddParameter("@woid", wo.ID)
    '    dba.ExecuteNonQuery()

    '    If nestrCount > 0 Then
    '        For Each eStr As String In wo.Employee
    '            Dim geStr As Guid = New Guid(eStr)
    '            Dim nid As Guid = Guid.NewGuid()
    '            dba.CommandText = "INSERT INTO Unloader (LoadID, EmployeeID, ID) VALUES (@LoadID, @EmployeeID, @ID)"
    '            dba.AddParameter("@LoadID", wo.ID)
    '            dba.AddParameter("@EmployeeID", eStr)
    '            dba.AddParameter("@ID", nid)
    '            dba.ExecuteNonQuery()
    '            apdal.doUnloaderRepLog(wo.ID, geStr, nid)
    '        Next
    '    End If

    '    Dim empdal As New empDAL()
    '    Dim rtdsUserID As String = Utilities.getRTDSidByUserID(HttpContext.Current.Session("userID").ToString)
    '    Dim emp As Employee = empdal.GetEmployeeByID(New Guid(rtdsUserID))
    '    Dim nm As String = emp.rtdsFirstName & " " & emp.rtdsLastName
    '    If nm.Length < 5 Then nm = HttpContext.Current.User.Identity.Name
    '    nm &= " : " & HttpContext.Current.Request.ServerVariables("REMOTE_ADDR")

    '    Dim FieldName As String = "Unloader"
    '    Dim oldValue As String = oestr
    '    Dim newValue As String = nestr
    '    apdal.UpdateAudit("WorkOrder", oestr, nestr, "Unloader", wo.ID)

    'End Sub

    'Public Function AddWorkOrder(ByVal wo As WorkOrder, Optional writeComments As Boolean = False, Optional repl As Boolean = True) As String
    '    Dim ut As New Utilities
    '    If wo.CreatedBy = "" Then wo.CreatedBy = ut.CreatedByToText(HttpContext.Current.Session("userID"))
    '    Dim dba As New DBAccess()
    '    '        Dim dt As DataTable
    '    If wo.PaymentType Is Nothing Then wo.PaymentType = ""
    '    dba.CommandText = "INSERT INTO WorkOrder (Status, LogDate, LogNumber, LoadNumber, LocationID, DepartmentID, LoadTypeID, CustomerID, " & _
    '        "VendorNumber, ReceiptNumber, PurchaseOrder, Amount,SplitPaymentAmount, IsCash, LoadDescriptionID, CarrierID, " & _
    '        "TruckNumber, TrailerNumber, AppointmentTime, GateTime, DockTime, StartTime, CompTime, TTLTime, " & _
    '        "PalletsUnloaded, DoorNumber, Pieces, Weight, Comments, Restacks, PalletsReceived, BadPallets, " & _
    '        "NumberOfItems, CheckNumber, BOL, ID, CreatedBy,PaymentType) " & _
    '        " VALUES (@Status, @LogDate, @LogNumber, @LoadNumber, @LocationID, @DepartmentID, @LoadTypeID, @CustomerID, " & _
    '        "@VendorNumber, @ReceiptNumber, @PurchaseOrder, @Amount,@SplitPaymentAmount, @IsCash, @LoadDescriptionID, @CarrierID, " & _
    '        "@TruckNumber, @TrailerNumber, @AppointmentTime, @GateTime, @DockTime, @StartTime, @CompTime, @TTLTime, " & _
    '        "@PalletsUnloaded, @DoorNumber, @Pieces, @Weight, @Comments, @Restacks, @PalletsReceived, @BadPallets, " & _
    '        "@NumberOfItems, @CheckNumber, @BOL, @ID, @CreatedBy,@PaymentType)"
    '    dba.AddParameter("@Status", wo.Status)
    '    dba.AddParameter("@LogDate", wo.LogDate)
    '    dba.AddParameter("@LogNumber", wo.LogNumber)
    '    dba.AddParameter("@LoadNumber", wo.LoadNumber)
    '    dba.AddParameter("@LocationID", wo.LocationID)
    '    dba.AddParameter("@DepartmentID", wo.DepartmentID)
    '    dba.AddParameter("@LoadTypeID", wo.LoadTypeID)
    '    dba.AddParameter("@CustomerID", wo.CustomerID)
    '    dba.AddParameter("@VendorNumber", wo.VendorNumber)
    '    dba.AddParameter("@ReceiptNumber", wo.ReceiptNumber)
    '    dba.AddParameter("@PurchaseOrder", wo.PurchaseOrder)
    '    dba.AddParameter("@Amount", wo.Amount)
    '    dba.AddParameter("@SplitPaymentAmount", wo.SplitPaymentAmount)
    '    dba.AddParameter("@IsCash", wo.IsCash)
    '    dba.AddParameter("@LoadDescriptionID", wo.LoadDescriptionID)
    '    dba.AddParameter("@CarrierID", wo.CarrierID)
    '    dba.AddParameter("@TruckNumber", wo.TruckNumber)
    '    dba.AddParameter("@TrailerNumber", wo.TrailerNumber)
    '    dba.AddParameter("@AppointmentTime", wo.AppointmentTime)
    '    dba.AddParameter("@GateTime", wo.GateTime)
    '    dba.AddParameter("@DockTime", wo.DockTime)
    '    dba.AddParameter("@StartTime", wo.StartTime)
    '    dba.AddParameter("@CompTime", wo.CompTime)
    '    dba.AddParameter("@TTLTime", wo.TTLTime)
    '    dba.AddParameter("@PalletsUnloaded", wo.PalletsUnloaded)
    '    dba.AddParameter("@DoorNumber", wo.DoorNumber)
    '    dba.AddParameter("@Pieces", wo.Pieces)
    '    dba.AddParameter("@Weight", wo.Weight)
    '    dba.AddParameter("@Comments", wo.Comments)
    '    dba.AddParameter("@Restacks", wo.Restacks)
    '    dba.AddParameter("@PalletsReceived", wo.PalletsReceived)
    '    dba.AddParameter("@BadPallets", wo.BadPallets)
    '    dba.AddParameter("@NumberOfItems", wo.NumberOfItems)
    '    dba.AddParameter("@CheckNumber", wo.CheckNumber)
    '    dba.AddParameter("@BOL", wo.BOL)
    '    dba.AddParameter("@ID", wo.ID)
    '    dba.AddParameter("@CreatedBy", wo.CreatedBy)
    '    dba.AddParameter("@PaymentType", wo.PaymentType)
    '    Try
    '        Dim i As Integer = dba.ExecuteNonQuery()
    '    Catch ex As Exception
    '        Dim errstr As String = ex.Message
    '        Return errstr
    '    End Try
    '    If Not wo.Employee Is Nothing Then

    '        If wo.Employee.Count > 0 Then

    '            For Each Str As String In wo.Employee
    '                dba.CommandText = "SELECT COUNT(ID) FROM Unloader WHERE LoadID=@LoadID AND EmployeeID=@EmployeeID"
    '                dba.AddParameter("@LoadID", wo.ID)
    '                dba.AddParameter("@EmployeeID", Str)
    '                Dim ex As Integer = dba.ExecuteScalar
    '                If ex = 0 Then
    '                    dba.CommandText = "INSERT INTO Unloader (LoadID, EmployeeID, ID) VALUES (@LoadID, @EmployeeID, @ID)"
    '                    dba.AddParameter("@LoadID", wo.ID)
    '                    dba.AddParameter("@EmployeeID", Str)
    '                    dba.AddParameter("@ID", Guid.NewGuid())
    '                    dba.ExecuteNonQuery()
    '                End If
    '            Next

    '        End If
    '    End If
    '    If Left(wo.CreatedBy, 9) <> "Imported:" Then
    '        If repl Then

    '            If FormatDateTime(Date.Now(), DateFormat.ShortDate) = FormatDateTime(wo.LogDate, DateFormat.ShortDate) Then
    '                Dim ardal As New AuditRepLogDAL()
    '                ardal.doWorkOrderRepLog(wo, 0, writeComments)
    '            End If
    '        Else
    '            Dim ardal As New AuditRepLogDAL()
    '            ardal.doWorkOrderRepLog(wo, 0, writeComments)
    '        End If
    '    End If

    '    Return "OK"

    'End Function

    'Public Function doesExistByDatePoLocation(ByVal logdate As Date, ByVal po As String, ByVal locaid As Guid) As DataTable
    '    Dim retval As DataTable = New DataTable
    '    logdate = logdate.ToShortDateString
    '    Dim dba As New DBAccess
    '    dba.CommandText = "SELECT WorkOrder.ID " & _
    '        "FROM WorkOrder INNER JOIN " & _
    '        "Location ON WorkOrder.LocationID = Location.ID " & _
    '        "WHERE (WorkOrder.LocationID = @LocaID) AND (WorkOrder.PurchaseOrder = @po) AND (WorkOrder.LogDate = CONVERT(DATETIME, @logdate, 102))"
    '    dba.AddParameter("@LocaID", locaid)
    '    dba.AddParameter("@po", po)
    '    dba.AddParameter("@logdate", logdate)
    '    retval = New DataTable
    '    retval = dba.ExecuteDataSet.Tables(0)
    '    Return retval
    'End Function
    'Public Function doesBackhaulExistByDatePoLocation(ByVal logdate As Date, ByVal po As String, ByVal locaid As Guid) As DataTable
    '    Dim retval As DataTable = New DataTable
    '    Dim dba As New DBAccess
    '    Dim backhaulid As String = "0369F50A-52CA-4C97-8323-650ADC182E04"
    '    dba.CommandText = "SELECT WorkOrder.ID " & _
    '        "FROM WorkOrder INNER JOIN " & _
    '        "Location ON WorkOrder.LocationID = Location.ID " & _
    '        "WHERE (WorkOrder.LocationID = @LocaID) AND (WorkOrder.PurchaseOrder = @po) AND (WorkOrder.LoadTypeID=@backhaulid) AND (WorkOrder.LogDate = CONVERT(DATETIME, @logdate, 102))"
    '    dba.AddParameter("@LocaID", locaid)
    '    dba.AddParameter("@po", po)
    '    dba.AddParameter("@logdate", logdate)
    '    dba.AddParameter("@backhaulid", backhaulid)
    '    retval = New DataTable
    '    retval = dba.ExecuteDataSet.Tables(0)
    '    Return retval
    'End Function

    'Public Function nextLoadNumber(ByVal logdate As Date) As Integer
    '    Dim strloadnumber As String = Format(logdate, "MMddHHmmss")
    '    Dim intloadnumber As Integer = CType(strloadnumber, Integer)
    '    nextLoadNumber = intloadnumber
    '    Return nextLoadNumber
    'End Function

    'Public Function getCustomerID(ByVal locaID As Guid, ByVal vNum As String) As Guid
    '    Dim dba As New DBAccess()
    '    dba.CommandText = "SELECT Vendor.ID " & _
    '        "FROM Vendor INNER JOIN " & _
    '        "ParentCompany ON Vendor.ParentCompanyID = ParentCompany.ID INNER JOIN " & _
    '        "Location ON ParentCompany.ID = Location.ParentCompanyID " & _
    '        "WHERE (Location.ID = @locaID) AND (Vendor.Number = @vNum) "
    '    dba.AddParameter("@locaID", locaID)
    '    dba.AddParameter("@vNum", vNum)
    '    Dim cid As Guid = dba.ExecuteScalar
    '    Return cid
    'End Function

    'Public Function getWorkOrderProductivityByDateRangeAndLocation(ByVal locaID As Guid, ByVal sDate As DateTime, ByVal eDate As DateTime) As DataTable
    '    Dim dt As DataTable = New DataTable
    '    Dim dba As New DBAccess()
    '    dba.CommandText = "SELECT CONVERT(varchar(10), dbo.WorkOrder.LogDate, 110) AS WorkDate, " & _
    '        "COUNT(DISTINCT dbo.WorkOrder.PurchaseOrder) AS NumOfPOs, " & _
    '        "COUNT(DISTINCT CONVERT(varchar(10), dbo.WorkOrder.LogDate, 110)+ dbo.LoadType.Name + dbo.WorkOrder.DoorNumber + dbo.WorkOrder.TrailerNumber) AS NumOfLoads, " & _
    '        "SUM(dbo.WorkOrder.PalletsUnloaded)AS PalUnld, " & _
    '        "SUM(dbo.WorkOrder.Pieces) AS Pieces, " & _
    '        "SUM(dbo.WorkOrder.PalletsReceived) AS PalRecd, " & _
    '        "SUM(dbo.WorkOrder.BadPallets) AS Bad, " & _
    '        "SUM(dbo.WorkOrder.Restacks) AS Resk, " & _
    '        "(SELECT SUM(dbo.TimeInOut.HoursWorked) AS HOURS " & _
    '        "FROM dbo.Employee INNER JOIN " & _
    '        "dbo.TimePunche ON dbo.Employee.ID = dbo.TimePunche.EmployeeID INNER JOIN " & _
    '        "dbo.TimeInOut ON dbo.TimePunche.ID = dbo.TimeInOut.TimepuncheID LEFT OUTER JOIN " & _
    '        "dbo.Location ON dbo.Employee.LocationID = dbo.Location.ID LEFT OUTER JOIN " & _
    '        "dbo.Employment ON dbo.Employee.ID = dbo.Employment.EmployeeID " & _
    '        "WHERE (dbo.Location.ID = @locaID) AND (dbo.TimePunche.DateWorked = CONVERT(varchar(10), dbo.WorkOrder.LogDate, 110)) AND " & _
    '        "(dbo.Employment.JobTitle LIKE 'Unloader%')) AS Hours " & _
    '        "FROM dbo.WorkOrder INNER JOIN " & _
    '        "dbo.Location AS Location_1 ON dbo.WorkOrder.LocationID = Location_1.ID INNER JOIN " & _
    '        "dbo.LoadType ON dbo.WorkOrder.LoadTypeID = dbo.LoadType.ID " & _
    '        "WHERE (Location_1.ID = @locaID) " & _
    '        "AND (dbo.WorkOrder.LogDate >= @startdate)  " & _
    '        "AND (dbo.WorkOrder.LogDate <= DATEADD(n, 1439, @enddate)) " & _
    '        "GROUP BY CONVERT(varchar(10), dbo.WorkOrder.LogDate, 110) " & _
    '        "ORDER BY WorkDate DESC "
    '    dba.AddParameter("@locaID", locaID)
    '    dba.AddParameter("@startdate", sDate)
    '    dba.AddParameter("@enddate", eDate)
    '    Try
    '        dt = dba.ExecuteDataSet.Tables(0)

    '    Catch ex As Exception

    '    End Try

    '    Return dt
    'End Function

    'Public Function getPercentLoadsOverTwo(ByVal locaID As Guid, ByVal sDate As DateTime, ByVal eDate As DateTime) As Double
    '    Dim retDbl As Double = 0
    '    Dim dba As New DBAccess()
    '    dba.CommandText = "SELECT Location_1.ID AS Location, COUNT(WorkOrder_1.ID) AS UnderTwo, " & _
    '        "(SELECT COUNT(dbo.WorkOrder.ID) AS OverTwo " & _
    '        "FROM dbo.Location INNER JOIN " & _
    '        "dbo.WorkOrder ON dbo.Location.ID = dbo.WorkOrder.LocationID INNER JOIN " & _
    '        "dbo.LoadType ON dbo.WorkOrder.LoadTypeID = dbo.LoadType.ID " & _
    '        "WHERE(dbo.WorkOrder.LogDate >= @startdate) AND (dbo.WorkOrder.LogDate <= DATEADD(n, 1439, @enddate)) AND  " & _
    '        "(dbo.LoadType.Name = 'Cash' OR " & _
    '        "dbo.LoadType.Name = 'Invoice') AND (dbo.Location.ID = @locaID) AND (DATEDIFF(n, dbo.WorkOrder.DockTime,  " & _
    '        "dbo.WorkOrder.CompTime) >= 120)) AS OverTwo " & _
    '        "FROM dbo.Location AS Location_1 INNER JOIN " & _
    '        "dbo.WorkOrder AS WorkOrder_1 ON Location_1.ID = WorkOrder_1.LocationID INNER JOIN " & _
    '        "dbo.LoadType AS LoadType_1 ON WorkOrder_1.LoadTypeID = LoadType_1.ID " & _
    '        "WHERE(WorkOrder_1.LogDate >= @startdate) AND (WorkOrder_1.LogDate <= DATEADD(n, 1439, @enddate)) AND (LoadType_1.Name = 'Cash' OR " & _
    '        "LoadType_1.Name = 'Invoice') AND (Location_1.ID = @locaID) " & _
    '        "GROUP BY Location_1.ID "
    '    dba.AddParameter("@locaID", locaID)
    '    dba.AddParameter("@startdate", sDate)
    '    dba.AddParameter("@enddate", eDate)
    '    Dim dt As DataTable = New DataTable()
    '    Try
    '        dt = dba.ExecuteDataSet.Tables(0)

    '    Catch ex As Exception

    '    End Try
    '    If dt.Rows.Count > 0 Then
    '        Dim rw As DataRow = dt.Rows(0)
    '        Dim under As Integer = rw.Item("UnderTwo")
    '        Dim over As Integer = rw.Item("OverTwo")
    '        Dim ttl As Integer = under + over
    '        retDbl = over / ttl
    '    End If
    '    Return retDbl

    'End Function

    'Public Function MoveToNoShowWorkOrder(ByVal wo As WorkOrder) As String
    '    Dim ut As New Utilities
    '    Dim dba As New DBAccess()
    '    'to do 
    '    'for each work order, first check for dupe
    '    'if wo.id already exist in backup then
    '    'do nothing
    '    'else
    '    ' do this below
    '    dba.CommandText = "INSERT INTO WorkOrderNoShow (Status, LogDate, LogNumber, LoadNumber, LocationID, DepartmentID, LoadTypeID, CustomerID, " & _
    '        "VendorNumber, ReceiptNumber, PurchaseOrder, Amount, IsCash, LoadDescriptionID, CarrierID, " & _
    '        "TruckNumber, TrailerNumber, AppointmentTime, GateTime, DockTime, StartTime, CompTime, TTLTime, " & _
    '        "PalletsUnloaded, DoorNumber, Pieces, Weight, Comments, Restacks, PalletsReceived, BadPallets, " & _
    '        "NumberOfItems, CheckNumber, BOL, ID, CreatedBy) " & _
    '        " VALUES (@Status, @LogDate, @LogNumber, @LoadNumber, @LocationID, @DepartmentID, @LoadTypeID, @CustomerID, " & _
    '        "@VendorNumber, @ReceiptNumber, @PurchaseOrder, @Amount, @IsCash, @LoadDescriptionID, @CarrierID, " & _
    '        "@TruckNumber, @TrailerNumber, @AppointmentTime, @GateTime, @DockTime, @StartTime, @CompTime, @TTLTime, " & _
    '        "@PalletsUnloaded, @DoorNumber, @Pieces, @Weight, @Comments, @Restacks, @PalletsReceived, @BadPallets, " & _
    '        "@NumberOfItems, @CheckNumber, @BOL, @ID, @CreatedBy)"
    '    dba.AddParameter("@Status", wo.Status)
    '    dba.AddParameter("@LogDate", wo.LogDate)
    '    dba.AddParameter("@LogNumber", wo.LogNumber)
    '    dba.AddParameter("@LoadNumber", wo.LoadNumber)
    '    dba.AddParameter("@LocationID", wo.LocationID)
    '    dba.AddParameter("@DepartmentID", wo.DepartmentID)
    '    dba.AddParameter("@LoadTypeID", wo.LoadTypeID)
    '    dba.AddParameter("@CustomerID", wo.CustomerID)
    '    dba.AddParameter("@VendorNumber", wo.VendorNumber)
    '    dba.AddParameter("@ReceiptNumber", wo.ReceiptNumber)
    '    dba.AddParameter("@PurchaseOrder", wo.PurchaseOrder)
    '    dba.AddParameter("@Amount", wo.Amount)
    '    dba.AddParameter("@IsCash", wo.IsCash)
    '    dba.AddParameter("@LoadDescriptionID", wo.LoadDescriptionID)
    '    dba.AddParameter("@CarrierID", wo.CarrierID)
    '    dba.AddParameter("@TruckNumber", wo.TruckNumber)
    '    dba.AddParameter("@TrailerNumber", wo.TrailerNumber)
    '    dba.AddParameter("@AppointmentTime", wo.AppointmentTime)
    '    dba.AddParameter("@GateTime", wo.GateTime)
    '    dba.AddParameter("@DockTime", wo.DockTime)
    '    dba.AddParameter("@StartTime", wo.StartTime)
    '    dba.AddParameter("@CompTime", wo.CompTime)
    '    dba.AddParameter("@TTLTime", wo.TTLTime)
    '    dba.AddParameter("@PalletsUnloaded", wo.PalletsUnloaded)
    '    dba.AddParameter("@DoorNumber", wo.DoorNumber)
    '    dba.AddParameter("@Pieces", wo.Pieces)
    '    dba.AddParameter("@Weight", wo.Weight)
    '    dba.AddParameter("@Comments", wo.Comments)
    '    dba.AddParameter("@Restacks", wo.Restacks)
    '    dba.AddParameter("@PalletsReceived", wo.PalletsReceived)
    '    dba.AddParameter("@BadPallets", wo.BadPallets)
    '    dba.AddParameter("@NumberOfItems", wo.NumberOfItems)
    '    dba.AddParameter("@CheckNumber", wo.CheckNumber)
    '    dba.AddParameter("@BOL", wo.BOL)
    '    dba.AddParameter("@ID", wo.ID)
    '    dba.AddParameter("@CreatedBy", wo.CreatedBy)

    '    Try
    '        Dim i As Integer = dba.ExecuteNonQuery()
    '    Catch ex As Exception

    '    End Try
    '    ' end if

    '    dba.CommandText = "DELETE FROM WorkOrder WHERE ID=@woID"
    '    dba.AddParameter("@woID", wo.ID)
    '    Try
    '        dba.ExecuteNonQuery()
    '    Catch ex As Exception

    '    End Try

    '    Return "OK"

    'End Function


#Region "notes / snippets"
    'Public Function SaveNewLoad(ByVal wo As WorkOrder) As String
    '    Dim svd As String = String.Empty
    '    Dim wodal As New WorkOrderDAL()
    '    Dim dba As New DBAccess()
    '    dba.CommandText = "INSERT INTO WorkOrder (Status, LogDate, LogNumber, LoadNumber, LocationID, DepartmentID, LoadTypeID, CustomerID, " & _
    '        "VendorNumber, ReceiptNumber, PurchaseOrder, Amount, IsCash, LoadDescriptionID, CarrierID, " & _
    '        "TruckNumber, TrailerNumber, AppointmentTime, GateTime, DockTime, StartTime, CompTime, TTLTime, " & _
    '        "PalletsUnloaded, DoorNumber, Pieces, Weight, Comments, Restacks, PalletsReceived, BadPallets, " & _
    '        "NumberOfItems, CheckNumber, BOL, ID, CreatedBy) " & _
    '        " VALUES (@Status, @LogDate, @LogNumber, @LoadNumber, @LocationID, @DepartmentID, @LoadTypeID, @CustomerID, " & _
    '        "@VendorNumber, @ReceiptNumber, @PurchaseOrder, @Amount, @IsCash, @LoadDescriptionID, @CarrierID, " & _
    '        "@TruckNumber, @TrailerNumber, @AppointmentTime, @GateTime, @DockTime, @StartTime, @CompTime, @TTLTime, " & _
    '        "@PalletsUnloaded, @DoorNumber, @Pieces, @Weight, @Comments, @Restacks, @PalletsReceived, @BadPallets, " & _
    '        "@NumberOfItems, @CheckNumber, @BOL, @ID, @CreatedBy"
    '    dba.AddParameter("@Status", wo.Status)
    '    dba.AddParameter("@LogDate", wo.LogDate)
    '    dba.AddParameter("@LogNumber", wo.LogNumber)
    '    dba.AddParameter("@LoadNumber", wo.LoadNumber)
    '    dba.AddParameter("@LocationID", wo.LocationID)
    '    dba.AddParameter("@DepartmentID", wo.DepartmentID)
    '    dba.AddParameter("@LoadTypeID", wo.LoadTypeID)
    '    dba.AddParameter("@CustomerID", wo.CustomerID)
    '    dba.AddParameter("@VendorNumber", wo.VendorNumber)
    '    dba.AddParameter("@ReceiptNumber", wo.ReceiptNumber)
    '    dba.AddParameter("@PurchaseOrder", wo.PurchaseOrder)
    '    dba.AddParameter("@Amount", wo.Amount)
    '    dba.AddParameter("@IsCash", wo.IsCash)
    '    dba.AddParameter("@LoadDescriptionID", wo.LoadDescriptionID)
    '    dba.AddParameter("@CarrierID", wo.CarrierID)
    '    dba.AddParameter("@TruckNumber", wo.TruckNumber)
    '    dba.AddParameter("@TrailerNumber", wo.TrailerNumber)
    '    dba.AddParameter("@AppointmentTime", wo.AppointmentTime)
    '    dba.AddParameter("@GateTime", wo.GateTime)
    '    dba.AddParameter("@DockTime", wo.DockTime)
    '    dba.AddParameter("@StartTime", wo.StartTime)
    '    dba.AddParameter("@CompTime", wo.CompTime)
    '    dba.AddParameter("@TTLTime", wo.TTLTime)
    '    dba.AddParameter("@PalletsUnloaded", wo.PalletsUnloaded)
    '    dba.AddParameter("@DoorNumber", wo.DoorNumber)
    '    dba.AddParameter("@Pieces", wo.Pieces)
    '    dba.AddParameter("@Weight", wo.Weight)
    '    dba.AddParameter("@Comments", wo.Comments)
    '    dba.AddParameter("@Restacks", wo.Restacks)
    '    dba.AddParameter("@PalletsReceived", wo.PalletsReceived)
    '    dba.AddParameter("@BadPallets", wo.BadPallets)
    '    dba.AddParameter("@NumberOfItems", wo.NumberOfItems)
    '    dba.AddParameter("@CheckNumber", wo.CheckNumber)
    '    dba.AddParameter("@BOL", wo.BOL)
    '    dba.AddParameter("@ID", wo.ID)
    '    dba.AddParameter("@CreatedBy", wo.CreatedBy)
    '    Dim i As Integer = dba.ExecuteNonQuery()
    '    Return svd

    'End Function

#End Region
End Class

'dba.CommandText = "INSERT INTO WORKORDER (Status, LogDate, LogNumber, LoadNumber, LocationID, DepartmentID, LoadTypeID, CustomerID, VendorNumber,  " & _
'    "ReceiptNumber, PurchaseOrder, Amount, IsCash, LoadDescriptionID, CarrierID, TruckNumber, TrailerNumber,  " & _
'    "AppointmentTime, GateTime, DockTime, StartTime, CompTime, TTLTime, PalletsUnloaded, DoorNumber, Pieces,  " & _
'    "Weight, Comments, Restacks, PalletsReceived, BadPallets, NumberOfItems, CheckNumber, BOL, ID, CreatedBy)  " & _
'    "VALUES (@Status, @LogDate, @LogNumber, @LoadNumber, @LocationID, @DepartmentID, @LoadTypeID, @CustomerID, @VendorNumber,  " & _
'    "@ReceiptNumber, @PurchaseOrder, @Amount, @IsCash, @LoadDescriptionID, @CarrierID, @TruckNumber, @TrailerNumber,  " & _
'    "@AppointmentTime, @GateTime, @DockTime, @StartTime, @CompTime, @TTLTime, @PalletsUnloaded, @DoorNumber, @Pieces,  " & _
'    "@Weight, @Comments, @Restacks, @PalletsReceived, @BadPallets, @NumberOfItems, @CheckNumber, @BOL, @ID, @CreatedBy) "

'dba.AddParameter("@Status", wo.Status)
'dba.AddParameter("@LogDate", wo.LogDate)
'dba.AddParameter("@LogNumber", wo.LogNumber)
'dba.AddParameter("@LoadNumber", wo.LoadNumber)
'dba.AddParameter("@LocationID", wo.LocationID)
'dba.AddParameter("@DepartmentID", wo.DepartmentID)
'dba.AddParameter("@LoadTypeID", wo.LoadTypeID)
'dba.AddParameter("@CustomerID", wo.CustomerID)
'dba.AddParameter("@VendorNumber", wo.VendorNumber)
'dba.AddParameter("@ReceiptNumber", wo.ReceiptNumber)
'dba.AddParameter("@PurchaseOrder", wo.PurchaseOrder)
'dba.AddParameter("@Amount", wo.Amount)
'dba.AddParameter("@IsCash", wo.IsCash)
'dba.AddParameter("@LoadDescriptionID", wo.LoadDescriptionID)
'dba.AddParameter("@CarrierID", wo.CarrierID)
'dba.AddParameter("@TruckNumber", wo.TruckNumber)
'dba.AddParameter("@TrailerNumber", wo.TrailerNumber)
'dba.AddParameter("@AppointmentTime", wo.AppointmentTime)
'dba.AddParameter("@GateTime", wo.GateTime)
'dba.AddParameter("@DockTime", wo.DockTime)
'dba.AddParameter("@StartTime", wo.StartTime)
'dba.AddParameter("@CompTime", wo.CompTime)
'dba.AddParameter("@TTLTime", wo.TTLTime)
'dba.AddParameter("@PalletsUnloaded", wo.PalletsUnloaded)
'dba.AddParameter("@DoorNumber", wo.DoorNumber)
'dba.AddParameter("@Pieces", wo.Pieces)
'dba.AddParameter("@Weight", wo.Weight)
'dba.AddParameter("@Comments", wo.Comments)
'dba.AddParameter("@Restacks", wo.Restacks)
'dba.AddParameter("@PalletsReceived", wo.PalletsReceived)
'dba.AddParameter("@BadPallets", wo.BadPallets)
'dba.AddParameter("@NumberOfItems", wo.NumberOfItems)
'dba.AddParameter("@CheckNumber", wo.CheckNumber)
'dba.AddParameter("@BOL", wo.BOL)
'dba.AddParameter("@ID", wo.ID)
'dba.AddParameter("@CreatedBy", wo.CreatedBy)

