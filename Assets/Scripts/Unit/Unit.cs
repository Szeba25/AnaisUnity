namespace Anais {

    public class Unit {

        public Body Body { get; }
        public Behavior Behavior { get; }
        public Stats Stats { get; }

        public Unit(Body body, Behavior behavior, Stats stats) {
            // Assign components
            Body = body;
            Behavior = behavior;
            Stats = stats;

            // Register components
            Body.Register(stats);
            Behavior.Register(body, stats);
        }

    }

}
