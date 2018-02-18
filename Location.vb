
Public Class Location
#Region "Private Variables"
    Private _ParentCompanyID As Guid
    Private _Name As String
    Private _ID As Guid
    Private _BeginDayOffset As Integer  'negative offset for when workday begins (relative to midnight)
    Private _TimezoneOffset As Integer  'offset GMT
    Private _InActive As Boolean        '0=we work here now  ...  1=we don't work here anymore
    Private _hhPrintTimeStamp As Boolean        '0=we work here now  ...  1=we don't work here anymore
    Private _ParentCompanyName As String
    Private _locaCity As String
    Private _locaState As String
    Private _locazip As String
    Private _CheckCharge As Decimal
    Private _Dividend As Decimal
    Private _loginPrefix As String
    Private _Prices As List(Of PriceList)
    Private _EnableSickLeave As Boolean        '0=we work here now  ...  1=we don't work here anymore
    Private _AdministrativeFee As Decimal
    Private _CustomerFee As Decimal
#End Region

#Region "Public Properties"
    Public Property ParentCompanyID() As Guid
        Get
            Return _ParentCompanyID
        End Get
        Set(ByVal value As Guid)
            _ParentCompanyID = value
        End Set
    End Property

    Public Property Name() As String
        Get
            Return _Name
        End Get
        Set(ByVal value As String)
            _Name = value
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

    Public Property BeginDayOffset() As Integer
        Get
            Return _BeginDayOffset
        End Get
        Set(ByVal value As Integer)
            _BeginDayOffset = value
        End Set
    End Property

    Public Property TimezoneOffset() As Integer
        Get
            Return _TimezoneOffset
        End Get
        Set(ByVal value As Integer)
            _TimezoneOffset = value
        End Set
    End Property

    Public Property InActive() As Boolean
        Get
            Return _InActive
        End Get
        Set(ByVal value As Boolean)
            _InActive = value
        End Set
    End Property

    Public Property hhPrintTimeStamp() As Boolean
        Get
            Return _hhPrintTimeStamp
        End Get
        Set(ByVal value As Boolean)
            _hhPrintTimeStamp = value
        End Set
    End Property

    Public Property ParentCompanyName() As String
        Get
            Return _ParentCompanyName
        End Get
        Set(ByVal value As String)
            _ParentCompanyName = value
        End Set
    End Property

    Public Property CheckCharge() As Decimal
        Get
            Return _CheckCharge
        End Get
        Set(ByVal value As Decimal)
            _CheckCharge = value
        End Set
    End Property

    Public Property Dividend() As Decimal
        Get
            Return _Dividend
        End Get
        Set(ByVal value As Decimal)
            _Dividend = value
        End Set
    End Property

    Public Property Prices() As List(Of PriceList)
        Get
            Return _Prices
        End Get
        Set(ByVal value As List(Of PriceList))
            _Prices = value
        End Set
    End Property

    Public Property locaCity() As String
        Get
            Return _locaCity
        End Get
        Set(ByVal value As String)
            _locaCity = value
        End Set
    End Property

    Public Property locaState() As String
        Get
            Return _locaState
        End Get
        Set(ByVal value As String)
            _locaState = value
        End Set
    End Property

    Public Property locazip() As String
        Get
            Return _locazip
        End Get
        Set(ByVal value As String)
            _locazip = value
        End Set
    End Property

    Public Property loginPrefix() As String
        Get
            Return _loginPrefix
        End Get
        Set(ByVal value As String)
            _loginPrefix = value
        End Set
    End Property

    Public Property EnableSickLeave() As Boolean
        Get
            Return _EnableSickLeave
        End Get
        Set(ByVal value As Boolean)
            _EnableSickLeave = value
        End Set
    End Property

    Public Property AdministrativeFee() As Decimal
        Get
            Return _AdministrativeFee
        End Get
        Set(ByVal value As Decimal)
            _AdministrativeFee = value
        End Set
    End Property

    Public Property CustomerFee() As Decimal
        Get
            Return _CustomerFee
        End Get
        Set(ByVal value As Decimal)
            _CustomerFee = value
        End Set
    End Property
#End Region

End Class

Public Class PriceList

