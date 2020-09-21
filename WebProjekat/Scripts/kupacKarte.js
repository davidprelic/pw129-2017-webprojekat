$(document).ready(function () {

    $('.header').height($(window).height());

    $.ajax({
        url: '/sesija',
        method: 'GET',
        success: function (data) {
            if (data !== 'KUPAC') {
                window.location.href = "index.html";
            }
            else {
                var kartice = '<li class="nav-item"><a class="nav-link" href="kupacKarte.html">Moje karte</a></li>';
                $('#kartice').after(kartice);
                $('#logProfKartica').text('Profil');
                $('#regLogoutKartica').text('Odjavi se');
            }
        }
    });


    $('body').on('click', '#odustani', function () {
        console.log("PROVERA KLIKA ODUSTANI");
        $.ajax({
            url: `/odustani-od-karte`,
            method: 'GET',
            data: {
                Id: $(this).attr("data-id")
            },
            success: function (data) {
                window.location.href = "index.html";
            }
        });
    });

    $.ajax({
        url: '/karte',
        method: 'GET',
        success: function (data) {
            for (var i = 0; i < data.length; i++) {
                var datum = new Date(data[i].DatumVremeManifestacije);
                var mesec = datum.getMonth() + 1;

                var tipKarte = "";

                switch (data[i].Tip) {
                    case 0:
                        tipKarte = "VIP";
                        break;
                    case 1:
                        tipKarte = "REGULAR";
                        break;
                    case 2:
                        tipKarte = "FANPIT";
                }

                var trenutniDatum = new Date();
                trenutniDatum.setDate(trenutniDatum.getDate() + 7);

                var eachKarta = "<tr>"
                    + "<td>" + `<a data-id="${data[i].ManifestacijaID}" onclick="prikaziManif(this)">` + data[i].NazivManif + '</a>' + "</td>"
                    + "<td>" + datum.getDate() + '/' + mesec + '/' + datum.getFullYear() + "</td>"
                    + "<td>" + data[i].Cena + "</td>"
                    + "<td>" + '<a>' + data[i].KorImeKupca + '</a>' + "</td>"
                    + "<td>" + tipKarte + "</td>";
                if (data[i].Status === 0)
                    eachKarta += "<td>Rezervisana</td>";
                else
                    eachKarta += "<td>Odustanak</td>";

                if (trenutniDatum < datum && data[i].Status === 0) {
                    eachKarta += `<td><button type="button" id="odustani" class="btn btn-primary" data-id="${data[i].Id}">Odustani</button></td>`;
                }
                    eachKarta += "</tr>";
                $('#tbodyKarteKupca').append(eachKarta);
            }

        }
    });




    $('.primeniFiltere').click(function () {
        var sort = document.getElementById("opcijaSort");
        var sortOption = sort.options[sort.selectedIndex].value;

        var filter = document.getElementById("opcijaFilter");
        var filterOption = filter.options[filter.selectedIndex].value;

        $("#tbodyKarteKupca").remove();

        $.ajax({
            url: '/filterkarte',
            method: 'GET',
            data: {
                Manifestacija: $('#manifestacija').val(),
                DatumOd: $('#datumOd').val(),
                DatumDo: $('#datumDo').val(),
                CenaOd: $('#cenaOd').val(),
                CenaDo: $('#cenaDo').val(),
                OpcijaSort: sortOption,
                OpcijaFilter: filterOption
            },
            success: function (data) {
                console.log("PROVERA PROVERA");
                $('table').append('<tbody id="tbodyKarteKupca"></tbody>');

                for (var i = 0; i < data.length; i++) {
                    var datum = new Date(data[i].DatumVremeManifestacije);
                    var mesec = datum.getMonth() + 1;

                    var tipKarte = "";

                    switch (data[i].Tip) {
                        case 0:
                            tipKarte = "VIP";
                            break;
                        case 1:
                            tipKarte = "REGULAR";
                            break;
                        case 2:
                            tipKarte = "FANPIT";
                    }

                    var trenutniDatum = new Date();
                    trenutniDatum.setDate(trenutniDatum.getDate() + 7);

                    var eachKarta = "<tr>"
                        + "<td>" + `<a data-id="${data[i].ManifestacijaID}" onclick="prikaziManif(this)">` + data[i].NazivManif + '</a>' + "</td>"
                        + "<td>" + datum.getDate() + '/' + mesec + '/' + datum.getFullYear() + "</td>"
                        + "<td>" + data[i].Cena + "</td>"
                        + "<td>" + '<a>' + data[i].KorImeKupca + '</a>' + "</td>"
                        + "<td>" + tipKarte + "</td>";
                    if (data[i].Status === 0)
                        eachKarta += "<td>Rezervisana</td>";
                    else
                        eachKarta += "<td>Odustanak</td>";

                    if (trenutniDatum < datum && data[i].Status === 0) {
                        eachKarta += `<td><button type="button" id="odustani" class="btn btn-primary" data-id="${data[i].Id}">Odustani</button></td>`;
                    }
                    eachKarta += "</tr>";
                    $('#tbodyKarteKupca').append(eachKarta);
                }
            },
            error: function (jqXHR) {
                alert("ERROR");
            }
        });
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
});

function prikaziManif(jednaManif) {
    window.location = "pregledManifestacije.html" + `?id=${$(jednaManif).attr("data-id")}`;
}