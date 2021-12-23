# EpiPlan
Rotation planner using available logging information to help assist in the
performance of DPS classes in FFXIV.


## WIP




### Whitepage

EpiPlan is a rotation helper that generalizing FFXIV DPS rotation into:
- Phases: Parts of a rotation with differing priorities. 
- Priorities
- Chains

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


___
Inspiration is taken from World of Warcraft's Hekili addon.  
https://www.curseforge.com/wow/addons/hekili   
![hekili](http://i.imgur.com/90h4L8s.png)
___