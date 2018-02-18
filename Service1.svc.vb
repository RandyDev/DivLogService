'Imports SmarterTrack.Connector
'Imports System
'Imports System.Text
'Imports System.Collections
'Imports System.DirectoryServices
'Imports System.ComponentModel
'Imports System.Web.Services


'<ToolboxItem(False)>
'<WebService(Namespace:="http://tempuri.div-log.com/")>
'<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)>
'    Public Class DivLogSmarterTrackProvider
'        Inherits WebService
'        Implements IExternalLoginProvider, IExternalCustomFieldProvider, IExternalTicketProvider
'        'todo add this password to smartertrack
'        Private Const WebServicePassword As String = "@ct$2016p"

'#Region "Example External Login Provide"





'#End Region
'    Public Sub New()
'            MyBase.New()
'        End Sub

'    <WebMethod>
'    Public Function Authenticate(inputs As ExternalLoginProviderInputs) As ExternalLoginProviderResult
'        Dim iData As New ParsedLoginInputData(inputs)
'        Dim result As ExternalLoginProviderResult

'        ' Verify that all necessary criteria to call this function are met
'        If iData.WebServiceAuthorizationCode <> WebServicePassword Then
'            Return New ExternalLoginProviderResult(False, "Permission Denied")
'        End If
'        If String.IsNullOrEmpty(iData.LoginUsername) OrElse String.IsNullOrEmpty(iData.LoginPassword) Then
'            Return New ExternalLoginProviderResult(False, "Required Input Missing")
'        End If

'        ' This is a sample of how to do a basic username/password check (use your own database or list here)
'        If iData.LoginUsername.ToLowerInvariant() = "myusername" AndAlso iData.LoginPassword.ToLowerInvariant() = "mypassword" Then
'            result = New ExternalLoginProviderResult()
'            result.Success = True
'            result.Message = "Login Successful"
'            result.OutputVariables.Add("Authentication=OK")
'            result.OutputVariables.Add("EmailAddress=putUsersEmailAddressHere@example.com")
'            ' optional
'            result.OutputVariables.Add("DisplayName=putUsersFullNameHere")
'            ' optional
'            Return result
'        End If

'        ' Below is a sample Active Directory Authentication implementation
'        'string[] unDomainUsernameSplit = loginUsername.Split(new char[] { '/', '\\' }, 2);
'        'if (unDomainUsernameSplit.Length == 2)
'        '{
'        '    if (LDAP.AuthenticateLogin(unDomainUsernameSplit[0], unDomainUsernameSplit[1],
'        '        loginPassword, LDAP.NetworkType.ActiveDirectory) == LDAP.LDAPResult.Authenticated)
'        '    {
'        '        return new ExternalLoginProviderResult(true, "Login Successful", "Authentication=OK");
'        '    }
'        '}

'        ' Return a failure if there's no match
'        Return New ExternalLoginProviderResult(True, "Login Failure", "Authentication=FAIL")
'    End Function

'    <WebMethod>
'        Public Function GetCustomFieldOptions(ByVal inputs As ExternalCustomFieldProviderInputs) As SmarterTrack.Connector.ExternalCustomFieldProviderResult Implements IExternalCustomFieldProvider.GetCustomFieldOptions
'            Dim externalCustomFieldProviderResult As SmarterTrack.Connector.ExternalCustomFieldProviderResult
'            'todo
'            If ((New ParsedCustomFieldInputData(inputs)).WebServiceAuthorizationCode = "password") Then
'                Dim canStartTickets As Boolean = True
'                Dim canStartChats As Boolean = True
'                Dim result As SmarterTrack.Connector.ExternalCustomFieldProviderResult = New SmarterTrack.Connector.ExternalCustomFieldProviderResult(True, "Result okay", New String(-1) {}) With
'                {
'                    .Success = True
'                }
'                result.OutputVariables.Add(String.Concat("CanStartTickets=", canStartTickets.ToString()))
'                result.OutputVariables.Add(String.Concat("CanStartChats=", canStartChats.ToString()))
'                externalCustomFieldProviderResult = result
'            Else
'                externalCustomFieldProviderResult = New SmarterTrack.Connector.ExternalCustomFieldProviderResult(False, "Permission Denied", New String(-1) {})
'            End If
'            Return externalCustomFieldProviderResult
'        End Function

'    <WebMethod>
'    Public Function GetSignInCookieInfo(inputs As ExternalLoginProviderInputs) As ExternalLoginProviderResult
'        Dim iData As New ParsedLoginInputData(inputs)

'        ' Verify that all necessary criteria to call this function are met
'        If iData.WebServiceAuthorizationCode <> WebServicePassword Then
'            Return New ExternalLoginProviderResult(False, "Permission Denied")
'        End If
'        If String.IsNullOrEmpty(iData.LoginUsername) Then
'            Return New ExternalLoginProviderResult(False, "Required Input Missing")
'        End If

