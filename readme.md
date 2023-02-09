[![build and test](https://github.com/agu5a/androkat/actions/workflows/dotnet.yml/badge.svg)](https://github.com/agu5a/androkat/actions/workflows/dotnet.yml)

# AndroKat

Az AndroKat 2012.10.09-én indult el, mint magyar katolikus alkalmazás android operációs rendszerrel működő telefonra.

MOTTÓ
*"Az istentisztelet, az erkölcsi élet, a hivatalok és funkciók s minden más arra való az Egyházban, hogy ezt az isteni szeretetet viszonozza Istennek, illetve megjelenítse, közvetítse a világ számára." (Barsi Balázs)*

#### AndroKat alkalmazás a Google Play-en
[play.google.com](https://play.google.com/store/apps/details?id=hu.AndroKat)

#### AndroKat a Facebook-on
[facebook](https://www.facebook.com/androkat)

#### AndroKat a Twitter-en
[![](https://img.shields.io/twitter/follow/AndroKat?style=social)](https://twitter.com/AndroKat)

#### AndroKat a Youtube-on
[![](https://img.shields.io/youtube/channel/views/UCF3mEbdkhZwjQE8reJHm4sg?style=social)](https://www.youtube.com/@androkat3634)

#### AndroKat az Instagram-on
[instagram](https://www.instagram.com/androkat_app)

## Adatbázis
Jelenleg SQLite az adatbázis. Tekintve, hogy a tárolandó anyagmennyiség nem számottevő és minden tartalom alapvetően "in-memory cache"-ből érhető el a weboldalon és a mobil alkalmazásban, így nincs szükség erősebb adatbázis engine-re.

## Deploy
Jelenleg az AndroKat backend az AWS Lightsail szolgáltatását használja. A webalkalmazás .NET 7-re épül és egy Ubuntu instance-on fut Kestrel használva, mint egy cross-platform web server.
A Kestrel előtt még egy Nginx webserver is telepítve van, mint reverse proxy.
A deploy egyelőre manuálisan történik, de van rá terv, hogy a GitHub-ról automatikusan kikerüljön a legfrissebb verzió.


