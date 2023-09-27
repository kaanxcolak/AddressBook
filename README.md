# AddressBook_SUNUM

Merhaba, Address Book projesi İstanbul - Beşiktaş Wissen Akademi'de Kursiyerlik yaptığım süreçte yazdık.Bu projede Trendyol,Getir gibi uygulamalardaki adres ekleme bölümünden esinlendik. Amacımız adres kaydı yapmaktır. Ayrıca gerçek hayata yaklaşmak ve istihdam sürecindeki hazır bulunuşluğu maksimum seviyeye getirebilmektir.

***PROJE HAKKINDA TEKNİK BİLGİLER:***

- Proje Visual Studio .Net 6 ASP.NET MVC CORE ile yazıldı.
- Proje Entity Framework Core Code-First yaklaşımıyla yazılmıştır.
- Projede AspnetCore Identity kullanarak üyelik sistemini yazdık.
- Projeyi 5 katman (EL,DAL,BLL,UI, AddressBookNeighborhoodsLoad) olarak yazdık.
-AddressBookNeighborhoodsLoad katmanı Console uygulaması olup Mahalle datasını eklemektedir. (70bin data bulunuyor)
- Projede İlleri  ve İlçeleri Excel dosyasını back-endde okuyarak veritabanına proje ilk ayağa kalktığında ekledik.
- Projede Adres listesi ve Adres Ekle - Adres Sil ekranları bulunuyor. 
- Adres Ekle sayfasındaki işlemleri AJAX ile yapmaktayız. Örneğin; ili seçtiğinde ilçeler sayfa yenilenmeden gelir. İlçeyi seçtiğinde mahalleler sayfa yenilenmeden gelir.
- Mahalleyi seçince o mahallenin posta kodunu APi'den çektik.  https://api.ubilisim.com/postakodu/il/34
- Proje gelişmeye açık olup zaman buldukça yeni sayfalar ya da yeni özellikler eklenecektir.

