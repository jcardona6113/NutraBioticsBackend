$(document).ready(function () {
    $(function () {
        $('#datepicker').datetimepicker({ format: 'DD/MM/YYYY HH:mm:ss' });
        $('#datepicker').prop("SetDate", Date.now);
    });



    $(function () {
        $('#datepicker2').datetimepicker({ format: 'DD/MM/YYYY HH:mm:ss' });
        $('#datepicker2').prop("setDate", Date.now);
    });

})