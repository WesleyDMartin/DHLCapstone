using System.Collections.Generic;

public interface ICulturesAndQuestionsApi
{
    List<Culture> GetCultures();

    List<Question> GetQuestions(Culture culture);
}