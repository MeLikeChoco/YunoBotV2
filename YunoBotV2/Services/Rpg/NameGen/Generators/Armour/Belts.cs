using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YunoBotV2.Services.Rpg.NameGen.Armour
{
    [GenName("belts")]
    public class Belts : IGenerator
    {

        public IEnumerable<string> GeneratedResults { get; }

        private static readonly string[] Name1 = new string[] { "Ancient", "Binding", "Blessed", "Blind", "Broken", "Burning", "Condemned", "Conquered",
            "Cursed", "Damned", "Dark", "Demonic", "Distant", "Divine", "Doomed", "Ending", "Endless", "Eternal", "Faded", "Fallen", "Fleeting", "Frozen",
            "Hallowed", "Haunted", "Hellish", "Holy", "Imminent", "Immortal", "Infernal", "Infinite", "Lost", "Ominous", "Relentless", "Sacred", "Shattered",
            "Silent", "Smoldering", "Timeless", "Twisted", "Unholy" };
        private static readonly string[] Name2 = new string[] { "Ancestors", "Bloodlust", "Comrades", "Damnation", "Dreams", "Fire", "Fires", "Fortune",
            "Fortunes", "Freedom", "Glory", "Hell", "Hells", "Honor", "Hope", "Illusions", "Justice", "Kings", "Lands", "Magic", "Memories", "Might",
            "Misery", "Nightmares", "Nights", "Power", "Powers", "Protection", "Punishment", "Sorrow", "Souls", "Torment", "Trials", "Vengeance", "Visions",
            "Voices", "Warlords", "Wars", "Whispers", "Worlds" };
        private static readonly string[] Name3 = new string[] { "Ancient", "Arcane", "Atuned", "Bandit's", "Baneful", "Banished", "Barbarian", "Barbaric",
            "Battleworn", "Blood Infused", "Blood-Forged", "Bloodcursed", "Bloodied", "Bloodlord's", "Bloodsurge", "Brutal", "Brutality", "Burnished",
            "Captain's", "Cataclysm", "Cataclysmic", "Challenger", "Challenger's", "Champion", "Champion's", "Cold-Forged", "Conqueror", "Conqueror's",
            "Corrupted", "Crazed", "Crying", "Cursed", "Defender", "Defender's", "Defiled", "Demonic", "Desire's", "Desolation", "Destiny's", "Dire", "Doom",
            "Doom's", "Dragon's", "Dragon", "Ebon", "Enchanted", "Engraved", "Eternal", "Exile", "Extinction", "Faith's", "Faithful", "Fearful", "Feral",
            "Fierce", "Fiery", "Fire Infused", "Firesoul", "Forsaken", "Fortune's", "Frenzied", "Frost", "Frozen", "Furious", "Fusion", "Ghastly", "Ghost-Forged",
            "Ghostly", "Gladiator", "Gladiator's", "Grieving", "Guard's", "Guardian's", "Hatred", "Haunted", "Heartless", "Hero", "Hero's", "Hollow", "Holy",
            "Honed", "Honor's", "Hope's", "Hopeless", "Howling", "Hungering", "Incarnated", "Infused", "Inherited", "Jade Infused", "Judgement", "Keeper's",
            "Knightly", "Legionnaire's", "Liar's", "Lich", "Lightning", "Lonely", "Loyal", "Lusting", "Malevolent", "Malicious", "Malignant", "Massive", "Mended",
            "Mercenary", "Military", "Misfortune's", "Mourning", "Nightmare", "Oathkeeper's", "Ominous", "Peacekeeper", "Peacekeeper's", "Phantom", "Possessed",
            "Pride's", "Primal", "Prime", "Primitive", "Promised", "Protector's", "Proud", "Recruit's", "Reforged", "Reincarnated", "Relentless", "Remorse",
            "Renewed", "Renovated", "Restored", "Retribution", "Ritual", "Roaring", "Ruby Infused", "Rune-Forged", "Savage", "Sentinel", "Shadow", "Silent",
            "Singing", "Sinister", "Soldier's", "Solitude's", "Sorrow's", "Soul", "Soul Infused", "Soul-Forged", "Soulless", "Spectral", "Spite", "Storm",
            "Storm-Forged", "Stormfury", "Stormguard", "Terror", "Thunder", "Thunder-Forged", "Thunderfury", "Thunderguard", "Thundersoul", "Thunderstorm",
            "Timeworn", "Tormented", "Trainee's", "Treachery's", "Twilight", "Twilight's", "Twisted", "Tyrannical", "Undead", "Unholy", "Vanquisher", "Vengeance",
            "Vengeful", "Vicious", "Victor", "Vindication", "Vindicator", "Vindictive", "War-Forged", "Warden's", "Warlord's", "Warped", "Warrior", "Warrior's",
            "Whistling", "Wicked", "Wind's", "Wind-Forged", "Windsong", "Woeful", "Wrathful", "Wretched", "Yearning", "Zealous", "", "", "", "", "", "", "", "", "" };
        private static readonly string[] Name41 = new string[] { "Adamantite", "Scaled", "Bone", "Bronze", "Bronzed", "Ivory", "Ebon", "Golden", "Iron", "Mithril",
            "Obsidian", "Silver", "Skeletal", "Steel", "Titanium", "Demon" };
        private static readonly string[] Name42 = new string[] { "Cloth", "Heavy Hide", "Heavy Leather", "Hide", "Leather", "Linen", "Padded", "Rugged Leather",
            "Scaled", "Silk", "Wool", "Embroided" };
        private static readonly string[] Name51 = new string[] { "Belt", "Gunbelt", "Girdle", "Links", "Waistband", "Waistguard", "Band", "Chain", "Cord" };
        private static readonly string[] Name52 = new string[] { "Belt", "Sash", "Strap", "Girdle", "Waistband", "Cord", "Gunbelt" };
        private static readonly string[] Name6 = new string[] { "Absorption", "Heavy Loads", "Carrying", "Heavy Pants", "Perseverance", "Firmness", "Stability",
            "Steady Hands", "Fidelity", "Silence", "Muffled Steps", "Fleet Feet", "the Phoenix", "Adventure", "Agony", "Ancient Power", "Ancient Powers", "Anger",
            "Anguish", "Annihilation", "Arcane Magic", "Arcane Power", "Arcane Resist", "Archery", "Ashes", "Assassination", "Assassins", "Assaults", "Auras",
            "Awareness", "Barriers", "Beginnings", "Binding", "Black Magic", "Blast Protection", "Blessed Fortune", "Blessed Fortunes", "Blessings", "Blight",
            "Blood", "Bloodlust", "Bloodshed", "Bravery", "Broken Bones", "Broken Dreams", "Broken Families", "Broken Worlds", "Burdens", "Carnage", "Cataclysms",
            "Chaos", "Clarity", "Conquered Worlds", "Corruption", "Courage", "Creation", "Cunning", "Danger", "Dark Magic", "Dark Powers", "Dark Souls", "Darkness",
            "Dawn", "Decay", "Deception", "Defiance", "Deflection", "Delirium", "Delusions", "Demon Fire", "Demons", "Denial", "Desecration", "Despair", "Destruction",
            "Devotion", "Diligence", "Discipline", "Dishonor", "Dismay", "Dominance", "Domination", "Doom", "Dragons", "Dragonsouls", "Dread", "Dreams", "Due Diligence",
            "Duels", "Dusk", "Echoes", "Enchantments", "Ended Dreams", "Ending Hope", "Ending Misery", "Ends", "Eternal Bloodlust", "Eternal Damnation", "Eternal Glory",
            "Eternal Justice", "Eternal Rest", "Eternal Sorrow", "Eternal Struggles", "Eternity", "Executions", "Extinction", "Faded Memories", "Fallen Kings",
            "Fallen Souls", "Fire", "Fire Magic", "Fire Power", "Fire Protection", "Fire Resist", "Fools", "Forging", "Fortitude", "Fortune", "Frost", "Frost Power",
            "Frost Resist", "Frozen Hells", "Fury", "Ghosts", "Giants", "Giantslaying", "Glory", "Grace", "Greed", "Grieving Widows", "Guardians", "Hate", "Hatred",
            "Healing", "Hell", "Hell's Games", "Hellfire", "Hellish Torment", "Heroes", "Holy Might", "Honor", "Hope", "Horrors", "Ice", "Ice Magic", "Illusions",
            "Immortality", "Inception", "Infinite Trials", "Infinity", "Insanity", "Justice", "Kings", "Life", "Lifemending", "Lifestealing", "Light's Hope", "Limbo",
            "Lost Comrades", "Lost Hope", "Lost Souls", "Lost Voices", "Lost Worlds", "Mercy", "Might", "Miracles", "Misery", "Mists", "Moonlight", "Mysteries",
            "Mystery", "Nature", "Necromancy", "Nightmares", "Oblivion", "Paradise", "Patience", "Phantoms", "Power", "Prayers", "Pride", "Pride's Fall", "Prophecies",
            "Protection", "Putrefaction", "Reckoning", "Recoil", "Redemption", "Regret", "Regrets", "Resilience", "Respect", "Riddles", "Ruins", "Runes", "Salvation",
            "Secrecy", "Secrets", "Serenity", "Shadows", "Shifting Sands", "Silence", "Slaughter", "Slaying", "Smite", "Solitude", "Souls", "Stealth", "Stone", "Storms",
            "Strength", "Subtlety", "Suffering", "Suffering's End", "Sunfire", "Sunlight", "Swordbreaking", "Tears", "Terror", "Terrors", "Thieves", "Thorns", "Thunder",
            "Thunders", "Titans", "Torment", "Traitors", "Trust", "Truth", "Truths", "Twilight", "Twilight's End", "Twisted Visions", "Undoing", "Unholy Blight",
            "Unholy Might", "Valiance", "Valor", "Vengeance", "Vigor", "Visions", "War", "Whispers", "Wisdom", "Woe", "Wonders", "Wraiths", "Zeal", "the Ancients",
            "the Archer", "the Banished", "the Basilisk", "the Bear", "the Beast", "the Berserker", "the Blessed", "the Boar", "the Breaking Storm", "the Brotherhood",
            "the Burning Sun", "the Caged Mind", "the Cataclysm", "the Champion", "the Claw", "the Corrupted", "the Covenant", "the Crown", "the Crusader", "the Damned",
            "the Day", "the Daywalker", "the Dead", "the Depth", "the Depths", "the Dragons", "the Dreadlord", "the Eagle", "the Earth", "the East", "the Eclipse",
            "the Emperor", "the End", "the Enigma", "the Fallen", "the Falling Sky", "the Flames", "the Forest", "the Forests", "the Forgotten", "the Forsaken",
            "the Gargoyle", "the Gladiator", "the Gods", "the Harvest", "the Hunter", "the Immortal", "the Immortals", "the Incoming Storm", "the Insane", "the Isles",
            "the King", "the Knight", "the Lasting Night", "the Leviathan", "the Light", "the Lion", "the Lionheart", "the Lone Victor", "the Lone Wolf", "the Lost",
            "the Mage", "the Moon", "the Moonwalker", "the Mountain", "the Mountains", "the Night", "the Night Sky", "the Nightstalker", "the North", "the Occult",
            "the Oracle", "the Phoenix", "the Plague", "the Prince", "the Princess", "the Prisoner", "the Prodigy", "the Prophecy", "the Prophet", "the Protector",
            "the Queen", "the Scourge", "the Seer", "the Serpent", "the Setting Sun", "the Shadows", "the Sky", "the South", "the Stars", "the Steward", "the Storm",
            "the Summoner", "the Sun", "the Sunwalker", "the Swamp", "the Talon", "the Titans", "the Undying", "the Victor", "the Void", "the Volcano", "the Ward",
            "the Warrior", "the West", "the Whale", "the Whispers", "the Wicked", "the Wind", "the Wolf", "the World", "the Wretched" };

        public Belts(int amount, int type)
        {

            if (type != 0 && type != 1)
                throw new ArgumentOutOfRangeException("type", type, "Belts only take 2 types, 0 or 1. 0: heavy belts, 1: light belts.");

            var array = new string[amount];

            for (int i = 0; i < amount; i++)
            {

                var r = Rand.Next(11);
                var r1 = Rand.Next(Name1.Length);
                var r2 = Rand.Next(Name2.Length);
                var r3 = Rand.Next(Name3.Length);
                var r4 = Rand.Next(type == 1 ? Name42.Length : Name41.Length);
                var r5 = Rand.Next(type == 1 ? Name52.Length : Name51.Length);
                var r6 = Rand.Next(Name6.Length);

                var n4 = type == 1 ? Name42[r4] : Name41[r4];
                var n5 = type == 1 ? Name52[r5] : Name51[r5];

                if (r < 2)
                    array[i] = $"{n5} of {Name1[r1]} {Name2[r2]}";
                else if (r < 4)
                    array[i] = $"{n4} {n5} of {Name1[r1]} {Name2[r2]}";
                else if (r < 7)
                    array[i] = $"{Name3[r3]} {n4} {n5}";
                else
                    array[i] = $"{Name3[r3]} {n5} of {Name6[r6]}";

            }

            GeneratedResults = array;

        }

    }
}
