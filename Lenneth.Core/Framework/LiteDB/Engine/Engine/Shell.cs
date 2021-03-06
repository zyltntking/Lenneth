﻿using System;
using System.Collections.Generic;
using System.Linq;
using Lenneth.Core.Framework.LiteDB.Shell;

namespace Lenneth.Core.Framework.LiteDB
{
    public partial class LiteEngine
    {
        private static List<ICommand> _commands = new List<ICommand>();

        /// <summary>
        /// Run a shell command from a string. Execute command in current database and returns an IEnumerable collection of results
        /// </summary>
        public IList<BsonValue> Run(string command)
        {
            if (_commands.Count == 0)
            {
                RegisterCommands();
            }

            var s = new StringScanner(command);

            // test all commands
            foreach (var cmd in _commands)
            {
                if (!cmd.IsCommand(s)) continue;

                var values = cmd.Execute(s, this);

                return values.ToList();
            }

            throw LiteException.InvalidCommand(command);
        }

        #region Register all shell commands

        private static void RegisterCommands()
        {
            lock (_commands)
            {
                var type = typeof(ICommand);
                var types = TypeInfoExtensions.GetTypeInfo(typeof(LiteEngine)).Assembly
                    .GetTypes()
                    .Where(p => type.IsAssignableFrom(p) && TypeInfoExtensions.GetTypeInfo(p).IsClass);

                _commands.Clear();

                foreach (var cmd in types)
                {
                    _commands.Add(Activator.CreateInstance(cmd) as ICommand);
                }
            }
        }

        #endregion
    }
}