using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

public class RiddleSystem : MonoBehaviour
{
    private const string PROMPT = "Give me a unique riddle and its answer in this format: Riddle: <riddle> Answer: <answer>";
    private const string API_KEY = "api_key";
    private const string API_URL = "https://api.openai.com/v1/chat/completions";

    private string currentRiddle = "";
    private string currentAnswer = "";

    private class ChatGptReq
    {
        public string model;
        public ChatGptMessage[] messages;
        public int max_tokens;
    }

    private class ChatGptMessage
    {
        public string role;
        public string content;
    }

    public IEnumerator GetRiddle()
    {
        var requestData = new ChatGptReq
        {
            model = "gpt-4o",
            messages = new ChatGptMessage[]
            {
                new(){ role = "system", content = "You are a sinister old man that gives riddles." },
                new(){ role = "user", content = PROMPT }
            },
            max_tokens = 150
        };

        string jsonData = JsonConvert.SerializeObject(requestData);

        using UnityWebRequest request = new(API_URL, "POST");
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
            
            string text = jsonResponse["choices"][0]["message"]["content"].ToString();

            // Extract riddle and answer
            int riddleStart = text.IndexOf("Riddle:") + 7;
            int answerStart = text.IndexOf("Answer:");
            currentRiddle = text[riddleStart..answerStart].Trim();
            currentAnswer = text[(answerStart + 7)..].Trim();

            Debug.Log($"Riddle: {currentRiddle}");
            Debug.Log($"Answer: {currentAnswer}");
        }
        else
        {
            Debug.LogError("Error: " + request.error);
        }
    }
}
