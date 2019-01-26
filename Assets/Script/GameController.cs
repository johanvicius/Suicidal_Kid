using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameController: MonoBehaviour {
    public string serverIp = "10.4.179.6";
    public int serverPort = 80;
    public string creaEndpoint = "/api/crea";
    public string unirseEndpoint = "/api/unirse";
    public string statusEndpoint = "/api/status";

    // Start is called before the first frame update



    void OnEnable() {
        StartCoroutine(Unirse());
    }

    IEnumerator Unirse() {
        UnityWebRequest www = UnityWebRequest.Post("http://" + serverIp + unirseEndpoint, "");
        yield
        return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError) {
            Debug.Log(www.error);
        } else {
            Debug.Log(www.downloadHandler.text);
        }
    }

    // Update is called once per frame
    void Update() {

    }
}