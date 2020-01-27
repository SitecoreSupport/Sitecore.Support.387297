    using Sitecore.Data.Items;
    using Sitecore.Diagnostics;
    using Sitecore.Shell.Framework;
    using Sitecore.Web.UI.Sheer;
    using System;

namespace Sitecore.Shell.Framework.Commands
{
    [Serializable]
    public class Delete : Command
    {
        public override void Execute(CommandContext context)
        {
            if (context.Items.Length == 0)
            {
                SheerResponse.Alert("The selected item could not be found.\n\nIt may have been deleted by another user.\n\nSelect another item.", Array.Empty<string>());
            }
            else
            {
                if (context.Items.Length == 1)
                {
                    object[] args = new object[] { context.Items[0].ID };
                    SheerResponse.Eval("if(this.Content && this.Content.loadNextSearchedItem){{this.Content.loadNextSearchedItem('{0}');}}", args);
                }
                Items.Delete(context.Items);
            }
        }

        public override CommandState QueryState(CommandContext context)
        {
            CommandState disabled;
            Error.AssertObject(context, "context");
            if (context.Items.Length == 0)
            {
                disabled = CommandState.Disabled;
            }
            else
            {
                Item[] items = context.Items;
                int index = 0;
                while (true)
                {
                    if (index >= items.Length)
                    {
                        disabled = base.QueryState(context);
                    }
                    else
                    {
                        Item item = items[index];
                        if (!item.Access.CanDelete())
                        {
                            disabled = CommandState.Disabled;
                        }
                        else if (item.Appearance.ReadOnly)
                        {
                            disabled = CommandState.Disabled;
                        }
                        else
                        {
                            if (!IsLockedByOther(item))
                            {
                                index++;
                                continue;
                            }
                            disabled = CommandState.Disabled;
                        }
                    }
                    break;
                }
            }
            return disabled;
        }
    }
}
