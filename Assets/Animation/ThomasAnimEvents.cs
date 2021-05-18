using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThomasAnimEvents : MonoBehaviour
{

    public TrackManager jukebox;
    public SocketListener sockets;

    public enum actions
    {
        nextTrack,
        shutter,
        empty
    }

    public actions currentAction;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void nextTrack()
    {
        jukebox.nextTrack();
    }

    public void pushButton()
    {
        if(currentAction == actions.nextTrack)
        {
            jukebox.nextTrack();
        }
        else if (currentAction == actions.shutter)
        {
            sockets.handleShuttering();
        }
        currentAction = actions.empty;
    }
}
