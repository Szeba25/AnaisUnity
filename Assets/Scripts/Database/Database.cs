using System.IO;
using UnityEngine;

namespace Anais {

    public class Database {

        private PartyStatsRaw partyStatsRaw;

        public Database() {
            string path = Path.Combine(Application.streamingAssetsPath, "Database/Party/stats.json");
            string data = File.ReadAllText(path);
            partyStatsRaw = JsonUtility.FromJson<PartyStatsRaw>(data);
            Debug.Log("Database JSON test: " + partyStatsRaw.stats[0].name);
        }

    }

}
