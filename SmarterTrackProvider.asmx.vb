
'Imports System.Web.Services
'Imports System.ComponentModel
'Imports System.Collections.Generic
'Imports SmarterTrack.Connector

'Namespace dlWebService1
'    <WebService([Namespace]:="http://tempuri.org/")>
'    <WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)>
'    <ToolboxItem(False)>
'    Public Class dlSmarterProvider
'        Inherits System.Web.Services.WebService
'        Implements IExternalLoginProvider, IExternalCustomFieldProvider, IExternalTicketProvider
'        ' Change this to secure your web service.  Also change in SmarterTrack interface.
'        Private Const WebServicePassword As String = "@ct$2016p"

'#Region "Example External Login Provider"

'        '  This function is used to authenticate user credentials when they login to the interface.  The return
'        '  values are used to create and link an account from SmarterTrack into your system.
'        '
'        '  Inputs
'        '      authPW = Validates that calling software is valid
'        '      loginUsername
'        '      loginPassword
'        '  Outputs
'        '      Authentication = OK or FAIL
'        '      EmailAddress = Email address of user
'        '      DisplayName = Display Name of User
'        <WebMethod>
'        Public Function Authenticate(inputs As ExternalLoginProviderInputs) As ExternalLoginProviderResult
'            Dim iData As New ParsedLoginInputData(inputs)
'            Dim result As ExternalLoginProviderResult

'            ' Verify that all necessary criteria to call this function are met
'            If iData.WebServiceAuthorizationCode <> WebServicePassword Then
'                Return New ExternalLoginProviderResult(False, "Permission Denied")
'            End If
'            If String.IsNullOrEmpty(iData.LoginUsername) OrElse String.IsNullOrEmpty(iData.LoginPassword) Then
'                Return New ExternalLoginProviderResult(False, "Required Input Missing")
'            End If

'            ' This is a sample of how to do a basic username/password check (use your own database or list here)
'            If iData.LoginUsername.ToLowerInvariant() = "myusername" AndAlso iData.LoginPassword.ToLowerInvariant() = "mypassword" Then
'                result = New ExternalLoginProviderResult()
'                result.Success = True
'                result.Message = "Login Successful"
'                result.OutputVariables.Add("Authentication=OK")
'                result.OutputVariables.Add("EmailAddress=putUsersEmailAddressHere@example.com")
'                ' optional
'                result.OutputVariables.Add("DisplayName=putUsersFullNameHere")
'                ' optional
'                Return result
'            End If

'            ' Below is a sample Active Directory Authentication implementation
'            'string[] unDomainUsernameSplit = loginUsername.Split(new char[] { '/', '\\' }, 2);
'            'if (unDomainUsernameSplit.Length == 2)
'            '{
'            '    if (LDAP.AuthenticateLogin(unDomainUsernameSplit[0], unDomainUsernameSplit[1],
'            '        loginPassword, LDAP.NetworkType.ActiveDirectory) == LDAP.LDAPResult.Authenticated)
'            '    {
'            '        return new ExternalLoginProviderResult(true, "Login Successful", "Authentication=OK");
'            '    }
'            '}

'            ' Return a failure if there's no match
'            Return New ExternalLoginProviderResult(True, "Login Failure", "Authentication=FAIL")
'        End Function

'        '  Same as Authenticate function, but does not verify password because cookie is authenticated.  Therefore, your
'        '  logic will be identical to the Authenticate function, but does not require you to check the password.
'        '
'        '  Inputs
'        '      authPW = Validates that calling software is valid
'        '      loginUsername
'        '  Outputs
'        '      Authentication = OK or FAIL
'        '      EmailAddress = Email address of user
'        '      DisplayName = Display Name of User
'        <WebMethod>
'        Public Function GetSignInCookieInfo(inputs As ExternalLoginProviderInputs) As ExternalLoginProviderResult
'            Dim iData As New ParsedLoginInputData(inputs)

'            ' Verify that all necessary criteria to call this function are met
'            If iData.WebServiceAuthorizationCode <> WebServicePassword Then
'                Return New ExternalLoginProviderResult(False, "Permission Denied")
'            End If
'            If String.IsNullOrEmpty(iData.LoginUsername) Then
'                Return New ExternalLoginProviderResult(False, "Required Input Missing")
'            End If

