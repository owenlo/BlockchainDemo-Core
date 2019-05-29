$(document).ready(function () {

            $("#addblock").click(function () {
                $.ajax({
                    type: 'post',
                    url: 'api/blocks',
                    dataType: 'json',
                    contentType: 'application/json',
                    data: JSON.stringify( { "data": $('#datainput').val(), "prevhash": $('#prevhashinput').val(), "difficulty": $('#difficultyinput').val() } ),
                    processData: false,
                    success: function( data, textStatus, jQxhr ){
                        addBlock(data);

                        $('#datainput').val("");
                        $('#prevhashinput').val(data.hash);
                    },
                    error: function( jqXhr, textStatus, errorThrown ){
                        console.log( errorThrown );
                        console.log($('#datainput').val());
                    }
                });
            });

            $.ajax({
                type: 'GET',
                url: 'api/blocks',
                dataType: 'json',
                success: function (data) {
                    $.each(data, function (index, value) {
                        addBlock(value);

                        $('#prevhashinput').val(value.hash);
                    });
                }
            });
        });

        function addBlock(value) {
            var tr = $('<tr/>');

            tr.append('<td>' + value.id + '</td>');
            tr.append('<td>' + value.data + '</td>');
            tr.append('<td>' + trim(value.hash, value.difficulty) + '...</td>');
            tr.append('<td>' + trim(value.prevhash, value.difficulty) + '...</td>');
            tr.append('<td>' + value.timestamp + '</td>');
            tr.append('<td>' + value.difficulty + '</td>');
            tr.append('<td>' + value.nonce + '</td>');
            tr.append('<td>' + Math.round(value.performance * 100) / 100 + '</td>');

            $('#blocks').append(tr);
        }

        function trim(string, length) {
            return string.substring(0, length + 5);
        }