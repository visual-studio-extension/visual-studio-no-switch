using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace VisualStudio.NoSwitch.Classifier
{
    internal static class NoSwitchClassifierClassificationDefinition
    {
        [Export(typeof(ClassificationTypeDefinition))]
        [Name("NoSwitchClassifier")]
        private static ClassificationTypeDefinition typeDefinition;
    }
}
