using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

public class ChatGptReq
{
    public string model;
    public ChatGptMessage[] messages;
    public int max_tokens;
}

public class ChatGptMessage
{
    public string role;
    public string content;
}

public class ChatGptRiddle
{
    public string Riddle;
    public string Answer;
}

public static class RiddleUtils
{
    private const string SYSTEM_PROMPT = "You are a sinister old man that gives riddles.";
    private const string RIDDLE_PROMPT = "Give me a unique riddle and its answer in this format: Riddle: <riddle> Answer: <answer>";
    private const string API_KEY = "api_key";
    private const string API_URL = "https://api.openai.com/v1/chat/completions";

    public static IEnumerator GenerateRiddle(ChatGptRiddle chatGptRiddle)
    {
        var requestData = new ChatGptReq
        {
            model = "gpt-4o",
            messages = new ChatGptMessage[]
            {
                new(){ role = "system", content = SYSTEM_PROMPT },
                new(){ role = "user", content = RIDDLE_PROMPT }
            },
            max_tokens = 150
        };

        var jsonData = JsonConvert.SerializeObject(requestData);

        using UnityWebRequest request = new(API_URL, "POST");
        var bodyRaw = Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + API_KEY);

        yield return request.SendWebRequest();
        
        if (request.result == UnityWebRequest.Result.Success)
        {
            string response = request.downloadHandler.text;
            JObject jsonResponse = JObject.Parse(response);
            
            string text = jsonResponse["choices"][0]["message"]["content"].ToString();

            // Extract riddle and answer
            int riddleStart = text.IndexOf("Riddle:") + 7;
            int answerStart = text.IndexOf("Answer:");
            chatGptRiddle.Riddle = text[riddleStart..answerStart].Trim();
            chatGptRiddle.Answer = text[(answerStart + 7)..].Trim();
        }
        else
        {
            Debug.LogError("Error: " + request.error);
        }
    }

    public static IEnumerator CheckAnswer(ChatGptRiddle chatGptRiddle, string playerAnswer, bool result)
    {
        var prompt = $"I asked the player this riddle: \"{chatGptRiddle.Riddle}\". The correct answer is \"{chatGptRiddle.Answer}\". The player answered: \"{playerAnswer}\". Is their answer correct? Reply with only 'Yes' or 'No'.";

        var requestData = new
        {
            model = "gpt-4-turbo",
            messages = new[]
            {
                new { role = "system", content = SYSTEM_PROMPT },
                new { role = "user", content = prompt }
            },
            max_tokens = 5
        };

        string jsonData = JsonUtility.ToJson(requestData);

        using (UnityWebRequest request = new UnityWebRequest(API_URL, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", "Bearer " + API_KEY);

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string response = request.downloadHandler.text;
                JObject jsonResponse = JObject.Parse(response);
                string aiResponse = jsonResponse["choices"][0]["message"]["content"].ToString().Trim();

                if (aiResponse.ToLower() == "yes")
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            else
            {
                result = false;
                Debug.LogError("Error: " + request.error);
            }
        }
    }
}
