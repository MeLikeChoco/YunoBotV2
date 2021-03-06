﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YunoBotV2.Services.Rpg.NameGen.PopCulture
{
    [GenName("animeattacks")]
    public class AnimeAttacks : IGenerator
    {

        public IEnumerable<string> GeneratedResults { get; }

        private static readonly string[] Attack1 = new string[] { "Abominable", "Aching", "Adamantine", "Adept", "Advanced", "Aggressive",
            "Agile", "Agonizing", "Amplified", "Ancient", "Angelic", "Angry", "Arch", "Azure", "Basic", "Black", "Blinding", "Bright",
            "Brilliant", "Brutal", "Burning", "Careless", "Chaotic", "Chief", "Classic", "Clean", "Colossal", "Combination", "Complete",
            "Complex", "Composed", "Confusing", "Corrupt", "Counter", "Courageous", "Crazed", "Crazy", "Crimson", "Cruel", "Crushing",
            "Dancing", "Dark", "Dead", "Deadly", "Defensive", "Defiant", "Definitive", "Delayed", "Delirious", "Demonic", "Devouring",
            "Dieing", "Diligent", "Direct", "Dirty", "Double", "Draconic", "Drunken", "Dual", "Dynamic", "Eager", "Elegant", "Elementary",
            "Enchanted", "Enchanting", "Enlightened", "Enraged", "Executing", "Expert", "Extreme", "Fading", "Faint", "Fake", "Falling",
            "False", "Fatal", "Fearless", "Feral", "Firm", "First", "Flawless", "Flowing", "Flying", "Focused", "Forbidden", "Forsaken",
            "Frozen", "Furious", "Giant", "Gigantic", "Gilded", "Glaring", "Golden", "Graceful", "Grand", "Grave", "Greater", "Grim",
            "Hallowed", "Haunting", "Hidden", "High", "Hollow", "Holy", "Honored", "Humming", "Hungry", "Identical", "Impending", "Impossible",
            "Impure", "Infinite", "Iron", "Leaping", "Light", "Living", "Lucky", "Majestic", "Major", "Minor", "Misty", "Mithril", "Mixed",
            "Monstrous", "Mortal", "Mysterious", "Nimble", "Numbing", "Obsidian", "Parallel", "Perfect", "Powerful", "Primal", "Primary",
            "Prime", "Pristine", "Puny", "Pure", "Quick", "Quiet", "Rabid", "Ragged", "Rapid", "Raw", "Reckless", "Reflecting", "Regal",
            "Rotten", "Rough", "Royal", "Sadistic", "Savage", "Scarlet", "Scathing", "Sealed", "Secret", "Serene", "Severing", "Shallow",
            "Shameless", "Shining", "Shocking", "Silent", "Silver", "Simple", "Single", "Skeletal", "Slaying", "Smooth", "Sneaky", "Staight",
            "Steel", "Stunning", "Subtle", "Super", "Superior", "Surging", "Surprise", "Swift", "Thundering", "Timeless", "True", "Twin",
            "Unholy", "Unleashed", "Vacuum", "Vengeful", "Vicious", "Vile", "Violent", "Violet", "Warped", "Weeping", "Whispered", "White",
            "Wicked", "Wild", "Winding", "Wise", "Wretched" };
        private static readonly string[] Attack2 = new string[] { "Angel", "Annihilation", "Arachnid", "Armageddon", "Ash", "Assassin",
            "Aura", "Autumn", "Avalanche", "Bane", "Barrier", "Bear", "Beast", "Behemoth", "Bird", "Blessing", "Blizzard", "Blood",
            "Blossom", "Body", "Bomb", "Bone", "Boulder", "Bubble", "Bug", "Butterfly", "Camouflage", "Canine", "Chain", "Chakra", "Chaos",
            "Claw", "Clone", "Cloud", "Cocoon", "Cold", "Comet", "Cosmos", "Crab", "Crane", "Crescent", "Cricket", "Crystal", "Darkness",
            "Day", "Daydream", "Death", "Deception", "Delusion", "Demon", "Diamond", "Dragon", "Dragonfly", "Dream", "Droplet", "Dust",
            "Earth", "Earthquake", "Echo", "Electric", "Elemental", "Elephant", "Enigma", "Execution", "Exorcism", "Explosion", "Falcon",
            "Fang", "Feather", "Feline", "Fire", "Flame", "Frost", "Frostfire", "Fury", "Ghost", "Giant", "God", "Golem", "Gravity", "Hawk",
            "Head", "Heaven", "Hell", "Hellfire", "Hornet", "Horror", "Hot", "Hurricane", "Ice", "Illusion", "Impact", "Inferno", "Infinity",
            "Instant", "Jaw", "Judgment", "Ki", "Lava", "Leaf", "Leech", "Legend", "Light", "Lightning", "Lily", "Limbo", "Lion", "Lotus",
            "Magma", "Mania", "Maple", "Melting", "Meteor", "Mime", "Mind", "Mirror", "Mist", "Monkey", "Moon", "Moonlight", "Mountain",
            "Needle", "Night", "Nightmare", "Nova", "Oak", "Oracle", "Palm", "Panther", "Paragon", "Paralysis", "Petal", "Phantom", "Phoenix",
            "Poison", "Prism", "Prison", "Propulsion", "Puppet", "Quick", "Rage", "Rain", "Rainbow", "Retribution", "Reverse", "River",
            "Rock", "Sand", "Scorpion", "Secret", "Serpent", "Shadow", "Shark", "Skeleton", "Sky", "Smoke", "Snake", "Snow", "Soul", "Spider",
            "Spiral", "Spirit", "Spring", "Starfall", "Starlight", "Stealth", "Stone", "Storm", "Summer", "Sun", "Sunlight", "Supreme",
            "Talon", "Teardrop", "Terror", "Thorn", "Thunder", "Tiger", "Tornado", "Toxin", "Tree", "Turtle", "Twin", "Typhoon", "Vapor",
            "Vengeance", "Venom", "Void", "Vortex", "Water", "Waterfall", "Whirlwind", "Wild", "Willow", "Wind", "Wing", "Winter", "Wolf",
            "Yang", "Yin" };
        private static readonly string[] Attack3 = new string[] { "Ambush", "Assault", "Attack", "Barrage", "Bash", "Binding", "Bite",
            "Blade", "Blast", "Blitz", "Blow", "Bolt", "Bullet", "Burst", "Bust", "Cannon", "Chain", "Charge", "Chop", "Clap", "Claw",
            "Coil", "Crack", "Crush", "Cut", "Dance", "Dive", "Drain", "Eruption", "Fall", "Fang", "Fist", "Fists", "Flash", "Flush",
            "Force", "Fracture", "Hit", "Hurl", "Impact", "Jolt", "Kick", "Kiss", "Knock", "Launch", "Method", "Pound", "Punch", "Push",
            "Raze", "Release", "Rush", "Seal", "Shatter", "Shock", "Shot", "Slam", "Slash", "Smash", "Spear", "Spell", "Stab", "Stomp",
            "Strike", "Surge", "Technique", "Thrust", "Trap", "Trash", "Volley", "Wave", "Whip" };
        private static readonly string[] Attack4 = new string[] { "Admired", "Adored", "Advanced", "Aether", "Alert", "Anchored", "Ancient",
            "Angelic", "Arctic", "Aromatic", "Authentic", "Autumn", "Beautiful", "Beloved", "Bitter", "Bleak", "Blind", "Blissful", "Bold",
            "Bright", "Brilliant", "Broken", "Bronze", "Calm", "Careful", "Careless", "Clouded", "Colossal", "Common", "Composed", "Corrupt",
            "Crescent", "Cruel", "Damaged", "Dapper", "Darling", "Dearest", "Defensive", "Demise", "Demonic", "Dependable", "Determined",
            "Devoted", "Diligent", "Dual", "Dutiful", "Eager", "Earnest", "Echo", "Eclipse", "Elegant", "Enchanted", "Enigma", "Esteemed",
            "Everlasting", "Evil", "Exalted", "False", "Fatal", "Fearless", "Flawless", "Focused", "Forceful", "Forsaken", "Fortunate",
            "Gentle", "Giant", "Gigantic", "Glacier", "Glorious", "Golden", "Graceful", "Grand", "Grave", "Gravity", "Grim", "Hallowed",
            "Harmonic", "Harmonious", "Haunting", "Heaven", "Heavenly", "Hell", "Hibernating", "Hidden", "Hollow", "Honored", "Horizon",
            "Humble", "Hungry", "Illusion", "Imitation", "Immortal", "Juvenile", "Kings", "Last", "Legend", "Light", "Living", "Lone", "Lonely",
            "Lost", "Loyal", "Lucky", "Lunar", "Majestic", "Menacing", "Mild", "Mysterious", "Nimble", "Outlandish", "Parallel", "Parasitic",
            "Peaceful", "Perfect", "Poison", "Powerful", "Prestigious", "Prime", "Prism", "Prison", "Proud", "Quick", "Quiet", "Radiant",
            "Rainbow", "Regal", "Royal", "Secret", "Serene", "Shadow", "Silent", "Skeletal", "Solar", "Spiteful", "Spring", "Stark", "Summer",
            "Swift", "Toxin", "True", "Twilight", "Twin", "Vengeful", "Venom", "Void", "Vortex", "Watchful", "Weather", "Wicked", "Wild",
            "Winter", "Wise", "Worthy", "Wretched", "Zealous" };
        private static readonly string[] Attack5 = new string[] { "Alligator", "Anaconda", "Angel", "Ape", "Armadillo", "Assassin", "Aura",
            "Axe", "Baboon", "Badger", "Bandicoot", "Basilisk", "Bear", "Beast", "Beaver", "Bee", "Beetle", "Behemoth", "Bird", "Blade",
            "Blood", "Blossom", "Boa", "Boar", "Body", "Bolt", "Branch", "Brook", "Buffalo", "Bug", "Butterfly", "Cat", "Chain", "Chameleon",
            "Chimp", "Claw", "Clone", "Cloud", "Cobra", "Cocoon", "Comet", "Condor", "Cougar", "Crab", "Crane", "Cricket", "Crocodile", "Crow",
            "Deer", "Demon", "Dove", "Dragon", "Dragonfly", "Eagle", "Edge", "Elephant", "Falcon", "Fang", "Feather", "Firefly", "Fish",
            "Flame", "Fog", "Forrest", "Fox", "Frog", "Ghost", "Giant", "Goat", "God", "Golem", "Gorilla", "Grass", "Hare", "Hawk", "Head",
            "Hippo", "Hook", "Hornet", "Horse", "Hyena", "Ice", "Jackal", "Jaguar", "Katana", "Leaf", "Leech", "Lemur", "Light", "Lightning",
            "Lily", "Lizard", "Locust", "Lotus", "Lynx", "Macaw", "Magpie", "Mandrill", "Mantis", "Meteor", "Mime", "Mind", "Mist", "Monkey",
            "Moon", "Moose", "Mountain", "Mouse", "Mushroom", "Needle", "Night", "Nightingale", "Octopus", "Oracle", "Panda", "Panther",
            "Paragon", "Petal", "Phantom", "Phoenix", "Puppet", "Raccoon", "Rain", "Ram", "Rat", "Raven", "Rhino", "Rock", "Root", "Rose",
            "Salamander", "Scorpion", "Shark", "Shield", "Smoke", "Snake", "Snow", "Spear", "Spider", "Spirit", "Star", "Sun", "Swallow",
            "Swan", "Sword", "Tiger", "Titan", "Toad", "Tortoise", "Tree", "Turtle", "Viper", "Vulture", "Wasp", "Water", "Wolf", "Wolverine",
            "World" };
        private static readonly string[] Attack6 = new string[] { "Accelerating", "Activating", "Adapting", "Alighting", "Anticipating",
            "Arising", "Assembling", "Attaching", "Attacking", "Balancing", "Battling", "Beating", "Bending", "Binding", "Biting", "Bleeding",
            "Blessing", "Blinding", "Blowing", "Boiling", "Bolting", "Bouncing", "Breaking", "Breathing", "Burning", "Bursting", "Carving",
            "Casting", "Catching", "Changing", "Charging", "Chasing", "Cheating", "Chopping", "Clinging", "Coiling", "Coming", "Commanding",
            "Confusing", "Constructing", "Containing", "Contracting", "Controlling", "Copying", "Cracking", "Crashing", "Crawling", "Crossing",
            "Crushing", "Crying", "Curving", "Cutting", "Dancing", "Deceiving", "Destroying", "Directing", "Diverting", "Dividing", "Diving",
            "Doubling", "Draining", "Dreaming", "Drinking", "Drowning", "Drumming", "Eating", "Eliminating", "Ending", "Enduring", "Enforcing",
            "Enhancing", "Escaping", "Expanding", "Exploding", "Extending", "Extracting", "Fading", "Fearing", "Feeding", "Fetching", "Fighting",
            "Firing", "Flowing", "Flying", "Forcing", "Freezing", "Frightening", "Frying", "Glowing", "Grabbing", "Grinding", "Gripping",
            "Growing", "Guarding", "Harming", "Hating", "Hiding", "Hovering", "Hurting", "Hypnotizing", "Injecting", "Injuring", "Intensifying",
            "Interrupting", "Jesting", "Judging", "Kicking", "Killing", "Knocking", "Laughing", "Launching", "Leaping", "Living", "Melting",
            "Misleading", "Mixing", "Multiplying", "Obeying", "Overflowing", "Perfecting", "Pinching", "Planting", "Possessing", "Praying",
            "Pretending", "Preying", "Protecting", "Punching", "Puncturing", "Punishing", "Radiating", "Raging", "Raining", "Reducing",
            "Reflecting", "Reigning", "Releasing", "Removing", "Repeating", "Revealing", "Riding", "Ruining", "Ruling", "Running", "Rushing",
            "Saving", "Scattering", "Scorching", "Scratching", "Screaming", "Shining", "Shocking", "Shooting", "Singing", "Sinning", "Slaying",
            "Sleeping", "Smashing", "Smiting", "Sneaking", "Spinning", "Splitting", "Stinging", "Striking", "Suffering", "Tearing", "Thrusting",
            "Transforming", "Twisting", "Vanishing", "Wandering", "Watching", "Whirling", "Whistling" };

        public AnimeAttacks(int amount, int type)
        {

            if (type != 0)
                throw new ArgumentOutOfRangeException("type", type, "Anime attacks only takes 1 type. 0: default.");

            var array = new string[amount];

            for(int i = 0; i < amount; i++)
            {

                var r = Rand.Next(11);
                
                if(r < 2)
                {

                    var r1 = Rand.Next(Attack2.Length);
                    var r2 = Rand.Next(Attack3.Length);

                    array[i] = $"{Attack2[r1]} {Attack3[r2]}";

                }
                else if(r < 4)
                {

                    var r1 = Rand.Next(Attack1.Length);
                    var r2 = Rand.Next(Attack2.Length);
                    var r3 = Rand.Next(Attack3.Length);

                    array[i] = $"{Attack1[r1]} {Attack2[r2]} {Attack3[r3]}";

                }
                else if(r < 6)
                {

                    var r1 = Rand.Next(Attack4.Length);
                    var r2 = Rand.Next(Attack5.Length);
                    var r3 = Rand.Next(Attack6.Length);
                    var r4 = Rand.Next(Attack3.Length);

                    array[i] = $"{Attack4[r1]} {Attack5[r2]}, {Attack6[r3]} {Attack3[r4]}";

                }
                else if(r < 8)
                {

                    var r1 = Rand.Next(Attack4.Length);
                    var r2 = Rand.Next(Attack5.Length);
                    var r3 = Rand.Next(Attack6.Length);
                    var r4 = Rand.Next(Attack5.Length);

                    while (r2 == r4)
                        r2 = Rand.Next(Attack5.Length);

                    array[i] = $"{Attack4[r1]} {Attack5[r2]} {Attack6[r3]} {Attack5[r4]}";

                }
                else
                {

                    var r1 = Rand.Next(Attack6.Length);
                    var r2 = Rand.Next(Attack3.Length);
                    var r3 = Rand.Next(Attack4.Length);
                    var r4 = Rand.Next(Attack5.Length);

                    array[i] = $"{Attack6[r1]} {Attack3[r2]} of {Attack4[r3]} {Attack5[r4]}";

                }

            }

            GeneratedResults = array;

        }

    }
}
