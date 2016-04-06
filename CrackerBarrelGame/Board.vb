Public Class Board

#Region "Constants"
    Private Const FirstHole As Integer = 1
    Private Const LastHole As Integer = 15
#End Region

#Region "Properties"
    Private Property Holes As List(Of Hole)

    Private ReadOnly Property Holes(holeNumber As Integer) As Hole
        Get
            Return Me.Holes.Find(Function(h) h.Number = holeNumber)
        End Get
    End Property

    Private ReadOnly Property AvailableMoves As List(Of Move)
        Get
            Dim list As New List(Of Move)

            For Each hole As Hole In Holes
                For Each move As Move In hole.Moves
                    If CheckMove(move) Then
                        list.Add(move)
                    End If
                Next
            Next

            Return list
        End Get
    End Property

    Private Property MadeMoves As List(Of Move)

    Public Property Solutions As New SortedSet(Of String)
#End Region

#Region "Constructors"
    Public Sub New(Optional emptyHoleNumber As Integer = FirstHole)
        Holes = New List(Of Hole)

        For holeNumber = FirstHole To LastHole
            Dim newHole As New Hole(holeNumber)
            If holeNumber = emptyHoleNumber Then
                newHole.Empty = True
            End If
            Holes.Add(newHole)
        Next

        MadeMoves = New List(Of Move)
    End Sub
#End Region

#Region "Methods"
    Public Sub Solve(Optional board As Board = Nothing)
        If board Is Nothing Then
            board = Me
        End If

        Dim solved As Boolean = True

        For Each move In board.AvailableMoves
            solved = False

            Dim newBoard As Board = board.Clone
            newBoard.MakeMove(move)
            Solve(newBoard)

            For Each solution In newBoard.Solutions
                board.Solutions.Add(solution)
            Next
        Next

        If solved Then
            Dim pegCount As Integer = board.Holes.FindAll(Function(h) h.Empty = False).Count
            Dim solution As String = pegCount.ToString & vbTab
            For Each madeMove In board.MadeMoves
                solution &= madeMove.ToString & vbTab
            Next
            board.Solutions.Add(solution)
        End If
    End Sub

    Private Function CheckMove(move As Move) As Boolean
        Return (Holes(move.FromHole).Empty = False And Holes(move.OverHole).Empty = False And Holes(move.ToHole).Empty = True)
    End Function

    Private Function MakeMove(move As Move) As Boolean
        If CheckMove(move) Then
            Holes(move.FromHole).Empty = True
            Holes(move.OverHole).Empty = True
            Holes(move.ToHole).Empty = False

            MadeMoves.Add(move)

            Return True
        End If

        Return False
    End Function

    Private Function Clone() As Board
        Dim newBoard As New Board

        For Each myHole In Me.Holes
            newBoard.Holes(myHole.Number).Empty = myHole.Empty
        Next

        For Each move In Me.MadeMoves
            Dim newMove As New Move(move.FromHole, move.OverHole, move.ToHole)
            newBoard.MadeMoves.Add(newMove)
        Next

        Return newBoard
    End Function

    Private Shared Sub CheckHoleNumber(holeNumber As Integer)
        If holeNumber < Board.FirstHole Or holeNumber > Board.LastHole Then
            Throw New ArgumentOutOfRangeException("Hole number must be an integer between " & Board.FirstHole.ToString & " and " & Board.LastHole.ToString & ".")
        End If
    End Sub
#End Region

#Region "Classes"
    Private Class Hole

#Region "Properties"
        Private _Number As Integer
        Public Property Number As Integer
            Get
                Return _Number
            End Get
            Set(value As Integer)
                CheckHoleNumber(value)
                _Number = value
            End Set
        End Property

        Public Property Empty As Boolean

        Public ReadOnly Property Moves As Move()
            Get
                Select Case Me.Number
                    Case 1
                        Return {New Move(1, 2, 4), New Move(1, 3, 6)}
                    Case 2
                        Return {New Move(2, 4, 7), New Move(2, 5, 9)}
                    Case 3
                        Return {New Move(3, 5, 8), New Move(3, 6, 10)}
                    Case 4
                        Return {New Move(4, 2, 1), New Move(4, 5, 6), New Move(4, 7, 11), New Move(4, 8, 13)}
                    Case 5
                        Return {New Move(5, 8, 12), New Move(5, 9, 14)}
                    Case 6
                        Return {New Move(6, 3, 1), New Move(6, 5, 4), New Move(6, 9, 13), New Move(6, 10, 15)}
                    Case 7
                        Return {New Move(7, 4, 2), New Move(7, 8, 9)}
                    Case 8
                        Return {New Move(8, 5, 3), New Move(8, 9, 10)}
                    Case 9
                        Return {New Move(9, 5, 2), New Move(9, 8, 7)}
                    Case 10
                        Return {New Move(10, 6, 3), New Move(10, 9, 8)}
                    Case 11
                        Return {New Move(11, 7, 4), New Move(11, 12, 13)}
                    Case 12
                        Return {New Move(12, 8, 5), New Move(12, 13, 14)}
                    Case 13
                        Return {New Move(13, 8, 4), New Move(13, 9, 6), New Move(13, 12, 11), New Move(13, 14, 15)}
                    Case 14
                        Return {New Move(14, 9, 5), New Move(14, 13, 12)}
                    Case 15
                        Return {New Move(15, 10, 6), New Move(15, 14, 13)}
                End Select

                Return Nothing
            End Get
        End Property
#End Region

#Region "Constructors"
        Public Sub New(number As Integer)
            Me.Number = number
            Empty = False
        End Sub
#End Region

    End Class

    Private Class Move

#Region "Properties"
        Public Property FromHole As Integer
        Public Property OverHole As Integer
        Public Property ToHole As Integer
#End Region

#Region "Constructors"
        Public Sub New(fromHole As Integer, overHole As Integer, toHole As Integer)
            CheckHoleNumber(fromHole)
            CheckHoleNumber(overHole)
            CheckHoleNumber(toHole)

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
#End Region

End Class
