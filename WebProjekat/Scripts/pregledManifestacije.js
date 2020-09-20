$(document).ready(function () {

    $('.header').height($(window).height());

    $.ajax({
        url: '/sesija',
        method: 'GET',
        success: function (data) {
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
            else if (data === 'KUPAC') {
                var kartice = '<li class="nav-item"><a class="nav-link" href="kupacKarte.html">Moje karte</a></li>';
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

    $('body').on('click', '#kupi', function () {
        console.log("PROVERA KLIKA KUPI");
        var tipKarte = document.getElementById("tipkarte");
        izabranTipKarte = tipKarte.options[tipKarte.selectedIndex].value;

        $.ajax({
            url: '/provera-popusta',
            method: 'GET',
            success: function (data) {
                kupacId = data.Id;
                console.log(data);
                var cenaKarte;
                if (izabranTipKarte === 'regular')
                    cenaKarte = cenaRegularKarte;
                else if (izabranTipKarte === 'vip')
                    cenaKarte = 4 * cenaRegularKarte;
                else
                    cenaKarte = 2 * cenaRegularKarte;

                var ukupnaCena = $('#brojKarata').val() * cenaKarte;
                cenaKarteSaPopustom = cenaKarte - (cenaKarte * (data.Popust / 100));
                var ukupnaCenaSaPopustom = $('#brojKarata').val() * cenaKarteSaPopustom;

                brojDodatnihBodova = cenaKarte / 1000 * 133;

                var potvrdaIspis = '<div class="ispismodal">'
                    + '<p>Rezervisete ' + $('#brojKarata').val() + ' ' + izabranTipKarte + ' karte po ceni od ' + cenaKarteSaPopustom + ' po karti' +'</p>'
                    + '<p>Ukupna cena: ' + ukupnaCenaSaPopustom + '</p>'
                    + `</div>`;

                $('.modal-body').append(potvrdaIspis);
            }
        });
    });

    $('body').on('click', '.odustanak', function () {
        console.log("ODUSTANI KLIK");
        $(".ispismodal").remove();
    });

    $('body').on('click', '.potvrda', function () {
        console.log("POTVRDA KLIK");
        $.ajax({
            url: '/karte',
            method: 'POST',
            data: {
                ManifestacijaId: idParametar,
                DatumVremeManifestacije: datumManif,
                Cena: cenaKarteSaPopustom,
                BrojRezervisanihKarata: $('#brojKarata').val(),
                KupacID: kupacId,
                Tip: izabranTipKarte,
                BrojDodatnihBodova: brojDodatnihBodova
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


    $('body').on('click', '#komentarisi', function () {
        console.log("PROVERA KLIKA KOMENTARISI");

        $.ajax({
            url: '/komentari',
            method: 'POST',
            data: {
                ManifestacijaId: idParametar,
                Tekst: $('#komentar').val(),
                Ocena: $('#ocena').val(),
                Status: $('#status').val(),
                IsDeleted: $('#isDeleted').val(),
            },
            success: function () {
                console.log("USPESNA POSLATI PODACI AJAXOM");
            },
            error: function (jqXHR) {
                alert("ERROR");
            }
        });
    });


    var cenaRegularKarte;
    var datumManif;

    $.ajax({
        url: `/manifestacije/${idParametar}`,
        method: 'GET',
        success: function (data) {
            datumManif = data.DatumVremeOdrzavanja;
            var datum = new Date(data.DatumVremeOdrzavanja);
            var mesec = datum.getMonth() + 1;
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

            var map = new ol.Map({
                target: 'map',
                layers: [
                    new ol.layer.Tile({
                        source: new ol.source.OSM()
                    })
                ],
                view: new ol.View({
                    center: ol.proj.fromLonLat([data.GeografskaDuzina, data.GeografskaSirina]),
                    zoom: 14
                })
            });

            var manif = '<div class="card mb-3">'
                + '<div class="row no-gutters">'
                + '<div class="col-md-4">'
                + `<img src="${data.PosterManifestacije}" class="card-img" alt="...">`
                + '</div>'
                + '<div class="col-md-8 test">'
                + '<div class="card-body">'
                + `<h5 class="card-title">${data.Naziv}</h5>`
                + `<p class="card-text">${tipManif}</p>`
                + `<p class="card-text">Broj mesta: ${data.BrojMesta}</p>`
                + `<p class="card-text">Prestali broj karata: regular: ${data.BrojRegularKarata}, fanpit: ${data.BrojFanpitKarata}, vip: ${data.BrojVipKarata}</p>`
                + '<p class="card-text">Datum odrzavanja: ' + datum.getDate() + '/' + mesec + '/' + datum.getFullYear() + "</p>"
                + `<p class="card-text">Cena karata: regular: ${data.CenaRegularKarte} rsd, fanpit: ${data.CenaRegularKarte * 2} rsd, vip: ${data.CenaRegularKarte * 4} rsd</p>`
                + '<p class="card-text">Status: ' + status + '</p>'
                + `<p class="card-text">Mesto odrzavanja: ${data.Ulica}, ${data.Grad}, ${data.Drzava}, ${data.PostanskiBroj}</p>`;
            if (data.OcenaManifestacije)
                manif += '<p class="card-text">Prosecna ocena: ' + data.OcenaManifestacije + "</p>";

                manif += '</div>'
                + '</div>'
                + '</div>'
                + '</div>'
            $('#prikazManif').append(manif);

            $.ajax({
                url: '/komentari-jedne-manif',
                method: 'GET',
                data: {
                    ManifestacijaId: idParametar,
                },
                success: function (data) {
                    var komentari = JSON.parse(data);

                    var ocenaManif = "";

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

                        var komentar = '<div class="jedanKomentar" border>'
                            + "<span>" + komentari[i].KorisnickoIme + " | " + komentari[i].Tekst + " | " +  ocenaManif + "</span>"
                            + "</div>";
                        $('#prikazManif').append(komentar);

                    }
                }
            });



            $.ajax({
                url: '/kupac-poseduje-kartu',
                method: 'GET',
                data: {
                    Id: idParametar
                },
                success: function (data) {
                    var trenutniDatum = new Date();

                    if (data.KorisnikJeKupac && trenutniDatum < datum) {
                        var kupovinaKarata = '<select id="tipkarte" name="tipkarte">'
                                                + '<option value="regular">Regular karta</option>'
                                                + '<option value="vip">Vip karta</option>'
                                                + '<option value="fanpit">Fanpit karta</option>'
                                           + '</select>'               
                                           + '<input type="number" min="1" step="1" id="brojKarata" name="brojKarata" placeholder="Broj karata">'
                                           + `<button type="button" id="kupi" class="btn btn-primary" data-toggle="modal" data-target="#exampleModal">Rezervisi</button>`;
                        $('.kupovina').append(kupovinaKarata);
                    }
                    else if (data.KorisnikJeKupac && data.KorisnikPosedujeKartuManifestacije && trenutniDatum > datum) {
                        var komentarisanje = 'Ocena manifestacije : ' + '<select id="ocena" name="ocena">'
                                                + '<option value="pet">5</option>'
                                                + '<option value="cetiri">4</option>'
                                                + '<option value="tri">3</option>'
                                                + '<option value="dva">2</option>'
                                                + '<option value="jedan">1</option>'
                                            + '</select>'  
                                            + '<br>'
                                            + '<textarea id="komentar" name="komentar" rows="4" cols="50"></textarea>'
                                            + '<input id="isDeleted" name="isDeleted" type="hidden" value="False">'
                                            + '<input id="status" name="status" type="hidden" value="Nacekanju">'
                                            + '<br>'
                                            + `<button type="button" id="komentarisi" class="btn btn-primary">Komentarisi i oceni</button>`;
                        $('.komentari').append(komentarisanje);
                    }
                },
                error: function (jqXHR) {
                    alert("ERROR");
                }
            });
        },
        error: function (jqXHR) {
            alert("ERROR");
        }
    });


});