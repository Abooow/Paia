﻿namespace Paia.Components
{
    public interface IComponent
    {
        ViewManager ViewManager { get; set; }

        void Render();
    }
}