'            ' This is a sample of how to do a basic username/password check (use your own database or list here)
'            If iData.LoginUsername.ToLowerInvariant() = "myusername" Then
'                Dim result As New ExternalLoginProviderResult()
'                result.Success = True
'                result.Message = "Login Successful"
'                result.OutputVariables.Add("Authentication=OK")
'                result.OutputVariables.Add("EmailAddress=putUsersEmailAddressHere@example.com")
'                ' optional
'                result.OutputVariables.Add("DisplayName=putUsersFullNameHere")
'                ' optional
'                Return result
'            End If

'            ' Below is a sample Active Directory Authentication implementation
'            'string[] unDomainUsernameSplit = loginUsername.Split(new char[] { '/', '\\' }, 2);
'            'if (unDomainUsernameSplit.Length == 2)
'            '{
'            '    if (LDAP.AuthenticateLogin(unDomainUsernameSplit[0], unDomainUsernameSplit[1],
'            '        "", LDAP.NetworkType.ActiveDirectory) == LDAP.LDAPResult.Authenticated)
'            '    {
'            '        return new ExternalLoginProviderResult(true, "Login Successful", "Authentication=OK");
'            '    }
'            '}

'            Return New ExternalLoginProviderResult(True, "Login Failure", "Authentication=FAIL")
'        End Function

'#End Region

'#Region "Example External Custom Field Provider"

'        '  A lot can be done with the custom field provider.  It is called at two steps during the ticket and chat creation process.
'        '  This example function shows several potential samples that can be uncommented and altered as needed.
'        '
'        '  When the person begins the ticket or chat process, this function is called with location=PRE.  This gives you a chance to
'        '  customize the custom fields that show up for the person.  You may want to show a list of their domains or products, for
'        '  example.
'        '
'        '  When a person finishes the pre-chat or pre-ticket step (the step where the search fields appear), this function is called 
'        '  again with location=REGULAR.  This gives you a chance to fill out the data values of custom fields.  You may want to show
'        '  lists based on the selection in the PRE step.  This is NOT the time to store custom fields for the user.  This should
'        '  happen with the StartTicket or StartChat functions
'        '
'        '  Inputs
'        '      authPW = Validates that calling software is valid
'        '      loginUsername
'        '      departmentName
'        '      location = PRE or REGULAR (pre called after department selection made.  Regular called after search page and before compose page)
'        '      cf_* = value of current custom fields, such as "cf_ticket type=phone ticket"
'        '  Outputs
'        '      CanStartTickets = true or false
'        '      CanStartChats = true or false
'        '		Custom field overrides
'        <WebMethod>
'        Public Function GetCustomFieldOptions(inputs As ExternalCustomFieldProviderInputs) As ExternalCustomFieldProviderResult
'            Dim iData As New ParsedCustomFieldInputData(inputs)

'            ' Verify that all necessary criteria to call this function are met
'            If iData.WebServiceAuthorizationCode <> WebServicePassword Then
'                Return New ExternalCustomFieldProviderResult(False, "Permission Denied")
'            End If

'            Dim canStartTickets As Boolean = True
'            Dim canStartChats As Boolean = True
'            Dim result As New ExternalCustomFieldProviderResult(True, "Result okay")
'            Dim cfdata As ExternalCustomFieldData

'            '#Region "Example: Allow tickets/chats to only certain customers"
'            '
'            '			 * You could check a database and see if the user has the ability to contact certain departments, for example.
'            '			 * In this example, only the user "myusername" is allowed to start tickets and chats for all departments.  
'            '			 * Everyone else can only start chats (not tickets), and can only contact the sales department
'            '			 

'            'if (iData.LoginUsername == "myusername")
'            '{
'            '    canStartTickets = true;
'            '    canStartChats = true;
'            '}
'            'else
'            '{
'            '    result.Message = "You are not currently permitted to contact that department."; // shown in ticket pages
'            '    canStartTickets = false;
'            '    if (iData.DepartmentName.ToUpperInvariant() == "SALES DEPARTMENT")
'            '        canStartChats = true;
'            '    else
'            '        canStartChats = false;
'            '}
'            '#End Region

'            '#Region "Example: Set a custom list of domains for them to choose from"
'            '
'            '			 * This example shows how to fill custom field drop-down lists for the user based on their login.  In this case,
'            '			 * we fill a dropdown list with their accounts listed in our database, so that they can choose which one 
'            '			 * the support is for.
'            '			 * 
'            '			 * It is important that the custom field already exist in SmarterTrack
'            '			 * 

