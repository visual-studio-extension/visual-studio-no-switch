using Microsoft.VisualStudio.Text.Tagging;
using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Operations;
using System.Linq;
using VisualStudio.NoSwitch.Mapper;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell;
using EnvDTE80;
using EnvDTE;
using Microsoft.VisualStudio.TextManager.Interop;

namespace VisualStudio.NoSwitch.Tag
{
    public class CakeHelper
    {
        static Action<string> Output = OutputWindow();

        static CakeHelper()
        {
            var outWindow = Package.GetGlobalService(typeof(SVsOutputWindow)) as IVsOutputWindow;
            var customGuid = new Guid("1E7A8380-9E06-4A12-A0C7-DBC5EFBB5F23");

            IVsOutputWindowPane outputPane;
            outWindow.CreatePane(ref customGuid, "NoSwitch", 1, 1);
            outWindow.GetPane(ref customGuid, out outputPane);
            outputPane.Activate();

            Output = (message) => outputPane.OutputString(message + Environment.NewLine);
        }

        public static Action<string> OutputWindow()
        {
            return Output;
        }
    }

    class State
    {
        public bool ActivateThai { set; get; }
        public int StartPosition { set; get; }
        public void Reset()
        {
            ActivateThai = false;
            StartPosition = 0;
        }
    }

    class NoSwitchTagger : ITagger<NoSwitchTag>
    {
        private State _state = new State();
        private Action<string> _output = CakeHelper.OutputWindow();

        private DTE2 _dte = Package.GetGlobalService(typeof(DTE)) as DTE2;
        private IVsTextManager _textManager = Package.GetGlobalService(typeof(SVsTextManager)) as IVsTextManager;

        public event EventHandler<SnapshotSpanEventArgs> TagsChanged;

        private ITextSearchService2 _searchService;

        private ITextBuffer _buffer;

        public NoSwitchTagger(ITextBuffer buffer, ITextSearchService2 search)
        {
            _buffer = buffer;
            _searchService = search;
        }

        //private void Release(SnapshotSpan span)
        private void Release()
        {
            _state.ActivateThai = false;
            _state.StartPosition = 0;
        }

        private void Start(int location)
        {
            _state.ActivateThai = true;
            _state.StartPosition = location;
        }


        private void Replace(SnapshotSpan span, string replaceWith)
        {
            var edit = _buffer.CreateEdit();
            edit.Replace(span, replaceWith);
            edit.Apply();
        }

        public int GetCursorPosition()
        {
            IVsTextView textViewCurrent;
            _textManager.GetActiveView(1, null, out textViewCurrent);
            var row = 0;
            var column = 0;

            if (textViewCurrent == null) return 0;
            else
            {
                textViewCurrent.GetCaretPos(out row, out column);
                return column;
            }
        }

        public IEnumerable<ITagSpan<NoSwitchTag>> GetTags(NormalizedSnapshotSpanCollection spans)
        {
            return TryGetTags(spans);
        }

        public IEnumerable<ITagSpan<NoSwitchTag>> TryGetTags(NormalizedSnapshotSpanCollection spans)
        {
            foreach (SnapshotSpan currentSpan in spans)
            {
                var containingLine = currentSpan.Start.GetContainingLine();
                var location = containingLine.Start.Position;
                var line = containingLine.GetText();
                var cursor = GetCursorPosition() + location;

                var start = line.IndexOf('~');
                if (start == -1)
                {
                    _state.Reset();
                }
                else
                {
                    _state.StartPosition = location + start;
                }

                foreach (var token in line.ToArray())
                {
                    var activateThai = _state.ActivateThai;
                    var startPosition = _state.StartPosition;

                    if (token == '~')
                    {
                        if (activateThai)
                        {
                            if (location > startPosition)
                            {
                                _output($"|| release => startPosition = {startPosition} location = {location}");
                                Release();
                            }
                        }
                        else
                        {
                            _output($"|| start => startPosition = {startPosition} location = {location}");
                            Start(location);
                        }
                    }
                    else
                    {
                        if (activateThai && location > startPosition && location < cursor)
                        {
                            var thai = KeyboardMap.GetThaiChar(token);

                            if (thai != token)
                            {
                                _output($"|| replace => startPosition = {startPosition}, location = {location}, replace {token} => {thai}");
                                var tokenSpan = new SnapshotSpan(currentSpan.Snapshot, new Span(location, 1));
                                if (tokenSpan.IntersectsWith(currentSpan))
                                {
                                    Replace(tokenSpan, thai.ToString());
                                }
                            }

                            _output($"|| more => startPosition = {startPosition} location = {location}");

                            var startSpan = new SnapshotSpan(currentSpan.Snapshot, new Span(startPosition, location - startPosition));
                            if (startSpan.IntersectsWith(currentSpan))
                            {
                                var tag = new NoSwitchTag(NoSwitchTaggerTypes.Start);
                                var newSpan = new TagSpan<NoSwitchTag>(startSpan, tag);
                                yield return newSpan;
                            }

                        }
                    }
                    location++;
                }
            }
        }
    }
}
