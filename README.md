# Schlupys-World
RPG PRoject

ALPHA v0.131
- Added LivingEntity.cs // Contains values for 'living' entities (gold, hp, inventory)
- Added GroupedInventoryItem.cs // Groups inventory quantities and sorts by IsUnique

ALPHA v0.14
- Rewrote code for HitPoints and Gold (LivingEntity.cs)
- Bugfix: Only 1 item instead of x was taken from Inventory when completing Quest
- Rewrote ExperiencePoints and added Levels 

ALPHA v0.14.1 
- Worked about the combat, added a global cooldown
- Added potions and healing abilities in the game
- Added customizable description box for locations
- Tried to get changable name to work, but failed and quit the project for now (You currently spawn without a weapon, just uncomment the code in the GameSession constructor)

To-do:
- Armor
- Sql integration
- Abilities/spells
- Rework the world
