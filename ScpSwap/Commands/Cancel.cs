// -----------------------------------------------------------------------
// <copyright file="Cancel.cs" company="Build">
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
    /// Cancels an active swap request.
    /// </summary>
    public class Cancel : ICommand
    {
        /// <inheritdoc />
        public string Command { get; set; } = "cancel";

        /// <inheritdoc />
        public string[] Aliases { get; set; } = { "c" };

        /// <inheritdoc />
        public string Description { get; set; } = "Cancels an active swap request.";

        /// <inheritdoc />
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player playerSender = Player.Get(sender);
            if (playerSender == null)
            {
                response = Plugin.Instance.Config.Translation.ExecutorIsntPlayer;
                return false;
            }

            Swap swap = Swap.FromSender(playerSender);
            if (swap == null)
            {
                response = Plugin.Instance.Config.Translation.NoPendingRequest;
                return false;
            }

            swap.Cancel();
            response = Plugin.Instance.Config.Translation.SwapRequestCancelled;
            return true;
        }
    }
}