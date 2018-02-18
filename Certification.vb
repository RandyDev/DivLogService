'Imports System.Data.SqlClient

Public Class Certification
#Region "Private Variables"
    Private _EmployeeID As Guid
    Private _TypeID As Guid
    Private _Date As Date
    Private _ID As Guid
    Private _certName As String
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

    Public Property TypeID() As Guid
        Get
            Return _TypeID
        End Get
        Set(ByVal value As Guid)
            _TypeID = value
        End Set
    End Property

    Public Property certDate() As Date
        Get
            Return _Date
        End Get
        Set(ByVal value As Date)
            _Date = value
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

    Public Property certName() As String
        Get
            Return _certName
        End Get
        Set(ByVal value As String)
            _certName = value
        End Set
    End Property

#End Region





End Class
