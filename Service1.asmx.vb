Imports SmarterTrack.Connector
Imports System.Web.Services
Imports System.ComponentModel
Imports System.Xml
Imports System.IO
'Imports DiversifiedLogistics
'Imports System.Data
'Imports System.Configuration
'Imports System.Data.SqlClient
'Imports System.ServiceModel
'Imports System.Runtime.Serialization


' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
<System.Web.Script.Services.ScriptService()>
<System.Web.Services.WebService(Namespace:="http://seu-us.com/")>
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)>
<ToolboxItem(False)>
Public Class Service1
    Inherits System.Web.Services.WebService
    Implements IExternalLoginProvider, IExternalCustomFieldProvider, IExternalTicketProvider
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

    <WebMethod(Description:="Returns list of Employees", EnableSession:=False)>
    Public Function GetEmployeesByLocation(ByVal locationId As String) As DataSet
        Dim ds As DataSet = Nothing
        Dim dba As New DBAccess
        dba.CommandText = "SELECT ID, FirstName, LastName, Login, Password " &
            "From Employee WHERE LocationID = @locationId And (TermDate < GETDATE())"
        dba.AddParameter("@locationId", locationId)
        ds = dba.ExecuteDataSet()
        Return ds
    End Function

    <WebMethod(Description:="Returns list of Employees with photo data", EnableSession:=False)>
    Public Function GetEmployeesByLocationwPhoto(ByVal locationId As String) As DataSet
        Dim ds As DataSet = Nothing
        Dim dba As New DBAccess
        dba.CommandText = "SELECT ID, FirstName, LastName, PhotoJpegData, Login, Password " &
            "From Employee WHERE LocationID = @locationId And (TermDate < GETDATE())"
        dba.AddParameter("@locationId", locationId)
        ds = dba.ExecuteDataSet()
        Return ds
    End Function

    <WebMethod(Description:="Returns single Employee w/ photo data", EnableSession:=False)>
    Public Function GetEmployeewPhoto(ByVal employeeId As String) As DataTable
        Dim dba As New DBAccess
        dba.CommandText = "Select FirstName, LastName,PhotoJpegData, LocationID, LOgin, Password from Employee where ID = '" & employeeId & "'"
        Dim dt As DataTable
        dt = dba.ExecuteDataSet.Tables(0)
        If dt.Rows.Count > 0 Then
            Return dt
        Else
            dt = New DataTable
        End If
        Return Nothing
    End Function

    <WebMethod(Description:="Returns single Employee", EnableSession:=False)>
    Public Function GetEmployee(ByVal employeeId As String) As DataTable
        Dim dba As New DBAccess
        dba.CommandText = "Select FirstName, LastName,LocationID, LOgin, Password from Employee where ID = '" & employeeId & "'"
        Dim dt As DataTable
        dt = dba.ExecuteDataSet.Tables(0)
        If dt.Rows.Count > 0 Then
            Return dt
        Else
            dt = New DataTable
        End If
        Return Nothing
    End Function

    <WebMethod(Description:="Returns first and last name of Employees with birthday today", EnableSession:=False)>
    Public Function GetBirthdays(ByVal locationID As String) As DataSet
        Dim ds As DataSet = New DataSet
        Dim dba As New DBAccess("divlogHR")
        dba.CommandText = "SELECT EmployeeFirstName, EmployeeLastName FROM Employees " &
            "WHERE DATEPART(d, EmployeeDOB) = DATEPART(d, GETDATE()) " &
            "AND DATEPART(m, EmployeeDOB) = DATEPART(m, GETDATE()) " &
            "AND LocaID = @locaID" ' @@@@@ Stored as string in 0/0/00 format 0/0/00
        dba.AddParameter("@locaID", locationID)
        ds = dba.ExecuteDataSet
        Return ds
    End Function

    <WebMethod(Description:="Returns single WorkOrder", EnableSession:=False)>
    Public Function GetWorkOrder(ByVal workorderid As String) As XmlDocument
        Dim xmldoc As New XmlDocument
        Dim wo As Object = New WorkOrder
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
        dba.AddParameter("@loadID", workorderid)
        Dim dsWorkOrder As DataSet = dba.ExecuteDataSet
        Dim xmlSW As System.IO.StreamWriter = New System.IO.StreamWriter("WorkOrder.xml")
        dsWorkOrder.WriteXml(xmlSW)
        xmlSW.Close()
        Dim fs As New FileStream("WorkOrder.xml", FileMode.Open, FileAccess.Read)
        xmldoc.Load(fs)
        Return xmldoc

    End Function

    <WebMethod(Description:="Returns list of open loads", EnableSession:=False)>
    Public Function GetEditLoads(ByVal locaID As String, ByVal sDate As DateTime, ByVal eDate As DateTime) As DataSet 'As List(Of Load)
        'Dim loadList As New List(Of Load)
        Dim xmlDoc As New XmlDocument
        Dim dteditLoads As New DataTable()
        Dim dba As New DBAccess()
        eDate = DateAdd(DateInterval.Second, 86399, eDate) 'advance enddate by 23hrs 59mins 59 secs
        If locaID.Length < 6 Then
            locaID = Guid.NewGuid().ToString
        End If
        Dim cmd As String = "SELECT WO.Status, WO.LogDate, WO.LogNumber, WO.LoadNumber, WO.LocationID, WO.DepartmentID, WO.LoadTypeID, WO.CustomerID,  " &
            "WO.VendorNumber, V.Name AS VendorName, WO.ReceiptNumber, WO.PurchaseOrder, WO.Amount,WO.SplitPaymentAmount, WO.IsCash, WO.LoadDescriptionID, WO.CarrierID,  " &
            "Car.Name AS CarrierName, WO.TruckNumber, WO.TrailerNumber, WO.AppointmentTime, WO.GateTime, WO.DockTime, WO.StartTime, WO.CompTime,  " &
            "WO.TTLTime, WO.PalletsUnloaded, WO.DoorNumber, WO.Pieces, WO.Weight, WO.Comments, WO.Restacks, WO.PalletsReceived, WO.BadPallets,  " &
            "WO.NumberOfItems, WO.CheckNumber, WO.BOL, WO.ID, VWO.userID, VWO.timeStamp, Dept.Name AS DepartmentName,  " &
            "Des.Name AS LoadDescription, LoadType.Name AS LoadTypeName, " &
            "(SELECT COUNT(WorkOrderID) AS PicCount " &
            "FROM dbo.LoadImages " &
            "GROUP BY WorkOrderID " &
            "HAVING (WorkOrderID = WO.ID)) AS PicCount " &
            "FROM WorkOrder AS WO LEFT OUTER JOIN " &
            "LoadType ON WO.LoadTypeID = LoadType.ID LEFT OUTER JOIN " &
            "Vendor AS V ON WO.CustomerID = V.ID LEFT OUTER JOIN " &
            "Carrier AS Car ON WO.CarrierID = Car.ID LEFT OUTER JOIN " &
            "Department AS Dept ON WO.DepartmentID = Dept.ID LEFT OUTER JOIN " &
            "Description AS Des ON WO.LoadDescriptionID = Des.ID LEFT OUTER JOIN " &
            "VerifiedWorkOrders AS VWO ON WO.ID = VWO.workOrderID " &
            "WHERE (WO.LocationID = @locaID) AND (WO.LogDate >= @sDate) AND (WO.LogDate <= @eDate) " ' AND (WO.Status < 79)"
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

        Dim dsEditLoads As DataSet = dba.ExecuteDataSet
        Dim thucount As Integer = dsEditLoads.Tables(0).Rows.Count
        dteditLoads = dsEditLoads.Tables(0)
        '        Dim xmlSW As System.IO.StreamWriter = New System.IO.StreamWriter("EditLoads.xml")
        '        dsEditLoads.WriteXml(xmlSW, XmlWriteMode.WriteSchema)
        '        xmlSW.Close()
        '        Dim fs As New FileStream("EditLoads.xml", FileMode.Open, FileAccess.Read)
        '        xmlDoc.Load(fs)
        Return dsEditLoads
        '        Return dt   ' loadList
    End Function

    <WebMethod(Description:="Returns name of location parent company", EnableSession:=False)>
    Public Function getParentCompany(ByVal locationID As String) As String
        Dim parentCompany As String = String.Empty
        Dim dba As New DBAccess
        dba.CommandText = "SELECT ParentCompany.Name " &
            "FROM Location INNER JOIN " &
            "ParentCompany ON Location.ParentCompanyID = ParentCompany.ID " &
            "WHERE (Location.ID = '" & locationID & "')"
        parentCompany = dba.ExecuteScalar()

        Return parentCompany
    End Function

    <WebMethod(Description:="Returns location specific Dock Monitor Banners", EnableSession:=False)>
    Public Function getBanners(ByVal locationID As String) As DataSet
        Dim ds As New DataSet
        Dim dba As New DBAccess
        dba.CommandText = "SELECT Banner From DockMonitorBanners WHERE Enabled=1 and LocationID=@locaID ORDER BY SortOrder"
        dba.AddParameter("@locaID", locationID)
        ds = dba.ExecuteDataSet
        Return ds
    End Function

    <WebMethod(Description:="Returns Dock Monitor load grids data", EnableSession:=False)>
    Public Function getDockMonitorReportData(ByVal locationName As String, ByVal showAll As Boolean) As DataSet
        Dim dba As New DBAccess
        'default True = all loads False hides completed (active loads only)
        '        Dim constr As String = "Data Source=reports.div-log.com;Initial Catalog=RTDS;Persist Security Info=True;User ID=rtds;Password=southeast1"
        If showAll Then
            dba.CommandText = "SELECT DoorNumber, Vendor, PurchaseOrder, Carrier, TrailerNumber, AppointmentTime, DockTime, StartTime, CompTime, Department, ID, Unloaders " &
                "FROM dbo.tblfunc_DockMonAllLoads('" & locationName & "') AS DockMonAllLoads" 'all loads
        Else
            dba.CommandText = "SELECT DoorNumber, Vendor, PurchaseOrder, Carrier, TrailerNumber, AppointmentTime, DockTime, StartTime, CompTime, Department, ID, Unloaders " &
                "FROM dbo.tblfunc_DockMonOpenLoads('" & locationName & "') AS DockMonOpenLoads" 'hides completed
        End If
        'Dim sqldataadapter As SqlDataAdapter = New SqlDataAdapter(strSql, constr)
        'sqldataadapter.SelectCommand.CommandTimeout = 120
        'sqldataadapter.SelectCommand.CommandType = CommandType.Text
        Dim dt As DataTable = New DataTable
        Dim dsDMLoads As DataSet = New DataSet
        Dim xmldoc As New XmlDocument
        Try
            dsDMLoads = dba.ExecuteDataSet() 'got a dataset
            'TODO Create a streamwriter to do what?
            '            Dim xmlSW As System.IO.StreamWriter = New System.IO.StreamWriter("dsDMLoads.xml")
            '            dsDMLoads.WriteXml(xmlSW, XmlWriteMode.IgnoreSchema)
            '            xmldoc.Load(dsDMLoads.GetXml)
            '            File.Delete("dsDMLoads.xml")
            dt = dsDMLoads.Tables(0)
        Catch ex As Exception
            Dim err As String = ex.Message()
        End Try
        If dt.Rows.Count = 0 Then
            dt = Nothing
        End If
        '        Dim fs As New FileStream("dmrLoads.xml", FileMode.Open, FileAccess.Read)
        '        xmldoc.Load(xmlDS)
        Return dsDMLoads

    End Function

    'TODO put guage values into native object for getDockMonitorGuageValues to work
    '<WebMethod(Description:="Returns Dock Monitor gauge data", EnableSession:=False)>
    'Public Function getDockMonitorGuageValues(ByVal locationName As String, ByRef piecesScheduled As Integer, ByRef piecesUnloaded As Integer, ByRef lblLocation As Label) As Object
    '    'get scheduled
    '    Dim dba As New DBAccess
    '    Dim schedTry As Integer = 0
    '    dba.CommandText = "Select SUM(WorkOrder.Pieces) As PiecesScheduled " &
    '        "FROM Location As Location INNER JOIN " &
    '        "WorkOrder As WorkOrder On Location.ID = WorkOrder.LocationID " &
    '        "WHERE (Location.Name = @location) " &
    '        "GROUP BY CONVERT(VARCHAR(10), WorkOrder.LogDate, 101), Location.Name " &
    '        "HAVING (CONVERT(VARCHAR(10), WorkOrder.LogDate, 101) = CONVERT(VARCHAR(10), GETDATE(), 101))"
    '    dba.AddParameter("@location", locationName)
    '    Try
    '        While schedTry < 4
    '            Dim varpiecesScheduled As Integer
    '            varpiecesScheduled = dba.ExecuteScalar
    '            If varpiecesScheduled < 1 Then
    '                schedTry += 1
    '            Else
    '                piecesScheduled = varpiecesScheduled
    '                '                    exitme = 0
    '                schedTry = 4
    '            End If
    '        End While
    '    Catch ex As Exception
    '        lblLocation.Text = ex.Message
    '    End Try

    '    'get received\
    '    Dim unlTry As Integer = 0
    '    dba.CommandText = "Select SUM(WorkOrder.Pieces) As PiecesRecieved " &
    '        "FROM Location As Location INNER JOIN " &
    '        "WorkOrder As WorkOrder On Location.ID = WorkOrder.LocationID " &
    '        "WHERE (Location.Name = @location) And (CONVERT(VARCHAR(10), WorkOrder.LogDate, 101) = CONVERT(VARCHAR(10), GETDATE(), 101)) And  " &
    '        "(CONVERT(VARCHAR(10), WorkOrder.CompTime, 101) = CONVERT(VARCHAR(10), WorkOrder.LogDate, 101)) " &
    '        "GROUP BY CONVERT(VARCHAR(10), WorkOrder.LogDate, 110)"
    '    dba.AddParameter("@location", locationName)
    '    Try
    '        While unlTry < 4
    '            Dim varPiecesUnloaded As Integer
    '            varPiecesUnloaded = dba.ExecuteScalar
    '            If varPiecesUnloaded < 1 Then
    '                unlTry += 1
    '            Else
    '                piecesUnloaded = varPiecesUnloaded
    '                '                    exitme = 0
    '                unlTry = 4
    '            End If
    '        End While
    '    Catch ex As Exception
    '        lblLocation.Text = ex.Message
    '    End Try
    '    ''appy to report
    '    'TODO report data for guage
    '    Return "This Is  place holder End point that takes a locationId: " & locationName & " as parameter"
    'End Function


    ''<WebMethod()>
    ''Public Function Authorize(ByVal loginUsername As String, ByVal loginPassword As String) As String
    ''    Dim aw As ActiveDirectoryWrapper.ADWrapper = New ActiveDirectoryWrapper.ADWrapper()
    ''    Dim isauth As Boolean = aw.IsValidADLogin("div-log.com/RFinster", "Randy", "Finster")
    ''    Dim retstr As String = String.Empty ' isauth.ToString
    ''    ' TESTING        Dim path As String = "LDAP://mail.div-log.com:636/" 'local debug
    ''    Dim path As String = "LDAP://mail.div-log.com:636/" 'production

    ''    '        loginUsername = "div-log\z_LDAP" '<<-- to test this I used Bills credintials
    ''    '        loginPassword = "@ct$2016p"
    ''    Dim ldap As LdapAuthentication = New LdapAuthentication()
    ''    Dim sResult As SearchResult = ldap.IsAuthenticated("div-log.com", loginUsername, loginPassword)
    ''    If Not sResult Is Nothing Then
    ''        Dim name As String = ""
    ''        Dim pwLen As Integer = loginPassword.Length
    ''        Dim rpw As String = String.Empty
    ''        For i = 0 To pwLen
    ''            rpw &= "*"
    ''        Next
    ''        retstr = loginUsername & " " & rpw & " Good to go" & vbCrLf & sResult.Path
    ''        '           Dim narray() As Array = Nothing
    ''        '           sResult.Properties.CopyTo(narray, 0)
    ''    Else
    ''        retstr = "user not found"
    ''    End If
    ''    Return retstr
    ''End Function

