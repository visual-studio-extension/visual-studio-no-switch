﻿//------------------------------------------------------------------------------
// <copyright file="NoSwtichClassifierFormat.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.ComponentModel.Composition;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace VisualStudio.NoSwitch.Classifier
{
    /// <summary>
    /// Defines an editor format for the NoSwtichClassifier type that has a purple background
    /// and is underlined.
    /// </summary>
    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "NoSwitchClassifier")]
    [Name("NoSwitchClassifier")]
    [UserVisible(true)] // This should be visible to the end user
    [Order(Before = Priority.Default)] // Set the priority to be after the default classifiers
    internal sealed class NoSwitchClassifierFormat : ClassificationFormatDefinition
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NoSwitchClassifierFormat"/> class.
        /// </summary>
        public NoSwitchClassifierFormat()
        {
            this.DisplayName = "NoSwitchClassifier"; // Human readable version of the name
            //this.BackgroundColor = Colors.BlueViolet;
            //this.TextDecorations = System.Windows.TextDecorations.Underline;
            this.ForegroundColor = Colors.Pink;
        }
    }
}
