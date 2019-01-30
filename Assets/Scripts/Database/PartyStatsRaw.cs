using System;

namespace Anais {

    [Serializable]
    public class StatsRaw {
        public string name;

        public int vitality;
        public int strength;
        public int dexterity;
        public int spellpower;
        public int willpower;
        public int piety;

    }

    [Serializable]
    public class PartyStatsRaw {
        public StatsRaw[] stats;
    }

}
