﻿using JSDelivrCLI.Services;
using System.CommandLine;

namespace delivr.Commands
{
    internal class InitCommand : Command
    {
        private readonly ConfigService configService;

        public InitCommand() : base("init", "Initialize a package configuration file")
        {
            configService = new();
            SetAction(_ => Execute());
        }

        private void Execute()
        {
            configService.Save();
        }
    }
}
