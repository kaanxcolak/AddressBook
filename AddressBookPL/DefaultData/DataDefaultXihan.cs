using AddressBookBL.ImplementationsOfManagers;
using AddressBookBL.InterfacesOfManagers;
using AddressBookEL.IdentityModels;
using AddressBookEL.ViewModels;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Identity;

namespace AddressBookPL.DefaultData
{
    public class DataDefaultXihan
    {
        public void CheckAndCreateRoles(RoleManager<AppRole> roleManager)
        {
            try
            {
                //admin // customer // misafir vb...
                string[] roles = new string[] { "Admin", "Customer", "Guest" };

                //rolleri tek tek dönüp sisteme olup olmadığına bakacağız. Yoksa ekleyeceğiz.
                foreach (var item in roles)
                {
                    //ROLDEN YOK MU ?	
                    if (!roleManager.RoleExistsAsync(item.ToLower()).Result)
                    {
                        //rolden yokmuş ekleyelim
                        AppRole role = new AppRole()
                        {
                            Name = item
                        };
                        var result = roleManager.CreateAsync(role).Result;
                    }
                }
            }
            catch (Exception ex)
            {
                //ex loglanabilir
                //yazılımcıya acil başlıklı email gönderilebilir
            }
        }

        public void CreateAllCities(ICityManager cityManager)
        {
            try
            {
                //1) Veritbaanındaki illeri listeye ekleyelim
                //2)Exceli açıp satır satır okuyup 
                //2,5)Olmayan ili veri tabanına ekleyelim
                var cityList = cityManager.GetAll(x => !x.IsRemoved).Data;
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Excels");
                string fileName = Path.GetFileName("Cities.xlsx");
                string filePath = Path.Combine(path, fileName);

                using (var excelBook = new XLWorkbook(filePath))
                {
                    var rows = excelBook.Worksheet(1).RowsUsed(); //82
                    foreach (var item in rows)
                    {
                        if (item.RowNumber() > 1) //1.Satırda başlık var
                        {
                            //satırdaki hücrelere ulaşabiliriz.
                            string? cityName = item.Cell(1).Value.ToString()?.Trim();
                            string? plateCode = item.Cell(2).Value.ToString()?.Trim();
                            //bu cityName'den listede var mı yok mu
                            if (cityList.Count(x => x.Name.ToLower() == cityName?.ToLower()) == 0)
                            {
                                CityVM c = new CityVM()
                                {
                                    CreatedDate = DateTime.Now,
                                    Name = cityName,
                                    PlateCode = plateCode
                                };
                                cityManager.Add(c);
                            }
                        } //if bitti
                    } //foreach bitti
                }
            }
            catch (Exception ex)
            {
                //loglanacak
            }
        }

        public void CreateAllDistricts(IDistrictManager districtManager)
        {
            try
            {
                var districts = districtManager.GetAll(x => !x.IsRemoved).Data;
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Excels");

                string filePath = Path.Combine(path, "Districts.xlsx");

                using (var excelBook = new XLWorkbook(filePath)) //C:Users.......
                {
                    var rows = excelBook.Worksheet(1).RowsUsed(); //82
                    foreach (var item in rows)
                    {
                        if (item.RowNumber() > 1) //1.Satırda başlık var
                        {
                            string districtName = item.Cell(1).Value.ToString(); //Beşiktaş

                            int cityId = Convert.ToInt32(item.Cell(2).Value.ToString().Trim()); //34

                            if (districts.Count(x=> x.Name .ToLower() == districtName.ToLower() && x.CityId ==cityId) == 0)
                            {
                                DistrictVM d = new DistrictVM()
                                {
                                    CreatedDate = DateTime.Now,
                                    Name = districtName,
                                    CityId = cityId
                                };
                                districtManager.Add(d);
                            }

                        } //if bitti
                    } //foreach bitti
                } //using bitti
            }
            catch (Exception)
            {

            }
        }

        public void CreateSomeNeighbourhood(INeighbourhoodManager neighbourhoodManager,ICityManager cityManager,IDistrictManager districtManager)
        {
            try
            {
                //return; //NOT:
                //BU METODU YAZMAK BEST PRACTICE DEĞİLDİR! 70 BİN ADTASI OLAN EXCELİ PROJEYİ YORMAMAK İÇİN INSERT INTO İLE EKLEMEK DATA MANTIKLIDIR
                //YA DA Console application ile excele'i okuyup ekleyebilirsiniz
                //burada sadece bir kaç ilin mahallesini ekleyeceğiz
                var neighbours = neighbourhoodManager.GetAll(x => !x.IsRemoved).Data;

                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Excels");
                string filePath = Path.Combine(path, "NeighborhoodPostalCode.xlsx");

                using (var excelBook = new XLWorkbook(filePath)) //C:Users.......
                {
                    var rows = excelBook.Worksheet("Istanbul").RowsUsed(); 
                    foreach (var item in rows)
                    {
                        if (item.RowNumber() > 1) //1.Satırda başlık var
                        {
                            //İl
                            string cityName = item.Cell(1).Value.ToString().Trim();

                            //İlçe
                            string districtName = item.Cell(2).Value.ToString().Trim();

                            //Mahalle
                            string neighbourName = item.Cell(3).Value.ToString().Trim();

                            var city = cityManager.GetByConditions(x=> x.Name.ToLower() == cityName.ToLower()).Data;

                            var district = districtManager.GetByConditions(x=> x.Name.ToLower() == districtName.ToLower() && x.CityId == city.Id).Data;

                            if (neighbours.Count(x=> x.Name.ToLower() == neighbourName.ToLower() && x.DistrictId == district.Id) == 0)
                            {
                                NeighbourhoodVM n = new() //c# bilmem kaçla gelmiş özellik
                                {
                                    CreatedDate = DateTime.Now,
                                    Name = neighbourName,
                                    DistrictId = district.Id,
                                    PostCode = "12345"
                                };
                                neighbourhoodManager.Add(n);
                            }



                        } //if bitti
                    } //foreach bitti
                } //using bitti
            }

            
            catch (Exception)
            {
                                
            }
        }
    }
}
