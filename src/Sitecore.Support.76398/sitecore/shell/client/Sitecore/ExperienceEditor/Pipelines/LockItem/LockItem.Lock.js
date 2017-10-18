define(["sitecore"], function (Sitecore) {
    return Sitecore.ExperienceEditor.PipelinesUtil.generateRequestProcessor("ExperienceEditor.LockItem", function (response) {
        if (response.context.currentContext.version != response.responseValue.value.Version) {
            response.context.app.refreshOnItem(response.context.currentContext);
        }

        var locked = response.responseValue.value.Locked;

        Sitecore.Commands.Lock.setButtonTitle(Sitecore.ExperienceEditor.instance, locked);
        Sitecore.ExperienceEditor.instance.currentContext.isLocked = locked;
        if (locked) {
            Sitecore.ExperienceEditor.instance.currentContext.isLockedByCurrentUser = true;
        }
    });
});