# Folk Heroes

Hello and welcome to Folk Heroes, an upcoming indie RPGenre bender designed and developed by yours truly.

## Concept

Folk Heroes hopes to capture some of the basebuilding fun of classic games like Stronghold or Age of Empires, while blending it with a bullet-hell RPG that will probably never stop expanding in scope and reach (please help me).

![FirstDemoGif](DemoImages/AIswarm.gif?raw=true "Just need to add some bullets, some traps, a few more monsters...")

## Next Steps
#### Command Modes
   Only by mastering both Combat and Command, will you become a worthy Folk Hero...

#### Building
   Gatherers can collect things, now let's use their efforts to forge an empire...

#### Abilities and Classes
   Guns are only the beginning! Swords, spells, bombs, and much more is in the plan...

#### More Art, A Story
   Eventually this shit will have to have some kind of theme... right?

![AIInteraction](DemoImages/AIresponses.gif?raw=true "Why would these slime monsters kill such tiny cuties?!?!")

## Changelog
#### 1/15/2019
##### Resource node randomization
* Resource nodes now spawn with a random number of resources.
* Currently the range is small, but I intend to address this further during map generation.

##### Gatherer improvements
* Gatherers now visually indicate when they are fleeing with a ! icon
* Gatherers no longer attempt to mine depleted resources
* Gatherer flee reaction speed and pathing improved

#### 1/14/2019
##### Added AI state machine
* Gatherers now randomly seek Resource Nodes
* Hostiles now seek nearest Gatherer within range first, then nearest Player within range

##### Resources are go, but unsure what they are yet
* Gatherers can now mine Iron & Gold, or (mine) Mana
* The actual names of the resources haven't been nailed down yet (lol)

## Trying the Demo Thusfar

While I highly do not recommend you try to play the game in its current state (it's ugly and bad), if you insist...

### Prerequisites

You will need a copy of the [UnityEditor](https://unity3d.com/)

	1) Download the repository
	2) Open your UnityEditor
		a) locate the Unity Projects directory (or create it)
	3) Create a new Project
		a) title it "Folk Heroes" to ensure compatability (maybe? I have no idea if that will matter)
	4) Clone this Repo
		a) paste the contents of the cloned directory into the "Folk Heroes" project folder, merging and replacing existing files
	5) ??????
	6) Profit!

## Authors

* **Tyler Brandt** - *Art, design, scripting* - [McWenker](https://github.com/McWenker/)

## License

This project is licensed under a Creative Commons BY-NC license. Currently the project is non-commercial, however that obviously may change going forward if things go well.

## Acknowledgments

* Tip of the hat to [Unity CodeMonkey](https://unitycodemonkey.com) for a useful utilities library
* Respect to the creators of [TextMesh Pro](http://digitalnativestudios.com/textmeshpro/docs/)
* Clear inspiration: [Enter the Gungeon](http://dodgeroll.com/)
