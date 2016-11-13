using HoloToolkit;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;
using HoloToolkit.Unity;

//using UnityEngine.Generic;

public class MicrophoneManager : MonoBehaviour
{
    [Tooltip("A text area for the recognizer to display the recognized strings.")]
    public Text DictationDisplay;
    public Dictionary<string, string> headers;
    public string first, second, third, website;
    public string username, password;
    public bool web;
    private DictationRecognizer dictationRecognizer;

    // Use this string to cache the text currently displayed in the text box.
    private StringBuilder textSoFar;

    // Using an empty string specifies the default microphone. 
    private static string deviceName = string.Empty;
    private int samplingRate;
    private const int messageLength = 10;

    // Use this to reset the UI once the Microphone is done recording after it was started.
    private bool hasRecordingStarted;
    WWWForm form;

    byte[] rawData;
    WWW www;
    void Awake()
    {
        /* TODO: DEVELOPER CODING EXERCISE 3.a */
        web = false;
        // 3.a: Create a new DictationRecognizer and assign it to dictationRecognizer variable.
        dictationRecognizer = new DictationRecognizer();

        // 3.a: Register for dictationRecognizer.DictationHypothesis and implement DictationHypothesis below
        // This event is fired while the user is talking. As the recognizer listens, it provides text of what it's heard so far.
        dictationRecognizer.DictationHypothesis += DictationRecognizer_DictationHypothesis;

        // 3.a: Register for dictationRecognizer.DictationResult and implement DictationResult below
        // This event is fired after the user pauses, typically at the end of a sentence. The full recognized string is returned here.
        dictationRecognizer.DictationResult += DictationRecognizer_DictationResult;

        // 3.a: Register for dictationRecognizer.DictationComplete and implement DictationComplete below
        // This event is fired when the recognizer stops, whether from Stop() being called, a timeout occurring, or some other error.
        dictationRecognizer.DictationComplete += DictationRecognizer_DictationComplete;

        // 3.a: Register for dictationRecognizer.DictationError and implement DictationError below
        // This event is fired when an error occurs.
        dictationRecognizer.DictationError += DictationRecognizer_DictationError;

        // Query the maximum frequency of the default microphone. Use 'unused' to ignore the minimum frequency.
        int unused;
        Microphone.GetDeviceCaps(deviceName, out unused, out samplingRate);

        // Use this string to cache the text currently displayed in the text box.
        textSoFar = new StringBuilder();

        // Use this to reset the UI once the Microphone is done recording after it was started.
        hasRecordingStarted = false;

        form = new WWWForm();
        headers = form.headers;
        //var headers = new Hashtable();
        username = "8a905ee6-6fab-4a28-80cd-8ea5ec1e8fd5";
        password = "urgjXadoRbR1";

        form.AddField(username, password);
        rawData = form.data;
    }

    void Update()
    {

        //  Debug.Log("out if");
        // 3.a: Add condition to check if dictationRecognizer.Status is Running
        // first = "https://8a905ee6-6fab-4a28-80cd-8ea5ec1e8fd5:urgjXadoRbR1@gateway.watsonplatform.net/retrieve-and-rank/api/v1/solr_clusters/sc75ab4474_e7c4_48ae_adec_07dc35726001/solr/sahil_collection/select?q=";
        // second = textSoFar.ToString();
        // third = "&wt=json&fl=id,title";
        //website = first + second + third;
        //InteractibleManager.Instance.websiteText.GetComponent<TextMesh>().text = website;

        /*WWW www = null; 
        if (web == false)
        {
        
            www = new WWW("https://8a905ee6-6fab-4a28-80cd-8ea5ec1e8fd5:urgjXadoRbR1@gateway.watsonplatform.net/retrieve-and-rank/api/v1/solr_clusters/sc75ab4474_e7c4_48ae_adec_07dc35726001/solr/sahil_collection/select?q=what%20is%20supersonic%20flow&wt=json&fl=id,title");
            web = true;
        }
        if (www != null && www.isDone)
        {
            
            InteractibleManager.Instance.websiteText.GetComponent<TextMesh>().text = "Hello text " + www.text + " end text";
        }*/
        if (web == false)
        {
           
            if (www != null && www.isDone)
            {
                int index = www.text.IndexOf(",\"title\":[");
                int end_index = www.text.IndexOf("]}");
                string output = www.text.Substring(index + 11, end_index - 3 - (index + 11));
                InteractibleManager.Instance.websiteText.GetComponent<TextMesh>().text = output;
                if (output == null)
                {
                    TextToSpeechManager.Instance.SpeakText("Frank doesn't know the answer to that question");
                }
                else
                {
                    TextToSpeechManager.Instance.SpeakText("Frank found the answer as ");
                    TextToSpeechManager.Instance.SpeakText(output);
                  //  TextToSpeechManager.Instance.SpeakText("yes");
                }
                web = true;
                www = null;
            } 
        }

        if (hasRecordingStarted && !Microphone.IsRecording(deviceName) && dictationRecognizer.Status == SpeechSystemStatus.Running)
        {
            Debug.Log("in if");
            // Reset the flag now that we're cleaning up the UI.
            hasRecordingStarted = false;

            // This acts like pressing the Stop button and sends the message to the Communicator.
            // If the microphone stops as a result of timing out, make sure to manually stop the dictation recognizer.
            // Look at the StopRecording function.
            SendMessage("RecordStop");
        }
    }

