using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SocketIO;

public class SocketListener : MonoBehaviour
{


    public SocketIOComponent socket;
    public string currentRoom;

    public bool isSecondary;

    public string app_url = "http://wordabeasts.herokuapp.com/api/v1/room";

    // Use this for initialization
    void Start()
    {
        //StartCoroutine(getRoomCode());
        initializeSocketListeners();
    }

    public void quitSocket()
    {
        socket.Close();
    }

    public void initializeSocketListeners()
    {
        //socket.On("connect", TestBoop); //TODO error when no service connection can be found
        if (isSecondary) {
            socket.On("sendChoices", this.receiveData);
        }

        joinRoom("TRON");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("r"))
        {
            joinRoom(currentRoom);
        }
        if (Input.GetKeyDown("p"))
        {
            newTrack(125, "DJ", "New Song Title", "Daft Punk");
        }
    }

    public void updateTrack(string title, string artist, int bpm)
    {
        newTrack(bpm, "DJ", title, artist);
    }

    public void receiveData(SocketIOEvent e)
    {
        Debug.Log("Caught it");
        Debug.Log(e);

        string title = e.data.GetField("values").GetField("title").ToString().Replace("\"", "");
        string artist = e.data.GetField("values").GetField("artist").ToString().Replace("\"", "");
        float bpm = float.Parse(e.data.GetField("values").GetField("bpm").ToString().Replace("\"", ""));

        FindObjectOfType<SecondaryManager>().receiveTrackInfo(title, artist, bpm);
    }

    //----------------------------------------THINGS TO SEND TO THE SOCKET-----------------------------------------------

    public void joinRoom(string roomCode)
    {
        Debug.Log("joinRoom + " + roomCode);
        string values_JSON = @" ""roomCode"": """ + roomCode + @"""";
        socketCall call = new socketCall("joinRoom", "server", roomCode, values_JSON);
        string jsonData = call.getCall();

        JSONObject js = new JSONObject(jsonData);
        socket.Emit("joinRoom", js);
    }


    //Sends choices for all players who did not submit the current clue word [RESENDABLE]
    public void newTrack(int bpm, string playerWhoGaveClue, string title, string artist)
    {

        string values_JSON = @"     ""bpm"": """ + bpm + @""",
                                    ""title"": """ + title + @""",
                                    ""artist"": """ + artist + @"""
                              ";

        socketCall call = new socketCall("sendChoices", "app", currentRoom, values_JSON, playerWhoGaveClue);
        string jsonData = call.getCall();

        JSONObject js = new JSONObject(jsonData);
        socket.Emit("sendChoices", js);
    }

    public void TestBoop(SocketIOEvent e) { Debug.Log(string.Format("[name: {0}, data: {1}]", e.name, e.data)); }



    //--Actual socket event class
    class socketCall
    {
        public string event_name = "";
        public string destination_id = "";
        public string room_code = "";
        public string values_JSON = "";
        public string destinationExemption = null;

        public socketCall(string event_n, string dest_id, string room, string val_json, string destination = null)
        {
            event_name = event_n;
            destination_id = dest_id;
            values_JSON = val_json;
            room_code = room;
            destinationExemption = destination;
        }


        public string getCall()
        {
            string jsonData = @"{
                                ""eventName"": """ + event_name + @""",
                                ""origin"": ""host"",
                                ""destination"": """ + destination_id + @""",
                                ""roomCode"": """ + room_code + @""",
                                ""values"": { " + values_JSON + @"
                                }
                            }";
            if (destinationExemption != null)
            {
                jsonData = @"{
                                ""eventName"": """ + event_name + @""",
                                ""origin"": ""host"",
                                ""destination"": """ + destination_id + @""",
                                ""roomCode"": """ + room_code + @""",
                                ""destinationExemption"": """ + destinationExemption + @""",
                                ""values"": { " + values_JSON + @"
                                }
                            }";
            }

            Debug.Log("SOCKET CALL");
            Debug.Log(jsonData);


            return jsonData;
        }
    }

    [System.Serializable]
    public class roomCodeJSON
    {
        public string status;
        public string message;
        public whatAStupidParsingSystem data;


        [System.Serializable]
        public class whatAStupidParsingSystem
        {
            public string roomCode;
        }
    }
}


