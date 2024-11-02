# home-assignment-developer-jesus-miguel-garcia
MadBox home assigment developer solution Jesus Miguel Garcia Canales

## How you approached this test: what were your different game making phases?

**-> 1. Technical Design:**
  First of all, I created a small technical design for setting the foundations of the assignment (Movement, Enemy spawning logic, Targetting and Attack).
  
**-> 2. Foundations Implementation:**
  After this, I started coding the systems needed for assignment's gameplay core, following the design created.
  
**-> 3. Scale foundations up:**
  The following step was to scale up the systems already created for reusing the player control for the enemies to give them a basic AI and check the game feel.
  
**-> 4. Gameplay quick iterations:**
  Once in this stage, I was able to do a quick fine tunning of the gameplay and evaluate a list of things missing that can help to improve the game feel (like health bars and weapons).
  
**-> 5. Add basic weapon drop system and weapon stats:**
  Adding this, I was able to iterate further changing different weapons stats, enemies movement speed and attack range.
  
**-> 6. Add performance improvement systems:**
  As enemies were created and destroyed very often, I created a basic object pooling logic in the LevelController to avoid possible performance issues in the future.
  
**-> 7. Visual refinement:**
  Also, I added pure visual refinements, like showing the weapon of the main character.

## The time you spent on each phase of the exercise
**-> 1. Technical Design**
  1 hour
  
**-> 2. Foundations Implementation**
  5 hours

**-> 3. Scale foundations up**
  2 hours

**-> 4. Gameplay quick iterations**
  1 hour

**-> 5. Add basic weapon drop system and weapon stats**
  1 hour

**-> 6. Add performance improvement systems**
  1 hour
  
**-> 7. Visual refinement**
  1 hour 
  
## The features that were difficult for you and why
  Targeting systems are always a tricky part. I know this is pretty simple but usually tend to get messy when you scale them up. Also, is very common to put all logic in game's hot path to avoid miss target, so they can get worse the game performance If It gets complicated. For this reason, I aimed to avoid heavy and frequent checks. Additionally, I tried to do It flexible and scalable because usually games need variety of targeting logics.

## The features you think you could do better and how.
  1. Object pooling is pretty simple and only used for enemies spawning/destroy. I would like to do a more generic solution and avoid allocations also for weapon items. However, I think my solution is pretty simple gameplay-wise. 

 2. Enemies AI system could be based on basic Actions/Conditions system approaching It to a behaviour tree implementation, but I've decided to keep It small and simple. 

 3. Adding a victory/defeat condition.
  
## What you would do if you could go a step further on this game
  1. Adding different types of enemies with different behaviours (range enemies that shoot projectiles, mid bosses etc.).
  2. Improving the enemy generation logic to make It based on waves.
  3. In the player part, I think that might be interesting to add a dash move to avoid projectiles, double tapping in the direction you want to move. 
  4. Including more upgrade items with different uses like temporary shields, companions that attack with projectiles and power slashes.

## Any comment you may have
 With the proper progression system, I think this gameplay can be fun, kind of Vampire Survivor approach. But, I also feel the market is pretty crowded with this type of proposals, so I doubt this can be a viable project.
