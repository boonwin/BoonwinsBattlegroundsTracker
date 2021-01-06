# Welcome to Boonwins Battlegrounds Tracker!

Hi! I'm Boonwin I made **Boonwins Battlegrounds Tracker** mainly for streamers, so they can show their audience the placements and mmr change of the day. First this used to be a standalone application, you can still find this on www.boonwin.de but then I decided to make a [Hearthstone Deck Tracker](https://github.com/HearthSim/Hearthstone-Deck-Tracker) Plugin out of it. That beeing said, you need HDT first otherwise you can't us this plugin. 

# How to Install

1. [Click here](https://github.com/boonwin/BoonwinsBattlegroundsTracker/releases) to download the latest YYYYmmDDBoowinsbattlegroundstracker.zip from the [releases page](https://github.com/boonwin/BoonwinsBattlegroundsTracker/releases).
2.  Unblock the zip file before unzipping, by  [right-clicking it and choosing properties](http://blogs.msdn.com/b/delay/p/unblockingdownloadedfile.aspx):  
[![Unblock](https://i.imgur.com/jic3r5R.png?raw=true)](https://i.imgur.com/jic3r5R.png?raw=true)
3.  Make sure you remove any old versions of BoonwinsBattlegroundsTracker directory in the plugins directory of Hearthstone Deck Tracker completely, before upgrading versions.
4.  Unzip the archive to  `%AppData%/HearthstoneDeckTracker/Plugins`  To find this directory, you can click the following button in the Hearthstone Deck Tracker options menu:  `Options -> Tracker -> Plugins -> Plugins Folder`
5.  If you've done it correctly, BoonwinsBattlegroundsTracker directory should be inside the Plugins directory. Inside the directory, should be a bunch of files, including a file called BoonwinsBattlegroundsTracker.dll.
6.  Launch Hearthstone Deck Tracker. Enable the plugin in  `Options -> Tracker -> Plugins`.
8.  If it is not working you can enable a debug mode in the options window and join my Discord to tell me whats wrong. https://discord.gg/55ZS6hy

## How to Use

Well its fairly easy to use the plugin. As already written launch Hearthstone Deck Tracker. Enable the plugin in  `Options -> Tracker -> Plugins`. 
Now its pretty much already working, if you want to show your Stats on your stream, then you either need to share your whole desktop in OBS or you follow this instructions: [Streaming Instructions for HDT](https://github.com/HearthSim/Hearthstone-Deck-Tracker/wiki/Streaming-Instructions)
Within the Plugin you have couple options, you can change the color of the text and on which side you want to show the tracker. 
To get to the options you need first open up HDT, click on "plugins" then click "Boonwins Battelgrounds Tracker Settings" this will open on the left site a panel where you can change text colors and set the location on the screen were you want the tracker.

![https://i.imgur.com/t05nXsz.png](https://i.imgur.com/t05nXsz.png)

To change the location please enter first your screen width and then click right, so far I only added three different sizes, since i developed the plugin for 1080p this would be max so far.
if everything worked you will see this in the Hearthstone-menu :
![enter image description here](https://i.imgur.com/fuiGhUS.png)

As soon as you go into Battlegrounds-menu it will load your MMR and set it as the start-value for the day. If the game and HDT crashes, it will reset the MMR start-value! So far I dont safe this data, might add this in the future.
In-game it will add the average rank, the current mmr and the banned class to the top panel of "Bobs Buddy"
![enter image description here](https://i.imgur.com/I3Fv1PS.png)

Well thats it, have fun using it.

## Contact
If you have any comments just come into my discord https://discord.gg/mbvv2j

# Credits
Since im new to making hdt plugins I used [Jawslouis Battleground Plugin](https://github.com/jawslouis/battlegrounds-stats/) as an exsample, so check his plugin out too, if you like to record your stats over a long period it, realy good.
And also thanks to [JohnnyToumieh](https://https://github.com/JohnnyToumieh) who helped me with the banned tribe part. 
