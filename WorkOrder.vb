Public Class WorkOrder
#Region "Private Variables"
    Private _Status As Integer  '192
    Private _LogDate As Date
    Private _LogNumber As Integer   '-1
    Private _LoadNumber As String      'increment last loca loadnumber 
    Private _LocationID As Guid
    Private _DepartmentID As Guid
    Private _Department As String    'not in workorder table
    Private _LoadTypeID As Guid
    Private _LoadType As String     'not in workorder table
    Private _CustomerID As Guid
    Private _VendorNumber As String
    Private _VendorName As String     'not in workorder table
    Private _ReceiptNumber As String   'NEVER ON Backhaul/Inbound default to -1
    Private _PurchaseOrder As String
    Private _PaymentType As String
    Private _Amount As Decimal
    Private _SplitPaymentAmount As Decimal
    Private _IsCash As Boolean      'always false on backhaul/inbound
    Private _LoadDescriptionID As Guid
    Private _LoadDescription As String     'not in workorder table
    Private _CarrierID As Guid
    Private _CarrierName As String     'not in workorder table
    Private _TruckNumber As String      'if empty "DROP"   
    Private _TrailerNumber As String    '
    Private _AppointmentTime As Date    '1/1/1900
    Private _GateTime As Date    '1/1/1900
    Private _DockTime As Date
    Private _StartTime As Date
    Private _CompTime As Date
    Private _TTLTime As Integer 'in minutes
    Private _PalletsUnloaded As Integer
    Private _DoorNumber As String
    Private _Pieces As Integer
    Private _Weight As Integer
    Private _Comments As String
    Private _Restacks As Integer
    Private _PalletsReceived As Integer
    Private _BadPallets As Integer
    Private _NumberOfItems As Integer
    Private _CheckNumber As String      'or null
    Private _BOL As String              'or null
    Private _ID As Guid         'the ONLY not null
    Private _isClosed As Boolean     'not in workorder table
    Private _Employee As List(Of String)     'not in workorder table
    '  UPDATE FOR DATABASE
    Private _CreatedBy As String

#End Region
    Public Sub New()
        _ID = Guid.NewGuid
        _LogDate = Date.Now
        _LogNumber = -1
        Dim strloadnumber As String = Format(_LogDate, "MMddHHmmss")
        _LoadNumber = strloadnumber
        _ReceiptNumber = _LoadNumber
        _BadPallets = -1
        _Restacks = -1
        _Weight = -1
        _PalletsUnloaded = -1
        _NumberOfItems = -1
        _AppointmentTime = CDate("1/1/1900 12:00 AM")
        _GateTime = CDate("1/1/1900 12:00 AM")
        _StartTime = CDate("1/1/1900 12:00 AM")
        _CompTime = CDate("1/1/1900 12:00 AM")
        _DockTime = CDate("1/1/1900 12:00 AM")
    End Sub
