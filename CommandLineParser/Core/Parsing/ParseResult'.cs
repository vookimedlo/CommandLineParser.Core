﻿using System;
using System.Collections.Generic;
using System.Linq;
using MatthiWare.CommandLine.Abstractions;
using MatthiWare.CommandLine.Abstractions.Parsing;
using MatthiWare.CommandLine.Abstractions.Parsing.Command;

namespace MatthiWare.CommandLine.Core.Parsing
{
    internal class ParseResult<TResult> : IParserResult<TResult>
    {
        private readonly List<ICommandParserResult> commandParserResults = new List<ICommandParserResult>();
        private readonly List<Exception> exceptions = new List<Exception>();

        #region Properties

        public TResult Result { get; private set; }

        public bool HasErrors { get; private set; } = false;

        public IReadOnlyList<ICommandParserResult> CommandResults => commandParserResults.AsReadOnly();

        public bool HelpRequested => HelpRequestedFor != null;

        public IArgument HelpRequestedFor { get; set; } = null;

        public IReadOnlyCollection<Exception> Errors => exceptions;

        #endregion

        public void MergeResult(ICommandParserResult result)
        {
            HasErrors |= result.HasErrors;

            if (result.HelpRequested)
                HelpRequestedFor = result.HelpRequestedFor;

            commandParserResults.Add(result);
        }

        public void MergeResult(ICollection<Exception> errors)
        {
            if (!errors.Any()) return;

            HasErrors = true;

            exceptions.AddRange(errors);
        }

        public void MergeResult(TResult result)
        {
            this.Result = result;
        }

        public void ExecuteCommands()
        {
            if (HasErrors) throw new InvalidOperationException("Parsing failed commands might be corrupted.");

            foreach (var cmdResult in CommandResults)
                cmdResult.ExecuteCommand();
        }
    }
}
