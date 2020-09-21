$(document).ready(function () {

    $('.header').height($(window).height());

    $.ajax({
        url: '/sesija',
        method: 'GET',
        success: function (data) {
            if (data !== 'ADMINISTRATOR') {
                window.location.href = "index.html";
            }
            else {
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


    $('.primeniFiltere').click(function () {
        var sort = document.getElementById("opcijaSort");
        var sortOption = sort.options[sort.selectedIndex].value;

        var filter = document.getElementById("opcijaFilter");
        var filterOption = filter.options[filter.selectedIndex].value;

        $("#tbodyAdmin").remove();
        $("#tbodyProdavac").remove();
        $("#tbodyKupac").remove();

        $.ajax({
            url: '/filter-korisnici',
            method: 'GET',
            data: {
                Ime: $('#ime').val(),
                Prezime: $('#prezime').val(),
                KorisnickoIme: $('#korisnickoIme').val(),
                OpcijaSort: sortOption,
                OpcijaFilter: filterOption
            },
            success: function (data) {

                $('#listaAdmina').append('<tbody id="tbodyAdmin"></tbody>');
                $('#listaProdavaca').append('<tbody id="tbodyProdavac"></tbody>');
                $('#listaKupaca').append('<tbody id="tbodyKupac"></tbody>');

                for (var i = 0; i < data.length; i++) {
                    //Administrator
                    if (data[i].Uloga === 0) {
                        var datum = new Date(data[i].DatumRodjenja);
                        var mesec = datum.getMonth() + 1;
                        var eachrow = "<tr>"
                            + "<td>" + data[i].KorisnickoIme + "</td>"
                            + "<td>" + data[i].Lozinka + "</td>"
                            + "<td>" + data[i].Ime + "</td>"
                            + "<td>" + data[i].Prezime + "</td>"
                            + "<td>" + (data[i].Pol ? 'Zensko' : 'Musko') + "</td>"
                            + "<td>" + datum.getDate() + '/' + mesec + '/' + datum.getFullYear() + "</td>"
                            + "</tr>";
                        $('#tbodyAdmin').append(eachrow);
                    }
                    //Prodavac
                    else if (data[i].Uloga === 1) {
                        var datum = new Date(data[i].DatumRodjenja);
                        var mesec = datum.getMonth() + 1;
                        var eachrow = "<tr>"
                            + "<td>" + data[i].KorisnickoIme + "</td>"
                            + "<td>" + data[i].Lozinka + "</td>"
                            + "<td>" + data[i].Ime + "</td>"
                            + "<td>" + data[i].Prezime + "</td>"
                            + "<td>" + (data[i].Pol ? 'Zensko' : 'Musko') + "</td>"
                            + "<td>" + datum.getDate() + '/' + mesec + '/' + datum.getFullYear() + "</td>"
                            + "<td>" + '<button class="prikazDugme">Blokiraj</button>' + "</td>"
                            + `<td><button type="button" id="obrisiKorisnika" class="btn btn-primary" data-id="${data[i].Id}">Obrisi (blokiraj) korisnika</button></td>`
                            + "</tr>";
                        $('#tbodyProdavac').append(eachrow);
                    }
                    //Kupac
                    else {
                        var tipKor = "";
                        switch (data[i].TipKorisn.ImeTipa) {
                            case 0:
                                tipKor = "ZLATNI";
                                break;
                            case 1:
                                tipKor = "SREBRNI";
                                break;
                            case 2:
                                tipKor = "BRONZANI";
                        }

                        var datum = new Date(data[i].DatumRodjenja);
                        var mesec = datum.getMonth() + 1;
                        var eachrow = "<tr>"
                            + "<td>" + data[i].KorisnickoIme + "</td>"
                            + "<td>" + data[i].Lozinka + "</td>"
                            + "<td>" + data[i].Ime + "</td>"
                            + "<td>" + data[i].Prezime + "</td>"
                            + "<td>" + (data[i].Pol ? 'Zensko' : 'Musko') + "</td>"
                            + "<td>" + datum.getDate() + '/' + mesec + '/' + datum.getFullYear() + "</td>"
                            + "<td>" + data[i].BrojSakupljenihBodova + "</td>"
                            + "<td>" + tipKor + "</td>"
                            + `<td><button type="button" id="obrisiKorisnika" class="btn btn-primary" data-id="${data[i].Id}">Obrisi (blokiraj) korisnika</button></td>`
                            + "<td>" + '<button class="prikazDugme">Blokiraj</button>' + "</td>"
                            + "</tr>";
                        $('#tbodyKupac').append(eachrow);
                    }
                }
            },
            error: function (jqXHR) {
                alert("ERROR");
            }
        });
    });

    $('body').on('click', '#obrisiKorisnika', function () {
        console.log("PROVERA KLIKA OBRISI KORISNIKA");
        $.ajax({
            url: `/obrisi-korisnika?id=${$(this).attr("data-id")}`,
            method: 'DELETE',
            success: function (data) {
                window.location.href = "index.html";
            }
        });
    });

    $.ajax({
        url: '/korisnici',
        method: 'GET',
        success: function (data) {
            var korisnici = JSON.parse(data);

            for (var i = 0; i < korisnici.length; i++) {
                //Administrator
                if (korisnici[i].Uloga === 0) {
                    var datum = new Date(korisnici[i].DatumRodjenja);
                    var mesec = datum.getMonth() + 1;
                    var eachrow = "<tr>"
                        + "<td>" + korisnici[i].KorisnickoIme + "</td>"
                        + "<td>" + korisnici[i].Lozinka + "</td>"
                        + "<td>" + korisnici[i].Ime + "</td>"
                        + "<td>" + korisnici[i].Prezime + "</td>"
                        + "<td>" + (korisnici[i].Pol ? 'Zensko' : 'Musko') + "</td>"
                        + "<td>" + datum.getDate() + '/' + mesec + '/' + datum.getFullYear() + "</td>"
                        + "</tr>";
                    $('#tbodyAdmin').append(eachrow);
                }
                //Prodavac
                else if (korisnici[i].Uloga === 1) {
                    var datum = new Date(korisnici[i].DatumRodjenja);
                    var mesec = datum.getMonth() + 1;
                    var eachrow = "<tr>"
                        + "<td>" + korisnici[i].KorisnickoIme + "</td>"
                        + "<td>" + korisnici[i].Lozinka + "</td>"
                        + "<td>" + korisnici[i].Ime + "</td>"
                        + "<td>" + korisnici[i].Prezime + "</td>"
                        + "<td>" + (korisnici[i].Pol ? 'Zensko' : 'Musko') + "</td>"
                        + "<td>" + datum.getDate() + '/' + mesec + '/' + datum.getFullYear() + "</td>"
                        + `<td><button type="button" id="obrisiKorisnika" class="btn btn-primary" data-id="${korisnici[i].Id}">Obrisi (blokiraj) korisnika</button></td>`
                        + "<td>" + '<button class="prikazDugme">Blokiraj</button>' + "</td>"
                        + "</tr>";
                    $('#tbodyProdavac').append(eachrow);
                }
            }

                $.ajax({
                    url: '/korisnici-kupci',
                    method: 'GET',
                    success: function (data) {
                        var kupci = JSON.parse(data);
                        var tipKor = "";

                        for (var i = 0; i < kupci.length; i++) {

                            switch (kupci[i].TipKorisn.ImeTipa) {
                                case 0:
                                    tipKor = "ZLATNI";
                                    break;
                                case 1:
                                    tipKor = "SREBRNI";
                                    break;
                                case 2:
                                    tipKor = "BRONZANI";
                            }

                                var datum = new Date(kupci[i].DatumRodjenja);
                                var mesec = datum.getMonth() + 1;
                                var eachrow = "<tr>"
                                    + "<td>" + kupci[i].KorisnickoIme + "</td>"
                                    + "<td>" + kupci[i].Lozinka + "</td>"
                                    + "<td>" + kupci[i].Ime + "</td>"
                                    + "<td>" + kupci[i].Prezime + "</td>"
                                    + "<td>" + (kupci[i].Pol ? 'Zensko' : 'Musko') + "</td>"
                                    + "<td>" + datum.getDate() + '/' + mesec + '/' + datum.getFullYear() + "</td>"
                                    + "<td>" + kupci[i].BrojSakupljenihBodova + "</td>"
                                    + "<td>" + tipKor + "</td>"
                                    + `<td><button type="button" id="obrisiKorisnika" class="btn btn-primary" data-id="${kupci[i].Id}">Obrisi (blokiraj) korisnika</button></td>`
                                    + "<td>" + '<button class="prikazDugme">Blokiraj</button>' + "</td>"
                                    + "</tr>";
                                $('#tbodyKupac').append(eachrow);
                        }
                    }
                });
        }
    });
});