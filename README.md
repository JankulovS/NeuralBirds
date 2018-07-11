# NeuralBirds

![Preview](https://thumbs.gfycat.com/TimelySmoggyEquine-size_restricted.gif)

## Neuro-evolutional Flappy Bird AI
NeuralBirds is a flappy bird clone that is playable by either a human player or by neural networks. Birds can either learn from scratch or import pretrained networks. If the game is being played by neural networks, their learning progress can be saved at any point.

## Requirements
SFML.net library

Game sprites (folder \data)

## Algorithm
NeuralBirds uses a combination of random artificial neural networks for decision making and a very basic genetic algorithm for learning. 

The very first generation of birds consists of neural networks with random behaviour. When all birds lose the game, a new generation is formed. When forming the new generation birds are picked at random. Every bird's chance of being picked is directly proportional to their score divided by the total score of all birds. Since the birds have identical behaviours as in their previous lives, a change of behaviour is necessary. Every new bird has a relatively low chance of being mutated, which will slightly tweak the network's behaviour. The overall best bird from every previous generation is always picked (elitism). There is no crossover of genes, birds pass on their genes as is. The only way birds learn is by pure mutation chance. 

If all birds do very poorly (slam into the ground or ceiling immediately) the whole process restarts and new random neural networks are formed.
