Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic


Public Class UnloaderByID
    Private _LogDate As Date
    Private _DoorNumber As String
    Private _empID As String
    Private _empName As String
    Private _ulCount As Integer
    Private _Amount As Double
    Private _LoadNumber As Integer
    Private _PurchaseOrder As String
    Private _AmountEachUnloader As Double

    Public Property LogDate() As Date
        Get
            Return _LogDate
        End Get
        Set(ByVal value As Date)
            _LogDate = value
        End Set
    End Property

    Public Property DoorNumber() As String
        Get
            Return _DoorNumber
        End Get
        Set(ByVal value As String)
            _DoorNumber = value
        End Set
    End Property

    Public Property empID() As String
        Get
            Return _empID
        End Get
        Set(ByVal value As String)
            _empID = value
        End Set
    End Property

    Public Property empName() As String
        Get
            Return _empName
        End Get
        Set(ByVal value As String)
            _empName = value
        End Set
    End Property

    Public Property ulCount() As Integer
        Get
            Return _ulCount
        End Get
        Set(ByVal value As Integer)
            _ulCount = value
        End Set
    End Property

    Public Property Amount() As Double
        Get
            Return _Amount
        End Get
        Set(ByVal value As Double)
            _Amount = value
        End Set
    End Property

    Public Property AmountEachUnloader() As Double
        Get
            Return _AmountEachUnloader
        End Get
        Set(ByVal value As Double)
            _AmountEachUnloader = value
        End Set
    End Property

    Public Property LoadNumber() As Integer
        Get
            Return _LoadNumber
        End Get
        Set(ByVal value As Integer)
            _LoadNumber = value
        End Set
    End Property

    Public Property PurchaseOrder() As String
        Get
            Return _PurchaseOrder
        End Get
        Set(ByVal value As String)
            _PurchaseOrder = value
        End Set
    End Property


End Class

Public Class UnloaderByIdDAL


    Public Function UnloaderByIDandDateRange(ByVal empID As Guid, ByVal sDate As Date, ByVal eDate As Date) As List(Of UnloaderByID)
        Dim ulList As New List(Of UnloaderByID)
        Dim dt As DataTable = New DataTable()
        Dim sql As String = "SELECT WO.LogDate, WO.DoorNumber, E.Login AS empID, E.FirstName + ' ' + E.LastName AS empName,  " & _
        "(SELECT COUNT(EmployeeID) AS empID " & _
        "FROM Unloader AS Un " & _
        "WHERE (LoadID = WO.ID)) AS ulCount, WO.Amount, WO.LoadNumber, WO.PurchaseOrder  " & _
        "FROM Employee AS E INNER JOIN  " & _
        "Unloader AS U ON E.ID = U.EmployeeID LEFT OUTER JOIN  " & _
        "WorkOrder AS WO ON U.LoadID = WO.ID  " & _
        "WHERE (U.EmployeeID = @empID) AND (WO.LogDate >= @startDate) AND (WO.LogDate <= @endDate)  " & _
        "ORDER BY WO.LogDate, WO.DoorNumber  "
        Dim adapter As New SqlDataAdapter(sql, ConfigurationManager.ConnectionStrings("rtdsConnectionString").ConnectionString)
        Dim param1 As New SqlParameter("empID", empID)
        Dim param2 As New SqlParameter("startDate", sDate)
        Dim param3 As New SqlParameter("endDate", eDate)
        adapter.SelectCommand.Parameters.Add(param1)
        adapter.SelectCommand.Parameters.Add(param2)
        adapter.SelectCommand.Parameters.Add(param3)
        adapter.Fill(dt)
        For Each rw As DataRow In dt.Rows
            Dim nul As New UnloaderByID
            nul.LogDate = rw.Item("LogDate")
            nul.DoorNumber = rw.Item("DoorNumber")
            nul.empID = rw.Item("empID")
            nul.empName = rw.Item("empName")
            nul.ulCount = rw.Item("ulCount")
            nul.Amount = rw.Item("Amount")
            nul.LoadNumber = rw.Item("LoadNumber")
            nul.PurchaseOrder = rw.Item("PurchaseOrder")
            nul.AmountEachUnloader = nul.Amount / nul.ulCount
            ulList.Add(nul)
        Next

        Return ulList
    End Function

End Class
