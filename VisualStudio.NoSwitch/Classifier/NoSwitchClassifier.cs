using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Tagging;
using VisualStudio.NoSwitch.Tag;
using Microsoft.VisualStudio.Text.Classification;

namespace VisualStudio.NoSwitch.Classifier
{
    internal class NoSwitchClassifier : ITagger<ClassificationTag>
    {
        private ITagAggregator<NoSwitchTag> _aggregator;
        private ITextBuffer _buffer;

        IDictionary<NoSwitchTaggerTypes, IClassificationType> _types;

        public event EventHandler<SnapshotSpanEventArgs> TagsChanged
        {
            add { }
            remove { }
        }

        public NoSwitchClassifier(ITextBuffer buffer,
                             ITagAggregator<NoSwitchTag> aggregator,
                             IClassificationTypeRegistryService typeService)
        {
            _buffer = buffer;
            _aggregator = aggregator;

            _types = new Dictionary<NoSwitchTaggerTypes, IClassificationType>();
            _types[NoSwitchTaggerTypes.Start] = typeService.GetClassificationType("NoSwitchClassifier");
        }

        public IEnumerable<ITagSpan<ClassificationTag>> GetTags(NormalizedSnapshotSpanCollection spans)
        {
            foreach (var tagSpan in _aggregator.GetTags(spans))
            {
                var span = spans[0];
                var tagSpans = tagSpan.Span.GetSpans(span.Snapshot);
                var classTag = new ClassificationTag(_types[tagSpan.Tag.Type]);
                var newTagSpan = new TagSpan<ClassificationTag>(tagSpans[0], classTag);
                yield return newTagSpan;
            }
        }
    }
}
