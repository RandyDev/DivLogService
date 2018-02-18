Imports System.Data
'Imports DiversifiedLogistics.ssUser
'Imports System.Web
'Imports DiversifiedLogistics.Utilities


Public Class userDAL

#Region "Populate one user to an ssUser class"
    Public Function getUserByID(ByVal uid As Guid) As ssUser
        Dim muser As MembershipUser = Membership.GetUser(uid)
        Return getUserByName(muser.UserName)
    End Function

    Public Function getUserByName(ByVal userName As String) As ssUser
        Dim usr As New ssUser()
        Dim usrInfo As MembershipUser = Membership.GetUser(userName)
        If Not usrInfo Is Nothing Then
            usr.userID = usrInfo.ProviderUserKey
            usr.userName = usrInfo.UserName
            usr.eMail = usrInfo.Email
            '            usr.Password = usrInfo.GetPassword()
            usr.CreationDate = usrInfo.CreationDate
            usr.IsApproved = usrInfo.IsApproved
            usr.IsLockedOut = usrInfo.IsLockedOut
            usr.IsOnline = usrInfo.IsOnline
            usr.LastActivityDate = usrInfo.LastActivityDate
            usr.LastLockoutDate = usrInfo.LastLockoutDate
            usr.LastLoginDate = usrInfo.LastLoginDate
            usr.LastPasswordChangedDate = usrInfo.LastPasswordChangedDate
            usr.PasswordQuestion = usrInfo.PasswordQuestion
            usr.PasswordAnswer = getAns(usr.userID)
            usr.Comment = usrInfo.Comment
            getRoleList(usr)
            getProfile(usr)
        End If
        Return usr
    End Function

    Private Sub getRoleList(ByRef usr As ssUser)
        Dim rlst As New List(Of String)
        Dim lst() As String = Roles.GetRolesForUser(usr.userName)
        Dim i As Integer = 0
        For i = 0 To lst.Length - 1
            rlst.Add(lst(i))
        Next
        usr.myRoles = rlst
    End Sub
    Private Sub getProfile(ByRef usr As ssUser)
        Dim dba As New DBAccess()
        dba.CommandText = "SELECT firstName, mi, lastName, title, phone, cellText, rtdsEmployeeID FROM UserProfile WHERE userID =  '" & usr.userID.ToString & "'"
        Dim dt As DataTable = dba.ExecuteDataSet.Tables(0)
        If dt.Rows.Count > 0 Then
            Try

                For Each row As DataRow In dt.Rows
                    If Not IsDBNull(row.Item("firstName")) Then usr.FirstName = row.Item("firstName")
                    If Not IsDBNull(row.Item("mi")) Then usr.MI = row.Item("mi")
                    If Not IsDBNull(row.Item("lastName")) Then usr.LastName = row.Item("lastName")
                    If Not IsDBNull(row.Item("title")) Then usr.Title = row.Item("title")
                    If Not IsDBNull(row.Item("phone")) Then usr.Phone = row.Item("phone")
                    If Not IsDBNull(row.Item("cellText")) Then usr.cellText = row.Item("cellText")
                    If Not IsDBNull(row.Item("rtdsEmployeeID")) Then usr.rtdsEmployeeID = row.Item("rtdsEmployeeID")
                Next
            Catch ex As Exception
                Dim msg As String = ex.Message

            End Try
            ' **********************************************************************************************
            ' **************** delete section and people table when everyone iz moved **********************
            ' **********************************************************************************************
            'pick up old people table
            'If usr.FirstName Is Nothing Then
            '    dba.CommandText = "SELECT firstName, mi, lastName, title, phone FROM people WHERE pid =  '" & usr.userID.ToString & "'"
            '    dt = dba.ExecuteDataSet.Tables(0)
            '    If dt.Rows.Count > 0 Then
            '        For Each row As DataRow In dt.Rows
            '            If Not IsDBNull(row.Item("firstName")) Then usr.FirstName = row.Item("firstName")
            '            If Not IsDBNull(row.Item("mi")) Then usr.MI = row.Item("mi")
            '            If Not IsDBNull(row.Item("lastName")) Then usr.LastName = row.Item("lastName")
            '            If Not IsDBNull(row.Item("title")) Then usr.Title = row.Item("title")
            '            If Not IsDBNull(row.Item("phone")) Then usr.Phone = row.Item("phone")
            '        Next
            '        ' if people info there, update the userProfile table
            '        dba.CommandText = "UPDATE UserProfile SET firstName=@firstName, mi=@mi, lastName=@lastName, title=@title, phone=@phone WHERE userID=@userID"
            '        dba.AddParameter("@userID", usr.userID)
            '        dba.AddParameter("@firstName", usr.FirstName)
            '        dba.AddParameter("@mi", usr.MI)
            '        dba.AddParameter("@lastName", usr.LastName)
            '        dba.AddParameter("@title", usr.Title)
            '        dba.AddParameter("@phone", usr.Phone)
            '        Dim err As String = Nothing
            '        Try
            '            dba.ExecuteNonQuery()
            '        Catch ex As Exception
            '            err = ex.Message
            '        End Try
            '        If Not err Is Nothing Then
            '            'and delete the people table entry
            '            dba.CommandText = "DELETE FROM people WHERE pid=@pid"
            '            dba.AddParameter("@pid", usr.userID)
            '            Try
            '                dba.ExecuteNonQuery()
            '            Catch ex As Exception
            '                err = "Delete People table entry failed. Reason " & ex.Message
            '            End Try
            '        End If
            '    End If
            'End If
            ' **********************************************************************************************
            ' **********************************************************************************************

        Else
            'no profile
        End If
    End Sub
