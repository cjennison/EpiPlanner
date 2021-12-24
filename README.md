# EpiPlan
Rotation planner using available logging information to help assist in the
performance of DPS classes in FFXIV.

## WIP
This plugin is under development.

### Roadmap
#### Openers
Display Opener chains up to any number of predefined actions. Opener actions disappear when used.
#### Burst Phases
Burst phases initiated on actions with tied burst initiations. Display burst chains.
#### Customization
Allow custom phases to be developed and selected for a given fight/instance.
#### All Phases
Full 'Hekili' style plugin.

### References
DalamudPluginProjectTemplate: https://github.com/karashiiro/DalamudPluginProjectTemplate  
SamplePlugin: https://github.com/goatcorp/SamplePlugin


### Whitepage

EpiPlan is a rotation helper that generalizing FFXIV DPS rotation into:
- Phases: Parts of a rotation with differing priorities. 
- Priorities: Tiered list of abilities that take precedence over others, separated by GCD and oGCDs.
- Chains: Abilities with dependencies.

Each job implements its own set of these controls.
At a basic version, EpiPlan can detect what phase a player enters into and offers guidance on the next set of
button presses to execute.
As this becomes more intelligent button presses can change mid-phase depending on cooldowns, buff times, and
many other class states.

EpiPlan is made up of the following critical components:
- ActionPlan: An object that represents the set of elements that need to be activated
- Listener: FFXIV Event hook
- JobRunner: (per Job) computes the current action plan
- DisplayEngine: Receives instructions to display icons via an action plan 
- Orchestrator: Connects all major components

The *ActionPlan* is basically an array of elements, or proposed actions, that should be activated in
a specific order. The basic component, an <Action>, provides information about the name, type, cast time, mana cost,
and other information that the display engine can use to display the Action accordingly.

The *Listener* hooks into the FFXIV event system to get details about the game, echoing back cooldowns,
action activations, and any relevant class information such as cooldowns, stacks of de/buffs, etc.
For use of mocking, the Listener in the UITester is a Simulator UI that can send events to the Orchestrator.
The Orchestrator for each tick will consume the Listener's gamestate. (this.listener.getState())

The *JobRunner* is the central processing system. The JobRunner is a mini-orchestrator that integrates with a
<Job>Machine (ie MCHMachine, BLMMachine) to compute the ActionPlan depending on the current state.

The *DisplayEngine* consumes an ActionPlan and displays it on the UI. 

The *Orchestrator* connects all of the above.

![diagram](https://i.imgur.com/DODxE0e.png)

An example of this would be as follows:   
```
Machinist starts combat
JobRunner initiates MCHEngine and sends the GameState
Phase: Filler
  Priorities:
    0. Barrel Stabilizer
    1. Reassmble
    2. Chainsaw
    3. Air Anchor
    4. Drill
    5. Ricochet
    6. Gauss Round
    7. Heated Split Shot > Heated Slug Shot > Heated Clean Shot

    Chainsaw, Drill, Air Anchor delayed if Reassmble CD < 5 seconds.
    Weave Reassmeble in oGCD

    Use Barrel Stabilizer on CD if heat would not exceed 100%

MCHEngine returns ActionPlan mchActionPlan:
[
  Action{ name: "Barrel Stabilizer", type: 1 },
  Action{ name: "Chainsaw", type: 0 },
  Action{ name: "Gauss Round", type: 1 },
  Action{ name: "Ricochet", type: 1 },
  Action{ name: "Reassmble", type: 1 },
  Action{ name: "Air Anchor", type: 0 },
  Action{ name: "Gauss Round", type: 1 },
  Action{ name: "Ricochet", type: 1 },
  Action{ name: "Drill", type: 0 },
  Action{ name: "Gauss Round", type: 1 },
  Action{ name: "Ricochet", type: 1 },
  Action{ name: "Heated Split Shot", type: 0 },
  Action{ name: "Heated Slug Shot", type: 0 },
  Action{ name: "Heated Clean Shot", type: 0 },
]

displayEngine.render(mchActionPlan)


Machinist uses Barrel Stabilizer
Listener catches the event and stores it in the game state
MCHEngine reads the state and sees Barrel Stabilizer used and re-computes state - removing the Barrel Stabilizer.

```

A resulting UI representation may look like:   
![Example](https://i.imgur.com/jkO892r.png)

### How would Openers be handled
Openers would be considered a phase. The job engine would know to return the Opener phase almost as a constant
and not as something requiring any degree of intelligence.

### Is this against ToS?
No, not in my opinion. The information this tool provides is information a player would have anyways, but displayed in a digestable form.
This plugin would not press any keys for the user and in fact, the output of the plugin is subjective providing an suggestion - not the correct answer.
Encounter Mechanics may alter the true rotation one may need to implement. This plugin serves as a good starting point, but mastery of a role exceeds the scope of this plugin.

### Simulation Strings
While currently each class would need to be engineered, it is not out of the realm of possibility to utilize a Simulation string similar to that of SimulationCraft used in World of Warcraft.
Such a string would come later in development. For now due to the nature of the oGCD and GCD system, the class engineering will need to be handled manually.

Theoretical example:
```
phase filler
p barrel stabilizer if heat <= 50
p reassemble 1 > chainsaw 0
p reassemble 1 > air anchor 0
p reassemble 1 > drill 0
p chainsaw 0 
p air anchor 0
p drill 0
p gauss round 1
p ricochet 1
....
p overdrive --> phase overdrive

phase overdrive
p heat shot 0
p gauss round 1
p ricochet 1
c overheated end --> phase filler

```


### Tracking Actions
Utilize the https://goatcorp.github.io/Dalamud/api/Dalamud.Game.ClientState.Objects.Types.BattleChara.html
To track the current spell/ability/weaponskill being cast and using additional information such as 
total cast time, and current cast time. This helps in tracking the last ability used.

Additional Resources:
https://goatcorp.github.io/Dalamud/api/FFXIVClientStructs.FFXIV.Client.Game.ActionManager.html
https://goatcorp.github.io/Dalamud/api/FFXIVClientStructs.FFXIV.Client.Game.Character.BattleChara.CastInfo.html

___
Inspiration is taken from World of Warcraft's Hekili addon.  
https://www.curseforge.com/wow/addons/hekili   
![hekili](http://i.imgur.com/90h4L8s.png)
___