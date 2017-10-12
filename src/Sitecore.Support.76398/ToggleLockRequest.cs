using Sitecore.Data.Items;
using Sitecore.ExperienceEditor.Speak.Server.Contexts;
using Sitecore.ExperienceEditor.Speak.Server.Requests;
using Sitecore.ExperienceEditor.Speak.Server.Responses;
using Sitecore.Web;
using System;

namespace Sitecore.Support.ExperienceEditor.Speak.Ribbon.Requests.LockItem
{
    public class ToggleLockRequest : PipelineProcessorRequest<ItemContext>
    {
        public override PipelineProcessorResponseValue ProcessRequest()
        {
            base.RequestContext.ValidateContextItem();
            Item item = this.SwitchLock(base.RequestContext.Item);
            this.HandleVersionCreating(item);
            return new PipelineProcessorResponseValue
            {
                Value = new
                {
                    Locked = item.Locking.IsLocked(),
                    Version = item.Version.Number,
                    Revision = item[FieldIDs.Revision]
                }
            };
        }

        protected Item SwitchLock(Item item)
        {
            if (item.Locking.IsLocked())
            {
                item.Locking.Unlock();
                return item;
            }
            if (Sitecore.Context.User.IsAdministrator)
            {
                item.Locking.Lock();
                return item;
            }
            return Sitecore.Context.Workflow.StartEditing(item);
        }

        private void HandleVersionCreating(Item finalItem)
        {
            if (base.RequestContext.Item.Version.Number != finalItem.Version.Number)
            {
                WebUtil.SetCookieValue(base.RequestContext.Site.GetCookieKey("sc_date"), string.Empty, DateTime.MinValue);
            }
        }
    }
}