#End Region

#Region "Get Methods"
    Public Function getUsers() As List(Of ssUser)
        Dim userLst As New List(Of ssUser)
        Dim dba As New DBAccess()
        dba.CommandText = "SELECT Distinct aspnet_Users.UserName, UserProfile.FirstName, UserProfile.LastName " & _
            "FROM aspnet_Users INNER JOIN " & _
            "UserProfile ON aspnet_Users.UserId = UserProfile.userID INNER JOIN  " & _
            "aspnet_UsersInRoles ON aspnet_Users.UserId = aspnet_UsersInRoles.UserId INNER JOIN  " & _
            "aspnet_Roles ON aspnet_UsersInRoles.RoleId = aspnet_Roles.RoleId  " & _
            "WHERE ((aspnet_Roles.RoleName <> 'Client') AND (aspnet_Roles.RoleName <> 'Carrier')AND (aspnet_Roles.RoleName <> 'Vendor'))   " & _
            "ORDER BY UserProfile.LastName "
        Dim dt As DataTable = dba.ExecuteDataSet.Tables(0)
        If dt.Rows.Count > 0 Then
            Dim tuser As ssUser = Nothing
            For Each rw As DataRow In dt.Rows
                Try

                    tuser = getUserByName(rw.Item("UserName"))
                Catch ex As Exception
                    Dim msg As String = ex.Message
                End Try
                userLst.Add(tuser)
            Next
        End If
        Dim a As String = ""
        Return userLst
    End Function

    Public Function getClientServicesUsers() As List(Of ssUser)
        Dim userLst As New List(Of ssUser)
        Dim dba As New DBAccess()
        dba.CommandText = "SELECT Distinct aspnet_Users.UserName, UserProfile.FirstName, UserProfile.LastName " & _
            "FROM aspnet_Users INNER JOIN " & _
            "UserProfile ON aspnet_Users.UserId = UserProfile.userID INNER JOIN  " & _
            "aspnet_UsersInRoles ON aspnet_Users.UserId = aspnet_UsersInRoles.UserId INNER JOIN  " & _
            "aspnet_Roles ON aspnet_UsersInRoles.RoleId = aspnet_Roles.RoleId  " & _
            "WHERE (aspnet_Roles.RoleName = @roleName0)  OR (aspnet_Roles.RoleName = @roleName1)  OR (aspnet_Roles.RoleName = @roleName2) OR (aspnet_Roles.RoleName = @roleName3)" & _
            "ORDER BY UserProfile.LastName "
        dba.AddParameter("@roleName0", "Client")
        dba.AddParameter("@roleName1", "Vendor")
        dba.AddParameter("@roleName2", "Carrier")
        dba.AddParameter("@roleName3", "Guest")
        Dim dt As DataTable = dba.ExecuteDataSet.Tables(0)
        If dt.Rows.Count > 0 Then
            For Each rw As DataRow In dt.Rows
                Dim tuser As ssUser = getUserByName(rw.Item("UserName"))
                userLst.Add(tuser)
            Next
        End If
        Return userLst


    End Function


    Public Function getUsersByRole(ByVal role As String) As List(Of ssUser)
        Dim userLst As New List(Of ssUser)
        Dim dba As New DBAccess()
        dba.CommandText = "SELECT UserProfile.FirstName, UserProfile.LastName, aspnet_Users.UserName, aspnet_Roles.RoleName " & _
            "FROM aspnet_Users INNER JOIN " & _
            "aspnet_UsersInRoles ON aspnet_Users.UserId = aspnet_UsersInRoles.UserId INNER JOIN " & _
            "UserProfile ON aspnet_Users.UserId = UserProfile.userID INNER JOIN " & _
            "aspnet_Roles ON aspnet_UsersInRoles.RoleId = aspnet_Roles.RoleId " & _
            "WHERE (aspnet_Roles.RoleName = @rolename)  " & _
            "order by lastname"
        dba.AddParameter("@roleName", role.Trim)
        Dim dt As DataTable = dba.ExecuteDataSet.Tables(0)
        If dt.Rows.Count > 0 Then
            For Each rw As DataRow In dt.Rows
                Dim tuser As ssUser = getUserByName(rw.Item("UserName"))
                userLst.Add(tuser)
            Next
        End If
        Return userLst
    End Function
    Public Function getSEUusers() As List(Of ssUser)
        Dim userLst As New List(Of ssUser)
        Return userLst
    End Function
    Public Function getSEUHourlyUsers() As List(Of ssUser)
        Dim userLst As New List(Of ssUser)
        Return userLst
    End Function
    Public Function getSEUAdminUsers() As List(Of ssUser)
        Dim userLst As New List(Of ssUser)
        Return userLst
    End Function
    Public Function getPublicUsers() As List(Of ssUser)
        Dim userLst As New List(Of ssUser)
        Return userLst
    End Function
    Public Function getClientUsers() As List(Of ssUser)
        Dim userLst As New List(Of ssUser)
        Return userLst
    End Function
    Public Function getCarrierUsers() As List(Of ssUser)
        Dim userLst As New List(Of ssUser)
        Return userLst
    End Function
    Public Function getVendorUsers() As List(Of ssUser)
        Dim userLst As New List(Of ssUser)
        Return userLst
    End Function
    Public Function getNoRoleUsers() As List(Of ssUser)
        Dim userLst As New List(Of ssUser)
        Return userLst
    End Function
    Private Function getAns(ByVal usrID As Guid) As String
        Dim strResponse As String = Nothing
        Dim dba As New DBAccess()
        dba.CommandText = "Select PasswordAnswer FROM aspnet_Membership WHERE UserID = @UserID"
        dba.AddParameter("@UserID", usrID)
        Try
            strResponse = dba.ExecuteScalar

        Catch ex As Exception

        End Try
        Return strResponse
    End Function

    'get list of Employees
    'Public Function getAgents() As List(Of ssUser)
    '    Dim userLst As New List(Of ssUser)
    '    Dim allUsers As MembershipUserCollection = Membership.GetAllUsers()
    '    Dim user As MembershipUser
    '    For Each user In allUsers
    '        Dim userRoles() As String = Roles.GetRolesForUser(user.UserName)
    '        Dim isCandidate = (userRoles.Length = 1) And (userRoles(0) = "Candidate")
    '        If Not isCandidate Then
    '            Dim tuser As ssUser = getUserByName(user.UserName)
    '            userLst.Add(tuser)
    '        End If
    '    Next
    '    Return userLst
    'End Function
