using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Utilities;
using Microsoft.VisualStudio.Text.Tagging;
using System;
using VisualStudio.NoSwitch.Tag;
using Microsoft.VisualStudio.Text.Classification;

namespace VisualStudio.NoSwitch.Classifier
{
    [Export(typeof(ITaggerProvider))]
    [ContentType("text")] 
    [TagType(typeof(ClassificationTag))]
    internal class NoSwitchClassifierProvider : ITaggerProvider
    {
        [Import]
        internal IBufferTagAggregatorFactoryService AggregatorFactory = null;

        [Import]
        internal IClassificationTypeRegistryService ClassificationTypeRegistry = null;

        public ITagger<T> CreateTagger<T>(ITextBuffer buffer) where T : ITag
        {
            var aggregator = AggregatorFactory.CreateTagAggregator<NoSwitchTag>(buffer);

            return new NoSwitchClassifier(buffer, aggregator, ClassificationTypeRegistry) as ITagger<T>;

        }
    }
}
