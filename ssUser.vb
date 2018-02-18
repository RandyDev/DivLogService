'Imports Microsoft.VisualBasic
'Imports System.Security.Permissions
'Imports System.Web.Security

Public Class ssUser

#Region "private variables"
    Private _ProviderUserKey As Guid
    Private _userName As String
    Private _eMail As String
    Private _CreationDate As Date
    Private _Password As String
    Private _IsApproved As Boolean
    Private _IsLockedOut As Boolean
    Private _IsOnline As Boolean
    Private _LastActivityDate As Date
    Private _LastLockoutDate As Date
    Private _LastLoginDate As Date
    Private _LastPasswordChangedDate As Date
    Private _PasswordQuestion As String
    Private _PasswordAnswer As String
    Private _Comment As String
    Private _myRoles As List(Of String)
    Private _FirstName As String
    Private _MI As String
    Private _LastName As String
    Private _Title As String
    Private _Phone As String
    Private _cellText As String
    Private _rtdsEmployeeID As Guid
#End Region

#Region "Public Properties"
    Public Property userID() As Guid
        Get
            Return _ProviderUserKey
        End Get
        Set(ByVal value As Guid)
            _ProviderUserKey = value

        End Set
    End Property
    Public Property userName() As String
        Get
            Return _userName
        End Get
        Set(ByVal value As String)
            _userName = value
        End Set
    End Property
    Public Property eMail() As String
        Get
            Return _eMail
        End Get
        Set(ByVal value As String)
            _eMail = value
        End Set
    End Property
    Public Property CreationDate() As Date
        Get
            Return _CreationDate
        End Get
        Set(ByVal value As Date)
            _CreationDate = value
        End Set
    End Property
    Public Property Password() As String
        Get
            Return _Password
        End Get
        Set(ByVal value As String)
            _Password = value
        End Set
    End Property
    Public Property IsApproved() As Boolean
        Get
            Return _IsApproved
        End Get
        Set(ByVal value As Boolean)
            _IsApproved = value
        End Set
    End Property
    Public Property IsLockedOut() As Boolean
        Get
            Return _IsLockedOut
        End Get
        Set(ByVal value As Boolean)
            _IsLockedOut = value
        End Set
    End Property
    Public Property IsOnline() As Boolean
        Get
            Return _IsOnline
        End Get
        Set(ByVal value As Boolean)
            _IsOnline = value
        End Set
    End Property
    Public Property LastActivityDate() As Date
        Get
            Return _LastActivityDate
        End Get
        Set(ByVal value As Date)
            _LastActivityDate = value
        End Set
    End Property
    Public Property LastLockoutDate() As Date
        Get
            Return _LastLockoutDate
        End Get
        Set(ByVal value As Date)
            _LastLockoutDate = value
        End Set
    End Property
    Public Property LastLoginDate() As Date
        Get
            Return _LastLoginDate
        End Get
        Set(ByVal value As Date)
            _LastLoginDate = value
        End Set
    End Property
    Public Property LastPasswordChangedDate() As Date
        Get
            Return _LastPasswordChangedDate
        End Get
        Set(ByVal value As Date)
            _LastPasswordChangedDate = value
        End Set
    End Property
    Public Property PasswordQuestion() As String
        Get
            Return _PasswordQuestion
        End Get
        Set(ByVal value As String)
            _PasswordQuestion = value
        End Set
    End Property
    Public Property PasswordAnswer() As String
        Get
            Return _PasswordAnswer
        End Get
        Set(ByVal value As String)
            _PasswordAnswer = value
        End Set
    End Property
    Public Property Comment() As String
        Get
            Return _Comment
        End Get
        Set(ByVal value As String)
            _Comment = value
        End Set
    End Property
    'Roles
    Public Property myRoles() As List(Of String)
        Get
            Return _myRoles
        End Get
        Set(ByVal value As List(Of String))
            _myRoles = value
        End Set
    End Property
    'Profile
    Public Property FirstName() As String
        Get
            Return _FirstName
        End Get
        Set(ByVal value As String)
            _FirstName = value
        End Set
    End Property
    Public Property MI() As String
        Get
            Return _MI
        End Get
        Set(ByVal value As String)
            _MI = value
        End Set
    End Property
    Public Property LastName() As String
        Get
            Return _LastName
        End Get
        Set(ByVal value As String)
            _LastName = value
        End Set
    End Property
    Public Property Title() As String
        Get
            Return _Title
        End Get
        Set(ByVal value As String)
            _Title = value
        End Set
    End Property
    Public Property Phone() As String
        Get
            Return _Phone
        End Get
        Set(ByVal value As String)
            _Phone = value
        End Set
    End Property
    Public Property cellText() As String
        Get
            Return _cellText
        End Get
        Set(ByVal value As String)
            _cellText = value
        End Set
    End Property
    Public Property rtdsEmployeeID() As Guid
        Get
            Return _rtdsEmployeeID
        End Get
        Set(ByVal value As Guid)
            _rtdsEmployeeID = value
        End Set
    End Property
#End Region

End Class
