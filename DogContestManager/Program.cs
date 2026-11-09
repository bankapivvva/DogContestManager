using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogContestManager
{
    internal class Program
    {
        // Перечисление пород собак
        enum Breeds
        {
            None,
            Bulldog,
            Golden_Retriever,
            Boxer,
            Chihuahua
        }

        // Структура записи об участнике выставки
        struct Contestant
        {
            public Breeds breed;
            public string name;
            public string surname;
            public string patronymic;
            public string dog_name;
            public double exterier_score;
        };

        // Инициализация списка участников

        static List<Contestant> contestants = new List<Contestant>();
        static HashSet<Contestant> check_double = new HashSet<Contestant>(); // Хэшсет для проверки уникальности записей

        // Процедура вывода значений перечисляемого поля
        static void EnumWrite()
        {
            foreach (Breeds breed in Enum.GetValues(typeof(Breeds)))
            {
                int number = (int)breed; // Явное приведение к числу
                Console.WriteLine($"{number} — {breed}");
            }

        }
        // Процедура добавления участника в список
        static void AddContestant()
        {
            Contestant contestant = new Contestant();
            try
            {

                // Ввод данных участника

                while (true)
                {
                    Console.WriteLine("Введите ваше имя: ");
                    contestant.name = Console.ReadLine();
                    if (CheckString(contestant.name))
                    {
                        break;
                    }
                }
                
                while (true)
                {
                    Console.WriteLine("Введите вашу фамилию: ");
                    contestant.surname = Console.ReadLine();
                    if(CheckString(contestant.surname))
                    {
                        break;
                    }
                }

                while (true)
                {
                    Console.WriteLine("Введите ваше отчество: ");
                    contestant.patronymic = Console.ReadLine();
                    if(CheckString(contestant.patronymic))
                    {
                        break;
                    }
                }
                
                while (true)
                {
                    Console.WriteLine("Введите кличку вашей собаки: ");
                    contestant.dog_name = Console.ReadLine();
                    if(CheckString(contestant.dog_name))
                    {
                        break;
                    }
                }

                Breeds myDogBreed;

                while (true)
                {
                    Console.WriteLine("Введите породу вашей собаки: ");
                    Console.WriteLine("Доступные породы: ");
                    EnumWrite();

                    string input = Console.ReadLine();

                    if (Enum.TryParse<Breeds>(input, out myDogBreed) && Enum.IsDefined(typeof(Breeds), myDogBreed))
                    {
                        break; 
                    }

                    Console.WriteLine("Неверная порода, попробуйте еще раз.\n");
                }

                while (true)
                {
                    Console.WriteLine("Введите количество набранных баллов за экстерьер: ");
                    string input = Console.ReadLine();
                    if (double.TryParse(input, out contestant.exterier_score) && contestant.exterier_score >= 0)
                    {
                        break; 
                    }
                    Console.WriteLine("Ошибка! Введите корректное число (баллы не могут быть меньше 0).");
                }


                if (check_double.Add(contestant))
                {
                    contestants.Add(contestant); 
                    Console.WriteLine("Участник добавлен.");
                }
                else
                {
                    Console.WriteLine("Такая запись уже существует! Участник не добавлен.");
                }
                
                Console.WriteLine();
            }
            catch
            {
                Console.WriteLine("Ошибка ввода! Форма сброшена. Попробуйте еще раз!");
            }
        }

        // Процедура для вывода элемента списка
        static void ShowContestant(Contestant contestant)
        {
            Console.WriteLine("Имя владельца: " + contestant.name);
            Console.WriteLine("Фамилия владельца: " + contestant.surname);
            Console.WriteLine("Отчество владельца: " + contestant.patronymic);
            Console.WriteLine("Кличка собаки: " + contestant.dog_name);
            Console.WriteLine("Порода собаки: " + contestant.breed);
            Console.WriteLine("Кол-во баллов за экстерьер: " + contestant.exterier_score);
            Console.WriteLine();
        }

        // Функция проверки вводимой строки в имени, фамилии, отчестве, кличке собаки
        static bool CheckString(string str)
        {
            if (!str.All(char.IsLetter) || string.IsNullOrWhiteSpace(str))
            {
                return false;
            }
            else return true;
        }

        // Процедура для вывода списка
        static void ShowList()
        {
            for (int i = 0; i < contestants.Count; i++)
            {
                ShowContestant (contestants[i]);
            }
            Console.WriteLine();

            if (contestants.Count == 0)
            {
                Console.WriteLine("Список пуст.");
            }
        }

        // Процедура поиска участников по породе
        static void SearchByBreed()
        {
            Console.WriteLine("Введите породу, по которой хотите отобрать участников: ");
            EnumWrite();
            string search = Console.ReadLine();

            if(Enum.TryParse(search, true, out Breeds searchbreed))
            {
                for(int i = 0; i < contestants.Count;i++)
                {
                    Contestant res = contestants[i];
                    if(res.breed == searchbreed)
                    {
                       ShowContestant(contestants[i]);
                    }
                }
            }
            else
            {
                Console.WriteLine("Такой породы нет в списке!");
            }
        }

        // Процедура удаления участника
        static void RemoveContestant()
        {
            Console.WriteLine("Какого участника удалить (начиная с 0)?");
            int choice = Convert.ToInt32(Console.ReadLine());

            contestants.RemoveAt(choice);
            Console.WriteLine($"Участник {choice} удален из списка участников.");
        }

        // Процедура отбора первых трех мест по баллам
        static void ShowTop3()
        {
            if (contestants.Count == 0)
            {
                Console.WriteLine("Список пуст.");
            }
            else
            {
                for (int i = 0; i < contestants.Count; i++)
                {
                    for (int j = i + 1; j < contestants.Count; j++)
                    {
                        if (contestants[j].exterier_score > contestants[i].exterier_score)
                        {
                            Contestant temp = contestants[i];
                            contestants[i] = contestants[j];
                            contestants[j] = temp;
                        }
                    }
                }

                Console.WriteLine("--- ТОП-3 УЧАСТНИКОВ ---");
                for (int i = 0; i < contestants.Count; i++)
                {
                    if (i == 3) break; 

                    Console.WriteLine($"{i + 1} место: {contestants[i].dog_name} — {contestants[i].exterier_score} баллов");
                }
            }
                
        }

        // Главная функция (реализация меню управления)
        static void Main(string[] args)
        {
            while (true)
            {
                int menu_choice = 0;
                Console.WriteLine("1 - Вывести список");
                Console.WriteLine("2 - Добавить участника");
                Console.WriteLine("3 - Удалить участника");
                Console.WriteLine("4 - Отфильтровать по породе");
                Console.WriteLine("5 - Вывести первые три места по баллам");
                Console.WriteLine("6 - Выйти из программы");
                Console.WriteLine("Введите ваш выбор: ");
                try
                {
                    menu_choice = Convert.ToInt32(Console.ReadLine());
                }
                catch (FormatException)
                {
                    Console.WriteLine("Ошибка ввода!");
                }
             
                switch (menu_choice)
                {
                    case 1:
                        ShowList();
                        break;
                    case 2:
                        AddContestant();
                        break;
                    case 3:
                        RemoveContestant();
                        break;
                    case 4:
                        SearchByBreed();
                        break;
                    case 5:
                        ShowTop3();
                        break;
                    case 6:
                        return;

                }
            }
        }
    }
}