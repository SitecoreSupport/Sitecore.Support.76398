using Sitecore.Data.Items;
using Sitecore.ExperienceEditor.Speak.Server.Contexts;
using Sitecore.ExperienceEditor.Speak.Server.Requests;
using Sitecore.ExperienceEditor.Speak.Server.Responses;
using System;

namespace Sitecore.ExperienceEditor.Speak.Ribbon.Requests.LockItem
{
    public class ToggleLockRequest : PipelineProcessorRequest<ItemContext>
    {
        public override PipelineProcessorResponseValue ProcessRequest()
        {
            base.RequestContext.ValidateContextItem();
            Sitecore.Data.Items.Item item = this.SwitchLock(base.RequestContext.Item);
            return new PipelineProcessorResponseValue
            {
                Value = new
                {
                    Locked = item.Locking.IsLocked(),
                    Version = item.Version.Number
                }
            };
        }

        protected Sitecore.Data.Items.Item SwitchLock(Sitecore.Data.Items.Item item)
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
    }
}