Imports System
Imports System.Text
Imports System.Collections
Imports System.DirectoryServices

Public Class LdapAuthentication
    Dim _path As String
    Dim _filterAttribute As String
    Public Sub New()
        'debug string
        _path = "LDAP://mail.div-log.com:636/" '"LDAP://10.8.1.5/CN=Users;DC=Div-Log,DC=com"
        ' PRODUCTION STRING
        'todo _path = "LDAP://10.8.1.5/"
    End Sub
    'Public Function IsAuthenticated(ByVal domain As String, ByVal username As String, ByVal pwd As String) As Boolean ' SearchResult
    '    Dim domainAndUsername As String = domain & "\" & username
    '    Dim dirEntry As DirectoryEntry = New DirectoryEntry(_path & "DC=div-log,DC=com", username, pwd)
    '    Dim sResult As SearchResult
    '    'dirEntry.Path = _path ' ""
    '    'dirEntry.Username = "div-log\z_LDAP"
    '    'dirEntry.Password = "@ct$2016p"
    '    Try
    '        'Bind to the native AdsObject to force authentication.
    '        Dim obj As Object = dirEntry.NativeObject
    '        Dim search As DirectorySearcher = New DirectorySearcher(dirEntry)
    '        search.Filter = "(SAMAccountName=" & username & ")"
    '        search.PropertiesToLoad.Add("cn")
    '        sResult = search.FindOne()
    '        If (sResult Is Nothing) Then
    '            sResult = Nothing
    '        End If
    '        'update the new path to the user in the directory
    '        _path = sResult.Path
    '        _filterAttribute = CType(sResult.Properties("cn")(0), String)
    '        Return True
    '    Catch ex As Exception

    '        Return False '
    '        sResult = Nothing
    '    End Try
    '    Return sResult
    'End Function

    Public Function GetGroups() As String
        Dim search As DirectorySearcher = New DirectorySearcher(_path)
        search.Filter = "(cn=" & _filterAttribute & ")"
        search.PropertiesToLoad.Add("memberOf")
        Dim groupNames As StringBuilder = New StringBuilder
        Try
            Dim result As SearchResult = search.FindOne()
            Dim propertyCount As Integer = result.Properties("memberOf").Count
            Dim dn As String
            Dim equalsIndex, commaIndex
            Dim propertyCounter As Integer
            For propertyCounter = 0 To propertyCount - 1
                dn = CType(result.Properties("memberOf")(propertyCounter), String)
                equalsIndex = dn.IndexOf("=", 1)
                commaIndex = dn.IndexOf(",", 1)
                If (equalsIndex = -1) Then
                    Return Nothing
                End If
                groupNames.Append(dn.Substring((equalsIndex + 1), (commaIndex - equalsIndex) - 1))
                groupNames.Append("|")
            Next
        Catch ex As Exception
            Throw New Exception("Error obtaining group names. " & ex.Message)
        End Try
        Return groupNames.ToString()
    End Function

End Class