#Region "Public Properties"
    Public Property Status() As Integer
        Get
            Return _Status
        End Get
        Set(ByVal value As Integer)
            _Status = value
        End Set
    End Property

    Public Property LogDate() As Date
        Get
            Return _LogDate
        End Get
        Set(ByVal value As Date)
            _LogDate = value
        End Set
    End Property
    Public Property LogNumber() As Integer
        Get
            Return _LogNumber
        End Get
        Set(ByVal value As Integer)
            _LogNumber = value
        End Set
    End Property
    Public Property LoadNumber() As String
        Get
            Return _LoadNumber
        End Get
        Set(ByVal value As String)
            _LoadNumber = value
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
    Public Property Department() As String
        Get
            Return _Department
        End Get
        Set(ByVal value As String)
            _Department = value
        End Set
    End Property
    Public Property LoadTypeID() As Guid
        Get
            Return _LoadTypeID
        End Get
        Set(ByVal value As Guid)
            _LoadTypeID = value
        End Set
    End Property
    Public Property LoadType() As String
        Get
            Return _LoadType
        End Get
        Set(ByVal value As String)
            _LoadType = value
        End Set
    End Property
    Public Property CustomerID() As Guid
        Get
            Return _CustomerID
        End Get
        Set(ByVal value As Guid)
            _CustomerID = value
        End Set
    End Property
    Public Property VendorNumber() As String
        Get
            Return _VendorNumber
        End Get
        Set(ByVal value As String)
            _VendorNumber = value
        End Set
    End Property
    Public Property VendorName() As String
        Get
            Return _VendorName
        End Get
        Set(ByVal value As String)
            _VendorName = value
        End Set
    End Property
    Public Property ReceiptNumber() As String
        Get
            Return _ReceiptNumber
        End Get
        Set(ByVal value As String)
            _ReceiptNumber = value
        End Set
    End Property
    Public Property PurchaseOrder() As String
        Get
            Return _PurchaseOrder
        End Get
        Set(ByVal value As String)
            _PurchaseOrder = value
        End Set
    End Property
    Public Property PaymentType() As String
        Get
            Return _PaymentType
        End Get
        Set(ByVal value As String)
            _PaymentType = value
        End Set
    End Property
    Public Property Amount() As Decimal
        Get
            Return _Amount
        End Get
        Set(ByVal value As Decimal)
            _Amount = value
        End Set
    End Property

    Public Property SplitPaymentAmount() As Decimal
        Get
            Return _SplitPaymentAmount
        End Get
        Set(ByVal value As Decimal)
            _SplitPaymentAmount = value
        End Set
    End Property

    Public Property IsCash() As Boolean
        Get
            Return _IsCash
        End Get
        Set(ByVal value As Boolean)
            _IsCash = value
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
    Public Property LoadDescription() As String
        Get
            Return _LoadDescription
        End Get
        Set(ByVal value As String)
            _LoadDescription = value
        End Set
    End Property
    Public Property CarrierID() As Guid
        Get
            Return _CarrierID
        End Get
        Set(ByVal value As Guid)
            _CarrierID = value
        End Set
    End Property
    Public Property CarrierName() As String
        Get
            Return _CarrierName
        End Get
        Set(ByVal value As String)
            _CarrierName = value
        End Set
    End Property
    Public Property TruckNumber() As String
        Get
            Return _TruckNumber
        End Get
        Set(ByVal value As String)
            _TruckNumber = value
        End Set
    End Property
    Public Property TrailerNumber() As String
        Get
            Return _TrailerNumber
        End Get
        Set(ByVal value As String)
            _TrailerNumber = value
        End Set
    End Property
    Public Property AppointmentTime() As Date
        Get
            Return _AppointmentTime
        End Get
        Set(ByVal value As Date)
            _AppointmentTime = value
        End Set
    End Property
    Public Property GateTime() As Date
        Get
            Return _GateTime
        End Get
        Set(ByVal value As Date)
            _GateTime = value
        End Set
    End Property
    Public Property DockTime() As Date
        Get
            Return _DockTime
        End Get
        Set(ByVal value As Date)
            _DockTime = value
        End Set
    End Property
    Public Property StartTime() As Date
        Get
            Return _StartTime
        End Get
        Set(ByVal value As Date)
            _StartTime = value
        End Set
    End Property
    Public Property CompTime() As Date
        Get
            Return _CompTime
        End Get
        Set(ByVal value As Date)
            _CompTime = value
        End Set
    End Property
    Public Property TTLTime() As Integer
        Get
            Return _TTLTime
        End Get
        Set(ByVal value As Integer)
            _TTLTime = value
        End Set
    End Property
    Public Property PalletsUnloaded() As Integer
        Get
            Return _PalletsUnloaded
        End Get
        Set(ByVal value As Integer)
            _PalletsUnloaded = value
        End Set
    End Property
    Public Property DoorNumber() As String
        Get
            Return _DoorNumber
        End Get
        Set(ByVal value As String)
            _DoorNumber = value
        End Set
    End Property
    Public Property Pieces() As Integer
        Get
            Return _Pieces
        End Get
        Set(ByVal value As Integer)
            _Pieces = value
        End Set
    End Property
    Public Property Weight() As Integer
        Get
            Return _Weight
        End Get
        Set(ByVal value As Integer)
            _Weight = value
        End Set
    End Property
    Public Property Comments() As String
        Get
            Return _Comments
        End Get
        Set(ByVal value As String)
            _Comments = value
        End Set
    End Property
    Public Property Restacks() As Integer
        Get
            Return _Restacks
        End Get
        Set(ByVal value As Integer)
            _Restacks = value
        End Set
    End Property
    Public Property PalletsReceived() As Integer
        Get
            Return _PalletsReceived
        End Get
        Set(ByVal value As Integer)
            _PalletsReceived = value
        End Set
    End Property
    Public Property BadPallets() As Integer
        Get
            Return _BadPallets
        End Get
        Set(ByVal value As Integer)
            _BadPallets = value
        End Set
    End Property
    Public Property NumberOfItems() As Integer
        Get
            Return _NumberOfItems
        End Get
        Set(ByVal value As Integer)
            _NumberOfItems = value
        End Set
    End Property
    Public Property CheckNumber() As String
        Get
            Return _CheckNumber
        End Get
        Set(ByVal value As String)
            _CheckNumber = value
        End Set
    End Property
    Public Property BOL() As String
        Get
            Return _BOL
        End Get
        Set(ByVal value As String)
            _BOL = value
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
    Public Property isClosed() As Boolean
        Get
            Return _isClosed
        End Get
        Set(ByVal value As Boolean)
            _isClosed = value
        End Set
    End Property
    Public Property Employee() As List(Of String)
        Get
            Return _Employee
        End Get
        Set(ByVal value As List(Of String))
            _Employee = value
        End Set
    End Property
    Public Property CreatedBy() As String
        Get
            Return _CreatedBy
        End Get
        Set(ByVal value As String)
            _CreatedBy = value
        End Set
    End Property

