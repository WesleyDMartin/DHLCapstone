using System;
using System.Collections.Generic;

public interface ICulturesAndQuestionsApi
{
    List<string> GetQuestions(string culture);

    List<string> GetCultures();
}