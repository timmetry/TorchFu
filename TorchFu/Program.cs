using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TorchFu
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateGraphs();
            //CreateImagesets();
            //CreateAffixes();
            //CreateRecipes();
            CreateSkills();

            //int unitGUIDcounter = 14000000;
            //CreateSocketables(ref unitGUIDcounter);

            Console.Out.WriteLine("Done.");
        }

        static void CreateRecipes()
        {
            const int gemQuantity = 30;
            CreateGemRecipes("Amethyst", gemQuantity);
            CreateGemRecipes("Aquamarine", gemQuantity);
            CreateGemRecipes("Diamond", gemQuantity);
            CreateGemRecipes("Emerald", gemQuantity);
            CreateGemRecipes("Ruby", gemQuantity);
            CreateGemRecipes("Sapphire", gemQuantity);
            CreateGemRecipes("Topaz", gemQuantity);
        }

        static void CreateGemRecipes(string gemtype, int quantity)
        {
            RecipeFile recipe;

            for (int i = 0; i < quantity - 1; ++i)
            {
                recipe = new RecipeFile(gemtype + (i + 1).ToString("00") + "x2", "gems/");
                recipe.AddIngredient("unit", gemtype + (i + 1).ToString("00"), 2);
                recipe.AddResult("unit", gemtype + (i + 2).ToString("00"));
                recipe.Create();
            }
            for (int i = 0; i < quantity - 2; ++i)
            {
                recipe = new RecipeFile(gemtype + (i + 1).ToString("00") + "x4", "gems/");
                recipe.AddIngredient("unit", gemtype + (i + 1).ToString("00"), 4);
                recipe.AddResult("unit", gemtype + (i + 3).ToString("00"));
                recipe.Create();
            }
            for (int i = 0; i < quantity - 2; ++i)
            {
                recipe = new RecipeFile(gemtype + (i + 1).ToString("00") + "x2+" + (i + 2).ToString("00"), "gems/");
                recipe.AddIngredient("unit", gemtype + (i + 1).ToString("00"), 2);
                recipe.AddIngredient("unit", gemtype + (i + 2).ToString("00"), 1);
                recipe.AddResult("unit", gemtype + (i + 3).ToString("00"));
                recipe.Create();
            }
            /*
            for (int i = 0; i < quantity - 3; ++i)
            {
                recipe = new RecipeFile(gemtype + (i + 1).ToString("00") + "x8", "gems/");
                recipe.AddIngredient("unit", gemtype + (i + 1).ToString("00"), 8);
                recipe.AddResult("unit", gemtype + (i + 4).ToString("00"));
                recipe.Create();
            }
            //*/
            for (int i = 0; i < quantity - 3; ++i)
            {
                recipe = new RecipeFile(gemtype + (i + 1).ToString("00") + "x2+" + (i + 2).ToString("00")
                    + '+' + (i + 3).ToString("00"), "gems/");
                recipe.AddIngredient("unit", gemtype + (i + 1).ToString("00"), 2);
                recipe.AddIngredient("unit", gemtype + (i + 2).ToString("00"), 1);
                recipe.AddIngredient("unit", gemtype + (i + 3).ToString("00"), 1);
                recipe.AddResult("unit", gemtype + (i + 4).ToString("00"));
                recipe.Create();
            }
        }

        static void CreateAffixes()
        {
            double[] percents = new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10,
                                               12, 14, 16, 18, 20, 22, 24, 26, 28, 30,
                                               32, 34, 36, 38, 40, 42, 44, 46, 48, 50 };

            CreateGemAffix("Ruby", "strength bonus", 100, "damage reflection", 15, "xp gain bonus", percents);
            CreateGemAffix("Topaz", "magic", 100, "mana steal", 100, "percent gold drop", percents);
            CreateGemAffix("Emerald", "dexterity bonus", 100, "degrade armor", 15, "percent magical drop", percents);
            CreateGemAffix("Amethyst", "defense", 100, "life steal", 100, "fame gain bonus", percents);
            CreateGemAffix("Diamond", "armor bonus", 20, "damage bonus", 18, "potion efficiency", percents);
            CreateGemAffix("Aquamarine", "max hp", 12, "hp recharge player", 2, "percent pet health", percents);
            CreateGemAffix("Sapphire", "max mana", 12, "mana recharge player", 2, "percent pet damage", percents);
        }

        static void CreateGemAffix(string gemtype, string armorEffect, double armorValue,
                                   string weaponEffect, double weaponValue, string trinketEffect, double[] trinketValues)
        {
            EffectSec effect;
            TorchFile file;

            effect = new EffectSec("passive", armorEffect, armorValue, armorValue);
            file = new GemAffixFile(gemtype + "Armor", 1, 0, "armor", effect);
            file.Create();

            effect = new EffectSec("passive", weaponEffect, weaponValue, weaponValue);
            file = new GemAffixFile(gemtype + "Weapon", 1, 0, "weapon", effect);
            file.Create();

            bool isDamageBonue = trinketEffect == "pet damage bonus";
            for (int i = 0; i < trinketValues.Length; ++i)
            {
                if (isDamageBonue)
                    effect = new EffectSec("passive", trinketEffect, "all", trinketValues[i], trinketValues[i]);
                else
                    effect = new EffectSec("passive", trinketEffect, trinketValues[i], trinketValues[i]);
                file = new GemAffixFile(gemtype + "Trinket" + (i + 1).ToString("00"), 1, 0, "trinket", effect);
                file.Create();
            }
        }

        static void CreateImagesets()
        {
            //CreateGem("Amber");                 // orange
            CreateGemImageset("Amethyst");      // purple
            CreateGemImageset("Aquamarine");    // turquoise
            CreateGemImageset("Diamond");       // white
            CreateGemImageset("Emerald");       // green
            //CreateGem("Obsidian");              // black
            CreateGemImageset("Ruby");          // red
            CreateGemImageset("Sapphire");      // blue
            CreateGemImageset("Topaz");         // yellow
        }

        static void CreateGemImageset(string gemtype)
        {
            TorchFile file = new ImagesetFile(gemtype, "itemicons/", gemtype + ".png", 48, 48, 6, 5);
            file.Create();
        }

        static void CreateSocketables(ref int gc)
        {
            //CreateGem(ref gc, "Amber");         // orange
            CreateGem(ref gc, "Amethyst");      // purple
            CreateGem(ref gc, "Aquamarine");    // turquoise
            CreateGem(ref gc, "Diamond");       // white
            CreateGem(ref gc, "Emerald");       // green
            //CreateGem(ref gc, "Obsidian");      // black
            CreateGem(ref gc, "Ruby");          // red
            CreateGem(ref gc, "Sapphire");      // blue
            CreateGem(ref gc, "Topaz");         // yellow
        }

        static void CreateGem(ref int gc, string gemtype)
        {
            int[] levels = new int[] { 3, 8, 16, 24, 32, 40, 
                                       50, 60, 70, 80, 90, 100,
                                       120, 140, 160, 180, 200, 220,
                                       240, 300, 360, 420, 480, 540,
                                       600, 680, 760, 840, 920, 1000,
                                       10000000, 10000000, 10000000, 10000000, 
                                       10000000, 10000000, 10000000, 10000000 };

            string[] prefixes = new string[] { "Cracked", "Flawed", "Dull", "Refined", "Sparkling", "Dazzling" };
            string[] suffixes = new string[] { "Ore", "Gem", "Stone", "Block", "Star" };
            int quantity = prefixes.Length * suffixes.Length;
            string[] names = new string[quantity];
            int i = 0;
            foreach (string suffix in suffixes)
                foreach (string prefix in prefixes)
                    names[i++] = prefix + ' ' + gemtype + ' ' + suffix;

            int rarity = (int)Math.Pow(2, quantity - 1);
            for (i = 0; i < quantity; ++i)
            {
                AffixesSec affixSec = new AffixesSec(new string[] { gemtype + "Weapon", gemtype + "Armor", 
                                                                gemtype + "Trinket" + (i + 1).ToString("00") });
                SocketableFile file = new SocketableFile(gemtype + (i + 1).ToString("00"), names[i], gemtype + (i + 1).ToString(), 
                        affixSec, rarity, 500, levels[i], levels[i], levels[i + 8] - 1);
                file.unitGuid = (++gc).ToString();
                file.Create();
                rarity /= 2;
            }
        }

        static void CreateSkills()
        {
            CreatePassiveSkills();
            CreateAttackSkills();
        }

        static void CreatePassiveSkills()
        {
            EffectSec effect1, effect2, effect3;
            SkillFile skill;

            // make Adventurer affix
            effect1 = new EffectSec("passive", "xp gain bonus", 200, 200);
            effect1.name = "XPGAIN";
            effect1.duration = "always";
            effect1.graphOverride = "linear_graph";
            effect2 = new EffectSec("passive", "fame gain bonus", 200, 200);
            effect2.name = "FAMEGAIN";
            effect2.duration = "always";
            effect2.graphOverride = "linear_graph";
            effect3 = new EffectSec("passive", "potion efficiency", 100, 100);
            effect3.name = "POTIONEFF";
            effect3.duration = "always";
            effect3.graphOverride = "mastery_graph";
            CreateSkillAffix("skill_adventurer_mastery", new EffectSec[] { effect1, effect2, effect3 });

            // make Adventurer skill
            skill = new SkillFile("Adventurer", "shared/passives/");
            skill.displayName = skill.name;
            skill.description = "Improves the potency of potions, increases the rate of Experience and Fame gain, "
                        + "and reduces resurrection penalties";
        }

        static void CreateAttackSkills()
        {
            EffectSec effect1;

            // make the Slash Attack affix file
            effect1 = new EffectSec("dynamic", "damage", 40, 80);
            effect1.duration = "instant";
            effect1.statModifyName = "melee";
            effect1.statModifyPercent = "100";
            CreateSkillAffix("WhirlingGust", new EffectSec[] { effect1 });

            // prepare the Slash Attack skill file
            SkillFile skill = new SkillFile("WhirlingGust", "warrior/whirlinggust/");
            skill.name = "Whirling Gust";
            skill.displayName = "Slash Attack";
            skill.description = "Attacks all foes in front of you\\nwith all equipped weapons\\n";
            skill.skillIconActive = "skill_slash";
            skill.skillIconInactive = "skill_slash_gray";
            skill.animation = "Special_Cleave";
            skill.animationDW = "Special_DWCleave";
            skill.range = 2;
            skill.requirementRight = "melee";
            skill.requirementLeft = "melee";
            skill.maxInvestLevel = 100;
            skill.uniqueGUID = 7711097244085326302;
            CreateSkillLevels(skill, "WhirlingGust", "media/skills/warrior/whirlinggust/whirlinggust.layout", true);
            skill.Create();
        }

        static void CreateSkillAffix(string affixName, EffectSec[] effects)
        {
            AffixFile affix = new AffixFile(affixName, "Skills/", 1, 0, 0, 0, 9999999, new string[] { "any" }, effects);
            affix.Create();
        }

        static void CreateSkillLevels(SkillFile skill, string affixName, string layoutFile, bool isAttack)
        {
            LevelSec levelSec;
            // mana cost progression by skill level
            double manaCost = 2;
            double manaInc1 = 1;
            double manaInc2 = 0;
            double manaInc3 = 0.1;
            double manaInc4 = 0.005;
            // level requirement progression by skill level
            int levelReq = 1;
            int levelReqInc = 1;
            // affix level progression by skill level
            double affixLevel = 1;
            double affixLevelInc = 2;
            double affixLevelInc2 = 0.106;
            if (!isAttack)
            {
                affixLevelInc = 1;
                affixLevelInc2 = 0;
            }
            // add all the skill levels
            for (int skillLvl = 1; skillLvl <= 120; ++skillLvl)
            {
                // prepare all the needed sections
                if (isAttack)
                    levelSec = new LevelSec(skillLvl, (int)(manaCost + 0.5), levelReq);
                else
                    levelSec = new LevelSec(skillLvl, 0, levelReq);
                if (skillLvl > 100)
                    levelSec.levelRequired = 0;
                EventSec eventSec = new EventSec(EventSec.EVENT_TRIGGER, layoutFile);
                AffixesSec affixesSec = new AffixesSec(affixName);
                affixesSec.affixLevel = (int)(affixLevel + 0.5);
                // add the sections in the proper order
                eventSec.AddAffixes(affixesSec);
                levelSec.AddEvent(eventSec);
                skill.AddLevel(levelSec);

                // increase mana cost
                manaInc3 += manaInc4;
                manaInc2 += manaInc3;
                manaInc1 += manaInc2;
                manaCost += manaInc1;
                // increase level requirements
                if (skillLvl % 10 == 0)
                    ++levelReqInc;
                levelReq += levelReqInc;
                // increase affix level
                affixLevelInc += affixLevelInc2;
                affixLevel += affixLevelInc;
            }
        }

        static void CreateGraphs()
        {
            GraphFile file;
            file = new GraphFile("experience_monster_easy", "stats/", 1, 1200, 1, true, 8, 2, 0.4, 0);
            file.Create();
            file = new GraphFile("experience_monster", "stats/", 1, 1200, 1, true, 10, 2.5, 0.5, 0);
            file.Create();
            file = new GraphFile("experience_monster_hard", "stats/", 1, 1200, 1, true, 12, 3, 0.6, 0);
            file.Create();
            file = new GraphFile("experience_monster_veryhard", "stats/", 1, 1200, 1, true, 16, 4, 0.8, 0);
            file.Create();
            file = new GraphFile("experience_championmonster_easy", "stats/", 1, 1200, 1, true, 80, 20, 4, 0);
            file.Create();
            file = new GraphFile("experience_championmonster", "stats/", 1, 1200, 1, true, 100, 25, 5, 0);
            file.Create();
            file = new GraphFile("experience_championmonster_hard", "stats/", 1, 1200, 1, true, 120, 30, 6, 0);
            file.Create();
            file = new GraphFile("experience_championmonster_veryhard", "stats/", 1, 1200, 1, true, 160, 40, 8, 0);
            file.Create();
            file = new GraphFile("ExperienceGate", "stats/", 1, 1000, 1, false, 400, 600, 800, 100);
            file.Create();

            file = new GraphFile("fame_championmonster_easy", "stats/", 1, 1000, 1, true, 100, 20, 0, 0);
            file.Create();
            file = new GraphFile("fame_championmonster", "stats/", 1, 1000, 1, true, 125, 25, 0, 0);
            file.Create();
            file = new GraphFile("fame_championmonster_hard", "stats/", 1, 1000, 1, true, 150, 30, 0, 0);
            file.Create();
            file = new GraphFile("fame_championmonster_veryhard", "stats/", 1, 1000, 1, true, 200, 40, 0, 0);
            file.Create();
            file = new GraphFile("FameGate", "stats/", 1, 1000, 1, false, 1000, 2000, 2000, 1000);
            file.Create();

            file = new GraphFile("level_versus_level_experience_modifier-OLD", "stats/", -18, 80, 1, true, 2.35, 5.85, -0.05, 0);
            file = new GraphFile("level_versus_level_experience_modifier-OLD2", "stats/", -10, 1000, 1, true, 0, 10, 0, 0);
            file = new GraphFile("level_versus_level_experience_modifier", "stats/", -100, 200, 1, true, 0, 1, 0, 0);
            file.Create();

            file = new GraphFile("health_monster_bylevel_easy", "stats/", 1, 1000, 1, true, 20, 10, 2, 0.4);
            file.Create();
            file = new GraphFile("health_monster_bylevel", "stats/", 1, 1000, 1, true, 40, 20, 4, 0.8);
            file.Create();
            file = new GraphFile("health_monster_bylevel_hard", "stats/", 1, 1000, 1, true, 60, 30, 6, 1.2);
            file.Create();
            file = new GraphFile("health_monster_bylevel_veryhard", "stats/", 1, 1000, 1, true, 100, 50, 10, 2);
            file.Create();
            file = new GraphFile("health_championmonster_bylevel_easy", "stats/", 1, 1000, 1, true, 100, 50, 10, 2);
            file.Create();
            file = new GraphFile("health_championmonster_bylevel", "stats/", 1, 1000, 1, true, 200, 100, 20, 4);
            file.Create();
            file = new GraphFile("health_championmonster_bylevel_hard", "stats/", 1, 1000, 1, true, 300, 150, 30, 6);
            file.Create();
            file = new GraphFile("health_championmonster_bylevel_veryhard", "stats/", 1, 1000, 1, true, 500, 250, 50, 10);
            file.Create();

            file = new GraphFile("armor_monster_bylevel_easy", "stats/", 1, 1000, 1, true, 2, 4, 1.2, 0);
            file.Create();
            file = new GraphFile("armor_monster_bylevel", "stats/", 1, 1000, 1, true, 2.5, 5, 1.5, 0);
            file.Create();
            file = new GraphFile("armor_monster_bylevel_hard", "stats/", 1, 1000, 1, true, 3, 6, 1.8, 0);
            file.Create();
            file = new GraphFile("armor_monster_bylevel_veryhard", "stats/", 1, 1000, 1, true, 4, 8, 2.4, 0);
            file.Create();
            file = new GraphFile("armor_championmonster_bylevel_easy", "stats/", 1, 1000, 1, true, 2.5, 5.0, 1.5, 0);
            file.Create();
            file = new GraphFile("armor_championmonster_bylevel", "stats/", 1, 1000, 1, true, 3.125, 6.25, 1.875, 0);
            file.Create();
            file = new GraphFile("armor_championmonster_bylevel_hard", "stats/", 1, 1000, 1, true, 3.75, 7.5, 2.25, 0);
            file.Create();
            file = new GraphFile("armor_championmonster_bylevel_veryhard", "stats/", 1, 1000, 1, true, 5, 10, 3.0, 0);
            file.Create();

            file = new GraphFile("damage_monster_easy", "stats/", 1, 1000, 1, true, 12, 8, 2, 0);
            file.Create();
            file = new GraphFile("damage_monster", "stats/", 1, 1000, 1, true, 18, 12, 3, 0);
            file.Create();
            file = new GraphFile("damage_monster_hard", "stats/", 1, 1000, 1, true, 24, 16, 4, 0);
            file.Create();
            file = new GraphFile("damage_monster_veryhard", "stats/", 1, 1000, 1, true, 36, 24, 6, 0);
            file.Create();
            file = new GraphFile("damage_championmonster_easy", "stats/", 1, 1000, 1, true, 24, 16, 4, 0);
            file.Create();
            file = new GraphFile("damage_championmonster", "stats/", 1, 1000, 1, true, 36, 24, 6, 0);
            file.Create();
            file = new GraphFile("damage_championmonster_hard", "stats/", 1, 1000, 1, true, 48, 32, 8, 0);
            file.Create();
            file = new GraphFile("damage_championmonster_veryhard", "stats/", 1, 1000, 1, true, 72, 48, 12, 0);
            file.Create();

            // player health and mana will start with the same progression, but gradually increase by a faster rate
            file = new GraphFile("health_player_generic", "stats/", 1, 1000, 1, true, 200, 40, 4, 0);
            file.Create();
            file = new GraphFile("health_player_destroyer", "stats/", 1, 1000, 1, true, 300, 60, 6, 0);
            file.Create();
            file = new GraphFile("health_player_vanquisher", "stats/", 1, 1000, 1, true, 200, 40, 4, 0);
            file.Create();
            file = new GraphFile("health_player_alchemist", "stats/", 1, 1000, 1, true, 200, 40, 4, 0);
            file.Create();
            file = new GraphFile("mana_player_generic", "stats/", 1, 1000, 1, true, 20, 4, 0.4, 0);
            file.Create();
            file = new GraphFile("mana_player_destroyer", "stats/", 1, 1000, 1, true, 20, 4, 0.4, 0);
            file.Create();
            file = new GraphFile("mana_player_vanquisher", "stats/", 1, 1000, 1, true, 20, 4, 0.4, 0);
            file.Create();
            file = new GraphFile("mana_player_alchemist", "stats/", 1, 1000, 1, true, 20, 4, 0.4, 0);
            file.Create();

            // minions have as much health and armor as a champion (on easy), and as much damage as a champion has armor (on very hard)
            file = new GraphFile("armor_minion_bylevel", "stats/", 1, 1000, 1, true, 2.5, 5, 0.5, 0);
            file.Create();
            file = new GraphFile("damage_minion_bylevel", "stats/", 1, 1000, 1, true, 5, 10, 3, 0);
            file.Create();
            file = new GraphFile("health_minion_bylevel", "stats/", 1, 1000, 1, true, 50, 25, 5, 1.25);
            file.Create();

            file = new GraphFile("item_level_requirements", "stats/", 1, 1000, 1, true, 1, 1, 0, 0);
            file.Create();

            // item stat requirements are staying the same until i complete my item overhaul
            file = new GraphFile("item_strength_requirements", "stats/", 1, 1000, 1, true, 10, 2, 0, 0);
            file.Create();
            file = new GraphFile("item_dexterity_requirements", "stats/", 1, 1000, 1, true, 10, 2, 0, 0);
            file.Create();
            file = new GraphFile("item_magic_requirements", "stats/", 1, 1000, 1, true, 10, 2, 0, 0);
            file.Create();
            file = new GraphFile("item_defense_requirements", "stats/", 1, 1000, 1, true, 10, 2, 0, 0);
            file.Create();

            // gold drops increase with each difficulty, and gradually increase more at higher levels
            file = new GraphFile("golddrop_easy", "stats/", 1, 1000, 1, true, 1, 0.2, 0.02, 0);
            file.Create();
            file = new GraphFile("golddrop", "stats/", 1, 1000, 1, true, 1.5, 0.3, 0.03, 0);
            file.Create();
            file = new GraphFile("golddrop_hard", "stats/", 1, 1000, 1, true, 2, 0.4, 0.04, 0);
            file.Create();
            file = new GraphFile("golddrop_veryhard", "stats/", 1, 1000, 1, true, 3, 0.6, 0.06, 0);
            file.Create();

            // item prices scale better with gold drops
            file = new GraphFile("price_playerbuy_normal", "stats/", 1, 1000, 1, true, 50, 10, 1, 0);
            file.Create();
            file = new GraphFile("price_playerbuy_magic", "stats/", 1, 1000, 1, true, 200, 40, 4, 0);
            file.Create();
            file = new GraphFile("price_playerbuy_unique", "stats/", 1, 1000, 1, true, 1000, 200, 20, 0);
            file.Create();
            file = new GraphFile("price_playersell_normal", "stats/", 1, 1000, 1, true, 5, 1, 0.1, 0);
            file.Create();
            file = new GraphFile("price_playersell_magic", "stats/", 1, 1000, 1, true, 20, 4, 0.4, 0);
            file.Create();
            file = new GraphFile("price_playersell_unique", "stats/", 1, 1000, 1, true, 100, 20, 2, 0);
            file.Create();
            file = new GraphFile("price_enchant", "stats/", 1, 1000, 1, true, 200, 40, 4, 0);
            file.Create();
            file = new GraphFile("price_playergamble_magic", "stats/", 1, 1000, 1, true, 200, 40, 4, 0);
            file.Create();

            // these item affixes are staying the same for now
            file = new GraphFile("attribute_bonus", "stats/", 1, 1000, 1, true, 1, 0.3333333333, 0, 0);
            file.Create();
            file = new GraphFile("base_weapon_damage", "stats/", 1, 1000, 1, true, 21, 6, 0, 0);
            file.Create();

            // experimenting with the following item affixes
            file = new GraphFile("steal_health_and_mana", "stats/", 1, 1000, 1, true, 5, 1, 0.1, 0);
            file.Create();

            // new percentage graph for Evolved Ember
            file = new GraphFile("gem_percent", "stats/", 1, 9, 1, true, 1, 1, 0, 0);
            file.AddLine(10, 30, 1, 10, 2);
            file.Create();

            // mastery graphs for Evolved Abilities
            file = new GraphFile("mastery_graph", "stats/", 1, 24, 1, true, 4, 4, 0, 0);
            file.AddLine(25, 120, 1, 100, 2);
            file.Create();
            file = new GraphFile("mastery_graph_2", "stats/", 1, 9, 1, true, 1, 1, 0, 0);
            file.AddLine(10, 29, 1, 10, 0.5);
            file.AddLine(30, 59, 1, 20, 0.3333333333);
            file.AddLine(60, 120, 1, 30, 0.25);
            file.Create();
        }
    }
}
