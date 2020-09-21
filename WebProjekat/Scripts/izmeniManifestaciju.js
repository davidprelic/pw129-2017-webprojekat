$(document).ready(function () {

    $('.header').height($(window).height());

    $.ajax({
        url: '/sesija',
        method: 'GET',
        success: function (data) {
            if (data !== 'PRODAVAC') {
                window.location.href = "index.html";
            }
            else {
                var kartice = '<li class="nav-item"><a class="nav-link" href="prodavacManifestacije.html"> Moje manifestacije</a></li>';
                $('#kartice').after(kartice);
                var kartice = '<li class="nav-item"><a class="nav-link" href="prodavacKarte.html"> Rezervisane karte</a></li>';
                $('#kartice').after(kartice);
                var kartice = '<li class="nav-item"><a class="nav-link" href="kreirajManifestaciju.html"> Kreiraj manifestaciju</a></li>';
                $('#kartice').after(kartice);
                var kartice = '<li class="nav-item"><a class="nav-link" href="komentari.html"> Prikaz komentara</a></li>';
                $('#kartice').after(kartice);
                $('#logProfKartica').text('Profil');
                $('#regLogoutKartica').text('Odjavi se');
            }
        }
    });

    $('#regLogoutKartica').click(function () {
        if ($('#regLogoutKartica').text() === 'Odjavi se') {
            $.ajax({
                url: '/account/logout',
                method: 'GET',
                success: function () {
                    window.location.href = "index.html";
                }
            });
        }
    });

    var getUrlParameter = function getUrlParameter(sParam) {
        var sPageURL = window.location.search.substring(1),
            sURLVariables = sPageURL.split('&'),
            sParameterName,
            i;
        for (i = 0; i < sURLVariables.length; i++) {
            sParameterName = sURLVariables[i].split('=');
            if (sParameterName[0] === sParam) {
                return sParameterName[1] === undefined ? true : decodeURIComponent(sParameterName[1]);
            }
        }
    };

    var idParametar = getUrlParameter('id');

    var kupacId;
    var izabranTipKarte;
    var cenaKarteSaPopustom;
    var brojDodatnihBodova;

    var cenaRegularKarte;
    var datumManif;

    function pad(str, max) {
        str = str.toString();
        return str.length < max ? pad("0" + str, max) : str;
    }

    $.ajax({
        url: `/manifestacije/${idParametar}`,
        method: 'GET',
        success: function (data) {
            datumManif = data.DatumVremeOdrzavanja;
            var datum = new Date(data.DatumVremeOdrzavanja);
            var dan = pad(datum.getDate(), 2);
            var mesec = pad(datum.getMonth() + 1, 2);
            var godina = datum.getFullYear();
            var tipManif = "";
            var status = "";

            switch (data.Status) {
                case 0:
                    status = "AKTIVNA";
                    break;
                case 1:
                    status = "NEAKTIVNA";
            }

            cenaRegularKarte = data.CenaRegularKarte;

            switch (data.Tip) {
                case 0:
                    tipManif = "KONCERT";
                    break;
                case 1:
                    tipManif = "FESTIVAL";
                    break;
                case 2:
                    tipManif = "POZORISTE";
                    break;
                case 3:
                    tipManif = "SPORT";
            }

            var manif = '<div class="card mb-3">'
                + '<div class="row no-gutters">'
                + '<div class="col-md-4">'
                + `<img src="${data.PosterManifestacije}" class="card-img" alt="...">`
                + '</div>'
                + '<div class="col-md-8">'
                + '<div class="card-body">'
                + `<h5 class="card-title"><input type="text" id="naziv" name="naziv" value="${data.Naziv}"></h5>`
                + `<p class="card-text">${tipManif}</p>`
                + `<p class="card-text">Broj mesta: <input type="number" id="brojMesta" min = "0" step = "1" name="brojMesta" value="${data.BrojMesta}"></p>`
                + `<p class="card-text">Prestali broj regular karata: regular: <input type = "number" min = "0" step = "1" id = "brojRegularKarata" name = "brojRegularKarata" value="${data.BrojRegularKarata}" ></p>`
                + `<p class="card-text">Prestali broj vip karata: regular: <input type="number" min = "0" step = "1" id = "brojVipKarata" name = "brojVipKarata" value="${data.BrojVipKarata}" ></p>`
                + `<p class="card-text">Prestali broj fanpit karata: regular: <input type="number" min = "0" step = "1" id = "brojFanpitKarata" name = "brojFanpitKarata" value="${data.BrojFanpitKarata}" ></p>`
                + '<p class="card-text">Datum odrzavanja: ' + `<input type="date" id="datum" name="datum" value="${godina}-${mesec}-${dan}">` + "</p>"
                + `<p class="card-text">Cena regular karte:  <input type = "number" min = "0" step = "0.01" id = "cenaRegularKarte" name = "cenaRegularKarte" value="${data.CenaRegularKarte}" > </p>`
                + '<p class="card-text">Status: ' + status + '</p>'
                + `<p class="card-text">Mesto odrzavanja: ${data.Ulica}, ${data.Grad}, ${data.Drzava}, ${data.PostanskiBroj}</p>`
                + `<p class="card-text"><button class="sacuvajIzmene btn btn-primary" data-id="${idParametar}" onclick="izmeniManifestaciju(this)" > Sacuvaj izmene</button ></p>`
                + '</div>'
                + '</div>'
                + '</div>'
                + '</div>'
            $('#prikazManif').append(manif);
        },
        error: function (jqXHR) {
            alert("ERROR");
        }
    });
});

function izmeniManifestaciju(manif) {
    $(document).on('click', '.sacuvajIzmene', function () {
        $("p.poruka").remove();
        if ($('#naziv').val().length < 4)
            $('#naziv').after('<p class="poruka">Naziv manifestacije mora imati minimum 4 karaktera</p>');
        else if ($('#brojMesta').val() < 1)
            $('#brojMesta').after('<p class="poruka">Unesite broj mesta</p>');
        else if ($('#brojRegularKarata').val() < 1)
            $('#brojRegularKarata').after('<p class="poruka">Unesite broj regular karata</p>');
        else if ($('#brojVipKarata').val() < 1)
            $('#brojVipKarata').after('<p class="poruka">Unesite broj vip karata</p>');
        else if ($('#brojFanpitKarata').val() < 1)
            $('#brojFanpitKarata').after('<p class="poruka">Unesite broj fanpit karata</p>');
        else if (!Date.parse($('#datumVremeOdrzavanja').val()))
            $('#datumVremeOdrzavanja').after('<p class="poruka">Unesite datum odrzavanja</p>');
        else if ($('#cenaRegularKarte').val() < 1)
            $('#cenaRegularKarte').after('<p class="poruka">Unesite cenu regular karte</p>');
        else {
            $.ajax({
                url: '/manifestacije',
                method: 'PUT',
                data: {
                    Id: $(manif).attr("data-id"),
                    Naziv: $('#naziv').val(),
                    BrojMesta: $('#brojMesta').val(),
                    BrojRegularKarata: $('#brojRegularKarata').val(),
                    BrojVipKarata: $('#brojVipKarata').val(),
                    BrojFanpitKarata: $('#brojFanpitKarata').val(),
                    DatumVremeOdrzavanja: $('#datum').val(),
                    CenaRegularKarte: $('#cenaRegularKarte').val()
                },
                success: function () {
                    window.location.href = "index.html";
                    console.log("USPESNA POSLATI PODACI AJAXOM");
                },
                error: function (jqXHR) {
                    alert("ERROR");
                }
            });
        }

        
    });
}