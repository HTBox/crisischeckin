jQuery(document).ready(function () {
    var allChecked = true;
    jQuery("input[type=checkbox][id^=SelectedDisasterClusters]").each(function () {
        allChecked = allChecked && this.checked;
    });
    jQuery("#SelectedDisasterClustersAll").prop("indeterminate", !allChecked);
    if (!allChecked) {
        jQuery("#SelectedDisasterClustersAll").addClass("wasIndeterminate");
    }
    jQuery("#SelectedDisasterClustersAll").click(function () {
        // The first click of the Select All checkbox that started in the indeterminate state
        // should make it checked, not unchecked (which is the default).
        if (jQuery(this).hasClass("wasIndeterminate")) {
            jQuery(this).prop("checked", true);
            jQuery(this).removeClass("wasIndeterminate");
        }
        if (this.checked) {
            jQuery("input[type=checkbox][id^=SelectedDisasterClusters]").prop("checked", true);
        }
        else {
            jQuery("input[type=checkbox][id^=SelectedDisasterClusters]").prop("checked", false);
        }
    });
});