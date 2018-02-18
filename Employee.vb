'Imports Microsoft.VisualBasic
'Imports System.Security.Permissions
'Imports System.Web.Security

Public Class Employee
#Region "Private Variables"
    Private _rtdsFirstName As String
    Private _rtdsLastName As String
    Private _Comments As String
    Private _Certification
    Private _empCertifications As List(Of Certification)
    Private _PhotoJpegData As Byte()
    Private _LocationID As Guid
    Private _Login As String
    Private _rtdsPassword As String
    Private _Locked As Boolean
    Private _AccessRightsMask As Integer
    Private _ID As Guid
    Private _rowguid As Guid
    Private _Employment As Employment
    Private _ssUser As ssUser
    Private _LocationName As String
    Private _HomeLocationID As Guid
    Private _PayrollEmpNum As Integer
    Private _TermDate As Date


#End Region

#Region "Public Properties"
    Public Property rtdsFirstName() As String
        Get
            Return _rtdsFirstName
        End Get
        Set(ByVal value As String)
            _rtdsFirstName = value
        End Set
    End Property

    Public Property rtdsLastName() As String
        Get
            Return _rtdsLastName
        End Get
        Set(ByVal value As String)
            _rtdsLastName = value
        End Set
    End Property

    Public Property Comments() As String
        Get
            Return _Comments
        End Get
        Set(ByVal value As String)
            _Comments = value
        End Set
    End Property

    Public Property Certification() As String
        Get
            Return _Certification
        End Get
        Set(ByVal value As String)
            _Certification = value
        End Set
    End Property

    Public Property empCertifications() As List(Of Certification)
        Get
            Return _empCertifications
        End Get
        Set(ByVal value As List(Of Certification))
            _empCertifications = value
        End Set
    End Property

    Property PhotoJpegData() As Byte()
        Get
            Return _PhotoJpegData
        End Get
        Set(ByVal value As Byte())
            _PhotoJpegData = value
        End Set
    End Property

    Public Property LocationID() As Guid
        Get
            Return _LocationID
        End Get
        Set(ByVal value As Guid)
            _LocationID = value
        End Set
    End Property

    Public Property LocationName() As String
        Get
            Return _LocationName
        End Get
        Set(ByVal value As String)
            _LocationName = value
        End Set
    End Property

    Public Property HomeLocationID() As Guid
        Get
            Return _HomeLocationID
        End Get
        Set(ByVal value As Guid)
            _HomeLocationID = value
        End Set
    End Property

    Public Property rowguid() As Guid
        Get
            Return _rowguid
        End Get
        Set(ByVal value As Guid)
            _rowguid = value
        End Set
    End Property

    Public Property Login() As String
        Get
            Return _Login
        End Get
        Set(ByVal value As String)
            _Login = value
        End Set
    End Property

    Public Property rtdsPassword() As String
        Get
            Return _rtdsPassword
        End Get
        Set(ByVal value As String)
            _rtdsPassword = value
        End Set
    End Property

    Public Property Locked() As Boolean
        Get
            Return _Locked
        End Get
        Set(ByVal value As Boolean)
            _Locked = value
        End Set
    End Property

    Public Property AccessRightsMask() As Integer
        Get
            Return _AccessRightsMask
        End Get
        Set(ByVal value As Integer)
            _AccessRightsMask = value
        End Set
    End Property

    Public Property ID() As Guid
        Get
            Return _ID
        End Get
        Set(ByVal value As Guid)
            _ID = value
        End Set
    End Property

    Public Property Employment() As Employment
        Get
            Return _Employment
        End Get
        Set(ByVal value As Employment)
            _Employment = value
        End Set
    End Property

    Public Property ssUser() As ssUser
        Get
            Return _ssUser
        End Get
        Set(ByVal value As ssUser)
            _ssUser = value
        End Set
    End Property

    Public Property PayrollEmpNum() As Integer
        Get
            Return _PayrollEmpNum
        End Get
        Set(ByVal value As Integer)
            _PayrollEmpNum = value
        End Set
    End Property

    Public Property TermDate() As Date
        Get
            Return _TermDate
        End Get
        Set(ByVal value As Date)
            _TermDate = value
        End Set
    End Property

#End Region

End Class

Public Class Unloader
#Region "Private Variables"
    Private _EmployeeID As Guid
    Private _EmployeeName As String
    Private _EmployeeLogin As String
#End Region
#Region "Public Properties"
    Public Property EmployeeID() As Guid
        Get
            Return _EmployeeID
        End Get
        Set(ByVal value As Guid)
            _EmployeeID = value
        End Set
    End Property
    Public Property EmployeeName() As String
        Get
            Return _EmployeeName
        End Get
        Set(ByVal value As String)
            _EmployeeName = value
        End Set
    End Property
    Public Property EmployeeLogin() As String
        Get
            Return _EmployeeLogin
        End Get
        Set(ByVal value As String)
            _EmployeeLogin = value
        End Set
    End Property

#End Region

End Class

