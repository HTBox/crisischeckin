(function ($) {
    $(document).ready(function () {
        $("#dp_startDate").datepicker({ minDate: 'today', onSelect: updateEndDate }).datepicker('setDate',
            '@(Model.SelectedStartDate == default(DateTime) ? DateTime.Now : Model.SelectedStartDate)');
        $("#dp_endDate").datepicker({ minDate: 'today' }).datepicker('setDate',
            '@(Model.SelectedEndDate == default(DateTime) ? DateTime.Now : Model.SelectedEndDate)');

        $('.contact').tooltip();
        
    });

    function updateEndDate(dateText) {
        $('#dp_endDate').datepicker('option', 'minDate', dateText);
    }

    $(document).ready(function () {
        $("#dp_resStartDate").datepicker({ minDate: 'today', onSelect: updateResEndDate }).datepicker('setDate',
            '@(Model.ResourceStartDate == default(DateTime) ? DateTime.Now : Model.ResourceStartDate)');
        $("#dp_resEndDate").datepicker({ minDate: 'today' }).datepicker('setDate',
            '@(Model.ResourceEndDate == default(DateTime) ? DateTime.Now : Model.ResourceEndDate)');

    });

    function updateResEndDate(dateText) {
        $('#dp_resEndDate').datepicker('option', 'minDate', dateText);
    }

    $(function () {
        $('select#disasterList').change(function () {
            var disasterId = $('#disasterList').val();
            $.ajax({
                url: '/Home/LoadDisasterClusterList',
                type: "GET",
                dataType: "JSON",
                data: { disasterId: disasterId },
                success: function (disasterClusters) {
                    $("#activityList").html(""); // clear before appending new list 
                    $.each(disasterClusters, function (i, disasterCluster) {
                        $("#activityList").append(
                            $('<option></option>').val(disasterCluster.Cluster.Id).html(disasterCluster.Cluster.Name));
                    });
                }
            });
        });

        $('#delayed-button').on('click', function (evt) {
            evt.preventDefault();
            $('#delayed-modal :text').datepicker();
            $('#delayed-modal').modal({
                backdrop: 'static'
            });

            $('#new-dates-button').on('click', function () {
                $('<input />').attr('type', 'hidden')
                  .attr('value', $('#new-start-date').val())
                  .attr('name', 'startDate')
                  .appendTo('#delayed-form');
                $('<input />').attr('type', 'hidden')
                  .attr('value', $('#new-end-date').val())
                  .attr('name', 'endDate')
                  .appendTo('#delayed-form');
                $('#delayed-form').submit();
            });
        });
        
    });

})(jQuery);