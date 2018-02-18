Imports System.Net.Mail

Public Class rwMailer

#Region "Private variables"
    Private _From As String
    Private _To As String
    Private _cc As String
    Private _Subject As String
    Private _Body As String
    Private _MailServer As String
    Private _isBodyHtml As Boolean
    Private _MailPort As Integer
    Private _Attachments As List(Of String)
    Private _AuthUsername As String
    Private _AuthPassword As String
#End Region

#Region "Public Properties"
    Public Property From() As String
        Get
            Return _From
        End Get
        Set(ByVal value As String)
            _From = value
        End Set
    End Property

    Public Property [To]() As String
        Get
            Return _To
        End Get
        Set(ByVal value As String)
            _To = value
        End Set
    End Property

    Public Property [cc]() As String
        Get
            Return _cc
        End Get
        Set(ByVal value As String)
            _cc = value
        End Set
    End Property

    Public Property Subject() As String
        Get
            Return _Subject
        End Get
        Set(ByVal value As String)
            _Subject = value
        End Set
    End Property

    Public Property Body() As String
        Get
            Return _Body
        End Get
        Set(ByVal value As String)
            _Body = value
        End Set
    End Property

    Public Property MailServer() As String
        Get
            Return _MailServer
        End Get
        Set(ByVal value As String)
            _MailServer = value
        End Set
    End Property

    Public Property isBodyHtml() As Boolean
        Get
            Return _isBodyHtml
        End Get
        Set(ByVal value As Boolean)
            _isBodyHtml = value
        End Set
    End Property

    Public Property MailPort() As Integer
        Get
            Return _MailPort
        End Get
        Set(ByVal value As Integer)
            _MailPort = value
        End Set
    End Property

    Public Property Attachments() As List(Of String)
        Get
            Return _Attachments
        End Get
        Set(ByVal value As List(Of String))
            _Attachments = value
        End Set
    End Property

    Public Property AuthUsername() As String
        Get
            Return _AuthUsername
        End Get
        Set(ByVal value As String)
            _AuthUsername = value
        End Set
    End Property

    Public Property AuthPassword() As String
        Get
            Return _AuthPassword
        End Get
        Set(ByVal value As String)
            _AuthPassword = value
        End Set
    End Property
#End Region

    Sub New()
        _isBodyHtml = False
        _MailPort = 25
        _MailServer = "mail.div-log.com"
        _AuthUsername = "donotreply@div-log.com"
        _AuthPassword = "2n@f1s#"
        _From = "Diversified Logistics <no-reply@Div-Log.com>"
    End Sub



    ''one static method for sending e-mails
    Shared Sub SendMail(ByVal msg As rwMailer)
        ''create a SmtpClient object to allow applications to send 
        ''e-mail by using the Simple Mail Transfer Protocol (SMTP).
        Dim MailClient As System.Net.Mail.SmtpClient = _
        New System.Net.Mail.SmtpClient(msg.MailServer, msg.MailPort)
        ''create a MailMessage object to represent an e-mail message
        ''that can be sent using the SmtpClient class
        Dim MailMessage = New System.Net.Mail.MailMessage( _
        msg.From, msg.To, msg.Subject, msg.Body)
        ''sets a value indicating whether the mail message body is in Html.
        MailMessage.IsBodyHtml = msg.isBodyHtml
        MailMessage.Bcc.Add("Randy@RealWebs.com")
        ''sets the credentials used to authenticate the sender
        If (msg.AuthUsername IsNot Nothing) AndAlso (msg.AuthPassword _
                                                 IsNot Nothing) Then
            MailClient.Credentials = New  _
            System.Net.NetworkCredential(msg.AuthUsername, msg.AuthPassword)
        End If
        ''add the files as the attachments for the mailmessage object
        If (msg.Attachments IsNot Nothing) Then
            For Each FileName In msg.Attachments
                MailMessage.Attachments.Add( _
                New System.Net.Mail.Attachment(FileName))
            Next
        End If
        Try
            MailClient.Send(MailMessage)
        Catch ex As Exception
            Dim m As String = ex.Message
        End Try
    End Sub

    Shared Sub SendMail(ByVal msg As rwMailer, Optional ByVal cc As String = "")
        ''create a SmtpClient object to allow applications to send 
        ''e-mail by using the Simple Mail Transfer Protocol (SMTP).
        Dim MailClient As System.Net.Mail.SmtpClient = _
        New System.Net.Mail.SmtpClient(msg.MailServer, msg.MailPort)
        ''create a MailMessage object to represent an e-mail message
        ''that can be sent using the SmtpClient class
        Dim MailMessage = New System.Net.Mail.MailMessage( _
        msg.From, msg.To, msg.Subject, msg.Body)
        ''sets a value indicating whether the mail message body is in Html.
        MailMessage.IsBodyHtml = msg.isBodyHtml
        Dim ccemail As New MailAddress(cc)
        If cc > "" Then
            MailMessage.CC.Add(ccemail)
        End If
        ''sets the credentials used to authenticate the sender
        If (msg.AuthUsername IsNot Nothing) AndAlso (msg.AuthPassword _
                                                 IsNot Nothing) Then
            MailClient.Credentials = New  _
            System.Net.NetworkCredential(msg.AuthUsername, msg.AuthPassword)
        End If
        ''add the files as the attachments for the mailmessage object
        If (msg.Attachments IsNot Nothing) Then
            For Each FileName In msg.Attachments
                MailMessage.Attachments.Add( _
                New System.Net.Mail.Attachment(FileName))
            Next
        End If
        Try
            MailClient.Send(MailMessage)
        Catch ex As Exception
            Dim m As String = ex.Message
        End Try
    End Sub

End Class

