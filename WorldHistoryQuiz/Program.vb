Imports System

''' <author>Terence Lee</author>
''' <summary>
'''     A simple world history multiple-choice quiz program
''' </summary>
Module Program

    Dim userScore As Integer = 0
    Dim quizQuestionXDocument As XDocument
    Dim listOfQuizQuestions As List(Of QuizQuestion) =
                                    New List(Of QuizQuestion)


    Sub Main(args As String())

        LoadEventXMLFile()

        AddQuizQuestionFromXMLToListOfQuizQuestion()

        DisplayStartOfQuizMessage()

        DisplayAllQuizQuestionsAndPromptAnswer()

        DisplayOverallScoreAndEndOfQuizMessage()
    End Sub


    ''' <summary>
    ''' Load the XML file containing all the question, answers and explanation
    ''' for the quiz
    ''' 
    ''' If the xml file does not exist or cannot be opened, then the program
    ''' will terminate with exit code 1
    ''' </summary>
    Private Sub LoadEventXMLFile()

        Const QuizDataXmlFileName As String = "quiz-data.xml"

        Try
            quizQuestionXDocument = XDocument.Load(QuizDataXmlFileName)
        Catch ex As Exception
            Console.WriteLine($"Failed to load the xml {QuizDataXmlFileName} file. The program will now terminate")
            System.Environment.Exit(1)
        End Try


    End Sub


    ''' <summary>
    ''' Iterate through the xml document containing the quiz questions and
    ''' convert them to the model object QuizQuestion
    ''' </summary>
    ''' <exception>
    ''' If any exception is thrown, display the error message and terminate the
    ''' program
    ''' </exception>
    Sub AddQuizQuestionFromXMLToListOfQuizQuestion()

        Dim listOfQuizQuestionElements As IEnumerable(Of XElement) =
                quizQuestionXDocument.Descendants("question-and-answer")


        Try
            For Each currentQuizQuestionElement As XElement In listOfQuizQuestionElements

                Dim quizQuestion As QuizQuestion =
                            XElementToQuizQuestionAdapter.Convert(currentQuizQuestionElement)

                listOfQuizQuestions.Add(quizQuestion)
            Next
        Catch ex As Exception
            Console.WriteLine("Error : " + ex.Message)
            System.Environment.Exit(2)
        End Try

    End Sub


    ''' <summary>
    ''' Iterate through all the questions in listOfQuizQuestions,and prompt
    ''' user for the answers to these questions. After each prompt, inform the
    ''' user whether the selected answer is correct, and along with the explanation
    ''' </summary>
    Sub DisplayAllQuizQuestionsAndPromptAnswer()

        Dim questionNumber As Integer = 1

        For Each quizQuestion As QuizQuestion In listOfQuizQuestions
            DisplayQuestion(quizQuestion, questionNumber)
            DisplayPossibleOptions(quizQuestion)
            Dim userAnswer As String = PromptUserForAnswer()

            CheckIfAnswerIsCorrect(quizQuestion, userAnswer)

            DisplayCorrectAnswer(quizQuestion)
            DisplayExplanation(quizQuestion)

            questionNumber += 1

            WaitForUserToEnterAnyKeyBeforeGoingToNextQuestion()
        Next

    End Sub


    ''' <summary>
    ''' A message to be displayed to the user at the start of the quiz
    ''' </summary>
    Sub DisplayStartOfQuizMessage()
        Console.WriteLine("-------- World History Quiz By Terence Lee --------")
        Console.WriteLine("Thank you for taking interest in this quiz")
        Console.WriteLine()
        Console.WriteLine("This quiz has a total of 10 multiple-choice questions")
        Console.WriteLine("---------------------------------------------------")
        Console.WriteLine()
    End Sub


    ''' <summary>
    ''' Display the current quiz question to the user
    ''' </summary>
    ''' <param name="quizQuestion">The current quiz question of interest</param>
    ''' <param name="questionNumber">The question number of this quiz question</param>
    Sub DisplayQuestion(ByRef quizQuestion As QuizQuestion,
                        questionNumber As Integer)

        Console.WriteLine($"Question {questionNumber} of {listOfQuizQuestions.Count}:")
        Console.WriteLine(quizQuestion.Question)
        Console.WriteLine()
    End Sub


    ''' <summary>
    ''' Display the 4 options available to the user
    ''' </summary>
    ''' 
    ''' <param name="quizQuestion">The current question of interest</param>
    Sub DisplayPossibleOptions(ByRef quizQuestion As QuizQuestion)
        Console.WriteLine("[A] " + quizQuestion.Options(0))
        Console.WriteLine("[B] " + quizQuestion.Options(1))
        Console.WriteLine("[C] " + quizQuestion.Options(2))
        Console.WriteLine("[D] " + quizQuestion.Options(3))
        Console.WriteLine()
    End Sub


    ''' <summary>
    ''' Prompt user to select one of the options as answer. Will keep prompting
    ''' the user until the user selects one of the 4 valid options
    ''' </summary>
    ''' <returns>Always return either the string "A", "B", "C" or "D"
    '''     The return value is always a single character string, and always
    '''     uppercase
    ''' </returns>
    Function PromptUserForAnswer() As String

        Dim userInputAnswer As String = ""

        While True
            Console.Write("Enter answer [A,B,C or D]: ")

            userInputAnswer = Console.ReadLine()
            userInputAnswer = userInputAnswer.Trim().ToUpper()

            If userInputAnswer = "A" Or userInputAnswer = "B" Or
                userInputAnswer = "C" Or userInputAnswer = "D" Then
                Exit While
            Else
                Console.WriteLine("Invalid option selected")
            End If

        End While


        Return userInputAnswer

    End Function


    ''' <summary>
    ''' Check whether the answer the user selected for the question is correct
    ''' </summary>
    ''' <param name="quizQuestion">The quiz question of interest, where the answer
    ''' provided by the user is going to be checked</param>
    ''' 
    ''' <param name="userSelectedAnswer">The answer selected by the user,
    '''     and will only either be "A", "B", "C" or "D"
    ''' </param>
    Sub CheckIfAnswerIsCorrect(ByRef quizQuestion As QuizQuestion,
                               userSelectedAnswer As String)

        Dim choicesArrayIndex = -1

        Select Case (userSelectedAnswer)
            Case "A"
                choicesArrayIndex = 0

            Case "B"
                choicesArrayIndex = 1

            Case "C"
                choicesArrayIndex = 2

            Case "D"
                choicesArrayIndex = 3
        End Select

        If choicesArrayIndex = quizQuestion.CorrectAnswerIndex Then
            Console.WriteLine("The answer is correct!")
            userScore += 1
        Else
            Console.WriteLine("The answer is incorrect")

        End If
    End Sub


    ''' <summary>
    ''' Display the correct answer to the user
    ''' </summary>
    ''' <param name="quizQuestion">The current quiz question of interest</param>
    Sub DisplayCorrectAnswer(ByRef quizQuestion As QuizQuestion)

        Dim correctAnswerCharacter As Char

        Select Case (quizQuestion.CorrectAnswerIndex)
            Case 0
                correctAnswerCharacter = "A"c

            Case 1
                correctAnswerCharacter = "B"c

            Case 2
                correctAnswerCharacter = "C"c

            Case 3
                correctAnswerCharacter = "D"c
        End Select


        Console.WriteLine($"The correct answer is [{correctAnswerCharacter}]")
        Console.WriteLine()
    End Sub



    ''' <summary>
    ''' Display the explanation for the correct answer after the
    ''' correct answer has been shown to the user
    ''' </summary>
    ''' <param name="quizQuestion"></param>
    Sub DisplayExplanation(ByRef quizQuestion As QuizQuestion)
        Console.WriteLine("Explanation:")
        Console.WriteLine(quizQuestion.Explanation)
        Console.WriteLine()
    End Sub


    ''' <summary>
    ''' Wait for the user to enter any key (before proceeding to next question)
    ''' </summary>
    Sub WaitForUserToEnterAnyKeyBeforeGoingToNextQuestion()

        Console.WriteLine("Enter any key to continue to next question...")
        Console.ReadKey()
        Console.WriteLine()
        Console.WriteLine("---------------------------------------------------")
        Console.WriteLine()
    End Sub


    ''' <summary>
    ''' Display the user overalll score and an end-of-quiz thank you message'
    ''' </summary>
    Sub DisplayOverallScoreAndEndOfQuizMessage()
        Console.Write(
            $"Your overall score is {userScore.ToString()}/{listOfQuizQuestions.Count}")
        Console.WriteLine()
        Console.WriteLine("Thank you for spending your time in this quiz")
        Console.WriteLine()
        Console.WriteLine("-------- End of World History Quiz By Terence Lee --------")
    End Sub



End Module