'            'if (iData.DisplayLocation == DisplayLocations.Page1_Pre && iData.DepartmentName.ToUpperInvariant() == "SUPPORT DEPARTMENT")
'            '{
'            '    // Here you would look up the options.  For now, we'll assume we brought back abc.com and def.com from our database
'            '    cfdata = new ExternalCustomFieldData();
'            '    cfdata.CustomFieldName = "Account";
'            '    cfdata.ChangeRequired = true;
'            '    cfdata.NewRequiredValue = true;
'            '    cfdata.ChangeListOptions = true;
'            '    cfdata.DisplayWithInformationLookup = false; // Set this to true if you need to show/hide other custom fields based on the value they select.
'            '    cfdata.ListOptions.Add("abc.com");
'            '    cfdata.ListOptions.Add("def.com");
'            '    result.CustomFieldDataItems.Add(cfdata);
'            '}
'            '#End Region

'            '#Region "Example: Custom Issue Types by Department"
'            '
'            '			 * This example shows how to fill custom field drop-down lists for the user based on the department chosen.  In this case,
'            '			 * we fill a dropdown list with possible types of issues
'            '			 * 
'            '			 * It is important that the custom field already exist in SmarterTrack
'            '			 * 

'            'cfdata = new ExternalCustomFieldData();
'            'cfdata.CustomFieldName = "Type of Issue";
'            'cfdata.ChangeListOptions = true;
'            'cfdata.DisplayWithInformationLookup = false;
'            'cfdata.ChangeRequired = true;
'            'cfdata.NewRequiredValue = true;
'            'cfdata.ChangeVisible = true;
'            'cfdata.NewVisibleValue = true;
'            'if (iData.DepartmentName.ToUpperInvariant().IndexOf("SALES") != -1)
'            '{
'            '    cfdata.ListOptions.Add("Change Plan");
'            '    cfdata.ListOptions.Add("Help with order");
'            '    cfdata.ListOptions.Add("Questions about plans");
'            '    cfdata.ListOptions.Add("Questions about features");
'            '    cfdata.ListOptions.Add("Help finding a product");
'            '}
'            'else if (iData.DepartmentName.ToUpperInvariant().IndexOf("SUPPORT") != -1)
'            '{
'            '    cfdata.ListOptions.Add("Web site issues");
'            '    cfdata.ListOptions.Add("Database issues");
'            '    cfdata.ListOptions.Add("Email issues");
'            '    cfdata.ListOptions.Add("Dedicated server issues");
'            '    cfdata.ListOptions.Add("Statistics issues");
'            '    cfdata.ListOptions.Add("Network connectivity");
'            '}
'            'else
'            '{
'            '    // Removing issue types field, since there's no options
'            '    cfdata.ChangeRequired = true;
'            '    cfdata.NewRequiredValue = false;
'            '    cfdata.ChangeVisible = true;
'            '    cfdata.NewVisibleValue = false;
'            '}
'            'result.CustomFieldDataItems.Add(cfdata);
'            '#End Region

'            result.Success = True
'            result.OutputVariables.Add("CanStartTickets=" + canStartTickets)
'            result.OutputVariables.Add("CanStartChats=" + canStartChats)
'            Return result
'        End Function

'#End Region

'#Region "IExternalTicketProvider Members"

'        '  This function is called right after the person clicks on the Submit Ticket button.  This gives your provider one last
'        '  chance to abort the process or store any custom fields.
'        '
'        '  Inputs
'        '      authPW = Validates that calling software is valid
'        '      loginUsername (for tickets submitted through web interface)
'        '      emailFrom (for tickets submitted through email)
'        '      departmentName
'        '      subject
'        '      ticketNumber = the ticket number about to be created
'        '      cf_* = values of custom fields
'        '  Outputs
'        '      TicketCreation = TRUE or FALSE
'        '      cf_* = output values of custom fields
'        <WebMethod>
'        Public Function StartTicket(inputs As ExternalTicketProviderInputs) As ExternalTicketProviderResult
'            Dim iData As New ParsedTicketInputData(inputs)
'            Dim result As New ExternalTicketProviderResult()