    /// <summary>
    /// Turns on the dictation recognizer and begins recording audio from the default microphone.
    /// </summary>
    /// <returns>The audio clip recorded from the microphone.</returns>
    public AudioClip StartRecording()
    {
        Debug.Log("AudioClip");
        // 3.a Shutdown the PhraseRecognitionSystem. This controls the KeywordRecognizers
        PhraseRecognitionSystem.Shutdown();

        // 3.a: Start dictationRecognizer
        dictationRecognizer.Start();

        // 3.a Uncomment this line
        DictationDisplay.text = "Dictation is starting. It may take time to display your text the first time, but begin speaking now...";

        // Set the flag that we've started recording.
        hasRecordingStarted = true;

        // Start recording from the microphone for 10 seconds.
        return Microphone.Start(deviceName, false, messageLength, samplingRate);
    }

    /// <summary>
    /// Ends the recording session.
    /// </summary>
    public void StopRecording()
    {
        Debug.Log("stop recording");

        // 3.a: Check if dictationRecognizer.Status is Running and stop it if so
        if (dictationRecognizer.Status == SpeechSystemStatus.Running)
        {
            dictationRecognizer.Stop();
        }

        Microphone.End(deviceName);
    }

    /// <summary>
    /// This event is fired while the user is talking. As the recognizer listens, it provides text of what it's heard so far.
    /// </summary>
    /// <param name="text">The currently hypothesized recognition.</param>
    private void DictationRecognizer_DictationHypothesis(string text)
    {
        Debug.Log("diction recognizer");
        // 3.a: Set DictationDisplay text to be textSoFar and new hypothesized text
        // We don't want to append to textSoFar yet, because the hypothesis may have changed on the next event
        DictationDisplay.text = textSoFar.ToString() + " " + text + "...";
    }

    /// <summary>
    /// This event is fired after the user pauses, typically at the end of a sentence. The full recognized string is returned here.
    /// </summary>
    /// <param name="text">The text that was heard by the recognizer.</param>
    /// <param name="confidence">A representation of how confident (rejected, low, medium, high) the recognizer is of this recognition.</param>
    private void DictationRecognizer_DictationResult(string text, ConfidenceLevel confidence)
    {
        form = new WWWForm();
        string first, second, third, url;
        headers = form.headers;
        //var headers = new Hashtable();
        username = "8a905ee6-6fab-4a28-80cd-8ea5ec1e8fd5";
        password = "urgjXadoRbR1";

        form.AddField(username, password);
        rawData = form.data;
        first = "https://gateway.watsonplatform.net/retrieve-and-rank/api/v1/solr_clusters/sc75ab4474_e7c4_48ae_adec_07dc35726001/solr/sahil_collection/select?q=";
        second = textSoFar.ToString();
        second = second.Replace(" ", "%20");
        third = "&wt=json&fl=id,title";
        url = first + second + third;
        web = false;
      //  string url = "what%20is%20supersonic%20flow";

        // Add a custom header to the request.
        // In this case a basic authentication to access a password protected resource.
        headers["Authorization"] = "Basic " + System.Convert.ToBase64String(
            System.Text.Encoding.ASCII.GetBytes(username + ":" + password));

        // Post a request to an URL with our custom headers

        www = new WWW(url, rawData, headers);


        Debug.Log("diction result");
        // 3.a: Append textSoFar with latest text
        textSoFar.Append(text + ". ");
        Debug.Log("text" + text);
        // 3.a: Set DictationDisplay text to be textSoFar
        DictationDisplay.text = textSoFar.ToString();


        /*WebRequest webRequest = WebRequest.Create(website);
        WebResponse webResp = webRequest.GetResponse();*/


    }

    /// <summary>
    /// This event is fired when the recognizer stops, whether from Stop() being called, a timeout occurring, or some other error.
    /// Typically, this will simply return "Complete". In this case, we check to see if the recognizer timed out.
    /// </summary>
    /// <param name="cause">An enumerated reason for the session completing.</param>
    private void DictationRecognizer_DictationComplete(DictationCompletionCause cause)
    {
        Debug.Log("dictation complete  " + cause);
        web = true;
        www = null;
        // If Timeout occurs, the user has been silent for too long.
        // With dictation, the default timeout after a recognition is 20 seconds.
        // The default timeout with initial silence is 5 seconds.
        if (cause == DictationCompletionCause.TimeoutExceeded)
        {
            Microphone.End(deviceName);

            DictationDisplay.text = "Dictation has timed out. Please press the record button again.";
            SendMessage("ResetAfterTimeout");
        }

    }

    /// <summary>
    /// This event is fired when an error occurs.
    /// </summary>
    /// <param name="error">The string representation of the error reason.</param>
    /// <param name="hresult">The int representation of the hresult.</param>
    private void DictationRecognizer_DictationError(string error, int hresult)
    {
        web = true;
        www = null;
        Debug.Log("dictation error");
        // 3.a: Set DictationDisplay text to be the error string
        DictationDisplay.text = error + "\nHRESULT: " + hresult;
    }

    private IEnumerator RestartSpeechSystem(KeywordManager keywordToStart)
    {


        Debug.Log("restart");
        while (dictationRecognizer != null && dictationRecognizer.Status == SpeechSystemStatus.Running)
        {
            yield return null;
        }

        keywordToStart.StartKeywordRecognizer();
    }
}
