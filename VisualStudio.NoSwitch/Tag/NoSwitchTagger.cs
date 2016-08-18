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
            foreach (SnapshotSpan curSpan in spans)
            {
                ITextSnapshotLine containingLine = curSpan.Start.GetContainingLine();
                int curLoc = containingLine.Start.Position;
                string[] tokens = containingLine.GetText().ToLower().Split(' ');

                foreach (string ookToken in tokens)
                {
                        var tokenSpan = new SnapshotSpan(curSpan.Snapshot, new Span(curLoc, ookToken.Length));
                    if (tokenSpan.IntersectsWith(curSpan))
                    {
                        var x = new NoSwitchTag(NoSwitchTaggerTypes.Start);
                        var y = new TagSpan<NoSwitchTag>(tokenSpan, x );
                        yield return y;
                    }
                    curLoc += ookToken.Length + 1;
                }
            }
        }
    }
}
