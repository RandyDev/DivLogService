Imports System.DirectoryServices

Public Class LDAP
    Public Enum LDAPResult
        Authenticated = 0
        BadDomain
        BadUser
        BadPassword
    End Enum

    Public Enum NetworkType
        ActiveDirectory
        NovellEDirectory
    End Enum

    Private Sub New()
    End Sub

    Public Shared Function AuthenticateLogin(ByRef adr As ADrecord, domain As String, username As String, password As String, nettype As NetworkType) As LDAPResult
        Dim authtype As AuthenticationTypes = AuthenticationTypes.SecureSocketsLayer
        If nettype = NetworkType.ActiveDirectory Then
            authtype = AuthenticationTypes.Secure
        End If

        'Using dirEntry As New DirectoryEntry(Convert.ToString("LDAP://10.8.1.5"), username, password, authtype)
        Using dirEntry As New DirectoryEntry(Convert.ToString("LDAP://mail.div-log.com:636"), username, password, authtype)
                Dim searcher As DirectorySearcher = New DirectorySearcher(dirEntry, "SamAccountName=" & username)
                '            searcher.Filter = "(SamAccountName =" & username & ")"
                searcher.PropertiesToLoad.Add("givenName")    'Users first name
                searcher.PropertiesToLoad.Add("cn")
                searcher.PropertiesToLoad.Add("SAMAccountName")   'Users login name
                searcher.PropertiesToLoad.Add("mail")
                ' and adspath is retrieved too  LDAP ://mail.div-log.com:636/CN=Test User,CN=Users,DC=div-log,DC=com
                Dim userEmail As String = String.Empty
                Dim myResultPropColl As ResultPropertyCollection
                Try
                    Dim result As SearchResult = searcher.FindOne()
                    If result IsNot Nothing Then userEmail = result.Properties("mail").ToString
                    myResultPropColl = result.Properties

                Dim a As String = String.Empty
                For Each myKey In myResultPropColl.PropertyNames
                        Select Case myKey
                            Case "givenname"
                                adr.firstName = myKey
                                For Each itm In myResultPropColl(myKey)
                                    adr.firstName = itm
                                Next
                            Case "cn"
                                adr.cn = myKey
                                For Each itm In myResultPropColl(myKey)
                                    adr.cn = itm
                                Next
                            Case "adspath"
                                adr.adspath = myKey
                                For Each itm In myResultPropColl(myKey)
                                    adr.adspath = itm
                                Next
                            Case "samaccountname"
                                adr.samaccountname = myKey
                                For Each itm In myResultPropColl(myKey)
                                    adr.samaccountname = itm
                                Next
                            Case "mail"
                                adr.email = myKey
                                For Each itm In myResultPropColl(myKey)
                                    adr.email = itm
                                Next
                        End Select
                    Next

                    Return LDAPResult.Authenticated
                Catch ex As Exception
                    If IsFailueMessage(ex) Then
                        Return LDAPResult.BadPassword
                    End If
                    Return LDAPResult.BadDomain
                End Try
            End Using
    End Function

    Private Shared Function IsFailueMessage(ex As Exception) As Boolean
        Return ex.Message.IndexOf("Logon failure") <> -1 OrElse ex.Message.IndexOf("0x8007052E") <> -1
    End Function
End Class


