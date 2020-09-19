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
            center: ol.proj.fromLonLat([37.41, 8.82]),
            zoom: 4
        })
    });


    $("#btnManif").click(function () {
        var manifTip = document.getElementById("tip");
        var manifTipSelected = manifTip.options[manifTip.selectedIndex].value;

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
                MestoOdrzavanjaID: $('#mestoOdrzavanjaID').val(),
                PosterManifestacije: $('#posterManifestacije').val(),
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