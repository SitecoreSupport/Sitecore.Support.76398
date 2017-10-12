using Sitecore.Data.Items;
using Sitecore.ExperienceEditor.Speak.Server.Contexts;
using Sitecore.ExperienceEditor.Speak.Server.Requests;
using Sitecore.ExperienceEditor.Speak.Server.Responses;
using Sitecore.Globalization;
using Sitecore.Pipelines.Save;
using System;

namespace Sitecore.Support.ExperienceEditor.Speak.Ribbon.Requests.SaveItem
{
    public class CheckRevision : PipelineProcessorRequest<PageContext>
    {
        public override PipelineProcessorResponseValue ProcessRequest()
        {
            SaveArgs.SaveItem saveItem = base.RequestContext.GetSaveArgs().Items[0];
            PipelineProcessorResponseValue pipelineProcessorResponseValue = new PipelineProcessorResponseValue();
            //Item item = base.RequestContext.Item.Database.GetItem(saveItem.ID, Language.Parse(base.RequestContext.Language), Sitecore.Data.Version.Parse(base.RequestContext.Version));
            Item item = base.RequestContext.Item.Database.GetItem(saveItem.ID, saveItem.Language, saveItem.Version);
            if (item == null)
            {
                return pipelineProcessorResponseValue;
            }
            string text = item[FieldIDs.Revision].Replace("-", string.Empty);
            if (saveItem.Revision == string.Empty)
            {
                saveItem.Revision = text;
            }
            string strB = saveItem.Revision.Replace("-", string.Empty);
            if (string.Compare(text, strB, StringComparison.InvariantCultureIgnoreCase) != 0 && string.Compare("#!#Ignore revision#!#", strB, StringComparison.InvariantCultureIgnoreCase) != 0)
            {
                pipelineProcessorResponseValue.ConfirmMessage = Translate.Text("One or more items have been changed.\n\nDo you want to overwrite these changes?");
            }
            return pipelineProcessorResponseValue;
        }
    }
}