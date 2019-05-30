$(document).ready(function () {   
    $.ajax({
        type: 'GET',
        url: 'api/blocks',
        dataType: 'json',
        success: function (data) {
            $.each(data, function (index, value) {
                addBlock(value);

                if (index === data.length - 1) {
                    $('#prevhashinput').val(value.hash);
                }
            });
        }
    });

    $("#addblock").click(function () {
        event.preventDefault();

        $.ajax({
            type: 'post',
            url: 'api/blocks',
            dataType: 'json',
            contentType: 'application/json',
            data: JSON.stringify({ "data": $('#datainput').val(), "prevhash": $('#prevhashinput').val(), "difficulty": $('#difficultyinput').val() }),
            processData: false,
            success: function (data, textStatus, jQxhr) {
                addBlock(data);

                $('#datainput').val("");
                $('#prevhashinput').val(data.hash);
            },
            error: function (jqXhr, textStatus, errorThrown) {
                console.log(errorThrown);
            }
        });
    });
});

function addBlock(value) {
    var tr = $('<tr/>');

    tr.append('<td>' + value.id + '</td>');
    tr.append('<td>' + value.data + '</td>');
    tr.append('<td>' + value.hash + '</td>');
    tr.append('<td>' + value.prevhash + '</td>');
    tr.append('<td>' + value.timestamp + '</td>');
    tr.append('<td>' + value.difficulty + '</td>');
    tr.append('<td>' + value.nonce + '</td>');
    tr.append('<td>' + Math.round(value.performance * 100) / 100 + '</td>');

    $('#blocks').append(tr);
}
