using System.Collections;
using System.Timers;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameController: MonoBehaviour {
    public static int maxBabiesID = 100;
    public string serverIp = "10.4.179.6";
    public int serverPort = 80;
    public string creaEndpoint = "/api/crea";
    public string unirseEndpoint = "/api/unirse";
    public string statusEndpoint = "/api/status";
    public string accionPosicionEndPoint = "/api/accion/posicion";
    public string accionCogerEndPoint = "/api/accion/coger";
    public string accionDejarEndPoint = "/api/accion/dejar";
    public GameObject FatherPrefab;
    public GameObject BabyPrefab;
    public Transform playerTransform;
    //public JsonObject objetoRecibido;
    private int myID = 0;
    private GameObject[] babies = new GameObject[maxBabiesID];
    private GameObject father = null;
    private StatusModel status;

    void OnEnable() {
        StartCoroutine(Unirse());
    }

    IEnumerator Unirse() {
        UnityWebRequest www = UnityWebRequest.Get("http://" + serverIp + unirseEndpoint);
        yield
        return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError) {
            //Debug.Log(www.error);
        } else {
            status = JsonUtility.FromJson<StatusModel>(www.downloadHandler.text);
            myID = status.elementos;
            yield return AccionPosicion();
            //objetoRecibido = Windows.Data.Json.JsonValue.Parse( recibido ).GetObject();
        }
    }

    public void InstantiateBaby(BabyModel baby) {
        GameObject tmpBaby = Object.Instantiate(BabyPrefab, new Vector3(baby.Transform.X, baby.Transform.Y, baby.Transform.Z), Quaternion.identity, this.transform);
        babies[baby.ID] = tmpBaby;
    }

    public void InstantiateFather(FatherModel tmpFather){
        father = Object.Instantiate(FatherPrefab, new Vector3(tmpFather.Transform.X, tmpFather.Transform.Y, tmpFather.Transform.Z), Quaternion.identity, this.transform);
    }

    public void MoveBaby(BabyModel baby) {
        babies[baby.ID].transform.position = new Vector3(baby.Transform.X, baby.Transform.Y, baby.Transform.Z);
    }

    public void MoveFather(FatherModel tmpFather) {
        father.transform.position = new Vector3(tmpFather.Transform.X, tmpFather.Transform.Y, tmpFather.Transform.Z);
    }

    public void ProcessStatus() {
        Debug.Log("Prcessign status, child id: "+myID);
        //GameObject flor = Object.Instantiate(flores[tipo], new Vector3(posicion.x, 1f, posicion.y), Quaternion.identity, this.transform);
        foreach(BabyModel tmpBaby in status.Hijos)
        {
            if(tmpBaby.ID != myID) {
                Debug.Log("Precessing child: "+tmpBaby.ID);
                if(babies[tmpBaby.ID] == null) {
                    InstantiateBaby(tmpBaby);
                } else {
                    MoveBaby(tmpBaby);
                }
            }
        }

        if(father == null) {
            InstantiateFather(status.Padre);
        } else {
            MoveFather(status.Padre);
        }
    }

    public IEnumerator AccionPosicion()
    {
        //Debug.Log("AccionPosicion");
        string jsonEnvio = "{\"ID\":"+myID+",\"Transform\":{\"X\":\""+playerTransform.position.x+"\",\"Y\":\""+playerTransform.position.y+"\",\"Z\":\""+playerTransform.position.z+"\"},\"Movil\":true,\"Animacion\":\"\"}";
        //Debug.Log(jsonEnvio);
        UnityWebRequest www = UnityWebRequest.Post("http://" + serverIp + accionPosicionEndPoint, jsonEnvio);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            //Debug.Log(www.error);
        }
        else
        {
            status = JsonUtility.FromJson<StatusModel>(www.downloadHandler.text);
            ProcessStatus();
        }
        //Debug.Log("Going to wait");
        yield return new WaitForSeconds(0.5f);
        yield return AccionPosicion();
    }
}


[System.Serializable]
public class StatusModel
{
    public FatherModel Padre;
    public List<BabyModel> Hijos;
    public int elementos;
    public UDateTime timestamp;
}

[System.Serializable]
public class CutreTransform
{
    public float X;
    public float Y;
    public float Z;
}

[System.Serializable]
public class BabyModel
{
    public int ID;
    public CutreTransform Transform;
    public bool Movil;
    public string Animacion;
}

[System.Serializable]
public class FatherModel
{
    public int ID;
    public CutreTransform Transform;
}