'            ' Verify that all necessary criteria to call this function are met
'            If iData.WebServiceAuthorizationCode <> WebServicePassword Then
'                Return New ExternalTicketProviderResult(False, "Permission Denied")
'            End If

'            ' This would be the place to check custom fields and the person's account to see if they can 
'            ' submit tickets.  If they can, and you use a pay-per-ticket model, deduct the ticket from
'            ' the person's account here.

'            ' For example, to return an error that the person is out of support tickets, use the following code
'            ' return new ExternalTicketProviderResult(true, "There are no more tickets in your account.", "TicketCreation=FALSE");

'#Region "Example: Set the customer's custom fields"
'            '
'            '			 * In this example, we set some hidden custom fields based on the logged in user's information.  These
'            '			 * custom fields MUST exist already in SmarterTrack (without the cf_ prefix)
'            '			 * 

'            'if (iData.LoginUsername.ToLowerInvariant() == "myusername")
'            '{
'            '    result.OutputVariables.Add("cf_Billing ID=4126");
'            '    result.OutputVariables.Add("cf_Plan Type=ASP.NET Semi-dedicated");
'            '}
'#End Region

'#Region "Example: Reroute tickets"
'            '
'            '			 * In this example, we reroute any tickets with the subject containing Urgent to the Critical Support department.  
'            '			 * We also reroute any tickets with a custom field value of "Server Down" to the Server Operations Department
'            '			 * (assuming we had that custom field set up in our system).
'            '			 * You could also modify this to reroute customers with certain attributes in your database (priority customers).
'            '			 * 

'            'if (iData.Subject.ToUpperInvariant().Contains("URGENT"))
'            '{
'            '    result.OutputVariables.Add("departmentName=Critical Support");
'            '}
'            'string value;
'            'if (iData.CustomFields.TryGetValue("Issue Type", out value) && value.ToUpperInvariant() == "SERVER DOWN")
'            '{
'            '    result.OutputVariables.Add("departmentName=Server Operations Department");
'            '}
'#End Region

'            result.Success = True
'            result.Message = "Ticket Creation Successful"
'            result.OutputVariables.Add("TicketCreation=TRUE")
'            Return result
'        End Function

'#End Region

'#Region "IExternalChatProvider Members"

'        '  This function models the StartTicket function almost exactly.  It is provided so you can make the processes run differently
'        '  for each.
'        '
'        '  Inputs
'        '      authPW = Validates that calling software is valid
'        '      loginUsername (for chats submitted through web interface)
'        '      emailFrom (for chats submitted through email)
'        '      departmentName
'        '      cf_* = values of custom fields
'        '  Outputs
'        '      ChatCreation = TRUE or FALSE
'        '      cf_* = output values of custom fields
'        <WebMethod>
'        Public Function StartChat(inputs As ExternalChatProviderInputs) As ExternalChatProviderResult
'            Dim iData As New ParsedChatInputData(inputs)
'            Dim result As New ExternalChatProviderResult()

'            ' Verify that all necessary criteria to call this function are met
'            If iData.WebServiceAuthorizationCode <> WebServicePassword Then
'                Return New ExternalChatProviderResult(False, "Permission Denied")
'            End If

'            ' This would be the place to check custom fields and the person's account to see if they can 
'            ' submit chats.  If they can, and you use a pay-per-chat model, deduct the chat from
'            ' the person's account here.

'            ' For example, to return an error that the person is out of support chats, use the following code
'            ' return new ExternalChatProviderResult(true, "There are no more chats in your account.", "ChatCreation=FALSE");

'            ' You can also set output custom fields, which will be filled in with the chat.

'            '#Region "Example: Set the customer's custom fields"
'            '
'            '			 * In this example, we set some hidden custom fields based on the logged in user's information.  These
'            '			 * custom fields MUST exist already in SmarterTrack (without the cf_ prefix)
'            '			 * 

'            'if (iData.LoginUsername.ToLowerInvariant() == "myusername")
'            '{
'            '    result.OutputVariables.Add("cf_Billing ID=4126");
'            '    result.OutputVariables.Add("cf_Plan Type=ASP.NET Semi-dedicated");
'            '}
'            '#End Region

'            result.Success = True
'            result.Message = "Chat Creation Successful"
'            result.OutputVariables.Add("ChatCreation=TRUE")
'            Return result
'        End Function

'#End Region
'    End Class
'End Namespace


