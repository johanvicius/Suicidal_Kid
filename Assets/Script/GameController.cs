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

    private Timer timer1;
    private int timer1interval = 200;

    // Start is called before the first frame update



    void OnEnable() {
        StartCoroutine(Unirse());
        //StartCoroutine(AutoupdateAccionPosicion());
        AutoupdateAccionPosicion();
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
            //objetoRecibido = Windows.Data.Json.JsonValue.Parse( recibido ).GetObject();
        }
    }

    public void AutoupdateAccionPosicion()
    {
        timer1 = new Timer();
        timer1.Elapsed += new ElapsedEventHandler( RepetidorAccionPosicion );
        timer1.Enabled = true;
        timer1.Interval = 10 ; // in miliseconds
        timer1.Start();
        
    }

    public void RepetidorAccionPosicion(object source, ElapsedEventArgs e)
    {
        AccionPosicion();
    }
    public void AccionPosicion()
    {
        string jsonEnvio = "{\"ID\":2,\"Transform\":{\"X\":1,\"Y\":2,\"Z\":3},\"Movil\":true,\"Animacion\":\"\"}";
        Debug.Log(message: jsonEnvio);
        UnityWebRequest www = UnityWebRequest.Post("http://" + serverIp + accionPosicionEndPoint, jsonEnvio);
        www.SendWebRequest();
        Debug.Log(www.downloadHandler.text);
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
        }
    }



    // Update is called once per frame
    void Update() {

    }
}