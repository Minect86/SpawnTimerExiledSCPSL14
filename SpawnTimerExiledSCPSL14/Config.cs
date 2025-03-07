﻿using Exiled.API.Interfaces;

namespace SpawnTimerExiledSCPSL14
{
    internal class Config : IConfig
    {
        public bool IsEnabled {  get; set; } = true;
        public bool Debug { get; set; } = false;
        public string ColorNTF { get; set; } = "#6D9FF7";
        public string ColorChaos { get; set; } = "#608F38";
        public string TextNTF { get; set; } = "<color=#6D9FF7>Ntf</color>";
        public string TextChaos { get; set; } = "<color=#608F38>Chaos</color>";
    }
}
