Module Game
    Public Sub Main()
        Dim allSolutions As New SortedSet(Of String)

        For emptyHole = 1 To 15
            Dim board As New Board(emptyHole)
            board.Solve()
            For Each solution In board.Solutions
                allSolutions.Add(solution)
            Next
        Next

        Dim output As New System.IO.StreamWriter("all_solutions.txt")
        For Each solution In allSolutions
            output.WriteLine(solution)
        Next
        output.Flush()
        output.Close()
    End Sub
End Module
