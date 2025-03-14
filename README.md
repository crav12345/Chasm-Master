# Chasm-Master

_Chasm Master_ is a video game made with Unity. The design goal of _Chasm Master_ is to recreate the 'Bridge of Death' scene from _Monty Python and the Holy Grail_. The game achieves this by leveraging a RESTful API server developed with Node.js. The server sends a random riddle to the client from a curated collection of riddles and their answers stored in a MongoDB database. The player's open-ended input is then validated by GPT-4 to avoid common string parsing pitfalls.