#Region "Example External Login Provider"

    '  This function is used to authenticate user credentials when they login to the interface.  The return
    '  values are used to create and link an account from SmarterTrack into your system.
    '
    '  Inputs
    '      authPW = Validates that calling software is valid
    '      loginUsername
    '      loginPassword
    '  Outputs
    '      Authentication = OK or FAIL
    '      EmailAddress = Email address of user
    '      DisplayName = Display Name of User

    Private Const WebServicePassword As String = "@ct$2016p"

    <WebMethod(Description:="AD integration: External login Provider for Support Site", EnableSession:=False)>
    Public Function Authenticate(inputs As ExternalLoginProviderInputs) As ExternalLoginProviderResult
        Dim iData As New ParsedLoginInputData(inputs)
        Dim result As ExternalLoginProviderResult

        'Verify that all necessary criteria to call this function are met
        If iData.WebServiceAuthorizationCode <> WebServicePassword Then
            Return New ExternalLoginProviderResult(False, "Permission Denied")
        End If
        If String.IsNullOrEmpty(iData.LoginUsername) OrElse String.IsNullOrEmpty(iData.LoginPassword) Then
            Return New ExternalLoginProviderResult(False, "Required Input Missing")
        End If

        Dim adRecord As New ADrecord
        If LDAP.AuthenticateLogin(adRecord, "div-log.com", iData.LoginUsername, iData.LoginPassword, LDAP.NetworkType.ActiveDirectory) = LDAP.LDAPResult.Authenticated Then
            result = New ExternalLoginProviderResult(True, "Login Successful", "Authentication=OK")

            result.OutputVariables.Add("EmailAddress=" & adRecord.email)
            ' optional
            result.OutputVariables.Add("DisplayName=" & adRecord.cn)
            ' optional
            Return result
        End If
        '        Return a failure If there's no match
        Return New ExternalLoginProviderResult(True, "Login Failure", "Authentication=FAIL")
    End Function

    'this version of Authenticate takes input from form
    <WebMethod(Description:="AD integration Test: External login Provider for Support Site", EnableSession:=False)>
    Public Function tAuthenticate(ByVal LoginUsername As String, loginPassword As String) As ExternalLoginProviderResult ' As ExternalLoginProviderInputs) As ExternalLoginProviderResult
        Dim _inputs As New ExternalLoginProviderInputs
        _inputs.InputVariables.Add("authPW=@ct$2016p")
        _inputs.InputVariables.Add("loginUsername=" & LoginUsername)
        _inputs.InputVariables.Add("loginPassword=" & loginPassword)
        Dim iData As New ParsedLoginInputData(_inputs)

        Dim result As ExternalLoginProviderResult
        Dim input As ExternalLoginProviderInputs = New ExternalLoginProviderInputs()

        'Verify that all necessary criteria to call this function are met
        If iData.WebServiceAuthorizationCode <> WebServicePassword Then
            Return New ExternalLoginProviderResult(False, "Permission Denied")
        End If
        If String.IsNullOrEmpty(iData.LoginUsername) OrElse String.IsNullOrEmpty(iData.LoginPassword) Then
            Return New ExternalLoginProviderResult(False, "Required Input Missing")
        End If

        ' This is a sample of how to do a basic username/password check (use your own database or list here)
        'Dim unlv As String = iData.LoginUsername.ToLowerInvariant
        'Dim pwlv As String = iData.LoginPassword.ToLowerInvariant
        ''           If iData.LoginUsername.ToLowerInvariant() = "myusername" AndAlso iData.LoginPassword.ToLowerInvariant() = "mypassword" Then
        'result = New ExternalLoginProviderResult()
        '    result.Success = True
        '    result.Message = "Login Successful"
        '    result.OutputVariables.Add("Authentication=OK")
        '    result.OutputVariables.Add("EmailAddress=putUsersEmailAddressHere@example.com")
        '    ' optional
        '    result.OutputVariables.Add("DisplayName=putUsersFullNameHere")
        '    ' optional
        '    Return result
        ''           End If

        ' Below is a sample Active Directory Authentication implementation

        Dim adRecord As New ADrecord
        If LDAP.AuthenticateLogin(adRecord, "div-log.com", iData.LoginUsername, iData.LoginPassword, LDAP.NetworkType.ActiveDirectory) = LDAP.LDAPResult.Authenticated Then
            result = New ExternalLoginProviderResult(True, "Login Successful", "Authentication=OK")

            result.OutputVariables.Add("EmailAddress=" & adRecord.email)
            ' optional
            result.OutputVariables.Add("DisplayName=" & adRecord.cn)
            ' optional
            Return result
        End If
        '        Return a failure If there's no match
        Return New ExternalLoginProviderResult(True, "Login Failure", "Authentication=FAIL")
    End Function

