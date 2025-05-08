
Imports System.Xml

''' <author>Terence Lee</author>
''' <summary>
'''     A class that converts quiz question XElement to QuizQuestion model object,
'''     using the adapter design pattern 
''' </summary>
''' 
''' <example-usage>
'''     Dim document As XDocument = XDocument.Load("some-xml-file.xml")
'''     Dim listOfQuizQuestionElement As List(Of XElement) 
'''                         = document.Descendants("quiz-question")
'''                         
'''     For Each quizQuestionElement in listOfQuizQuestionElement
'''         Dim quizQuestion As QuizQuestion = 
'''                     XElementToQuizQuestionAdapter.Convert(quizQuestionElement)
'''     End For
''' </example-usage>
Public Class XElementToQuizQuestionAdapter

    ''' <summary>
    ''' The constructor is made private as it is not required 
    ''' </summary>
    Private Sub New()

    End Sub


    ''' <summary>
    ''' Convert quiz-question XElement to a QuizQuestion model object
    ''' </summary>
    ''' <param name="quizQuestionElement">the quizQuestionElement to be converted
    ''' </param>
    ''' 
    ''' <throws name="ArgumentNullException"> if quizQuestionElement is Nothing
    ''' </throws>
    ''' <throws name="InvalidOperationException"> if quizQuestionElement does not
    '''     contain any of the expected elements/attributes
    ''' </throws>
    ''' 
    ''' <returns></returns>
    Public Shared Function Convert(ByRef quizQuestionElement As XElement) As QuizQuestion

        ValidateQuizQuestionElementIsNotNothing(quizQuestionElement)

        Return ConvertQuizQuestionElementToQuizQuestion(quizQuestionElement)

    End Function



    ''' <summary>
    ''' Validate that the quiz Question XElement is not nothing
    ''' </summary>
    ''' <param name="quizQuestionElement">
    ''' the quizQuestionElement to be validated
    ''' </param>
    ''' <throws>If quizQuestionElement is nothing, 
    ''' throw an ArgumentNullException. Otherwise, do nothing</throws>
    ''' 
    Private Shared Sub ValidateQuizQuestionElementIsNotNothing(
                                              ByRef quizQuestionElement As XElement)

        If quizQuestionElement Is Nothing Then
            Throw New ArgumentNullException("quizQuestionElement cannot be nothing")
        End If
    End Sub


    ''' <summary>
    '''     Convert quiz-question XElement to a QuizQuestion model object
    ''' </summary>
    ''' <param name="quizQuestionElement">The quiz-question XElement to be 
    '''     converted to a QuizQuestion model object</param>
    '''     
    ''' <returns>QuizQuestion model object containing all the interested data
    ''' retrieved from the quizQuestionElement (XElement) object
    ''' </returns>
    Private Shared Function ConvertQuizQuestionElementToQuizQuestion(
                             ByRef quizQuestionElement As XElement) As QuizQuestion


        Dim quizQuestion As QuizQuestion = New QuizQuestion()


        RetrieveAndSetQuestion(quizQuestionElement, quizQuestion)
        RetrieveAndSetExplanation(quizQuestionElement, quizQuestion)
        RetrieveAndSetOptions(quizQuestionElement, quizQuestion)
        RetrieveAndSetCorrectAnswerIndex(quizQuestionElement, quizQuestion)

        Return quizQuestion

    End Function





    ''' <summary>
    ''' Retrieve the value of question from the quiz question XElement, and
    ''' set the value in the quizQuestion model object
    ''' </summary>
    ''' <param name="quizQuestionElement">quiz question element containing
    '''     data of interest, which is the question
    '''  </param>
    ''' <param name="quizQuestion">this is the return value of the sub, where
    ''' the Question property will be set</param>
    ''' <throws name="InvalidOperationException">if the quizQuestionElement contains
    '''         no 'question' elements</throws>
    Private Shared Sub RetrieveAndSetQuestion(ByRef quizQuestionElement As XElement,
                                             ByRef quizQuestion As QuizQuestion)

        quizQuestion.Question = quizQuestionElement.Descendants("question").First().Value

    End Sub


    ''' <summary>
    ''' Retrieve the value of explanation from the quiz question XElement, and
    ''' set the value in the quizQuestion model object
    ''' </summary>
    ''' 
    ''' <param name="quizQuestionElement">quiz question element containing
    '''     data of interest, which is the explanation
    '''  </param>
    '''  
    ''' <param name="quizQuestion">this is the return value of the sub, where
    ''' the Explanation property will be set</param>
    ''' 
    ''' <throws name="InvalidOperationException">if the quizQuestionElement has
    '''         contains no 'explanation' element
    '''  </throws>
    Private Shared Sub RetrieveAndSetExplanation(ByRef quizQuestionElement As XElement,
                                                 ByRef quizQuestion As QuizQuestion)

        quizQuestion.Explanation = quizQuestionElement.Descendants("explanation").First().Value
    End Sub



    ''' <summary>
    ''' Retrieve the value of all options from the quiz question XElement, and
    ''' set the value in the quizQuestion model object
    ''' </summary>
    ''' 
    ''' <param name="quizQuestionElement">quiz question element containing
    '''     data of interest, which is the options
    '''  </param>
    '''  
    ''' <param name="quizQuestion">this is the return value of the sub, where
    ''' the Options property will be set</param>
    ''' 
    ''' <throws name="InvalidOperationException">if the quizQuestionElement has
    '''         contains no 'option' element
    '''  </throws>
    Private Shared Sub RetrieveAndSetOptions(ByRef quizQuestionElement As XElement,
                                             ByRef quizQuestion As QuizQuestion)

        Dim listOfOptionElements As IEnumerable(Of XElement) =
                                quizQuestionElement.Descendants("option")

        Dim listOfOptions As List(Of String) = New List(Of String)(4)
        Dim index = 0

        For Each optionElement As XElement In listOfOptionElements

            listOfOptions.Add(optionElement.Value)
            index += 1
        Next

        quizQuestion.Options = listOfOptions
    End Sub



    ''' <summary>
    ''' Retrieve the value of the correct answer index the quiz question XElement, and
    ''' set the value in the quizQuestion model object
    ''' </summary>
    ''' 
    ''' <param name="quizQuestionElement">quiz question element containing
    '''     data of interest, which is the correct answer attribute
    '''  </param>
    '''  
    ''' <param name="quizQuestion">this is the return value of the sub, where
    ''' the CorrectAnswerIndex property will be set</param>
    ''' 
    ''' <throws name="InvalidOperationException">if the quizQuestionElement has
    '''         contains no 'option' element
    '''  </throws>
    Private Shared Sub RetrieveAndSetCorrectAnswerIndex(ByRef xElementObject As XElement,
                                                  ByRef quizQuestion As QuizQuestion)

        Dim listOfOptionElements As IEnumerable(Of XElement) =
                                xElementObject.Descendants("option")

        Dim choicesArray(3) As String
        Dim currentArrayIndex As Integer = 0
        Dim correctAnswerIndex As Integer = -1


        For Each optionElement As XElement In listOfOptionElements

            If String.Compare(optionElement.Attribute("isCorrectAnswer"), "true") = 0 Then
                correctAnswerIndex = currentArrayIndex
                Exit For
            End If

            currentArrayIndex += 1
        Next


        quizQuestion.CorrectAnswerIndex = correctAnswerIndex

    End Sub



End Class

