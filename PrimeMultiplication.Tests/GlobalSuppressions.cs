// MIT License, Copyright 2020 Samu Lang

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores", Justification = "This is test method naming convention")]
[assembly: SuppressMessage("Usage", "CA2234:Pass system uri objects instead of strings", Justification = "Would clutter tests")]
[assembly: SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "Test methods document themselves with name and content")]
[assembly: SuppressMessage("Reliability", "CA2007:Consider calling ConfigureAwait on the awaited task", Justification = "Honestly I don't know which option I should choose")]
