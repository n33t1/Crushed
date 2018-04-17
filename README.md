# Crush
Crushed is a RPG/Simulation game where a boy is trying to impress his crush. The player will play the role of the boy and collect objects to win the heart of the girl while fighting against the ex-boyfriend. The game will implement AI techniques such as A*, Goal-Orientation Behavior and Optimized Greedy Search.

![Crushed Preview](https://raw.githubusercontent.com/n33t1/Crushed/master/preview.png)

## Setup
1. You need to install [Unity](https://unity3d.com/get-unity/download).
2. Download this repository.
3. Open this repository from Unity if you want to view the source code or start playing.

## Attributes
* **Mood Bar**: There are three mood bars in total: friendship, happiness and romance. Your goal is trying to increase the two bars such that you can win the girl's heart. But be careful you don't want to be ended up as friend with the girl, so keep an eye on the friendship bar. Also, the girl's ex is trying to decrease the happiness and romance bars. Watch out for him!

* **Health Bar**: You can shoot the girl's ex to freeze him for a second. But the guy will become desperate when the girl almost fall in love with you (meaning that he will chasing after you and trying to terminate you). If your health bar goes to 0, you will lose. 

## Instructions
Keep in mind, your goal is to make Happiness and Mood Bar reach 100% and win the girl's heart (as well as the game).
1.   Using arrow keys to navigate the player
2.   Press Q when you reach an object to pick it up
3.   Press E when you reach the girl to give her the object
4.   Some objects can form combo when you give them one after the other. Cheat-sheet for the combos are available below.
5.   Some objects raise only one bar while other objects raise both bars.
6.   Some objects can reduce both bars. However, these objects can raise bar tremendously when used as combo.
7.   The Girl’s ex-boyfriend will also try to collect objects and give them to the girl to reduce her mood.
8.   You can shoot the ex (using Space) to reduce his health. When his health goes to 0, it pauses for 30 seconds which gives you time to perform high value combos.
9.   When both Happiness and Friendship bars are 100%, go to the girl and press P to propose and win.
10.  Giving same object again and again will raise the friendship bar. If friendship bar goes to full, you get Friendzoned.

## Team
[Harshit Gupta](https://github.com/harshit0921): level design, storytelling, goal-orientation behavior, optimized greedy search
[Amber Han](https://github.com/n33t1): character design, implementation for pathfinding using Navmesh, general implementation, player combat

## Game Mechanics

### Lose Conditions:
1.   Both the happiness and romance bar go to 0%
2.   Friendship bar goes to 100% and you get FRIENDZONED.

### Win Conditions:
1.   When both Happiness and Friendship bars are 100%, go to the girl and press P to propose and win.

### Combos:
* Red rose → Chocolate (1.5x Romance increase)
* Red rose → Ring (1.5x Romance increase)
* Chocolate → Ring (2x Romance increase)
* Red rose → Chocolate → Ring (2x Happiness increase, 3x Romance increase)
* Blue rose → Dress (1.5x Happiness increase)

### AI Systems

* **Pathfinding with Navmesh/A***: We linked the unity pathfinding library to our game which implements A* such that our NPC can reach the destinations we assigned to him without any collision.

* **Object analysis**: The NPC analyzes the mood change after a certain object is given to the girl and add the object to the correct list. If it is a new object, then the NPC will add it to the unexplored list. Othervise, if the object helps the NPC reduce the mood bars, the object will be allocated to the correct mood list. Or if the object increase the mood bar meaning that it helps the player, then the object is added to the unsafe list and the object will not be traversed in the future.

* **Combo detection**: We implemented a graph creation algorithm for the NPC. Whenever the mood reduces greater than the specified amount for that particular object, the NPC analyzed the current and previous objects and allocates the objects into the appropriate graph and position within the graph. For this, we implemented a graph for each mood, namely romantic and happiness. If a combo is for a particular mood, then the combo is added to the corresponding graph. If the combo is for both moods, it is added to both graphs. 

* **Goal-oriented behavior for the combos**: There are 4 behaviors in total: winning, safe, danger and critical. The execution of each behavior is determined from the sum of squares of all the mood values. The winning behavior prioritizes the exploration behavior with the combination of combo execution behavior. The safe behavior only focus on exploration. The danger behavior prioritizes combo execution with the combination of exploration. The critical behavior only focus on the execution of optimal combo based on the current mood values. 

Optimized greedy search: During the execution if the combos, the NPC keeps track of the highest optimal combo relative to each mood. During the critical behaviour, the NPC compares the current optimal combo with the highest value combo obtained by traversing through the graph and obtaining the sum of individual combos. We have implemented the greedy search algorithm that focuses on optimizing the graph search by shortening the combo length and removing the redundant paths.


* **Behavior Diversion**: When the player almost win and the NPC almost lose, the NPC will enter the behavior of chasing the player and trying to shoot the player. The movement speed for the NPC will gradually increase or decrease based on the mood bars. The behaviour of the NPC is completely diverted from trying to execute the combos and shifts to going on the offensive.

## Failures and setbacks

* We tried to implement A* for pathfinding directly. But since the 2D map in Unity was not a graph, we swapped to Navmesh insteadly.
* Our initial evaluation function for goal-oriented behavior is taking the original value and compare it to the threshold. This prevented our NPC from distinguishing the emotions needed immediate attention. 
* We faced animation problems for player movement when the player moves diagonally. We fixed this problem by restricting the player movement in 4 directions, which eased the implementation of shooting. 
* We tweaked parameters for the player movement speed and shooting speed as well as NPC movement speed and wait time. We also tweaked the parameter for threshold values in goal-oriented behavior. 
* We also changed the collision radius to make the collision smooth between the NPC and the game objects. 
* We struggled with the correct data structure to use for our implementation of graph. We tried hashtable, but it didn’t allow store multiple values for a given key. Eventually we used a dictionary with combination with list to implement the graph. 

## Successes
* Our NPC is hard to defeat. The combo detection mechanic optimizes the damage he can do to the player. When the player gets closer to winning, it will increase the moving speed and doing more damage to the player. The behavior diversion allows him to forbid the player from winning by shooting at the player. 
* Our NPC is able to automatically detect the combos. We don’t need to hardcode anything. It is able to create a graph when it detects a combo and keeps track of the optimized combo to execute based on the current mood requirement. 
* Our NPC is able to accurately reach the objects with shortest path and avoiding collisions. The animation of our NPC and player regarding movement is smooth. 
* Even though all the objects are in 2D, we are able to implement 3D features by tweaking the collision and Z-axis values relative to the camera.

## Evaluation
We had a play testing night with 10 volunteers. We received mostly positive feedbacks on the game experience with some bugs on NPC movement reported.

## Lessons learned
* While we were trying to implement the adaptive learning, we realized the optimized greedy search is more relevant to our game than machine learning approach. We realized we can achieve similar effects with simpler approach. Complex algorithms do not mean high entertainments. 
* It is easier to separate different AI mechanics in different scripts and different classes rather than keeping them in the same script. 
* Testing with game development is difficult in that many AI features are dependent on each other that makes unit testing complex. For example, we need to have pathfinding implemented first before we can test combo execution. 