'        ' This is a sample of how to do a basic username/password check (use your own database or list here)
'        If iData.LoginUsername.ToLowerInvariant() = "myusername" Then
'            Dim result As New ExternalLoginProviderResult()
'            result.Success = True
'            result.Message = "Login Successful"
'            result.OutputVariables.Add("Authentication=OK")
'            result.OutputVariables.Add("EmailAddress=putUsersEmailAddressHere@example.com")
'            ' optional
'            result.OutputVariables.Add("DisplayName=putUsersFullNameHere")
'            ' optional
'            Return result
'        End If

'        ' Below is a sample Active Directory Authentication implementation
'        'string[] unDomainUsernameSplit = loginUsername.Split(new char[] { '/', '\\' }, 2);
'        'if (unDomainUsernameSplit.Length == 2)
'        '{
'        '    if (LDAP.AuthenticateLogin(unDomainUsernameSplit[0], unDomainUsernameSplit[1],
'        '        "", LDAP.NetworkType.ActiveDirectory) == LDAP.LDAPResult.Authenticated)
'        '    {
'        '        return new ExternalLoginProviderResult(true, "Login Successful", "Authentication=OK");
'        '    }
'        '}

'        Return New ExternalLoginProviderResult(True, "Login Failure", "Authentication=FAIL")
'    End Function

'    <WebMethod>
'        Public Function StartChat(ByVal inputs As ExternalChatProviderInputs) As SmarterTrack.Connector.ExternalChatProviderResult
'            Dim externalChatProviderResult As SmarterTrack.Connector.ExternalChatProviderResult
'            Dim iData As ParsedChatInputData = New ParsedChatInputData(inputs)
'            Dim result As SmarterTrack.Connector.ExternalChatProviderResult = New SmarterTrack.Connector.ExternalChatProviderResult()
'            'todo
'            If (iData.WebServiceAuthorizationCode = "password") Then
'                result.Success = True
'                result.Message = "Chat Creation Successful"
'                result.OutputVariables.Add("ChatCreation=TRUE")
'                externalChatProviderResult = result
'            Else
'                externalChatProviderResult = New SmarterTrack.Connector.ExternalChatProviderResult(False, "Permission Denied", New String(-1) {})
'            End If
'            Return externalChatProviderResult
'        End Function
'#Region "IExternalTicketProvider Members"

'    '  This function is called right after the person clicks on the Submit Ticket button.  This gives your provider one last
'    '  chance to abort the process or store any custom fields.
'    '
'    '  Inputs
'    '      authPW = Validates that calling software is valid
'    '      loginUsername (for tickets submitted through web interface)
'    '      emailFrom (for tickets submitted through email)
'    '      departmentName
'    '      subject
'    '      ticketNumber = the ticket number about to be created
'    '      cf_* = values of custom fields
'    '  Outputs
'    '      TicketCreation = TRUE or FALSE
'    '      cf_* = output values of custom fields
'    <WebMethod>
'    Public Function StartTicket(inputs As ExternalTicketProviderInputs) As ExternalTicketProviderResult
'        Dim iData As New ParsedTicketInputData(inputs)
'        Dim result As New ExternalTicketProviderResult()

'        ' Verify that all necessary criteria to call this function are met
'        If iData.WebServiceAuthorizationCode <> WebServicePassword Then
'            Return New ExternalTicketProviderResult(False, "Permission Denied")
'        End If

'        ' This would be the place to check custom fields and the person's account to see if they can 
'        ' submit tickets.  If they can, and you use a pay-per-ticket model, deduct the ticket from
'        ' the person's account here.

'        ' For example, to return an error that the person is out of support tickets, use the following code
'        ' return new ExternalTicketProviderResult(true, "There are no more tickets in your account.", "TicketCreation=FALSE");

'        '#Region "Example: Set the customer's custom fields"
'        '
'        '			 * In this example, we set some hidden custom fields based on the logged in user's information.  These
'        '			 * custom fields MUST exist already in SmarterTrack (without the cf_ prefix)
'        '			 * 

'        'if (iData.LoginUsername.ToLowerInvariant() == "myusername")
'        '{
'        '    result.OutputVariables.Add("cf_Billing ID=4126");
'        '    result.OutputVariables.Add("cf_Plan Type=ASP.NET Semi-dedicated");
'        '}
'#End Region

'#Region " Reroute tickets"
'        '
'        '			 * In this example, we reroute any tickets with the subject containing Urgent to the Critical Support department.  
'        '			 * We also reroute any tickets with a custom field value of "Server Down" to the Server Operations Department
'        '			 * (assuming we had that custom field set up in our system).
'        '			 * You could also modify this to reroute customers with certain attributes in your database (priority customers).
'        '			 * 

'        'if (iData.Subject.ToUpperInvariant().Contains("URGENT"))
'        '{
'        '    result.OutputVariables.Add("departmentName=Critical Support");
'        '}
'        'string value;
'        'if (iData.CustomFields.TryGetValue("Issue Type", out value) && value.ToUpperInvariant() == "SERVER DOWN")
'        '{
'        '    result.OutputVariables.Add("departmentName=Server Operations Department");
'        '}
'#End Region

'        result.Success = True
'        result.Message = "Ticket Creation Successful"
'        result.OutputVariables.Add("TicketCreation=TRUE")
'        Return result
'    End Function
'End Class