#End Region

#Region "Example External Custom Field Provider"

    '  A lot can be done with the custom field provider.  It is called at two steps during the ticket and chat creation process.
    '  This example function shows several potential samples that can be uncommented and altered as needed.
    '
    '  When the person begins the ticket or chat process, this function is called with location=PRE.  This gives you a chance to
    '  customize the custom fields that show up for the person.  You may want to show a list of their domains or products, for
    '  example.
    '
    '  When a person finishes the pre-chat or pre-ticket step (the step where the search fields appear), this function is called 
    '  again with location=REGULAR.  This gives you a chance to fill out the data values of custom fields.  You may want to show
    '  lists based on the selection in the PRE step.  This is NOT the time to store custom fields for the user.  This should
    '  happen with the StartTicket or StartChat functions
    '
    '  Inputs
    '      authPW = Validates that calling software is valid
    '      loginUsername
    '      departmentName
    '      location = PRE or REGULAR (pre called after department selection made.  Regular called after search page and before compose page)
    '      cf_* = value of current custom fields, such as "cf_ticket type=phone ticket"
    '  Outputs
    '      CanStartTickets = true or false
    '      CanStartChats = true or false
    '		Custom field overrides

    '<WebMethod>
    'Public Function GetCustomFieldOptions(inputs As ExternalCustomFieldProviderInputs) As ExternalCustomFieldProviderResult
    '    Dim iData As New ParsedCustomFieldInputData(inputs)

    '    ' Verify that all necessary criteria to call this function are met
    '    If iData.WebServiceAuthorizationCode <> WebServicePassword Then
    '        Return New ExternalCustomFieldProviderResult(False, "Permission Denied")
    '    End If

    '    Dim canStartTickets As Boolean = True
    '    Dim canStartChats As Boolean = True
    '    Dim result As New ExternalCustomFieldProviderResult(True, "Result okay")
    '    Dim cfdata As ExternalCustomFieldData = New ExternalCustomFieldData()

    '    '#Region "Example: Allow tickets/chats to only certain customers"
    '    '
    '    '			 * You could check a database and see if the user has the ability to contact certain departments, for example.
    '    '			 * In this example, only the user "myusername" is allowed to start tickets and chats for all departments.  
    '    '			 * Everyone else can only start chats (not tickets), and can only contact the sales department
    '    '			 

    '    'if (iData.LoginUsername == "myusername")
    '    '{
    '    '    canStartTickets = true;
    '    '    canStartChats = true;
    '    '}
    '    'else
    '    '{
    '    '    result.Message = "You are not currently permitted to contact that department."; // shown in ticket pages
    '    '    canStartTickets = false;
    '    '    if (iData.DepartmentName.ToUpperInvariant() == "SALES DEPARTMENT")
    '    '        canStartChats = true;
    '    '    else
    '    '        canStartChats = false;
    '    '}
    '    '#End Region

    '    '#Region "Example: Set a custom list of domains for them to choose from"
    '    '
    '    '			 * This example shows how to fill custom field drop-down lists for the user based on their login.  In this case,
    '    '			 * we fill a dropdown list with their accounts listed in our database, so that they can choose which one 
    '    '			 * the support is for.
    '    '			 * 
    '    '			 * It is important that the custom field already exist in SmarterTrack
    '    '			 * 

    '    'if (iData.DisplayLocation == DisplayLocations.Page1_Pre && iData.DepartmentName.ToUpperInvariant() == "SUPPORT DEPARTMENT")
    '    '{
    '    '    // Here you would look up the options.  For now, we'll assume we brought back abc.com and def.com from our database
    '    '    cfdata = new ExternalCustomFieldData();
    '    '    cfdata.CustomFieldName = "Account";
    '    '    cfdata.ChangeRequired = true;
    '    '    cfdata.NewRequiredValue = true;
    '    '    cfdata.ChangeListOptions = true;
    '    '    cfdata.DisplayWithInformationLookup = false; // Set this to true if you need to show/hide other custom fields based on the value they select.
    '    '    cfdata.ListOptions.Add("abc.com");
    '    '    cfdata.ListOptions.Add("def.com");
    '    '    result.CustomFieldDataItems.Add(cfdata);
    '    '}
    '    '#End Region

    '    '#Region "Example: Custom Issue Types by Department"
    '    '
    '    '			 * This example shows how to fill custom field drop-down lists for the user based on the department chosen.  In this case,
    '    '			 * we fill a dropdown list with possible types of issues
    '    '			 * 
    '    '			 * It is important that the custom field already exist in SmarterTrack
    '    '			 * 

    '    'cfdata = new ExternalCustomFieldData();
    '    'cfdata.CustomFieldName = "Type of Issue";
    '    'cfdata.ChangeListOptions = true;
    '    'cfdata.DisplayWithInformationLookup = false;
    '    'cfdata.ChangeRequired = true;
    '    'cfdata.NewRequiredValue = true;
    '    'cfdata.ChangeVisible = true;
    '    'cfdata.NewVisibleValue = true;
    '    'if (iData.DepartmentName.ToUpperInvariant().IndexOf("SALES") != -1)
    '    '{
    '    '    cfdata.ListOptions.Add("Change Plan");
    '    '    cfdata.ListOptions.Add("Help with order");
    '    '    cfdata.ListOptions.Add("Questions about plans");
    '    '    cfdata.ListOptions.Add("Questions about features");
    '    '    cfdata.ListOptions.Add("Help finding a product");
    '    '}
    '    'else if (iData.DepartmentName.ToUpperInvariant().IndexOf("SUPPORT") != -1)
    '    '{
    '    '    cfdata.ListOptions.Add("Web site issues");
    '    '    cfdata.ListOptions.Add("Database issues");
    '    '    cfdata.ListOptions.Add("Email issues");
    '    '    cfdata.ListOptions.Add("Dedicated server issues");
    '    '    cfdata.ListOptions.Add("Statistics issues");
    '    '    cfdata.ListOptions.Add("Network connectivity");
    '    '}
    '    'else
    '    '{
    '    '    // Removing issue types field, since there's no options
    '    '    cfdata.ChangeRequired = true;
    '    '    cfdata.NewRequiredValue = false;
    '    '    cfdata.ChangeVisible = true;
    '    '    cfdata.NewVisibleValue = false;
    '    '}
    '    'result.CustomFieldDataItems.Add(cfdata);
    '    '#End Region

    '    result.Success = True
    '    result.OutputVariables.Add("CanStartTickets=" + canStartTickets)
    '    result.OutputVariables.Add("CanStartChats=" + canStartChats)
    '    Return result
    'End Function

