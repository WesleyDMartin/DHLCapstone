using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;

public class CulturesAndQuestionsApi : ICulturesAndQuestionsApi
{
    private static string BASE_URL = @"https://nameless-eyrie-58237.herokuapp.com/l";

    public List<Culture> GetCultures()
    {
        throw new System.NotImplementedException();
    }

    public List<Question> GetQuestions(Culture culture)
    {
        throw new System.NotImplementedException();
    }
}
