# daft-punk-djs
We're having a party and want Daft Punk to DJ!

Unity project that will (eventually) output to two displays for a seamless DJ window.

## Instructions:

### Music
Since dynamic music frequencies are A) A pain and B) never exactly on beat, this project uses preanalyzed audio tracks stored locally.
Any user of this project should create a directory at Assets/RawTracks and place their audio files there.  For purposes of this party, I'm creating track information for the entire "Tron Legacy Reconfigured" album. (My guess is that Github does some DMCA stuff so I'm gitignoring the tracks themselves).  
My *plan* is to design the track info easily switchable so you can update it with your own version.
The reality is that I have a month to complete this project and character animation is gonna be most of that.  Sorry in advance if it's a hardcoded mess.

### Output
I'm designing this to run on two primary monitors, as well as two additional monitors running a secondary app on another laptop.  This app (Monitors 2 and 3) will do the heavy lifting, processing all the music, animating characters, and running visualizers with the frequencies.  The secondary app (monitors 1 and 4) will have minimal UI with very few responsive elements.  Secondary app will listen on a websocket for simple events from the primary app and update the graphics accordingly (lighting changes, BPM data when the track changes, etc)

                      ---------------------------------             
       --------------/               //               /-------------
      /     1       /      2        //       3       /     4      /
     /             /     (Guy)     //    (Thomas)   /            /
    ------------------------------------------------------------

A physical front panel will be providing the angled dividers in my setup, but the app should be agnostic to that design choice, as long as the two center monitors are equal size it should appear as one continuous window (barring whatever bezel your monitors may have
