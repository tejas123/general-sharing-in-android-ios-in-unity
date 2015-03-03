using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;


public class GeneralSharing : MonoBehaviour {
	public Texture2D MyImage;


	public void OnEnable ()
	{
		ScreenshotHandler.ScreenshotFinishedSaving += ScreenshotSaved;
		
	}
	
	void OnDisable()
	{
		ScreenshotHandler.ScreenshotFinishedSaving -= ScreenshotSaved;
		
	}
	#if UNITY_IPHONE
	
	[DllImport("__Internal")]
	private static extern void sampleMethod (string iosPath, string message);
	
	[DllImport("__Internal")]
	private static extern void sampleTextMethod (string message);
	
	#endif

	public void OnAndroidTextSharingClick()
	{
		#if UNITY_ANDROID
		// Create Refernece of AndroidJavaClass class for intent
		AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
		// Create Refernece of AndroidJavaObject class intent
		AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");

		// Set action for intent
		intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
		//Set Subject of action
		intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_SUBJECT"), "Text Sharing ");
		//Set title of action or intent
		intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TITLE"), "Text Sharing ");
		// Set actual data which you want to share
		intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), "Text Sharing Android Demo");


		AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
		// Invoke android activity for passing intent to share data
		currentActivity.Call("startActivity", intentObject);

		
		#endif
		

	}

	public void OnAndroidMediaSharingClick()
	{
		Debug.Log("Media Share");
		StartCoroutine(SaveAndShare());
		
	}

	public void OniOSTextSharingClick()
	{
		
		#if UNITY_IPHONE || UNITY_IPAD
		string shareMessage = "Wow I Just Share Text ";
		sampleTextMethod (shareMessage);
		
		#endif
	}

	public void OniOSMediaSharingClick()
	{
		Debug.Log("Media Share");
#if UNITY_IPHONE
		byte[] bytes = MyImage.EncodeToPNG();
		string path = Application.persistentDataPath + "/MyImage.png";
		File.WriteAllBytes(path, bytes);

		string path_ =  "MyImage.png";


		StartCoroutine(ScreenshotHandler.Save(path_, "Media Share", true));
		
#endif
	}

	IEnumerator SaveAndShare()
	{

		yield return new WaitForEndOfFrame();
		#if UNITY_ANDROID

		byte[] bytes = MyImage.EncodeToPNG();
		string path = Application.persistentDataPath + "/MyImage.png";
		File.WriteAllBytes(path, bytes);

		AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
		AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");

		intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
		intentObject.Call<AndroidJavaObject>("setType", "image/*");
		intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_SUBJECT"), "Media Sharing ");
		intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TITLE"), "Media Sharing ");
		intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), "Media Sharing Android Demo");

		AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");
		AndroidJavaClass fileClass = new AndroidJavaClass("java.io.File");




		AndroidJavaObject fileObject = new AndroidJavaObject("java.io.File", path);// Set Image Path Here

		AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("fromFile", fileObject);

		//			string uriPath =  uriObject.Call<string>("getPath");
		bool fileExist = fileObject.Call<bool>("exists");
		Debug.Log("File exist : " + fileExist);
		if (fileExist)
			intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_STREAM"), uriObject);

		AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
		currentActivity.Call("startActivity", intentObject);
#endif

	}
	void ScreenshotSaved ()
	{

		#if UNITY_IPHONE || UNITY_IPAD
		string shareMessage = "Wow I Just Post Image ";
		sampleMethod (ScreenshotHandler.savedImagePath, shareMessage);
		#endif
		
	}
}
