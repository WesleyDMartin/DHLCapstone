using System.Collections.Generic;

public interface ICulturesAndQuestionsApi
{
    List<string> GetCultures();

    List<Question> GetQuestions(string culture);
}