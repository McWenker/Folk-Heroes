# Changelog
### 1/28/2019
#### AI targeting improvements & friendly AI
* AI characters can now have targeting priorities. Hostile AI prioritize Soldiers > Gatherers > Players > Buildings
* Hostile AI will destroy buildings if no other targets are found
* Friendly AI Soldiers can now fight Hostile AI, much to improve

### 1/22/2019
#### Dualwielding, enemy improvements
![Dualwielding](DemoImages/DUALWIELD.gif?raw=true "It's a little rough around the edges, but I like it so far.")
* Two weapons can now be wielded at once, with right click controlling the off-hand weapon
* Weapons cannot be fired simultaneously, this may be changed or added as some kind of class feature

![SlimeDash](DemoImages/SLIMMWO.gif?raw=true "Now they're even more terrifying!")

#### Enemy attacks, enemy spawns
* Enemies now have the ability to dash attack, play an animated attack, or fire a projectile
* Enemies will now spawn from their own homes, just like Dweffis
---

### 1/20/2019
#### Melee Weapons!
* Took the first swing at some other weapon types, and making the different weapon modes play nice together
* Currently, melee weapons do not deal friendly fire (guns do)

![Houses](DemoImages/spawningWheel.gif?raw=true "Need a chimney with a cozy fire...")

#### Houses are homes
* Gatherers are now created from houses, up to a maximum per house
* Currently this happens automatically, but later it will change
---

### 1/18/2019
#### Mode swapping first implementation
* Very exciting! The player can now switch between Combat and Command modes
* Command mode offers only some simple building options

#### Construction first implementation
* Homes for the little Dweffis can now be placed
* Homes check for eligibility before construction, and consume resources
* Newly constructed buildings are baked into the navigation at instantiation

#### Gatherer improvements
* Gatherer flee pathing improved, they will get stuck on one another less frequently
---

### 1/15/2019
#### Resource node randomization
* Resource nodes now spawn with a random number of resources
* Currently the range is small, but I intend to address this further during map generation.

#### Gatherer improvements
* Gatherers now visually indicate when they are fleeing with a ! icon
* Gatherers no longer attempt to mine depleted resources
* Gatherer flee reaction speed and pathing improved

#### Enemy improvements
* Enemies now chase the nearest priority target sooner
* Enemies now better alert their nearby allies to a target
---

### 1/14/2019
#### Added AI state machine
* Gatherers now randomly seek Resource Nodes
* Hostiles now seek nearest Gatherer within range first, then nearest Player within range

#### Resources are go, but unsure what they are yet
* Gatherers can now mine Iron & Gold, or (mine) Mana
* The actual names of the resources haven't been nailed down yet (lol)