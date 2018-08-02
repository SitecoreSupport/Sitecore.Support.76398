using Sitecore.ExperienceEditor.Speak.Server.Contexts;
using Sitecore.ExperienceEditor.Speak.Server.Requests;
using Sitecore.ExperienceEditor.Speak.Server.Responses;
using System;

namespace Sitecore.Support.ExperienceEditor.Speak.Ribbon.Requests.SaveItem
{
    public class CheckRevision : PipelineProcessorRequest<PageContext>
    {
        public override PipelineProcessorResponseValue ProcessRequest()
        {
            Sitecore.Pipelines.Save.SaveArgs.SaveItem saveItem = base.RequestContext.GetSaveArgs().Items[0];
            PipelineProcessorResponseValue pipelineProcessorResponseValue = new PipelineProcessorResponseValue();
            //Sitecore.Data.Items.Item item = base.RequestContext.Item.Database.GetItem(saveItem.ID, Sitecore.Globalization.Language.Parse(base.RequestContext.Language), Sitecore.Data.Version.Parse(base.RequestContext.Version));
            Sitecore.Data.Items.Item item = base.RequestContext.Item.Database.GetItem(saveItem.ID, saveItem.Language, saveItem.Version);
            if (item == null)
            {
                return pipelineProcessorResponseValue;
            }
            string text = item[Sitecore.FieldIDs.Revision].Replace("-", string.Empty);
            if (string.IsNullOrEmpty(saveItem.Revision))
            {
                saveItem.Revision = text;
            }
            string strB = saveItem.Revision.Replace("-", string.Empty);
            if (string.Compare(text, strB, StringComparison.OrdinalIgnoreCase) != 0 && string.Compare("#!#Ignore revision#!#", strB, StringComparison.OrdinalIgnoreCase) != 0)
            {
                pipelineProcessorResponseValue.ConfirmMessage = Sitecore.Globalization.Translate.Text("One or more items have been changed.\n\nDo you want to overwrite these changes?");
            }
            return pipelineProcessorResponseValue;
        }
    }
}