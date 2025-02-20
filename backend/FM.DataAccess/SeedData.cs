using System;
using System.Collections.Generic;
using FM.Core.Enums;
using FM.DataAccess.Entities;

public static class SeedData
{
    public static List<FederalDistrictEntity> FederalDistricts => new()
    {
        new FederalDistrictEntity { Id = 1, Name = "Центральный федеральный округ" },
        new FederalDistrictEntity { Id = 2, Name = "Северо-Западный федеральный округ" },
        new FederalDistrictEntity { Id = 3, Name = "Южный федеральный округ" },
        new FederalDistrictEntity { Id = 4, Name = "Северо-Кавказский федеральный округ" },
        new FederalDistrictEntity { Id = 5, Name = "Приволжский федеральный округ" },
        new FederalDistrictEntity { Id = 6, Name = "Уральский федеральный округ" },
        new FederalDistrictEntity { Id = 7, Name = "Сибирский федеральный округ" },
        new FederalDistrictEntity { Id = 8, Name = "Дальневосточный федеральный округ" }
    };

    public static List<AirportEntity> Airports => new()
    {
        new AirportEntity { Id = 1, Name = "Шереметьево", City = "Москва", FederalDistrictId = 1 },
        new AirportEntity { Id = 2, Name = "Домодедово", City = "Москва", FederalDistrictId = 1 },
        new AirportEntity { Id = 3, Name = "Внуково", City = "Москва", FederalDistrictId = 1 },
        new AirportEntity { Id = 4, Name = "Пулково", City = "Санкт-Петербург", FederalDistrictId = 2 },
        new AirportEntity { Id = 5, Name = "Платов", City = "Ростов-на-Дону", FederalDistrictId = 3 },
        new AirportEntity { Id = 6, Name = "Сочи", City = "Сочи", FederalDistrictId = 3 },
        new AirportEntity { Id = 7, Name = "Минеральные Воды", City = "Минеральные Воды", FederalDistrictId = 4 },
        new AirportEntity { Id = 8, Name = "Казань", City = "Казань", FederalDistrictId = 5 },
        new AirportEntity { Id = 9, Name = "Нижний Новгород", City = "Нижний Новгород", FederalDistrictId = 5 },
        new AirportEntity { Id = 10, Name = "Курган", City = "Курган", FederalDistrictId = 6 },
        new AirportEntity { Id = 11, Name = "Екатеринбург", City = "Екатеринбург", FederalDistrictId = 6 },
        new AirportEntity { Id = 12, Name = "Толмачёво", City = "Новосибирск", FederalDistrictId = 7 },
        new AirportEntity { Id = 13, Name = "Красноярск", City = "Красноярск", FederalDistrictId = 7 },
        new AirportEntity { Id = 14, Name = "Иркутск", City = "Иркутск", FederalDistrictId = 7 },
        new AirportEntity { Id = 15, Name = "Владивосток", City = "Владивосток", FederalDistrictId = 8 },
        new AirportEntity { Id = 16, Name = "Хабаровск", City = "Хабаровск", FederalDistrictId = 8 },
        new AirportEntity { Id = 17, Name = "Якутск", City = "Якутск", FederalDistrictId = 8 },
        new AirportEntity { Id = 18, Name = "Калининград", City = "Калининград", FederalDistrictId = 2 },
        new AirportEntity { Id = 19, Name = "Уфа", City = "Уфа", FederalDistrictId = 5 },
        new AirportEntity { Id = 20, Name = "Челябинск", City = "Челябинск", FederalDistrictId = 6 }
    };

