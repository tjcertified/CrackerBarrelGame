Public Class Move

#Region "Properties"
    Public Property FromHole As Integer
    Public Property OverHole As Integer
    Public Property ToHole As Integer
#End Region

#Region "Constructors"
    Public Sub New(fromHole As Integer, overHole As Integer, toHole As Integer)
        Me.FromHole = fromHole
        Me.OverHole = overHole
        Me.ToHole = toHole
    End Sub
#End Region

#Region "Methods"
    Public Overrides Function ToString() As String
        Return FromHole.ToString.PadLeft(2, "0") & "-" & ToHole.ToString.PadLeft(2, "0")
    End Function
#End Region

End Class
