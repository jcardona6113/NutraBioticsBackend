$(document).ready(function () {
    $("#cedulaSH").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "ShipToesAutoComplete",
                type: "POST",
                dataType: "json",
                data: { CustomerId: $("#CustomerId").val(), Prefix: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.ShipToNum, value: item.ShipToNum };
                    }))
                }
            })
        },
        messages: {
            noResults: "",
            results: function (resultsCount) { }
        }
    });
})

$(document).ready(function () {
    $("#cedulaSH").focusout(function () {

        $.ajax({
            url: "SelectShiptoeFromTextBox",
            dataType: 'json',
            type: "POST",
            data: { CustomerId: $("#CustomerId").val(), ShipToNum: $("#cedulaSH").val() },
            success: function (shipto) {
                if (shipto.ShipToNum != "9999") {
                    $("#sID").prop("value", shipto.ShipToId);
                }
                else {
                    alert("No Encontrado");
                }
            },
        });
        return false;
    })
})


$(document).ready(function () {
    $("#sID").change(function () {
        $("#cedulaSH").empty();
        $.ajax({
            type: 'POST',
            url: "GetShipToeData",
            dataType: 'json',
            data: { ShipToId: $("#sID").val() },
            success: function (shiptoes) {
                $.each(shiptoes, function (i, shipto) {
                    $("#cedulaSH").prop("value", shipto.ShipToNum);
                });
            },
        });

        return false;
    })
})




$(document).ready(function () { 
    $("#sID").focusout(function () {
        $("#ContactId").empty();
        $.ajax({
            type: 'POST',
            url: "GetContactList",
            dataType: 'json',
            data: { ShipToId: $("#sID").val() },
            success: function (contacts) {
                $.each(contacts, function (i, contact) {
                    $("#ContactId").append('<option value="'
                        + contact.ContactId + '">'
                        + contact.Name + '</option>');
                });
            },
        });
    });
})