    public static List<FlightEntity> Flights => new()
    {
        new FlightEntity
        {
            Id = 1,
            FlightNumber = "SU1001",
            Destination = "Санкт-Петербург",
            DepartureTime = DateTime.UtcNow.AddHours(2),
            ArrivalTime = DateTime.UtcNow.AddHours(4),
            AvailableSeats = 180,
            AirplanePhotoUrl = "https://admspvoskresenskoe.ru/wp-content/uploads/Ezegodno-9-fevrala-v-Rossii-otmecaetsa-Den'-grazdanskoj-aviacii.jpg",
            AirportId = 1
        },
        new FlightEntity
        {
            Id = 2,
            FlightNumber = "SU1002",
            Destination = "Екатеринбург",
            DepartureTime = DateTime.UtcNow.AddHours(3),
            ArrivalTime = DateTime.UtcNow.AddHours(5),
            AvailableSeats = 150,
            AirplanePhotoUrl = "https://vesti-ural.ru/wp-content/uploads/2021/09/1590132398_4.jpeg",
            AirportId = 1 
        },
        new FlightEntity
        {
            Id = 3,
            FlightNumber = "SU1003",
            Destination = "Сочи",
            DepartureTime = DateTime.UtcNow.AddHours(1),
            ArrivalTime = DateTime.UtcNow.AddHours(3),
            AvailableSeats = 200,
            AirplanePhotoUrl = "https://avatars.mds.yandex.net/get-mpic/5221857/img_id9103962167301146378.jpeg/orig",
            AirportId = 1
        },
        new FlightEntity
        {
            Id = 4,
            FlightNumber = "SU1004",
            Destination = "Владивосток",
            DepartureTime = DateTime.UtcNow.AddHours(4),
            ArrivalTime = DateTime.UtcNow.AddHours(8),
            AvailableSeats = 220,
            AirplanePhotoUrl = "https://wallpapers.com/images/hd/4k-planes-28pfplts81ps0e4h.jpg",
            AirportId = 1
        },
        new FlightEntity
        {
            Id = 5,
            FlightNumber = "SU1005",
            Destination = "Калининград",
            DepartureTime = DateTime.UtcNow.AddHours(2),
            ArrivalTime = DateTime.UtcNow.AddHours(3),
            AvailableSeats = 130,
            AirplanePhotoUrl = "https://cdn.culture.ru/images/e47f7cd8-4712-5f93-93c7-ae9c96e2492c",
            AirportId = 1
        },
        new FlightEntity
        {
            Id = 6,
            FlightNumber = "DP2001",
            Destination = "Новосибирск",
            DepartureTime = DateTime.UtcNow.AddHours(3),
            ArrivalTime = DateTime.UtcNow.AddHours(6),
            AvailableSeats = 190,
            AirplanePhotoUrl = "https://exclusive.kz/wp-content/uploads/2022/06/126245.jpg",
            AirportId = 2
        },
        new FlightEntity
        {
            Id = 7,
            FlightNumber = "DP2002",
            Destination = "Красноярск",
            DepartureTime = DateTime.UtcNow.AddHours(2),
            ArrivalTime = DateTime.UtcNow.AddHours(5),
            AvailableSeats = 160,
            AirplanePhotoUrl = "https://cdn1.ozone.ru/s3/multimedia-6/6447919386.jpg",
            AirportId = 2
        },
        new FlightEntity
        {
            Id = 8,
            FlightNumber = "DP2003",
            Destination = "Иркутск",
            DepartureTime = DateTime.UtcNow.AddHours(5),
            ArrivalTime = DateTime.UtcNow.AddHours(9),
            AvailableSeats = 210,
            AirplanePhotoUrl = "https://habrastorage.org/files/d7f/c1a/c07/d7fc1ac07c244551b7198a5caae9d687.jpg",
            AirportId = 2
        },
        new FlightEntity
        {
            Id = 9,
            FlightNumber = "DP2004",
            Destination = "Хабаровск",
            DepartureTime = DateTime.UtcNow.AddHours(4),
            ArrivalTime = DateTime.UtcNow.AddHours(8),
            AvailableSeats = 180,
            AirplanePhotoUrl = "https://avatars.mds.yandex.net/get-mpic/5042167/img_id145303521372402621.jpeg/orig",
            AirportId = 2
        },
        new FlightEntity
        {
            Id = 10,
            FlightNumber = "VP3001",
            Destination = "Ростов-на-Дону",
            DepartureTime = DateTime.UtcNow.AddHours(2),
            ArrivalTime = DateTime.UtcNow.AddHours(4),
            AvailableSeats = 170,
            AirplanePhotoUrl = "https://s0.rbk.ru/v6_top_pics/media/img/3/57/754755745720573.jpg",
            AirportId = 3
        },
        new FlightEntity
        {
            Id = 11,
            FlightNumber = "VP3002",
            Destination = "Минеральные Воды",
            DepartureTime = DateTime.UtcNow.AddHours(1),
            ArrivalTime = DateTime.UtcNow.AddHours(3),
            AvailableSeats = 140,
            AirplanePhotoUrl = "https://cdnstatic.rg.ru/uploads/images/156/67/67/Depositphotos_41367457_m-2015.jpg",
            AirportId = 3
        },
        new FlightEntity
        {
            Id = 12,
            FlightNumber = "VP3003",
            Destination = "Казань",
            DepartureTime = DateTime.UtcNow.AddHours(3),
            ArrivalTime = DateTime.UtcNow.AddHours(5),
            AvailableSeats = 200,
            AirplanePhotoUrl = "https://cdn1.ozone.ru/s3/multimedia-l/6450210693.jpgg",
            AirportId = 3
        },
            new FlightEntity
        {
            Id = 13,
            FlightNumber = "VP3004",
            Destination = "Нижний Новгород",
            DepartureTime = DateTime.UtcNow.AddHours(2),
            ArrivalTime = DateTime.UtcNow.AddHours(4),
            AvailableSeats = 160,
            AirplanePhotoUrl = "https://www.ixbt.com/img/n1/news/2022/9/6/avia_large.png",
            AirportId = 3
        },
        new FlightEntity
        {
            Id = 14,
            FlightNumber = "MR4001",
            Destination = "Уфа",
            DepartureTime = DateTime.UtcNow.AddHours(3),
            ArrivalTime = DateTime.UtcNow.AddHours(5),
            AvailableSeats = 180,
            AirplanePhotoUrl = "https://i.pinimg.com/originals/1f/6f/88/1f6f88acd68ba05da46838932162a14b.jpg",
            AirportId = 4
        },
        new FlightEntity
        {
            Id = 15,
            FlightNumber = "MR4002",
            Destination = "Челябинск",
            DepartureTime = DateTime.UtcNow.AddHours(4),
            ArrivalTime = DateTime.UtcNow.AddHours(7),
            AvailableSeats = 190,
            AirplanePhotoUrl = "https://cdn1.ozone.ru/multimedia/1022239843.jpg",
            AirportId = 4
        },
        new FlightEntity
        {
            Id = 16,
            FlightNumber = "MR4003",
            Destination = "Самара",
            DepartureTime = DateTime.UtcNow.AddHours(2),
            ArrivalTime = DateTime.UtcNow.AddHours(4),
            AvailableSeats = 150,
            AirplanePhotoUrl = "https://static.tildacdn.com/tild6666-3933-4737-b836-616635623763/samolety-krasivye-ka.jpg",
            AirportId = 4
        },
        new FlightEntity
        {
            Id = 17,
            FlightNumber = "MR4004",
            Destination = "Пермь",
            DepartureTime = DateTime.UtcNow.AddHours(3),
            ArrivalTime = DateTime.UtcNow.AddHours(6),
            AvailableSeats = 170,
            AirplanePhotoUrl = "https://otvet.imgsmail.ru/download/4e9ff0c84d505cbb79586c493d25132e_h-324.jpg",
            AirportId = 4
        },
        new FlightEntity
        {
            Id = 18,
            FlightNumber = "MR4005",
            Destination = "Волгоград",
            DepartureTime = DateTime.UtcNow.AddHours(2),
            ArrivalTime = DateTime.UtcNow.AddHours(4),
            AvailableSeats = 160,
            AirplanePhotoUrl = "https://img.goodfon.com/original/2388x1668/a/79/cathay-pacific-boeing-777.jpg",
            AirportId = 4
        },
        new FlightEntity
        {
            Id = 19,
            FlightNumber = "NS5001",
            Destination = "Омск",
            DepartureTime = DateTime.UtcNow.AddHours(5),
            ArrivalTime = DateTime.UtcNow.AddHours(8),
            AvailableSeats = 200,
            AirplanePhotoUrl = "https://s9.travelask.ru/uploads/post/000/026/337/main_image/facebook-3c16fde96f14711ae2a04a727c4025c1.jpg",
            AirportId = 5
        },
        new FlightEntity
        {
            Id = 20,
            FlightNumber = "NS5002",
            Destination = "Барнаул",
            DepartureTime = DateTime.UtcNow.AddHours(4),
            ArrivalTime = DateTime.UtcNow.AddHours(7),
            AvailableSeats = 180,
            AirplanePhotoUrl = "https://www.atorus.ru/sites/default/files/styles/head_carousel/public/2021-09/131872.jpg.webp?itok=w02d3ccD",
            AirportId = 5
        },
        new FlightEntity
        {
            Id = 21,
            FlightNumber = "NS5003",
            Destination = "Тюмень",
            DepartureTime = DateTime.UtcNow.AddHours(3),
            ArrivalTime = DateTime.UtcNow.AddHours(6),
            AvailableSeats = 190,
            AirplanePhotoUrl = "https://cdn1.ozone.ru/s3/multimedia-i/6449418186.jpg",
            AirportId = 5
        },
        new FlightEntity
        {
            Id = 22,
            FlightNumber = "NS5004",
            Destination = "Махачкала",
            DepartureTime = DateTime.UtcNow.AddHours(2),
            ArrivalTime = DateTime.UtcNow.AddHours(4),
            AvailableSeats = 150,
            AirplanePhotoUrl = "https://img5tv.cdnvideo.ru/webp/shared/files/202102/1_1263737.jpg",
            AirportId = 5
        },
        new FlightEntity
        {
            Id = 23,
            FlightNumber = "NS5005",
            Destination = "Улан-Удэ",
            DepartureTime = DateTime.UtcNow.AddHours(6),
            ArrivalTime = DateTime.UtcNow.AddHours(10),
            AvailableSeats = 210,
            AirplanePhotoUrl = "https://static.life.ru/publications/2022/2/25/622639184735.0912.png",
            AirportId = 5
        },
        new FlightEntity
        {
            Id = 24,
            FlightNumber = "NS5006",
            Destination = "Сыктывкар",
            DepartureTime = DateTime.UtcNow.AddHours(4),
            ArrivalTime = DateTime.UtcNow.AddHours(7),
            AvailableSeats = 160,
            AirplanePhotoUrl = "https://i.pinimg.com/originals/5d/47/b3/5d47b3b01b41bbd587043be61b4f5230.jpg",
            AirportId = 5
        },
    };

