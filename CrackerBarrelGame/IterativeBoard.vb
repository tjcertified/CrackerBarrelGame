Public Class IterativeBoard
    Public Const FirstHole As Integer = 0
    Public Const LastHole As Integer = 14
    Public Shared ReadOnly AvailableMoves As List(Of Move) = New List(Of Move) From {
            New Move(0, 1, 3), New Move(0, 2, 5),
            New Move(1, 3, 6), New Move(1, 4, 8),
            New Move(2, 4, 7), New Move(2, 5, 9),
            New Move(3, 1, 0), New Move(3, 4, 5), New Move(3, 6, 10), New Move(3, 7, 12),
            New Move(4, 7, 11), New Move(4, 8, 13),
            New Move(5, 2, 0), New Move(5, 4, 3), New Move(5, 8, 12), New Move(5, 9, 14),
            New Move(6, 3, 1), New Move(6, 7, 8),
            New Move(7, 4, 2), New Move(7, 8, 9),
            New Move(8, 4, 1), New Move(8, 7, 6),
            New Move(9, 5, 2), New Move(9, 8, 7),
            New Move(10, 6, 3), New Move(10, 11, 12),
            New Move(11, 7, 4), New Move(11, 12, 13),
            New Move(12, 7, 3), New Move(12, 8, 5), New Move(12, 11, 10), New Move(12, 13, 14),
            New Move(13, 8, 4), New Move(13, 12, 11),
            New Move(14, 9, 5), New Move(14, 13, 12)
        }

    Public Sub New(emptyStartHole As Integer)
        Holes = New List(Of Boolean)

        For holeNumber = FirstHole To LastHole
            Holes.Add(True)
        Next
        Holes(emptyStartHole) = False
        Me.StartPosition = emptyStartHole
        MadeMoves = New List(Of Move)
    End Sub

    Public Property StartPosition As Integer

    Public Property MadeMoves As List(Of Move)

    Public Property Holes As List(Of Boolean)

    Public ReadOnly Property CurrentValidMoves As List(Of Move)
        Get
            Return AvailableMoves.FindAll(Function(m As Move) IsValidMove(m))
        End Get
    End Property

    Public ReadOnly Property Solved As Boolean
        Get
            Return CurrentValidMoves.Count = 0
        End Get
    End Property

    Public ReadOnly Property EndHoleCount As Integer
        Get
            Return Holes.FindAll(Function(x) x = True).Count
        End Get
    End Property

    Public Function IsValidMove(move As Move) As Boolean
        Return (Holes(move.FromHole) = True And Holes(move.OverHole) = True And Holes(move.ToHole) = False)
    End Function

    Public Function MakeMove(move As Move) As Boolean
        If IsValidMove(move) Then
            Holes(move.FromHole) = False
            Holes(move.OverHole) = False
            Holes(move.ToHole) = True

            MadeMoves.Add(move)
            Return True
        End If

        Return False
    End Function

    Public Function Clone() As IterativeBoard
        Dim newBoard As IterativeBoard = New IterativeBoard(Me.StartPosition)

        newBoard.MadeMoves = New List(Of Move)(Me.MadeMoves)
        newBoard.Holes = New List(Of Boolean)(Me.Holes)

        Return newBoard
    End Function

    Public Function SolutionAsString() As String
        Dim solution As String
        solution = String.Join(",", MadeMoves)
        solution.Insert(0, EndHoleCount.ToString + ",")
        Return solution
    End Function

End Class