#Region "PriceList Private variables"
    Private _PriceID As Guid
    Private _LocationID As Guid
    Private _DepartmentID As Guid
    Private _LoadtypeID As Guid
    Private _LoadDescriptionID As Guid
    Private _RatePerCase As Decimal
    Private _RatePerPallet As Decimal
    Private _PerPalletLow As Integer
    Private _PerPalletHigh As Integer
    Private _RatePerLoad As Decimal
    Private _RateBadPallet As Decimal
    Private _RateRestack As Decimal
    Private _PriceMax As Decimal
    Private _RateDoubleStack As Decimal
    Private _RatePinWheeled As Decimal
    Private _DepartmentName As String
    Private _LoadTypeName As String
    Private _LoadDescriptionName As String

#End Region

#Region "PriceList Public Properties"
    Public Property PriceID() As Guid
        Get
            Return _PriceID
        End Get
        Set(ByVal value As Guid)
            _PriceID = value
        End Set
    End Property

    Public Property LocationID() As Guid
        Get
            Return _LocationID
        End Get
        Set(ByVal value As Guid)
            _LocationID = value
        End Set
    End Property

    Public Property DepartmentID() As Guid
        Get
            Return _DepartmentID
        End Get
        Set(ByVal value As Guid)
            _DepartmentID = value
        End Set
    End Property

    Public Property LoadtypeID() As Guid
        Get
            Return _LoadtypeID
        End Get
        Set(ByVal value As Guid)
            _LoadtypeID = value
        End Set
    End Property

    Public Property LoadDescriptionID() As Guid
        Get
            Return _LoadDescriptionID
        End Get
        Set(ByVal value As Guid)
            _LoadDescriptionID = value
        End Set
    End Property

    Public Property RatePerCase() As Decimal
        Get
            Return _RatePerCase
        End Get
        Set(ByVal value As Decimal)
            _RatePerCase = value
        End Set
    End Property

    Public Property RatePerPallet() As Decimal
        Get
            Return _RatePerPallet
        End Get
        Set(ByVal value As Decimal)
            _RatePerPallet = value
        End Set
    End Property

    Public Property PerPalletLow() As Integer
        Get
            Return _PerPalletLow
        End Get
        Set(ByVal value As Integer)
            _PerPalletLow = value
        End Set
    End Property

    Public Property PerPalletHigh() As Integer
        Get
            Return _PerPalletHigh
        End Get
        Set(ByVal value As Integer)
            _PerPalletHigh = value
        End Set
    End Property

    Public Property RatePerLoad() As Decimal
        Get
            Return _RatePerLoad
        End Get
        Set(ByVal value As Decimal)
            _RatePerLoad = value
        End Set
    End Property

    Public Property RateBadPallet() As Decimal
        Get
            Return _RateBadPallet
        End Get
        Set(ByVal value As Decimal)
            _RateBadPallet = value
        End Set
    End Property

    Public Property RateRestack() As Decimal
        Get
            Return _RateRestack
        End Get
        Set(ByVal value As Decimal)
            _RateRestack = value
        End Set
    End Property

    Public Property PriceMax() As Decimal
        Get
            Return _PriceMax
        End Get
        Set(ByVal value As Decimal)
            _PriceMax = value
        End Set
    End Property

    Public Property RateDoubleStack() As Decimal
        Get
            Return _RateDoubleStack
        End Get
        Set(ByVal value As Decimal)
            _RateDoubleStack = value
        End Set
    End Property

    Public Property RatePinWheeled() As Decimal
        Get
            Return _RatePinWheeled
        End Get
        Set(ByVal value As Decimal)
            _RatePinWheeled = value
        End Set
    End Property

    'not in PriceList Table

    Public Property DepartmentName() As String
        Get
            Return _DepartmentName
        End Get
        Set(ByVal value As String)
            _DepartmentName = value
        End Set
    End Property

    Public Property LoadTypeName() As String
        Get
            Return _LoadTypeName
        End Get
        Set(ByVal value As String)
            _LoadTypeName = value
        End Set
    End Property

    Public Property LoadDescriptionName() As String
        Get
            Return _LoadDescriptionName
        End Get
        Set(ByVal value As String)
            _LoadDescriptionName = value
        End Set
    End Property

#End Region





End Class