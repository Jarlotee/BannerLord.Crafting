### BannerLord Crafting

A collection of quality-of-life improvements for crafting

#### How it works

##### Stamina Recovery
As of the 1.0.0 release, the only way to recover crafting stamina is to rest in a settlement.

While I get that traveling and potentially fighting can take it out of your character there are plenty of other activities (such as waiting for nightfall to attack a bandit camp) that I would define as restful.

This mod allows you and your companions to recover crafting stamina at 50% of the normal rate while completing objectives and traveling around Caladria.

##### Part Research and Unlocking Behavior Overhaul

As of the 1.0.0 release, unlocking parts is tied directly to the selling price of the crafted item and the number of parts available.

The more parts available the longer it takes to get any kind of progression, to the point where you have 250 smithing and still can't complete a high-level dagger, mace or axe work order.

On the other hand, high-price items with medium to low part counts (e.g. 2H Swords, Throwing Axes) unlock so quickly that you stop bothering to craft anything else.

Here is how this mod attempts to rectify these issues:

* When you smelt an item with a component you have yet to unlock, you have a high chance (baseline 20%) to unlock said part
    * This includes a low chance to unlock a high-tier part as a novice and an improved chance to unlock a low-tier part as an experienced artisan
    * Your chance to unlock a particular part improves with each failed attempt (cumulative)
    * The unlock chance applies to each part of the smelted weapon, so you can unlock more than one part through smelting
    * The Curious Smelter perk now improves your chances of unlocking a part as if your crafting level was 50 higher

* When you craft an item you have a low chance (baseline 5%) of unlocking a new part (working up from the lowest tier to the highest)
    * Free Build and Orders both now give the same chance of unlocking new parts
    * This includes a low chance to unlock high-tier parts as a novice and an improved chance to unlock a low-tier part as an experienced artisan
    * Your chance to unlock a part improves with each failed attempt (cumulative)

##### Crafting Experience Improvements

As of the 1.0.0 release, you gain crafting experience at different rates depending on the activity

* Refining gives 30% of the value of your produced goods
* Smelting gives 2% of the value of the smelted items
* Free Build gives you 2% of the value of the produced item
* Crafting Orders gives you 10% of the value of the produced item

Does that seem a little lopsided to you? 

* Refining 3 charcoal from 2 wood might be worth 150 denars netting you 45xp
* Smelting one Pugio at about 250 denars would net you 3xp
* Crafting a 2H sword worth 800 denars would net you 16xp
* Crafting that same 2H sword for an order would net you 80xp

Here is how this mod attempts to rectify these issues:

* Reduce refining from 30% to 10% (45xp => 15xp)
* Boost smelting from 2% to 5% (3xp => 12xp) 
* Boost Free Build from 2% to 5% (16xp => 40xp)

#### Configuration

Feel free to tweak these multipliers (or disable the functionality) to suit your particular preferences by modifying `Modules\BannerLord.Crafting\ModuleData\Config.json` and restarting your game.

The base configuration is as follows:

```json
{
    "enableTravelingCraftingStaminaRecovery": true,
    "enableCraftingResearchBehavior": true,
    "enableImprovedCraftingXpMultipliers": true,
    "travelingCraftingStaminaRecoveryMultiplier": 0.5,
    "smeltingBaseUnlockChance": 0.2,
    "craftingBaseUnlockChance": 0.05,
    "refiningXpMultiplier": 0.1,
    "smeltingXpMultiplier": 0.05,
    "freeBuildCraftingXpMultiplier": 0.05
}
```