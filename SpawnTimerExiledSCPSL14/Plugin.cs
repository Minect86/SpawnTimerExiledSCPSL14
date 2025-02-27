using Exiled.API.Features;
using Exiled.API.Features.Waves;
using Exiled.Events.EventArgs.Server;
using MEC;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpawnTimerExiledSCPSL14
{
    internal class Plugin : Plugin<Config>
    {
        private CoroutineHandle _TimerCoroutine;

        public override void OnEnabled()
        {
            Exiled.Events.Handlers.Server.RoundStarted += StartedRound;
            Exiled.Events.Handlers.Server.RestartingRound += RestartingRound;
            Exiled.Events.Handlers.Server.RoundEnded += EndedRound;
            base.OnEnabled();
        }
        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Server.RoundStarted -= StartedRound;
            Exiled.Events.Handlers.Server.RestartingRound -= RestartingRound;
            Exiled.Events.Handlers.Server.RoundEnded -= EndedRound;
            base.OnDisabled();
        }

        private void StartedRound()
        {
            _TimerCoroutine = Timing.RunCoroutine(TimerNumerator());
        }
        private void RestartingRound()
        {
            if (Timing.IsRunning(_TimerCoroutine))
                Timing.KillCoroutines(_TimerCoroutine);
        }
        private void EndedRound(RoundEndedEventArgs ev)
        {
            if (Timing.IsRunning(_TimerCoroutine))
                Timing.KillCoroutines(_TimerCoroutine);
        }

        private IEnumerator<float> TimerNumerator()
        {
            while (true)
            {
                yield return Timing.WaitForSeconds(1);
                foreach (Player pl in Player.List)
                {
                    if ((pl.Role.Type == RoleTypeId.Spectator) && !Round.IsLobby)
                        pl.Broadcast(1, $"<size=45%><b>{TimeSpawn()}</b></size>");
                }
            }
        }

        private string TimeSpawn()
        {
            List<WaveTimer> waves = new List<WaveTimer>()
            {
                WaveTimer.GetWaveTimers()[0],
                WaveTimer.GetWaveTimers()[1]
            };

            var minTimer = waves.Aggregate((min, next) => next.TimeLeft.TotalSeconds < min.TimeLeft.TotalSeconds ? next : min);

            if (minTimer.Name == "NtfSpawnWave")
            {
                if (minTimer.TimeLeft < TimeSpan.FromSeconds(1))
                    return $"<color=#6D9FF7>Ntf</color>";
                else
                    return $"<color=#6D9FF7>{minTimer.TimeLeft.Minutes}:{minTimer.TimeLeft.Seconds:00}</color>";
            }
            else if (minTimer.Name == "ChaosSpawnWave")
            {
                if (minTimer.TimeLeft < TimeSpan.FromSeconds(1))
                    return $"<color=#608F38>Chaos</color>";
                else
                    return $"<color=#608F38>{minTimer.TimeLeft.Minutes}:{minTimer.TimeLeft.Seconds:00}</color>";
            }

            return "Error";
        }
    }
}
