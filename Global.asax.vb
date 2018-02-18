Imports System.Web.SessionState
Imports System.Web.Security
Imports System.Security.Principal


Public Class Global_asax
    Inherits System.Web.HttpApplication

    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the application is started
    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the session is started
    End Sub

    Sub Application_BeginRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires at the beginning of each request
    End Sub

    Sub Application_AuthenticateRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires upon attempting to authenticate the use
        Dim cookieName As String = FormsAuthentication.FormsCookieName
        Dim authCookie As HttpCookie = Context.Request.Cookies(cookieName)
        If (authCookie Is Nothing) Then
            'There is no authentication cookie.
            Return
        End If
        Dim authTicket As FormsAuthenticationTicket = Nothing
        Try
            authTicket = FormsAuthentication.Decrypt(authCookie.Value)
        Catch ex As Exception
            'Write the exception to the Event log.
            Return
        End Try
        If (authTicket Is Nothing) Then
            'Cookie failed to decrypt.
            Return
        End If
        'When the ticket was created, the UserData property was assigned a pipe-delimited string of group names.
        Dim groups As String() = authTicket.UserData.Split(New Char() {"|"})
        'Create an Identity
        Dim id As GenericIdentity = New GenericIdentity(authTicket.Name, "LdapAuthentication")
        'This principle flows throughout the request
        Dim principle As GenericPrincipal = New GenericPrincipal(id, groups)
        Context.User = principle
    End Sub

    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when an error occurs
    End Sub

    Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the session ends
    End Sub

    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the application ends
    End Sub

End Class