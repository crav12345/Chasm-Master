using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;

public class GetRiddleResponse
{
    public string RiddleId;
    public string Riddle;
    public string Answer;
}

public class PostAnswerRequest
{
    public string RiddleId;
    public string Answer;
}

public class PostAnswerResponse
{
    public bool Correct;
}

/// <summary>
/// Static class for retrieving riddles and evaluating user responses. This is
/// done with OpenAI's GPT API.
/// </summary>
public static class RiddleUtils
{
    private const string CHASM_MASTER_API_URL = "https://chasm-master-api-server-690085889009.us-east4.run.app/api";

    public static IEnumerator GetRiddle(Action<GetRiddleResponse> callback)
    {
        var response = new GetRiddleResponse();
        var url = $"{CHASM_MASTER_API_URL}/riddles/";

        using UnityWebRequest request = new(url, "GET");
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            var responseJson = request.downloadHandler.text;   
            response = JsonConvert.DeserializeObject<GetRiddleResponse>(responseJson);
        }
        else
        {
            Debug.LogError("Error: " + request.error);
        }

        callback?.Invoke(response);
    }

    public static IEnumerator CheckAnswer(string riddleId, string playerAnswer, Action<bool> callback)
    {
        var response = false;
        var url = $"{CHASM_MASTER_API_URL}/answers/";
        var bodyData = new PostAnswerRequest
        {
            RiddleId = riddleId,
            Answer = playerAnswer
        };

        var bodyJson = JsonConvert.SerializeObject(bodyData);
        using UnityWebRequest request = new(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJson);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            var responseJson = request.downloadHandler.text;
            var responseObj = JsonConvert.DeserializeObject<PostAnswerResponse>(responseJson);
            
            response = responseObj.Correct;
        }
        else
        {
            Debug.LogError("Error: " + request.error);
        }

        callback?.Invoke(response);
    }
}
