using System.Collections.Generic;
using System;
namespace GTA
{
    class World
    {
        private static World _instance;
        private readonly List<BaseGameEntity> _entities;

        private World()
        {
            _entities = new List<BaseGameEntity> {new Thug()};
        }

        public static World GetInstance()
        {
            return _instance ?? (_instance = new World());
        }

        public void Update(TimeSpan timeElapsed)
        {
            foreach (var entity in _entities)
                entity.Update(timeElapsed);
        }

        public void Render()
        {
            foreach (var entity in _entities)
                entity.Render();
        }
    }
}