    public static List<ServiceEntity> Services => new()
    {
        new ServiceEntity { Id = 1, Name = "Meal", Cost = 10.00m },
        new ServiceEntity { Id = 2, Name = "Extra Luggage", Cost = 20.00m },
        new ServiceEntity { Id = 3, Name = "Priority Boarding", Cost = 15.00m },
        new ServiceEntity { Id = 4, Name = "In-Flight Entertainment", Cost = 5.00m },
        new ServiceEntity { Id = 5, Name = "Wi-Fi Access", Cost = 12.00m },
        new ServiceEntity { Id = 6, Name = "Extra Legroom Seat", Cost = 25.00m },
        new ServiceEntity { Id = 7, Name = "Airport Transfer", Cost = 30.00m },
        new ServiceEntity { Id = 8, Name = "Travel Insurance", Cost = 18.00m },
        new ServiceEntity { Id = 9, Name = "Lounge Access", Cost = 40.00m },
        new ServiceEntity { Id = 10, Name = "Special Meal Request", Cost = 12.00m }
    };

    public static List<TicketEntity> Tickets => new()
    {
        new TicketEntity
        {
            Id = 1,
            TicketType = TicketType.Economy,
            Price = 150.00f,
            Seat = "A1",
            FlightId = 1
        },
        new TicketEntity
        {
            Id = 2,
            TicketType = TicketType.Business,
            Price = 300.00f,
            Seat = "B1",
            FlightId = 1
        },
        new TicketEntity
        {
            Id = 3,
            TicketType = TicketType.Economy,
            Price = 180.00f,
            Seat = "C1",
            FlightId = 2
        },
        new TicketEntity
        {
            Id = 4,
            TicketType = TicketType.VIP,
            Price = 500.00f,
            Seat = "D1",
            FlightId = 2
        },
        new TicketEntity
        {
            Id = 5,
            TicketType = TicketType.Economy,
            Price = 160.00f,
            Seat = "E1",
            FlightId = 3
        },
        new TicketEntity
        {
            Id = 6,
            TicketType = TicketType.Business,
            Price = 320.00f,
            Seat = "F1",
            FlightId = 3
        },
        new TicketEntity
        {
            Id = 7,
            TicketType = TicketType.Economy,
            Price = 170.00f,
            Seat = "G1",
            FlightId = 4
        },
        new TicketEntity
        {
            Id = 8,
            TicketType = TicketType.VIP,
            Price = 550.00f,
            Seat = "H1",
            FlightId = 4
        },
        new TicketEntity
        {
            Id = 9,
            TicketType = TicketType.Economy,
            Price = 190.00f,
            Seat = "I1",
            FlightId = 5
        },
        new TicketEntity
        {
            Id = 10,
            TicketType = TicketType.Business,
            Price = 330.00f,
            Seat = "J1",
            FlightId = 5
        }
    };