#End Region

#Region "IExternalTicketProvider Members"

    '  This function is called right after the person clicks on the Submit Ticket button.  This gives your provider one last
    '  chance to abort the process or store any custom fields.
    '
    '  Inputs
    '      authPW = Validates that calling software is valid
    '      loginUsername (for tickets submitted through web interface)
    '      emailFrom (for tickets submitted through email)
    '      departmentName
    '      subject
    '      ticketNumber = the ticket number about to be created
    '      cf_* = values of custom fields
    '  Outputs
    '      TicketCreation = TRUE or FALSE
    '      cf_* = output values of custom fields
    '    <WebMethod>
    '    Public Function StartTicket(inputs As ExternalTicketProviderInputs) As ExternalTicketProviderResult
    '        Dim iData As New ParsedTicketInputData(inputs)
    '        Dim result As New ExternalTicketProviderResult()

    '        ' Verify that all necessary criteria to call this function are met
    '        If iData.WebServiceAuthorizationCode <> WebServicePassword Then
    '            Return New ExternalTicketProviderResult(False, "Permission Denied")
    '        End If

    '        ' This would be the place to check custom fields and the person's account to see if they can 
    '        ' submit tickets.  If they can, and you use a pay-per-ticket model, deduct the ticket from
    '        ' the person's account here.

    '        ' For example, to return an error that the person is out of support tickets, use the following code
    '        ' return new ExternalTicketProviderResult(true, "There are no more tickets in your account.", "TicketCreation=FALSE");

    '#Region "Example: Set the customer's custom fields"
    '        '
    '        '			 * In this example, we set some hidden custom fields based on the logged in user's information.  These
    '        '			 * custom fields MUST exist already in SmarterTrack (without the cf_ prefix)
    '        '			 * 

    '        'if (iData.LoginUsername.ToLowerInvariant() == "myusername")
    '        '{
    '        '    result.OutputVariables.Add("cf_Billing ID=4126");
    '        '    result.OutputVariables.Add("cf_Plan Type=ASP.NET Semi-dedicated");
    '        '}
    '#End Region

    '#Region "Example: Reroute tickets"
    '        '
    '        '			 * In this example, we reroute any tickets with the subject containing Urgent to the Critical Support department.  
    '        '			 * We also reroute any tickets with a custom field value of "Server Down" to the Server Operations Department
    '        '			 * (assuming we had that custom field set up in our system).
    '        '			 * You could also modify this to reroute customers with certain attributes in your database (priority customers).
    '        '			 * 

    '        'if (iData.Subject.ToUpperInvariant().Contains("URGENT"))
    '        '{
    '        '    result.OutputVariables.Add("departmentName=Critical Support");
    '        '}
    '        'string value;
    '        'if (iData.CustomFields.TryGetValue("Issue Type", out value) && value.ToUpperInvariant() == "SERVER DOWN")
    '        '{
    '        '    result.OutputVariables.Add("departmentName=Server Operations Department");
    '        '}
    '#End Region

    '        result.Success = True
    '        result.Message = "Ticket Creation Successful"
    '        result.OutputVariables.Add("TicketCreation=TRUE")
    '        Return result
    '    End Function

