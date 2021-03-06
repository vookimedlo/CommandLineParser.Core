﻿using System;

namespace MatthiWare.CommandLine.Abstractions.Command
{
    public interface ICommandBuilder<TOption>
    {
        /// <summary>
        /// Configures how the command should be invoked.
        /// Default behavior is to auto invoke the command.
        /// </summary>
        /// <param name="invoke">True if the command executor will be invoked (default), false if you want to invoke manually.</param>
        /// <returns><see cref="ICommandBuilder{TOption}"/></returns>
        ICommandBuilder<TOption> InvokeCommand(bool invoke);

        /// <summary>
        /// Configures if the command is required
        /// </summary>
        /// <param name="required">True or false</param>
        /// <returns><see cref="ICommandBuilder{TOption}"/></returns>
        ICommandBuilder<TOption> Required(bool required = true);

        /// <summary>
        /// Describes the command, used in the usage output. 
        /// </summary>
        /// <param name="desc">description of the command</param>
        /// <returns><see cref="ICommandBuilder{TOption}"/></returns>
        ICommandBuilder<TOption> Description(string description);

        /// <summary>
        /// Configures the command name
        /// </summary>
        /// <param name="name">name</param>
        /// <returns><see cref="ICommandBuilder{TOption}"/></returns>
        ICommandBuilder<TOption> Name(string name);

        /// <summary>
        /// Configures the execution of the command
        /// </summary>
        /// <param name="required">True or false</param>
        /// <returns><see cref="ICommandBuilder{TOption}"/></returns>
        ICommandBuilder<TOption> OnExecuting(Action<TOption> action);
    }
}
