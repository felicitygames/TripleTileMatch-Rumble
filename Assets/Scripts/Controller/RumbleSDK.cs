using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;

[Serializable]
public class PostData
{
	public string session_id;
	public int score;
	public string roomId;
	public User user;
}
[Serializable]
public class User
{
	public string name;
	public string sub;
}
[Serializable]
public class FetchData
{
	public int code;
	public bool success;
	public DataResult data;
}
[Serializable]
public class DataResult
{
	public Metadata[] progress_metadata;
}
[System.Serializable]
public class BalanceResponse
{
	public int code;
	public bool success;
	public ResponseData data;
}
[System.Serializable]
public class ResponseData
{
	public float rumble_balance;
}


[Serializable]
public class Metadata
{
	//All the data you have stored which is to be fetched.
	public int UnlockedAllLevels;
	public int LevelsUnlocked;
	public string unique_match_id;
    public string gameData;
}
[System.Serializable]
public class MyRequestData
{
	public int amount;
	public MyMetaData meta_data;
	public string session_id;
	public string type;
}

[System.Serializable]
public class MyMetaData
{
	public string spent_on;
}
[System.Serializable]
public class Serialization<T>
{
	public T[] Items;
}

[System.Serializable]
public class MyData
{
	//All the data to be stored.
	public int UnlockedAllLevels;
	public int LevelsUnlocked;
	public string unique_match_id;
    public string gameData;
}
public class BodyData
{
	public MyData metadata;
	public string userId;
	public string gameId;
	public string sessionId;
	public string lobbyId;
	public string type;
	public string room_id;
}
public class RumbleSDK : MonoBehaviour
{
	public string gameURL = "localhost:8000?roomDetails=eyJyb29tSWQiOiI1NTA5XzEwOTYiLCJtYXhQbGF5ZXJzIjoyMCwibWluUGxheWVycyI6MTAsIm1heFdhaXQiOjEwLCJyb3VuZHMiOjEwLCJ0ZXh0IjoicGxheV9hZ2FpbiIsImFsbG93Qm90cyI6dHJ1ZSwidXNlciI6eyJuYW1lIjoiU2hydXRoaWs4IiwicGhvdG8iOiJodHRwczovL2Fzc2V0cy1kZXYucnVtYmxlYXBwLmdnL3VzZXItYXZhdGFyLzE4NS8xNjYyMzU5NjU1OTEzLWZfMTY2MjM1OTY1NzAyNS5qcGVnIiwic3ViIjoiMTg1In19&session_id=d89f4fe1-1d95-4e3c-9bdd-dd801729adfa&env=dev&source=cutysvcv167t63t4&gamingEnv=development";

	private string AuthKey = "felicity123!@#";//for dev environment
	private string session_id = "";
	private string lobby_id = "";
	private string game_id = "";
	private string roomDetails = "";
	private string room_id = "";
	private string user_id = "";
	private string env = "";
	private string user_name = "";
	private string user_photo = "";

	public float startTime;
	public PostData _postData;
	public FetchData fetchData;
	public static RumbleSDK instance;

	private void Awake()
	{
    	if (instance == null)
    	{
        	instance = this;
        	DontDestroyOnLoad(this.gameObject);
    	}
    	else
    	{
        	Destroy(this.gameObject);
    	}
	}
	private void Start()
	{
    	#if UNITY_EDITOR
        	FetchDataFromURL();
    	#endif
	}
	public void SetUrl(string url){
		//Debug.Log("URL-"+url);
    	gameURL = url;
    	FetchDataFromURL();
	}
	public void FetchDataFromURL()
	{
        #if !UNITY_EDITOR
            //gameURL = GetURLFromPage();
        #endif
    	////Debug.Log("url>" + gameURL);
    	string queryString = new System.Uri(gameURL).Query;
    	var queryDictionary = System.Web.HttpUtility.ParseQueryString(queryString);
    	//string.Join(string.Empty, gameURL.Split('?').Skip(1));

    	////Debug.Log(queryDictionary + "fetch data>>\t" + gameURL);
 	 
    	startTime = Time.time;
    	UpdateParamsFromURL(gameURL);
	}
  
