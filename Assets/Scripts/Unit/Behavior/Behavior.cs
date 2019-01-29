namespace Anais {

    public class Behavior {

        // Registered components
        private Body body;
        private Stats stats;

        public Behavior() {
            // Set registered components to null
            body = null;
            stats = null;
        }

        public void Register(Body body, Stats stats) {
            this.body = body;
            this.stats = stats;
        }

    }

}
