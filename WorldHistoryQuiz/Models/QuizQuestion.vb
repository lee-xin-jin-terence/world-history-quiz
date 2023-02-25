'''<author> Terence Lee </author>
''' <summary>
'''  A simple model class to contain the various properties of a
'''  quiz question. Contains only property setters and getters
''' </summary>
Public Class QuizQuestion

    Private _question As String
    Private _options As List(Of String) = Nothing
    Private _correctAnswerIndex As Integer = -1
    Private _explanation As String

    Public Sub New()

    End Sub

    ''' <summary>
    ''' Setter and getter for the question of the quiz
    ''' 
    ''' When the value is set, it will trim the leading and trailing whitespace 
    ''' characters before setting the value
    ''' </summary>
    ''' <returns>the question of the quiz</returns>
    Public Property Question As String
        Get
            Return Me._question
        End Get
        Set(value As String)
            Me._question = value.Trim()
        End Set
    End Property


    ''' <summary>
    ''' Setter and getter of the available options for the quiz. Expects
    ''' exactly 4 options when setting the value
    ''' </summary>
    ''' <returns>
    '''     If the options has not been previously set, returns null
    '''     Otherwise returns a list of 4 elements containing the available
    '''     options for the quiz question
    ''' </returns>
    Public Property Options As List(Of String)

        Get

            Return New List(Of String)(Me._options)
        End Get

        Set(value As List(Of String))

            Me._options = New List(Of String)(4)

            For Each choice As String In value
                choice = choice.Trim()
                Me._options.Add(choice)
            Next


            Me.Options.TrimExcess()

        End Set

    End Property


    ''' <summary>
    ''' Setter and getter for the correct answer index
    ''' 
    ''' When setting value, only accepts the value between 0 (inclusive) and 3 (inclusive)
    ''' 
    ''' When getting the value, if the value has not been set, returns default value
    ''' of -1. Otherwise returns the value that was set
    ''' </summary>
    ''' <returns></returns>
    Public Property CorrectAnswerIndex As Integer
        Get

            Return Me._correctAnswerIndex
        End Get

        Set(value As Integer)


            If value < 0 Or value > 3 Then
                Throw New ArgumentException(value.ToString() + " is not a valid value")
            End If


            Me._correctAnswerIndex = value
        End Set
    End Property


    ''' <summary>
    ''' Simple Setter and getter for the explanation of the question's correct answer.
    ''' 
    ''' When the value is set, it will trim the leading and trailing whitespace 
    ''' characters before setting the value
    ''' </summary>
    ''' <returns>explanation of the quiz question</returns>
    Public Property Explanation As String

        Get
            Return Me._explanation
        End Get


        Set(value As String)
            Me._explanation = value.Trim()
        End Set

    End Property
End Class