	// Fetch API URL
	public string FetchApiUrl()
	{
    	string url = "https://api-stage.rumbleapp.gg/"; // stage
    	if (env == "dev")
    	{
        	url = "https://apidevgcp.rumbleapp.gg/"; // dev
        	AuthKey = "felicity123!@#";
    	}
    	else if (env == "prod")
    	{
        	url = "https://api.rumbleapp.gg/api/v1/";
    	}
    	else
    	{
        	AuthKey = "felicity123!@#";//for stage
    	}
    	return url;
	}

 
	// Fetch param values from URL
	public void UpdateParamsFromURL(string current_url)
	{
    	var uri = new Uri(current_url);
    	var queryString = uri.Query;
    	var parameters = System.Web.HttpUtility.ParseQueryString(queryString);

    	session_id = parameters.Get("session_id");
    	//Debug.Log(session_id);
    	roomDetails = parameters.Get("roomDetails");
    	env = parameters.Get("env");

    	byte[] data = Convert.FromBase64String(roomDetails);
   	 string decodeData = System.Text.Encoding.UTF8.GetString(data);
   	 
    	// Parse the JSON data within room_id
    	_postData = JsonUtility.FromJson<PostData>(decodeData);
    	//Debug.Log("jsonData" + _postData.roomId);
    	//Debug.Log("jsonData" + _postData.user);
    	user_id = _postData.user.sub;
    	user_name = _postData.user.name;
    	room_id = _postData.roomId;
    	string[] room_idParts = _postData.roomId.Split('_');
    	lobby_id = room_idParts[0];
    	game_id = room_idParts[1];
    	//Debug.Log("room=>" + room_id);

    	StartCoroutine(GetDataAsync("PROGRESS"));
    	//StartCoroutine(GetDataAsync("PURCHASES"));
    	StartCoroutine(GetRumbleBalanceAsync());
	}
	// Get Purchases or PROGRESS as type
	IEnumerator GetDataAsync(string type)
	{
    	string url = FetchApiUrl() + $"api/v1/transaction/game-developer/get-data?game_id={game_id}&lobby_id={lobby_id}&type={type}&user_id={user_id}&gameplay_id={MakeUniqueId(10)}";
    	//Debug.Log(AuthKey+"getdata url=>" + url);
    	UnityWebRequest www = UnityWebRequest.Get(url);
   	 
    	www.SetRequestHeader("Authorization", AuthKey);

    	yield return www.SendWebRequest();

    	if (www.result != UnityWebRequest.Result.Success)
    	{
        	//Debug.Log($"Error: {www.error}");
			for (int i = 0; i < 18; i++)
			{
				GeneralDataManager.GameData.OpenElementsIndex.Add(i);
			}
				GeneralDataManager.GameData.NewElementOpenCount++;
				GeneralDataManager.Save_Data();
				StartCoroutine(RumbleSDK.instance.SaveDataCoroutine("PROGRESS",JsonConvert.SerializeObject(GeneralDataManager.GameData),PlayerPrefs.GetInt("LevelsUnlocked",1),PlayerPrefs.GetInt("UnlockedAllLevels",1)));

    	}
    	else
    	{
        	// Show results as text
       	 //Debug.Log(www.downloadHandler.text);

        	// Or retrieve results as binary data
        	byte[] results = www.downloadHandler.data;
        	////Debug.Log("res"+results.Length);
        	fetchData = JsonUtility.FromJson<FetchData>(www.downloadHandler.text);	
        	if (fetchData != null && fetchData.data != null && fetchData.data.progress_metadata != null)
        	{	//Debug.Log(fetchData.data.progress_metadata[0].gameData);
            	if (type == "PROGRESS")
            	{
                	int lastIndex = 0;
                	if (lastIndex >= 0)
                	{   
              		// Whatever data you stored are available in (fetchData.data.progress_metadata[lastIndex]).For example - you need to fetch the data like this -
					if(fetchData.data.progress_metadata.Length > 0){
						if(fetchData.data.progress_metadata[lastIndex].gameData != null){
							GeneralDataManager.GameData = JsonConvert.DeserializeObject<GeneralDataManager.Game_Data>(fetchData.data.progress_metadata[lastIndex].gameData);
							PlayerPrefs.SetInt("LevelsUnlocked",fetchData.data.progress_metadata[lastIndex].LevelsUnlocked);
							PlayerPrefs.SetInt("UnlockedAllLevels",fetchData.data.progress_metadata[lastIndex].UnlockedAllLevels);
						}
						else {
								for (int i = 0; i < 18; i++)
								{
									GeneralDataManager.GameData.OpenElementsIndex.Add(i);
								}
									GeneralDataManager.GameData.NewElementOpenCount++;
									GeneralDataManager.Save_Data();
									StartCoroutine(RumbleSDK.instance.SaveDataCoroutine("PROGRESS",JsonConvert.SerializeObject(GeneralDataManager.GameData),PlayerPrefs.GetInt("LevelsUnlocked",1),PlayerPrefs.GetInt("UnlockedAllLevels",1)));
						}
					}
					else {
							for (int i = 0; i < 18; i++)
							{
								GeneralDataManager.GameData.OpenElementsIndex.Add(i);
							}
								GeneralDataManager.GameData.NewElementOpenCount++;
								GeneralDataManager.Save_Data();
								StartCoroutine(RumbleSDK.instance.SaveDataCoroutine("PROGRESS",JsonConvert.SerializeObject(GeneralDataManager.GameData),PlayerPrefs.GetInt("LevelsUnlocked",1),PlayerPrefs.GetInt("UnlockedAllLevels",1)));
					}
        			
                          }
            	else if (type == "PURCHASES")
            	{	 
            	}
        	}
    	}
	}
    }

