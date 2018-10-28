﻿using System;
using System.Linq.Expressions;
using System.Reflection;
using MatthiWare.CommandLine.Abstractions;
using MatthiWare.CommandLine.Abstractions.Models;
using MatthiWare.CommandLine.Abstractions.Parsing;

namespace MatthiWare.CommandLine.Core
{
    internal class CommandLineOption<TSource, TProperty> :
        CommandLineOptionBase,
        ICommandLineOption<TProperty>,
        IOptionBuilder<TProperty> where TSource : class
    {
        private readonly TSource source;
        private readonly Expression<Func<TSource, TProperty>> selector;
        private TProperty m_defaultValue = default(TProperty);
        private readonly ICommandLineArgumentResolver<TProperty> resolver;

        public CommandLineOption(TSource source, Expression<Func<TSource, TProperty>> selector, ICommandLineArgumentResolver<TProperty> resolver)
        {
            this.source = source ?? throw new ArgumentNullException(nameof(source));
            this.selector = selector ?? throw new ArgumentNullException(nameof(selector));
            this.resolver = resolver ?? throw new ArgumentNullException(nameof(resolver));
        }

        public TProperty DefaultValue
        {
            get => m_defaultValue;
            set
            {
                HasDefault = true;
                m_defaultValue = value;
            }
        }

        public override void UseDefault()
            => AssignValue(DefaultValue);

        public override bool CanParse(ArgumentModel model)
            => resolver.CanResolve(model);

        public override void Parse(ArgumentModel model)
            => AssignValue(resolver.Resolve(model));

        IOptionBuilder<TProperty> IOptionBuilder<TProperty>.Default(TProperty defaultValue)
        {
            DefaultValue = defaultValue;

            return this;
        }

        IOptionBuilder<TProperty> IOptionBuilder<TProperty>.HelpText(string help)
        {
            HelpText = help;

            return this;
        }

        IOptionBuilder<TProperty> IOptionBuilder<TProperty>.LongName(string longName)
        {
            LongName = longName;

            return this;
        }

        IOptionBuilder<TProperty> IOptionBuilder<TProperty>.Required(bool required = true)
        {
            IsRequired = required;

            return this;
        }

        IOptionBuilder<TProperty> IOptionBuilder<TProperty>.ShortName(string shortName)
        {
            ShortName = shortName;

            return this;
        }

        private void AssignValue(TProperty value)
        {
            var property = (PropertyInfo)((MemberExpression)selector.Body).Member;
            property.SetValue(source, value, null);
        }
    }
}