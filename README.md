# WORMSLIKE
## Unity Game using Mirror  

<p align="center">
    <img src="https://media.discordapp.net/attachments/1078794808673435648/1089160405802307595/bg.png?width=1342&height=671" />
</p>

## THE PROJECT
The goal of the project was to create an online game using [Unity Mirror](https://mirror-networking.com), an interface to program networking solutions on an Unity game.  
The project lasted 4 weeks, was supervised by [Alex Dana](https://github.com/MisterAlex95) Lead Web Developer at Ubisoft Paris and was done with a group of 4 people :  
- Pierre MARCURAT : Map, Sounds, Assets and Main Menu
- Dimitri CLAIN : Main Menu, Room Menu, Architecture
- Tim FERTIN : Gameplay

## THE GAME
We aimed to recreate our very own [Worms](https://store.steampowered.com/app/327030/Worms_WMD/) game with simplified options (no crafts, no different game modes, no sudden death or rising waters and so on...).  
We wanted to create a Room with a Lobby before the game that allows the clients to join the host to custom the game modes and settings (turn time, powers, health start amount...).  
We also wanted to adapt the [Marching Squares](https://en.wikipedia.org/wiki/Marching_squares) algorithm to allow the host to generate procedural maps that would fit the needs of our game but that was a bit too advanced...  

## CLONING, PLAYING AND WORKING ON OUR GAME
### Bindings
Those bindings are supposed to be keyboard layout's independent, meaning on a different keyboard layout than AZERTY, it is *supposed* to work, but we didn't test it ([here's why](https://media.discordapp.net/attachments/1078794808673435648/1089170008267030639/Eha7r9KWsAIoXDr.png?width=474&height=671))
- [ Q/D ] : Left & Right
- [ Z/S ] : Aim Up/Down
- [ Space ] : Jump
- [ X (Hold) ] : Shoot (Increase Shot Power)
- [ Mouse Drag & Wheel ] : Move Camera & Zoom
- [ T ] : Reset Camera


### Important information
To play our game, the host must enable port forwarding on 7777 on his/her router to allow clients from other networks to connect to his game instance. The game is still playable if he/she wants to play locally by creating multiple instances of the game.
### Working on the game
Feel free to check the code and message us if you have any questions or inquiries about what's going on, we'll do our best to help! :>

### How to improve our game
We struggled to comprehend every aspects of Mirror and lacked time to implement everything we aimed to add to our game, resulting in a *not so pretty* game result. It is still playable, players can move and shoot around, destroying the one and only map asset we have (that is not generated using the Marching Squares algorithm), but we have a big issue exiting the Game Scene. Moreover, we have little to no customization on the Lobby, and our menus are very 101 basic (we spent a lot of time trying to implement [Mirror's Room System](https://mirror-networking.gitbook.io/docs/manual/examples/room), yet not enough...).  
Clearly, our game could be better and improved.

## CREDITS
### Developers
- [Pierre MARCURAT](https://github.com/3uph0riah)
- [Dimitri CLAIN](https://github.com/Dimitri-CLAIN)
- [Tim FERTIN](https://github.com/Tim-Snugget)

### Assets
- [Particle Pack](https://assetstore.unity.com/packages/vfx/particles/particle-pack-127325) by Unity
- [Kawaii Slimes](https://assetstore.unity.com/packages/3d/characters/creatures/kawaii-slimes-221172) by Awaii Studio

### Honorable Thanks
- [Alex Dana](https://github.com/MisterAlex95)
- [Léo Ménard](https://github.com/softhy85) who was of great help reading the Mirror documentation
- [Team17 and Worms](https://www.team17.com/games/worms/) for the inspiration
- [Nespresso](https://www.youtube.com/watch?v=dQw4w9WgXcQ) for always standing by our side on the work desk
- [DapperDino](https://www.youtube.com/watch?v=5LhA4Tk_uvI&list=PLS6sInD7ThM1aUDj8lZrF4b4lpvejB2uB) with his very enriching YouTube guide on how to use Mirror
- [Samyam](https://www.youtube.com/@samyam) with her guides about the Unity Input System
- [a good boy game](https://www.youtube.com/@agoodboygames) with his diverse guides
- [Waldo](https://pressstart.vip/tutorials/2018/11/9/78/perspective-camera-panning.html) wiht a good guide on the camera movements using the mouse drag
