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

        var data = $('#datainput').val();

        $.ajax({
            type: 'post',
            url: 'api/blocks',
            dataType: 'json',
            contentType: 'application/json',
            data: JSON.stringify(data),
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
    tr.append('<td>' + formatDate(value.timestamp) + '</td>');
    tr.append('<td>' + value.difficulty + '</td>');
    tr.append('<td>' + value.nonce + '</td>');
    tr.append('<td>' + Math.round(value.performance * 100) / 100 + '</td>');

    $('#blocks').append(tr);
}

//Source: https://stackoverflow.com/a/25275808
function formatDate(date) {

  var date = new Date(date);
  var hours = date.getHours();
  var minutes = date.getMinutes();
  var ampm = hours >= 12 ? 'pm' : 'am';
  hours = hours % 12;
  hours = hours ? hours : 12; // the hour '0' should be '12'
  minutes = minutes < 10 ? '0'+minutes : minutes;
  var strTime = hours + ':' + minutes + ' ' + ampm;
  return date.getMonth()+1 + "/" + date.getDate() + "/" + date.getFullYear() + "  " + strTime;
}
