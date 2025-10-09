// -----------------------------------------------------------------------
// <copyright file="Plugin.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

using LabApi.Features;
using LabApi.Loader.Features.Plugins;
using LabApi.Loader.Features.Plugins.Enums;

namespace ScpSwap
{
    using System;

    /// <summary>
    /// The main plugin class.
    /// </summary>
    public class Plugin : Plugin<Config>
    {
        private EventHandlers eventHandlers;

        /// <summary>
        /// Gets the only existing instance of the <see cref="Plugin"/> class.
        /// </summary>
        public static Plugin Instance { get; private set; }

        public override string Description { get; } = "A plugin for SCP:SL that allows Scps to swap roles with each other";

        /// <inheritdoc />
        public override string Author => "TayTay";

        public override void Enable()
        {
            Instance = this;

            eventHandlers = new EventHandlers();
            eventHandlers.RegisterEvents();
            //PlayerHandlers.Left += eventHandlers.OnPlayerLeave;
        }

        public override void Disable()
        {
            //PlayerHandlers.Left -= eventHandlers.OnPlayerLeave;
            if(eventHandlers != null)
                eventHandlers.UnregisterEvents();
            eventHandlers = null;

            Instance = null;

        }

        /// <inheritdoc />
        public override string Name => "ScpSwap.LabAPI";

        /// <inheritdoc />
        public override Version Version { get; } = new Version(1, 3, 0);

        public override LoadPriority Priority { get; } = LoadPriority.Low;

        public override Version RequiredApiVersion { get; } = new Version(LabApiProperties.CompiledVersion);
        
    }
}