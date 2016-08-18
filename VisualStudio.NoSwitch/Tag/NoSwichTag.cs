using Microsoft.VisualStudio.Text.Tagging;

namespace VisualStudio.NoSwitch.Tag
{
    class NoSwitchTag : ITag
    {
        public NoSwitchTaggerTypes Type;

        public NoSwitchTag(NoSwitchTaggerTypes type)
        {
            Type = type;
        }
    }
}
