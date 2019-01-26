using System.Collections;
using System.Timers;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameController: MonoBehaviour {
    public string serverIp = "10.4.179.6";
    public int serverPort = 80;
    public string creaEndpoint = "/api/crea";
    public string unirseEndpoint = "/api/unirse";
    public string statusEndpoint = "/api/status";
    public string accionPosicionEndPoint = "/api/accion/posicion";
    public string accionCogerEndPoint = "/api/accion/coger";
    public string accionDejarEndPoint = "/api/accion/dejar";
    public Transform playerTransform;
    //public JsonObject objetoRecibido;

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
            string recibido = www.downloadHandler.text;
            Debug.Log( recibido );
            AccionPosicion();
            //objetoRecibido = Windows.Data.Json.JsonValue.Parse( recibido ).GetObject();
        }
    }

    public IEnumerator AccionPosicion()
    {
        string jsonEnvio = "{\"ID\":2,\"Transform\":{\"X\":"+playerTransform.position.x+",\"Y\":"+playerTransform.position.y+",\"Z\":"+playerTransform.position.z+"},\"Movil\":true,\"Animacion\":\"\"}";
        Debug.Log(message: jsonEnvio);
        UnityWebRequest www = UnityWebRequest.Post("http://" + serverIp + accionPosicionEndPoint, jsonEnvio);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
        }

        yield return WaitForSeconds(0.5f);
        AccionPosicion();
    }
}