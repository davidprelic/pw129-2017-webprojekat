$(document).ready(function () {

    $('.header').height($(window).height());
    var uloga = "";
    $.ajax({
        url: '/sesija',
        method: 'GET',
        success: function (data) {
            uloga = data;
            if (data === 'ADMINISTRATOR') {
                var kartice = '<li class="nav-item"><a class="nav-link" href="korisnici.html"> Pregled korisnika</a></li>';
                $('#kartice').after(kartice);
                var kartice = '<li class="nav-item"><a class="nav-link" href="adminKarte.html"> Sve karte</a></li>';
                $('#kartice').after(kartice);
                var kartice = '<li class="nav-item"><a class="nav-link" href="kreirajProdavca.html"> Kreiraj prodavca</a></li>';
                $('#kartice').after(kartice);
                var kartice = '<li class="nav-item"><a class="nav-link" href="potvrdaManifestacija.html"> Potvrda manifestacija</a></li>';
                $('#kartice').after(kartice);
                var kartice = '<li class="nav-item"><a class="nav-link" href="komentari.html"> Prikaz komentara</a></li>';
                $('#kartice').after(kartice);
                $('#logProfKartica').text('Profil');
                $('#regLogoutKartica').text('Odjavi se');
            }
            else if (data === 'PRODAVAC') {
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

    $('body').on('click', '.odobriDugme', function () {
        $.ajax({
            url: '/odobri-komentar',
            method: 'GET',
            data: {
                Id: $(this).attr("data-id")
            },
            success: function () {
                window.location.href = "komentari.html";
            }
        });
    });

    $('body').on('click', '.odbijDugme', function () {
        $.ajax({
            url: '/odbij-komentar',
            method: 'GET',
            data: {
                Id: $(this).attr("data-id")
            },
            success: function () {
                window.location.href = "komentari.html";
            }
        });
    });


    $('body').on('click', '#obrisiKomentar', function () {
        console.log("PROVERA KLIKA OBRISI KOMENTAR");
        console.log($(this).attr("data-id"));
        $.ajax({
            url: `/obrisi-komentar?id=${$(this).attr("data-id")}`,
            method: 'DELETE',
            success: function (data) {
                window.location.href = "index.html";
            }
        });
    });


    $.ajax({
        url: '/komentari',
        method: 'GET',
        success: function (data) {
            var komentari = JSON.parse(data);

            var ocenaManif = "";
            var status = "";

            for (var i = 0; i < komentari.length; i++) {

                switch (komentari[i].Ocena) {
                    case 0:
                        ocenaManif = "1";
                        break;
                    case 1:
                        ocenaManif = "2";
                        break;
                    case 2:
                        ocenaManif = "3";
                        break;
                    case 3:
                        ocenaManif = "4";
                        break;
                    case 4:
                        tipManif = "5";
                }

                switch (komentari[i].Status) {
                    case 0:
                        status = "Na cekanju";
                        break;
                    case 1:
                        status = "Prihvacen";
                        break;
                    case 2:
                        status = "Odbijen";
                }

                var eachrow;

                if (status === "Na cekanju") {
                    eachrow = "<tr>"
                        + "<td>" + komentari[i].KorisnickoIme + "</td>"
                        + "<td>" + komentari[i].NazivManif + "</td>"
                        + "<td>" + `<textarea disabled>${komentari[i].Tekst}</textarea>` + "</td>"
                        + "<td>" + ocenaManif + "</td>"
                        + "<td>" + status + "</td>"
                        + "<td>" + `<button class="odobriDugme" data-id="${komentari[i].Id}">Odobri</button>` + `<button class="odbijDugme" data-id="${komentari[i].Id}">Odbij</button>` + "</td>"
                        + "</tr>";
                    $('#tbodySviKomentari').append(eachrow);
                }
                else {
                    eachrow = "<tr>"
                        + "<td>" + komentari[i].KorisnickoIme + "</td>"
                        + "<td>" + komentari[i].NazivManif + "</td>"
                        + "<td>" + `<textarea disabled>${komentari[i].Tekst}</textarea>` + "</td>"
                        + "<td>" + ocenaManif + "</td>"
                        + "<td>" + status + "</td>";
                    if (uloga == "ADMINISTRATOR") {
                        eachrow += `<td><button type="button" id="obrisiKomentar" class="btn btn-primary" data-id="${komentari[i].Id}">Obrisi komentar</button></td>`;
                    }

                        eachrow += "</tr>";
                    $('#tbodySviKomentari').append(eachrow);
                }
            }
        }
    });


});