	public IEnumerator GetRumbleBalanceAsync()
	{
    	string url = FetchApiUrl() + $"api/v1/transaction/game-developer/user-balance?session_id={session_id}";
    	////Debug.Log(url);
    	UnityWebRequest request = UnityWebRequest.Get(url);
    	////Debug.Log(AuthKey);
    	request.SetRequestHeader("Authorization", AuthKey);

    	yield return request.SendWebRequest();

    	if (request.result == UnityWebRequest.Result.Success)
    	{
        	string data = request.downloadHandler.text;
        	BalanceResponse balanceResponse = JsonUtility.FromJson<BalanceResponse>(data);
        	float balance = balanceResponse.data.rumble_balance;
        	PlayerPrefs.SetFloat("RumbleBalance",balance);
        	//Debug.Log($"Rumble Balance: {balance}");
       	 
    	}
    	else
    	{
        	string data = request.downloadHandler.text;
        	//Debug.Log("Data"+data);
        	Debug.LogError($"Error: {request.responseCode} - {request.error}");
    	}
	}

	// Update Balance
	public IEnumerator UpdateBalanceAsync(int amount,string spent_on1)
	{	
    	PlayerPrefs.SetString("RewardType",spent_on1);
    	string url = FetchApiUrl() + "api/v1/transaction/game-developer/user-balance";

    	Dictionary<string, string> headers = new Dictionary<string, string>
    	{
        	{ "accept", "*/*" },
        	{ "Authorization", AuthKey },
        	{ "Content-Type", "application/json" }
    	};

    	var requestData = new MyRequestData
    	{
        	amount = amount,
        	meta_data = new MyMetaData
        	{
            	spent_on = spent_on1,
        	},
        	session_id = session_id,
        	type = "DEBIT"
    	};
    	////Debug.Log(AuthKey);
   	 
    	string body = JsonUtility.ToJson(requestData);
    	////Debug.Log(body);
    	UnityWebRequest request = UnityWebRequest.Put(url, body);
    	request.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(body));
    	request.downloadHandler = new DownloadHandlerBuffer();

