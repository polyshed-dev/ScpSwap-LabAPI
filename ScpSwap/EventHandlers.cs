// -----------------------------------------------------------------------
// <copyright file="EventHandlers.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Features.Wrappers;

namespace ScpSwap
{
    using MEC;
    using PlayerRoles;
    using ScpSwap.Models;

    /// <summary>
    /// Handles events derived from <see cref="Exiled.Events.Handlers"/>.
    /// </summary>
    public class EventHandlers
    {

        

        /// <inheritdoc cref="Exiled.Events.Handlers.Server.OnRoundStarted"/>
        public void OnSpawned(PlayerSpawnedEventArgs ev)
        {
            if ((ev.Player.IsSCP || ValidSwaps.GetCustom(ev.Player) != null) && ev.Player.ReferenceHub.roleManager.PreviouslySentRole[ev.Player.NetworkId].GetTeam() != Team.SCPs && Round.Duration.TotalSeconds < Plugin.Instance.Config.SwapTimeout) //If anything breaks, it's probably going to be this
                ev.Player.SendBroadcast(Plugin.Instance.Config.Translation.StartMessage,15);
        }

        /*/// <inheritdoc cref="Exiled.Events.Handlers.Server.OnReloadedConfigs"/>
        public void OnReloadedConfigs()
        {
            ValidSwaps.Refresh();
        }*/

        /// <inheritdoc cref="Exiled.Events.Handlers.Server.OnRestartingRound"/>
        public void OnRestartingRound()
        {
            Swap.Clear();
        }

        /// <inheritdoc cref="Exiled.Events.Handlers.Server.OnWaitingForPlayers"/>
        public void OnWaitingForPlayers()
        {
            ValidSwaps.Refresh();
        }

        /*public void OnPlayerLeave(LeftEventArgs ev)
        {
            VolunteerSwap.ScpLeft(Player scpPlayer);
        }*/

        public void RegisterEvents()
        {
            LabApi.Events.Handlers.PlayerEvents.Spawned += OnSpawned;
            LabApi.Events.Handlers.ServerEvents.RoundRestarted += OnRestartingRound;
            LabApi.Events.Handlers.ServerEvents.WaitingForPlayers += OnWaitingForPlayers;
        }
        
        public void UnregisterEvents()
        {
            LabApi.Events.Handlers.PlayerEvents.Spawned -= OnSpawned;
            LabApi.Events.Handlers.ServerEvents.RoundRestarted -= OnRestartingRound;
            LabApi.Events.Handlers.ServerEvents.WaitingForPlayers -= OnWaitingForPlayers;
        }
    }
}