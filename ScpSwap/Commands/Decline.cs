// -----------------------------------------------------------------------
// <copyright file="Decline.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

using LabApi.Features.Wrappers;

namespace ScpSwap.Commands
{
    using System;
    using CommandSystem;
    using ScpSwap.Models;

    /// <summary>
    /// Rejects an active swap request.
    /// </summary>
    public class Decline : ICommand
    {
        /// <inheritdoc />
        public string Command { get; set; } = "hayır";

        /// <inheritdoc />
        public string[] Aliases { get; set; } = { "reddet", "n", "no", "decline" };

        /// <inheritdoc />
        public string Description { get; set; } = "Gelen değişim isteğini reddeder.";

        /// <inheritdoc />
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player playerSender = Player.Get(sender);
            if (playerSender == null)
            {
                response = Plugin.Instance.Config.Translation.ExecutorIsntPlayer;
                return false;
            }

            Swap swap = Swap.FromReceiver(playerSender);
            if (swap == null)
            {
                response = Plugin.Instance.Config.Translation.NoPendingRequest;
                return false;
            }

            swap.Decline();
            response = Plugin.Instance.Config.Translation.SwapRequestCancelled;
            return true;
        }
    }
}