    	foreach (var header in headers)
    	{
        	request.SetRequestHeader(header.Key, header.Value);
    	}

    	yield return request.SendWebRequest();

    	if (request.result == UnityWebRequest.Result.Success)
    	{	
        	if(PlayerPrefs.GetString("RewardType") == "IncreaseCoinRewardAd"){
                	PlayerPrefs.DeleteKey("RewardType");
					PlayerPrefs.SetInt("IncreaseCoinReward",1);
        	}
        	else if(PlayerPrefs.GetString("RewardType") == "LevelCompleteRewardAd"){
            	PlayerPrefs.DeleteKey("RewardType");
				PlayerPrefs.SetInt("LevelCompleteReward",1);
            }   
			else if(PlayerPrefs.GetString("RewardType") == "PowerUpRewardAd"){
            	PlayerPrefs.DeleteKey("RewardType");
				PlayerPrefs.SetInt("PowerUpReward",1);
            } 
			else if(PlayerPrefs.GetString("RewardType") == "GameOverRewardAd"){
            	PlayerPrefs.DeleteKey("RewardType");
				PlayerPrefs.SetInt("GameOverReward",1);
            } 
			else if(PlayerPrefs.GetString("RewardType") == "DailyRewardAd"){
            	PlayerPrefs.DeleteKey("RewardType");
				PlayerPrefs.SetInt("DailyReward",1);
            } 
			else if(PlayerPrefs.GetString("RewardType") == "LevelChestRewardAd"){
            	PlayerPrefs.DeleteKey("RewardType");
				PlayerPrefs.SetInt("LevelChestReward",1);
            }  
			else if(PlayerPrefs.GetString("RewardType") == "StarChestRewardAd"){
            	PlayerPrefs.DeleteKey("RewardType");
				PlayerPrefs.SetInt("StarChestReward",1);
            }
			else if(PlayerPrefs.GetString("RewardType") == "LevelsUnlockedAd"){
            	PlayerPrefs.DeleteKey("RewardType");
				PlayerPrefs.SetInt("LevelsUnlocked",0);
				PlayerPrefs.Save();
				StartCoroutine(RumbleSDK.instance.SaveDataCoroutine("PROGRESS",JsonConvert.SerializeObject(GeneralDataManager.GameData),PlayerPrefs.GetInt("LevelsUnlocked",1),PlayerPrefs.GetInt("UnlockedAllLevels",1)));
            }	
        	//Debug.Log("Balance updated successfully");
        	StartCoroutine(GetRumbleBalanceAsync());
    	}
    	else
    	{   string data = request.downloadHandler.text;
        	////Debug.Log("Data"+data);
        	Debug.LogError($"Failed to update balance: {request.responseCode} - {request.error}");
    	}
	}

	public IEnumerator SaveDataCoroutine(string type, string game_data,int LevelsUnlocked,int UnlockedAllLevels)
	{
    	string url = FetchApiUrl() + "api/v1/transaction/game-developer/save-data";
    	string body = "";
    	if(type == "PROGRESS"){
        	var bodyData = new BodyData
        	{
            	metadata = new MyData
            	{
					//set your metadata class variable to the parameter you passed. For example-
					LevelsUnlocked = LevelsUnlocked,
					UnlockedAllLevels = UnlockedAllLevels,
                	gameData = game_data,
                	unique_match_id = MakeUniqueId(10)
            	},
            	userId = user_id,
            	gameId = game_id,
            	sessionId = session_id,
            	lobbyId = lobby_id,
            	type = type,
            	room_id = room_id
        	};
        	body = JsonConvert.SerializeObject(bodyData);
    	}
    	long timeStamp = DateTimeOffset.Now.ToUnixTimeSeconds();
    	string new_body = body + "|" + timeStamp;
		string encrypted_body = Convert.ToBase64String(Encoding.UTF8.GetBytes(new_body));
    	using (var www = new UnityWebRequest(url, "POST"))
    	{
        	var json = JsonConvert.SerializeObject(new { message = encrypted_body, timestamp = timeStamp });
        	//Debug.Log(url+"send json" + json);
        	//var json = "{\"session_id\": \"1ff47f0d-7ae3-4385-8ed4-efc1d8f04b17\", \"score\" : 102, \"session_duration\" : 12}";
        	var jsonBytes = Encoding.UTF8.GetBytes(json);
        	www.downloadHandler = new DownloadHandlerBuffer();
        	www.uploadHandler = new UploadHandlerRaw(jsonBytes);
        	www.SetRequestHeader("Content-Type", "application/json");
        	//www.SetRequestHeader("Accept", " text/plain");
        	www.SetRequestHeader("accept", "*/*");
        	www.SetRequestHeader("Authorization", AuthKey);
        	//httpClient.DefaultRequestHeaders.Add("Content-Type", "application/json");

        	var content = new StringContent(JsonUtility.ToJson(new { message = encrypted_body, timestamp = timeStamp }), Encoding.UTF8, "application/json");
        	//var response = await httpClient.PostAsync(url, content);
       	 
        	yield return www.SendWebRequest();
        	if (www.result != UnityWebRequest.Result.Success)
        	{
            	//Debug.Log($"Failed to save data: {www.error}");
           	 
        	}
        	else
        	{
            	//Debug.Log("Data saved successfully");
				StartCoroutine(GetDataAsync("PROGRESS"));
        	}
        	//Debug.Log("PURCHASES"+www.result);
    	}
	}

	public void OnRewardLvlAdButton()
	{	
    	StartCoroutine(PushSocketAsync("RV"));
		PlayerPrefs.SetInt("InitialMusic",PlayerPrefs.GetInt("Music"));
        PlayerPrefs.SetInt("InitialSound",PlayerPrefs.GetInt("Sound"));
        SoundManager.Inst.IsMusicOn = false;
        SoundManager.Inst.IsSoundEffectsOn = false;
        if (SoundManager.Inst.IsMusicOn)
            SoundManager.Inst.Play("bg_sound", true);
        else
            SoundManager.Inst.Stop("bg_sound");
	}

	public void OnRewardAdButton()
	{	
    	StartCoroutine(PushSocketAsync("RV"));
		PlayerPrefs.SetInt("InitialMusic",PlayerPrefs.GetInt("Music"));
        PlayerPrefs.SetInt("InitialSound",PlayerPrefs.GetInt("Sound"));
        SoundManager.Inst.IsMusicOn = false;
        SoundManager.Inst.IsSoundEffectsOn = false;
        if (SoundManager.Inst.IsMusicOn)
            SoundManager.Inst.Play("bg_sound", true);
        else
            SoundManager.Inst.Stop("bg_sound");
	}
	public void OnIAPButton()
	{    
    	StartCoroutine(PushSocketAsync("IAP"));
	}
	public void OnIAPNewButton()
	{    
    	StartCoroutine(PushSocketAsync("IAP_PACKAGE",1));
		PlayerPrefs.SetInt("InitialMusic",PlayerPrefs.GetInt("Music"));
        PlayerPrefs.SetInt("InitialSound",PlayerPrefs.GetInt("Sound"));
        SoundManager.Inst.IsMusicOn = false;
        SoundManager.Inst.IsSoundEffectsOn = false;
        if (SoundManager.Inst.IsMusicOn)
            SoundManager.Inst.Play("bg_sound", true);
        else
            SoundManager.Inst.Stop("bg_sound");
	}
	public void OnIAPUnlockButton()
	{   PlayerPrefs.SetString("IAPType","UnlockAllLevelsAd"); 
    	StartCoroutine(PushSocketAsync("IAP_PACKAGE",43));
		PlayerPrefs.SetInt("InitialMusic",PlayerPrefs.GetInt("Music"));
        PlayerPrefs.SetInt("InitialSound",PlayerPrefs.GetInt("Sound"));
        SoundManager.Inst.IsMusicOn = false;
        SoundManager.Inst.IsSoundEffectsOn = false;
        if (SoundManager.Inst.IsMusicOn)
            SoundManager.Inst.Play("bg_sound", true);
        else
            SoundManager.Inst.Stop("bg_sound");
	}

	public void OnResumeGame()
	{    //StartCoroutine(GetRumbleBalanceAsync());
		if(PlayerPrefs.GetInt("InitialMusic") == 1){
            SoundManager.Inst.IsMusicOn = true;
			if (SoundManager.Inst.IsMusicOn)
				SoundManager.Inst.Play("bg_sound", true);
			else
				SoundManager.Inst.Stop("bg_sound");
			}
		if(PlayerPrefs.GetInt("InitialSound") == 1){
			SoundManager.Inst.IsSoundEffectsOn = true;
		}
	}
	// Push Socket - To watch rewarded ad or let users buy rumbles
	IEnumerator PushSocketAsync(string type, int packageId = 1)
	{
    	string url = FetchApiUrl() + "api/v1/transaction/game-developer/push-socket";
		string body = JsonConvert.SerializeObject(new { sessionId = session_id, type });
		if(type == "IAP_PACKAGE")
    	body = JsonConvert.SerializeObject(new { sessionId = session_id, type, packageId, purchaseTransactionId = MakeUniqueId(10)});
    	long timeStamp = DateTimeOffset.Now.ToUnixTimeSeconds();
    	string new_body = body + "|" + timeStamp;
    	string encrypted_body = Convert.ToBase64String(Encoding.UTF8.GetBytes(new_body));
    	// Debug.Log("push>" + body);
    	// Debug.Log("push>" + encrypted_body);
    	using (var www = new UnityWebRequest(url, "POST"))
    	{
        	www.SetRequestHeader("accept", "*/*");
        	www.SetRequestHeader("Authorization", AuthKey);
        	www.SetRequestHeader("Content-Type", "application/json");
        	var json = JsonConvert.SerializeObject(new { message = encrypted_body, timestamp = timeStamp });
        	//var content = new StringContent(JsonConvert.SerializeObject(new { message = encrypted_body, timestamp = timeStamp }), Encoding.UTF8, "application/json");
        	var jsonBytes = Encoding.UTF8.GetBytes(json);
        	www.downloadHandler = new DownloadHandlerBuffer();
        	www.uploadHandler = new UploadHandlerRaw(jsonBytes);
        	yield return www.SendWebRequest();

        	if (www.result == UnityWebRequest.Result.Success)
        	{
            	Debug.Log("PushSocket success");
				if(PlayerPrefs.GetString("RewardAdType") == "LevelsUnlockedAd"){
					PlayerPrefs.DeleteKey("RewardAdType");
					PlayerPrefs.SetInt("LevelsUnlocked",0);
					PlayerPrefs.Save();
					StartCoroutine(RumbleSDK.instance.SaveDataCoroutine("PROGRESS",JsonConvert.SerializeObject(GeneralDataManager.GameData),PlayerPrefs.GetInt("LevelsUnlocked",1),PlayerPrefs.GetInt("UnlockedAllLevels",1)));
            	}
				if(PlayerPrefs.GetString("IAPType") == "UnlockAllLevelsAd"){
					PlayerPrefs.DeleteKey("IAPType");
					PlayerPrefs.SetInt("UnlockedAllLevels",0);
					PlayerPrefs.Save();
					StartCoroutine(RumbleSDK.instance.SaveDataCoroutine("PROGRESS",JsonConvert.SerializeObject(GeneralDataManager.GameData),PlayerPrefs.GetInt("LevelsUnlocked",1),PlayerPrefs.GetInt("UnlockedAllLevels",1)));
            	}
        	}
        	else
        	{
            	Debug.Log($"PushSocket failed: {www.error}");
        	}
    	}
	}
	// Make Unique ID
	public string MakeUniqueId(int length)
	{
    	string result = "";
    	const string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
    	int charactersLength = characters.Length;

    	for (int i = 0; i < length; i++)
    	{
        	result += characters[new System.Random().Next(charactersLength)];
    	}

    	return result;
	}
}