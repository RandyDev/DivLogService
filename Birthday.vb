Public Class Birthday
    Private _First As String
    Private _Last As String

    Public Property First() As String
        Get
            Return _First
        End Get
        Set(ByVal value As String)
            _First = value
        End Set
    End Property

    Public Property Last() As String
        Get
            Return _Last
        End Get
        Set(ByVal value As String)
            _Last = value
        End Set
    End Property
End Class
