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
                var kartice = '<li class="nav-item"><a class="nav-link" href="kreirajProdavca.html"> Kreiraj prodavca</a></li>';
                $('#kartice').after(kartice);
                var kartice = '<li class="nav-item"><a class="nav-link" href="potvrdaManifestacija.html"> Potvrda manifestacija</a></li>';
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
                        + "<td>" + '<button class="prikazDugme">Prikazi detaljno</button>' + "</td>"
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
                        + "<td>" + '<button class="prikazDugme">Prikazi detaljno</button>' + "</td>"
                        + "</tr>";
                    $('#tbodyProdavac').append(eachrow);
                }
                //Kupac
                else {
                    var datum = new Date(korisnici[i].DatumRodjenja);
                    var mesec = datum.getMonth() + 1;
                    var eachrow = "<tr>"
                        + "<td>" + korisnici[i].KorisnickoIme + "</td>"
                        + "<td>" + korisnici[i].Lozinka + "</td>"
                        + "<td>" + korisnici[i].Ime + "</td>"
                        + "<td>" + korisnici[i].Prezime + "</td>"
                        + "<td>" + (korisnici[i].Pol ? 'Zensko' : 'Musko') + "</td>"
                        + "<td>" + datum.getDate() + '/' + mesec + '/' + datum.getFullYear() + "</td>"
                        + "<td>" + '<button class="prikazDugme">Prikazi detaljno</button>' + "</td>"
                        + "</tr>";
                    $('#tbodyKupac').append(eachrow);
                }
            }
        }
    });


});