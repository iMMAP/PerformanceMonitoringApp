using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Web.Script.Services;
using System.Collections.Generic;
using System.Linq;

public class Car
{
    public string Make;
    public string Model;
    public int Year;
    public int Doors;
    public string Colour;
    public float Price;
}

/// <summary>
/// Summary description for CarService
/// </summary>

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[ScriptService]
public class CarService : WebService
{
    List<Car> Cars = new List<Car>{
    new Car{Make="Audi",Model="A4",Year=1995,Doors=5,Colour="Red",Price=2995f},
    new Car{Make="Ford",Model="Focus",Year=2002,Doors=5,Colour="Black",Price=3250f},
    new Car{Make="BMW",Model="5 Series",Year=2006,Doors=4,Colour="Grey",Price=24950f},
    new Car{Make="Renault",Model="Laguna",Year=2000,Doors=5,Colour="Red",Price=3995f},
    new Car{Make="Toyota",Model="Previa",Year=1998,Doors=5,Colour="Green",Price=2695f},
    new Car{Make="Mini",Model="Cooper",Year=2005,Doors=2,Colour="Grey",Price=9850f},
    new Car{Make="Mazda",Model="MX 5",Year=2003,Doors=2,Colour="Silver",Price=6995f},
    new Car{Make="Ford",Model="Fiesta",Year=2004,Doors=3,Colour="Red",Price=3759f},
    new Car{Make="Honda",Model="Accord",Year=1997,Doors=4,Colour="Silver",Price=1995f}
    };

    [WebMethod]
    public List<Car> GetAllCars()
    {
        return Cars;
    }
}