    public static List<TicketServiceEntity> TicketServices => new()
    {
        new TicketServiceEntity { Id = 1, ServiceId = 1, TicketId = 1 },
        new TicketServiceEntity { Id = 2, ServiceId = 2, TicketId = 1 },
        new TicketServiceEntity { Id = 3, ServiceId = 3, TicketId = 1 },
        new TicketServiceEntity { Id = 4, ServiceId = 1, TicketId = 2 },
        new TicketServiceEntity { Id = 5, ServiceId = 2, TicketId = 2 },
        new TicketServiceEntity { Id = 6, ServiceId = 3, TicketId = 3 },
        new TicketServiceEntity { Id = 7, ServiceId = 4, TicketId = 4 },
        new TicketServiceEntity { Id = 8, ServiceId = 1, TicketId = 5 },
        new TicketServiceEntity { Id = 9, ServiceId = 2, TicketId = 5 },
        new TicketServiceEntity { Id = 10, ServiceId = 5, TicketId = 5 },
        new TicketServiceEntity { Id = 11, ServiceId = 6, TicketId = 5 },
        new TicketServiceEntity { Id = 12, ServiceId = 6, TicketId = 6 },
        new TicketServiceEntity { Id = 13, ServiceId = 7, TicketId = 7 },
        new TicketServiceEntity { Id = 14, ServiceId = 8, TicketId = 8 },
        new TicketServiceEntity { Id = 15, ServiceId = 9, TicketId = 9 },
        new TicketServiceEntity { Id = 16, ServiceId = 10, TicketId = 10 }
    };
}