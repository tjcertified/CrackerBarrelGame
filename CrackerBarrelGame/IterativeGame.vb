Public Class IterativeGame
    Public Shared Sub Run()
        Dim s As New Stopwatch
        s.Start()
        ' Container of all the game states
        Dim boardList = New List(Of IterativeBoard)

        ' Container of cloned boards to add at the end of a 'For Each' cycle
        Dim clonedBoardList = New List(Of IterativeBoard)

        ' Add default beginning game states
        For i As Integer = IterativeBoard.FirstHole To IterativeBoard.LastHole
            boardList.Add(New IterativeBoard(i))
        Next

        Dim solved As Boolean = False

        While (Not solved)
            For Each iboard In boardList
                ' if there is more than 1 valid move, then we need extra board states, since each board only has one solution
                If iboard.CurrentValidMoves.Count > 1 Then
                    Dim firstmove As Boolean = True
                    ' preserve original state
                    Dim clonedBoard = iboard.Clone
                    For Each move In iboard.CurrentValidMoves
                        ' modify original board first time
                        If (firstmove) Then
                            iboard.MakeMove(move)
                            firstmove = False
                        Else
                            ' clone and modify other boards any time after first
                            Dim newBoard = clonedBoard.Clone
                            newBoard.MakeMove(move)
                            clonedBoardList.Add(newBoard)
                        End If
                    Next
                ElseIf iboard.CurrentValidMoves.Count = 1 Then
                    iboard.MakeMove(iboard.CurrentValidMoves(0))
                End If
            Next

            If (clonedBoardList.Count > 0) Then
                boardList.AddRange(clonedBoardList)
                clonedBoardList.Clear()
            End If

            solved = boardList.All(Function(b) b.Solved = True)
            'Console.WriteLine("------------------")
            'Console.WriteLine("Boards: " + boardList.Count.ToString)
            'Console.WriteLine("------------------" + vbCrLf)
        End While

        PrintStats("Straight", boardList, s.Elapsed)

    End Sub

    Public Shared Sub RunParallel()
        Dim s As New Stopwatch
        s.Start()

        Dim boardListList = New List(Of List(Of IterativeBoard))

        ' to parallelize, create 15 new lists, each with 1 starting board with 1 hole empty
        For i As Integer = IterativeBoard.FirstHole To IterativeBoard.LastHole
            Dim tmpblist = New List(Of IterativeBoard)
            tmpblist.Add(New IterativeBoard(i))
            boardListList.Add(tmpblist)
        Next

        Dim taskList = New List(Of Task)
        For Each xboardList In boardListList
            Dim t As New Task(Sub()
                                  SolveSingleStartHole(xboardList)
                              End Sub)
            taskList.Add(t)
            t.Start()
        Next
        Task.WaitAll(taskList.ToArray())
        Dim boardList = New List(Of IterativeBoard)
        For Each tmpBoardList In boardListList
            boardList.AddRange(tmpBoardList)
        Next
        PrintStats("Parallel", boardList, s.Elapsed)
    End Sub

    Public Shared Sub RunExtraParallel()
        Dim s As New Stopwatch
        s.Start()

        Dim boardListList = New List(Of List(Of IterativeBoard))

        ' Attempt to pre-empt the first move, and parallelize more boards than just 15?
        ' Resulted in only a few seconds difference on my machine, so it is not included in the main Module file.
        For i As Integer = IterativeBoard.FirstHole To IterativeBoard.LastHole
            Dim startBoard = New IterativeBoard(i)
            For Each move In startBoard.CurrentValidMoves
                Dim nextBoard = startBoard.Clone
                nextBoard.MakeMove(move)
                Dim tmpblist = New List(Of IterativeBoard)
                tmpblist.Add(nextBoard)
                boardListList.Add(tmpblist)
            Next
        Next
        Dim taskList = New List(Of Task)
        For Each xboardList In boardListList
            Dim t As New Task(Sub()
                                  SolveSingleStartHole(xboardList)
                              End Sub)
            taskList.Add(t)
            t.Start()
        Next
        Task.WaitAll(taskList.ToArray())
        Dim boardList = New List(Of IterativeBoard)
        For Each tmpBoardList In boardListList
            boardList.AddRange(tmpBoardList)
        Next
        PrintStats("X-Parallel", boardList, s.Elapsed)
    End Sub

    Private Shared Sub SolveSingleStartHole(boardList As List(Of IterativeBoard))
        ' Container of cloned boards to add at the end of a 'For Each' cycle
        Dim clonedBoardList = New List(Of IterativeBoard)

        Dim solved As Boolean = False

        While (Not solved)
            For Each iboard In boardList
                ' if there is more than 1 valid move, then we need extra board states, since each board only has one solution
                If iboard.CurrentValidMoves.Count > 1 Then
                    Dim firstmove As Boolean = True
                    ' preserve original state
                    Dim clonedBoard = iboard.Clone
                    For Each move In iboard.CurrentValidMoves
                        ' modify original board first time
                        If (firstmove) Then
                            iboard.MakeMove(move)
                            firstmove = False
                        Else
                            ' clone and modify other boards any time after first
                            Dim newBoard = clonedBoard.Clone
                            newBoard.MakeMove(move)
                            clonedBoardList.Add(newBoard)
                        End If
                    Next
                ElseIf iboard.CurrentValidMoves.Count = 1 Then
                    iboard.MakeMove(iboard.CurrentValidMoves(0))
                End If
            Next

            If (clonedBoardList.Count > 0) Then
                boardList.AddRange(clonedBoardList)
                clonedBoardList.Clear()
            End If

            solved = boardList.All(Function(b) b.Solved = True)
            'Console.WriteLine("------------------")
            'Console.WriteLine("Boards: " + boardList.Count.ToString)
            'Console.WriteLine("------------------" + vbCrLf)
        End While
    End Sub

    Private Shared Sub PrintStats(title As String, boardList As List(Of IterativeBoard), runTime As TimeSpan)
        Console.WriteLine(vbCrLf + vbCrLf + title + "--------------------------------------------------------")
        Console.WriteLine("++++++++++++++++++++++++++")
        Console.WriteLine("------")
        Console.WriteLine("Total Time: " + runTime.TotalSeconds.ToString())
        Console.WriteLine("------")

        Console.WriteLine("Total Solutions: " + boardList.Count.ToString)
        Console.WriteLine("Total Solutions w/ 1 Peg: " + boardList.FindAll(Function(b) b.EndHoleCount = 1).Count.ToString)
        Console.WriteLine("Total Solutions w/ 2 Peg: " + boardList.FindAll(Function(b) b.EndHoleCount = 2).Count.ToString)
        Console.WriteLine("Total Solutions w/ 3 Peg: " + boardList.FindAll(Function(b) b.EndHoleCount = 3).Count.ToString)
        Console.WriteLine("Total Solutions w/ 4 Peg: " + boardList.FindAll(Function(b) b.EndHoleCount = 4).Count.ToString)
        Console.WriteLine("Total Solutions w/ 5 Peg: " + boardList.FindAll(Function(b) b.EndHoleCount = 5).Count.ToString)
        Console.WriteLine("Total Solutions w/ 6 Peg: " + boardList.FindAll(Function(b) b.EndHoleCount = 6).Count.ToString)
        Console.WriteLine("Total Solutions w/ 7 Peg: " + boardList.FindAll(Function(b) b.EndHoleCount = 7).Count.ToString)
        Console.WriteLine("Total Solutions w/ 8 Peg: " + boardList.FindAll(Function(b) b.EndHoleCount = 8).Count.ToString)
        Console.WriteLine("Total Solutions w/ 9 Peg: " + boardList.FindAll(Function(b) b.EndHoleCount = 9).Count.ToString)
        Console.WriteLine("Total Solutions w/ 10 Peg: " + boardList.FindAll(Function(b) b.EndHoleCount = 10).Count.ToString)
        Console.WriteLine("Total Solutions w/ 11 Peg: " + boardList.FindAll(Function(b) b.EndHoleCount = 11).Count.ToString)
        Console.WriteLine("Total Solutions w/ 12 Peg: " + boardList.FindAll(Function(b) b.EndHoleCount = 12).Count.ToString)
        Console.WriteLine("Total Solutions w/ 13 Peg: " + boardList.FindAll(Function(b) b.EndHoleCount = 13).Count.ToString + vbCrLf)
        Console.WriteLine("------")
        Console.WriteLine("Total Solutions w/1st Hole Empty: " + boardList.FindAll(Function(b) b.StartPosition = 0).Count.ToString)
        Console.WriteLine("Total Solutions w/2nd Hole Empty: " + boardList.FindAll(Function(b) b.StartPosition = 1).Count.ToString)
        Console.WriteLine("Total Solutions w/3rd Hole Empty: " + boardList.FindAll(Function(b) b.StartPosition = 2).Count.ToString)
        Console.WriteLine("Total Solutions w/4th Hole Empty: " + boardList.FindAll(Function(b) b.StartPosition = 3).Count.ToString)
        Console.WriteLine("Total Solutions w/5th Hole Empty: " + boardList.FindAll(Function(b) b.StartPosition = 4).Count.ToString)
        Console.WriteLine("Total Solutions w/6th Hole Empty: " + boardList.FindAll(Function(b) b.StartPosition = 5).Count.ToString)
        Console.WriteLine("Total Solutions w/7th Hole Empty: " + boardList.FindAll(Function(b) b.StartPosition = 6).Count.ToString)
        Console.WriteLine("Total Solutions w/8th Hole Empty: " + boardList.FindAll(Function(b) b.StartPosition = 7).Count.ToString)
        Console.WriteLine("Total Solutions w/9th Hole Empty: " + boardList.FindAll(Function(b) b.StartPosition = 8).Count.ToString)
        Console.WriteLine("Total Solutions w/10th Hole Empty: " + boardList.FindAll(Function(b) b.StartPosition = 9).Count.ToString)
        Console.WriteLine("Total Solutions w/11th Hole Empty: " + boardList.FindAll(Function(b) b.StartPosition = 10).Count.ToString)
        Console.WriteLine("Total Solutions w/12th Hole Empty: " + boardList.FindAll(Function(b) b.StartPosition = 11).Count.ToString)
        Console.WriteLine("Total Solutions w/13th Hole Empty: " + boardList.FindAll(Function(b) b.StartPosition = 12).Count.ToString)
        Console.WriteLine("Total Solutions w/14th Hole Empty: " + boardList.FindAll(Function(b) b.StartPosition = 13).Count.ToString)
        Console.WriteLine("Total Solutions w/15th Hole Empty: " + boardList.FindAll(Function(b) b.StartPosition = 14).Count.ToString)
        Console.WriteLine("++++++++++++++++++++++++++")
    End Sub

End Class
