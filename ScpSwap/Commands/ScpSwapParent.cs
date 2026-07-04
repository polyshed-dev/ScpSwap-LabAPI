// -----------------------------------------------------------------------
// <copyright file="ScpSwapParent.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

using System.Threading.Tasks;
using LabApi.Features.Permissions;
using LabApi.Features.Wrappers;

namespace ScpSwap.Commands
{
    using System;
    using System.Linq;
    using CommandSystem;

    using PlayerRoles;
    using ScpSwap.Configs;
    using ScpSwap.Models;

    /// <summary>
    /// The base command for ScpSwapParent.
    /// </summary>
    [CommandHandler(typeof(ClientCommandHandler))]
    public class ScpSwapParent : ParentCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScpSwapParent"/> class.
        /// </summary>
        public ScpSwapParent() => LoadGeneratedCommands();

        /// <inheritdoc />
        public override string Command => "scpswap";

        /// <inheritdoc />
        public override string[] Aliases { get; } = { "swap", "ss", "değiştir" };

        /// <inheritdoc />
        public override string Description => "SCPSwap için ana komut.";
        
        private static readonly RoleTypeId[] ExclusivePair = { RoleTypeId.Scp079, RoleTypeId.Scp096 };

        /// <inheritdoc />
        public sealed override void LoadGeneratedCommands()
        {

            RegisterCommand(new Accept());
            RegisterCommand(new Cancel());
            RegisterCommand(new Decline());
            RegisterCommand(new List());
        }

        private static bool BlocksExclusivity(RoleTypeId target)
        {
            if (!ExclusivePair.Contains(target))
                return false;
            RoleTypeId other = ExclusivePair.First(r => r != target);
            return Player.List.Any(p => p.Role == other);
        } 

        /// <inheritdoc />
        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player playerSender = Player.Get(sender);
            if (playerSender == null)
            {
                response = Plugin.Instance.Config.Translation.ExecutorIsntPlayer;
                return false;
            }

            if (!Round.IsRoundStarted)
            {
                response = Plugin.Instance.Config.Translation.RoundIsntStarted;
                return false;
            }

            if (Round.Duration.TotalSeconds > Plugin.Instance.Config.SwapTimeout)
            {
                response = Plugin.Instance.Config.Translation.SwapPeriodEnded;
                return false;
            }

            if (arguments.IsEmpty())
            {
                response = $"Kullanım: .{Command} SCPnumarası (örn.: .{Command} 49)";
                return false;
            }

            if (Plugin.Instance.Config.AllowUserSwapByPermission)
            {
                if (!playerSender.HasPermissions("scpswap.allowed"))
                {
                    response = Plugin.Instance.Config.Translation.AllowUserSwapByPermission;
                    return false;
                }
            }

            if (!playerSender.IsSCP && ValidSwaps.GetCustom(playerSender) == null)
            {
                response = Plugin.Instance.Config.Translation.NotAnScp;
                return false;
            }

            if (Swap.FromSender(playerSender) != null)
            {
                response = Plugin.Instance.Config.Translation.AlreadyHasPendingRequest;
                return false;
            }

            Player receiver = GetReceiver(arguments.At(0), out Action<Player> spawnMethod);
            if (playerSender == receiver)
            {
                response = Plugin.Instance.Config.Translation.CannotSwapWithYourself;
                return false;
            }

            if (Plugin.Instance.Config.BlacklistedSwapFromScps.Contains(playerSender.Role))
            {
                response = Plugin.Instance.Config.Translation.CannotSwapOffThisScp;
                return false;
            }

            if (receiver != null)
            {
                Swap.Send(playerSender, receiver);
                response = Plugin.Instance.Config.Translation.RequestSent;
                return true;
            }

            if (spawnMethod == null)
            {
                response = Plugin.Instance.Config.Translation.CannotFindRole;
                return false;
            }

            if (Plugin.Instance.Config.AllowNewScps)
            {
                if (BlocksExclusivity(resolvedRole))
                {
                    
                }
                
                spawnMethod(playerSender);
                response = Plugin.Instance.Config.Translation.SuccessfulSwap;
                return true;
            }

            response = Plugin.Instance.Config.Translation.CannotFindPlayerWithRole;
            return false;
        }

        private static Player GetReceiver(string request, out Action<Player> spawnMethod, out RoleTypeId resolvedRole)
        {
            CustomSwap customSwap = ValidSwaps.GetCustom(request);
            if (customSwap != null)
            {
                spawnMethod = customSwap.SpawnMethod;
                resolvedRole = RoleTypeId.None;
                return Player.List.FirstOrDefault(player => customSwap.VerificationMethod(player));
            }

            RoleTypeId roleSwap = ValidSwaps.Get(request);
            if (roleSwap != RoleTypeId.None)
            {
                spawnMethod = player => player.SetRole(roleSwap);
                resolvedRole = roleSwap;
                return Player.List.FirstOrDefault(player => player.Role == roleSwap);
            }

            spawnMethod = null;
            resolvedRole = roleSwap;
            return null;
        }
    }
}