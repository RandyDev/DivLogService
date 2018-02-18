Public Class Employment

#Region "Private Variables"
    Private _EmployeeID As Guid
    Private _DateOfEmployment As Date
    Private _DateOfDismiss As Date
    Private _JobTitle As String
    Private _PayType As Integer
    Private _PayRateHourly As Double
    Private _PayRatePercentage As Double
    Private _SpecialPay As Double
    Private _HolidayPay As Double
    Private _SalaryPay As Double
    Private _ID As Guid
#End Region

#Region "Public Properties"
    Public Property EmployeeID() As Guid
        Get
            Return _EmployeeID
        End Get
        Set(ByVal value As Guid)
            _EmployeeID = value
        End Set
    End Property

    Public Property DateOfEmployment() As Date
        Get
            Return _DateOfEmployment
        End Get
        Set(ByVal value As Date)
            _DateOfEmployment = value
        End Set
    End Property

    Public Property DateOfDismiss() As Date
        Get
            Return _DateOfDismiss
        End Get
        Set(ByVal value As Date)
            _DateOfDismiss = value
        End Set
    End Property

    Public Property JobTitle() As String
        Get
            Return _JobTitle
        End Get
        Set(ByVal value As String)
            _JobTitle = value
        End Set
    End Property

    Public Property PayType() As Integer
        Get
            Return _PayType
        End Get
        Set(ByVal value As Integer)
            _PayType = value
        End Set
    End Property

    Public Property PayRateHourly() As Double
        Get
            Return _PayRateHourly
        End Get
        Set(ByVal value As Double)
            _PayRateHourly = value
        End Set
    End Property

    Public Property PayRatePercentage() As Double
        Get
            Return _PayRatePercentage
        End Get
        Set(ByVal value As Double)
            _PayRatePercentage = value
        End Set
    End Property

    Public Property SpecialPay() As Double
        Get
            Return _SpecialPay
        End Get
        Set(ByVal value As Double)
            _SpecialPay = value
        End Set
    End Property

    Public Property HolidayPay() As Double
        Get
            Return _HolidayPay
        End Get
        Set(ByVal value As Double)
            _HolidayPay = value
        End Set
    End Property

    Public Property SalaryPay() As Double
        Get
            Return _SalaryPay
        End Get
        Set(ByVal value As Double)
            _SalaryPay = value
        End Set
    End Property

    Public Property ID() As Guid
        Get
            Return _ID
        End Get
        Set(ByVal value As Guid)
            _ID = value
        End Set
    End Property

#End Region

End Class
