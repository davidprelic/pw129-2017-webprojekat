$(document).ready(function () {

    $('.header').height($(window).height());

    $.ajax({
        url: '/sesija',
        method: 'GET',
        success: function (data) {
            if (data !== 'NEULOGOVAN') {
                window.location.href = "index.html";
            }
        }
    });

    $("#btnReg").click(function () {
        $.ajax({
            url: '/korisnik',
            method: 'POST',
            data: {
                KorisnickoIme: $('#korIme').val(),
                Lozinka: $('#lozinka').val(),
                Ime: $('#ime').val(),
                Prezime: $('#prezime').val(),
                Pol: $('input[name="pol"]:checked').val(),
                DatumRodjenja: $('#datumRodj').val(),
                Uloga: $('#uloga').val(),
                BrojSakupljenihBodova: $('brojSakupljenihBodova').val(),
                IsDeleted: $('isDeleted').val()
            },
            success: function () {
                console.log("USPESNA POSLATI PODACI AJAXOM");
                window.location.href = "index.html";
            },
            error: function (jqXHR) {
                alert("ERROR");
            }
        });
    });
});