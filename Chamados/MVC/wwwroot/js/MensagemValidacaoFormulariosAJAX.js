
    var onFailure = function(response, status, error) {
        $("#return").html(response.responseText).removeClass().addClass("alert alert-danger");
    };


        var onSuccess = function(response, status) {
        $("#return").html("Aleração realizada com sucesso").removeClass().addClass("alert alert-success");
    };
