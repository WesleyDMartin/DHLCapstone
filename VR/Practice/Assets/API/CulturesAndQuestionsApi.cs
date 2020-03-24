using RestSharp;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;

public class CulturesAndQuestionsApi : ICulturesAndQuestionsApi
{
    //private static string BASE_URL = @"https://nameless-eyrie-58237.herokuapp.com/";
    private static string BASE_URL = @"http://192.168.0.117:3000/";

    public List<string> GetCultures()
    {
        var ret = new List<string>();
        var client = new RestClient(BASE_URL + "cultures");

        var request = new RestRequest("", Method.GET);
        // Add HTTP headers
        request.AddHeader("User-Agent", "Nothing");

        // Execute the request and automatically deserialize the result.
        var cultures = client.Execute<List<Culture>>(request);
        cultures.Data.ForEach(x => ret.Add(x.name));

        return ret;
    }

    public List<Question> GetQuestions(string culture = "")
    {
        var ret = new List<Question>();
        RestClient client;
        client = culture != string.Empty
            ? new RestClient(BASE_URL + "questions?culture=" + culture)
            : new RestClient(BASE_URL + "questions");

        var request = new RestRequest("", Method.GET);
        // Add HTTP headers
        request.AddHeader("User-Agent", "Nothing");

        // Execute the request and automatically deserialize the result.
        var questions = client.Execute<List<Question>>(request);
        questions.Data.ForEach(x => ret.Add(x));

        return ret;
    }
}
