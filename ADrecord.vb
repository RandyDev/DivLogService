Public Class ADrecord
    Private _firstName As String
    Private _samaccountname As String
    Private _email As String
    Private _adspath As String
    Private _cn As String


    Public Property firstName() As String
        Get
            Return _firstName
        End Get
        Set(value As String)
            _firstName = value
        End Set
    End Property
    Public Property samaccountname() As String
        Get
            Return _samaccountname
        End Get
        Set(value As String)
            _samaccountname = value
        End Set
    End Property
    Public Property email() As String
        Get
            Return _email
        End Get
        Set(value As String)
            _email = value
        End Set
    End Property
    Public Property adspath() As String
        Get
            Return _adspath
        End Get
        Set(value As String)
            _adspath = value
        End Set
    End Property
    Public Property cn() As String
        Get
            Return _cn
        End Get
        Set(value As String)
            _cn = value
        End Set
    End Property


End Class
