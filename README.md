# Star Sign

A 3D-game Zodiac Sign maze game which includes a robot named Barnum powered by Gemini
which covers

# Back-End

The back-end is handled in the `Flask/` folder. As the name implies, it uses a Flask to receive and return information. It also predomeniately utilizes the Gemini API and the Eleven Labs API

**How it Works**: When the game first starts, it sends a request to /zodiac, which gets the
zodiac sign posted which the player selected, and returns an id which will now be associated
with the player and saves their id and zodiac sign.

Afterwards, if a request is made to the associated id. It will do the following:
It will receive the actions of the player, and feed into Gemini. Gemini has a system prompt which determines
Barnum's personality and how will they respond, and the system prompt changes slighly differently depending
on what the player's Zodiac sign. After receiving a response from Gemini, its response is fed into ElevenLabs, which generates an audio clip from the text, and then returns the audio clip to the front-end.
The front-end can then play this audio clip and it can be heard by the player
# Credits
- Front-End/Game Logic by **Johaan Riat**
- Backend-Logic and System Prompts by **Santiago Sanhueza**
- Barnum, Player Design/Animation, and Music by **Anthony Pellito**
