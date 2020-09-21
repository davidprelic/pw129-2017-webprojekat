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

    var map = new ol.Map({
        target: 'map',
        layers: [
            new ol.layer.Tile({
                source: new ol.source.OSM()
            })
        ],
        view: new ol.View({
            center: ol.proj.fromLonLat([21.0059, 44.0165]),
            zoom: 4
        })
    });

    var geoDuzina;
    var geoSirina;
    var ulica;
    var grad;
    var drzava;
    var postanskiBroj;

    function reverseGeocode(coords) {
        fetch('http://nominatim.openstreetmap.org/reverse?format=json&lon=' + coords[0] + '&lat=' + coords[1])
            .then(function (response) {
                return response.json();
            }).then(function (json) {
                console.log(json);
                $("#ulica").text("Ulica: " + json.address.road);
                ulica = json.address.road;
                $("#grad").text("Grad: " + json.address.city);
                grad = json.address.city;
                $("#drzava").text("Drzava: " + json.address.country);
                drzava = json.address.country;
                $("#postanskiBroj").text("Postanski broj: " + json.address.postcode);
                postanskiBroj = json.address.postcode;
                //console.log(json.display_name);
            });
    }

    map.on('click', function (evt) {
        var coord = ol.proj.toLonLat(evt.coordinate);
        geoDuzina = coord[0];
        geoSirina = coord[1];
        $("#geoDuz").text("Geografska duzina: (" + geoDuzina + ")");
        $("#geoSir").text("Geografska sirina: (" + geoSirina + ")");
        
        reverseGeocode(coord);
    });


    $("#btnManif").click(function () {
        var manifTip = document.getElementById("tip");
        var manifTipSelected = manifTip.options[manifTip.selectedIndex].value;

        var posterManif = `/Images/${$('input[type=file]').val().replace(/C:\\fakepath\\/i, '')}`;

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
                url: '/manifestacija',
                method: 'POST',
                data: {
                    Naziv: $('#naziv').val(),
                    Tip: manifTipSelected,
                    BrojMesta: $('#brojMesta').val(),
                    BrojRegularKarata: $('#brojRegularKarata').val(),
                    BrojVipKarata: $('#brojVipKarata').val(),
                    BrojFanpitKarata: $('#brojFanpitKarata').val(),
                    DatumVremeOdrzavanja: $('#datumVremeOdrzavanja').val(),
                    CenaRegularKarte: $('#cenaRegularKarte').val(),
                    Status: $('#status').val(),
                    PosterManifestacije: posterManif,
                    GeografskaSirina: geoSirina,
                    GeografskaDuzina: geoDuzina,
                    Ulica: ulica,
                    Grad: grad,
                    Drzava: drzava,
                    PostanskiBroj: postanskiBroj,
                    IsDeleted: $('isDeleted').val()
                },
                success: function () {
                    console.log("USPESNA POSLATI PODACI AJAXOM");

                    var formData = new FormData();
                    var opmlFile = $('#posterManifestacije')[0];
                    formData.append("opmlFile", opmlFile.files[0]);

                    $.ajax({
                        url: '/upload-slike',
                        type: 'POST',
                        data: formData,
                        cache: false,
                        contentType: false,
                        processData: false
                    });

                    window.location.href = "index.html";
                },
                error: function (jqXHR) {
                    alert("ERROR");
                }

            });
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
});