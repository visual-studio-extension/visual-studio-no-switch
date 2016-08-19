using Microsoft.VisualStudio.Text.Tagging;
using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;

namespace VisualStudio.NoSwitch.Tag
{
    class NoSwitchTagger : ITagger<NoSwitchTag>
    {
        public event EventHandler<SnapshotSpanEventArgs> TagsChanged;

        private ITextBuffer _buffer;

        public NoSwitchTagger(ITextBuffer buffer)
        {
            _buffer = buffer;
        }

        public IEnumerable<ITagSpan<NoSwitchTag>> GetTags(NormalizedSnapshotSpanCollection spans)
        {
            foreach (SnapshotSpan currentSpan in spans)
            {
                var containingLine = currentSpan.Start.GetContainingLine();
                var location = containingLine.Start.Position;
                var line = containingLine.GetText();
                var tokens = line.Split(' ');

                foreach (string token in tokens)
                {
                    if (token == "%%")
                    {
                        var index = line.IndexOf(token);
                        var tokenSpan = new SnapshotSpan(currentSpan.Snapshot, new Span(location, token.Length));
                        if (tokenSpan.IntersectsWith(currentSpan))
                        {
                            var tag = new NoSwitchTag(NoSwitchTaggerTypes.Start);
                            var newSpan = new TagSpan<NoSwitchTag>(tokenSpan, tag);
                            yield return newSpan;
                        }
                    }

                    location += token.Length + 1;
                }
            }
        }
    }
}