#End Region
#Region "Membership, Role and Profile CRUD"

    Public Function addUser(ByVal usr As ssUser) As String
        Dim createStatus As MembershipCreateStatus
        Dim strResponse As String = Nothing
        Try
            Dim isApproved As Boolean = usr.IsApproved
            Dim newUser As MembershipUser
            newUser = Membership.CreateUser(usr.userName, usr.Password, usr.eMail, "na", "na", isApproved, createStatus)
            Select Case createStatus
                Case MembershipCreateStatus.Success
                    strResponse = "The user account was successfully created!"
                Case MembershipCreateStatus.DuplicateUserName
                    strResponse = "That username already exists."
                    Return strResponse
                Case MembershipCreateStatus.DuplicateEmail
                    strResponse = "A user with that Email address already exists."
                    Return strResponse
                Case MembershipCreateStatus.InvalidEmail
                    strResponse = "PLease enter a VALID email address."
                    Return strResponse
                Case MembershipCreateStatus.InvalidPassword
                    strResponse = "Invalid Password"
                    Return strResponse
                Case Else
                    strResponse = "LoginID or eMail address already in use."
                    Return strResponse
            End Select
        Catch ex As MembershipCreateUserException
            strResponse = ex.StatusCode
            Return strResponse
        End Try

        'set profile
        If usr.MI Is Nothing Then usr.MI = ""
        If usr.Phone Is Nothing Then usr.Phone = ""
        If usr.cellText Is Nothing Then usr.cellText = ""
        If usr.Title Is Nothing Then usr.Title = ""
        Dim puser As MembershipUser = Membership.GetUser(usr.userName)
        Dim dba As New DBAccess()
        dba.CommandText = "INSERT INTO UserProfile (userID,FirstName,Mi,LastName,Title,Phone,cellText,rtdsEmployeeID) VALUES (@userID, @FirstName,@Mi, @LastName, @Title, @Phone, @cellText, @rtdsEmployeeID)"
        dba.AddParameter("@userID", puser.ProviderUserKey)
        dba.AddParameter("@FirstName", usr.FirstName)
        dba.AddParameter("@Mi", usr.MI)
        dba.AddParameter("@LastName", usr.LastName)
        dba.AddParameter("@Title", usr.Title)
        dba.AddParameter("@Phone", usr.Phone)
        dba.AddParameter("@cellText", usr.cellText)
        dba.AddParameter("@rtdsEmployeeID", usr.rtdsEmployeeID)
        dba.ExecuteNonQuery()
        'set roles
        For Each str As String In usr.myRoles
            Roles.AddUserToRole(usr.userName, str)
        Next

        Return strResponse  ' anything OTHER than "The user account was successfully created!" is a failure!
    End Function

    Public Function UpdateEmail(ByVal userID As Guid, ByVal email As String) As String
        Dim strResponse As String = String.Empty
        Dim udal As New userDAL
        'retrieve existing usr informaiton
        Dim exuser As ssUser = udal.getUserByID(userID)
        If email <> exuser.eMail Then 'we need to change email address in membership svcs
            Dim dba As New DBAccess
            dba.CommandText = "SELECT COUNT(Email) FROM aspnet_Membership WHERE LoweredEmail = @email"
            dba.AddParameter("@email", email.ToLower)
            Dim dupe As Integer = dba.ExecuteScalar
            If dupe > 0 Then    'already exist
                strResponse = "eMail Address already in use"
            Else
                Dim mUser As MembershipUser = Membership.GetUser(userID)
                mUser.Email = email
                Try
                    Membership.UpdateUser(mUser)
                Catch ex As Exception
                    strResponse = ex.Message
                End Try
            End If
        End If
        Return strResponse
    End Function

    Public Function UpdatePhone(ByVal userID As Guid, ByVal phone As String) As String
        Dim strResponse As String = String.Empty
        Dim dba As New DBAccess("uszipcode")
        dba.CommandText = "UPDATE UserProfile SET Phone=@Phone WHERE userID=@userID"
        dba.AddParameter("@Phone", phone)
        dba.AddParameter("@userID", userID)
        Try
            dba.ExecuteNonQuery()
        Catch ex As Exception
            strResponse = ex.Message
        End Try
        Return strResponse
    End Function

    Public Function UpdateUser(ByVal usr As ssUser) As String
        Dim strResponse As String = String.Empty
        Dim udal As New userDAL
        'retrieve existing usr information
        Dim exuser As ssUser = udal.getUserByName(usr.userName)
        Dim mUser As MembershipUser = Membership.GetUser(usr.userName)
        ' compare submitted (usr) with existing (exuser)
        If usr.eMail <> exuser.eMail Then 'we need to change email address in membership svcs
            mUser.Email = usr.eMail
            Try
                Membership.UpdateUser(mUser)
            Catch ex As Exception
                '                strResponse = ex.Message & ": eMail Address already in use"
                strResponse = "eMail Address already in use"
                Return strResponse
            End Try
        End If

        mUser.IsApproved = usr.IsApproved
        mUser.Comment = Nothing '<-- SPACE AVAILABLE
        Try
            Membership.UpdateUser(mUser)
        Catch ex As Exception
            strResponse = ex.Message & ": sApproved, Comment update failed"
        End Try

        'check password

        updateRoles(usr)

        strResponse &= updateProfile(usr)


        Return strResponse

    End Function

    Public Function updateProfile(ByVal usr As ssUser) As String
        Dim strResponse As String = Nothing
        Dim dbresp As Integer
        Dim dba As New DBAccess()
        dba.CommandText = "SELECT COUNT(userID) FROM UserProfile where userID = '" & usr.userID.ToString & "'"
        Dim int As Integer = dba.ExecuteScalar
        If int = 1 Then
            dba.CommandText = "UPDATE UserProfile SET FirstName=@FirstName, LastName=@LastName, Title=@Title, Phone=@Phone, cellText=@cellText, rtdsEmployeeID=@rtdsEmployeeID WHERE userID = @userID"
        Else
            dba.CommandText = "INSERT INTO UserProfile (userID,FirstName, LastName, Title, Phone, cellText, rtdsEmployeeID) " & _
                "VALUES (@userID, @FirstName, @LastName, @Title, @Phone, @cellText, @rtdsEmployeeID)"
        End If
        dba.AddParameter("@firstName", usr.FirstName)
        dba.AddParameter("@lastName", usr.LastName)
        dba.AddParameter("@Title", usr.Title)
        dba.AddParameter("@Phone", usr.Phone)
        dba.AddParameter("@cellText", usr.cellText)
        dba.AddParameter("@rtdsEmployeeID", usr.rtdsEmployeeID)
        dba.AddParameter("@userID", usr.userID)
        Try
            dbresp = dba.ExecuteNonQuery
        Catch ex As Exception
            strResponse = ex.Message
        End Try
        If dbresp < 1 Then strResponse &= "UserProfile error"
        Return strResponse
    End Function

    Public Sub updateRoles(ByVal usr As ssUser)
        ' update Roles
        Dim roleList() As String = Roles.GetRolesForUser(usr.userName)
        'clear all roles
        For Each str As String In roleList
            Roles.RemoveUserFromRole(usr.userName, str)
        Next
        'add new rols
        For Each str As String In usr.myRoles
            Roles.AddUserToRole(usr.userName, str)
        Next
    End Sub

    Public Function deleteUser(ByVal usrName As String) As String
        Dim strResponse As String = String.Empty
        Dim muser As MembershipUser = Membership.GetUser(usrName)
        Dim dba As New DBAccess()
        dba.CommandText = "DELETE FROM UserProfile WHERE userID = '" & muser.ProviderUserKey.ToString & "'"
        Try
            Dim i As Integer = dba.ExecuteNonQuery
            If i = 0 Then strResponse = "Unable to delete profile"
        Catch ex As Exception
            strResponse &= ex.Message
        End Try
        If strResponse.Length < 1 Then
            Dim rolelst() As String = Roles.GetRolesForUser(usrName)
            For Each rl As String In rolelst
                Roles.RemoveUserFromRole(usrName, rl)
            Next
            Dim memberGone As Boolean = Membership.DeleteUser(usrName)
            If Not memberGone Then strResponse &= "Unable to delete Membership User"
        End If
        dba.CommandText = "DELETE FROM UserLocations WHERE userID=@userID"
        dba.AddParameter("@userID", muser.ProviderUserKey)
        dba.ExecuteNonQuery()
        Return strResponse  'if it's not empty, there was a problem
    End Function

    Public Function checkDupeUserName(ByVal usrName As String) As String
        Dim strResponse As String = String.Empty
        Dim counter As Integer = 0
        Dim dba As New DBAccess()
        dba.CommandText = "SELECT COUNT(LoweredUserName) FROM aspnet_Users WHERE LoweredUserName=@LoweredUserName"
        dba.AddParameter("@LoweredUserName", usrName.Trim().ToLower())
        counter = dba.ExecuteScalar
        If counter > 0 Then
            strResponse = "LoginID / EmpID already in use"
        Else
            dba.CommandText = "SELECT COUNT(Login) FROM Employee WHERE Login=@Login"
            dba.AddParameter("@Login", usrName.Trim())
            counter = dba.ExecuteScalar
            If counter > 0 Then
                strResponse = "EmpID / LoginID already in use."
            End If
        End If
        Return strResponse
    End Function

    Public Function checkDupeEmail(ByVal usrEmail As String) As String
        Dim strResponse As String = String.Empty
        Dim counter As Integer = 0
        Dim dba As New DBAccess()
        dba.CommandText = "SELECT COUNT(Email) FROM aspnet_Membership WHERE LoweredEmail = @LoweredEmail"
        dba.AddParameter("@LoweredEmail", usrEmail.Trim().ToLower())
        counter = dba.ExecuteScalar
        If counter > 0 Then
            strResponse = "eMail address already in use."
        End If
        Return strResponse
    End Function


    Public Function UpdateSecurityQuestion(ByVal usr As ssUser) As String
        Dim strResponse As String = Nothing
        Dim dba As New DBAccess()
        dba.CommandText = "Update aspnet_Membership set PasswordQuestion=@PasswordQuestion, PasswordAnswer=@PasswordAnswer WHERE UserID=@UserID"
        dba.AddParameter("@PasswordQuestion", usr.PasswordQuestion)
        dba.AddParameter("@PasswordAnswer", usr.PasswordAnswer)
        dba.AddParameter("@UserID", usr.userID)
        dba.ExecuteNonQuery()
        Return strResponse
    End Function

    Public Function ResetPasswordandSecurity(ByVal usrid As String) As String
        Dim strResponse As String = String.Empty

        Dim usr As MembershipUser = Membership.GetUser(New Guid(usrid))
        Dim tempPW As String = usr.ResetPassword()
        strResponse = usr.UserName & "welcome"
        Dim ok As Boolean = usr.ChangePassword(tempPW, strResponse)
        Dim dba As New DBAccess()
        dba.CommandText = "Update aspnet_Membership SET LastPasswordChangedDate = CreateDate, PasswordQuestion='na',PasswordAnswer='na' WHERE userID = @userID"
        dba.AddParameter("@userID", usrid)
        Dim suc As Integer = dba.ExecuteNonQuery
        If suc = 0 Then strResponse = "ooophf"
        Return strResponse
    End Function

    'Public Function UpdatecUser(ByVal usr As ssUser) As Integer
    '    Dim udal As New userDAL
    '    Dim exuser As ssUser = udal.getUserByName(usr.userName)
    '    Dim mUser As MembershipUser = Membership.GetUser(usr.userName)
    '    'check password
    '    If Len(usr.Password) > 5 And usr.Password <> exuser.Password Then
    '        '            Dim resetPwd As String = mUser.ResetPassword()
    '        '            mUser.ChangePassword(resetPwd, usr.Password)
    '    End If
    '    ' update Profile
    '    Dim dba As New DBAccess()
    '    dba.CommandText = "SELECT COUNT(userID) FROM UserProfile where userID = '" & exuser.userID.ToString & "'"
    '    Dim int As Integer = dba.ExecuteScalar
    '    If int = 1 Then
    '        dba.CommandText = "UPDATE UserProfile SET FirstName=@FirstName, LastName=@LastName, Title=@Title, Phone=@Phone, rtdsEmployeeID=@rtdsEmployeeID WHERE userID = '" & exuser.userID.ToString & "'"
    '        dba.AddParameter("@firstName", usr.FirstName)
    '        dba.AddParameter("@lastName", usr.LastName)
    '        dba.AddParameter("@Title", usr.Title)
    '        dba.AddParameter("@Phone", usr.Phone)
    '        dba.AddParameter("@rtdsEmployeeID", usr.rtdsEmployeeID)
    '        Dim suc As Integer = dba.ExecuteScalar
    '    End If

    '    Return 0
    'End Function

#End Region

End Class
