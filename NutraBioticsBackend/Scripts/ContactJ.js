$(document).ready(function () {
    $("#perid").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "PerConIDAutoComplete",
                type: "POST",
                dataType: "json",
                data: { Prefix: parseInt(request.term), __RequestVerificationToken: $('input[name=__RequestVerificationToken]').val() },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.PerConID, value: item.PerConID };
                    }))
                }
            })
        },
        messages: {
            noResults: "", results: ""
        }
    });
})


//$(document).ready(function () {
//    $("#perid").focusout(function () {
//        $.ajax({
//            url: "SelectPerConIDFromTextBox",
//            dataType: 'json',
//            type: "POST",
//            data: {
//                PerConId: $("#perid").val(), __RequestVerificationToken: $('input[name=__RequestVerificationToken]').val()
//            },
//            success: function (perconid) {
//                if (perconid.PerConID == 999) {
//                    alert("No Encontrado");
//                }
//                else {
//                    $("#PerConID").val(perconid.PerConID);
//                }
//            },
//        });
//        return false;
//    })
//})


$(document).ready(function () {
    $("#PerConID").change(function () {
        $("#perid").empty();
        $.ajax({
            type: 'POST',
            url: "GetPersonContactData",
            dataType: 'json',
            data: { PerConID: $("#PerConID").val(), __RequestVerificationToken: $('input[name=__RequestVerificationToken]').val()},
            success: function (perconid) {
                $.each(perconid, function (i, percon) {
                    $("#perid").prop("value", percon.PerConID);
                });
            },
        });

        return false;
    })
})