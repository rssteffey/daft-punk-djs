# daft-punk-djs
We're having a party and want Daft Punk to DJ!

Unity project that will (eventually) output to two displays for a seamless DJ window.

## Instructions:

### Music
This project uses audio tracks stored locally and added to the app with BPM and metadata included. In the Unity project, these are organized under Tracks on the Camera_master->Jukebox object in the project hierarchy.
Any user of this project should create a directory at Assets/RawTracks and place their audio files there.  For purposes of this party, I'm creating track information for the entire "Tron Legacy Reconfigured" album. (My guess is that Github does some DMCA stuff so I'm gitignoring the MP3s themselves).  
My *plan* is to design the track info easily switchable so you can update it with your own version.
The reality is that I have a month to complete this project and character animation is gonna be most of that.  Sorry in advance if it's a hardcoded mess.

### Output
I'm designing this to run on two primary monitors, as well as two additional monitors running a secondary app on another laptop.  This app (Monitors 2 and 3) will do the heavy lifting, processing all the music, animating characters, and running visualizers with the frequencies.  The secondary app (monitors 1 and 4) will have minimal UI with very few responsive elements.  Secondary app will listen on a websocket for simple events from the primary app and update the graphics accordingly (lighting changes, BPM data when the track changes, etc)

                      ---------------------------------             
       --------------/               //               /-------------
      /     1       /      2        //       3       /     4      /
     /             /     (Guy)     //    (Thomas)   /            /
    ------------------------------------------------------------

A physical front panel will be providing the angled dividers in my setup, but the app should be agnostic to that design choice, as long as the two center monitors are equal size it should appear as one continuous window (barring whatever bezel your monitors may have)

In the interest of keeping things uniform, this project actually assumes that all 4 monitors are equally sized (the shutters are currently designed to fill the entire screen, so when all 4 are active they take up the same real world space), but you shouldn't have any out-of-the-box issues if your monitor pairs (1&4, 2&3) are sized to match each other.  Adjusting the camera size should be easy enough if you do have a mismatch though.

### Odd Bits
It's worth noting that I was too lazy to spin up an entirely new web service for this project - as such, all communication currently runs on the Wordabeasts dev test server.  No guarantees this will be functional for long, so you'll probably need to set up your own Websocket instance to get apps A & B to communicate.
Because this was a utilization of the WaB infrastructure, the events for sending track info and triggering away mode are nonsensically named, and you should probably swap them out for more appropriate things.

When starting up the projects - '1' & '2' keys swap between primary and secondary app views.  'A' activates secondary monitors (to output both screens).  'S' triggers the away event and 'N' changes audio tracks (although I personally used an AWS lambda triggered by Alexa).  The event triggers will play an animation of Alexa speaking to Thomas, so you may ant to disable that and trigger things immediately, depending on your own use.

Ultimately - any actual use of this project will probably need HEAVY modification, but hopefully this is a solid jumping-off point.  It was the star of our party for sure.
