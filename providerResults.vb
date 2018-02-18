#Region "Assembly SmarterTrack.Connector, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
' C:\Projects\DivLogService\DivLogService\bin\SmarterTrack.Connector.dll
#End Region

Imports System.Collections.Generic

Namespace SmarterTrack.Connector
    Public Class ExternalLoginProviderResult
        Public Success As Boolean
        Public Message As String
        Public OutputVariables As List(Of String)

        Public Sub New()
        End Sub
        Public Sub New(success As Boolean, msg As String, ParamArray outputs() As String)
        End Sub
    End Class
    Public Class ExternalChatProviderResult
        Public Success As Boolean
        Public Message As String
        Public OutputVariables As List(Of String)

        Public Sub New()
        End Sub
        Public Sub New(success As Boolean, msg As String, ParamArray outputs() As String)
        End Sub
    End Class
    Public Class ExternalTicketProviderResult
        Public Success As Boolean
        Public Message As String
        Public OutputVariables As List(Of String)

        Public Sub New()
        End Sub
        Public Sub New(success As Boolean, msg As String, ParamArray outputs() As String)
        End Sub
    End Class
End Namespace
