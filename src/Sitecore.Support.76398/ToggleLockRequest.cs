using Sitecore.ExperienceEditor.Speak.Server.Contexts;
using Sitecore.ExperienceEditor.Speak.Server.Requests;
using Sitecore.ExperienceEditor.Speak.Server.Responses;
using Sitecore.Diagnostics;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Security;
using Sitecore.Web;
using System;
using Sitecore.Data.Items;

namespace Sitecore.Support.ExperienceEditor.Speak.Ribbon.Requests.LockItem
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
                    Version = item.Version.Number,
                    Revision = item[FieldIDs.Revision]
                }
            };
        }

        protected Sitecore.Data.Items.Item SwitchLock(Sitecore.Data.Items.Item item)
        {
            Assert.ArgumentNotNull(item, "item");
            if (base.RequestContext.IsLocked) // Patch: condition added from 901
            {
                if (item.Locking.IsLocked())
                {
                    item.Locking.Unlock();
                    return item;
                }
            }
            if (Sitecore.Context.User.IsAdministrator)
            {
                item.Locking.Lock();
                return item;
            }
            return Sitecore.Context.Workflow.StartEditing(item);
        }

        // Patch: copied from 901
        [Obsolete("This method is obsolete and will be deleted in the next product version.")]
        protected virtual void RefreshIndex(Item item)
        {
            using (IProviderSearchContext providerSearchContext = ContentSearchManager.GetIndex("sitecore_master_index").CreateSearchContext(SearchSecurityOptions.Default))
            {
                providerSearchContext.Index.Refresh(new SitecoreIndexableItem(item), IndexingOptions.ForcedIndexing);
            }
        }

        // Patch: copied from 901
        private void HandleVersionCreating(Item finalItem)
        {
            if (base.RequestContext.Item.Version.Number != finalItem.Version.Number)
            {
                WebUtil.SetCookieValue(base.RequestContext.Site.GetCookieKey("sc_date"), string.Empty, DateTime.MinValue);
            }
        }
    }
}