#End Region

    '#Region "IExternalChatProvider Members"

    '    '  This function models the StartTicket function almost exactly.  It is provided so you can make the processes run differently
    '    '  for each.
    '    '
    '    '  Inputs
    '    '      authPW = Validates that calling software is valid
    '    '      loginUsername (for chats submitted through web interface)
    '    '      emailFrom (for chats submitted through email)
    '    '      departmentName
    '    '      cf_* = values of custom fields
    '    '  Outputs
    '    '      ChatCreation = TRUE or FALSE
    '    '      cf_* = output values of custom fields
    '    <WebMethod>
    '    Public Function StartChat(inputs As ExternalChatProviderInputs) As ExternalChatProviderResult
    '        Dim iData As New ParsedChatInputData(inputs)
    '        Dim result As New ExternalChatProviderResult()

    '        ' Verify that all necessary criteria to call this function are met
    '        If iData.WebServiceAuthorizationCode <> WebServicePassword Then
    '            Return New ExternalChatProviderResult(False, "Permission Denied")
    '        End If

    '        ' This would be the place to check custom fields and the person's account to see if they can 
    '        ' submit chats.  If they can, and you use a pay-per-chat model, deduct the chat from
    '        ' the person's account here.

    '        ' For example, to return an error that the person is out of support chats, use the following code
    '        ' return new ExternalChatProviderResult(true, "There are no more chats in your account.", "ChatCreation=FALSE");

    '        ' You can also set output custom fields, which will be filled in with the chat.

    '        '#Region "Example: Set the customer's custom fields"
    '        '
    '        '			 * In this example, we set some hidden custom fields based on the logged in user's information.  These
    '        '			 * custom fields MUST exist already in SmarterTrack (without the cf_ prefix)
    '        '			 * 

    '        'if (iData.LoginUsername.ToLowerInvariant() == "myusername"){string a = "1"}

    '        '    result.OutputVariables.Add("cf_Billing ID=4126");
    '        '    result.OutputVariables.Add("cf_Plan Type=ASP.NET Semi-dedicated");
    '        '}
    '        '#End Region

    '        result.Success = True
    '        result.Message = "Chat Creation Successful"
    '        result.OutputVariables.Add("ChatCreation=TRUE")
    '        Return result
    '    End Function

    Private Function IExternalLoginProvider_Authenticate(inputs As ExternalLoginProviderInputs) As ExternalLoginProviderResult Implements IExternalLoginProvider.Authenticate
        Throw New NotImplementedException()
    End Function

    Private Function IExternalLoginProvider_GetSignInCookieInfo(inputs As ExternalLoginProviderInputs) As ExternalLoginProviderResult Implements IExternalLoginProvider.GetSignInCookieInfo
        Throw New NotImplementedException()
    End Function

    Private Function IExternalCustomFieldProvider_GetCustomFieldOptions(inputs As ExternalCustomFieldProviderInputs) As ExternalCustomFieldProviderResult Implements IExternalCustomFieldProvider.GetCustomFieldOptions
        Throw New NotImplementedException()
    End Function

    Private Function IExternalTicketProvider_StartTicket(inputs As ExternalTicketProviderInputs) As ExternalTicketProviderResult Implements IExternalTicketProvider.StartTicket
        Throw New NotImplementedException()
    End Function

    '#End Region

End Class

