namespace GTA
{
    class World
    {
        private static World _instance;

        private World()
        {

        }

        public static World GetInstance()
        {
            return _instance ?? (_instance = new World());
        }

        public void Update()
        {
            
        }

        public void Render()
        {
            
        }
    }
}
