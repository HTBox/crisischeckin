jQuery(document).ready(function () {
    var allChecked = true;
    jQuery("input[type=checkbox][id^=SelectedDisasterClusters]").each(function () {
        allChecked = allChecked && this.checked;
    });
    jQuery("#SelectedDisasterClustersAll").prop("indeterminate", !allChecked);
    jQuery("#SelectedDisasterClustersAll").prop("checked", allChecked);
    jQuery("#SelectedDisasterClustersAll").click(function () {
        if (this.checked) {
            jQuery("input[type=checkbox][id^=SelectedDisasterClusters]").prop("checked", true);
        }
        else {
            jQuery("input[type=checkbox][id^=SelectedDisasterClusters]").prop("checked", false);
        }
    });
});