#End Region

End Class


Public Class ActiveWorkOrder
#Region "Private Variables"
    Private _DoorNumber As String
    Private _Vendor As String
    Private _PurchaseOrder As String
    Private _Carrier As String
    Private _TruckNumber As String
    Private _TrailerNumber As String
    Private _StartTime As String
    Private _CompTime As String
    Private _Department As String
    Private _LoadType As String
    Private _Unloaders As String
#End Region

#Region "Public Properties"

    Public Property DoorNumber() As String
        Get
            Return _DoorNumber
        End Get
        Set(ByVal value As String)
            _DoorNumber = value
        End Set
    End Property

    Public Property Vendor() As String
        Get
            Return _Vendor
        End Get
        Set(ByVal value As String)
            _Vendor = value
        End Set
    End Property

    Public Property PurchaseOrder() As String
        Get
            Return _PurchaseOrder
        End Get
        Set(ByVal value As String)
            _PurchaseOrder = value
        End Set
    End Property

    Public Property Carrier() As String
        Get
            Return _Carrier
        End Get
        Set(ByVal value As String)
            _Carrier = value
        End Set
    End Property

    Public Property TruckNumber() As String
        Get
            Return _TruckNumber
        End Get
        Set(ByVal value As String)
            _TruckNumber = value
        End Set
    End Property

    Public Property TrailerNumber() As String
        Get
            Return _TrailerNumber
        End Get
        Set(ByVal value As String)
            _TrailerNumber = value
        End Set
    End Property

    Public Property StartTime() As String
        Get
            Return _StartTime
        End Get
        Set(ByVal value As String)
            _StartTime = value
        End Set
    End Property

    Public Property CompTime() As String
        Get
            Return _CompTime
        End Get
        Set(ByVal value As String)
            _CompTime = value
        End Set
    End Property

    Public Property Department() As String
        Get
            Return _Department
        End Get
        Set(ByVal value As String)
            _Department = value
        End Set
    End Property

    Public Property LoadType() As String
        Get
            Return _LoadType
        End Get
        Set(ByVal value As String)
            _LoadType = value
        End Set
    End Property

    Public Property Unloaders() As String
        Get
            Return _Unloaders
        End Get
        Set(ByVal value As String)
            _Unloaders = value
        End Set
    End Property
#End Region

End Class



