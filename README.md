# LightProsperity
Makes prosperities of towns count. Prosperity of towns (hearths of villages) affect the number and training level of provided recruits. Recruiting them cost prosperity. Garrison no longer starves.

In vanilla game, prosperity only affect tax income, which is also marginal as mid-to-late game gold is never an issue. On the other hand, it puts too much pressure on food consumption, often leads to starving of garrision. High prosperity is almost a bad thing in late game. This mod is trying to change that and make prosperity one of the most important propety of settlements.

### Changes:
1. Prosperity of towns (hearths of villages) affect the number of recruits avaliable. The higher the prosperity, the more recruits a town can provide. The recruit replenish rate is `Original_value * Prosperity / townProsperityThreshould` (default 3000)
for town and `Original_value * Hearths / villageProsperityThreshould` (default 300)
for village. Prosperity and hearths are the measurements of population, so higher population -> more recruits.

2. Recruiting from towns and villages reduces their prosperity. For every volunteer you or AI lord recruit from a settlement, it loses `townRecruitProsperityCost` (default 10) prosperity. Drafting people to army reduces the working population.

3. Higher prosperity allows more slots to be recruitable. In vanilla, only 1 slot is avaliable. Being in the same faction +1 slot, at war with that faction -1 slot, and the relationship with the notable affect slots. Now, if the settlement is owned by your clan, you have +1 slot (so 3 slots by default if it's your fief). Prosperity also affect the number of slots now: When not at war, for every `townProsperityPerBonusSlot` prosperity past `townProsperityThreshould`, one more slot is avaliable. The more total recruits avaliable, the easier to find someone who is willing to follow you.

4. Prosperity and the power of each notable affect the training level of recruits, especially the power. A town with high prosperity and powerful notables can generate an elite army in weeks.

5. Notables are more likely to generate noble recruits if they are capable. A notable in a village and have power above 200 is capable, and the chance of generate a noble recruit is `(Power - 200) / 200 * 100%` , this means at power above 400 every recruit will be noble. For each noble recruit generated, the notable loses `notableNobleRecruitPowerCost` (default 3) power. Notables spend their influence and power to summon nobles for war. The effects of prosperity and power on training level also apply.

6. Garrison no longer starves, but still consumes food. They are no longer hindered by prosperity, but still put a pressure on food, and may reduce prosperity through food shortage.

### Effects on gameplay:
Prelonged war between factions become a contest of manpower. At first, because garrison no longer vanish, the snowball effect seems to be weakened. And the army composition of AI lords will improve greatly because elite troops are avaliable. However, war consumes manpower, as every man drafted from villages and towns reduces its prosperity, slowing down more recruits from being generated. At first lords can draft large armies, at the cost of war potential of entire faction. If the war goes too long, the prosperity of settlements may reduce to low levels so large army can no longer be assembled.

This may change game balance as imperial settlements tend to have high prosperity at the beginning, while Khuzait, though large in terms of controlled area, lacks prosperity and thus manpower. A fast banner-changing frontline town can never be a good source of army, nor are villages living in danger. A peaceful home sector is vital for supplying war efforts with decent troop.

### Planned feature:
1. Recruit from castles, as castles should be the major source of noble recruits.

2. Incorporate a good diplomacy mod to make AI leaders consider their avaliable manpower (remaining prosperity). Factions drained of war may seek peace to recover, even when this means inequal treaties.

### Installation:
It is recomended to get the release version on nexus by Vortex: [Nexus link](https://www.nexusmods.com/mountandblade2bannerlord/mods/860).
For manual install, unzip and put the directory LightProsperity to C:\Program Files (x86)\Steam\steamapps\common\Mount & Blade II Bannerlord\Modules, or your custom install location.