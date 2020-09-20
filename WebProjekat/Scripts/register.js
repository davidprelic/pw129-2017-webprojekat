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
        $("p").remove();
        if ($('#korIme').val().length < 4)
            $('#korIme').after('<p>Korisnicko ime mora imati minimum 4 karaktera</p>');
        else if ($('#lozinka').val().length < 8)
            $('#lozinka').after('<p>Lozinka mora imati minimum 8 karaktera</p>');
        else if ($('#ime').val().length < 1)
            $('#ime').after('<p>Unesite ime</p>');
        else if ($('#prezime').val().length < 1)
            $('#prezime').after('<p>Unesite prezime</p>');
        else if (!Date.parse($('#datumRodj').val())) 
            $('#datumRodj').after('<p>Unesite datum rodjenja</p>');
        else {
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
        }
        
    });
});