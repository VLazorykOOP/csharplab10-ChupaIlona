using System;


namespace lab10
{
    internal class Program
    {
        public class Part
        {
            public int Id { get; }
            public string Name { get; }
            public string Status { get; set; }

            public Part(int id, string name)
            {
                Id = id;
                Name = name;
                Status = "Створено";
            }
        }

        public class Station
        {
            public string Name { get; }
            public event EventHandler<Part> ProcessingCompleted;

            public Station(string name)
            {
                Name = name;
            }

            // Метод обробки деталі
            public void Process(Part part)
            {
                Console.WriteLine($"{Name}: Почалася обробка деталі {part.Name}");
                Thread.Sleep(1000); 
                part.Status = $"Оброблено на {Name}";
                Console.WriteLine($"{Name}: Завершила обробку деталі {part.Name}");

                
                if (ProcessingCompleted != null)
                {
                    ProcessingCompleted(this, part);
                }
            }
        }

        public class Conveyor
        {
            private List<Station> _stations;
            private int _currentStationIndex;

            public Conveyor(List<Station> stations)
            {
                _stations = stations;
            }

            public void Start(Part part)
            {
                _currentStationIndex = 0;
                SubscribeToStations();
                _stations[_currentStationIndex].Process(part);
            }

            // Підписка на події станцій
            private void SubscribeToStations()
            {
                foreach (var station in _stations)
                {
                    station.ProcessingCompleted += (sender, part) =>
                    {
                        _currentStationIndex++;
                        if (_currentStationIndex < _stations.Count)
                        {
                            _stations[_currentStationIndex].Process(part);
                        }
                        else
                        {
                            Console.WriteLine($"Деталь {part.Name} завершила всі етапи обробки!");
                        }
                    };
                }
            }

        }



        static void Main(string[] args)
        {
            var stations = new List<Station>
            {
                new Station("Різання"),
                new Station("Шліфування"),
                new Station("Фарбування")
            };
            var conveyor = new Conveyor(stations);

           // Console.WriteLine("Вкажіть кількість деталей для обробки:");
            //int number_of_parts = int.Parse(Console.ReadLine());


           // for(int i = 0; i < number_of_parts; i++)
           // {
           //     conveyor.Start(new Part(i, "00" + i));
           // }
           var part = new Part(1, "Деталь 001");

            
            conveyor.Start(part);

            Console.WriteLine("Натисніть будь-яку клавішу для завершення...");
            Console.ReadKey();

        }
    }
}


