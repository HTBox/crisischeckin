(function ($) {$(document).ready(function () {
        $("#dp_resourceStartDate").datepicker({ minDate: 'today', onSelect: updateResourceEndDate }).datepicker('setDate',
            '@(Model.ResourceStartDate == default(DateTime) ? DateTime.Now : Model.ResourceStartDate)');
        $("#dp_resourceEndDate").datepicker({ minDate: 'today' }).datepicker('setDate',
            '@(Model.ResourceEndDate == default(DateTime) ? DateTime.Now : Model.ResourceEndDate)');

    });

    function updateResourceEndDate(dateText) {
        $('#dp_resourceEndDate').datepicker('option', 'minDate', dateText);
    }
})(jQuery);