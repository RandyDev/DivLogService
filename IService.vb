'Public Class Service
'Implements IService
'Public Function GetCustomers() As CustomerData Implements IService.GetCustomers
'    Dim constr As String = ConfigurationManager.ConnectionStrings("constr").ConnectionString
'    Using con As New SqlConnection(constr)
'        Using cmd As New SqlCommand("SELECT CustomerId, Name, Country FROM Customers")
'            Using sda As New SqlDataAdapter()
'                cmd.Connection = con
'                sda.SelectCommand = cmd
'                Using dt As New DataTable()
'                    Dim customers As New CustomerData()
'                    sda.Fill(customers.CustomersTable)
'                    Return customers
'                End Using
'            End Using
'        End Using
'    End Using
'End Function
'End Class

<ServiceContract()>
Public Interface IService
    <OperationContract()>
    Function GetCustomers() As CustomerData
End Interface

<DataContract()>
Public Class CustomerData
    Public Sub New()
        Me.CustomersTable = New DataTable("CustomersData")
    End Sub
    <DataMember()>
    Public Property CustomersTable() As DataTable
        Get
            Return m_CustomersTable
        End Get
        Set(value As DataTable)
            m_CustomersTable = value
        End Set
    End Property
    Private m_CustomersTable As DataTable
End Class